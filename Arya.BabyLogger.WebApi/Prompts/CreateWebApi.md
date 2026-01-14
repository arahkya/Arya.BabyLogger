# Create SleepController that works with SleepService via ISleepService

Declare {{DomainName}} as `Sleep`.

This {{DomainName}} feature has the controller and service in the `Arya.BabyLogger.WebApi` project, and request/response models in the `Arya.BabyLogger.Shared/{{DomainName}}` directory.

## Data Model (EF Core)

- Create `SleepEntity` in WebApi Db with properties:
  - `Id : Guid`
  - `SleepStartTime : DateTime` (required; stored as UTC)
  - `SleepEndTime : DateTime` (required; stored as UTC)
  - `Note : string?` (optional)
- Update `BabyLoggerDbContext` to add `DbSet<SleepEntity> Sleeps { get; set; }`.
- Add 20 recoreds Seedcs data with explicit Id and do not use loop.
- Optional: add indexes on `SleepStartTime` and `SleepEndTime` for list queries.

## Shared Models (DTOs)

Create the following under `Arya.BabyLogger.Shared/{{DomainName}}`:

- `SleepCreateRequest`
  - `startTime : DateTimeOffset` (required)
  - `endTime : DateTimeOffset` (required)
  - `note : string?`
- `SleepUpdateRequest : SleepCreateRequest`
  - `id : Guid` (required)
- `SleepListItemsResponse`
  - `items : List<SleepListItem>` where each `SleepListItem` has:
    - `id : Guid`
    - `startTime : DateTimeOffset`
    - `endTime : DateTimeOffset`
    - `durationMinutes : int` (rounded total minutes between start and end)
- `SleepDetailResponse`
  - `id : Guid`
  - `startTime : DateTimeOffset`
  - `endTime : DateTimeOffset`
  - `durationMinutes : int`
  - `note : string?`

Notes:

- Use camelCase for JSON fields.
- Map `DateTimeOffset` to `DateTime` in the entity (UTC).
- Validation: `startTime` and `endTime` are required; `endTime >= startTime`.

## Service Interface

Create `ISleepService` with:

- `Task<Guid> CreateAsync(SleepCreateRequest request)`
- `Task<SleepListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc)`
- `Task<SleepDetailResponse?> GetAsync(Guid id)`
- `Task UpdateAsync(Guid id, SleepUpdateRequest request)`
- `Task DeleteAsync(Guid id)`

## Service Implementation

- Implement `SleepService` using `BabyLoggerDbContext`.
- Convert DTO `DateTimeOffset` to UTC `DateTime` on persistence.
- Duration calculation: `durationMinutes = (int)Math.Round((end - start).TotalMinutes)`.
- Enforce validation; throw or return `BadRequest` from controller on invalid input.

## Controller (attribute routing)

Create `SleepController` in WebApi with these endpoints:

- `POST /api/sleep` → Create
  - Accepts `SleepCreateRequest`.
  - Returns `201 Created` with `Location` using `CreatedAtRoute("GetSleepById", new { id }, null)`.
- `GET /api/sleep` → List
  - Accepts optional query params: `startDate` and `endDate` (ISO-8601). If missing, returns all.
  - Returns `200 OK` with `SleepListItemsResponse`.
- `GET /api/sleep/{id}` (Name = `GetSleepById`) → Get by id
  - Returns `200 OK` with `SleepDetailResponse` or `404 NotFound`.
- `PUT /api/sleep/{id}` → Update
  - Accepts `SleepUpdateRequest`.
  - Returns `204 NoContent` on success.
- `DELETE /api/sleep/{id}` → Delete
  - Returns `204 NoContent`.

Validation handling in controller:

- If `endDate < startDate` (for list), return `400 BadRequest`.
- If `endTime < startTime` (for create/update), return `400 BadRequest`.

## DI Registration

- In `Program.cs`, register the service: `builder.Services.AddTransient<ISleepService, SleepService>();`
- Ensure `builder.Services.AddControllers();` and `app.MapControllers();` are present.

## HTTP Test File

- Create `Arya.BabyLogger.WebApi.Sleep.http` at the WebApi project root with sample calls for:
  - Create, List (with and without query), Get by id, Update, Delete.
- Use variables for `baseUrl` and a sample `sleepId`.

## EF Core Migration
- Ask User to Execute command
```
cd Arya.BabyLogger.WebApi/ && dotnet ef migrations -o Db/Migrations add "Add {{DomainName}} Table" && dotnet ef database update
```
