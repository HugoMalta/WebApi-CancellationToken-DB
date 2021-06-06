# WebApi-CancellationToken-DB


<!--ts-->
* Este projeto utiliza: 
  * Asp.Net Core 5.0
  * WebApi
  * Swagger
  * Dapper
  * Sql Server (usando docker e management Azure Data Studio)
	
* O que quero demonstrar:
  * Utilização de CancellationToken em WebApi, especialmente dentro do DB.

* Situação 1:
  * Quando se usa DI, os métodos não possuem, nativamente, implementação explícita do CancellationToken. Neste caso, foi feito validação antes de "comitar" a transação.
  * Vantagem:
    - Como utiliza DI, diminuindo o número de conexões criadas, economizando preciosos response-times.
  * Desvantagem:
    - Se demorar dentro do banco de dados, o cancelamento da request ocorrerá após este tempo. Se houver um bloqueio, demorará a liberar conexão e, consequentemente, o rollback.
		
* Situação 2:
  * Quando se instancia os objetos dentro do método, cria-se se a conexão na mão. Contudo com todo suporte para utilização do CancellationToken.
  * Vantagem:
		- Ao cancelar a request, de forma instantânea será interrompido a conexão com banco de dados e levantado exception.
  * Desvantagem:
		- Criação de conexão de forma arcaica, "na munheca", muitas vezes inviável considerando muitas chamadas por segundo porque a conexão demanda preciosos response-times.
			
* Observação:
  * A query executada possui uma instrução de delay dentro do DB, para demonstrar mais facilmente a possibilidade de cancelar a request enquanto estiver dentro do DB.
<!--te-->
