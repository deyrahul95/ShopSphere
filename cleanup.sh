#!/bin/bash

# Configuration
VERSION_FILE=".version"
IMAGE_PREFIX="shopsphare"
REPO_PREFIX="deyrahul95"

# Read or initialize version
if [ ! -f "$VERSION_FILE" ]; then
    echo "0.0.0" > "$VERSION_FILE"
fi

CURRENT_VERSION=$(cat "$VERSION_FILE")

# Remove old versioned images 
SERVICES=$(docker-compose config --services)
for SERVICE in $SERVICES; do
    IMAGE_SERVICE_NAME="${SERVICE//_/-}"
    
    OLD_IMAGE="$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:$CURRENT_VERSION"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi

    OLD_IMAGE="$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:latest"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi

    OLD_IMAGE="$REPO_PREFIX/$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:$CURRENT_VERSION"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi

    OLD_IMAGE="$REPO_PREFIX/$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:latest"
    if docker image inspect "$OLD_IMAGE" > /dev/null 2>&1; then
        echo "🗑️ Removing old image: $OLD_IMAGE"
        docker rmi "$OLD_IMAGE"
    fi
done