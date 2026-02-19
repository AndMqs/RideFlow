# 🚗 RideFlow API

API de transporte individual similar a Uber, desenvolvida em **.NET 10** com **Entity Framework Core** e **PostgreSQL**.  
O sistema permite gerenciar usuários, motoristas, corridas e avaliações, com geração de relatórios em CSV.

---

## 📋 Índice

- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Configuração do Banco](#configuração-do-banco)
- [Endpoints](#endpoints)
  - [Usuários](#usuários)
  - [Motoristas](#motoristas)
  - [Corridas](#corridas)
  - [Avaliações](#avaliações)
- [Relatórios](#relatórios)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Regras de Negócio](#regras-de-negócio-implementadas)
- [Autores](#autores)
- [Licença](#licença)

---

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

### Camada de Controllers (API)
Responsável por receber as requisições HTTP, validar entrada e retornar respostas.  
Cada recurso possui seu próprio controller.

### Camada de Service (Regras de Negócio)
Contém toda a lógica de negócio da aplicação:

- **UserService** — Regras para usuários  
- **DriverService** — Regras para motoristas  
- **RideService** — Lógica de corridas (criação, cancelamento, finalização)  
- **RatingService** — Regras para avaliações  
- **RelatorioService** — Geração de relatórios  

### Camada de Repository (Acesso a Dados)
Responsável pela comunicação com o banco de dados:

- UserRepository  
- DriverRepository  
- RideRepository  
- RatingRepository  
- ServiceTypeRepository  

### Camada de Models (Entidades)
Representa as tabelas do banco de dados:

- TbUser  
- TbDriver  
- TbRide  
- TbRating  
- TbServicetype  

### DTOs (Data Transfer Objects)
Objetos para transferência de dados entre as camadas:

- CreateUserDto, UpdateUserDto  
- CreateDriverDto  
- CreateRideDto, CancelRideDto, FinishedRideDto  
- CreateRatingDto  
- RideResponseDto, CancelRideResponseDto, FinishedRideResponseDto, RatingResponseDto  

### Regras de Negócio
- **DriverCategoryRules** — Define categoria do motorista baseado no ano do carro  
- **PriceRules** — Calcula preço da corrida baseado na categoria e distância  


### Modelagem de dados

---

## 🛠️ Tecnologias

- .NET 10  
- Entity Framework Core 10  
- PostgreSQL 17  
- Npgsql (Provider PostgreSQL para EF Core)  
- Swagger / OpenAPI  

---

## 🗄️ Configuração do Banco

### String de conexão (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=rideflow;Username=postgres;Password=12345"
  }
}
```

### Estrutura de Enums no Banco
O banco utiliza colunas VARCHAR para enums:

- `tb_servicetype.category` → basic, premium, vip  
- `tb_ride.ride_status` → in_progress, finished, canceled  
- `tb_ride.payment_method` → credit_card, debit_card, pix  

---

## 📍 Endpoints

### 👤 Usuários
Base URL: `/user`

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /user | Criar novo usuário |
| GET | /user | Listar usuários |
| PATCH | /user/{id} | Atualizar usuário |
| DELETE | /user/{id} | Remover usuário |

---

### 🚘 Motoristas
Base URL: `/driver`

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /driver | Cadastrar motorista |
| GET | /driver | Listar motoristas |
| GET | /driver/category/{category} | Buscar por categoria |

#### Exemplo POST `/driver`
```json
{
  "nameDriver": "Carlos Motorista",
  "cnh": "123456789",
  "plate": "ABC1D23",
  "yearCar": 2022,
  "modelCar": "Toyota Corolla"
}
```

---

### 🏁 Corridas
Base URL: `/ride`

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /ride | Criar corrida |
| GET | /ride/all | Listar corridas |
| GET | /ride/status/{status} | Buscar por status |
| POST | /ride/cancel | Cancelar corrida |
| POST | /ride/finish | Finalizar corrida |
| GET | /ride/relatorio/{userId}/detalhado | CSV detalhado |
| GET | /ride/relatorio/{userId}/resumo | CSV resumo |

#### Exemplo POST `/ride`
```json
{
  "userId": "879a274b-98cd-435c-8d5f-0b3dedbea733",
  "startpoint": "Shopping Center",
  "destiny": "Aeroporto",
  "category": "vip",
  "paymentMethod": "pix",
  "Km": 25
}
```

---

### ⭐ Avaliações
Base URL: `/rating`

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | /rating | Avaliar corrida |
| GET | /rating/driver/{driverId} | Listar avaliações |
| GET | /rating/driver/{driverId}/average | Média avaliações |

Regras:
- Só avalia corrida finalizada  
- Nota de 1 a 5  
- Uma avaliação por corrida  
- Comentário opcional  

---

## 📊 Relatórios

### Relatório detalhado
```
GET /ride/relatorio/{userId}/detalhado
```
Gera CSV completo das corridas.

### Relatório resumido
```
GET /ride/relatorio/{userId}/resumo
```
Gera estatísticas:
- total corridas  
- total gasto  
- média por corrida  
- média avaliações  

---

## 📁 Estrutura do Projeto
```
RideFlow/
├── Controllers/
│   ├── UserController.cs
│   ├── DriverController.cs
│   ├── RideController.cs
│   └── RatingController.cs
├── Service/
│   ├── UserService.cs
│   ├── DriverService.cs
│   ├── RideService.cs
│   ├── RatingService.cs
│   └── RelatorioService.cs
├── Repositories/
│   ├── UserRepository.cs
│   ├── DriverRepository.cs
│   ├── RideRepository.cs
│   ├── RatingRepository.cs
│   └── ServiceTypeRepository.cs
├── Models/
│   ├── TbUser.cs
│   ├── TbDriver.cs
│   ├── TbRide.cs
│   ├── TbRating.cs
│   ├── TbServicetype.cs
│   └── Enums/
│       ├── RideStatus.cs
│       ├── PaymentMethod.cs
│       └── ServiceCategory.cs
├── DTOs/
│   ├── User/
│   ├── Driver/
│   ├── Ride/
│   └── Rating/
├── Properties/
├── appsettings.json
└── Program.cs
```

---

## 🚀 Como Executar

1. Configure o PostgreSQL  
2. Atualize `appsettings.json`  
3. Execute:

```bash
dotnet run --project RideFlow/RideFlow.csproj
```

Acesse:
```
http://localhost:5015
```

---

## 📝 Regras de Negócio Implementadas

### Motoristas
- basic: até 2015  
- premium: 2016–2022  
- vip: 2023+  

### Corridas
- preço por categoria + km  
- cancelamento com taxa 30%  
- só cancela em andamento  

### Avaliações
- nota obrigatória  
- 1 avaliação por corrida  
- só corrida finalizada  

---

## 👥 Autores
Projeto de estudo em .NET, EF Core e PostgreSQL.

---

## 📄 Licença
Uso educacional.