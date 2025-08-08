# 🚗 Car Rental API

API RESTful robusta para um sistema de aluguel de veículos, desenvolvida com as tecnologias mais modernas do ecossistema .NET. Este projeto demonstra a aplicação de conceitos avançados de arquitetura de software, como **Clean Architecture** e **Domain-Driven Design (DDD)**, além de uma suíte completa de testes automatizados e deploy em nuvem.

<p align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker">
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL">
  <img src="https://img.shields.io/badge/AWS-%23FF9900.svg?style=for-the-badge&logo=amazon-aws&logoColor=white" alt="AWS">
</p>

---

## ✨ Principais Features

* **🏛️ Arquitetura Limpa (Clean Architecture) e DDD:** Código desacoplado, testável e de fácil manutenção.
* **🔐 Autenticação e Autorização:** Segurança implementada com JWT e Policies baseadas em Roles (Perfis).
* **📦 Containerização com Docker:** Ambiente de desenvolvimento e produção padronizado e portável.
* **🧪 Suíte de Testes Completa:** Testes de Unidade e de Integração (com Testcontainers que executam em Docker) garantindo a confiabilidade do sistema.
* **☁️ Deploy na AWS:** Aplicação hospedada na nuvem e acessível publicamente.

---

## 🚀 API em Produção (Live Demo)

A API está em produção na AWS e pode ser acessada e testada através da documentação interativa do Swagger.

<p align="center">
  <a href="http://18.117.216.57/swagger" target="_blank" rel="noopener noreferrer">
    <img src="https://img.shields.io/badge/Testar%20API%20ao%20vivo%20na%20AWS-%23FF9900?style=for-the-badge&logo=amazon-aws&logoColor=white" alt="Acessar API na AWS">
  </a>
</p>

> **Observação:** A aplicação roda no Nível Gratuito da AWS (instância `t3.micro`). A primeira requisição pode levar alguns segundos para "acordar" o servidor.

---

## 🏛️ Arquitetura

Este projeto adota os princípios da **Clean Architecture** para separar as responsabilidades em camadas bem definidas, garantindo um sistema coeso e de baixo acoplamento.

* **Domain:** O núcleo do software. Contém as entidades de negócio e a lógica de domínio pura.
* **Application:** Orquestra o fluxo de dados e contém a lógica dos casos de uso.
* **Infrastructure:** Implementa os detalhes técnicos, como acesso ao banco de dados (EF Core) e serviços.
* **Presentation:** A camada de entrada da aplicação, onde os endpoints da API (Minimal API) são definidos.

---

## 🛠️ Tecnologias Utilizadas

* **.NET 9** e **ASP.NET Core** para construção de Minimal APIs
* **Entity Framework Core** como ORM para acesso ao banco de dados
* **PostgreSQL** como banco de dados relacional
* **Docker** e **Docker Compose** para containerização
* **JWT (JSON Web Tokens)** para autenticação e autorização utlizando Microsoft.Identity
* **MSTest**, **FluentAssertions** e **Moq** para testes de unidade
* **Testcontainers (docker)** para testes de integração em um ambiente realista
* **NSwag** para geração da documentação OpenAPI (Swagger)

---

## 🚀 Como Executar o Projeto localmente

### Pré-requisitos

* [Git](https://git-scm.com/)
* [Docker](https://www.docker.com/products/docker-desktop/)

### 🐳 Rodando com Docker (Recomendado)

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/GustavoHerreira/car-rental-api.git
    cd car-rental-api
    ```

2. **Crie o arquivo de ambiente a partir do exemplo:**
    * Faça uma cópia do arquivo `.env.example` e renomeie a cópia para `.env`.
    * Altere as variáveis dentro do novo arquivo `.env` conforme necessário.

3.  **Suba os contêineres:**
    ```bash
    docker-compose up -d
    ```

4.  **Pronto!** A aplicação estará acessível:
    * **API:** `http://localhost:8080` (deve te redirecionar pra o Swagger)
    * **Swagger UI:** `http://localhost:8080/swagger`

---

## 🧪 Como Executar os Testes

Os testes garantem a qualidade e a estabilidade da aplicação.

* **Para rodar todos os testes (unidade e integração):**
    ```bash
    dotnet test
    ```
<details>
<summary><strong>📊 Clique para ver a cobertura de testes</strong></summary>
<br>
  <img width="1282" height="260" alt="image" src="https://github.com/user-attachments/assets/24ef21db-c825-46be-bf94-213922088f89" />


  <img width="703" height="1201" alt="image" src="https://github.com/user-attachments/assets/93e097f1-eef7-4030-84f8-af49d242fad6" />
</details>
