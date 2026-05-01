#!/usr/bin/env bash
# Manages the SQL Server Podman container for BabyLogger.
# Usage: ./podman-sqlserver.sh {start|stop|remove|logs|status}
#
# Required env var: MSSQL_SA_PASSWORD
# Example: MSSQL_SA_PASSWORD='MyStr0ng!Pass' ./podman-sqlserver.sh start

set -euo pipefail

CONTAINER_NAME="baby-logger-sqlserver"
VOLUME_NAME="baby-logger-sqlserver-data"
SA_PASSWORD="${MSSQL_SA_PASSWORD:?Set MSSQL_SA_PASSWORD env var before running this script}"
IMAGE="mcr.microsoft.com/mssql/server:2022-latest"

case "${1:-}" in
  start)
    podman volume create "${VOLUME_NAME}" 2>/dev/null || true
    podman run -d \
      --name "${CONTAINER_NAME}" \
      -e "ACCEPT_EULA=Y" \
      -e "MSSQL_SA_PASSWORD=${SA_PASSWORD}" \
      -e "MSSQL_PID=Express" \
      -p 1433:1433 \
      -v "${VOLUME_NAME}:/var/opt/mssql" \
      --restart=unless-stopped \
      "${IMAGE}"
    echo "Waiting 20 seconds for SQL Server to initialize..."
    sleep 20
    podman exec "${CONTAINER_NAME}" /opt/mssql-tools18/bin/sqlcmd \
      -S localhost -U SA -P "${SA_PASSWORD}" -No \
      -Q "IF DB_ID('BabyLoggerDb') IS NULL CREATE DATABASE BabyLoggerDb;"
    echo "SQL Server is ready. BabyLoggerDb database created."
    ;;
  stop)
    podman stop "${CONTAINER_NAME}"
    echo "SQL Server container stopped."
    ;;
  remove)
    podman stop "${CONTAINER_NAME}" 2>/dev/null || true
    podman rm "${CONTAINER_NAME}" 2>/dev/null || true
    podman volume rm "${VOLUME_NAME}" 2>/dev/null || true
    echo "SQL Server container and volume removed."
    ;;
  logs)
    podman logs -f "${CONTAINER_NAME}"
    ;;
  status)
    podman inspect "${CONTAINER_NAME}" --format "Status: {{.State.Status}}" 2>/dev/null || echo "Container not found."
    ;;
  *)
    echo "Usage: $0 {start|stop|remove|logs|status}"
    exit 1
    ;;
esac
