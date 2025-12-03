//go:build whisper

package asr

import (
	"context"
	"fmt"

	"brabble/internal/config"

	"github.com/sirupsen/logrus"
)

// Placeholder whisper recognizer; user can implement with whisper.cpp bindings.
type whisperRecognizer struct{}

func newWhisperRecognizer(cfg *config.Config, logger *logrus.Logger) (Recognizer, error) {
	return &whisperRecognizer{}, nil
}

func (w *whisperRecognizer) Run(ctx context.Context, out chan<- Segment) error {
	return fmt.Errorf("whisper recognizer not yet implemented")
}

func isWhisperEnabled() bool { return true }
