# 🚗 Car Rental API - Projeto DIO

Este projeto foi desenvolvido como parte do **Bootcamp da Digital Innovation One (DIO)** com foco em **Minimal APIs em C#**, **Entity Framework Core**, **testes automatizados** e **deploy na AWS**.

**Nota:** Esta é uma implementação própria da ideia proposta no bootcamp, com uma arquitetura e funcionalidades distintas da apresentada pelo professor.

<a href="http://18.117.216.57/swagger" target="_blank" rel="noopener noreferrer">
    <img src="https://img.shields.io/badge/Acessar%20API%20na%20AWS-%23FF9900?style=for-the-badge&logo=amazon-aws&logoColor=white" alt="Acessar API na AWS">
</a>

---

## 🚀 Deploy e Acesso Online

A API está atualmente em produção e hospedada na **AWS**, utilizando os seguintes serviços:
- **Computação:** **Amazon EC2**, onde a aplicação roda dentro de contêineres Docker.
- **Banco de Dados:** **PostgreSQL**, também em um contêiner Docker na mesma instância EC2.
- **Deploy:** O processo é feito manualmente via `git pull` e `docker-compose build` diretamente no servidor.

> **Você pode testar a API ao vivo agora mesmo através da documentação interativa do Swagger:**
> <h3><a href="http://18.117.216.57/swagger" target="_blank" rel="noopener noreferrer"><strong>http://18.117.216.57/swagger</strong></a></h3>

**Observação:** Como a aplicação está rodando no Nível Gratuito da AWS (instância `t3.micro`), a primeira requisição pode demorar alguns segundos para "acordar" o servidor.

## 📚 Tecnologias Utilizadas

- **🚀 .NET 9:** A versão mais recente do framework da Microsoft, com foco em performance e Minimal APIs.
- **💾 Entity Framework Core (EF Core):** ORM para interação com o banco de dados PostgreSQL.
- **📄 Swagger (NSwag):** Documentação e teste interativo da API.
- **🔐 Autenticação e Autorização:** Implementação com JWT (JSON Web Tokens) para proteger os endpoints, com base em perfis (Roles).
- **📦 Docker:** O projeto está containerizado para facilitar o desenvolvimento e o deploy. Inclui `Dockerfile` e `docker-compose.yml` para orquestração dos serviços (API e banco de dados).
- **🧪 Testes Automatizados:**
  - **Testes de Unidade:** Para validar a lógica de negócio de forma isolada.
  - **Testes de Integração:** Para garantir que os componentes do sistema funcionam corretamente em conjunto, incluindo a interação com o banco de dados de teste rodando em conteiner Docker.

## 🏛️ Arquitetura e Design (Clean Architecture e DDD)
O projeto foi estruturado seguindo os princípios da ✨**Clean Architecture**✨ e do ✨**Domain-Driven Design (DDD)** ✨, com o objetivo de separar as responsabilidades e criar um sistema coeso e de baixo acoplamento. A organização das camadas é a seguinte:

- **✅ Presentation**: Contém a camada de entrada e interação com o usuário. Neste caso, os endpoints da Minimal API.
- **✅ Application**: Contém os casos de uso (a lógica da aplicação), definindo o que o sistema faz. Orquestra o domínio e lida com DTOs e interfaces.
- **✅ Domain**: O núcleo do software. Contém apenas a lógica de negócio pura, com as entidades, enums e regras de negócio.
- **✅ Infrastructure**: Contém as implementações técnicas e detalhes de baixo nível, como acesso ao banco de dados, envio de e-mails ou comunicação com outras APIs.

Essa abordagem promove um código mais testável, flexível e fácil de manter a longo prazo.


## 🔧 Funcionalidades Implementadas

- ✅ **Autenticação de Administradores:** Cadastro e login com geração de token JWT.
- ✅ **Autorização baseada em Roles:** Endpoints protegidos que exigem um perfil específico (ex: Admin ou Editor).
- ✅ **CRUD de Veículos:** Operações completas de criação, leitura, atualização e exclusão de veículos.
- ✅ **Validações Personalizadas:** Regras de negócio aplicadas nos DTOs e entidades.
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

## 🧪 Testes Automatizados

🛡️ A confiança na API é construída sobre uma base sólida de testes automatizados que cobrem as principais funcionalidades.

* **🔬 Testes de Unidade:** Focam em validar as menores partes do código e suas regras de negócio.
* **🤝 Testes de Integração:** Simulam o uso real da aplicação, testando o fluxo completo e a interação com o banco de dados.

### 🐳 Testes em Ambiente Realista
Para aumentar a confiança nos resultados, os testes de integração são executados em Docker (usando a biblioteca Testcontainers.PostgreSql do Nuget). Isso garante que o ambiente de teste seja uma cópia fiel do ambiente de produção.

<details>
<summary><strong>📊 Clique para ver a cobertura de testes</strong></summary>
<br>
  <img width="1282" height="260" alt="image" src="https://github.com/user-attachments/assets/24ef21db-c825-46be-bf94-213922088f89" />


  <img width="703" height="1201" alt="image" src="https://github.com/user-attachments/assets/93e097f1-eef7-4030-84f8-af49d242fad6" />
</details>





