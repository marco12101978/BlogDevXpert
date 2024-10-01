
[BlogDevXpert.Net] - Aplicação de Blog Simples com MVC e API RESTful

1. Apresentação

Seja bem-vindo ao repositório do projeto BlogDevXpert.Net.
Este projeto faz parte do MBA DevXpert Full Stack .NET, relacionado ao módulo de Introdução ao Desenvolvimento ASP.NET Core.
O principal objetivo deste projeto é criar uma aplicação de blog que permita aos usuários gerenciar posts e comentários, incluindo funcionalidades de criação, edição, visualização e exclusão.
A aplicação oferece uma interface web baseada no padrão MVC, além de fornecer suporte a operações via API RESTful.

Autor
Marco Aurelio Roque Pinto

2. Proposta do Projeto

Este projeto abrange os seguintes componentes:

Aplicação MVC: Uma interface web para interação direta com o blog.
API RESTful: Disponibilização dos recursos do blog para permitir integrações com outras aplicações ou o desenvolvimento de front-ends alternativos.
Autenticação e Autorização: Controle de acesso, distinguindo entre administradores e usuários comuns.
Acesso a Dados: Integração com o banco de dados por meio de um ORM, facilitando a manipulação dos dados.


3. Tecnologias Utilizadas
Linguagem de Programação: C#
Frameworks:
ASP.NET Core MVC
ASP.NET Core Web API
Entity Framework Core
Banco de Dados: SQL Server
Autenticação e Autorização:
ASP.NET Core Identity
JWT (JSON Web Token) para autenticação nas APIs
Front-end:
Razor Pages/Views
HTML/CSS para a estilização
Documentação da API: Swagger

4. Estrutura do Projeto
A estrutura do projeto é organizada da seguinte forma:

src/
Blog.Web/ - Projeto MVC
Blog.Api/ - API RESTful
Blog.Data/ - Configuração do EF Core
Blog.Business/ - Modelos de Dados e Intarfaces
README.md - Arquivo de Documentação do Projeto
FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
.gitignore - Arquivo de Ignoração do Git

5. Funcionalidades Implementadas
CRUD para Posts e Comentários: Permite criar, editar, visualizar e excluir posts e comentários.
Autenticação e Autorização: Diferenciação entre usuários comuns e administradores.
API RESTful: Exposição de endpoints para operações CRUD via API.
Documentação da API: Documentação automática dos endpoints da API utilizando Swagger.

6. Como Executar o Projeto
Pré-requisitos
.NET SDK 8.0 ou superior
SQL Server
Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
Git

Passos para Execução
Clone o Repositório:

git clone https://github.com/marco12101978/BlogDevXpert.Net.git
cd nome-do-repositorio
Configuração do Banco de Dados:

No arquivo appsettings.json, configure a string de conexão do SQL Server.
Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos
Executar a Aplicação MVC:

cd src/Blog.Web/
dotnet run
Acesse a aplicação em: http://localhost:5000
Executar a API:

cd src/Blog.Api/
dotnet run
Acesse a documentação da API em: http://localhost:5001/swagger

7. Instruções de Configuração
JWT para API: As chaves de configuração do JWT estão no appsettings.json.
Migrações do Banco de Dados: As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

8. Documentação da API
A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

9. Avaliação
Este projeto é parte de um curso acadêmico e não aceita contribuições externas.
Para feedbacks ou dúvidas utilize o recurso de Issues
O arquivo FEEDBACK.md é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
