# Incremento de arquitetura: hexagonal

## Sobre a branch

Esta branch representa o mesmo sistema da `main` com os mesmos elementos que a caracterizam como Clean Architecture, mas com o adicional que agora ela também apresenta a visão da Arquitetura Hexagonal.

## Arquitetura Hexagonal

A Arquitetura Aexagonal não tem a mesma preocupação com a organização interna de seus componentes como a Clean Architecture, mas ela desloca a atenção para como se dá a comunicação com componentes externos. Nesta arquitetura, existe a preocupação de que a comunicação com a aplicação deve ser feita por meio de portas e adaptadores de entrada e saída, que são a interface para se comunicar e a sua implementação. Assim, toda comunicação com a aplicação é feita por meio de um adaptador que faz uso de uma porta e que isso deve ser simétrico tanto na entrada como na saída.

## A implementação

A utilização do sistema como se apresenta na `main` já coloca a implementação hexagonal na metade do caminho, haja vista que já são utilizados portas e adaptadores para comunicação de persistência dos dados (uma comunicação de saída), que são os `IRepository` das entidades e suas implementações concretas. Como é exigido simetria, o ponto que falta é criação da porta de entrada para a aplicação.

A porta mencionada são as interfaces para os casos de uso, que foram feitas apenas para a entidade `Autor`. Uma vez que essas portas foram criadas, qualquer menção concreta aos casos de uso, que estão nos adaptadores, foram alteradas para fazer uso da porta.

## Sobre a abordagem neste cenário

Admitidamente, não há um ganho claro, tal qual o uso da Clean Architecture, de utilizar essa abordagem no contexto atual da aplicação. Entretanto, essa definição clara do contrato de entrada pode ser útil em outros cenários, como para a criação de bibliotecas para serem disponibilizadas a várias aplicações.
