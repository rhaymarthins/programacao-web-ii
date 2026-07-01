# Trabalho Final - Programação Web II

Sistema web desenvolvido em ASP.NET Core MVC com Entity Framework Core, utilizando a base de dados fornecida nas aulas da disciplina.

## Tecnologias utilizadas

- ASP.NET Core MVC (.NET 8.0)
- Entity Framework Core 8.0
- SQL Server
- ASP.NET Core Identity
- Bootstrap

## Funcionalidades implementadas

### Item 1: Cadastro e gerenciamento do próprio profissional

- Registro separado para Médico e Nutricionista, cada um recebendo sua respectiva autorização (Role) automaticamente
- Após o cadastro, o profissional tem acesso apenas aos seus próprios dados (Meu Perfil)
- O profissional pode editar seus dados, porém o campo CPF é bloqueado na edição pelo próprio profissional
- Isolamento de dados garantido no Controller: o sistema usa o e-mail do usuário logado para buscar o registro, impedindo acesso a dados de outros profissionais mesmo por manipulação de URL

### Item 2: Gerentes com acessos diferenciados

- Três usuários gerentes criados e associados às Roles diretamente no banco de dados
- GerenteMedico: acesso apenas aos profissionais médicos cadastrados
- GerenteNutricionista: acesso apenas aos profissionais nutricionistas
- GerenteGeral: acesso a todos os profissionais
- Gerentes podem visualizar, editar e excluir profissionais, mas não podem criá-los
- Na edição pelo gerente, todos os campos incluindo CPF são editáveis
- Exclusão bloqueada para profissionais que possuem pacientes cadastrados, exibindo mensagem de erro ao tentar

### Item 3: Gerenciamento de pacientes pelo profissional

- Cada profissional pode criar, editar, visualizar e excluir seus próprios pacientes
- O vínculo entre profissional e paciente é feito automaticamente na tabela tbMedico_Paciente no momento do cadastro
- A listagem exibe apenas os pacientes vinculados ao profissional logado
- Acesso bloqueado a pacientes de outros profissionais, mesmo por manipulação direta da URL

## Itens opcionais implementados

- **Filtro de planos por tipo de profissional:** no registro de Médico aparecem apenas os planos Médico Total e Médico Parcial, enquanto no registro de Nutricionista aparece apenas o plano Nutricional

## Modificações realizadas em relação às aulas

Durante o desenvolvimento, foram necessárias adaptações devido a diferenças entre a versão do Visual Studio utilizada pelo professor e a versão mais atual (17.14.34). As principais modificações estão documentadas no histórico de commits do repositório.

## Como executar

1. Clone o repositório
2. Configure a connection string no `appsettings.json` apontando para seu SQL Server local
3. Execute o projeto pelo Visual Studio ou pelo comando `dotnet run`
4. Registre um profissional pelo link disponível na barra de navegação

## Autor

Antonio Ray Martins Vieira
