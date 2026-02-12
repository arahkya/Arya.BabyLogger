# Arya.BabyLogger.Mobile
## Database Migrations

__Packing__
Before transferring the database to the Wpi App Server.
```aiignore
sqlite3 ./BabyLoggerDb.sqlite "PRAGMA journal_mode;"
sqlite3 ./BabyLoggerDb.sqlite "PRAGMA wal_checkpoint(FULL);"
sqlite3 ./BabyLoggerDb.sqlite "PRAGMA wal_checkpoint(TRUNCATE);"
```
> Note: SQLite does required this packing from checkpoint to main database.