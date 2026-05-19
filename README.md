# Raízes do Nordeste API

API REST desenvolvida em *.NET 10* para gerenciamento de uma rede de restaurantes de culinária nordestina.

O projeto contempla:

- Gestão de usuários e autenticação JWT
- Controle de unidades/restaurantes
- Cardápio e produtos
- Controle de estoque
- Emissão e acompanhamento de pedidos
- Processamento de pagamento mock
- Controle de status do pedido
- Sistema de fidelidade por pontos

---

# 🚀 Tecnologias Utilizadas

- *.NET 10 / ASP.NET Core*
- *Entity Framework Core*
- *SQL Server 2022*
- *Docker & Docker Compose*
- *JWT Authentication*
- *FluentValidation*
- *Swagger / OpenAPI*
- *Postman*
- *DDD (Domain-Driven Design)*

---

# 🏗️ Arquitetura do Projeto

O sistema foi estruturado seguindo princípios de *DDD* e separação de responsabilidades.

## Camadas

### Domain
Responsável pelas regras centrais de negócio.

Contém:

- Entidades
- Enums
- Interfaces de repositório
- Regras de domínio

---

### Application
Responsável pelos casos de uso da aplicação.

Contém:

- Services
- DTOs / ViewModels
- Validações com FluentValidation

---

### Infrastructure
Responsável pelo acesso a dados e integração externa.

Contém:

- Entity Framework Core
- DbContext
- Repositórios
- Mapeamentos
- Persistência SQL Server

---

### API
Responsável pela exposição HTTP da aplicação.

Contém:

- Controllers REST
- Middlewares
- Configurações
- Swagger
- Tratamento global de erros

---

# 🐳 Executando com Docker

## Pré-requisitos

- Docker instalado
- Docker Compose habilitado

---

## Clonar o Repositório

### HTTPS

bash
git clone https://github.com/kedsonmrs/RaizesDoNordeste.git
cd RaizesDoNordeste


### SSH

bash
git clone git@github.com:kedsonmrs/RaizesDoNordeste.git
cd RaizesDoNordeste


---

## Subir os Containers

Execute na raiz do projeto:

bash
docker compose up --build -d


---

# ⚙️ Inicialização Automática

A aplicação possui inicialização automatizada.

## Migrations Automáticas

Ao iniciar:

- O SQL Server sobe via Docker
- A API executa:
  
csharp
context.Database.Migrate();


- Todas as migrations pendentes são aplicadas automaticamente

---

## Seed Inicial

Caso o banco esteja vazio:

- Um usuário administrador é criado automaticamente

### Credenciais padrão

| Campo | Valor |
|---|---|
| Email | admin@raizes.com |
| Senha | Senha@123 |

---

# 📖 Swagger

Após subir os containers:

## URLs

### Swagger

txt
http://localhost:8080/swagger


### Swagger UI

txt
http://localhost:8080/swagger/index.html


---

# 🧪 Testes com Postman

A coleção oficial está localizada em:

txt
postman/collection/RaizesDoNordeste_Final.postman_collection.json


---

# ▶️ Como Executar os Testes

## 1. Importar coleção

No Postman:

- Clique em *Import*
- Selecione o arquivo .json

---


## 2. Executar coleção

- Clique em *Run Collection*
- Execute a coleção completa

---

# ✅ Fluxos Testados

## Autenticação

- Cadastro de usuário
- Login cliente
- Login admin
- Token inválido
- Acesso sem autenticação

---

## Unidades

- Listar unidades
- Criar unidade
- Bloqueio por permissão

---

## Produtos e Estoque

- Criar produto
- Consultar cardápio
- Entrada de estoque
- Consultar saldo

---

## Pedidos

- Criar pedido válido
- Produto inexistente
- Estoque insuficiente
- Dados inválidos
- Consulta de pedidos
- Filtros e paginação

---

## Pagamentos

- Processar pagamento mock
- Bloqueio de pagamento duplicado

---

## Status do Pedido

Fluxo validado:

txt
AguardandoPagamento
→ EmPreparo
→ Pronto
→ Entregue


Também possui validação para impedir regressão de status.

---

## Fidelidade

- Acúmulo de pontos
- Consulta de saldo
- Resgate de pontos
- Validação de saldo insuficiente

---

# 🔗 Links Úteis

## Repositório

txt
https://github.com/seu-usuario/RaizesDoNordeste


---

## Swagger

txt
http://localhost:8080/swagger


---

---

## ATENÇÃO: CASO AO CLONAR O PROJETO DE ALGUM ERRO COM O PROJETO .API
### Exclua o projeto RaizesDoNordeste.API
### 1 - Vá na Solução
### 2 - Clique em Adicionar
### 3 - Adicionar projeto existente
### 4 - Selecione o arquivo .csproj do  projeto RaizesDoNordeste.API
### 4 - Compile a solução com CTRL + SHIFT + B
