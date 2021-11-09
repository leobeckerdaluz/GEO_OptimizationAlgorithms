# Estudo para Aumento de Performance de Versão Adaptativa do Algoritmo GEO (A-GEO) e sua aplicação no Projeto Conceitual de Sistemas Espaciais

## Objetivos:
- Implementar o mecanismo de controle de parâmetros do A-GEO no GEOvar, uma variante do GEO, verificando essa implementação através de experimentos numéricos utilizando um conjunto de funções teste; 
- Alterar a codificação das variáveis de projeto do A-GEO de binária para real, eliminando os erros de precisão de cada uma das variáveis de projeto, não sendo mais necessária a definição da quantidade de bits que codifica cada variável; 
- Estudar o modo como as variáveis de projeto são perturbadas na codificação real; 
- Implementar um mecanismo de controle para o parâmetro σ, buscando elaborar um algoritmo amigável ao usuário, sem a necessidade de ajuste de parâmetros e com alta performance; 
- Investigar o mecanismo de controle do parâmetro τ proposto para o A-GEO, o que pode indicar outras maneiras de controlar o parâmetro durante a busca; 
- Aplicar o algoritmo aprimorado na busca pela automatização da criação de soluções em um projeto conceitual de sistemas espaciais;

_______________________________________

## 1. Algoritmos

### Binários:
- **GEO_BINARIO.cs:** Algoritmo GEO canônico binário. Este algoritmo é base para todos os outros algoritmos com codificação binária.
- **GEOvar_BINARIO.cs:** Algoritmo GEOvar binário. Possui como base o algoritmo GEO_BINARIO. O único método sobreescrito é o que ordena e confirma a mutação dos bits, que ocorre de forma diferente do GEO canônico.
- **AGEOs_BINARIO.cs:** Algoritmos A-GEO1 e A-GEO2 binário. A escolha entre o 1 e o 2 é realizada através de um parâmetro de entrada. Este algoritmo possui como base o algoritmo GEO_BINARIO. O único método sobreescrito é a realização da mutação do tau.
- **AGEOsvar_BINARIO.cs:** Algoritmos A-GEO1var e A-GEO2var binário. A escolha entre o 1 e o 2 é realizada através de um parâmetro de entrada. Este algoritmo possui como base o algoritmo GEOvar_BINARIO. O único método sobreescrito é a realização da mutação do tau.

### Reais:
- **GEO_REAL1.cs:** ...
- **GEO_REAL2.cs:** ...