package control

import "time"

// Request describes an operation sent over the control socket.
type Request struct {
	Op string `json:"op"`
}

// Status reports daemon health and recent transcripts.
type Status struct {
	Running     bool         `json:"running"`
	UptimeSec   float64      `json:"uptime_sec"`
	Transcripts []Transcript `json:"transcripts"`
}

// SimpleResponse is a minimal OK/error envelope.
type SimpleResponse struct {
	OK      bool   `json:"ok"`
	Message string `json:"message"`
}

// Transcript holds a single recognized utterance and timestamp.
type Transcript struct {
	Text      string    `json:"text"`
	Timestamp time.Time `json:"timestamp"`
}
