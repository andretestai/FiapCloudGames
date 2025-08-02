# 🎮 FiapCloudGames

Sistema de gerenciamento de jogos, usuários e relacionamentos entre eles, utilizando ASP.NET Core com arquitetura em camadas.

---

## 🚀 **Funcionalidades principais**

✅ Cadastro, consulta, alteração e remoção de **Usuários**  
✅ Cadastro, consulta, alteração e remoção de **Jogos**  
✅ Associação entre **Usuários** e **Jogos** (UserGame)  
✅ Autenticação via **Token JWT**  
✅ Criptografia de senhas  
✅ Validações de segurança (senha, email)  
✅ Logs de atividades

---

## 🏗️ **Arquitetura do Projeto**

- **Controllers** → expõem a API REST (User, Game, UserGame).  
- **Services** → regra de negócio (criptografia, token, validação).  
- **Repositories** → acesso ao banco de dados (CRUD).  
- **Entities** → modelos de dados para persistência e transferências.

---

## 🖇️ **Principais Endpoints**

### 📌 **Usuário**

| Método | Endpoint                     | Descrição                        |
|-------- |-----------------------------|---------------------------------- |
| POST    | `/User`                     | Cadastra novo usuário            |
| GET     | `/User`                     | Lista todos os usuários (admin)  |
| GET     | `/User/{id}`                | Busca usuário por ID (admin)     |
| PUT     | `/User`                     | Atualiza usuário (admin)         |
| DELETE  | `/User/{id}`                | Remove usuário (admin)           |
| GET     | `/User/BuscarJogosUser/{id}`| Lista jogos do usuário           |
| GET     | `/User/Logar`               | Realiza login de usuário         |

---

### 📌 **Jogo**

| Método | Endpoint       | Descrição                        |
|-------- |---------------|---------------------------------- |
| POST    | `/Game`       | Cadastra novo jogo (admin)       |
| GET     | `/Game`       | Lista todos os jogos             |
| GET     | `/Game/{id}`  | Busca jogo por ID                |
| PUT     | `/Game`       | Atualiza jogo (admin)            |
| DELETE  | `/Game/{id}`  | Remove jogo (admin)              |

---

### 📌 **UserGame**

| Método | Endpoint         | Descrição                                  |
|-------- |-----------------|------------------------------------------- |
| POST    | `/UserGame`     | Associa usuário a um jogo (admin)          |
| GET     | `/UserGame`     | Lista todas as associações (admin)         |
| GET     | `/UserGame/{id}`| Consulta associação por ID (admin)         |
| PUT     | `/UserGame`     | Atualiza associação (admin)                |
| DELETE  | `/UserGame/{id}`| Remove associação (admin)                  |

---

## 🔐 **Segurança**

- Autenticação via **Token JWT**.
- Rotas protegidas com **[Authorize]** e políticas de **Admin**.
- Senhas armazenadas com **criptografia**.
- Validação de **email e senha** no cadastro.

---

## 🛠️ **Tecnologias utilizadas**

- **.NET 6+ / ASP.NET Core**
- **Entity Framework Core**
- **AutoMapper**
- **JWT Bearer**
- **FluentValidation**
- **Swagger** (opcional para testes)
- **SQL Server** ou outro banco relacional

---
