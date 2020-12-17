﻿// Definição se apresenta no console o passo a passo ou se só executa diretamente
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION
// #define MOSTRAR_NFOBS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace GEO
{
    /*
        Classe para armazenar as informações da mutação de um bit.
        Para cada bit, é armazenado o delta fitness caso mute e o indice do bit na população
    */
    public class BitVerificado{
        public double delta_fitness { get; set; }
        public int indice_bit_mutado { get; set; }
    }







    /*
        Classe principal contendo os algoritmos GEO
    */
    public class GEO
    {
        // Inicializa a variável global para a genração de números aleatórios
        public static Random random = new Random();
      






        /*
            Função que somente é executada no modo DEBUG. Ela serve para printar um cromossomo na tela
        */
        public static void ApresentaCromossomoBool(List<bool> boolarray){
            foreach(bool bit in boolarray){
                Console.Write( (bit ? "1" : "0") );
            }
            Console.WriteLine("");
        } 
       






        /*
            A função para calcula fenótipo de variáveis tem por objetivo percorrer toda a população de bits
            e calcular o fenótipo de cada uma das variáveis de projeto.
            A função retorna uma lista contendo o fenótipo de cada variável
        */
        public static List<double> calcula_fenotipos_variaveis(List<bool> populacao_de_bits, int n_variaveis_projeto, int bits_por_variavel_projeto, double function_min, double function_max){
            //============================================================
            // Calcula o fenótipo para cada variável de projeto
            //============================================================

            // Cria a lista que irá conter o fenótipo de cada variável de projeto
            List<double> fenotipo_variaveis_projeto = new List<double>();
            
            // Transforma o genótipo de cada variável em uma string para depois converter para decimal
            for (int i=0; i<n_variaveis_projeto; i++){
                // Cria string representando os bits da variável
                string fenotipo_xi = "";
                
                // Percorre o número de bits de cada variável de projeto
                for(int c = bits_por_variavel_projeto*i; c < bits_por_variavel_projeto*(i+1); c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    fenotipo_xi += (populacao_de_bits[c] ? "1" : "0");
                }

                // Converte essa string de bits para inteiro
                int variavel_convertida = Convert.ToInt32(fenotipo_xi, 2);

                // Mapeia o inteiro entre o intervalo mínimo e máximo da função
                // 0 --------- min
                // 2^bits ---- max
                // binario --- x
                // (max-min) / (2^bits - 0) ======> Variação de valor por bit
                // min + [(max-min) / (2^bits - 0)] * binario
                double fenotipo_variavel_projeto = function_min + ((function_max - function_min) * variavel_convertida / (Math.Pow(2, bits_por_variavel_projeto) - 1));

#if DEBUG_FUNCTION
                Console.WriteLine("Fenótipo " + i + ": " + fenotipo_xi);
                Console.WriteLine("variavel_convertida : " + variavel_convertida);
                Console.WriteLine("fenotipo_variavel_projeto : " + fenotipo_variavel_projeto);
#endif

                // Adiciona o fenótipo da variável na lista de fenótipos
                fenotipo_variaveis_projeto.Add(fenotipo_variavel_projeto);
            }

            // Retorna a lista dos fenótipos
            return fenotipo_variaveis_projeto;
        }







        /*
            A função DeJong#3 recebe como parâmetro a lista contendo o fenótipo de cada
            variável de projeto e calcula o valor da função.
        */
        public static double funcao_DeJong3_inteiro(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;

            // Laço para os somatórios
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                // Arredonda para o inteiro mais próximo
                laco_somatorio += Math.Round(fenotipo_variaveis_projeto[i], 0);
            }

            // Retorna o valor de f(x), que é o somatório
            return laco_somatorio;
        }
        
        




        
        /*
            A função de rosenbrock recebe como parâmetro a lista contendo o fenótipo de cada
            variável de projeto e calcula o valor da função.
        */
        public static double funcao_rosenbrock(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;

            // Laço para os somatórios
            for(int i=0; i<fenotipo_variaveis_projeto.Count-1; i++){
                // Obtém o valor da variável atual
                double Xi = fenotipo_variaveis_projeto[i];
                // Obtém o valor da próxima variável
                double Xi1 = fenotipo_variaveis_projeto[i+1];
                
                double primeira_parte = 100 * Math.Pow( (Math.Pow(Xi,2) - Xi1), 2 );
                // double primeira_parte = 100 * Math.Pow( (Xi1 - Math.Pow(Xi,2)), 2 );
                // double segunda_parte = Math.Pow( (1 + Xi), 2 );
                double segunda_parte = Math.Pow( (1 - Xi), 2 );
                double colchetes = primeira_parte + segunda_parte;
                laco_somatorio += colchetes;
            }

            // Retorna o valor de f(x), que é o somatório
            return laco_somatorio;
        }
        
        
        



        
        /*
            A função de griewank recebe como parâmetro a lista contendo o fenótipo de cada
            variável de projeto e calcula o valor da função.
        */
        public static double funcao_griewank(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;
            double laco_produto = 1;

            // Laço para os somatórios e pi
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                laco_somatorio += Math.Pow(fenotipo_variaveis_projeto[i], 2);
                // laco_produto *= Math.Cos( Math.PI * fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
                laco_produto *= Math.Cos( fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
            }

            // Expressão final de f(x)
            double fx = (1 + laco_somatorio/4000.0 - laco_produto);

            // Retorna o valor de f(x)
            return fx;
        }







        /*
            A função objetivo é a função fitness do algoritmo. Ela invoca os métodos para calcular
            o fenótipo de cada variável de projeto e, posteriormente, calcula o valor fitness.
        */
        public static double funcao_objetivo(List<bool> populacao_de_bits, int n_variaveis_projeto, int bits_por_variavel_projeto, double function_min, double function_max){
            // Calcula o fenótipo para cada variável de projeto
            List<double> fenotipo_variaveis_projeto = calcula_fenotipos_variaveis(populacao_de_bits, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max);

            // Calcula o valor da função objetivo
            // double fx = funcao_griewank(fenotipo_variaveis_projeto);
            double fx = funcao_rosenbrock(fenotipo_variaveis_projeto);
            // double fx = funcao_DeJong3_inteiro(fenotipo_variaveis_projeto);

            // Retorna o valor fitness
            return fx;
        }







        /*
            A função para ordenar e flipar um bit tem como objetivo ordenar toda a população de bits e escolher, com 
            probabilidade normalmente distribuida, um bit da população de bits para mutar.
            A função retorna a nova população de bits com o bit mutado.
        */
        public static List<bool> GEO_ordena_e_flipa_um_bit(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, int tamanho_populacao_bits, double tao){
            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

#if DEBUG_CONSOLE
            Console.WriteLine("Antes da ordenação");
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + " | deltaV_fitness: " + lista_informacoes_mutacao[i].delta_fitness);
            }
#endif

            // Ordena os bits
            lista_informacoes_mutacao.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.delta_fitness.CompareTo(b2.delta_fitness); });
           
#if DEBUG_CONSOLE
            Console.WriteLine("Depois da ordenação");
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + " | deltaV_fitness: " + lista_informacoes_mutacao[i].delta_fitness);
            }
#endif
            
            //============================================================
            // Flipa um bit com probabilidade Pk
            //============================================================

            // Verifica as probabilidades até que um bit seja ummutado
            bool bit_flipado = false;
            while (!bit_flipado){

                // Gera um número aleatório com distribuição uniforme
                double ALE = random.NextDouble();

                // k é o índice da população de bits ordenada
                int k = random.Next(1, tamanho_populacao_bits+1);
                
                // Probabilidade Pk => k^(-tao)
                double Pk = Math.Pow(k, -tao);

                // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                k -= 1;

#if DEBUG_CONSOLE
                Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+lista_informacoes_mutacao[k].indice_bit_mutado+" com Pk "+Pk+" >= ALE "+ALE);
#endif

                // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                if (Pk >= ALE){

#if DEBUG_CONSOLE
                    Console.WriteLine("Antes de flipar:");
                    ApresentaCromossomoBool(populacao_de_bits);
                    Console.WriteLine("Flipando o indice " + k + ", que é o bit " + lista_informacoes_mutacao[k].indice_bit_mutado);
#endif

                    // Flipa o bit
                    populacao_de_bits[ lista_informacoes_mutacao[k].indice_bit_mutado ] = !populacao_de_bits[ lista_informacoes_mutacao[k].indice_bit_mutado ];

                    // Atualiza que o bit foi flipado
                    bit_flipado = true;
                    break;
                }
            }

            // Retorna a nova população de bits
            return populacao_de_bits;
        }







        /*
            A função para ordenar e flipar um bit por variável tem como objetivo ordenar todos os bits
            de cada variável e, posteriormente, escolher com probabilidade normalmente distribuida, um 
            bit por variável para mutar
            A função retorna a nova população de bits com os bits mutados.
        */
        public static List<bool> GEOvar_ordena_e_flipa_bits(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, int n_variaveis_projeto, int bits_por_variavel_projeto, int tamanho_populacao_bits, double tao){
            
            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

            List<BitVerificado> depois_ordenar = new List<BitVerificado>();


#if DEBUG_CONSOLE
            Console.WriteLine("Antes da ordenação:");
            Console.WriteLine(lista_informacoes_mutacao.Count);
            ApresentaCromossomoBool(populacao_de_bits);
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + " | deltaV_fitness: " + lista_informacoes_mutacao[i].delta_fitness);
            }
#endif

            // Percorre cada variável de projeto para ordenar os bits e escolher o bit para filpar
            for (int i=0; i<n_variaveis_projeto; i++){
                // Cria uma lista com as informações de mutação de cada bit da variável
                List<BitVerificado> lista_informacoes_bits_variavel = new List<BitVerificado>();
                
                // Percorre o número de bits de cada variável de projeto
                for(int c = bits_por_variavel_projeto*i; c < bits_por_variavel_projeto*(i+1); c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    lista_informacoes_bits_variavel.Add( lista_informacoes_mutacao[c] );
                }

                // Ordena esses bits da variável
                lista_informacoes_bits_variavel.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.delta_fitness.CompareTo(b2.delta_fitness); });

                // Coloca esses bits ordenado na lista geral
                foreach (BitVerificado bit in lista_informacoes_bits_variavel){
                    depois_ordenar.Add(bit);
                }


                //============================================================
                // Flipa um bit da variável com probabilidade Pk
                //============================================================

                // Verifica as probabilidades até que um bit seja ummutado
                bool bit_flipado = false;
                while (!bit_flipado){

                    // Gera um número aleatório com distribuição uniforme
                    double ALE = random.NextDouble();

                    // k é o índice da população de bits ordenada
                    int k = random.Next(1, bits_por_variavel_projeto+1);
                    
                    // Probabilidade Pk => k^(-tao)
                    double Pk = Math.Pow(k, -tao);

                    // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                    k -= 1;

    #if DEBUG_CONSOLE
                    Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+lista_informacoes_bits_variavel[k].indice_bit_mutado+" com Pk "+Pk+" >= ALE "+ALE);
    #endif

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){

    #if DEBUG_CONSOLE
                        Console.WriteLine("Antes de flipar:");
                        ApresentaCromossomoBool(populacao_de_bits);
                        Console.WriteLine("Flipando o indice " + k + ", que é o bit " + lista_informacoes_bits_variavel[k].indice_bit_mutado);
    #endif

                        // Flipa o bit
                        populacao_de_bits[ lista_informacoes_bits_variavel[k].indice_bit_mutado ] = !populacao_de_bits[ lista_informacoes_bits_variavel[k].indice_bit_mutado ];

                        // Atualiza que o bit foi flipado
                        bit_flipado = true;
                        break;
                    }
                }
            }

#if DEBUG_CONSOLE
            Console.WriteLine("Depois da ordenação:");
            ApresentaCromossomoBool(populacao_de_bits);
            for (int m=0; m<depois_ordenar.Count; m++){
                Console.WriteLine(m + ": Bit: " + depois_ordenar[m].indice_bit_mutado + " | deltaV_fitness: " + depois_ordenar[m].delta_fitness);
            }
#endif

            // Retorna a nova população de bits
            return populacao_de_bits;
        }














        /*
            A função GEO é a função principal do código. Aqui nesse bloco toda a lógica implementada para os algoritmos
            GEOcanonico e GEOvar são implementadas, como a geração da população, o controle do critério de parada e a
            avaliação do flip de cada bit.
            A função retorna o melhor f(x) da execução.
        */
        public static double GEO_algorithm(int tipo_GEO, int n_variaveis_projeto, int bits_por_variavel_projeto, double function_min, double function_max, double tao, double valor_criterio_parada, List<int> NFOBs, double fx_esperado){            
            
            //============================================================
            // Inicializa algumas variáveis de controle do algoritmo
            //============================================================
            
            // Número de avaliações da função objetivo
            int NFOB = 0;

            // Melhor fitness até o momento. Como é minimização, começa com o maior valor possível
            double melhor_fx = function_max;
            
            //============================================================
            // Geração da População de Bits Inicial
            //============================================================
            
            // Define o tamanho da população de bits
            int tamanho_populacao_bits = n_variaveis_projeto * bits_por_variavel_projeto;
            
            // Inicializa a população de bits como uma lista de bits (bool)
            List<bool> populacao_de_bits = new List<bool>();
            
            // Gera um bit para cada posição da população de bits
            for (int i=0; i<tamanho_populacao_bits; ++i){
                populacao_de_bits.Add( (random.Next(0, 2)==1) ? true : false );
            }

            //============================================================
            // Calcula o primeiro Valor Referência
            //============================================================

            melhor_fx = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max);

#if DEBUG_CONSOLE    
            Console.WriteLine("População de bits gerado:");
            ApresentaCromossomoBool(populacao_de_bits);        
            Console.WriteLine("Melhor fx: " + melhor_fx);
#endif

            //============================================================
            // Iterações
            //============================================================
            
            // Executa o algoritmos até que o critério de parada (número de avaliações na FO) seja atingido
            
            while (NFOB < valor_criterio_parada){
            // while ( Math.Abs(Math.Abs(melhor_fx) - Math.Abs(fx_esperado)) > valor_criterio_parada ){
                // Console.WriteLine(Math.Abs(melhor_fx) + " - " Math.Abs(fx_esperado) ' in ABS > 0,01')
#if DEBUG_CONSOLE
                Console.WriteLine("População de Bits começo while:");
                ApresentaCromossomoBool(populacao_de_bits);
                double teste = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel_projeto,function_min, function_max);
                Console.WriteLine("Fx = " + teste);
                Console.WriteLine("Fx melhor = " + melhor_fx);
#endif

                //============================================================
                // Avalia o flip para cada bit
                //============================================================

                // Cria uma lista contendo as informações sobre mutar um bit
                List<BitVerificado> lista_informacoes_mutacao = new List<BitVerificado>();

                // Cria uma cópia da população de bits, flipa o bit e verifica a fitness
                for (int i=0; i<populacao_de_bits.Count; i++){
                    
                    // Cria uma cópia da população de bits
                    List<bool> populacao_de_bits_flipado = new List<bool>(populacao_de_bits);
                    
                    // Flipa o i-ésimo bit
                    populacao_de_bits_flipado[i] = !populacao_de_bits_flipado[i];

                    // Calcula a fitness da populacao_de_bits com o bit flipado
                    double fx = funcao_objetivo(populacao_de_bits_flipado, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max);

                    // Calcula o ganho ou perda de flipar
                    double deltaV = fx - melhor_fx;

#if DEBUG_CONSOLE
                    Console.WriteLine("DELTAV " + deltaV + " = fx " + fx + " - ref " + melhor_fx);
#endif
                    
                    // Armazena as informações dessa mutação do bit na lista de informações
                    BitVerificado informacoes_bit = new BitVerificado();
                    informacoes_bit.delta_fitness = deltaV;
                    informacoes_bit.indice_bit_mutado = i;
                    lista_informacoes_mutacao.Add(informacoes_bit);
                    
                    // Incrementa o número de avaliações da função objetivo
                    NFOB++;
                }
                
                //============================================================
                // Ordena os bits e flipa, atualizando a população
                //============================================================
                
                //GEOcanonico
                if (tipo_GEO == 0){ 
                    populacao_de_bits = GEO_ordena_e_flipa_um_bit(populacao_de_bits, lista_informacoes_mutacao, tamanho_populacao_bits, tao);
                }
                //GEOvar
                else if (tipo_GEO == 1){
                    populacao_de_bits = GEOvar_ordena_e_flipa_bits(populacao_de_bits, lista_informacoes_mutacao, n_variaveis_projeto, bits_por_variavel_projeto, tamanho_populacao_bits, tao);
                }


                // Calcula a fitness da nova população de bits
                double fitness_populacao_de_bits = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max);

                //============================================================
                // Atualiza, se possível, o valor (Valor Refrência)
                //============================================================

                // Se essa fitness for a menor que a melhor, atualiza a melhor da história
                if (fitness_populacao_de_bits < melhor_fx){
                    melhor_fx = fitness_populacao_de_bits;
                    // Console.WriteLine("Atualizou melhor "+melhor_fx+" em NFOB "+NFOB);
                }

#if MOSTRAR_NFOBS
                // Se o NFOB for algum da lista para mostrar, mostra a melhor fitness até o momento
                if (NFOB > NFOBs[0]){
                    Console.WriteLine("Fitness NFOB " + NFOB + ": " + melhor_fx);
                    NFOBs.RemoveAt(0);
				}
#endif

            }
            // Console.WriteLine("Acabou ==> Abs(" + melhor_fx + " - " + fx_esperado + ") <= " + valor_criterio_parada);

            // Ao fim da execução, retorna a melhor fitness
            return melhor_fx;
            // return NFOB;
        }

        

       



        /*
            A função main é a função que invoca a execução do GEO. Aqui nesse bloco são definidos os parâmetros de 
            execução do algoritmo e o algoritmo é executado.
        */
        public static void Main(string[] args){
            // // Parâmetros de execução do algoritmo
            // const int bits_por_variavel_projeto = 10;
            // const int n_variaveis_projeto = 2;
            // const double function_min = -2.048;
            // const double function_max = 2.048;
            // // Se o TAO é alto, é mais determinístico. Se o TAO é baixo, é mais estocástico
            // const double tao = 0.75;
            // // Define o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 
            // const int tipo_GEO = 0; 
            // // Define o critério de parada com o número de avaliações da NFOBs
            // const int criterio_parada_nro_avaliacoes_funcao = 200000;
            // double fx_esperado = 0;

            // // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            
            // // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            // //============================================================
            // // Execução do algoritmo
            // //============================================================

            // // Executa o GEO e recebe como retorno a melhor fitness da execução
            // double melhor_fx_geral = GEO_algorithm(tipo_GEO, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, tao, criterio_parada_nro_avaliacoes_funcao, NFOBs_desejados, fx_esperado);

            



            // // Cria lista para armazenar os valores fitness a cada NFOB desejado
            // List<List<double>> todas_execucoes_SGA_NFOB = new List<List<double>>();

            // // Apresenta o melhor resultado
            // Console.WriteLine("Execução " + i + ": " + SGA_bests_NFOB[SGA_bests_NFOB.Count - 1]);
            // // Adiciona na lista de execuções a execução atual
            // todas_execucoes_SGA_NFOB.Add(SGA_bests_NFOB);

            // // Para cada probabilidade de crossover, executa o algoritmo
            // List<double> valores_TAO_F3 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};
            


            // // ================================================================
            // // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F2
            // // ================================================================

            // // Parâmetros de execução do algoritmo
            // const int bits_por_variavel_projeto = 13;
            // const int n_variaveis_projeto = 2;
            // const double fx_esperado = 0;
            // const double function_min = -2.048;
            // const double function_max = 2.048;
            // // Define o critério de parada com o número de avaliações da NFOBs
            // const double valor_criterio_parada = 0.001;
            // List<double> valores_TAO_F2 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};
            // List<int> NFOBs_desejados = new List<int>(){400000};
            
            // foreach (double TAO in valores_TAO_F2){
            //     Console.WriteLine("===> TAO: " + TAO + "   |   GEOcan / GEOvar");
                
            //     // Executa o SGA por 50 vezes
            //     double somatorio_nro_avaliacoes_FO_encontrar_global_GEO = 0;
            //     double somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar = 0;

            //     // Executa o GEO e o GEOvar por 5 vezes
            //     for(int i=0; i<50; i++){
            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_nro_avaliacoes_FO_encontrar_global_GEO += GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);
            //     }

            //     // Calcula a média dos melhores f(x) pra esse TAO
            //     double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
            //     double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

            //     string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
            //     string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
            //     Console.WriteLine(string_media_nro_avaliacoes_GEO);
            //     Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            // }
            
            
            
            // // ================================================================
            // // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F3
            // // ================================================================

            // // Parâmetros de execução do algoritmo
            // const int bits_por_variavel_projeto = 11;
            // const int n_variaveis_projeto = 5;
            // const double fx_esperado = -25;
            // const double function_min = -5.12;
            // const double function_max = 5.12;
            // // Define o critério de parada com o número de avaliações da NFOBs
            // const double valor_criterio_parada = 0.01;
            //  List<double> valores_TAO_F3 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};
            // // List<double> valores_TAO_F2 = new List<double>(){3};
            // List<int> NFOBs_desejados = new List<int>(){400000};
            
            // foreach (double TAO in valores_TAO_F3){
            //     Console.WriteLine("===> TAO: " + TAO + "   |   GEOcan / GEOvar");
                
            //     // Executa o SGA por 50 vezes
            //     double somatorio_nro_avaliacoes_FO_encontrar_global_GEO = 0;
            //     double somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar = 0;

            //     // Executa o GEO e o GEOvar por 5 vezes
            //     for(int i=0; i<50; i++){
            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_nro_avaliacoes_FO_encontrar_global_GEO += GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);
            //     }

            //     // Calcula a média dos melhores f(x) pra esse TAO
            //     double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
            //     double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

            //     string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
            //     string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
            //     Console.WriteLine(string_media_nro_avaliacoes_GEO);
            //     Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            // }






            // // ================================================================
            // // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F5
            // // ================================================================

            // // Parâmetros de execução do algoritmo
            // const int bits_por_variavel_projeto = 14;
            // const int n_variaveis_projeto = 10;
            // const double fx_esperado = 0;
            // const double function_min = -600.0;
            // const double function_max = 600.0;
            // // Define o critério de parada com o número de avaliações da NFOBs
            // const double valor_criterio_parada = 100000;
            // List<double> valores_TAO_F5 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
            // // List<double> valores_TAO_F2 = new List<double>(){3};
            // List<int> NFOBs_desejados = new List<int>(){400000};
            
            // foreach (double TAO in valores_TAO_F5){
            //     Console.WriteLine("===> TAO: " + TAO + "   |   GEOcan / GEOvar");
                
            //     // Executa o SGA por 50 vezes
            //     double somatorio_melhores_GEO = 0;
            //     double somatorio_melhores_GEOvar = 0;

            //     // Executa o GEO e o GEOvar por 5 vezes
            //     for(int i=0; i<50; i++){
            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_melhores_GEO += GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            //         // Executa o GEO e recebe como retorno a melhor fitness da execução
            //         somatorio_melhores_GEOvar += GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, TAO, valor_criterio_parada, NFOBs_desejados, fx_esperado);
            //     }

            //     // Calcula a média dos melhores f(x) pra esse TAO
            //     double media_melhor_fx_GEO = somatorio_melhores_GEO / 50;
            //     double media_melhor_fx_GEOvar = somatorio_melhores_GEOvar / 50;

            //     string string_media_melhor_fx_GEO = (media_melhor_fx_GEO.ToString()).Replace('.',',');
            //     string string_media_melhor_fx_GEOvar = (media_melhor_fx_GEOvar.ToString()).Replace('.',',');
		        
            //     Console.WriteLine(string_media_melhor_fx_GEO);
            //     Console.WriteLine(string_media_melhor_fx_GEOvar);
            // }






            // ================================================================
            // 50 EXECUÇÕES para TAO=1 GEO e TAO=1,25 GEOvar para F2
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int bits_por_variavel_projeto = 13;
            const int n_variaveis_projeto = 2;
            const double fx_esperado = 0;
            const double function_min = -2.048;
            const double function_max = 2.048;
            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 15500;
            List<int> NFOBs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000,10500,11000,11500,12000,12500,13000,13500,14000,14500,15000};
            
            const double tao_GEO = 1.0;
            const double tao_GEOvar = 1.25;
            Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            // Executa o SGA por 50 vezes
            double somatorio_melhor_fx_GEO = 0;
            double somatorio_melhor_fx_GEOvar = 0;

            List<double> NFOBs_all_results_GEO = new List<double>();
            List<double> NFOBs_all_results_GEOvar = new List<double>();
            
            // Executa o GEO e o GEOvar por 5 vezes
            for(int i=0; i<50; i++){
                // Executa o GEO e recebe como retorno a melhor fitness da execução
                List<double> NFOBs_results_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

                NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

                // Executa o GEO e recebe como retorno a melhor fitness da execução
                List<double> NFOBs_results_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, function_min, function_max, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado);
                
                NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            }

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEO){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEO.Count;
                Console.WriteLine("Média do NFOB " + i + ": " + media);
            }

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEOvar){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEOvar.Count;
                Console.WriteLine("Média do NFOB " + i + ": " + media);
            }
            
            // Calcula a média dos melhores f(x) pra esse TAO
            double media_somatorio_melhor_fx_GEO = somatorio_melhor_fx_GEO / 50;
            double media_somatorio_melhor_fx_GEOvar = somatorio_melhor_fx_GEOvar / 50;

            string string_media_somatorio_melhor_fx_GEO = (media_somatorio_melhor_fx_GEO.ToString()).Replace('.',',');
            string string_media_somatorio_melhor_fx_GEOvar = (media_somatorio_melhor_fx_GEOvar.ToString()).Replace('.',',');
            
            Console.WriteLine(string_media_somatorio_melhor_fx_GEO);
            Console.WriteLine(string_media_somatorio_melhor_fx_GEOvar);

        }
    }
}