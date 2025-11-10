# Task Management API

A microservices-based task management system with Clean Architecture, JWT authentication, Redis caching, and Docker orchestration.

## Architecture

This project implements **Clean Architecture** with two main microservices:

- **TaskManagement.API** (Ports 5000/5001) - Core task management service
- **NotificationService.API** (Ports 5002/5003) - Notification microservice
- **PostgreSQL** (Port 5432) - Relational database
- **Redis** (Port 6379) - Distributed caching

### Microservices Communication

The services communicate via **synchronous HTTP calls**. When a task is assigned to a user, TaskManagement.API sends an HTTP request to NotificationService.API to log the notification.

## Technologies

- **Backend:** ASP.NET Core 9.0
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API)
- **Database:** PostgreSQL 16 with Entity Framework Core
- **Caching:** Redis with StackExchange.Redis
- **Authentication:** JWT Bearer tokens
- **Authorization:** Role-based (User, Manager, Admin)
- **Containerization:** Docker + Docker Compose
- **Documentation:** Swagger/OpenAPI

## Quick Start with Docker

### Prerequisites

- Docker Desktop installed and running
- Git

### 1. Clone Repository

git clone https://github.com/melqonyanaghvan/TaskManagementAPI
cd TaskManagementAPI

### 2. Start All Services

docker-compose up --build

This will start:
- PostgreSQL database
- Redis cache
- TaskManagement.API
- NotificationService.API

### 3. Access Services

- **TaskManagement API:** http://localhost:5000/swagger
- **NotificationService API:** http://localhost:5002/swagger

### 4. Stop Services

docker-compose down

To remove all data (including database):

docker-compose down -v


## 📋 API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Auth/register` | Register new user | No |
| POST | `/api/Auth/login` | Login and get JWT token | No |

### Projects

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | `/api/Projects` | Get all projects | Yes | All |
| GET | `/api/Projects/{id}` | Get project by ID | Yes | All |
| POST | `/api/Projects` | Create new project | Yes | Admin, Manager |
| PUT | `/api/Projects/{id}` | Update project | Yes | Admin, Manager |
| DELETE | `/api/Projects/{id}` | Delete project | Yes | Admin |

### Tasks

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | `/api/Tasks` | Get all tasks (paginated) | Yes | All |
| GET | `/api/Tasks/{id}` | Get task by ID | Yes | All |
| POST | `/api/Tasks` | Create new task | Yes | Admin, Manager |
| PUT | `/api/Tasks/{id}` | Update task | Yes | Admin, Manager |
| DELETE | `/api/Tasks/{id}` | Delete task | Yes | Admin |
| POST | `/api/Tasks/{id}/assign/{userId}` | Assign task to user | Yes | Admin, Manager |

### Users

| Method | Endpoint | Description | Auth Required | Roles |
|--------|----------|-------------|---------------|-------|
| GET | `/api/Users` | Get all users | Yes | Admin |
| GET | `/api/Users/{id}` | Get user by ID | Yes | All |

### Notifications (NotificationService)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Notifications/send` | Send notification | No |
| GET | `/api/Notifications/all` | Get all notifications | No |
| GET | `/api/Notifications/user/{userId}` | Get user notifications | No |

## Testing Example

### 1. Register Admin User

POST http://localhost:5000/api/Auth/register
Content-Type: application/json

{
"username": "admin",
"email": "admin@test.com",
"password": "Admin123!",
"role": "Admin"
}

### 2. Login

POST http://localhost:5000/api/Auth/login
Content-Type: application/json

{
"email": "admin@test.com",
"password": "Admin123!"
}


**Copy the JWT token from response.**

### 3. Create Project

POST http://localhost:5000/api/Projects
Authorization: Bearer {your_token}
Content-Type: application/json

{
"name": "My First Project",
"description": "Testing microservices integration"
}

### 4. Create Task
POST http://localhost:5000/api/Tasks
Authorization: Bearer {your_token}
Content-Type: application/json

{
"title": "Design Homepage",
"description": "Create modern UI mockups",
"status": "Pending",
"priority": "High",
"projectId": 1,
"deadline": "2025-11-15T18:00:00Z"
}


### 5. Register Regular User

POST http://localhost:5000/api/Auth/register
Content-Type: application/json

{
"username": "Mike",
"email": "Mike@test.com",
"password": "Mike123!",
"role": "User"
}


### 6. Assign Task to User

POST http://localhost:5000/api/Tasks/1/assign/1
Authorization: Bearer {your_token}

**This triggers HTTP call to NotificationService!**

### 7. Check Notifications

GET http://localhost:5002/api/Notifications/all


Expected response:

{
"total": 1,
"notifications": [
{
"userId": 1,
"message": "Task 'Design Homepage' has been assigned to you.",
"type": "Info",
"timestamp": "2025-11-10T18:15:00Z"
}
]
}

## User Roles

### Admin
- Full access to all resources
- Can create/update/delete projects and tasks
- Can manage users
- Can assign tasks to any user

### Manager
- Can create and update tasks
- Can assign tasks to users
- Can view all projects and tasks
- Cannot delete projects or manage users

### User
- Can view their own tasks
- Can update status of their own tasks
- Cannot create projects or tasks
- Cannot assign tasks

## Docker Commands

Start services
docker-compose up -d

View logs
docker-compose logs -f

View specific service logs
docker-compose logs -f taskmanagement-api

Stop services
docker-compose down

Rebuild and start
docker-compose up --build

Remove all data (including database)
docker-compose down -v

Check running containers
docker-compose ps


## Local Development (Without Docker)

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL 16
- Redis

### Setup

1. **Update connection strings** in `appsettings.json`:

{
"ConnectionStrings": {
"DefaultConnection": "Host=localhost;Port=5432;Database=taskmanagement;Username=postgres;Password=your_password",
"Redis": "localhost:6379"
}
}

2. **Apply migrations**:

cd TaskManagement.Infrastructure
dotnet ef database update


3. **Run services**:

Terminal 1 - TaskManagement.API
cd TaskManagement.API
dotnet run

Terminal 2 - NotificationService.API
cd NotificationService.API
dotnet run


## 🔧 Configuration

### JWT Settings

Edit `appsettings.json` in TaskManagement.API:

{
"Jwt": {
"Secret": "your-secret-key-min-32-characters",
"Issuer": "TaskManagementAPI",
"Audience": "TaskManagementClient",
"ExpiryMinutes": 60
}
}


### Database Connection

PostgreSQL connection string format:

Host=postgres;Port=5432;Database=taskmanagement;Username=postgres;Password=postgres


### Redis Connection

Redis connection string format:

redis:6379

## Features Implemented

- **Clean Architecture** with clear separation of concerns
- **JWT Authentication** with role-based authorization
- **Custom Middleware** (JWT validation, role authorization, exception handling)
- **Database Transactions** for data consistency
- **Redis Caching** for improved performance
- **Pagination and Filtering** for large datasets
- **Microservices Communication** via HTTP
- **Docker Orchestration** with docker-compose
- **Swagger Documentation** for both APIs
- **CRUD Operations** for all entities

## Contributing

- Built with ASP.NET Core 9.0
- PostgreSQL for reliable data storage
- Redis for high-performance caching
- Docker for containerization
