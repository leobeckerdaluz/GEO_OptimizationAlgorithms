// #define DEBUGT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace GEO_canonico
{
    public class GEO_canonico
    {
        // Inicializa as variáveis globais estáticas
        public static Random random = new Random();
        public static bool DEBUG_CONSOLE = false;

      




        /*
        Função que somente é executada no modo DEBUG. Ela serve para printar um cromossomo na tela
        */
        public static void ApresentaCromossomoBool(List<bool> boolarray){
            foreach(bool bit in boolarray){
                Console.Write( (bit ? "1" : "0") );
            }
            Console.WriteLine("");
        } 
       


        
        public static double funcao_objetivo(List<bool> cromossomo, int n_variaveis_projeto, double function_min, double function_max){
            // ===================================================
            // Calcula o fenótipo para cada variável de projeto
            // ===================================================

            // Calcula o número de bits por variável de projeto
            int bits_por_variavel_projeto = cromossomo.Count / n_variaveis_projeto;

            // Cria a lista que irá conter o fenótipo de cada variável de projeto
            List<double> fenotipo_variaveis_projeto = new List<double>();
            
            // Transforma o genótipo de cada variável em uma string para depois converter para decimal
            for (int i=0; i<bits_por_variavel_projeto; i++){
                // Cria string representando os bits da variável
                string fenotipo_xi = "";
                
                // Percorre o número de bits de cada variável de projeto
                for(int c = n_variaveis_projeto*i; c < n_variaveis_projeto*(i+1); c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    fenotipo_xi += (cromossomo[c] ? "1" : "0");
                }

                // Converte essa string de bits para inteiro
                int variavel_convertida = Convert.ToInt32(fenotipo_xi, 2);

                // Mapeia o inteiro entre o intervalo mínimo e máximo da função
                // 0 --------- min
                // 2^bits ---- max
                // binario --- x
                // (max-min) / (2^bits - 0) ======> Variação de valor por bit
                // min + [(max-min) / (2^bits - 0)] * binario
                double fenotipo_variavel_projeto = function_min + ((function_max - function_min) * variavel_convertida / (Math.Pow(2, n_variaveis_projeto) - 1));

                // Adiciona o fenótipo da variável na lista de fenótipos
                fenotipo_variaveis_projeto.Add(fenotipo_variavel_projeto);
                // Console.WriteLine("fenotipo x"+i+": " + fenotipo_variavel_projeto);
            }

            // ===================================================
            // Calcula a função de Griewank
            // ===================================================
            
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
            
            return fx;
        }



      
      
        public static double GEO(int n_variaveis_projeto, int bits_por_variavel, double function_min, double function_max, double tao, int criterio_parada_nro_avaliacoes_funcao, List<int> NFOBs){
            
            /*

                ECOSISTEMA? POPULAÇÃO?

                CRITÉRIO DE PARADA

                VALORES DE TAO

                PDFs

            */



            // ========================================
            // Inicializa algumas variáveis de controle do algoritmo
            // ========================================
            
            // Número de avaliações da função objetivo
            int NFOB = 0;

            // Melhor ecosistema e fitness
            List<bool> melhor_C = new List<bool>();
            double melhor_fx = 9999;
            

            // ========================================
            // Geração da População Inicial
            // ========================================
            
            // Define o tamanho do cromossomo
            int tamanho_ecosistema = n_variaveis_projeto * bits_por_variavel;
            
            // Inicializa o cromossomo como uma lista
            List<bool> ecosistema = new List<bool>();
            
            // Gera mu bit para cada posição do cromossomo
            for (int i=0; i<tamanho_ecosistema; ++i){
                ecosistema.Add( (random.Next(0, 2)==1) ? true : false );
            }
#if DEBUGT
            Console.WriteLine("Cromossomo gerado:");
            ApresentaCromossomoBool(ecosistema);
#endif


            // ========================================
            // Calcula o primeiro Valor Refrência
            // ========================================
            melhor_fx = funcao_objetivo(ecosistema, n_variaveis_projeto, function_min, function_max);
#if DEBUGT            
            Console.WriteLine("Melhor fx: " + melhor_fx);
#endif


            // ========================================
            // Iterações
            // ========================================
            
            // Executa o algoritmos até que o critério de parada (número de avaliações na FO) seja atingido
            while (NFOB < criterio_parada_nro_avaliacoes_funcao){
                   
#if DEBUGT
                Console.WriteLine("Ecosistema começo while:");
                ApresentaCromossomoBool(ecosistema);
#endif

                
                // ========================================
                // Avalia o flip para cada bit
                // ========================================

                List<double> deltaV_fitness = new List<double>();
                List<int> bits = new List<int>();

                // Cria uma cópia do cromossomo, flipa o bit e verifica a fitness
                for (int i=0; i<ecosistema.Count; i++){
                    
                    // Cria uma cópia do cromossomo
                    List<bool> ecosistema_flipado = new List<bool>(ecosistema);
                    
                    // Flipa o i-ésimo bit
                    ecosistema_flipado[i] = !ecosistema_flipado[i];

                    // Calcula a fitness do ecosistema com o bit flipado
                    double fx = funcao_objetivo(ecosistema_flipado, n_variaveis_projeto, function_min, function_max);
                    // Console.WriteLine("Fitness " + i + ": " + fx);

                    // Calcula o ganho ou perda de flipar
                    double deltaV = fx - melhor_fx;
#if DEBUGT
                    Console.WriteLine("DELTAV " + deltaV + " = fx " + fx + " - ref " + melhor_fx);
#endif
                    
                    // Adiciona o delta na lista
                    bits.Add(i);
                    deltaV_fitness.Add(deltaV);

                    // Incrementa o número de avaliações da função objetivo
                    NFOB++;
                }

                
            
                // ========================================
                // Ordena os bits conforme os indices fitness
                // ========================================

#if DEBUGT
                Console.WriteLine("Antes da ordenação");
                for (int i=0; i<deltaV_fitness.Count; i++){
                    Console.WriteLine(i + ": Bit: " + bits[i] + " | deltaV_fitness: " + deltaV_fitness[i]);
                }
#endif

                // Inverte o sinal pra conseguir ordenar ao contrário
                for (int i=0; i<deltaV_fitness.Count; i++){
                    deltaV_fitness[i] = (-1) * deltaV_fitness[i];
                }



                // Transforma a população e os fitness para array para poder ordenar
                var deltaV_fitness_array = deltaV_fitness.ToArray();
                var bits_array = bits.ToArray();
                // Ordena a população com base no fitness
                Array.Sort(deltaV_fitness_array, bits_array);
                // Converte novamente os arrays para listas
                deltaV_fitness = deltaV_fitness_array.ToList();
                bits = bits_array.ToList();



                // Inverte o sinal de volta
                for (int i=0; i<deltaV_fitness.Count; i++){
                    deltaV_fitness[i] = (-1) * deltaV_fitness[i];
                }

#if DEBUGT
                Console.WriteLine("Antes da ordenação");
                for (int i=0; i<deltaV_fitness.Count; i++){
                    Console.WriteLine(i + ": Bit: " + bits[i] + " | deltaV_fitness: " + deltaV_fitness[i]);
                }
#endif

                // // Cria novo ecossistema conforme os deltaV ordenados
                // List<bool> ecosistema_ordenado = new List<bool>();
                // foreach (int bit_ordenado in bits){
                //     ecosistema_ordenado.Add(ecosistema[bit_ordenado]);
                // }

                
                
                // // ATUALIZA O ECOSISTEMA COM O ORDENADO?????
                // ecosistema = ecosistema_ordenado;

                
                // ========================================
                // Flipa um bit com probabilidade Pk
                // ========================================

                // Verifica as probabilidades até que um bit seja mutado
                bool bit_flipado = false;
                while (!bit_flipado){

                    // Gera um número aleatório com distribuição uniforme
                    double ALE = random.NextDouble();

                    // k é o índice do array ordenado
                    int k = random.Next(1, tamanho_ecosistema+1);
                    
                    // Com probabilidade Pk => k^(-tao)
                    double Pk = Math.Pow(k, -tao);

                    // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                    k -= 1;
#if DEBUGT
                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+bits[k]+" com Pk "+Pk+" >= ALE "+ALE);
#endif
                    if (Pk >= ALE){
                        // Flipa o bit
                        ecosistema[ bits[k] ] = !ecosistema[ bits[k] ];
#if DEBUGT
                        Console.WriteLine("Flipando o indice " + k + ", que é o bit " + bits[k]);
#endif                  
                        // Atualiza que o bit foi flipado
                        bit_flipado = true;
                        break;
                        
                    }
                }
#if DEBUGT
                Console.WriteLine("Depois de flipar:");
                ApresentaCromossomoBool(ecosistema);
#endif


                // Calcula a fitness do ecosistema
                double fitness_ecosistema = funcao_objetivo(ecosistema, n_variaveis_projeto, function_min, function_max);

                
                // ========================================
                // Atualiza se possível o valor (Valor Refrência)
                // ========================================

                // Se essa fitness for a menor que a melhor, atualiza a melhor da história
                if (fitness_ecosistema < melhor_fx){
                    melhor_fx = fitness_ecosistema;
                }


                // Pra cada NFOB, mostra a melhor fitness
                if (NFOB > NFOBs[0]){
                    Console.WriteLine("Fitness NFOB " + NFOB + ": " + melhor_fx);
                    NFOBs.RemoveAt(0);
				}
            }

            return melhor_fx;
        }

        

       
       
        public static void Main(string[] args){
            // Parâmetros de execução do algoritmo
            const int bits_por_variavel = 10;
            const int n_variaveis_projeto = 4;
            const double function_min = -600.0;
            const double function_max = 600.0;
            
            // 0.75 a 5.0
            const double tao = 2;
            
            const int criterio_parada_nro_avaliacoes_funcao = 200000;
            
            // Definição se apresenta no console o passo a passo ou se só executa diretamente
            DEBUG_CONSOLE = false;

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo executa e retorna uma lista contendo o melhor valor fitness logo acima daquele NFOB.
            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            



            // Inicializa o temporizador
            var total_watch = System.Diagnostics.Stopwatch.StartNew();

            // Executa e recebe como retorno o valor fitness em cada NFOBs_desejados
            double melhor_fx_geral = GEO(n_variaveis_projeto, bits_por_variavel, function_min, function_max, tao, criterio_parada_nro_avaliacoes_funcao, NFOBs_desejados);

            // Para o temporizador
            total_watch.Stop();

            // // Apresenta os resultados
            // Console.WriteLine("-----------------");
            // for(int j=0; j<bests_NFOB.Count; j++){
            //     Console.WriteLine(NFOBs_desejados[j] + ": " + bests_NFOB[j]);
            // }
            // Console.WriteLine("-----------------");

            // Calcula o tempo de execução
            var elapsedMs = total_watch.ElapsedMilliseconds;
            Console.WriteLine("Tempo total de execução: " + elapsedMs/1000.0 + " segundos");
            // ================================================================
            
        }
    }
}