#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_FILE="$REPO_DIR/BingoMaker/BingoMaker.csproj"
ARTIFACTS_DIR="$REPO_DIR/artifacts"
ICON_DIR="$SCRIPT_DIR/icons/linux"
ICON_SIZES=(16 22 24 32 48 64 128 256 512)

ARCH="${1:-}"

case "$ARCH" in
    x64)
        APPIMAGE_ARCH="x86_64"
        ;;
    arm64)
        APPIMAGE_ARCH="aarch64"
        ;;
    *)
        echo "Usage: $0 <x64|arm64> <version>" >&2
        exit 1
        ;;
esac

VERSION="${2:-}"
VERSION_PATTERN='^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-(alpha|beta|rc)\.(0|[1-9][0-9]*))?$'

if ! [[ "$VERSION" =~ $VERSION_PATTERN ]]; then
    echo "Expected version: MAJOR.MINOR.PATCH[-alpha.N|-beta.N|-rc.N]" >&2
    exit 1
fi

if [ "$(uname -s)" != "Linux" ]; then
    echo "AppImage packaging requires Linux." >&2
    exit 1
fi

if [ ! -f "$PROJECT_FILE" ]; then
    echo "Project not found: $PROJECT_FILE" >&2
    exit 1
fi

if ! command -v appimagetool >/dev/null 2>&1; then
    echo "appimagetool not found in PATH." >&2
    exit 1
fi

for size in "${ICON_SIZES[@]}"; do
    ICON_FILE="$ICON_DIR/icon_${size}.png"
    if [ ! -f "$ICON_FILE" ]; then
        echo "Icon not found: $ICON_FILE" >&2
        exit 1
    fi
done

RUNTIME="linux-$ARCH"
PUBLISH_DIR="$REPO_DIR/BingoMaker/bin/Release/net10.0/$RUNTIME/publish"
ARTIFACT_FILE="$ARTIFACTS_DIR/BingoMaker-$VERSION-linux-$APPIMAGE_ARCH.AppImage"

dotnet publish "$PROJECT_FILE" \
    --configuration Release \
    --runtime "$RUNTIME" \
    --self-contained true \
    --output "$PUBLISH_DIR"

TMP_DIR="$(mktemp -d)"
trap 'rm -rf "$TMP_DIR"' EXIT

APP_DIR="$TMP_DIR/BingoMaker.AppDir"

mkdir -p "$APP_DIR/usr/bin"
mkdir -p "$APP_DIR/usr/share/applications"

cp -R "$PUBLISH_DIR/." "$APP_DIR/usr/bin/"
chmod 755 "$APP_DIR/usr/bin/BingoMaker"

cp "$ICON_DIR/icon_256.png" "$APP_DIR/bingomaker.png"
for size in "${ICON_SIZES[@]}"; do
    ICON_TARGET_DIR="$APP_DIR/usr/share/icons/hicolor/${size}x${size}/apps"
    mkdir -p "$ICON_TARGET_DIR"
    cp "$ICON_DIR/icon_${size}.png" "$ICON_TARGET_DIR/bingomaker.png"
done

ln -s bingomaker.png "$APP_DIR/.DirIcon"

cat > "$APP_DIR/bingomaker.desktop" <<EOF
[Desktop Entry]
Type=Application
Name=BingoMaker
Exec=BingoMaker
Icon=bingomaker
Categories=Game;
Terminal=false
EOF

cp "$APP_DIR/bingomaker.desktop" \
    "$APP_DIR/usr/share/applications/bingomaker.desktop"

cat > "$APP_DIR/AppRun" <<'EOF'
#!/bin/sh
APP_DIR="$(dirname "$(readlink -f "$0")")"
exec "$APP_DIR/usr/bin/BingoMaker" "$@"
EOF

chmod 755 "$APP_DIR/AppRun"

mkdir -p "$ARTIFACTS_DIR"

ARCH="$APPIMAGE_ARCH" APPIMAGE_EXTRACT_AND_RUN=1 \
    appimagetool "$APP_DIR" "$ARTIFACT_FILE"

chmod 755 "$ARTIFACT_FILE"

echo "Ready: $ARTIFACT_FILE"
