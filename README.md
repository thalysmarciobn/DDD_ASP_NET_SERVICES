# DDD ASP.NET Core Services

Projeto de microsserviços usando Domain-Driven Design (DDD) com ASP.NET Core, CQRS personalizado, PostgreSQL e YARP Gateway.

## Estrutura

```
DDD_ASP_NET_SERVICES/
├── gateway/Gateway/                 # Gateway YARP (porta 7000)
├── services/auth/src/
│   ├── Auth.Domain/                 # Entidades e interfaces
│   ├── Auth.Application/            # CQRS personalizado
│   ├── Auth.Infrastructure/         # EF Core e repositórios
│   └── Auth.API/                    # API REST (porta 7001)
└── DDD_ASP_NET_SERVICES.sln
```

## Tecnologias

- ASP.NET Core 9.0
- Entity Framework Core + PostgreSQL
- CQRS personalizado (sem MediatR)
- YARP Gateway
- JWT Authentication
- DDD Architecture

## Configuração

### Banco de Dados
```bash
# PostgreSQL configurado para 192.168.10.180:5433
# Banco: auth_db | Usuário: postgres | Senha: postgres

# Usando Docker
docker-compose up -d

# Ou configure manualmente em services/auth/src/Auth.API/appsettings.json
```

### Execução
```bash
# 1. Migrações
cd services/auth/src/Auth.Infrastructure
dotnet ef database update --startup-project ..\Auth.API

# 2. Gateway
cd gateway/Gateway
dotnet run

# 3. Auth Service
cd services/auth/src/Auth.API
dotnet run
```

## Endpoints

- `POST /api/auth/register` - Registro de usuário
- `POST /api/auth/login` - Login
- `GET /api/auth/me` - Dados do usuário (protegido)

## Autenticação

- JWT com claims: `name`, `email`, `nameidentifier`
- Token expira em 24 horas
- Header: `Authorization: Bearer {token}`

## Documentação

- **Swagger**: `https://localhost:7001/swagger`
- **[Integração Frontend](FRONTEND_INTEGRATION.md)** - Guia completo para frontend

## Status

✅ **Pronto para produção**
- Arquitetura DDD completa
- CQRS personalizado funcionando
- Autenticação JWT implementada
- Sistema de códigos de erro padronizado
- Código limpo sem logs de debug