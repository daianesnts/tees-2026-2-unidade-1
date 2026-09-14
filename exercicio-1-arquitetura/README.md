# Sistema de Biblioteca: Migração de Arquitetura

## Arquitetura Atual

### Visão Externa

A visão externa do sistema permite a observação de dois artefatos de deploy, `BibliotecaAPI` e `BibliotecaWeb`, que compartilham o mesmo código de regra de negócio (`Service`), a mesma camada de dados (`Core`), e também existe o artefato do banco de dados que é compartilhado com os dois artefatos anteriores. Essas características encaixam o sistema como um monolito, pois toda a aplicação é disponibilizada a partir de um único artefato que acessa um único banco compartilhado.

### Visão Interna

A visão interna do sistema permite enxerga como se relacionam as dependências dos componentes da aplicação. Atualmente, não existe uma separação clara de responsabilidade do ponto de vista dos componentes, pois, por exemplo, o `Core` conhece componentes externos do sistema (bancos de dados, por exemplo), o que é problemático pois ele se torna vulneráveis a mudanças nessas camadas externas.

A relação dos componentes na arquitetura atual pode ser vista da seguinte forma: a Aplicação (Web ou API) conhece a `Service`, esse conhece o `Core` e esse, por sua vez, conhece detalhes externos ao sistema, que, nesse caso, é o banco de dados. Além disso, essses componentes também conversam entre si a partir das implementações concretas, o que torna mais custoso realizar mudanças no sistema (como trocar o banco de dados utilizado) uma vez que ele crescer.

## Arquitetura nova

### Motivações da mudança

Pensando em resolver esse problemática da relação entre os componentes, foi adotado uma arquitetura a qual forçasse uma regra de dependência dos componentes que sempre apontasse para dentro do domínio, para o domínio do negócio. Nesse sentido, foi optada pela clean architecture, pois além de garantir o ponto mencionado anteriormente, também promove uma separação clara do que é regra do domínio e a regra da aplicação, concretizado com os casos de uso sendo uma camada à parte do domínio, mas que depende do domínio.

Essa escolha também visou a escrita de um código que incentivasse menos acoplamento e permitisse flexibilidade com a mudança de componentes (por exemplo, a mudança de um banco de dados para outro). Como bônus, isso torna mais fácil uma possível mudança da arquitetura monolítica do sistema para uma arquitetura de microsserviços.

### Sobre a mudança

#### O domínio

Este é o coração da arquitetura. Nesta camada, não deve haver nenhuma dependência a qualquer elementos externo. Aqui, foram definidos as entidades do negócio (neste caso, `Livro`, `Autor`, `Editora` e `ItemAcervo`). Também seriam definidas as regras de negócio aqui, mas como o `core` do código da biblioteca não continha regras de negócio associada, essas entidades ficaram anêmicas.

Nessa parte mais central é criada as interfaces (portas) utilizadas para enviar informação a serviços externos (adaptador), que necesse caso foi uma porta para comunicação com banco de dados.

Assim, o domínio foi criado de modo a ser completamente desconhecido do que existe fora dele, desconhecido até mesmo de outros detalhesd a própria aplicação.

#### A camada de casos de uso

Nesta parte (localizada em `Application`) é onde se encontram as regras da aplicação, cuja correta execução depende do conhecimento do domínio (entidades e regras de negócio). Cada caso de único corresponde a uma única funcionalidade da aplicação, que antes estava agrupada em um único serviço.

Além da separação das funcionalidades, esses casos de uso não têm conhecimento sobre como se comunicará com coisas externas (como banco de dados), mas eles sabem que existirá uma porta (a interface definida no domínio) para realizar a comunicação, então essa porta é utilizada para efetivar a comunicação. Aqui também foi definido um contrato de comunicação com camadas externas (DTOs) para que o caso de uso não vaze elementos do domínio que carregam, além de dados, as regras de negócio (embora o domínio seja anêmico por motivos mencionados anteriormente).


#### A camada de adaptadores

Este (localizado tanto em `BibliotecaWeb` e `BibiotecaAPI`) é o ponto de entrada para a aplicação, pois aqui se encontra os controllers do sistema. Devido a regra de dependência forçar um apontamento para dentro do domínio, os adaptadores conhecem os casos de uso e o domínio, mas não o contrário. Isso é interessante pois possibilita que diferentes adaptadores, como nesse caso que temos a `BibiliotecaAPI` (retorna só dados) e a `BibliotecaWeb` (renderiza páginas HTML), utilizem a mesma implementação de domínio e casos de uso, pois esses são completamente desconhecidos do que existe fora. Essa camada acaba sendo a fronteira que controla o acesso ao mundo do domínio.

#### A camada de frameworks

É a camada mais externa (localizada em `Infrastructure`, mas também em `BibliotecaWeb` e `BibliotecaAPI` devido ao ASP.NET) e que de fato tem conhecimento sobre banco de dados e frameworks web. Apesar de ser a camada mais externa, neste ponto fica evidente que há nuances quanto a separação e mapeamento das camadas da Clean Architecture às pastas do projeto. Em suma, aqui é onde detalhes concretos são implementados, então o banco e o seu adaptador foram definidos aqui.