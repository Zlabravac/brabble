package run

import (
	"time"
)

func (s *Server) watchdog(ctxDone <-chan struct{}) {
	ticker := time.NewTicker(60 * time.Second)
	defer ticker.Stop()
	for {
		select {
		case <-ctxDone:
			return
		case <-ticker.C:
			last := time.Unix(0, s.lastHeard.Load())
			if time.Since(last) > 2*time.Minute {
				s.logger.Warnf("no speech detected for %s", time.Since(last).Round(time.Second))
			}
		}
	}
}
