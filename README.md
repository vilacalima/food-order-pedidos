# FoodOrder.Pedidos

Microserviço responsável pelo gerenciamento de pedidos em um sistema de delivery.

## Coverage
[![codecov](https://codecov.io/gh/vilacaro/food-order-pedidos/branch/Master/graph/badge.svg)](https://codecov.io/gh/vilacaro/food-order-pedidos)

## 📦 Funcionalidades

- Criação de pedidos com itens de um cardápio externo
- Integração assíncrona com serviços de:
  - Pagamento
  - Produção
  - Cardápio
  - Usuário
- Comunicação via AWS SQS (mensageria)

## 🛠 Tecnologias Utilizadas

- .NET 8
- C#
- PostgreSQL
- Docker e Docker Compose
- AWS SQS
- DDD e Clean Architecture

## 📂 Estrutura do Projeto

```
food-order-pedidos/
├── src/ 
│ ├── FoodOrder.Pedidos.Application/ # Regras de negócio (casos de uso)
│ ├── FoodOrder.Pedidos.Domain/ # Entidades, agregados e interfaces
│ ├── FoodOrder.Pedidos.Infrastructure/ # Repositórios, banco de dados, integrações externas
│ ├── FoodOrder.Pedidos.Presentation/ # API (Controllers, Middlewares, etc.)
│ └── FoodOrder.Pedidos.Tests/ # Testes unitários
├── docker-compose/ # Arquivos para orquestração de containers
├── docker-compose.yml
└── README.md
```

## 🧭 Arquitetura do Serviço de Pedidos

![Arquitetura FoodOrder.Pedidos](./Readme/Arquitetura.jpg)

## 🚀 Como Rodar o Projeto

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### Passos

1. Clone o repositório:

```bash
git clone https://github.com/vilacalima/food-order-pedidos.git
cd food-order-pedidos
````

### :page_with_curl: Documentações
- [Documentação de arquitetura](./Readme/README_ARQUITETURA.md)
- [Documentação do banco de dados](./Readme/README_DB.md)

### :busts_in_silhouette: Autores
| [<img loading="lazy" src="https://avatars.githubusercontent.com/u/96452759?v=4" width=115><br><sub>Robson Vilaça - RM358345</sub>](https://github.com/vilacalima) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/16946021?v=4" width=115><br><sub>Diego Gomes - RM358549</sub>](https://github.com/diegogl12) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/8690168?v=4" width=115><br><sub>Nathalia Freire - RM359533</sub>](https://github.com/nathaliaifurita) |  [<img loading="lazy" src="https://avatars.githubusercontent.com/u/43392619?v=4" width=115><br><sub>Rafael Kamada - RM359345</sub>](https://github.com/RafaelKamada) |
| :---: | :---: | :---: | :---: |
