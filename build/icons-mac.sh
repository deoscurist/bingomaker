#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd)"
ICON_DIR="$SCRIPT_DIR/icons/macos"
OUTPUT="$ICON_DIR/BingoMaker.icns"
NAMES=(
    icon_16x16 icon_16x16@2x
    icon_32x32 icon_32x32@2x
    icon_128x128 icon_128x128@2x
    icon_256x256 icon_256x256@2x
    icon_512x512 icon_512x512@2x
)
SIZES=(16 32 32 64 128 256 256 512 512 1024)

command -v iconutil >/dev/null 2>&1 || {
    echo "macOS iconutil is required." >&2
    exit 1
}
command -v magick >/dev/null 2>&1 || {
    echo "ImageMagick (magick) is required." >&2
    exit 1
}

for i in "${!NAMES[@]}"; do
    file="$ICON_DIR/${NAMES[$i]}.png"
    size="${SIZES[$i]}"
    [[ -f "$file" ]] || { echo "Icon not found: $file" >&2; exit 1; }
    dimensions="$(magick identify -format '%wx%h' "$file")"
    [[ "$dimensions" == "${size}x${size}" ]] || {
        echo "Invalid dimensions: $file ($dimensions; expected ${size}x${size})" >&2
        exit 1
    }
done

WORK_DIR="$(mktemp -d)"
trap 'rm -rf -- "$WORK_DIR"' EXIT
ICONSET="$WORK_DIR/BingoMaker.iconset"
mkdir "$ICONSET"
for name in "${NAMES[@]}"; do
    cp "$ICON_DIR/$name.png" "$ICONSET/$name.png"
done

iconutil -c icns "$ICONSET" -o "$OUTPUT"
echo "Created: $OUTPUT"
