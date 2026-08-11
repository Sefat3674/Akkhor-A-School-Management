# Akkhor - Assignment & Submission Management System

Akkhor is a role-based Assignment & Submission Management System developed for a school/college environment.

The system allows administrators to manage users, academic structures, courses, subjects, and teacher assignments. Teachers can create and manage assignments, publish assignments or save them as drafts, view student submissions, provide marks and feedback, and manage submission status. Students can view assignments assigned to their class/course and submit or update their answers according to the applicable submission rules.

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

PostgreSQL is used as the database, with Entity Framework Core handling database access and migrations.

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

* View assigned classes/courses/subjects
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

The backend follows a layered architecture.


                    ┌──────────────────────┐
                    │     Angular UI       │
                    └──────────┬───────────┘
                               │
                               │ HTTP / REST API
                               ▼
                    ┌──────────────────────┐
                    │      Akkhor.API      │
                    │ Controllers
                    └──────────┬───────────┘
                               │
                               │ 
                               ▼
                    ┌──────────────────────┐
                    │     Angular UI       │
                    │   TypeScript / HTML  │
                    │        / SCSS        │
                    └──────────┬───────────┘
                               │
                               │ HTTP / REST API
                               ▼
                    ┌──────────────────────┐
                    │      Akkhor.API      │
                    │                      │
                    │ Controllers          │
                    │ JWT Authentication   │
                    │ Authorization        │
                    │ Middleware           │
                    │ Swagger / OpenAPI    │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │  Akkhor.Application  │
                    │                      │
                    │ Services             │
                    │ DTOs                 │
                    │ Interfaces           │
                    │ Business Logic       │
                    │ Validation            │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │    Akkhor.Domain     │
                    │                      │
                    │ Entities             │
                    │ Domain Models        │
                    │ Business Concepts    │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │ Akkhor.Infrastructure│
                    │                      │
                    │ Repositories         │
                    │ Entity Framework Core│
                    │ ApplicationDbContext │
                    │ Migrations           │
                    │ Identity Persistence │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │     PostgreSQL       │
                    │       Database       │
                    └──────────────────────┘


# Project Structure

The project is organized into separate frontend, backend, domain, application, infrastructure, and testing layers.

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

