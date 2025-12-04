package hook

import (
	"strings"

	"brabble/internal/config"
)

// hookMatches reports whether the lower‑cased text contains any of the
// configured wake tokens or aliases for a hook entry.
func hookMatches(lowerText string, hk *config.HookConfig) bool {
	tokens := make([]string, 0, len(hk.Wake)+len(hk.Aliases))
	for _, w := range hk.Wake {
		w = strings.ToLower(strings.TrimSpace(w))
		if w != "" {
			tokens = append(tokens, w)
		}
	}
	for _, a := range hk.Aliases {
		a = strings.ToLower(strings.TrimSpace(a))
		if a != "" {
			tokens = append(tokens, a)
		}
	}
	for _, t := range tokens {
		if strings.Contains(lowerText, t) {
			return true
		}
	}
	return false
}

// SelectHookConfig returns the first hook whose wake/alias tokens appear in
// the provided text. If none match, it falls back to the first configured hook.
// The returned index is the position in cfg.Hooks (or 0 on fallback); -1 when none.
func SelectHookConfig(cfg *config.Config, text string) (*config.HookConfig, int) {
	if len(cfg.Hooks) == 0 {
		return nil, -1
	}
	lower := strings.ToLower(text)
	for i := range cfg.Hooks {
		hk := &cfg.Hooks[i]
		if hookMatches(lower, hk) {
			return hk, i
		}
	}
	return &cfg.Hooks[0], 0
}
