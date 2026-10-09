#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
ICON_DIR="$SCRIPT_DIR/icons/windows"
OUTPUT="$ICON_DIR/BingoMaker.ico"
SIZES=(16 24 32 48 64 128 256)
FILES=()

command -v magick >/dev/null 2>&1 || {
    echo "ImageMagick (magick) is required." >&2
    exit 1
}

for size in "${SIZES[@]}"; do
    file="$ICON_DIR/icon_${size}.png"
    [[ -f "$file" ]] || { echo "Icon not found: $file" >&2; exit 1; }
    dimensions="$(magick identify -format '%wx%h' "$file")"
    [[ "$dimensions" == "${size}x${size}" ]] || {
        echo "Invalid dimensions: $file ($dimensions; expected ${size}x${size})" >&2
        exit 1
    }
    FILES+=("$file")
done

magick "${FILES[@]}" "$OUTPUT"
echo "Created: $OUTPUT"
