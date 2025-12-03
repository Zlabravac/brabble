package control

import (
	"fmt"
	"os"
	"path/filepath"

	"github.com/spf13/cobra"
)

// NewUninstallServiceCmd removes the user launchd plist.
func NewUninstallServiceCmd() *cobra.Command {
	return &cobra.Command{
		Use:   "uninstall-service",
		Short: "Remove user launchd plist (macOS)",
		RunE: func(cmd *cobra.Command, args []string) error {
			plist := filepath.Join(os.Getenv("HOME"), "Library", "LaunchAgents", "com.brabble.agent.plist")
			_ = os.Remove(plist)
			fmt.Printf("removed %s (if present); unload manually with: launchctl bootout gui/$(id -u) %s\n", plist, plist)
			return nil
		},
	}
}
