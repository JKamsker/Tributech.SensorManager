### Concept Document for Sensor Management API

#### Overview
This document outlines the design and implementation plan for a Sensor Management API. The API will allow users to manage sensors and their metadata, proxy sensor data retrieval, and enforce role-based authorization. The API will be developed using ASP.NET Core (.NET 8) and EF Core 8, with caching and unit tests to ensure optimal performance and reliability.

### Endpoints and Controllers

#### 1. SensorController

- **GET /sensors/{id}/values**
  - **Description**: Retrieves the live values of a specific sensor by proxying the request to the Tributech API.
  - **Response**: JSON array of sensor values.

- **GET /sensors**
  - **Description**: Retrieves a list of all sensors.
  - **Response**: JSON array of sensor objects.

- **GET /sensors/{id}**
  - **Description**: Retrieves details of a specific sensor.
  - **Response**: JSON object of the sensor.

- **POST /sensors**
  - **Description**: Adds a new sensor.
  - **Request Body**: JSON object containing sensor details.
  - **Response**: JSON object of the created sensor.

- **PUT /sensors/{id}**
  - **Description**: Updates an existing sensor.
  - **Request Body**: JSON object containing updated sensor details.
  - **Response**: JSON object of the updated sensor.

- **DELETE /sensors/{id}**
  - **Description**: Deletes a sensor.
  - **Response**: HTTP 204 No Content.

#### 2. SensorMetadataController

- **GET /sensors/{sensorId}/metadata**
  - **Description**: Retrieves metadata for a specific sensor.
  - **Response**: JSON array of metadata objects.

- **POST /sensors/{sensorId}/metadata**
  - **Description**: Adds metadata to an existing sensor.
  - **Request Body**: JSON object containing metadata details.
  - **Response**: JSON object of the created metadata.

- **PUT /sensors/{sensorId}/metadata/{id}**
  - **Description**: Updates existing metadata.
  - **Request Body**: JSON object containing updated metadata details.
  - **Response**: JSON object of the updated metadata.

- **DELETE /sensors/{sensorId}/metadata/{id}**
  - **Description**: Deletes metadata.
  - **Response**: HTTP 204 No Content.

### Database Design

#### Tables

1. **Sensors**
   - **Columns**:
     - `Id` (Primary Key, GUID)
     - `Name` (string, required, max length 128)
     - `Type` (SensorType, nullable, max length 64)
     - **Relationships**:
       - One-to-many relationship with `SensorMetadata` (owned type)

2. **SensorMetadata**
   - **Columns**:
     - `Id` (Primary Key, GUID)
     - `SensorId` (Foreign Key, GUID)
     - `Key` (string, required, max length 64)
     - `Value` (string, required, max length 128)

3. **MandatoryMetadata**
   - **Columns**:
     - `Id` (Primary Key, GUID)
     - `SensorType` (SensorType, unique, required, max length 64)
     - **Relationships**:
       - One-to-many relationship with `MandatoryMetadataItem` (owned type)

4. **MandatoryMetadataItem**
   - **Columns**:
     - `Id` (Primary Key, GUID)
     - `MandatoryMetadataId` (Foreign Key, GUID)
     - `Key` (string, required, max length 64)
     - `Type` (FieldType, required, default value `FieldType.None`, max length 64)
     - `DefaultValue` (string, max length 128)

### Entity-Relationship Diagram (ERD)

```plaintext
+------------------+        +----------------------+
|     Sensors      |        |    SensorMetadata    |
+------------------+        +----------------------+
| Id (PK, GUID)    |<------1| Id (PK, GUID)        |
| Name (string)    |        | SensorId (FK, GUID)  |
| Type (SensorType)|        | Key (string)         |
|                  |        | Value (string)       |
+------------------+        +----------------------+

+---------------------+        +---------------------------+
|   MandatoryMetadata |        |    MandatoryMetadataItem  |
+---------------------+        +---------------------------+
| Id (PK, GUID)       |<------1| Id (PK, GUID)             |
| SensorType          |        | MandatoryMetadataId (FK)  |
|                     |        | Key (string)              |
|                     |        | Type (FieldType)          |
|                     |        | DefaultValue (string)     |
+---------------------+        +---------------------------+
```

### Implementation Details

#### 1. ASP.NET Core API
- **Framework**: ASP.NET Core (.NET 8)
- **Design Pattern**: RESTful API, DDD, CQRS with MediatR
- **Principles**: KISS, SOLID, DRY
- **Dependency Injection**: Used for service and repository layers

#### 2. Database Setup
- **ORM**: EF Core 8
- **Database**: MSSQL (In-memory for testing)
- **Migrations**: Utilize EF Core migrations for database setup and updates

#### 3. CRUD Operations
- **Validation**: Implement basic validation for sensor and metadata inputs
- **Error Handling**: Return appropriate validation errors

#### 4. Proxy Call to Tributech Platform
- **Endpoint**: `https://testplatform.io/values/double?StreamId=<your-sensor-id>&From=<your-from-timestamp>&To=<your-to-timestamp>`
- **Proxy Implementation**: Use Abstraction over HTTP Client to call the Tributech API, use a Dummy service for testing. Polly for resilience and transient fault handling.

#### 5. Authorization
- **Keycloak**: Import realm file and configure roles
  - **Roles**:
    - Admin: Full access
    - Support Level 3: Update access
    - Read-Only: View access
- **Authorization Logic**: Implement role-based permissions in the controllers

#### 6. Caching
- **Strategy**: Cache GET requests for sensor details and metadata
- **Implementation**: Use in-memory caching
- **Considerations**: 
  - Cache invalidation strategies and potential stale data issues
  - In-memory caching because it's simple and effective for an PoC. If alot scaling is needed, consider external cache. (``IDistributedCache`` with Redis) 

#### 7. Unit Tests
- **Framework**: xUnit

### Conclusion

This concept document provides a structured plan for implementing the Sensor Management API, focusing on robust CRUD operations, efficient data retrieval via proxy, secure role-based authorization, and optimal performance through caching. The use of ASP.NET Core, EF Core, and modern development practices will ensure a scalable and maintainable solution.

