# AcessaAi — Backend

Backend da plataforma **AcessaAi**, uma solução colaborativa para avaliação e documentação de acessibilidade em estabelecimentos. Permite que usuários encontrem, avaliem e compartilhem informações sobre recursos de acessibilidade disponíveis em locais públicos e privados.

---

## Índice

- [Funcionalidades](#funcionalidades)
- [Arquitetura](#arquitetura)
- [Modelo de Dados](#modelo-de-dados)
- [Stack Tecnológica](#stack-tecnológica)
- [Configuração e Execução](#configuração-e-execução)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Endpoints da API](#endpoints-da-api)
- [Testes](#testes)
- [Contribuindo](#contribuindo)

---

## Funcionalidades

### Autenticação
- Login com retorno de **JWT Bearer Token**
- Gerenciamento de identidade via **ASP.NET Core Identity**
- Suporte a **Refresh Token**

### Usuários
- Cadastro com nome, e-mail, senha, data de nascimento e endereço
- Consulta de perfil com URL pré-assinada da foto (válida por 60 min)
- Upload de foto de perfil para **Cloudflare R2**

### Estabelecimentos
- CRUD completo de estabelecimentos
- Filtro avançado por nome, tipo, distância máxima (km), coordenadas geográficas e recursos de acessibilidade
- Upload de múltiplas imagens com distinção de **foto de capa** e galeria

### Avaliações
- Criação, edição e exclusão (soft delete) de avaliações
- Nota de 1 a 5 estrelas e comentário de até 500 caracteres
- Atualização automática da **média de estrelas** do estabelecimento

### Recursos de Acessibilidade
- Listagem de recursos cadastrados e ativos (rampas, elevadores, banheiros acessíveis, etc.)
- Associação de recursos a estabelecimentos

---

## Arquitetura

O projeto segue **Clean Architecture** com **Domain-Driven Design (DDD)**, organizado em quatro camadas:

```
src/
├── AcessaAi.API/           → Controllers, Middlewares, OpenAPI
├── AcessaAi.Application/   → Serviços de aplicação, DTOs, Mapeamentos
├── AcessaAi.Domain/        → Entidades, Value Objects, Interfaces, Erros de domínio
└── AcessaAi.Infrastructure/→ Repositórios, DbContext, JWT, Storage (R2)
```

**Padrões aplicados:**
- **Repository Pattern** com interface genérica `IRepository<TEntity>`
- **Unit of Work** para transações
- **ErrorOr** para tratamento de erros sem exceções na camada de domínio
- **Factory Methods** nas entidades para garantir invariantes de criação
- **Soft Delete** em avaliações e estabelecimentos (campo `Ativo`)
- **Value Objects**: `Geocordenadas` (record) e `Endereco`

---

## Modelo de Dados

```
┌──────────────┐       ┌─────────────────────┐       ┌──────────────────────────┐
│   Usuario    │       │   Estabelecimento    │       │  RecursoAcessibilidade   │
│──────────────│       │─────────────────────│       │──────────────────────────│
│ Id           │       │ Id                  │       │ Id                       │
│ NomeCompleto │       │ Nome                │       │ Nome                     │
│ Email        │       │ Tipo (enum)         │       │ Descricao                │
│ UrlFotoPerfil│       │ Geolocalizacao ─────┼──┐    │ Icone                    │
│ DataNascimento│      │ Endereco ───────────┼──┤    │ Ativo                    │
│ Endereco ────┼──┐   │ MediaEstrelas       │  │    └──────────────────────────┘
│ RefreshToken │  │   │ CadastradoRecente   │  │            ▲   M:N
│ Ativo        │  │   │ Ativo               │  │            │
└──────────────┘  │   └─────────────────────┘  │   ┌────────────────────────┐
       │          │            │  1:N           │   │  EstabelecimentoFoto   │
       │ 1:N      │            ▼                │   │────────────────────────│
       ▼          │   ┌─────────────────────┐   │   │ Id                     │
┌────────────────┐│   │     Avaliacao       │   │   │ Url                    │
│   Endereco     ││   │─────────────────────│   │   │ IsCapa                 │
│(Value Object)  ││   │ Id                  │   │   │ EstabelecimentoId      │
│────────────────││   │ Comentario          │   │   └────────────────────────┘
│ Logradouro     ││   │ QuantidadeEstrelas  │   │
│ UF             ││   │ UsuarioId           │   │   ┌──────────────────────────┐
│ Cidade         ││   │ EstabelecimentoId   │   │   │      Geocordenadas       │
│ Numero         ││   │ Ativo               │   │   │   (Value Object/Record)  │
│ CEP            ││   └─────────────────────┘   │   │──────────────────────────│
│ Bairro         ││                             └──►│ Latitude                 │
│ Complemento    ││                                 │ Longitude                │
└────────────────┘│                                 └──────────────────────────┘
                  └──── (mesmo tipo, usado em Usuario e Estabelecimento)
```

### Enum `TipoEstabelecimento`

| Valor | Nome |
|---|---|
| 1 | Restaurante |
| 2 | Farmacia |
| 3 | Saude |
| 4 | Banco |
| 5 | Shopping |
| 6 | Transporte |

---

## Stack Tecnológica

| Camada | Tecnologia |
|---|---|
| Framework | .NET 10 / ASP.NET Core 10 |
| Banco de Dados | PostgreSQL |
| ORM | Entity Framework Core 10 + Npgsql |
| Autenticação | JWT Bearer + ASP.NET Core Identity |
| Mapeamento | Mapster 4.1 |
| Storage | Cloudflare R2 (SDK S3-compatível via AWSSDK.S3) |
| Tratamento de Erros | ErrorOr 2.0 |
| Documentação | OpenAPI / Swagger UI |
| Testes | xUnit + FluentAssertions |

---

## Configuração e Execução

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local ou Docker)
- Conta Cloudflare R2 (ou outro storage S3-compatível)

### Passo a passo

**1. Clone o repositório**

```bash
git clone https://github.com/luisfernandomp/AcessaAi.git
cd AcessaAi/backend
```

**2. Configure as variáveis de ambiente**

Crie o arquivo `src/AcessaAi.API/appsettings.Development.json` com base na seção [Variáveis de Ambiente](#variáveis-de-ambiente) abaixo.

**3. Restaure as dependências**

```bash
dotnet restore
```

**4. Aplique as migrations**

```bash
dotnet ef database update --project src/AcessaAi.Infrastructure --startup-project src/AcessaAi.API
```

**5. Inicie a API**

```bash
dotnet run --project src/AcessaAi.API
```

A API estará disponível em `https://localhost:5001`.  
Documentação Swagger: `https://localhost:5001/swagger`

---

### Subindo com Docker (PostgreSQL)

Para subir apenas o banco rapidamente:

```bash
docker run -d \
  --name acessaai-db \
  -e POSTGRES_DB=AcessaAiDb \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=sua_senha \
  -p 5432:5432 \
  postgres:16
```

---

## Variáveis de Ambiente

Configure o arquivo `appsettings.Development.json` (nunca versione credenciais reais):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AcessaAiDb;Username=postgres;Password=sua_senha"
  },
  "JwtSettings": {
    "SecretKey": "chave-secreta-minimo-32-caracteres",
    "Issuer": "AcessaAi",
    "Audience": "AcessaAi.Client",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "AWS": {
    "Region": "auto",
    "ServiceURL": "https://<account-id>.r2.cloudflarestorage.com",
    "ForcePathStyle": true,
    "AccessKey": "sua-r2-access-key",
    "SecretKey": "sua-r2-secret-key"
  },
  "S3Settings": {
    "BucketName": "nome-do-bucket",
    "PublicBaseUrl": "https://<subdomain>.r2.dev"
  },
  "AllowedOrigins": ["http://localhost:4200"]
}
```

> O Cloudflare R2 expõe uma API S3-compatível. Por isso, a configuração usa as chaves `AWS` com `ServiceURL` apontando para o endpoint R2.

### Descrição das variáveis

| Chave | Descrição |
|---|---|
| `ConnectionStrings:DefaultConnection` | String de conexão PostgreSQL |
| `JwtSettings:SecretKey` | Chave secreta HMAC para assinar tokens (mín. 32 chars) |
| `JwtSettings:Issuer` | Identificador do emissor do JWT |
| `JwtSettings:Audience` | Público-alvo do JWT |
| `JwtSettings:AccessTokenExpirationMinutes` | Validade do access token em minutos |
| `JwtSettings:RefreshTokenExpirationDays` | Validade do refresh token em dias |
| `AWS:Region` | Região (`auto` para Cloudflare R2) |
| `AWS:ServiceURL` | Endpoint S3-compatível do Cloudflare R2 |
| `AWS:ForcePathStyle` | `true` para usar path-style (obrigatório no R2) |
| `AWS:AccessKey` | Access Key do bucket R2 |
| `AWS:SecretKey` | Secret Key do bucket R2 |
| `S3Settings:BucketName` | Nome do bucket no R2 |
| `S3Settings:PublicBaseUrl` | URL pública do domínio R2 para servir arquivos |
| `AllowedOrigins` | Origens permitidas pelo CORS |

---

## Endpoints da API

### Autenticação

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/auth/login` | Não | Realiza login e retorna JWT |

**Body `POST /api/auth/login`:**
```json
{
  "email": "usuario@email.com",
  "senha": "senha123"
}
```

**Response `200 OK`:**
```json
{
  "idUsuario": 1,
  "nomeUsuario": "João Silva",
  "token": "eyJhbGciOiJI...",
  "expiraEm": 15
}
```

---

### Usuários

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/usuario/cadastrar` | Não | Cadastra novo usuário |
| `GET` | `/api/usuario/{id}` | Sim | Retorna perfil do usuário |
| `POST` | `/api/usuario/{id}/foto-perfil` | Sim | Upload de foto de perfil (`multipart/form-data`) |

**Body `POST /api/usuario/cadastrar`:**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "Senha@123",
  "dataNascimento": "1990-05-15",
  "endereco": {
    "logradouro": "Av. Paulista",
    "uf": "SP",
    "cidade": "São Paulo",
    "numero": "1000",
    "cep": "01310-100",
    "bairro": "Bela Vista",
    "complemento": "Apto 42"
  }
}
```

**Response `GET /api/usuario/{id}` — `200 OK`:**
```json
{
  "nome": "João Silva",
  "dataNascimento": "1990-05-15",
  "ativo": true,
  "urlFotoPerfil": "https://pub-xxx.r2.dev/profiles/foto.jpg",
  "endereco": {
    "logradouro": "Av. Paulista",
    "uf": "SP",
    "cidade": "São Paulo",
    "numero": "1000",
    "cep": "01310-100",
    "bairro": "Bela Vista",
    "complemento": "Apto 42"
  }
}
```

---

### Estabelecimentos

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/estabelecimento` | Sim | Cria estabelecimento com fotos (`multipart/form-data`) |
| `GET` | `/api/estabelecimento` | Não | Filtra estabelecimentos |
| `GET` | `/api/estabelecimento/{id}` | Não | Busca estabelecimento por ID |
| `PATCH` | `/api/estabelecimento/{id}` | Sim | Atualiza dados do estabelecimento |
| `DELETE` | `/api/estabelecimento/{id}` | Sim | Remove o estabelecimento |
| `POST` | `/api/estabelecimento/{id}/imagem` | Sim | Upload de imagem para estabelecimento existente |

**Form fields `POST /api/estabelecimento`** (`multipart/form-data`):

| Campo | Tipo | Descrição |
|---|---|---|
| `Nome` | string | Nome do estabelecimento |
| `Tipo` | int | Valor do enum `TipoEstabelecimento` |
| `Latitude` | double | Latitude |
| `Longitude` | double | Longitude |
| `Logradouro` | string | Rua/avenida |
| `UF` | string | Estado (ex: SP) |
| `Cidade` | string | Cidade |
| `Numero` | string | Número |
| `CEP` | string | CEP |
| `Bairro` | string | Bairro |
| `Complemento` | string? | Complemento (opcional) |
| `Capa` | file | Foto de capa (opcional) |
| `Fotos` | file[] | Fotos adicionais (opcional) |

**Query params `GET /api/estabelecimento`:**

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `Nome` | string? | Filtro por nome |
| `Tipo` | int? | Filtro por tipo de estabelecimento |
| `DistanciaMaxima` | double? | Distância máxima em km |
| `Latitude` | double? | Latitude para cálculo de distância |
| `Longitude` | double? | Longitude para cálculo de distância |
| `RecursosAcessabilidadeIds` | int[]? | IDs dos recursos de acessibilidade |

**Body `PATCH /api/estabelecimento/{id}`:**
```json
{
  "nome": "Novo Nome",
  "tipo": 1,
  "geocordenadas": {
    "latitude": -23.5505,
    "longitude": -46.6333
  }
}
```

**Query param `POST /api/estabelecimento/{id}/imagem`** (`multipart/form-data`):

| Parâmetro | Tipo | Descrição |
|---|---|---|
| `isCapa` | bool | `true` para definir como capa |
| `file` | file | Arquivo de imagem |

---

### Avaliações

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/avaliacao` | Sim | Cria nova avaliação |
| `GET` | `/api/avaliacao/{id}` | Sim | Busca avaliação por ID |
| `PATCH` | `/api/avaliacao/{id}` | Sim | Atualiza avaliação existente |
| `DELETE` | `/api/avaliacao/{id}` | Sim | Exclui avaliação (soft delete) |

**Body `POST /api/avaliacao`:**
```json
{
  "comentario": "Ótimo atendimento e estrutura acessível!",
  "estrelas": 5,
  "usuarioId": 1,
  "estabelecimentoId": 42
}
```

**Body `PATCH /api/avaliacao/{id}`:**
```json
{
  "id": 10,
  "comentario": "Revisando minha avaliação anterior.",
  "estrelas": 4
}
```

**Response `200 OK`:**
```json
{
  "id": 10,
  "comentario": "Revisando minha avaliação anterior.",
  "estrelas": 4,
  "usuarioId": 1,
  "ativo": true
}
```

---

### Recursos de Acessibilidade

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `GET` | `/api/recurso-acessibilidade/listar-ativas` | Não | Lista todos os recursos ativos |

**Response `200 OK`:**
```json
[
  {
    "id": 1,
    "nome": "Rampa de Acesso",
    "descricao": "Rampa para cadeirantes na entrada principal",
    "icone": "ramp"
  },
  {
    "id": 2,
    "nome": "Banheiro Adaptado",
    "descricao": "Banheiro com barras de apoio e espaço para cadeira de rodas",
    "icone": "accessible-toilet"
  }
]
```

---

### Códigos de resposta comuns

| Código | Significado |
|---|---|
| `200 OK` | Sucesso |
| `201 Created` | Recurso criado |
| `204 No Content` | Operação concluída sem retorno |
| `400 Bad Request` | Dados de entrada inválidos |
| `401 Unauthorized` | Token ausente ou inválido |
| `404 Not Found` | Recurso não encontrado |
| `422 Unprocessable Entity` | Erros de validação de domínio |

---

## Testes

O projeto possui testes unitários cobrindo as entidades de domínio (30 testes).

```bash
# Rodar todos os testes unitários
dotnet test tests/AcessaAi.UnitTests

# Rodar com relatório detalhado
dotnet test tests/AcessaAi.UnitTests --logger "console;verbosity=detailed"
```

### Cobertura atual

| Entidade | Cenários testados |
|---|---|
| `Avaliacao` | Criação válida, estrelas inválidas (0 e 6), comentário vazio, comentário > 500 chars, usuário nulo, estabelecimento nulo, múltiplos erros, alteração, exclusão |
| `Estabelecimento` | Criação válida, nome vazio, geo nula, múltiplos erros, alteração (com/sem tipo), desativação, adição de avaliação (simples e múltiplas com cálculo de média), imagens (capa, não-capa, flag explícita) |
| `Usuario` | Criação, data de cadastro automática, adicionar endereço, substituir endereço, atualizar foto, substituir foto |

---

## Contribuindo

### Fluxo de trabalho

1. Faça um fork do repositório
2. Crie uma branch a partir de `main`:
   ```bash
   git checkout -b feature/minha-feature
   ```
3. Implemente as mudanças seguindo os padrões do projeto
4. Adicione testes unitários para a lógica de domínio
5. Certifique-se de que todos os testes passam:
   ```bash
   dotnet test
   ```
6. Abra um Pull Request descrevendo o que foi feito

### Adicionando uma nova Migration

```bash
dotnet ef migrations add NomeDaMigration \
  --project src/AcessaAi.Infrastructure \
  --startup-project src/AcessaAi.API

dotnet ef database update \
  --project src/AcessaAi.Infrastructure \
  --startup-project src/AcessaAi.API
```

### Estrutura de diretórios detalhada

```
src/
├── AcessaAi.API/
│   ├── Controllers/            # Um controller por agregado
│   ├── Middlewares/            # Tratamento global de exceções
│   ├── Extensions/             # Extensões de configuração (DI, Auth, CORS)
│   └── Requests/               # Modelos de form multipart
├── AcessaAi.Application/
│   ├── Autenticacao/
│   │   ├── Dtos/               # LoginRequest, LoginResponse
│   │   ├── Interfaces/         # IAutenticacaoApplicationService, ITokenService
│   │   └── Services/           # AutenticacaoApplicationService
│   ├── Avaliacoes/
│   │   ├── Dtos/               # AvaliacaoCreateRequest, AvaliacaoResponse...
│   │   ├── Interfaces/
│   │   └── Services/
│   ├── Estabelecimentos/
│   ├── RecursosAcessibilidades/
│   ├── Usuarios/
│   └── Storage/Interfaces/     # IImageStorageService
├── AcessaAi.Domain/
│   ├── Common/                 # EntityBase, IRepository<T>, IUnitOfWork, Endereco
│   ├── Avaliacoes/             # Avaliacao, AvaliacaoErrors, IAvaliacaoRepository
│   ├── Estabelecimentos/       # Estabelecimento, EstabelecimentoErros, Geocordenadas...
│   ├── RecursosAcessibilidades/
│   └── Usuarios/               # Usuario, IUsuarioRepository
└── AcessaAi.Infrastructure/
    ├── Data/                   # AppDbContext, configurações EF
    ├── Repositories/           # Implementações dos repositórios
    ├── Migrations/             # EF Core migrations
    ├── Authentication/         # TokenService (JWT)
    └── Storage/                # CloudflareR2StorageService

tests/
├── AcessaAi.UnitTests/
│   └── Domain/                 # AvaliacaoTests, EstabelecimentoTests, UsuarioTests
└── AcessaAi.IntegrationTests/
```

### Convenções do projeto

- **Erros de domínio** são retornados via `ErrorOr` — não use exceções para fluxo de negócio
- **Soft delete** via campo `Ativo = false`, nunca delete físico de avaliações ou estabelecimentos
- **Factory Methods** nas entidades: use sempre `Entidade.Criar(...)` para garantir invariantes
- **Mapeamentos** centralizados em `Application/*/Mappings/` via Mapster
- **Injeção de dependência** registrada em `Extensions/` na camada de API

---

## Licença

Este projeto está sob a licença MIT.
