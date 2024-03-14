
![Logo Nagare](https://i.ibb.co/r3jmyPm/image.png)


# Nagare API

Este projeto visa criar uma ferramenta de gerenciamento de projetos, oferecendo uma plataforma flexível para organizar tarefas em quadros e listas, proporcionando uma experiência de gerenciamento de projetos intuitiva e colaborativa.

# Planejamento do projeto

Afim de um desenvolvimento mais ágil usando a metodologia SCRUM e levando em consideração a ideia do "Dev em T", o projeto foi desenhado no **Figma** e planejado no **Trello**.
Acesse o mockup do projeto no **Figma** [neste link](https://www.figma.com/file/wJQpWsDcl19kgiepqDVW5C/Nagare-team-library?type=design&node-id=0-1&mode=design&t=2RUVoI9kZc8TO4qw-0)
Acesse o gerenciamento do projeto no estilo **SCRUM** no **Trello** [neste link](https://trello.com/b/DbrVKQYL/nagare)


## Aviso importante

Este repositório é somente a parte do back-end do projeto. Para funcionar do modo esperado, deve-se executar também a API que se encontra neste repositório: https://github.com/leopholdo/nagare-kanban-app


## Funcionalidades

- **Registro de usuários** persistindo os dados em banco ou memória.
- **Criptografia de password** com BCrypt.
- **Validações de dados de usuários** em camadas diferentes.
- **Autenticação de usuários**.
- **Comunicação com API** de forma anônima e autenticada via Bearer token.
- **Implementação de uma API segura** por CORS, autenticação e autorização com JWT e Bearer.
- **Reautenticação** automática de tokens expirados.
- **Quadros e listas dinâmicas** para gerenciar projetos e organizar tarefas em listas personalizadas.
- **Personalização de quadros** com diferentes imagens e cores.
- **Cartões de tarefas** com funcionalidades diversas, como adicionar descrição, comentários, etiquetas, checklists, data de entrega, entre outras coisas.
- **Arrastar e soltar** cartões entre listas e ajustar as ordens das listas facilmente.
- **Colaboração em equipe** com a possibilidade de atribuir membros à tarefas.


## Tecnologias utilizadas

**Front-end:** VueJS, Vuetify, Pinia, Marked, Vue Draggable Plus (SortableJS) e Vue Advanced Cropper.

**Back-end:** ASP.NET Core 8, Swagger UI, Entity Framework Core, BCrypt, e Authentication JwtBearer.

**Banco de dados:** PostgreSQL.


## Como usar

1. **Configuração do ambiente:**
- Clone e configure o repositório do front-end [que se encontra neste link](https://github.com/leopholdo/nagare-kanban);
- Certifique-se de ter o .NET SDK 8 instalado em sua máquina. Você pode baixar a versão mais recente [neste link](https://dotnet.microsoft.com/pt-br/download).
- Clone este repositório;
- Execute o comando abaixo para restaurar os pacotes e dependências do projeto:
```
dotnet restore
```

2. **Configuração do Banco de dados**
**InMemory Database:**
- Abra o arquivo **Program.cs**
- Descomente da linha 23 a 25;
- Comente da linha 28 a 30;

**PostgreSQL:**
- Abra o arquivo **Program.cs**
- Comente da linha 23 a 25;
- Descomente da linha 28 a 30;
- Certifique-se de ter o ambiente PostgreSQL configurado na sua máquina;
- Abra o arquivo **appsettings.json** e edite a **ConnectionString** com as credenciais do seu ambiente PostgreSQL;
- Aplique as Migrações do Banco de Dados com o comando: 
```
dotnet ef database update

```

3. **Execução do Projeto:**
- Execute o projeto com o comando 
```
dotnet run
```

## Acesso ao projeto
**Com a aplicação front-end**
- Execute o projeto APP com o comando 
```
npm run dev
```
Ou se preferir com o YARN,
```
yarn dev
```
- Abra o navegador e acesse a aplicação pelo endereço http://localhost:5001

**Swagger-UI**
- Abra o navegador e acesse o Swagger pelo endereço http://localhost:5298/swagger/index.html

## Contribuindo

Contribuições são sempre bem-vindas!

Se você encontrar algum problema ou tiver sugestões, sinta-se à vontade para abrir uma [issue](https://github.com/leopholdo/nagare-kanban-api/issues/new) ou enviar um [pull request](https://github.com/leopholdo/nagare-kanban-api/pulls).