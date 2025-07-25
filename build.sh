#!/bin/bash

# ========================
# Configuration
# ========================
VERSION_FILE=".version"
COMPOSE_FILE="docker-compose.yml"
PREFIX="shopsphare"

# ========================
# Versioning Logic
# ========================

# Read or initialize version
if [ ! -f "$VERSION_FILE" ]; then
    echo "0.0.0" > "$VERSION_FILE"
fi

# Custom version increment: patch → minor → major (each up to 9)
increment_version() {
    local v=$1
    IFS='.' read -r major minor patch <<< "$v"

    patch=$((patch + 1))
    if [ "$patch" -gt 9 ]; then
        patch=0
        minor=$((minor + 1))
    fi
    if [ "$minor" -gt 9 ]; then
        minor=0
        major=$((major + 1))
    fi

    echo "$major.$minor.$patch"
}

CURRENT_VERSION=$(cat "$VERSION_FILE")
NEW_VERSION=$(increment_version "$CURRENT_VERSION")
echo "🔧 Building version: $NEW_VERSION"

# ========================
# Remove Old Images
# ========================
SERVICES=$(docker-compose config --services)
for SERVICE in $SERVICES; do
    IMAGE_SERVICE_NAME="${SERVICE//_/-}"
    OLD_IMAGE="$PREFIX-$IMAGE_SERVICE_NAME:$CURRENT_VERSION"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi
done

# ========================
# Build with Docker Compose
# ========================
echo "📦 Building services with Docker Compose"
COMPOSE_BAKE=true TAG=$NEW_VERSION docker-compose -f "$COMPOSE_FILE" build

# ========================
# Save New Version
# ========================
echo "$NEW_VERSION" > "$VERSION_FILE"
echo "✅ Version updated to $NEW_VERSION"

# ========================
# Start Services
# ========================
echo "🚀 Starting services with version $NEW_VERSION"
TAG=$NEW_VERSION docker-compose -f "$COMPOSE_FILE" up -d

echo "✅ All services are up and running with version $NEW_VERSION"
