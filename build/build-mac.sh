#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_FILE="$REPO_DIR/BingoMaker/BingoMaker.csproj"
ARTIFACTS_DIR="$REPO_DIR/artifacts"
ICON_FILE="$SCRIPT_DIR/icons/macos/BingoMaker.icns"

ARCH="${1:-}"

case "$ARCH" in
    x64|arm64)
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

BUNDLE_VERSION="${VERSION%%-*}"

if [ ! -f "$PROJECT_FILE" ]; then
    echo "Project not found: $PROJECT_FILE" >&2
    exit 1
fi

RUNTIME="osx-$ARCH"
PUBLISH_DIR="$REPO_DIR/BingoMaker/bin/Release/net10.0/$RUNTIME/publish"
ARTIFACT_FILE="$ARTIFACTS_DIR/BingoMaker-$VERSION-macos-$ARCH.dmg"

if [ ! -f "$ICON_FILE" ]; then
    echo "Icon not found: $ICON_FILE" >&2
    exit 1
fi

dotnet publish "$PROJECT_FILE" \
    --configuration Release \
    --runtime "$RUNTIME" \
    --self-contained true \
    --output "$PUBLISH_DIR"

TMP_DIR="$(mktemp -d)"
trap 'rm -rf "$TMP_DIR"' EXIT

PACKAGE_DIR="$TMP_DIR/stage"
APP_DIR="$PACKAGE_DIR/BingoMaker.app"

mkdir -p "$APP_DIR/Contents/MacOS"
mkdir -p "$APP_DIR/Contents/Resources"

cp "$ICON_FILE" "$APP_DIR/Contents/Resources/BingoMaker.icns"
cp -R "$PUBLISH_DIR/." "$APP_DIR/Contents/MacOS/"
chmod 755 "$APP_DIR/Contents/MacOS/BingoMaker"

cat > "$APP_DIR/Contents/Info.plist" <<EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN"
    "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>BingoMaker</string>
    <key>CFBundleDisplayName</key>
    <string>BingoMaker</string>
    <key>CFBundleIdentifier</key>
    <string>com.friilancer.bingomaker</string>
    <key>CFBundleExecutable</key>
    <string>BingoMaker</string>
    <key>CFBundleIconFile</key>
    <string>BingoMaker.icns</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleInfoDictionaryVersion</key>
    <string>6.0</string>
    <key>CFBundleShortVersionString</key>
    <string>$BUNDLE_VERSION</string>
    <key>CFBundleVersion</key>
    <string>$BUNDLE_VERSION</string>
    <key>CFBundleSupportedPlatforms</key>
    <array>
        <string>MacOSX</string>
    </array>
    <key>NSHighResolutionCapable</key>
    <true/>
    <key>LSMinimumSystemVersion</key>
    <string>12.0</string>
    <key>CFBundleDevelopmentRegion</key>
    <string>en</string>
</dict>
</plist>
EOF

codesign --force --deep --sign - "$APP_DIR"

ln -s /Applications "$PACKAGE_DIR/Applications"

mkdir -p "$ARTIFACTS_DIR"

hdiutil create \
    -volname "BingoMaker" \
    -srcfolder "$PACKAGE_DIR" \
    -format ULMO \
    -ov \
    "$ARTIFACT_FILE"

echo "Ready: $ARTIFACT_FILE"
