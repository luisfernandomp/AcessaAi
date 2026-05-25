# AcessaAi - Backend

Backend da plataforma **AcessaAi**, uma solução completa para avaliação e documentação de acessibilidade em estabelecimentos. Permite que usuários encontrem, avaliem e compartilhem informações sobre recursos de acessibilidade disponíveis em locais públicos e privados.

## 🎯 Objetivo

Conectar pessoas com deficiência ou mobilidade reduzida com informações reais e atualizadas sobre acessibilidade em estabelecimentos, promovendo inclusão através de dados colaborativos.

## 🏗️ Arquitetura

Projeto estruturado em camadas seguindo **Clean Architecture** e **Domain-Driven Design (DDD)**:

- **AcessaAi.API** - Camada de apresentação (Controllers, Middlewares, OpenAPI)
- **AcessaAi.Application** - Camada de aplicação (Serviços, DTOs, Mapeamentos)
- **AcessaAi.Domain** - Camada de domínio (Entidades, Interfaces)
- **AcessaAi.Infrastructure** - Camada de infraestrutura (Repositórios, Banco de dados, Autenticação, Storage)

## 🔑 Recursos Principais

### 🔐 Autenticação e Autorização
- Autenticação via **JWT Bearer Token**
- Gerenciamento de identidade com ASP.NET Core Identity
- Endpoints protegidos por autorização

### 👤 Gerenciamento de Usuários
- Cadastro de novos usuários
- Login com autenticação JWT
- Perfil de usuário

### 🏢 Estabelecimentos
- Criar, visualizar, atualizar e deletar estabelecimentos
- Filtro avançado por nome, localização (coordenadas/endereço) e recursos de acessibilidade
- Upload de imagens (armazenamento em AWS S3)
- Busca por distância máxima

### ⭐ Avaliações
- Criar avaliações para estabelecimentos
- Avaliar recursos de acessibilidade disponíveis
- Atualizar e deletar avaliações próprias

### ♿ Recursos de Acessibilidade
- Cadastro de recursos de acessibilidade (rampas, elevadores, banheiros acessíveis, etc.)
- Listagem de recursos ativos
- Associação com estabelecimentos

## 📋 Stack Tecnológico

- **Framework:** ASP.NET Core 8+
- **Banco de Dados:** SQL Server
- **ORM:** Entity Framework Core
- **Autenticação:** JWT + ASP.NET Core Identity
- **Mapeamento:** Mapster
- **Storage:** AWS S3
- **Documentação:** OpenAPI/Swagger
- **CORS:** Configuração via appsettings

## 🚀 Configuração e Execução

### Pré-requisitos
- .NET 8 SDK
- SQL Server
- Credenciais AWS (para S3)

### Instalação

1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/AcessaAi.git
cd AcessaAi/backend
```

2. Restaure as dependências
```bash
dotnet restore
```

3. Configure as variáveis de ambiente no `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=AcessaAi;Trusted_Connection=true;"
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta-super-segura",
    "ExpirationMinutes": 60
  },
  "AWS": {
    "AccessKey": "sua-chave-de-acesso",
    "SecretKey": "sua-chave-secreta",
    "BucketName": "seu-bucket",
    "Region": "us-east-1"
  },
  "AllowedOrigins": ["http://localhost:4200"]
}
```

4. Aplique as migrations do banco de dados
```bash
dotnet ef database update
```

5. Inicie a API
```bash
dotnet run --project src/AcessaAi.API
```

A API estará disponível em `https://localhost:5001` com documentação Swagger em `/swagger`.

## 📚 Documentação da API

Consulte [API.md](API.md) para detalhes completos de endpoints, parâmetros e exemplos de requisição.

**Endpoints principais:**
- `POST /api/auth/login` - Login
- `POST /api/usuario/cadastrar` - Cadastro
- `GET /api/estabelecimento` - Listar estabelecimentos
- `POST /api/avaliacao` - Criar avaliação
- `GET /api/recursoacessibilidade/listar-ativas` - Listar recursos ativos

## 🧪 Testes

### Testes Unitários
```bash
dotnet test tests/AcessaAi.UnitTests
```

### Testes de Integração
```bash
dotnet test tests/AcessaAi.IntegrationTests
```

## 📁 Estrutura de Diretórios

```
src/
├── AcessaAi.API/              # Camada de apresentação
│   ├── Controllers/           # Endpoints
│   ├── Middlewares/          # Tratamento de erros global
│   └── Extensions/           # Extensões de configuração
├── AcessaAi.Application/      # Lógica de aplicação
│   ├── Services/             # Serviços da aplicação
│   ├── Dtos/                 # Data Transfer Objects
│   └── Mappings/             # Configuração de mapeamento
├── AcessaAi.Domain/          # Entidades e interfaces
│   └── Entities/             # Modelos de domínio
└── AcessaAi.Infrastructure/  # Acesso a dados e serviços externos
    ├── Data/                 # DbContext
    ├── Repositories/         # Repositórios
    └── Migrations/           # EF Core migrations
tests/
├── AcessaAi.UnitTests/       # Testes unitários
└── AcessaAi.IntegrationTests/ # Testes de integração
```

## 🤝 Contribuindo

1. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
2. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
3. Push para a branch (`git push origin feature/AmazingFeature`)
4. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT.

## 📧 Contato

Para dúvidas ou sugestões, abra uma issue no repositório.
