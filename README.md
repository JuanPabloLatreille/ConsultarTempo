# ☁️ Consultar Tempo

**Autor:** Juan Pablo Latreille

Aplicação full-stack para consulta e registro de temperaturas de cidades brasileiras, consumindo a API do OpenWeatherMap. Desenvolvida com .NET 8 no backend, Vue 3 + TypeScript no frontend e PostgreSQL como banco de dados relacional.

---

## Tecnologias

### Backend
- .NET 8 (C#)
- Clean Architecture com CQRS (MediatR)
- Entity Framework Core 8 com PostgreSQL
- FluentValidation
- Autenticação JWT
- RestSharp para integração com OpenWeatherMap
- Swagger/OpenAPI para documentação
- Testes unitários (NUnit, Moq, Bogus, FluentAssertions)
- Testes de integração (WebApplicationFactory)

### Frontend
- Vue 3 + TypeScript
- Vue Router com guards de autenticação
- Axios para comunicação com a API
- Chart.js + vue-chartjs para gráficos
- CSS puro (sem frameworks de UI)

### Infraestrutura
- Docker e Docker Compose
- PostgreSQL 17
- Nginx (servidor do frontend com proxy reverso)

---

## Funcionalidades

- **Registrar temperatura por cidade** — Endpoint que recebe o nome da cidade, consulta o provedor de clima (OpenWeatherMap), persiste no banco e retorna a temperatura atual.
- **Registrar temperatura por coordenadas** — Endpoint que recebe latitude e longitude, consulta o provedor de clima, persiste no banco e retorna a temperatura atual.
- **Consultar histórico de temperaturas** — Endpoint que recebe o nome da cidade ou coordenadas e retorna o histórico dos últimos 30 dias, ordenado do mais recente para o mais antigo.
- **Interface Web** — Frontend em Vue 3 + TypeScript que permite registrar temperaturas, consultar e visualizar o histórico em lista e em gráfico.
- **Autenticação JWT** — Cadastro de usuário e login com proteção dos endpoints de escrita.
- **Health Check** — Endpoint `/health` que verifica a saúde da aplicação e conexão com o banco.

---

## Arquitetura

```
ConsultWeather/
├── src/
│   ├── API/                  # Camada de apresentação (Controllers, Middlewares)
│   ├── Application/          # Camada de aplicação (Commands, Queries, Validators)
│   ├── Domain/               # Camada de domínio (Entidades, Interfaces, Exceptions)
│   └── Infraestructure/      # Camada de infraestrutura (Repositórios, EF Core, Services)
├── tests/
│   ├── UnitTests/            # Testes unitários
│   └── IntegrationTests/     # Testes de integração
├── frontend/                 # Vue 3 + TypeScript
├── docker-compose.yml
└── README.md
```

---

## Como Executar

### Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/) instalado e rodando

### Subir a aplicação completa

Na raiz do projeto, execute:

```bash
docker compose up -d --build
```

Esse comando sobe os 3 serviços:

| Serviço   | URL                          | Descrição                     |
|-----------|------------------------------|-------------------------------|
| Frontend  | http://localhost:3000         | Interface web (Vue 3)         |
| API       | http://localhost:8080         | Backend (.NET 8)              |
| Swagger   | http://localhost:8080/swagger | Documentação da API           |
| Health    | http://localhost:8080/health  | Health check da aplicação     |
| Banco     | localhost:5432               | PostgreSQL 17                 |

### Primeiros passos

1. Acesse `http://localhost:3000`
2. Crie uma conta na tela de login
3. Faça login com as credenciais criadas
4. Registre a temperatura de uma cidade (ex: Curitiba, Toledo, Cascavel)
5. Consulte o histórico de temperaturas na aba "Histórico"

---

## Endpoints da API

### Autenticação
| Método | Rota                        | Descrição              | Auth |
|--------|-----------------------------|------------------------|------|
| POST   | `/api/usuarios/criar-cadastro` | Criar conta          | Não  |
| POST   | `/api/usuarios/autenticar`     | Login (retorna JWT)  | Não  |

### Cidades
| Método | Rota                    | Descrição              | Auth |
|--------|-------------------------|------------------------|------|
| GET    | `/api/cidades`          | Listar todas as cidades | Não  |
| GET    | `/api/cidades/{id}`     | Buscar cidade por ID    | Não  |
| POST   | `/api/cidades`          | Cadastrar cidade        | Sim  |

### Histórico de Temperaturas
| Método | Rota                                          | Descrição                        | Auth |
|--------|-----------------------------------------------|----------------------------------|------|
| GET    | `/api/historicostemperaturas`                  | Listar todos os registros        | Não  |
| GET    | `/api/historicostemperaturas/por-nome/{cidade}` | Histórico por nome (últimos 30d) | Não  |
| GET    | `/api/historicostemperaturas/por-coordenadas`  | Histórico por coordenadas        | Não  |
| POST   | `/api/historicostemperaturas/por-nome`         | Registrar temperatura por nome   | Sim  |
| POST   | `/api/historicostemperaturas/por-coordenadas`  | Registrar por coordenadas        | Sim  |
| DELETE | `/api/historicostemperaturas/id/{id}`          | Remover registro                 | Sim  |

### Health Check
| Método | Rota      | Descrição         |
|--------|-----------|-------------------|
| GET    | `/health` | Status da aplicação |

---

## Comandos Docker úteis

```bash
# Subir tudo (API + banco + frontend)
docker compose up -d --build

# Subir somente o banco (para desenvolvimento local)
docker compose up -d db

# Ver containers rodando
docker compose ps

# Ver logs em tempo real
docker compose logs -f

# Ver logs de um serviço específico
docker compose logs -f api

# Parar tudo
docker compose down

# Parar tudo e limpar dados do banco
docker compose down -v
```

---

## Executar Testes

```bash
# Todos os testes
dotnet test

# Testes unitários
dotnet test tests/UnitTests

# Testes de integração (requer banco rodando)
docker compose up -d db
dotnet test tests/IntegrationTests
```

---

## Variáveis de Ambiente

| Variável                              | Descrição                    | Valor padrão                  |
|---------------------------------------|------------------------------|-------------------------------|
| `ConnectionStrings__DefaultConnection` | Connection string PostgreSQL | Configurado no docker-compose |
| `OpenWeatherMap__ApiKey`              | Chave da API OpenWeatherMap  | Configurado no docker-compose |
| `Jwt__SecretKey`                      | Chave secreta do JWT         | Configurado no docker-compose |
| `Jwt__Issuer`                         | Emissor do token             | ConsultWeather                |
| `Jwt__Audience`                       | Audiência do token           | ConsultWeather                |
| `Jwt__ExpirationInMinutes`            | Tempo de expiração do token  | 60                            |
