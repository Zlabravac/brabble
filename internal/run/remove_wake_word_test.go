package run

import "testing"

func TestRemoveWakeWord(t *testing.T) {
	cases := []struct {
		text    string
		word    string
		aliases []string
		expect  string
	}{
		{"clawd make it so", "clawd", nil, "make it so"},
		{"hey ClAwD computer", "clawd", nil, "hey computer"},
		{"clawd, launch torpedo", "clawd", nil, "launch torpedo"},
		{"we said clawd twice clawd", "clawd", nil, "we said twice clawd"},
		{"no wake here", "clawd", nil, "no wake here"},
		{"Claude engage", "clawd", []string{"claude"}, "engage"},
	}
	for _, c := range cases {
		got := removeWakeWord(c.text, c.word, c.aliases)
		if got != c.expect {
			t.Fatalf("removeWakeWord(%q)=%q want %q", c.text, got, c.expect)
		}
	}
}
