# Sistema de Biblioteca: Migração de Arquitetura

## Sobre o projeto

O sistema permite o cadastro e gerenciamento de Autores, Editoras, Livros e Itens de Acervo de uma biblioteca, com autenticação de usuários (ASP.NET Identity), disponibilizado tanto como aplicação web (MVC + Razor Pages) quanto como API REST.

Este repositório documenta a evolução da arquitetura do projeto entre dois momentos:

- Antes: arquitetura em camadas tradicional (Controller, Service, Contexto EF).
- Depois: início da migração para Clean Architecture (Controller, Use Case, Repositório), aplicada ao recurso Autor.

## Arquitetura "Antes" (em camadas)

```
BibliotecaWeb / BibliotecaAPI  (Controllers)
        │
        ▼
     Service        (regras de negócio + acesso a dados)
        │
        ▼
      Core           (Entidades, DTOs, BibliotecaContext - EF)
        │
        ▼
      MySQL          (banco real, via connection string)
```

- Core: entidades (`Autor`, `Editora`, `Livro`, `Itemacervo`...), DTOs, e o `BibliotecaContext` (Entity Framework) usado diretamente pelos serviços.
- Service: implementações como `AutorService`, `EditoraService`, `ItemAcervoService`, `LivroService`, que acessam o `BibliotecaContext` diretamente.
- Util: validações customizadas (CPF, CEP, telefone).
- BibliotecaWeb / BibliotecaAPI: os controllers dependiam diretamente das interfaces de `Service` (ex.: `IAutorService`), que por sua vez dependiam do Entity Framework.
- Banco de dados: MySQL real, configurado via `connection string` no `appsettings.json`.

Limitação dessa abordagem: a regra de negócio (Service) conhece o Entity Framework diretamente. Isso significa que trocar o banco de dados ou testar as regras de negócio sem um banco real exige mexer ou mockar várias camadas ao mesmo tempo.

## Arquitetura "Depois" (Clean Architecture, em migração)

```
BibliotecaWeb / BibliotecaAPI  (Controllers)
        │
        ▼
   Application       (Use Cases: CreateAutorUseCase, UpdateAutorUseCase...)
        │
        ▼
     Domain          (Entidades: AutorEntity + Interfaces: IAutorRepository)
        ▲
        │  implementa
        │
  Infrastructure      (AutorRepository + Context - EF InMemory)
```

- Domain: entidades puras (`AutorEntity`, `EditoraEntity`, `ItemAcervoEntity`) e interfaces de repositório (`IAutorRepository`, `IEditoraRepository`, `IItemAcervoRepository`), sem nenhuma dependência de Entity Framework.
- Application: Use Cases (um por operação de negócio), como `CreateAutorUseCase`, `UpdateAutorUseCase`, `DeleteAutorUseCase`, `GetAutorByIdUseCase`, `GetAllAutoresUseCase`, `GetAutoresPageUseCase`. Dependem apenas das interfaces do `Domain`, nunca do EF diretamente.
- Infrastructure: implementação concreta dos repositórios (`AutorRepository`) usando um novo `Context` (EF Core), além das implementações de `Editora` e `ItemAcervo`.
- Banco de dados: passou a usar EF Core InMemory (`UseInMemoryDatabase`), tanto para o `Context` novo quanto para o `BibliotecaContext` antigo e o `IdentityContext`. Isso elimina a necessidade de configurar um MySQL real para rodar o projeto localmente.

## Por que migrar para Clean Architecture?

- Desacoplamento: no código antigo, o `Service` conhecia o Entity Framework diretamente, então trocar de banco de dados exigiria alterar a camada de regra de negócio. No código novo, o `CreateAutorUseCase` só conhece a interface `IAutorRepository`, sem saber (nem precisar saber) que existe um EF Core por trás.
- Testabilidade: como o Use Case depende de uma interface (`IAutorRepository`) e não de um `DbContext` real, é possível testar a regra de negócio "criar um autor" simulando (mockando) o repositório, sem precisar de um banco de dados de verdade.
- Separação de responsabilidades: `Domain` define o que o sistema faz (regras e contratos); `Application` orquestra os casos de uso; `Infrastructure` decide como os dados são persistidos, e cada camada pode evoluir de forma independente.

## Responsabilidades e Acoplamento

### Responsabilidade Única (SRP)

O `AutorService` (arquitetura antiga) concentra numa única classe: validação de regra de negócio (ex.: checar se o ano de nascimento é válido), todas as operações de CRUD, e ainda lógica de ordenação, filtro e paginação para o datatable, tudo misturado em cerca de 180 linhas.

Na Clean Architecture, cada operação foi separada em sua própria classe de Use Case: `CreateAutorUseCase`, `UpdateAutorUseCase`, `DeleteAutorUseCase`, `GetAutorByIdUseCase`, `GetAllAutoresUseCase`, `GetAutoresPageUseCase`. O `CreateAutorUseCase`, por exemplo, faz apenas uma coisa: valida os dados do autor e delega a criação ao repositório.

```csharp
// Application/Autor/CreateAutorUseCase.cs
public class CreateAutorUseCase
{
    private readonly IAutorRepository _autorRepository;

    public uint Execute(AutorEntity autor)
    {
        ValidateAutor(autor);
        _autorRepository.Create(autor);
        return autor.Id;
    }
}
```

Isso deixa cada classe com um único motivo para mudar: se a regra de validação mudar, só o Use Case correspondente é afetado; se a forma de persistir mudar, só o repositório é afetado.

### Inversão de Dependência (DIP)

No código antigo, o `AutorService` depende diretamente de uma classe concreta do Entity Framework:

```csharp
// Service/AutorService.cs
public class AutorService : IAutorService
{
    private readonly BibliotecaContext context; // dependência concreta do EF
    ...
}
```

No código novo, o `CreateAutorUseCase` depende apenas de uma interface (`IAutorRepository`), definida dentro do próprio `Domain`:

```csharp
// Application/Autor/CreateAutorUseCase.cs
public class CreateAutorUseCase
{
    private readonly IAutorRepository _autorRepository; // dependência de abstração
    ...
}

// Domain/Autor/IAutorRepository.cs
public interface IAutorRepository
{
    void Create(AutorEntity autor);
    void Update(AutorEntity autor);
    void Delete(uint id);
    AutorEntity? GetById(uint id);
    IEnumerable<AutorEntity> GetAll();
    PagedResult<AutorEntity> GetPage(PageRequest request);
}
```

Quem implementa essa interface usando Entity Framework é o `AutorRepository`, na camada `Infrastructure`. O Use Case não sabe (nem precisa saber) que existe um EF Core por trás.

Isso é confirmado também pelas referências de projeto (`.csproj`): o Domain não referencia nenhum outro projeto, nem o Entity Framework, nem nada. Ele é o centro da arquitetura; `Application` e `Infrastructure` dependem dele, nunca o contrário. Essa é a regra de dependência da Clean Architecture: as dependências sempre apontam para dentro, em direção às regras de negócio, nunca para os detalhes técnicos.

## Estrutura de pastas

```
Codigo/Biblioteca/
├── BibliotecaWeb/          # App MVC + Razor Pages (Identity), usa Service (antigo) e Application (novo, só Autor)
├── BibliotecaAPI/          # API REST, Autor já usa Application (novo)
├── Core/                   # Entidades, DTOs e Contexto EF da arquitetura antiga
├── Service/                # Implementações de regra de negócio da arquitetura antiga
├── Util/                   # Validações customizadas (CPF, CEP, telefone)
├── Domain/                 # Entidades e interfaces de repositório (Clean Architecture)
├── Application/            # Use Cases (Clean Architecture)
├── Infrastructure/         # Implementação dos repositórios + Context EF (Clean Architecture)
├── BibliotecaWebTests/     # Testes dos controllers
└── ServiceTests/           # Testes dos serviços
```

## Testes

```bash
dotnet test BibliotecaWebTests
dotnet test ServiceTests
```

## Autores

- Daiane Santos (daianesnts)
- Guilherme Seixas (guilheeme1108-prog)
- Hiakewve Santos (Hiakewve)
- Igor Lemos (igorlemos01)
- Larissa Lavínia (larissa-lavinia)
- Paulo Ítalo (Pauloisc)
- Pedro Henrique (pedrohsmesquita)

Trabalho desenvolvido para a disciplina de Tópicos Especiais de Engenharia de Software, sob orientação do professor Éricles dos Santos.

## Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](./LICENSE) para mais detalhes.
