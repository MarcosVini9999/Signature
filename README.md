# Subscription API

API RESTful em ASP.NET Core 8 para gestão de usuários, planos de assinatura e assinaturas, com PostgreSQL, FluentMigrator e RabbitMQ via MassTransit.

## 📦 Estrutura do Projeto

```
SubscriptionService.sln
├── src
│   ├── Signature.API
│   │   ├── Controllers
│   │   ├── Properties
│   │   ├── appsettings.json
│   │   ├── docker-compose.yml
│   │   ├── Dockerfile
│   │   └── Program.cs
│   ├── Signature.API.Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   └── Services
│   ├── Signature.API.Domain
│   │   ├── Common
│   │   ├── Entities
│   │   ├── Events
│   │   └── Interfaces
│   ├── Signature.API.Infra.Data
│   │   ├── Context
│   │   ├── Migrations
│   │   │   └── InitialMigration.cs
│   │   └── Repositories
│   └── Signature.API.Infra.Ioc
│       └── DependencyInjection.cs
```

## 🛠️ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started) e [Docker Compose](https://docs.docker.com/compose/)

## ⚙️ Configuração

1. **appsettings.json** (em `Signature.API`) já configurado para Docker:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=db;Port=5432;Database=subscriptions;Username=postgres;Password=senha123"
     },
     "RabbitMq": {
       "Host": "rabbitmq://rabbitmq",
       "Username": "guest",
       "Password": "guest"
     }
   }
   ```
2. **docker-compose.yml** (em `Signature.API`) define os serviços:
   - **db**: PostgreSQL 15-alpine
   - **rabbitmq**: RabbitMQ 3.11 + UI de management
   - **api**: build da API `.NET 8`

## 🚀 Inicializando com Docker Compose

No diretório `Signature.API`, execute:

```bash
docker-compose up --build
```

- A API ficará exposta em `http://localhost:8080`
- Swagger UI: `http://localhost:8080/swagger`
- RabbitMQ Management: `http://localhost:15672` (guest/guest)

## ✨ Migrations Automáticas

No startup (`Program.cs`), o FluentMigrator é executado:

```csharp
var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();
```

As tabelas **Users**, **SubscriptionPlans** e **UserSubscriptions** serão criadas automaticamente.

## 📑 Endpoints Principais

- **Users**
  - `POST /api/users` – criar usuário (`CreateUserDto`).
  - `GET /api/users/{id}` – obter usuário.
- **SubscriptionPlans**
  - `POST /api/subscriptionplans` – criar plano (`CreateSubscriptionPlanDto`).
  - `GET /api/subscriptionplans` – listar planos.
  - `GET /api/subscriptionplans/{id}` – obter plano.
- **Subscriptions**
  - `POST /api/subscriptions/subscribe?userId={userId}&planId={planId}` – criar assinatura e publicar evento.
  - `GET /api/subscriptions/user/{userId}` – listar assinaturas do usuário.

## 📝 Observações

- A publicação de eventos (`UserSubscribed`) é feita via **MassTransit** para RabbitMQ.
- Consumers podem ser adicionados registrando `AddConsumer<MeuConsumer>()` em `DependencyInjection`.

---

Feito com ❤️ para gerenciar assinaturas de forma escalável e desacoplada.
