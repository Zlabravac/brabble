package logging

import (
	"io"
	"os"

	"brabble/internal/config"

	"github.com/sirupsen/logrus"
	"gopkg.in/natefinch/lumberjack.v2"
)

// Configure sets up logrus with rotation.
func Configure(cfg *config.Config) (*logrus.Logger, error) {
	if err := config.MustStatePaths(cfg); err != nil {
		return nil, err
	}
	logger := logrus.New()
	logger.SetFormatter(&logrus.TextFormatter{
		FullTimestamp: true,
	})
	rotator := &lumberjack.Logger{
		Filename:   cfg.Paths.LogPath,
		MaxSize:    20, // megabytes
		MaxBackups: 3,
		MaxAge:     30,
		Compress:   false,
	}
	logger.SetOutput(io.MultiWriter(os.Stdout, rotator))
	return logger, nil
}
