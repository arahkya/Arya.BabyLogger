# Create SleepController that works with SleepService via ISleepService

Declare {{DomainName}} as `BreastPump`.

This {{DomainName}} feature has the controller and service in the `Arya.BabyLogger.WebApi` project, and request/response models in the `Arya.BabyLogger.Shared/{{DomainName}}` directory.

## Data Model (EF Core)

- Create `{{DomainName}}Entity` in WebApi Db with properties:
  - `Id : Guid`
  - `PumpTime : DateTime` (required; stored as UTC)
  - `AmountML : Int` (required; stored as UTC)
  - `Note : string?` (optional)
- Update `BabyLoggerDbContext` to add `DbSet<SleepEntity> {{DomainName}}s { get; set; }`.
- Add 20 recoreds Seedcs data with explicit Id and do not use loop.
- Optional: add indexes on `PumpTime` for list queries.

## Shared Models (DTOs)

Create the following under `Arya.BabyLogger.Shared/{{DomainName}}`:

- `{{DomainName}}CreateRequest`
  - `PumpTime : DateTimeOffset` (required)
  - `AmountML : Int` (required)
  - `note : string?`
- `{{DomainName}}UpdateRequest : SleepCreateRequest`
  - `Id : Guid` (required)
- `{{DomainName}}ListItemsResponse`
  - `items : List<{{DomainName}}ListItem>` where each `{{DomainName}}ListItem` has:
    - `Id : Guid`
    - `PumpTime : DateTimeOffset`
    - `AmountML : Int`
- `{{DomainName}}DetailResponse`
  - `Id : Guid`
  - `PumpTime : DateTimeOffset`
  - `AmountML : Int`
  - `note : string?`

Notes:

- Use camelCase for JSON fields.
- Map `DateTimeOffset` to `DateTime` in the entity (UTC).
- Validation: `AmountML` must greater than 0.

## Service Interface

Create `I{{DomainName}}Service` with:

- `Task<Guid> CreateAsync({{DomainName}}CreateRequest request)`
- `Task<{{DomainName}}ListItemsResponse> ListAsync(DateTime startDateUtc, DateTime endDateUtc)`
- `Task<{{DomainName}}DetailResponse?> GetAsync(Guid id)`
- `Task UpdateAsync(Guid id, {{DomainName}}UpdateRequest request)`
- `Task DeleteAsync(Guid id)`

## Service Implementation

- Implement `{{DomainName}}Service` using `BabyLoggerDbContext`.
- Convert DTO `DateTimeOffset` to UTC `DateTime` on persistence.
- Enforce validation; throw or return `BadRequest` from controller on invalid input.

## Controller (attribute routing)

Create `{{DomainName}}Controller` in WebApi with these endpoints:

- `POST /api/{{DomainName}}` → Create
  - Accepts `{{DomainName}}CreateRequest`.
  - Returns `201 Created` with `Location` using `CreatedAtRoute("Get{{DomainName}}ById", new { id }, null)`.
- `GET /api/{{DomainName}}` → List
  - Accepts optional query params: `startDate` and `endDate` (ISO-8601). If missing, returns all.
  - Returns `200 OK` with `{{DomainName}}ListItemsResponse`.
- `GET /api/{{DomainName}}/{id}` (Name = `Get{{DomainName}}ById`) → Get by id
  - Returns `200 OK` with `{{DomainName}}DetailResponse` or `404 NotFound`.
- `PUT /api/{{DomainName}}/{id}` → Update
  - Accepts `{{DomainName}}UpdateRequest`.
  - Returns `204 NoContent` on success.
- `DELETE /api/{{DomainName}}/{id}` → Delete
  - Returns `204 NoContent`.

Validation handling in controller:

- If `endDate < startDate` (for list), return `400 BadRequest`.
- If `endTime < startTime` (for create/update), return `400 BadRequest`.

## DI Registration

- In `Program.cs`, register the service: `builder.Services.AddTransient<I{{DomainName}}Service, {{DomainName}}Service>();`
- Ensure `builder.Services.AddControllers();` and `app.MapControllers();` are present.

## HTTP Test File

- Create `Arya.BabyLogger.WebApi.{{DomainName}}.http` at the WebApi project root with sample calls for:
  - Create, List (with and without query), Get by id, Update, Delete.
- Use variables for `baseUrl` and a sample `{{DomainName}}Id`.

## EF Core Migration
- Ask User to Execute command
```
cd Arya.BabyLogger.WebApi/ && dotnet ef migrations add -o Db/Migrations "Add {{DomainName}} Table" && dotnet ef database update
```
