// Diretivas de compilação para controle de partes do código
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION
// #define DEBUG_NOVO_MELHOR_FX

using System;
using System.Collections.Generic;
using Funcoes_Definidas;
using System.Linq;
using System.Runtime.InteropServices;
using SpaceDesignTeste;

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
            Função que gera a população de bits inicial para a execução
        */
        public static List<bool> geracao_populacao_de_bits(List<int> bits_por_variavel_variaveis){
            // Soma os bits por variável de projeto para saber o tamanho da população
            int tamanho_populacao_bits = 0;
            foreach(int bits_variavel in bits_por_variavel_variaveis){
                tamanho_populacao_bits += bits_variavel;
            }
            
            // Inicializa a população de bits como uma lista de bits (bool)
            List<bool> populacao_de_bits = new List<bool>();
            
            // Gera um bit para cada posição da população de bits
            for (int i=0; i<tamanho_populacao_bits; ++i){
                populacao_de_bits.Add( (random.Next(0, 2)==1) ? true : false );
            }

            // Retorna a população de bits
            return populacao_de_bits;
        }
        



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
        public static List<double> calcula_fenotipos_variaveis(List<bool> populacao_de_bits, int n_variaveis_projeto, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, List<int> bits_por_variavel_variaveis){
            //============================================================
            // Calcula o fenótipo para cada variável de projeto
            //============================================================

            // Cria a lista que irá conter o fenótipo de cada variável de projeto
            List<double> fenotipo_variaveis_projeto = new List<double>();
            
            // Transforma o genótipo de cada variável em uma string para depois converter para decimal
            int iterator = 0;
            for (int i=0; i<n_variaveis_projeto; i++){
                double limite_superior_variavel = limites_superiores_variaveis[i];
                // if (i==2){
                //     limite_superior_variavel = fenotipo_variaveis_projeto[1]-1;
                // }
                double limite_inferior_variavel = limites_inferiores_variaveis[i];
                double bits_variavel_projeto = bits_por_variavel_variaveis[i];
                // Cria string representando os bits da variável
                string fenotipo_xi = "";

                // Percorre o número de bits de cada variável de projeto
                for(int c=0; c<bits_variavel_projeto; c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    fenotipo_xi += (populacao_de_bits[iterator] ? "1" : "0");
                    
                    iterator++;
                }

                // Converte essa string de bits para inteiro
                int variavel_convertida = Convert.ToInt32(fenotipo_xi, 2);

                // Mapeia o inteiro entre o intervalo mínimo e máximo da função
                // 0 --------- min
                // 2^bits ---- max
                // binario --- x
                // (max-min) / (2^bits - 0) ======> Variação de valor por bit
                // min + [(max-min) / (2^bits - 0)] * binario
                
                double fenotipo_variavel_projeto = limite_inferior_variavel + ((limite_superior_variavel - limite_inferior_variavel) * variavel_convertida / (Math.Pow(2, bits_variavel_projeto) - 1));

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
            A função objetivo é a função fitness do algoritmo. Ela invoca os métodos para calcular
            o fenótipo de cada variável de projeto e, posteriormente, calcula o valor fitness.
        */
        public static double calcula_valor_fx(int definicao_funcao_objetivo, List<bool> populacao_de_bits, int n_variaveis_projeto, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, List<int> bits_por_variavel_variaveis){
            // Calcula o fenótipo para cada variável de projeto
            List<double> fenotipo_variaveis_projeto = calcula_fenotipos_variaveis(populacao_de_bits, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);

            // Calcula o valor da função objetivo
            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, definicao_funcao_objetivo);

            // Retorna o valor fitness
            return fx;
        }




        /*
            A função tem como objetivo flipar cada bit e, para cada flip, calcular a fx e compará-lo com um fx referência, 
            gerando um deltaV para cada flip. Essa lista de deltaV's é retornada pela função.
        */
        public static List<BitVerificado> obtem_lista_deltaV_se_flipar_comparando_com(double fx_para_comparacao, List<bool> populacao_de_bits, int definicao_funcao_objetivo, int n_variaveis_projeto, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, List<int> bits_por_variavel_variaveis){

            // Cria uma lista contendo as informações sobre mutar um bit
            List<BitVerificado> lista_informacoes_mutacao = new List<BitVerificado>();

            // Cria uma cópia da população de bits, flipa o bit e verifica a fitness
            for (int i=0; i<populacao_de_bits.Count; i++){
                
                // Cria uma cópia da população de bits
                List<bool> populacao_de_bits_flipado = new List<bool>(populacao_de_bits);
                
                // Flipa o i-ésimo bit
                populacao_de_bits_flipado[i] = !populacao_de_bits_flipado[i];

                // Calcula a fitness da populacao_de_bits com o bit flipado
                double fx = calcula_valor_fx(definicao_funcao_objetivo, populacao_de_bits_flipado, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);
                
                // // Incrementa o número de avaliações da função objetivo
                // NFOB++;

                // Calcula o ganho ou perda de flipar
                double deltaV = fx - fx_para_comparacao;

#if DEBUG_CONSOLE
                Console.WriteLine("DELTAV " + deltaV + " = fx " + fx + " - ref " + fx_para_comparacao);
#endif
                
                // Armazena as informações dessa mutação do bit na lista de informações
                BitVerificado informacoes_bit = new BitVerificado();
                informacoes_bit.delta_fitness = deltaV;
                informacoes_bit.indice_bit_mutado = i;
                lista_informacoes_mutacao.Add(informacoes_bit);
            }

            // Retorna a lista contendo as informações de flips
            return lista_informacoes_mutacao;
        }



        
        /*
            A função para ordenar e flipar um bit tem como objetivo ordenar toda a população de bits e escolher, com 
            probabilidade normalmente distribuida, um bit da população de bits para mutar.
            A função retorna a nova população de bits com o bit mutado.
        */
        public static List<bool> GEO_ordena_e_flipa_um_bit(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, double tau){

            int tamanho_populacao_bits = populacao_de_bits.Count;

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
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -tau);

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
        public static List<bool> GEOvar_ordena_e_flipa_bits(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, double tau){
            
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
            int iterador = 0;
            for (int i=0; i<n_variaveis_projeto; i++){
                // Obtém o número de bits dessa variável de projeto
                int bits_variavel_projeto = bits_por_variavel_variaveis[i];
                
                // Cria uma lista com as informações de mutação de cada bit da variável
                List<BitVerificado> lista_informacoes_bits_variavel = new List<BitVerificado>();
                
                // Percorre o número de bits de cada variável de projeto
                for(int c=0; c<bits_variavel_projeto; c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    lista_informacoes_bits_variavel.Add( lista_informacoes_mutacao[iterador] );

                    iterador++;
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
                    int k = random.Next(1, bits_variavel_projeto+1);
                    
                    // Probabilidade Pk => k^(-tau)
                    double Pk = Math.Pow(k, -tau);

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
            A função verifica o critério de parada da execução.
        */
        public static bool Verifica_Criterio_Parada(int criterio_parada_NFOBouNFEouMELHORFX, double valor_criterio_parada, double fx_esperado, int NFOB, double melhor_fx){
            
            // Se o critério de parada for de NFOB...
            if (criterio_parada_NFOBouNFEouMELHORFX == 0 || criterio_parada_NFOBouNFEouMELHORFX == 2) {
                
                bool condition = (NFOB >= valor_criterio_parada);
                return condition;

            }
            
            // Se o critério de parada for por precisão...
            else if (criterio_parada_NFOBouNFEouMELHORFX == 1){
                bool condition1 = ( Math.Abs(Math.Abs(melhor_fx) - Math.Abs(fx_esperado)) <= valor_criterio_parada );
                bool condition2 = NFOB >= 100000;
                bool condition = condition1 || condition2;
                // bool condition = condition1;

                // if (NFOB % 1000 == 0){
                //     Console.WriteLine(NFOB);
                // }

                // if (condition){
                //     Console.WriteLine("PRECISION BREAK! NFOB = {0}", NFOB);
                // }
                
                return condition;
            }

            // Se o critério não for correto, retorna ok para a parada.
            else{
                return true;
            }
        }




        /*
            A função GEO é a função principal do código. Aqui nesse bloco toda a lógica implementada para os algoritmos
            GEOcanonico e GEOvar são implementadas, como a geração da população, o controle do critério de parada e a
            avaliação do flip de cada bit.
        */
        public static List<double> GEOs_algorithms(int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tau, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){  

            // definicao_funcao_objetivo
            // 0 - Griewangk
            // 1 - Rosenbrock
            // 2 - DeJong3
            
            //============================================================
            // Inicializa algumas variáveis de controle do algoritmo
            //============================================================

            // Variável que contém o número do NFOB no qual deve se armazenar o f(x)
            int proximo_step = step_obter_NFOBs;

            // Número de avaliações da função objetivo
            int NFOB = 0;

            // Melhor fitness até o momento. Como é minimização, começa com o maior valor possível
            double melhor_fx = Double.MaxValue;

            // Cria a lista que conterá os melhores f(x) a cada NFOB desejado
            List<double> melhores_NFOBs = new List<double>();
            
            //============================================================
            // Geração da População de Bits Inicial
            //============================================================
            
            List<bool> populacao_de_bits = geracao_populacao_de_bits(bits_por_variavel_variaveis);

            //============================================================
            // Calcula o primeiro Valor Referência
            //============================================================

            double atual_fx = calcula_valor_fx(definicao_funcao_objetivo, populacao_de_bits, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);
            
            // Incrementa o número de avaliações da função objetivo
            NFOB++;

            // Inicializa o melhor fx como o fx inicial.
            melhor_fx = atual_fx;

#if DEBUG_CONSOLE    
            Console.WriteLine("População de bits gerado:");
            ApresentaCromossomoBool(populacao_de_bits);        
            Console.WriteLine("Melhor fx: " + melhor_fx);
#endif

            //============================================================
            // Iterações
            //============================================================
            

            // Inicializa o CoI(i-1) para o controle da mutaçaão do TAU no AGEO
            double CoI_1 = 1.0 / Math.Sqrt(populacao_de_bits.Count);

            // tau começa como 0.5 se for o AGEO
            if (tipo_AGEO == 1 || tipo_AGEO == 2){
                tau = 0.5;
            }





            // Entra no while e avalia a condição de parada lá no fim
            while (true){

#if DEBUG_CONSOLE
                Console.WriteLine("População de Bits começo while:");
                ApresentaCromossomoBool(populacao_de_bits);
                double teste = calcula_valor_fx(definicao_funcao_objetivo, populacao_de_bits, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);
                Console.WriteLine("Fx = " + teste);
                Console.WriteLine("Fx melhor = " + melhor_fx);
#endif


                //============================================================
                // Avalia o flip para cada bit
                //============================================================

                List<BitVerificado> lista_informacoes_mutacao = obtem_lista_deltaV_se_flipar_comparando_com(melhor_fx, populacao_de_bits, definicao_funcao_objetivo, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);

                // FO foi avaliada por N vezes para cada flip. Portanto, incrementa aqui..
                for (int i=0; i<populacao_de_bits.Count; i++){
                    NFOB++;
                }
            

                //============================================================
                // Calcula a Chance of Improvement
                //============================================================
                
                if (tipo_AGEO == 1 || tipo_AGEO == 2){
                    List<BitVerificado> informacoes_mutacao_AGEO = new List<BitVerificado>();

                    if (tipo_AGEO == 1){
                        // Obtém a lista de deltaV's comparado ao melhor fx
                        informacoes_mutacao_AGEO = lista_informacoes_mutacao;

                        // // FO foi avaliada por N vezes para cada flip. Portanto, incrementa aqui..
                        // for (int i=0; i<populacao_de_bits.Count; i++){
                        //     NFOB++;
                        // }
                    }
                    else if (tipo_AGEO == 2){
                        // Obtém a lista de deltaV's comparado ao atual fx
                        informacoes_mutacao_AGEO = obtem_lista_deltaV_se_flipar_comparando_com(atual_fx, populacao_de_bits, definicao_funcao_objetivo, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);

                        // // FO foi avaliada por N vezes para cada flip. Portanto, incrementa aqui..
                        // for (int i=0; i<populacao_de_bits.Count; i++){
                        //     NFOB++;
                        // }
                    }

                    // Conta quantas mudanças que flipando dá melhor
                    int melhoraram = 0;
                    foreach(BitVerificado info in informacoes_mutacao_AGEO){
                        if (info.delta_fitness < 0){
                            melhoraram++;
                        }
                    }
                    
                    // Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_de_bits.Count, melhoraram);

                    // Calcula a Chance of Improvement
                    double CoI = (double)melhoraram / populacao_de_bits.Count;

                    // Verifica se INCREASE TAU ou RESTART TAU
                    if (CoI <= 0.0 || tau > 5){
                        // RESTART TAU
                        // Console.WriteLine("RESTART TAU");
                        
                        // tau = 0.5 * Math.Exp(random.NextDouble() * (1.0/Math.Sqrt(populacao_de_bits.Count)));
                        
                        // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_de_bits.Count)) );

                        // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_de_bits.Count), 1.0/2.0)));

                        tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_de_bits.Count), 1.0/2.0 )));

                    }
                    else if(CoI <= CoI_1){
                        // INCREASE TAU
                        // Console.WriteLine("INCREASE TAU");

                        tau += (0.5 + CoI) * random.NextDouble();
                    }
                    
                    // Console.WriteLine("Valor TAU: {0}", tau);

                    // Atualiza o CoI(i-1) como sendo o atual CoI(i)
                    CoI_1 = CoI;
                }


                //============================================================
                // Ordena os bits e flipa, atualizando a população
                //============================================================
                
                //GEOcanonico
                if (tipo_GEO == 0){ 
                    populacao_de_bits = GEO_ordena_e_flipa_um_bit(populacao_de_bits, lista_informacoes_mutacao, tau);
                }
                //GEOvar
                else if (tipo_GEO == 1){
                    populacao_de_bits = GEOvar_ordena_e_flipa_bits(populacao_de_bits, lista_informacoes_mutacao, n_variaveis_projeto, bits_por_variavel_variaveis, tau);
                }


                //============================================================
                // Atualiza, se possível, o valor (Valor Refrência)
                //============================================================

                // Calcula a fitness da nova população de bits
                double fitness_populacao_de_bits = calcula_valor_fx(definicao_funcao_objetivo, populacao_de_bits, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);
                
                // Incrementa o número de avaliações da função objetivo
                NFOB++;

                // Se essa fitness for a menor que a melhor...
                if (fitness_populacao_de_bits < melhor_fx){
                    // Atualiza o melhor da história
                    melhor_fx = fitness_populacao_de_bits;

#if DEBUG_NOVO_MELHOR_FX
                    Console.WriteLine("------------------");
                    Console.WriteLine("Atualizou o melhor "+melhor_fx+" em NFOB "+NFOB);
                    Console.WriteLine("------------------");
#endif
                }


                //============================================================
                // Verifica se armazena o valor f(x) no atual NFOB
                //============================================================

                // Se for a hora de guardar o valor f(x) nesse NFOB, guarda
                if (NFOB > proximo_step){
                    // Incrementa o step para verificação de NFOB
                    proximo_step += step_obter_NFOBs;
                        
                    // Adiciona o f(x) na lista de NFOBs
                    melhores_NFOBs.Add(melhor_fx);
                }


                //============================================================
                // Verifica o critério de parada
                //============================================================
                
                bool parada = Verifica_Criterio_Parada(criterio_parada_NFOBouNFEouMELHORFX, valor_criterio_parada, fx_esperado, NFOB, melhor_fx);
                
                if (parada){
                    break;
                }
            }
            
            //============================================================
            // Retorna o resultado da execução
            //============================================================

            // Se o critério de parada era por NFOB, retorna a lista de f(x) nos NFOBs
            if (criterio_parada_NFOBouNFEouMELHORFX == 0){
                return melhores_NFOBs;
            }
            // Se o critério de parada era por NFE, retorna o NFE ao fim da execução
            else if (criterio_parada_NFOBouNFEouMELHORFX == 1){
                return new List<double>(){NFOB};
            }
            // Se o critério de parada era por MELHORFX, retorna o melhor f(x) obtido no final
            else if (criterio_parada_NFOBouNFEouMELHORFX == 2){
                return new List<double>(){melhor_fx};
            }
            // Senão, retorna um número grande
            else{
                return new List<double>(){Double.MaxValue};
            }
        }
    }
}