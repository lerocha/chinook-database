#!/bin/bash

# This script runs in the background to initialize the Chinook database
# after DB2 is fully ready

(
    # Wait for DB2 instance to be fully ready and database to be accessible
    MAX_RETRIES=120  # 10 minutes max (120 * 5 seconds)
    RETRY_COUNT=0

    echo "(*) Waiting for DB2 to be fully ready before initializing Chinook database..."

    while [ $RETRY_COUNT -lt $MAX_RETRIES ]; do
        # Try to connect to the Chinook database
        if su - chinook -c "db2 connect to Chinook" > /dev/null 2>&1; then
            echo "(*) DB2 is ready! Database connection successful."

            # Run the initialization SQL script
            echo "(*) Running Chinook database initialization script..."
            su - chinook -c "db2 connect to Chinook && db2 -tf /scripts/Chinook_Db2.sql" > /dev/null 2>&1

            if [ $? -eq 0 ]; then
                echo "(*) Chinook database initialized successfully!"
                exit 0
            else
                echo "(!) Error: Failed to run Chinook initialization script"
                exit 1
            fi
        fi

        RETRY_COUNT=$((RETRY_COUNT + 1))
        if [ $((RETRY_COUNT % 12)) -eq 0 ]; then
            echo "(*) Still waiting for DB2... ($(( RETRY_COUNT * 5 )) seconds elapsed)"
        fi
        sleep 5
    done

    echo "(!) Error: DB2 did not become ready within the timeout period"
    exit 1
) &

# Return immediately so the setup script can continue
exit 0
