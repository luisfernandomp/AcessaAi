# AcessaAi — API Reference

**Base URL:** `/api`

> Autenticação via **Bearer JWT**. Endpoints marcados com **Sim** exigem o header `Authorization: Bearer <token>`.

---

## Autenticação

**Base:** `/api/auth`

| Método | Endpoint       | Auth | Descrição                        |
|--------|----------------|------|----------------------------------|
| POST   | `/auth/login`  | Não  | Login — retorna token JWT        |

---

## Usuário

**Base:** `/api/usuario`

| Método | Endpoint              | Auth | Descrição               |
|--------|-----------------------|------|-------------------------|
| POST   | `/usuario/cadastrar`  | Não  | Cadastrar novo usuário  |
| GET    | `/usuario/{id}`       | Sim  | Obter usuário por ID    |

---

## Estabelecimento

**Base:** `/api/estabelecimento`

| Método | Endpoint                        | Auth | Descrição                                                     |
|--------|---------------------------------|------|---------------------------------------------------------------|
| POST   | `/estabelecimento`              | Sim  | Criar novo estabelecimento                                    |
| GET    | `/estabelecimento`              | Não  | Filtrar estabelecimentos (query params)                       |
| GET    | `/estabelecimento/{id}`         | Não  | Obter estabelecimento por ID                                  |
| PATCH  | `/estabelecimento/{id}`         | Sim  | Atualizar dados do estabelecimento                            |
| DELETE | `/estabelecimento/{id}`         | Sim  | Excluir estabelecimento                                       |
| POST   | `/estabelecimento/{id}/imagem`  | Sim  | Upload de imagem (`multipart/form-data`)                      |

### Parâmetros de filtro — `GET /estabelecimento`

Todos os campos são opcionais e enviados via query string.

| Parâmetro                   | Tipo          | Descrição                                      |
|-----------------------------|---------------|------------------------------------------------|
| `nome`                      | `string`      | Filtrar por nome                               |
| `distanciaMaxima`           | `double`      | Distância máxima em km a partir das coordenadas |
| `geocordenadasRequest.*`    | `object`      | Latitude e longitude de referência             |
| `enderecoRequest.*`         | `object`      | Endereço de referência                         |
| `recursosAcessabilidadeIds` | `int[]`       | IDs dos recursos de acessibilidade             |

---

## Avaliação

**Base:** `/api/avaliacao`

> Todos os endpoints exigem autenticação.

| Método | Endpoint             | Auth | Descrição                   |
|--------|----------------------|------|-----------------------------|
| POST   | `/avaliacao`         | Sim  | Criar avaliação             |
| GET    | `/avaliacao/{id}`    | Sim  | Obter avaliação por ID      |
| PATCH  | `/avaliacao/{id}`    | Sim  | Atualizar avaliação         |
| DELETE | `/avaliacao/{id}`    | Sim  | Excluir avaliação           |

---

## Recursos de Acessibilidade

**Base:** `/api/recursoacessibilidade`

| Método | Endpoint                                  | Auth | Descrição                           |
|--------|-------------------------------------------|------|-------------------------------------|
| GET    | `/recursoacessibilidade/listar-ativas`    | Não  | Listar recursos de acessibilidade ativos |
