# 🚗 Car Rental API - Projeto DIO

Este projeto está a ser desenvolvido como parte do **Bootcamp da Digital Innovation One (DIO)** com foco em **Minimal APIs em C#**, **Entity Framework Core**, **testes automatizados** e **deploy na AWS**.

**Nota:** Esta é uma implementação própria da ideia proposta no bootcamp, com uma arquitetura e funcionalidades distintas da apresentada pelo professor.

## 📚 Tecnologias Utilizadas

- **🚀 .NET 9:** A versão mais recente do framework da Microsoft, com foco em performance e Minimal APIs.
- **💾 Entity Framework Core (EF Core):** ORM para interação com o banco de dados PostgreSQL.
- **📄 Swagger (NSwag):** Documentação e teste interativo da API.
- **🔐 Autenticação e Autorização:** Implementação com JWT (JSON Web Tokens) para proteger os endpoints, com base em perfis (Roles).
- **📦 Docker:** O projeto está containerizado para facilitar o desenvolvimento e o deploy. Inclui `Dockerfile` e `docker-compose.yml` para orquestração dos serviços (API e banco de dados).
- **🧪 Testes Automatizados:**
  - **Testes de Unidade:** Para validar a lógica de negócio de forma isolada.
  - **Testes de Integração:** Para garantir que os componentes do sistema funcionam corretamente em conjunto, incluindo a interação com o banco de dados de teste.

## 🔧 Funcionalidades Implementadas

- ✅ **Autenticação de Administradores:** Cadastro e login com geração de token JWT.
- ✅ **Autorização baseada em Roles:** Endpoints protegidos que exigem um perfil específico (ex: Admin ou Editor).
- ✅ **CRUD de Veículos:** Operações completas de criação, leitura, atualização e exclusão de veículos.
- ✅ **Validações Personalizadas:** Regras de negócio aplicadas nos DTOs e entidades.
- ✅ **Estrutura Organizada:** Separação de responsabilidades em camadas (Domain, Infrastructure, Presentation).
- ✅ **Testes Abrangentes:** Cobertura de testes para as principais funcionalidades.

## 🐳 Como Executar com Docker

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/CarRentalAPI.git
   cd CarRentalAPI
   ```

2. **Crie o arquivo de ambiente:**
   Crie um arquivo `.env` na raiz do projeto. Ele é essencial para configurar as variáveis de ambiente que o `docker-compose.yml` utiliza para subir os serviços. Exemplo de conteúdo:
   ```env
   POSTGRES_DB=carrentaldb
   POSTGRES_USER=user
   POSTGRES_PASSWORD=password
   ```

3. **Execute o Docker Compose:**
   ```bash
   docker-compose up -d
   ```

4. **Acesse a API:**
   - A API estará disponível em `http://localhost:8080`
   - A documentação do Swagger estará em `http://localhost:8080/swagger`

## ⚙️ Como Executar Localmente (Sem Docker)

1. **Configure o Banco de Dados:**
   - Certifique-se de ter o PostgreSQL instalado.
   - Atualize a `ConnectionString` no arquivo `appsettings.Development.json` para refletir suas configurações locais.

2. **Execute as Migrations:**
   ```bash
   dotnet ef database update
   ```

3. **Inicie a Aplicação:**
   ```bash
   dotnet run --project CarRentalAPI.API
   ```
