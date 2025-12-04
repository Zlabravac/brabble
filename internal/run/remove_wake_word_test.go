package run

import "testing"

func TestRemoveWakeWord(t *testing.T) {
	cases := []struct {
		text   string
		word   string
		expect string
	}{
		{"clawd make it so", "clawd", "make it so"},
		{"hey ClAwD computer", "clawd", "hey computer"},
		{"clawd, launch torpedo", "clawd", "launch torpedo"},
		{"we said clawd twice clawd", "clawd", "we said twice clawd"},
		{"no wake here", "clawd", "no wake here"},
	}
	for _, c := range cases {
		got := removeWakeWord(c.text, c.word)
		if got != c.expect {
			t.Fatalf("removeWakeWord(%q)=%q want %q", c.text, got, c.expect)
		}
	}
}
