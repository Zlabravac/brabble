//go:build !whisper

package control

import (
	"brabble/internal/config"

	"github.com/spf13/cobra"
)

// NewMicCmd groups mic subcommands.
func NewMicCmd(cfgPath *string) *cobra.Command {
	cmd := &cobra.Command{
		Use:     "mic",
		Short:   "Microphone management",
		Aliases: []string{"mics", "microphone"},
	}
	cmd.AddCommand(newMicListCmd())
	cmd.AddCommand(newMicSetCmd(cfgPath))
	return cmd
}

func newMicListCmd() *cobra.Command {
	return &cobra.Command{
		Use:   "list",
		Short: "List available microphones",
		RunE: func(cmd *cobra.Command, args []string) error {
			cmd.Println("build with '-tags whisper' to enable microphone listing (PortAudio required)")
			return nil
		},
	}
}

func newMicSetCmd(cfgPath *string) *cobra.Command {
	cmd := &cobra.Command{
		Use:   "set [name]",
		Short: "Set microphone device in config",
		Args:  cobra.MaximumNArgs(1),
		RunE: func(cmd *cobra.Command, args []string) error {
			cfg, err := config.Load(*cfgPath)
			if err != nil {
				return err
			}
			idx, _ := cmd.Flags().GetInt("index")
			if len(args) == 0 && idx < 0 {
				cmd.Println("hint: build with '-tags whisper' to list devices; set name or --index anyway")
			}
			if len(args) > 0 {
				cfg.Audio.DeviceName = args[0]
			}
			cfg.Audio.DeviceIndex = idx
			if err := config.Save(cfg, cfg.Paths.ConfigPath); err != nil {
				return err
			}
			cmd.Printf("mic set: name=%q index=%d in %s\n", cfg.Audio.DeviceName, cfg.Audio.DeviceIndex, cfg.Paths.ConfigPath)
			return nil
		},
	}
	cmd.Flags().Int("index", -1, "set by device index (from mic list)")
	return cmd
}
