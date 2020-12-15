// Definição se apresenta no console o passo a passo ou se só executa diretamente
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace GEO_canonico
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
    public class GEO_canonico
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
            A função objetivo é a função fitness do algoritmo. Ela recebe como parâmetro a população de 
            ... bits, calcula o fenótipo de cada variável de projeto e calcula o valor da função objetivo.
        */
        public static double funcao_objetivo(List<bool> populacao_de_bits, int n_variaveis_projeto, int bits_por_variavel_projeto, double function_min, double function_max){
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

            //============================================================
            // Calcula a função de Griewank
            //============================================================
            
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
            
#if DEBUG_FUNCTION
            Console.WriteLine("fx: " + fx);
#endif

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








//         public static List<bool> GEOvar_ordena_e_flipa_um_bit(List<bool> populacao_de_bits, List<double> deltaV_fitness, List<int> bits, int n_variaveis_projeto, int bits_por_variavel, int tamanho_populacao_bits, double tao){
//============================================================
//             // Ordena os bits por variável conforme os indices fitness
//============================================================

//             List<bool> nova_populacao = new List<bool>();

// #if DEBUG_CONSOLE
//             Console.WriteLine("Antes da ordenação");
//             for (int i=0; i<deltaV_fitness.Count; i++){
//                 Console.WriteLine(i + ": Bit: " + bits[i] + " | deltaV_fitness: " + deltaV_fitness[i]);
//             }
// #endif

//             // Para cada variável de projeto
//             // Percorre bit a bit para cada variável
            
//             List<bool> lista_temporaria = new List<bool>();

//             for(int i=0; i<tamanho_populacao_bits; i++){
//                 lista_temporaria.Add( populacao_de_bits[i] );

                    // Transforma o genótipo de cada variável em uma string para depois converter para decimal
                    // for (int i=0; i<n_variaveis_projeto; i++){
                    //     // Cria string representando os bits da variável
                    //     string fenotipo_xi = "";
                        
                    //     // Percorre o número de bits de cada variável de projeto
                    //     for(int c = bits_por_variavel_projeto*i; c < bits_por_variavel_projeto*(i+1); c++){
                    //         // Se o bit for true, concatena "1", senão, "0"
                    //         fenotipo_xi += (cromossomo[c] ? "1" : "0");
                    //     }


//                 // Se o iterador estiver 1 antes do tamanho da variável, obtém a lista
//                 if (i % (bits_por_variavel-1) == 0){
                    
//                     // Ordena a lista
                
//                     // Transforma a população e os fitness para array para poder ordenar
//                     var deltaV_fitness_array = deltaV_fitness.ToArray();
//                     var bits_array = bits.ToArray();
                    
//                     // Ordena a população com base no fitness
//                     Array.Sort(deltaV_fitness_array, bits_array);
                    
//                     // Converte novamente os arrays para listas
//                     deltaV_fitness = deltaV_fitness_array.ToList();
//                     bits = bits_array.ToList();



//                 }



//             }


// #if DEBUG_CONSOLE
//             Console.WriteLine("Depois da ordenação");
//             for (int i=0; i<deltaV_fitness.Count; i++){
//                 Console.WriteLine(i + ": Bit: " + bits[i] + " | deltaV_fitness: " + deltaV_fitness[i]);
//             }
// #endif
            
//============================================================
//             // Flipa um bit com probabilidade Pk
//============================================================

//             // Verifica as probabilidades até que um bit seja ummutado
//             bool bit_flipado = false;
//             while (!bit_flipado){

//                 // Gera um número aleatório com distribuição uniforme
//                 double ALE = random.NextDouble();

//                 // k é o índice do array ordenado
//                 int k = random.Next(1, tamanho_populacao_bits+1);
                
//                 // Com probabilidade Pk => k^(-tao)
//                 double Pk = Math.Pow(k, -tao);

//                 // k precisa ser de 1 a N, mas aqui nos índices começa em 0
//                 k -= 1;
// #if DEBUG_CONSOLE
//                 // Se o Pk é maior ou igual ao aleatório, então flipa o bit
//                 Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+bits[k]+" com Pk "+Pk+" >= ALE "+ALE);
// #endif
//                 if (Pk >= ALE){
// #if DEBUG_CONSOLE
//                     Console.WriteLine("Antes de flipar:");
//                     ApresentaCromossomoBool(populacao_de_bits);
// #endif
//                     // Flipa o bit
//                     populacao_de_bits[ bits[k] ] = !populacao_de_bits[ bits[k] ];
// #if DEBUG_CONSOLE
//                     Console.WriteLine("Flipando o indice " + k + ", que é o bit " + bits[k]);
// #endif                  
//                     // Atualiza que o bit foi flipado
//                     bit_flipado = true;
//                     break;
                    
//                 }
//             }

//             // Retorna a nova população de bits
//             return populacao_de_bits;
//         }
      





        /*
            A função GEO é a função principal do código. Aqui nesse bloco toda a lógica implementada para os algoritmos
            GEOcanonico e GEOvar são implementadas, como a geração da população, o controle do critério de parada e a
            avaliação do flip de cada bit.
            A função retorna o melhor f(x) da execução.
        */
        public static double GEO(int n_variaveis_projeto, int bits_por_variavel, double function_min, double function_max, double tao, int criterio_parada_nro_avaliacoes_funcao, List<int> NFOBs){            
            
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
            int tamanho_populacao_bits = n_variaveis_projeto * bits_por_variavel;
            
            // Inicializa a população de bits como uma lista de bits (bool)
            List<bool> populacao_de_bits = new List<bool>();
            
            // Gera um bit para cada posição da população de bits
            for (int i=0; i<tamanho_populacao_bits; ++i){
                populacao_de_bits.Add( (random.Next(0, 2)==1) ? true : false );
            }

            //============================================================
            // Calcula o primeiro Valor Referência
            //============================================================

            melhor_fx = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel, function_min, function_max);

#if DEBUG_CONSOLE    
            Console.WriteLine("População de bits gerado:");
            ApresentaCromossomoBool(populacao_de_bits);        
            Console.WriteLine("Melhor fx: " + melhor_fx);
#endif

            //============================================================
            // Iterações
            //============================================================
            
            // Executa o algoritmos até que o critério de parada (número de avaliações na FO) seja atingido
            while (NFOB < criterio_parada_nro_avaliacoes_funcao){
                   
#if DEBUG_CONSOLE
                Console.WriteLine("População de Bits começo while:");
                ApresentaCromossomoBool(populacao_de_bits);
                double teste = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel,function_min, function_max);
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
                    double fx = funcao_objetivo(populacao_de_bits_flipado, n_variaveis_projeto, bits_por_variavel,function_min, function_max);

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
                // Ordena os bits e flipa apenas um deles, atualizando a população
                //============================================================
                
                populacao_de_bits = GEO_ordena_e_flipa_um_bit(populacao_de_bits, lista_informacoes_mutacao, tamanho_populacao_bits, tao);

                // Calcula a fitness da nova população de bits
                double fitness_populacao_de_bits = funcao_objetivo(populacao_de_bits, n_variaveis_projeto, bits_por_variavel, function_min, function_max);

                //============================================================
                // Atualiza, se possível, o valor (Valor Refrência)
                //============================================================

                // Se essa fitness for a menor que a melhor, atualiza a melhor da história
                if (fitness_populacao_de_bits < melhor_fx){
                    melhor_fx = fitness_populacao_de_bits;
                    // Console.WriteLine("Atualizou melhor "+melhor_fx+" em NFOB "+NFOB);
                }

                // Se o NFOB for algum da lista para mostrar, mostra a melhor fitness até o momento
                if (NFOB > NFOBs[0]){
                    Console.WriteLine("Fitness NFOB " + NFOB + ": " + melhor_fx);
                    NFOBs.RemoveAt(0);
				}
            }

            // Ao fim da execução, retorna a melhor fitness
            return melhor_fx;
        }

        

       
       /*
            A função main é a função que invoca a execução do GEO. Aqui nesse bloco são definidos os parâmetros de 
            execução do algoritmo e o algoritmo é executado.
        */
        public static void Main(string[] args){
            
            //============================================================
            // Definições de parâmetros
            //============================================================
            
            // Parâmetros de execução do algoritmo
            const int bits_por_variavel = 10;
            const int n_variaveis_projeto = 4;
            const double function_min = -600.0;
            const double function_max = 600.0;
            
            // Se o tao é alto, é mais determinístico
            // Se o tao é baixo, é mais estocástico
            const double tao = 0.75;
            
            // Define o critério de parada com o número de avaliações da NFOBs
            const int criterio_parada_nro_avaliacoes_funcao = 200000;
            

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            
            List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            //============================================================
            // Execução do algoritmo
            //============================================================

            // Inicializa o temporizador
            var total_watch = System.Diagnostics.Stopwatch.StartNew();

            // Executa o GEO e recebe como retorno a melhor fitness da execução
            double melhor_fx_geral = GEO(n_variaveis_projeto, bits_por_variavel, function_min, function_max, tao, criterio_parada_nro_avaliacoes_funcao, NFOBs_desejados);

            // Para o temporizador
            total_watch.Stop();

            // Calcula o tempo de execução
            var elapsedMs = total_watch.ElapsedMilliseconds;
            Console.WriteLine("Tempo total de execução: " + elapsedMs/1000.0 + " segundos");
        }
    }
}