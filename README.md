# CRUD Intermediario
CRUD de nível avançado desenvolvido com template web do **ASP .NET 5** utilizando o **EntityFramework** e o gerênciador de pacotes **libman**.
___

## Dependências

ASP .NET version 5.0.408

```sh
dotnet new web --no-https --framework net5.0
```

/properties

```sh
dotnet new globaljson --sdk-version 5.0.408
```

jsdelivr

```sh
libman init -p jsdelivr
```

Bootstrap5

```sh
libman install bootstrap -d wwwroot/lib/bootstrap5
```

Entity Framework Design

```sh
dotnet add package Microsoft.EntityFrameworkCore.Design --version 5.0.9
```

Entity Framework - SQLite

```sh
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 5.0.9
```

___

## Funcionalidades

- Listar, Cadastrar, Editar e Excluir registros de Produtos;
- Listar, Cadastrar, Editar e Excluir registros de Categorias de Produtos;
- Listar, Cadastrar, Editar e Excluir registros de Clientes;
- Listar, Cadastrar, Editar e Excluir registros de Pedidos;

___

## Gerando o Database

- Verificando se as ferramentas do Entity Framework estão instaladas:

    ```sh
    dotnet ef --version
    ```

- Instalando as ferramentas do Entity Framework:

    ```sh
    dotnet tool install --global dotnet-ef
    ```

- Criando o Database com Entity Framework:

    ```sh
    dotnet ef migrations add 'version_name'
    dotnet ef database update
    ```