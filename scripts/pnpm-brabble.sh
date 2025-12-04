#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."

if [ "$#" -eq 0 ]; then
  exec ./bin/brabble start
else
  exec ./bin/brabble "$@"
fi
