#!/bin/bash

# Configuration
VERSION_FILE=".version"
COMPOSE_FILE="docker-compose.yml"
PREFIX="shopsphare"

# Read or initialize version
if [ ! -f "$VERSION_FILE" ]; then
    echo "0.0.0" > "$VERSION_FILE"
fi

# Increment patch version
increment_version() {
    local v=$1
    IFS='.' read -r -a parts <<< "$v"
    parts[2]=$((parts[2] + 1))
    echo "${parts[0]}.${parts[1]}.${parts[2]}"
}

CURRENT_VERSION=$(cat "$VERSION_FILE")
NEW_VERSION=$(increment_version "$CURRENT_VERSION")
echo "🔧 Building version: $NEW_VERSION"

# Remove old versioned images (optional but safe)
SERVICES=$(docker-compose config --services)
for SERVICE in $SERVICES; do
    IMAGE_SERVICE_NAME="${SERVICE//_/-}"
    OLD_IMAGE="$PREFIX-$IMAGE_SERVICE_NAME:$CURRENT_VERSION"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi
done

# Build using Docker Compose (respects build context and dockerfile)
echo "📦 Building services with Docker Compose"
COMPOSE_BAKE=true TAG=$NEW_VERSION docker-compose -f "$COMPOSE_FILE" build

# Update version
echo "$NEW_VERSION" > "$VERSION_FILE"

# Run the services
echo "🚀 Starting services with version $NEW_VERSION"
TAG=$NEW_VERSION docker-compose -f "$COMPOSE_FILE" up -d

echo "✅ All services are up and running with version $NEW_VERSION"
