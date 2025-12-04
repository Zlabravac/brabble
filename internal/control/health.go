package control

import (
	"encoding/json"
	"fmt"
	"net"

	"brabble/internal/config"

	"github.com/spf13/cobra"
)

// NewHealthCmd pings the daemon health endpoint.
func NewHealthCmd(cfgPath *string) *cobra.Command {
	return &cobra.Command{
		Use:   "health",
		Short: "Check daemon health",
		RunE: func(cmd *cobra.Command, args []string) error {
			cfg, err := config.Load(*cfgPath)
			if err != nil {
				return err
			}
			conn, err := net.Dial("unix", cfg.Paths.SocketPath)
			if err != nil {
				return fmt.Errorf("cannot connect to daemon: %w", err)
			}
			defer func() { _ = conn.Close() }()
			req := Request{Op: "health"}
			if err := json.NewEncoder(conn).Encode(req); err != nil {
				return err
			}
			var resp SimpleResponse
			if err := json.NewDecoder(conn).Decode(&resp); err != nil {
				return err
			}
			if !resp.OK {
				return fmt.Errorf("daemon unhealthy: %s", resp.Message)
			}
			fmt.Println("ok")
			return nil
		},
	}
}
