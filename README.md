# Backend - UsageDashboard.Api

API em C# responsável por alimentar o dashboard de uso da plataforma.

## Tecnologias

- C#
- ASP.NET Core Minimal API
- .NET 10
- Dados mockados em memória

## Pré-requisitos

- .NET SDK instalado

Para verificar a instalação:

```bash
dotnet --version
```

## Como rodar localmente

Entre na pasta da API:

```bash
cd backend/UsageDashboard.Api
```

Restaure as dependências:

```bash
dotnet restore
```

Suba a API na porta esperada pelo frontend:

```bash
dotnet run --urls http://localhost:5078
```

A API ficará disponível em:

```text
http://localhost:5078
```

## Endpoints disponíveis

Buscar opções dos filtros:

```text
GET http://localhost:5078/api/dashboard/filters
```

Buscar dados do dashboard:

```text
GET http://localhost:5078/api/dashboard?clientId=client-acme&companyId=company-north&period=month&userId=all
```

## Estrutura principal

- `Contracts`: DTOs de entrada e saída da API
- `Domain`: modelos de negócio
- `Repositories`: abstração e implementação mockada dos dados
- `Services`: regras de aplicação
- `Endpoints`: rotas HTTP da API
- `Extensions`: configuração de dependências e CORS

## Observação sobre banco de dados

Atualmente a API usa `MockUsageRepository` com dados em memória.
