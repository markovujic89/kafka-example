#!/bin/bash

INFRA_PROJECT="../LeadProducer.Infrastructure"
STARTUP_PROJECT="."

if [ "$1" == "update" ]; then
    echo "🔄 Updating database..."
    dotnet ef database update -p $INFRA_PROJECT -s $STARTUP_PROJECT
    exit 0
fi

if [ "$1" == "migrate" ]; then
    if [ -z "$2" ]; then
        echo "❗ Migration name missing."
        echo "Usage: ./db.sh migrate MigrationName"
        exit 1
    fi

    echo "📦 Creating migration: $2"
    dotnet ef migrations add "$2" -p $INFRA_PROJECT -s $STARTUP_PROJECT
    exit 0
fi

if [ "$1" == "info" ]; then
    echo "ℹ️  Getting DbContext info..."
    dotnet ef dbcontext info -p $INFRA_PROJECT -s $STARTUP_PROJECT
    exit 0
fi

echo "❗ Unknown command."
echo "Usage:"
echo "  ./db.sh migrate MigrationName"
echo "  ./db.sh update"
echo "  ./db.sh info"