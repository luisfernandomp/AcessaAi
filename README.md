# AcessaAí

Plataforma colaborativa de mapeamento urbano de acessibilidade. Permite que usuários reportem e consultem pontos de acessibilidade em espaços públicos, contribuindo para a mobilidade e inclusão de pessoas com deficiência.

## Tecnologias

- **Frontend:** Angular 17+ com TypeScript
- **Backend:** .NET 8 / C#
- **Banco de dados:** PostgreSQL 15
- **Infraestrutura:** AWS (EC2 + RDS)

## Estrutura do Projeto

```
AcessaAi/
├── frontend/       # Aplicação Angular
├── backend/        # API .NET/C#
├── LICENSE
└── README.md
```

## Como executar localmente

### Backend

```bash
cd backend
dotnet restore
dotnet run
```

### Frontend

```bash
cd frontend
npm install
ng serve
```

A aplicação estará disponível em `http://localhost:4200`.

## CI/CD

Utilizamos **GitHub Actions** como ferramenta de CI/CD. A escolha se justifica pela integração nativa com o GitHub, plano gratuito com 2.000 minutos/mês e configuração simplificada via YAML.

O pipeline é acionado a cada `push` ou `pull request` na branch `main` e executa dois jobs em paralelo:

- **Backend (.NET):** restore → build → testes unitários
- **Frontend (Angular):** install → lint → build

O arquivo de configuração está em `.github/workflows/ci.yml`.

### Fluxo do Pipeline

```
Push / Pull Request (main)
         │
         ├──────────────────────┐
         ▼                      ▼
   [Backend .NET]         [Frontend Angular]
         │                      │
   ┌─────┴─────┐          ┌────┴────┐
   │  Restore  │          │ npm ci  │
   │  Build    │          │  Lint   │
   │  Test     │          │  Build  │
   └───────────┘          └─────────┘
         │                      │
         └──────────┬───────────┘
                    ▼
            Pipeline concluído
```

## Infraestrutura (AWS)

A infraestrutura do projeto está hospedada na AWS com os seguintes recursos:

| Recurso | Serviço AWS | Configuração |
|---|---|---|
| Rede | VPC | 10.0.0.0/16 com subnets pública e privada |
| Servidor | EC2 | t3.micro, Ubuntu 22.04 LTS |
| Banco de dados | RDS | PostgreSQL 15, db.t3.micro |
| IP fixo | Elastic IP | Associado à EC2 |
| Firewall | Security Groups | SG-App (HTTP/HTTPS/SSH) e SG-DB (PostgreSQL) |

## Licença

Este projeto está licenciado sob a licença MIT.
Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.
