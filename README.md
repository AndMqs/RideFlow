[![C#](https://img.shields.io/badge/C%23-6E7EBE?style=flat&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-5B6FC2?style=flat&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-5F8FBF?style=flat&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![xUnit](https://img.shields.io/badge/xUnit-7C5BA3?style=flat&logo=xunit&logoColor=white)](https://xunit.net/)
[![EF Core](https://img.shields.io/badge/EF%20Core-6A8CAF?style=flat&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![Test Coverage](https://img.shields.io/badge/Coverage-98%25-6FB98F?style=flat)](README-DEV.md#cobertura-de-testes)
[![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-F4A261?style=flat)](README-DEV.md#arquitetura) 
[![Licença](https://img.shields.io/badge/Licen%C3%A7a-MIT-6FB98F?style=flat)](README-DEV.md#licenca)

---

Repositório destinado ao projeto final do Code RDIversity, desenvolvido por [Andresa Marques](https://www.linkedin.com/in/andresa-marques-dev/) e [Fernanda Worn](https://www.linkedin.com/in/fernandaworm/).



<img width="1536" height="1024" alt="logo" src="https://github.com/user-attachments/assets/6f2e17c7-0359-4119-b8f8-d0e358ac193a" />


# 🚗 RideFlow API

API de transporte individual similar a Uber, desenvolvida em **.NET 10** com **Entity Framework Core** e **PostgreSQL**.  
O sistema permite gerenciar usuários, motoristas, corridas e avaliações, com geração de relatórios em CSV.

---

## 📋 Índice

- [Arquitetura](README-DEV.md#arquitetura) 
- [Tecnologias](README-DEV.md#tecnologias)
- [Configuração do Banco](README-DEV.md#configuracao-do-banco)
- [Endpoints](README-DEV.md#endpoints)
  - [Usuários](README-DEV.md#usuarios)
  - [Motoristas](README-DEV.md#motoristas)
  - [Corridas](README-DEV.md#corridas)
  - [Avaliações](README-DEV.md#avaliacoes)
- [Relatórios](README-DEV.md#relatorios)
- [Estrutura do Projeto](README-DEV.md#estrutura-do-projeto)
- [Como Executar](README-DEV.md#como-executar)
- [Regras de Negócio](README-DEV.md#regras-de-negocio-implementadas)
- [Autores](README-DEV.md#autores)
- [Licença](README-DEV.md#licenca)


<h2 align="center"> Requisitos do Projeto Final </h2>

### Objetivo

Desenvolver uma API REST integrada a um banco de dados relacional (SQL), aplicando os conceitos estudados ao longo do curso, como:

- Modelagem de dados
- CRUD
- Relacionamentos entre tabelas
- Regras de negócio
- Filtros com parâmetros
- Consultas utilizando JOIN

### Requisitos Mínimos Obrigatórios

Todo projeto deve conter obrigatoriamente:

- No mínimo 5 tabelas no banco de dados
- 1 CRUD completo (Create, Read, Update e Delete)
- 1 rota que gere um relatório (criação de arquivo)
- 1 relacionamento N:N
- 1 regra de negócio implementada na API
- 1 filtro utilizando parâmetro
- 1 consulta utilizando JOIN
- Projeto deve ter um README listando todos suas funcionalidades
- Projeto deve ter no mínimo 80% de cobertura de testes
