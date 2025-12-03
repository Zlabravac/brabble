package main

import (
	"fmt"
	"os"

	"brabble/internal/control"
	"brabble/internal/daemon"

	"github.com/spf13/cobra"
)

func main() {
	if err := run(); err != nil {
		fmt.Fprintf(os.Stderr, "error: %v\n", err)
		os.Exit(1)
	}
}

func run() error {
	root := &cobra.Command{
		Use:   "brabble",
		Short: "Brabble — local wake-word voice hook daemon",
		Long: `Brabble listens on your mic, waits for a wake word ("clawd"), transcribes locally with whisper.cpp,
and fires a configurable hook (default: ../warelay send "Voice brabble from ${hostname}: <text>").

Key commands:
  start|stop|restart        Daemon lifecycle
  status --json             Uptime + last transcripts
  list-mics / set-mic       Select input device (whisper build)
  doctor                    Check deps/model/hook/portaudio
  setup                     Download default whisper model
  models list|download|set  Manage whisper.cpp models
  install-service           Write launchd plist (macOS)
  reload                    Reload hook/wake config live
  health                    Control-socket liveness ping

Notable flags/env:
  --metrics-addr <addr>     Enable /metrics (Prometheus text)
  --no-wake                 Disable wake word requirement
  Env overrides: BRABBLE_WAKE_ENABLED, BRABBLE_METRICS_ADDR,
                 BRABBLE_LOG_LEVEL/FORMAT, BRABBLE_TRANSCRIPTS_ENABLED,
                 BRABBLE_REDACT_PII`,
		Example: `  brabble start --metrics-addr 127.0.0.1:9317
  brabble list-mics
  brabble models download ggml-medium-q5_1.bin
  brabble models set ggml-medium-q5_1.bin
  brabble install-service --env BRABBLE_METRICS_ADDR=127.0.0.1:9317
  brabble reload
  brabble health`,
	}

	cfgPath := root.PersistentFlags().StringP("config", "c", "", "Path to config file (TOML). Defaults to ~/.config/brabble/config.toml")

	root.AddCommand(daemon.NewStartCmd(cfgPath))
	root.AddCommand(daemon.NewStopCmd(cfgPath))
	root.AddCommand(daemon.NewRestartCmd(cfgPath))
	root.AddCommand(control.NewStatusCmd(cfgPath))
	root.AddCommand(control.NewTailLogCmd(cfgPath))
	root.AddCommand(control.NewListMicsCmd())
	root.AddCommand(control.NewSetMicCmd(cfgPath))
	root.AddCommand(control.NewTestHookCmd(cfgPath))
	root.AddCommand(control.NewDoctorCmd(cfgPath))
	root.AddCommand(control.NewServiceCmd(cfgPath))
	root.AddCommand(control.NewUninstallServiceCmd())
	root.AddCommand(control.NewSetupCmd(cfgPath))
	root.AddCommand(control.NewReloadCmd(cfgPath))
	root.AddCommand(control.NewHealthCmd(cfgPath))
	root.AddCommand(control.NewModelsCmd(cfgPath))

	// Hidden internal serve command used by start.
	root.AddCommand(daemon.NewServeCmd(cfgPath))

	if err := root.Execute(); err != nil {
		return err
	}
	return nil
}
