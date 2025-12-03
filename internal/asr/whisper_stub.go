//go:build !whisper

package asr

import (
	"brabble/internal/config"

	"github.com/sirupsen/logrus"
)

func newWhisperRecognizer(cfg *config.Config, logger *logrus.Logger) (Recognizer, error) {
	return nil, nil
}
