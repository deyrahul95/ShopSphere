#!/bin/bash

# ========================
# Configuration
# ========================
VERSION_FILE=".version"
DOCKER_USER="deyrahul95"
IMAGE_PREFIX="shopsphare"
REMOTE_PREFIX="$DOCKER_USER"

# ========================
# Versioning Logic
# ========================
if [ ! -f "$VERSION_FILE" ]; then
    echo "❌ No version file found. Run build script first."
    exit 1
fi

VERSION=$(cat "$VERSION_FILE")
echo "📦 Deploying version: $VERSION to Docker Hub"

# ========================
# Deploying Images to Docker Hub
# ========================

# Get services from docker-compose
SERVICES=$(docker-compose config --services)

# Tag and push each service
for SERVICE in $SERVICES; do
    IMAGE_SERVICE_NAME="${SERVICE//_/-}"  # user_service -> user-service

    LOCAL_IMAGE="$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:$VERSION"
    REMOTE_IMAGE="$REMOTE_PREFIX/$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:$VERSION"
    REMOTE_LATEST="$REMOTE_PREFIX/$IMAGE_PREFIX-$IMAGE_SERVICE_NAME:latest"

    # Check if local image exists
    if ! docker image inspect "$LOCAL_IMAGE" > /dev/null 2>&1; then
        echo "⚠️ Image not found locally: $LOCAL_IMAGE — Skipping."
        continue
    fi

    # Ensure it starts with the expected prefix
    if [[ "$LOCAL_IMAGE" != $IMAGE_PREFIX-* ]]; then
        echo "⏩ Skipping third-party image: $LOCAL_IMAGE"
        continue
    fi

    # Tag versioned and latest
    echo "🔖 Tagging $LOCAL_IMAGE -> $REMOTE_IMAGE"
    docker tag "$LOCAL_IMAGE" "$REMOTE_IMAGE"

    echo "🔖 Tagging $LOCAL_IMAGE -> $REMOTE_LATEST"
    docker tag "$LOCAL_IMAGE" "$REMOTE_LATEST"

    # Push both
    echo "📤 Pushing $REMOTE_IMAGE"
    docker push "$REMOTE_IMAGE"

    echo "📤 Pushing $REMOTE_LATEST"
    docker push "$REMOTE_LATEST"
done

echo "✅ Deployment complete!"
