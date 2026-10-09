#!/bin/bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT_FILE="$REPO_DIR/BingoMaker/BingoMaker.csproj"
ARTIFACTS_DIR="$REPO_DIR/artifacts"

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

if ! [[ "$VERSION" =~ ^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$ ]]; then
    echo "Expected release version as second argument: MAJOR.MINOR.PATCH" >&2
    exit 1
fi

if [ ! -f "$PROJECT_FILE" ]; then
    echo "Project not found: $PROJECT_FILE" >&2
    exit 1
fi

RUNTIME="win-$ARCH"
PUBLISH_DIR="$REPO_DIR/BingoMaker/bin/Release/net10.0/$RUNTIME/publish"
PUBLISHED_EXE="$PUBLISH_DIR/BingoMaker.exe"
ARTIFACT_FILE="$ARTIFACTS_DIR/BingoMaker-$VERSION-windows-$ARCH.exe"

dotnet publish "$PROJECT_FILE" \
    --configuration Release \
    --runtime "$RUNTIME" \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:EnableCompressionInSingleFile=true \
    --output "$PUBLISH_DIR"

if [ ! -f "$PUBLISHED_EXE" ]; then
    echo "Published executable not found: $PUBLISHED_EXE" >&2
    exit 1
fi

UNEXPECTED_FILES="$(
    find "$PUBLISH_DIR" -type f \
        ! -path "$PUBLISHED_EXE" \
        ! -name '*.pdb'
)"

if [ -n "$UNEXPECTED_FILES" ]; then
    echo "Unexpected files remain beside the executable:" >&2
    echo "$UNEXPECTED_FILES" >&2
    exit 1
fi

mkdir -p "$ARTIFACTS_DIR"
cp "$PUBLISHED_EXE" "$ARTIFACT_FILE"

echo "Ready: $ARTIFACT_FILE"