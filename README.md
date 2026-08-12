# Akkhor - Assignment & Submission Management System

Akkhor is a role-based Assignment & Submission Management System developed for a school or college environment.

The system allows administrators to manage users, academic years, classes, courses, subjects, teacher assignments, assignments, and submissions. Teachers can create and manage assignments, publish assignments or save them as drafts, review student submissions, provide marks and feedback, and manage submission status. Students can view assignments assigned to their class or course and submit answers according to the applicable submission rules.

This project was developed as part of the **Assistant Software Engineer Recruitment Project – Assignment & Submission Management System** by **OnnoRokom Projukti Limited**.

---

## Table of Contents

* [Project Overview](#project-overview)
* [Key Features](#key-features)
* [User Roles and Permissions](#user-roles-and-permissions)
* [Technology Stack](#technology-stack)
* [Architecture](#architecture)
* [Project Structure](#project-structure)
* [System Modules](#system-modules)
* [Prerequisites](#prerequisites)
* [Getting Started](#getting-started)
* [Backend Setup](#backend-setup)
* [Frontend Setup](#frontend-setup)
* [Database Setup](#database-setup)
* [Environment Configuration](#environment-configuration)
* [Entity Framework Core Migrations](#entity-framework-core-migrations)
* [Running the Application](#running-the-application)
* [Swagger / API Documentation](#swagger--api-documentation)
* [Authentication](#authentication)
* [Authorization](#authorization)
* [Testing](#testing)
* [Test Structure](#test-structure)
* [Business Rules Tested](#business-rules-tested)
* [Database Structure](#database-structure)
* [API Modules](#api-modules)
* [Frontend Modules](#frontend-modules)
* [Assumptions](#assumptions)
* [Known Limitations](#known-limitations)
* [Security Considerations](#security-considerations)
* [Demo Credentials](#demo-credentials)
* [Troubleshooting](#troubleshooting)
* [Future Improvements](#future-improvements)
* [Submission Checklist](#submission-checklist)
* [Project Submission](#project-submission)
* [Author](#author)

---

# Project Overview

Akkhor is a full-stack web application for managing assignments and student submissions in a school or college environment.

The system provides separate functionality for three primary roles:

* **Admin**
* **Teacher**
* **Student**

The backend is developed using ASP.NET Core Web API and provides RESTful APIs for authentication, authorization, academic management, assignment management, and submission management.

The frontend is developed using Angular, TypeScript, HTML, and SCSS.

PostgreSQL is used as the relational database, with Entity Framework Core handling database access and migrations.

---

# Key Features

## Authentication

* User registration
* User login
* JWT-based authentication
* ASP.NET Core Identity
* Password management
* Role-based authorization
* Secure authenticated API requests

## Admin Features

* User management
* Create users
* Update users
* Activate/deactivate users
* Role management
* Academic year management
* Class management
* Course management
* Subject management
* Teacher assignment
* View assignments
* View submissions
* Application-level settings

## Teacher Features

* View assigned classes, courses, and subjects
* Create assignments
* Update assignments
* Delete assignments
* Publish assignments
* Save assignments as drafts
* Define assignment title
* Define assignment description
* Define maximum marks
* Define deadline
* View student submissions
* Assign marks
* Provide feedback
* Change submission status

## Student Features

* View assignments assigned to their class/course
* View assignment details
* View assignment deadline
* Submit answers
* Update submissions before the deadline when allowed
* View submission status
* View obtained marks
* View teacher feedback

---

# User Roles and Permissions

| Feature                  | Admin | Teacher | Student |
| ------------------------ | :---: | :-----: | :-----: |
| Login                    |  Yes  |   Yes   |   Yes   |
| User Management          |  Yes  |    No   |    No   |
| Academic Year Management |  Yes  |    No   |    No   |
| Class Management         |  Yes  |    No   |    No   |
| Course Management        |  Yes  |    No   |    No   |
| Subject Management       |  Yes  |    No   |    No   |
| Teacher Assignment       |  Yes  |    No   |    No   |
| Create Assignment        |   No  |   Yes   |    No   |
| Update Assignment        |   No  |   Yes   |    No   |
| Delete Assignment        |   No  |   Yes   |    No   |
| Publish Assignment       |   No  |   Yes   |    No   |
| Save Assignment as Draft |   No  |   Yes   |    No   |
| View Assignments         |  Yes  |   Yes   |   Yes   |
| Submit Assignment        |   No  |    No   |   Yes   |
| Update Submission        |   No  |    No   |   Yes   |
| View Submissions         |  Yes  |   Yes   |   Own   |
| Grade Submission         |   No  |   Yes   |    No   |
| Provide Feedback         |   No  |   Yes   |    No   |
| Change Submission Status |   No  |   Yes   |    No   |

---

# Technology Stack

## Frontend

* Angular
* TypeScript
* HTML5
* SCSS
* Bootstrap
* Font Awesome
* REST API integration
* Form validation

## Backend

* ASP.NET Core Web API
* C#
* .NET 8
* Entity Framework Core
* ASP.NET Core Identity
* JWT Authentication
* Role-Based Authorization
* RESTful APIs
* Swagger / OpenAPI

## Database

* PostgreSQL
* Npgsql Entity Framework Core Provider

## Testing

* xUnit
* Moq
* ASP.NET Core testing utilities
* Unit testing
* Authorization testing
* Business-rule testing
* Submission workflow testing

---

# Architecture

The backend follows a layered architecture based on separation of concerns.

```text
                    ┌──────────────────────────┐
                    │       Angular UI         │
                    │ TypeScript / HTML / SCSS │
                    └────────────┬─────────────┘
                                 │
                                 │ HTTP / REST API
                                 ▼
                    ┌──────────────────────────┐
                    │       Akkhor.API         │
                    │                          │
                    │ Controllers              │
                    │ JWT Authentication       │
                    │ Authorization            │
                    │ Middleware               │
                    │ Swagger / OpenAPI        │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │   Akkhor.Application     │
                    │                          │
                    │ Services                 │
                    │ DTOs                     │
                    │ Interfaces               │
                    │ Business Logic            │
                    │ Validation                │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │     Akkhor.Domain        │
                    │                          │
                    │ Entities                 │
                    │ Domain Models            │
                    │ Business Concepts        │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │  Akkhor.Infrastructure   │
                    │                          │
                    │ Repositories             │
                    │ Entity Framework Core    │
                    │ ApplicationDbContext     │
                    │ Migrations               │
                    │ Identity Persistence     │
                    └────────────┬─────────────┘
                                 │
                                 ▼
                    ┌──────────────────────────┐
                    │       PostgreSQL         │
                    │         Database         │
                    └──────────────────────────┘
```

## Architecture Layers

### Akkhor.API

The API layer is responsible for:

* HTTP request and response handling
* RESTful API endpoints
* Controllers
* JWT authentication
* Role-based authorization
* Middleware
* Swagger/OpenAPI configuration
* Dependency injection configuration

### Akkhor.Application

The application layer contains the application's business logic.

Responsibilities include:

* Application services
* DTOs
* Service interfaces
* Business rules
* Validation
* Application-level workflows

### Akkhor.Domain

The domain layer contains the core business entities and domain models.

Examples include:

* Users
* Roles
* Academic Years
* Classes
* Sections
* Courses
* Subjects
* Teacher Assignments
* Student Enrollments
* Assignments
* Assignment Submissions

The domain layer is independent of infrastructure and database implementation details.

### Akkhor.Infrastructure

The infrastructure layer is responsible for:

* Entity Framework Core
* PostgreSQL database access
* ApplicationDbContext
* Repository implementations
* Entity configurations
* Database migrations
* ASP.NET Core Identity persistence

### PostgreSQL

PostgreSQL is used as the primary relational database.

Entity Framework Core is used to manage:

* Database relationships
* Queries
* Insert/update/delete operations
* Migrations
* Database schema

---

# Project Structure

```text
Akkhor-A-School-Management/
│
├── Akkhor_Backend/
│   │
│   ├── src/
│   │   │
│   │   ├── Akkhor.API/
│   │   │   ├── Controllers/
│   │   │   ├── Middleware/
│   │   │   ├── Properties/
│   │   │   ├── Program.cs
│   │   │   └── appsettings.json
│   │   │
│   │   ├── Akkhor.Application/
│   │   │   ├── DTOs/
│   │   │   ├── Interfaces/
│   │   │   ├── Services/
│   │   │   └── ...
│   │   │
│   │   ├── Akkhor.Domain/
│   │   │   ├── Entities/
│   │   │   └── ...
│   │   │
│   │   └── Akkhor.Infrastructure/
│   │       ├── Data/
│   │       ├── Repositories/
│   │       ├── Configurations/
│   │       ├── Migrations/
│   │       └── ...
│   │
│   ├── tests/
│   │   └── Akkhor.Tests/
│   │       │
│   │       ├── Authorization/
│   │       │   ├── AdminAuthorizationTests.cs
│   │       │   ├── StudentAuthorizationTests.cs
│   │       │   ├── TeacherAuthorizationTests.cs
│   │       │   └── TestAuthenticationHandler.cs
│   │       │
│   │       ├── Controllers/
│   │       │   └── AuthControllerTests.cs
│   │       │
│   │       └── service/
│   │           ├── AssignmentServiceTests.cs
│   │           ├── AssignmentSubmissionServiceTests.cs
│   │           ├── ClassServiceTests.cs
│   │           ├── StudentEnrollmentServiceTests.cs
│   │           ├── TeacherAssignmentServiceTests.cs
│   │           └── UserManagementServiceTests.cs
│   │
│   └── Akkhor.sln
│
├── Akkhor_Frontend/
│   │
│   └── frontend/
│       ├── src/
│       │   ├── app/
│       │   ├── assets/
│       │   ├── environments/
│       │   └── ...
│       ├── angular.json
│       ├── package.json
│       ├── package-lock.json
│       ├── tsconfig.json
│       └── ...
│
├── README.md
└── .gitignore
```

---

# System Modules

## 1. Authentication Module

The authentication module provides:

* User registration
* User login
* JWT token generation
* Password validation
* Role information
* Authenticated API access

---

## 2. User Management Module

Administrators can manage application users.

Features include:

* View users
* Create users
* Update users
* Activate users
* Deactivate users
* Assign roles
* Manage user information

---

## 3. Academic Year Module

The Academic Year module manages academic sessions.

Example:

```text
2025-2026
2026-2027
```

An academic year can contain multiple classes.

---

## 4. Class Module

The Class module manages academic classes and their sections.

Examples:

```text
Class 6
Class 7
Class 8
Class 9
Class 10
```

Classes are associated with an academic year.

---

## 5. Course Module

Courses are associated with classes.

A course can contain multiple subjects through the course-subject relationship.

---

## 6. Subject Module

Subjects represent academic subjects.

Examples:

```text
Mathematics
English
Physics
Chemistry
Bangla
Computer Science
```

---

## 7. Teacher Assignment Module

The Teacher Assignment module determines which academic areas a teacher is responsible for.

A teacher assignment can contain:

```text
Teacher
Academic Year
Class
Section
Course
Subject
```

This relationship is also used for authorization and assignment ownership validation.

---

## 8. Student Enrollment Module

The Student Enrollment module associates students with their academic class/course context.

This information is used to determine which assignments a student is eligible to view and submit.

---

## 9. Assignment Module

Teachers can create and manage assignments.

Assignment information includes:

```text
Title
Description
Maximum Marks
Deadline
Teacher
Class
Course
Subject
Status
CreatedAt
UpdatedAt
```

Teachers can:

* Create assignments
* Update assignments
* Delete assignments
* Publish assignments
* Save assignments as drafts

---

## 10. Assignment Submission Module

Students can submit answers to assignments.

Students can:

* View eligible assignments
* View assignment details
* Submit answers
* Update submissions before the deadline when allowed
* View submission status
* View marks
* View teacher feedback

Teachers can:

* View submissions
* Assign marks
* Provide feedback
* Change submission status

---

# Prerequisites

Before running Akkhor locally, install the following:

* .NET 8 SDK
* Node.js
* npm
* Angular CLI
* PostgreSQL
* Git

Verify the installed versions:

```bash
dotnet --version
node --version
npm --version
ng version
psql --version
git --version
```

Recommended:

```text
.NET SDK: 8.x
PostgreSQL: 14+
Node.js: LTS version
Angular CLI: Compatible with package.json
```

---

# Getting Started

Clone the repository:

```bash
git clone https://github.com/Sefat3674/Akkhor-Assignment-Submission-Management-System.git
```

Navigate into the project:

```bash
cd Akkhor-A-School-Management
```

The repository contains:

```text
Akkhor_Backend
Akkhor_Frontend
README.md
```

The backend and frontend can be run independently.

---

# Backend Setup

Navigate to the backend directory:

```bash
cd Akkhor_Backend
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the backend:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project src/Akkhor.API
```

The API URL depends on the local ASP.NET Core configuration.

Check:

```text
Akkhor_Backend/src/Akkhor.API/Properties/launchSettings.json
```

for the configured HTTP/HTTPS ports.

---

# Frontend Setup

Open another terminal and navigate to:

```bash
cd Akkhor_Frontend/frontend
```

Install npm dependencies:

```bash
npm install
```

Run the Angular application:

```bash
ng serve
```

The frontend is normally available at:

```text
http://localhost:4200
```

---

# Database Setup

Akkhor uses PostgreSQL as its relational database.

Create a database named:

```text
AkkhorSchoolDb
```

Example:

```sql
CREATE DATABASE "AkkhorSchoolDb";
```

Configure the database connection in the backend configuration.

Example:

```text
Host=localhost;
Port=5432;
Database=AkkhorSchoolDb;
Username=postgres;
Password=YOUR_PASSWORD
```

Do not commit real database passwords or other sensitive credentials to Git.

---

# Environment Configuration

Sensitive configuration should not be committed to the repository.

Create local configuration based on the project's example configuration.

Example environment values:

```env
DATABASE_HOST=localhost
DATABASE_PORT=5432
DATABASE_NAME=AkkhorSchoolDb
DATABASE_USER=postgres
DATABASE_PASSWORD=YOUR_PASSWORD

JWT_ISSUER=Akkhor
JWT_AUDIENCE=AkkhorUsers
JWT_SECRET=YOUR_SECRET_KEY

API_URL=https://localhost:50268
```

Replace the placeholder values with local development values.

> The exact configuration keys depend on the current application configuration. Keep production secrets outside source control.

---

# Entity Framework Core Migrations

Akkhor uses Entity Framework Core migrations for database schema management.

From the backend directory, run:

```bash
dotnet ef database update --project src/Akkhor.Infrastructure --startup-project src/Akkhor.API
```

If `dotnet ef` is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

The migration files are included in the repository so that the evaluator can create the required database schema without manually creating all tables.

---

# Running the Application

Start PostgreSQL first.

Then start the backend:

```bash
cd Akkhor_Backend

dotnet run --project src/Akkhor.API
```

Open another terminal and start the frontend:

```bash
cd Akkhor_Frontend/frontend

ng serve
```

Then open:

```text
http://localhost:4200
```

The frontend communicates with the ASP.NET Core API through the configured API URL.

---

# Swagger / API Documentation

The backend provides Swagger/OpenAPI documentation.

After starting the API, open the Swagger URL configured by the application.

For example:

```text
https://localhost:50268/swagger
```

The port may be different depending on the local configuration.

Swagger can be used to:

* View API endpoints
* View request models
* View response models
* Test API endpoints
* Authenticate using JWT
* Verify role-based authorization

---

# Authentication

Akkhor uses JWT-based authentication.

The authentication flow is:

```text
User
  │
  ▼
Login
  │
  ▼
Auth API
  │
  ▼
Validate Credentials
  │
  ▼
Generate JWT
  │
  ▼
Frontend receives token
  │
  ▼
JWT sent with API requests
  │
  ▼
Backend validates JWT
  │
  ▼
Role-based authorization
```

Authenticated API requests require a valid JWT access token.

---

# Authorization

The backend enforces role-based authorization.

Primary application roles are:

```text
Admin
Teacher
Student
```

The system may also contain Identity roles such as:

```text
SuperAdmin
Normal User
```

depending on the configured application data.

Authorization is enforced on the backend rather than relying only on frontend navigation.

Examples:

```text
Admin
 ├── User Management
 ├── Academic Years
 ├── Classes
 ├── Courses
 ├── Subjects
 └── Teacher Assignments

Teacher
 ├── Create Assignment
 ├── Update Assignment
 ├── Delete Assignment
 ├── Publish Assignment
 ├── View Submissions
 ├── Grade Submissions
 └── Provide Feedback

Student
 ├── View Assignments
 ├── Submit Assignment
 ├── Update Submission
 └── View Own Submission Result
```

---

# Testing

The project contains unit tests for important business rules, authorization, authentication, and submission workflows.

Run all tests from the backend directory:

```bash
dotnet test
```

Run tests with a build:

```bash
dotnet build
dotnet test
```

The test project is:

```text
tests/Akkhor.Tests
```

---

# Test Structure

```text
tests/
└── Akkhor.Tests/
    │
    ├── Authorization/
    │   ├── AdminAuthorizationTests.cs
    │   ├── StudentAuthorizationTests.cs
    │   ├── TeacherAuthorizationTests.cs
    │   └── TestAuthenticationHandler.cs
    │
    ├── Controllers/
    │   └── AuthControllerTests.cs
    │
    └── service/
        ├── AssignmentServiceTests.cs
        ├── AssignmentSubmissionServiceTests.cs
        ├── ClassServiceTests.cs
        ├── StudentEnrollmentServiceTests.cs
        ├── TeacherAssignmentServiceTests.cs
        └── UserManagementServiceTests.cs
```

---

# Business Rules Tested

## Assignment Service

`AssignmentServiceTests.cs` covers important assignment business rules such as:

* Teacher creates assignment
* Teacher updates assignment
* Teacher deletes assignment
* Teacher publishes assignment
* Teacher saves assignment as draft
* Required title validation
* Description validation
* Maximum marks validation
* Deadline validation
* Assignment must belong to the teacher's assigned class/course/subject
* Teacher cannot modify another teacher's assignment

---

## Assignment Submission Service

`AssignmentSubmissionServiceTests.cs` covers important submission workflow rules such as:

* Student submits assignment
* Student updates submission
* Submission deadline validation
* Student cannot submit unauthorized assignments
* Student cannot modify another student's submission
* Teacher can view eligible submissions
* Teacher can assign marks
* Teacher can provide feedback
* Teacher can change submission status
* Invalid marks are rejected

---

## Authorization Tests

The following tests verify role-based authorization:

```text
AdminAuthorizationTests.cs
StudentAuthorizationTests.cs
TeacherAuthorizationTests.cs
```

These tests verify that protected operations are accessible only to appropriate roles.

---

## Authentication Tests

`AuthControllerTests.cs` covers authentication-related API behavior such as:

* Login
* Invalid credentials
* Authentication responses
* Registration behavior where applicable

---

## Other Service Tests

The following service test classes cover additional business functionality:

```text
ClassServiceTests.cs
StudentEnrollmentServiceTests.cs
TeacherAssignmentServiceTests.cs
UserManagementServiceTests.cs
```

---

# Database Structure

The application uses PostgreSQL with Entity Framework Core.

Main application tables include:

```text
AcademicYears
Classes
ClassSections
Courses
CourseSubjects
Subjects
TeacherAssignments
StudentEnrollments
Assignments
AssignmentSubmissions
```

ASP.NET Core Identity tables include:

```text
Users
Roles
UserRoles
UserClaims
UserLogins
UserTokens
RoleClaims
```

The actual database schema is managed through Entity Framework Core entities, configurations, and migrations.

---

# Core Database Relationships

The main academic relationship is:

```text
Academic Year
      │
      ▼
    Class
      │
      ├──────────────► Class Section
      │
      └──────────────► Course
                         │
                         ▼
                  Course Subject
                         │
                         ▼
                      Subject
```

Teacher assignment:

```text
Teacher
   │
   ▼
Teacher Assignment
   │
   ├── Academic Year
   ├── Class
   ├── Section
   ├── Course
   └── Subject
```

Assignment and submission:

```text
Teacher
   │
   ▼
Assignment
   │
   ├── Class
   ├── Course
   ├── Subject
   ├── Deadline
   ├── Maximum Marks
   └── Status
          │
          ▼
   Assignment Submission
          │
          ├── Student
          ├── Answer
          ├── Marks
          ├── Feedback
          └── Status
```

---

# API Modules

The backend provides RESTful APIs for the major application modules.

Main API areas include:

```text
/api/Auth
/api/users
/api/academic-years
/api/courses
/api/teacher-assignments
/api/assignments
/api/assignment-submissions
```

The exact endpoints, request models, response models, and authorization requirements are available through Swagger.

---

# Frontend Modules

The frontend contains user interfaces for the major application modules.

```text
Authentication
├── Login
└── Register

Admin
├── User Management
├── Academic Years
├── Classes
├── Courses
├── Subjects
├── Teacher Assignments
├── Assignments
└── Submissions

Teacher
├── Dashboard
├── Assignments
├── Assignment Creation
├── Assignment Editing
└── Submissions

Student
├── Dashboard
├── Assignments
├── Assignment Details
└── Submissions
```

---

# Assumptions

The following assumptions were made where the original project requirements did not define detailed implementation rules.

## Assignment Ownership

A teacher can create or manage an assignment only when the teacher is assigned to the relevant academic context.

The relevant context may include:

* Academic Year
* Class
* Section
* Course
* Subject

## Assignment Drafts

Teachers can save assignments as drafts.

Draft assignments are not treated as published assignments for students.

## Published Assignments

Only published assignments are available to eligible students for submission.

## Assignment Deadline

The assignment deadline determines whether a student can submit or update a submission.

After the deadline, submission modification is restricted according to the application's business rules.

## Student Eligibility

A student can access an assignment only when the student belongs to the relevant class/course/academic context.

## Submission Ownership

A student can create and modify only their own submission.

A student cannot modify another student's submission.

## Teacher Submission Access

A teacher can view and evaluate submissions only for assignments belonging to the teacher's authorized academic context.

## Marks

The obtained marks cannot exceed the assignment's maximum marks.

Maximum marks must be a valid positive value.

## Role-Based Access

Backend authorization is treated as the source of truth for access control.

Frontend route and menu restrictions are not considered sufficient security by themselves.

---

# Known Limitations

The following features are outside the core scope of the recruitment assignment and may be added in future versions:

* Email notifications
* Real-time notifications
* Assignment file attachments
* Student file uploads
* Advanced reporting
* Attendance management
* Parent/guardian portal
* Online examination module
* Advanced analytics
* Docker deployment
* Cloud deployment
* CI/CD pipeline
* Advanced audit logging

These limitations do not affect the core assignment and submission workflow.

---

# Security Considerations

The application uses several security mechanisms:

* JWT-based authentication
* ASP.NET Core Identity
* Role-based authorization
* Backend authorization checks
* Business-level ownership validation
* Input validation
* Password hashing through ASP.NET Core Identity
* Sensitive configuration excluded from source control

For production deployment, the following should be configured:

* HTTPS
* Strong JWT secret
* Secure database credentials
* Environment variables
* Proper CORS configuration
* Secure logging
* Rate limiting where required

---

# Demo Credentials

The recruitment assignment requires working demo accounts for:

* Admin
* Teacher
* Student

Replace the placeholders below with the actual credentials used for evaluation.

## Admin

```text
Email: admin@akkhor.com
Password: Admin@12345
```

## Teacher

```text
Email: teacher@akkhor.com
Password: Teacher@12345
```

## Student

```text
Email: student@akkhor.com
Password: Student@12345
```

> Do not use production credentials or personal passwords in the repository.

---

# Troubleshooting

## PostgreSQL Connection Error

Verify that:

1. PostgreSQL is running.
2. The database exists.
3. The username is correct.
4. The password is correct.
5. The PostgreSQL port is correct.
6. The connection string uses PostgreSQL/Npgsql format.

Example:

```text
Host=localhost;
Port=5432;
Database=AkkhorSchoolDb;
Username=postgres;
Password=YOUR_PASSWORD
```

---

## Entity Framework Migration Error

Run:

```bash
dotnet restore
```

Then:

```bash
dotnet build
```

Then:

```bash
dotnet ef database update --project src/Akkhor.Infrastructure --startup-project src/Akkhor.API
```

---

## Frontend Dependency Error

Navigate to:

```bash
cd Akkhor_Frontend/frontend
```

Run:

```bash
npm install
```

Then:

```bash
ng serve
```

---

## API Connection Error

Check the Angular environment configuration.

Example:

```typescript
apiUrl: 'https://localhost:50268/api'
```

Make sure the backend is running on the configured port.

---

## JWT Authorization Error

Check that:

* The user is logged in.
* The JWT token is valid.
* The token has not expired.
* The frontend sends the JWT token with API requests.
* The user has the required role.
* JWT configuration is consistent between token generation and validation.

---

# Future Improvements

Potential future improvements include:

1. Docker Compose configuration.
2. CI/CD pipeline.
3. Cloud deployment.
4. Automated database seeding.
5. File attachments.
6. Email notifications.
7. Push notifications.
8. Advanced assignment filtering.
9. Pagination.
10. Server-side searching.
11. Assignment analytics.
12. Student performance reports.
13. Teacher performance reports.
14. Automated code coverage reporting.
15. Integration testing.
16. End-to-end testing.
17. Comprehensive audit logging.

---



---

# Project Submission

This project was developed for:

**Assistant Software Engineer Recruitment Project**

**Assignment & Submission Management System**

Organization:

**OnnoRokom Projukti Limited**

Submission Deadline:

**14 August 2026**

Submission Link:

https://q-rp.com/c/4CIs

---

# Author

**MD. SEFAT AHMED**

Akkhor - Assignment & Submission Management System

---

# License

This project was developed as a recruitment assignment and is intended for evaluation purposes.
