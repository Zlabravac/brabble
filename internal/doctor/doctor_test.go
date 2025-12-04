package doctor

import (
	"os"
	"path/filepath"
	"testing"
)

func TestCheckHookExecutablePath(t *testing.T) {
	dir := t.TempDir()
	target := filepath.Join(dir, "hook.sh")
	if err := os.WriteFile(target, []byte("#!/bin/sh\nexit 0\n"), 0o755); err != nil {
		t.Fatal(err)
	}
	res := checkHookExecutable(target)
	if !res.Pass {
		t.Fatalf("expected pass, got %v", res.Detail)
	}
}

func TestCheckHookExecutableDirectory(t *testing.T) {
	dir := t.TempDir()
	res := checkHookExecutable(dir)
	if res.Pass {
		t.Fatalf("expected fail for directory")
	}
}

func TestCheckHookExecutableNotSet(t *testing.T) {
	res := checkHookExecutable("")
	if res.Pass {
		t.Fatalf("expected fail for empty")
	}
}
