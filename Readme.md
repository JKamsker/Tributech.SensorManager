# Tributech.SensorManager

## Overview
Tributech.SensorManager is a robust API designed for managing sensors and their metadata, proxying sensor data retrieval, and enforcing role-based authorization. Built with ASP.NET Core (.NET 8) and EF Core 8, this API ensures high performance and reliability through caching and unit tests.

## Features
- **Sensor Management**: Create, update, delete, and retrieve sensor data.
- **Metadata Management**: Manage sensor metadata with CRUD operations.
- **Data Retrieval**: Proxy requests to Tributech API for live sensor values.
- **Authorization**: Role-based access control to secure endpoints.
- **Caching**: Implemented caching strategy for optimal performance.

## Endpoints

### SensorController

| Method | Endpoint                  | Description                    |
|--------|---------------------------|--------------------------------|
| GET    | /sensors/{id}/values      | Retrieves live values of a specific sensor. |
| GET    | /sensors                  | Retrieves a list of all sensors. |
| GET    | /sensors/{id}             | Retrieves details of a specific sensor. |
| POST   | /sensors                  | Adds a new sensor. |
| PUT    | /sensors/{id}             | Updates an existing sensor. |
| DELETE | /sensors/{id}             | Deletes a sensor. |

### SensorMetadataController

| Method | Endpoint                           | Description                    |
|--------|------------------------------------|--------------------------------|
| GET    | /sensors/{sensorId}/metadata       | Retrieves metadata for a specific sensor. |
| POST   | /sensors/{sensorId}/metadata       | Adds metadata to a sensor. |
| PUT    | /sensors/{sensorId}/metadata/{id}  | Updates existing metadata. |
| DELETE | /sensors/{sensorId}/metadata/{id}  | Deletes metadata. |

## Technologies
- **Framework**: ASP.NET Core (.NET 8)
- **ORM**: EF Core 8
- **Caching**: In-memory and distributed caching
- **Unit Testing**: Comprehensive unit tests for reliability

## Setup
Ensure that you have a local instance of SQL Server running. Update the connection string in `appsettings.json` to match your local database configuration.

1. Clone the repository:
   ```bash
   git clone https://github.com/JKamsker/Tributech.SensorManager
   ```
2. Navigate to the project directory:
   ```bash
   cd Tributech.SensorManager
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Update the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```

## License
This project is licensed under the MIT License.

