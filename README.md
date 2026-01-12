# 🏢 Enterprise Clean Architecture & CQRS Template (.NET 9)

[![.NET 9](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Supported-blue.svg)](https://www.docker.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-green.svg)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![Testing](https://img.shields.io/badge/Testing-xUnit%20%7C%20Moq-blueviolet.svg)](#-testing)

A **production-grade, enterprise-ready backend template** built with  
**Clean Architecture + CQRS**, designed for **scalability, security, testability, and observability** from day one.

This repository represents a **real-world backend foundation**, not a tutorial or demo project.

---

## 🎯 Why This Template?

Most boilerplates focus on *features*.  
This template focuses on **architecture correctness, maintainability, and operational readiness**.

✔ Built for real production workloads  
✔ Clear separation of concerns and strict boundaries  
✔ Testable by design (Application layer fully isolated)  
✔ Ready for scaling teams and complex domains  

---

## 🏗️ Architecture Principles

### Clean Architecture
- Domain-driven design
- Dependency inversion
- Infrastructure pushed to the edge
- Application layer free from frameworks

### CQRS with MediatR
- Clear separation of Read & Write operations
- Use-case oriented handlers
- No business logic in controllers

---

## 🔄 Request Flow (High-Level)

```text
HTTP Request
     ↓
Rate Limiting Middleware
     ↓
Global Exception Middleware
     ↓
Audit Logging Middleware
     ↓
Controller (Thin)
     ↓
MediatR Pipeline
     ↓ ─── Validation Behavior
     ↓ ─── Logging / Performance Behavior
     ↓
Command / Query Handler
     ↓
Database / External Services
```
🚀 Key Features

🏗️ Architecture & Patterns
Clean Architecture (Domain / Application / Infrastructure / API)

CQRS using MediatR

Pipeline Behaviors:

Validation

Audit Logging

Performance Profiling

🛡️ Security & Identity
JWT Authentication

Refresh Token Rotation

ASP.NET Core Identity

Email Verification & Password Reset flows

Built-in Rate Limiting for sensitive endpoints

📊 Observability & Reliability
Serilog + Seq

Structured logging

Distributed-friendly logs

Audit Logging (Database)

Tracks user actions (endpoint, status, duration, user)

Global Exception Handling middleware

🧪 Testing
This project is test-first friendly and designed for easy unit testing.

xUnit for testing framework

Moq for mocking dependencies

FluentAssertions for expressive assertions

EF Core InMemory for isolated database tests

AutoMapper validation in tests

What’s Covered:
Command & Query Handlers

Validation & error scenarios

Email service behavior

Database edge cases

Token & identity flows

Tests live in a dedicated ZHS.Test project and validate business behavior, not infrastructure details.

✉️ Integrations & DevOps
MailKit SMTP integration

Docker & Docker Compose

Swagger / OpenAPI

Environment-based configuration

🛠️ Tech Stack
Category	Technology
Framework	.NET 9
Database	SQL Server
ORM	EF Core
Messaging	MediatR
Logging	Serilog + Seq
Audit Logs	Database
Security	JWT + Refresh Tokens
Validation	FluentValidation
Testing	xUnit, Moq, FluentAssertions
Email	MailKit
Containers	Docker

🚦 Getting Started
Prerequisites
Docker Desktop

Run the Project
```bash
Copy code
git clone https://github.com/ziadhelal1/CleanArch.CQRS.Template.git
cd CleanArch.CQRS.Template
docker-compose up -d --build
```
```text
Service Endpoints
Swagger	http://localhost:8080/swagger
Service	URL
Seq	http://localhost:5341
```

🏛️ Project Structure
```text

src/
 ├── Domain          → Core business entities & rules
 ├── Application     → CQRS, DTOs, Validators, Interfaces
 ├── Infrastructure  → EF Core, Identity, Mail, Logging
 └── API             → Controllers, Middlewares, DI
 tests
      ├── Common          → Test helpers, factories & shared setup
      └── Features        → Use-case driven unit tests (CQRS handlers)
```
📈 Roadmap
🔲 Testing coverage will grow with the system’s business rules.

🔲 Integration Testing (WebApplicationFactory)

🔲 Redis Caching

🔲 Background Jobs

🔲 Outbox Pattern

🔲 Multi-tenancy support

🧠 Best Practices Applied
Thin Controllers

Fail-fast validation

Centralized exception handling

No infrastructure leakage into business logic

Explicit use-cases instead of generic services

Testability as a first-class concern

👨‍💻 Author
Ziad Helal
Focused on building scalable, maintainable, and enterprise-grade backend systems.

