package doctor

import (
	"os"
	"os/exec"

	"brabble/internal/config"
)

// Result represents a diagnostic check.
type Result struct {
	Name   string
	Pass   bool
	Detail string
}

// Run executes doctor checks (stub build: no PortAudio probe).
func Run(cfg *config.Config) []Result {
	results := []Result{
		checkFile("config path", cfg.Paths.ConfigPath),
		checkFile("model file", cfg.ASR.ModelPath),
		checkExecutable("warelay", cfg.Hook.Command),
	}
	results = append(results, checkPortAudio(false))
	return results
}

func checkFile(label, path string) Result {
	if path == "" {
		return Result{Name: label, Pass: false, Detail: "not set"}
	}
	if _, err := os.Stat(os.ExpandEnv(path)); err != nil {
		return Result{Name: label, Pass: false, Detail: err.Error()}
	}
	return Result{Name: label, Pass: true, Detail: path}
}

func checkExecutable(label, path string) Result {
	if path == "" {
		return Result{Name: label, Pass: false, Detail: "not set"}
	}
	if _, err := exec.LookPath(path); err != nil {
		return Result{Name: label, Pass: false, Detail: err.Error()}
	}
	return Result{Name: label, Pass: true, Detail: path}
}

// checkPortAudio is overridden in whisper build.
func checkPortAudio(_ bool) Result {
	return Result{Name: "portaudio", Pass: true, Detail: "skipped (build without whisper tag)"}
}
