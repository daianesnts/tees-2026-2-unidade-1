# Sistema de Biblioteca: Migração de Arquitetura

## Sobre o projeto

O sistema permite o cadastro e gerenciamento de Autores, Editoras, Livros e Itens de Acervo de uma biblioteca, com autenticação de usuários (ASP.NET Identity), disponibilizado tanto como aplicação web (MVC + Razor Pages) quanto como API REST.

Este repositório documenta a arquitetura atual e a migração para Clean Architecture, aplicada aos recursos Autor, Editora, ItemAcervo e Livro.

## Arquitetura Atual

A arquitetura atual pode ser descrita por duas perguntas independentes, cada uma feita a partir de um ponto de vista diferente sobre o mesmo sistema.

A primeira pergunta é feita de fora do sistema, olhando para quantos artefatos existem e quantos bancos de dados são usados. Existem dois artefatos de deploy, `BibliotecaAPI` e `BibliotecaWeb`, mas os dois compartilham o mesmo código de regra de negócio (`Service`), a mesma camada de dados (`Core`) e o mesmo banco de dados. Não existe separação por módulos nem por bancos, então não se trata de SOA, microsserviços ou serverless.

A segunda pergunta é feita de dentro do sistema, olhando para onde apontam as dependências entre as camadas. Para as quatro entidades migradas (Autor, Editora, Livro e ItemAcervo), a resposta é: Arquitetura Clean. O Controller chama um Use Case na camada Application, o Use Case depende apenas de uma porta (interface) declarada no Domain, e quem implementa essa porta usando Entity Framework é um adaptador na camada Infrastructure. As dependências apontam para dentro, em direção ao Domain, e não para fora, em direção ao banco de dados.

### Estilo Interno: Arquitetura em camadas clássica

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

Esse padrão ainda está em uso no EditoraController, LivroController e ItemAcervoController da BibliotecaWeb. Controller chama Service, Service chama Core, e o Core já importa o Entity Framework diretamente. Vale destacar que apenas 4 entidades foram migradas para a arquitetura Clean. 

- Core: entidades (`Autor`, `Editora`, `Livro`, `Itemacervo`), DTOs, e o `BibliotecaContext` (Entity Framework) usado diretamente pelos serviços.
- Service: implementações como `AutorService`, `EditoraService`, `ItemAcervoService`, `LivroService`, que acessam o `BibliotecaContext` diretamente.
- Util: validações customizadas (CPF, CEP, telefone).
- BibliotecaWeb / BibliotecaAPI: os controllers dependiam diretamente das interfaces de `Service` (ex.: `IAutorService`), que por sua vez dependiam do Entity Framework.
- Banco de dados: MySQL real, configurado via `connection string` no `appsettings.json`.

## Arquitetura Escolhida: Clean Architecture

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
- Infrastructure: implementação concreta dos repositórios (`AutorRepository`) usando um novo `Context` (EF Core), além das implementações de `Editora`, `ItemAcervo` e `Livro`.
- Banco de dados: passou a usar EF Core InMemory (`UseInMemoryDatabase`), tanto para o `Context` novo quanto para o `BibliotecaContext` antigo e o `IdentityContext`.

### Por que Clean Architecture, e não Hexagonal ou Onion?

- **Clean vs. Onion**: Foi optado a arquitetura Clean porque ela representa uma separação mais explícita das responsabilidades, principalmente através da divisão entre entidades, casos de uso, adaptadores e infraestrutura. Isso facilita a organização do projeto, a realização de testes e a manutenção do sistema à medida que ele cresce. Portanto, a Clean foi escolhida por oferecer uma estrutura mais detalhada e adequada às necessidades de evolução e manutenção da aplicação.
- 
- **Clean vs. Hexagonal**: Embora a arquitetura Hexagonal também ofereça baixo acoplamento e independência em relação a tecnologias externas, optamos pela arquitetura clean porque o projeto necessita de uma estrutura mais explícita para separar as regras de negócio, os casos de uso, os adaptadores e a infraestrutura. A Clean facilita a visualização das responsabilidades de cada parte do sistema e oferece uma organização adequada para projetos que precisam crescer e ser mantidos por um longo período. Dessa forma, a escolha foi baseada na necessidade de uma estrutura arquitetural mais clara e detalhada.

## Porta de Persistência e Adaptador

Esse padrão de porta e adaptador se repete de forma idêntica nas quatro entidades migradas (Autor, Editora, ItemAcervo e Livro): para cada uma existe uma interface dentro do Domain e uma implementação correspondente na Infrastructure.

```csharp
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

O AutorRepository, na camada Infrastructure, é o adaptador: a implementação concreta dessa porta usando Entity Framework Core, mantida fora do domínio.

As outras três entidades seguem exatamente essa mesma estrutura, cada uma com sua própria porta e seu próprio adaptador:

IEditoraRepository (Domain/Editora) implementada por EditoraRepository (Infrastructure/Editora).
ILivroRepository (Domain/Livro) implementada por LivroRepository (Infrastructure/Livro).
IItemAcervoRepository (Domain/ItemAcervo) implementada por ItemAcervoRepository (Infrastructure/ItemAcervo).

Em nenhum desses casos a camada Application (os Use Cases) referencia diretamente o Entity Framework: todas dependem apenas da porta correspondente do Domain.

## Responsabilidades e Acoplamento

### Responsabilidade Única (SRP)

Os dois princípios abaixo se repetem nas quatro entidades migradas, embora com evidências mais claras em algumas classes do que em outras.

Responsabilidade Única (SRP)

Os Services antigos concentram numa única classe a validação de regra de negócio, todas as operações de CRUD e, em alguns casos, lógica extra de ordenação.

Na Clean Architecture, cada operação de cada entidade foi separada em sua própria classe de Use Case: CreateAutorUseCase, UseCaseCriarLivro, UseCaseCriarEditora, CreateItemAcervoUseCase, e assim por diante para as demais operações (Update, Delete, GetById...). O CreateAutorUseCase, por exemplo, faz apenas uma coisa: valida os dados do autor e delega a criação à porta de persistência.

### Inversão de Dependência (DIP)

No código antigo, o `AutorService` depende diretamente de uma classe concreta do Entity Framework. O mesmo padrão se repete em LivroService, EditoraService e ItemAcervoService: todos recebem o BibliotecaContext diretamente no construtor. Nos Use Cases novos, a dependência é sempre com a porta correspondente do Domain, nunca com o Entity Framework


Quem implementa essa porta usando Entity Framework é o adaptador `AutorRepository`, na camada `Infrastructure`. O Use Case não sabe (nem precisa saber) que existe um EF Core por trás.

Isso é confirmado também pelas referências de projeto (`.csproj`): o Domain não referencia nenhum outro projeto, nem o Entity Framework, nem nada. Ele é o centro da arquitetura; `Application` e `Infrastructure` dependem dele, nunca o contrário. Essa é a regra de dependência da Clean Architecture: as dependências sempre apontam para dentro, em direção às regras de negócio, nunca para os detalhes técnicos.

## Estrutura de pastas

```
Codigo/Biblioteca/
├── BibliotecaWeb/          
├── BibliotecaAPI/          
├── Core/                   
├── Service/                
├── Util/                   
├── Domain/                 
├── Application/            
├── Infrastructure/         
├── BibliotecaWebTests/     
└── ServiceTests/           
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
