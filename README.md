# Backend - UsageDashboard.Api

API em C# responsavel por alimentar o dashboard de uso da plataforma e importar arquivos TXT para a tabela `controletributos`.

## Tecnologias

- C#
- ASP.NET Core Minimal API
- .NET 10
- Swagger / Swashbuckle
- MySQL
- MySqlConnector

## Pre-requisitos

- .NET SDK instalado
- MySQL rodando localmente

Para verificar o .NET:

```bash
dotnet --version
```

## Configuracao do MySQL

A connection string padrao fica em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=usage_dashboard;User ID=root;Password=;"
  }
}
```

Se o seu MySQL tiver senha, altere o campo `Password`.

Exemplo:

```json
"DefaultConnection": "Server=localhost;Port=3306;Database=usage_dashboard;User ID=root;Password=sua_senha;"
```

## Criar banco e tabela

A API possui uma rota que cria/verifica o banco `usage_dashboard` e a tabela `controletributos`:

```text
POST http://localhost:5078/api/controle-tributos/setup-database
```

Tambem existe um script SQL manual em:

```text
Database/create_usage_dashboard.sql
```

## Como rodar localmente

Entre na pasta da API:

```bash
cd backend/UsageDashboard.Api
```

Restaure as dependencias:

```bash
dotnet restore
```

Suba a API:

```bash
dotnet run --urls http://localhost:5078
```

A API ficara disponivel em:

```text
http://localhost:5078
```

## Swagger

Com a API rodando em ambiente de desenvolvimento, acesse:

```text
http://localhost:5078/swagger
```

No Visual Studio, o perfil do projeto esta configurado para abrir direto nessa tela.

## Endpoints principais

Buscar opcoes dos filtros:

```text
GET http://localhost:5078/api/dashboard/filters
```

Buscar dados do dashboard:

```text
GET http://localhost:5078/api/dashboard?clientId=client-acme&companyId=company-north&period=month&userId=all
```

Criar/verificar banco e tabela MySQL:

```text
POST http://localhost:5078/api/controle-tributos/setup-database
```

Importar TXT separado por pipe:

```text
POST http://localhost:5078/api/controle-tributos/upload-txt
```

No Swagger, use `Try it out`, selecione um arquivo `.txt` e execute.

## Formato esperado do TXT

Cada linha representa um item, e cada coluna deve estar separada por `|`.

Exemplo:

```text
|0000|LECD|01012019|31122019|Empresa TESTE DRIVE Teste Ltda|88888888000198|SP||3550308|49656538||0|1|0||0|0||N|N|0|0|2|
|0001|0|
|0007|00||
|0990|4|
```

Na importacao atual:

- `codigocontabil` recebe o primeiro campo da linha, por exemplo `0000`
- `DescricaoCodigoContabil` recebe o segundo campo, por exemplo `LECD`
- `Historico` e `HistoricoOriginal` recebem a linha original completa
- `DataCadastro` recebe a data/hora da importacao
- `ChaveVinculo` recebe `TXT-{numeroDaLinha}`
- `Regra` recebe `IMPORTACAO_TXT`
- `TipoRegra` recebe `Arquivo separado por pipe`
- `pontos` recebe a quantidade de colunas lidas

Resposta da importacao:

```json
{
  "fileName": "arquivo.txt",
  "totalLinesRead": 4,
  "parsedRecords": 4,
  "insertedRecords": 4,
  "totalElapsedMilliseconds": 120,
  "databaseElapsedMilliseconds": 85,
  "preview": []
}
```

- `totalElapsedMilliseconds`: tempo total do processo de importacao.
- `databaseElapsedMilliseconds`: tempo usado em operacoes de banco, incluindo verificacao/criacao da tabela e inserts.

## Estrutura principal

- `Contracts`: DTOs de entrada e saida da API
- `Domain`: modelos de negocio, incluindo `ControleTributo`
- `Infrastructure/Database`: criacao do banco/tabela MySQL
- `Repositories`: acesso aos dados
- `Services`: regras de aplicacao e importacao
- `Endpoints`: rotas HTTP da API
- `Extensions`: configuracao de dependencias, CORS e Swagger
