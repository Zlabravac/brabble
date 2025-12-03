package daemon

import (
	"fmt"
	"os"
	"os/exec"
	"path/filepath"
	"syscall"
	"time"

	"brabble/internal/config"
	"brabble/internal/logging"
	"brabble/internal/run"

	"github.com/spf13/cobra"
)

// NewStartCmd starts the daemon (background).
func NewStartCmd(cfgPath *string) *cobra.Command {
	return &cobra.Command{
		Use:   "start",
		Short: "Start brabble daemon",
		RunE: func(cmd *cobra.Command, args []string) error {
			cfg, err := config.Load(*cfgPath)
			if err != nil {
				return err
			}
			if err := ensureNotRunning(cfg); err != nil {
				return err
			}
			if err := os.MkdirAll(filepath.Dir(cfg.Paths.PidPath), 0o755); err != nil {
				return err
			}
			self, err := os.Executable()
			if err != nil {
				return err
			}
			child := exec.Command(self, "serve", "--config", cfg.Paths.ConfigPath)
			child.Stdout = os.Stdout
			child.Stderr = os.Stderr
			if err := child.Start(); err != nil {
				return err
			}
			// Wait a moment and confirm pid file appears.
			waited := 0
			for waited < 20 {
				if _, err := os.Stat(cfg.Paths.PidPath); err == nil {
					break
				}
				time.Sleep(100 * time.Millisecond)
				waited++
			}
			fmt.Printf("brabble started (pid %d)\n", child.Process.Pid)
			return nil
		},
	}
}

// NewServeCmd runs the daemon foreground (internal).
func NewServeCmd(cfgPath *string) *cobra.Command {
	return &cobra.Command{
		Use:    "serve",
		Short:  "Run brabble daemon (internal)",
		Hidden: true,
		RunE: func(cmd *cobra.Command, args []string) error {
			cfg, err := config.Load(*cfgPath)
			if err != nil {
				return err
			}
			logger, err := logging.Configure(cfg)
			if err != nil {
				return err
			}
			return run.Serve(cfg, logger)
		},
	}
}

// NewStopCmd stops the daemon.
func NewStopCmd(cfgPath *string) *cobra.Command {
	return &cobra.Command{
		Use:   "stop",
		Short: "Stop brabble daemon",
		RunE: func(cmd *cobra.Command, args []string) error {
			cfg, err := config.Load(*cfgPath)
			if err != nil {
				return err
			}
			pid, err := readPID(cfg.Paths.PidPath)
			if err != nil {
				return err
			}
			proc, err := os.FindProcess(pid)
			if err != nil {
				return err
			}
			if err := proc.Signal(syscall.SIGTERM); err != nil {
				return err
			}
			fmt.Println("stop signal sent")
			return nil
		},
	}
}

// NewRestartCmd stops then starts.
func NewRestartCmd(cfgPath *string) *cobra.Command {
	return &cobra.Command{
		Use:   "restart",
		Short: "Restart brabble daemon",
		RunE: func(cmd *cobra.Command, args []string) error {
			stopCmd := NewStopCmd(cfgPath)
			_ = stopCmd.RunE(cmd, args) // ignore error if not running
			startCmd := NewStartCmd(cfgPath)
			return startCmd.RunE(cmd, args)
		},
	}
}

func ensureNotRunning(cfg *config.Config) error {
	pid, err := readPID(cfg.Paths.PidPath)
	if err != nil {
		return nil
	}
	// Check if process alive.
	proc, err := os.FindProcess(pid)
	if err == nil {
		if err := proc.Signal(syscall.Signal(0)); err == nil {
			return fmt.Errorf("already running with pid %d", pid)
		}
	}
	return nil
}

func readPID(path string) (int, error) {
	data, err := os.ReadFile(path)
	if err != nil {
		return 0, err
	}
	var pid int
	if _, err := fmt.Sscanf(string(data), "%d", &pid); err != nil {
		return 0, err
	}
	return pid, nil
}
