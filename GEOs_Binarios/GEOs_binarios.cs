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

using Classes_Comuns_Enums;

namespace GEO{
    /*
        Classe principal contendo os algoritmos GEO
    */
    public class GEO{
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
            Função que somente é executada no modo DEBUG. Ela serve para printar uma população de bits na tela.
        */
        public static void ApresentaCromossomoBool(List<bool> boolarray){
            foreach(bool bit in boolarray){
                Console.Write( (bit ? "1" : "0") );
            }
            Console.WriteLine("");
        } 
       



        /*
            A função calcula o fenótipo de cada uma das variáveis de projeto.
        */
        public static List<double> calcula_fenotipos_variaveis(List<bool> populacao_de_bits, CodificacaoBinariaParaFenotipo codificacao_binaria_para_fenotipo){
            //============================================================
            // Calcula o fenótipo para cada variável de projeto
            //============================================================

            int n_variaveis_projeto = codificacao_binaria_para_fenotipo.bits_por_variavel_variaveis.Count;
            List<double> limites_inferiores_variaveis = codificacao_binaria_para_fenotipo.limites_inferiores_variaveis;
            List<double> limites_superiores_variaveis = codificacao_binaria_para_fenotipo.limites_superiores_variaveis;
            List<int> bits_por_variavel_variaveis = codificacao_binaria_para_fenotipo.bits_por_variavel_variaveis;

            // Cria a lista que irá conter o fenótipo de cada variável de projeto
            List<double> fenotipo_variaveis_projeto = new List<double>();
            
            // Transforma o genótipo de cada variável em uma string para depois converter para decimal
            int iterator = 0;
            for (int i=0; i<n_variaveis_projeto; i++){
                double limite_superior_variavel = limites_superiores_variaveis[i];
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
            A função tem como objetivo flipar cada bit e calcular a fx, retornando a fx para cada flip.
        */
        public static List<BitVerificado> obtem_lista_fxs_flipando(List<bool> populacao_de_bits, ParametrosDaFuncao parametros_problema, CodificacaoBinariaParaFenotipo codificacao_binaria_para_fenotipo){

            // Cria uma lista contendo as informações sobre mutar um bit
            List<BitVerificado> lista_informacoes_mutacao = new List<BitVerificado>();

            // Cria uma cópia da população de bits, flipa o bit e verifica a fitness
            for (int i=0; i<populacao_de_bits.Count; i++){
                
                // Cria uma cópia da população de bits
                List<bool> populacao_de_bits_flipado = new List<bool>(populacao_de_bits);
                
                // Flipa o i-ésimo bit
                populacao_de_bits_flipado[i] = !populacao_de_bits_flipado[i];

                // Calcula a fitness da populacao_de_bits com o bit flipado
                List<double> fenotipo_variaveis_projeto = calcula_fenotipos_variaveis(populacao_de_bits_flipado, codificacao_binaria_para_fenotipo);

                double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, parametros_problema.definicao_funcao_objetivo);
                
                // Armazena as informações dessa mutação do bit na lista de informações
                BitVerificado informacoes_bit = new BitVerificado();
                informacoes_bit.funcao_objetivo_flipando = fx;
                informacoes_bit.indice_bit_mutado = i;
                lista_informacoes_mutacao.Add(informacoes_bit);
            }

            // Retorna a lista contendo as informações de flips
            return lista_informacoes_mutacao;
        }



        
        /*
            A função ordena toda a população de bits e escolhe, com probabilidade normalmente 
            distribuida, um bit da população de bits para mutar.
        */
        public static List<bool> ordena_tudo_e_perturba_um_bit(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, double tau){

            int tamanho_populacao_bits = populacao_de_bits.Count;

            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

#if DEBUG_CONSOLE
            Console.WriteLine("Antes da ordenação");
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + lista_informacoes_mutacao[i].funcao_objetivo_flipando);
            }
#endif

            // Ordena os bits
            lista_informacoes_mutacao.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.funcao_objetivo_flipando.CompareTo(b2.funcao_objetivo_flipando); });
           
#if DEBUG_CONSOLE
            Console.WriteLine("Depois da ordenação");
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + lista_informacoes_mutacao[i].funcao_objetivo_flipando);
            }
#endif
            
            //============================================================
            // Flipa um bit com probabilidade Pk
            //============================================================

            // Verifica as probabilidades até que um bit seja mutado
            while (true){

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

                    // Sai do laço
                    break;
                }
            }

            // Retorna a nova população de bits
            return populacao_de_bits;
        }




        /*
            A função ordenaa todos os bits de cada variável e, posteriormente, escolhe com probabilidade normalmente 
            distribuida, um bit por variável para mutar.
        */
        public static List<bool> ordena_por_var_e_perturba_por_var(List<bool> populacao_de_bits, List<BitVerificado> lista_informacoes_mutacao, List<int> bits_por_variavel_variaveis, double tau){
            
            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

#if DEBUG_CONSOLE
            Console.WriteLine("Antes da ordenação:");
            Console.WriteLine(lista_informacoes_mutacao.Count);
            ApresentaCromossomoBool(populacao_de_bits);
            for (int i=0; i<lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + lista_informacoes_mutacao[i].funcao_objetivo_flipando);
            }
#endif

            // Percorre cada variável de projeto para ordenar os bits e escolher o bit para filpar
            List<BitVerificado> depois_ordenar = new List<BitVerificado>();
            int iterador = 0;
            int n_variaveis_projeto = bits_por_variavel_variaveis.Count;
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
                lista_informacoes_bits_variavel.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.funcao_objetivo_flipando.CompareTo(b2.funcao_objetivo_flipando); });

                // Coloca esses bits ordenado na lista geral
                foreach (BitVerificado bit in lista_informacoes_bits_variavel){
                    depois_ordenar.Add(bit);
                }


                //============================================================
                // Flipa um bit da variável com probabilidade Pk
                //============================================================

                // Verifica as probabilidades até que um bit por variável seja mutado
                while (true){

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

                        // Sai do laço
                        break;
                    }
                }
            }

#if DEBUG_CONSOLE
            Console.WriteLine("Depois da ordenação:");
            ApresentaCromossomoBool(populacao_de_bits);
            for (int m=0; m<depois_ordenar.Count; m++){
                Console.WriteLine(m + ": Bit: " + depois_ordenar[m].indice_bit_mutado + "\t| fx_flipado: " + depois_ordenar[m].funcao_objetivo_flipando);
            }
#endif

            // Retorna a nova população de bits
            return populacao_de_bits;
        }




        /*
            A função verifica o critério de parada da execução.
        */
        public static bool Verifica_Criterio_Parada(ParametrosCriterioParada parametros_criterio_parada, int NFOB, double melhor_fx){
            
            // Se o critério de parada for de NFOB...
            if (parametros_criterio_parada.tipo_criterio_parada == 0) {
                
                // Determina a condição final
                bool condition = (NFOB >= parametros_criterio_parada.NFOB_criterio_parada);
                
                return condition;
            }
            
            // Se o critério de parada for por precisão...
            else if (parametros_criterio_parada.tipo_criterio_parada == 1){
                
                // Determina a condição final
                bool condition = ( Math.Abs(Math.Abs(melhor_fx) - Math.Abs(parametros_criterio_parada.fx_esperado)) <= parametros_criterio_parada.PRECISAO_criterio_parada );
                
                return condition;
            }

            // Se o critério de parada for por precisão e por NFOB...
            else if (parametros_criterio_parada.tipo_criterio_parada == 2){
                
                // Verifica a condição por precisão
                bool condition1 = ( Math.Abs(Math.Abs(melhor_fx) - Math.Abs(parametros_criterio_parada.fx_esperado)) <= parametros_criterio_parada.PRECISAO_criterio_parada );
                
                // Verifica a condição por NFOB
                bool condition2 = NFOB >= parametros_criterio_parada.NFOB_criterio_parada;
                
                // Determina a condição final
                bool condition = condition1 || condition2;
                
                // if (condition){
                //     Console.WriteLine("PRECISION BREAK! NFOB = {0}", NFOB);
                // }
                
                return condition;
            }

            // Se o critério não for correto, retorna ok para a parada..
            else{
                return true;
            }
        }




        /*
            Essa função é a função principal. Nesse bloco toda a lógica para o GEO e GEOvar é implementada, 
            além do AGEO1, AGEO2 e o mix de AGEO1var e AGEO2var.
        */
        public static RetornoGEOs GEOs_algorithms(int tipo_GEO, int tipo_AGEO, double tau, ParametrosDaFuncao parametros_problema, CodificacaoBinariaParaFenotipo codificacao_binaria_para_fenotipo, ParametrosCriterioParada parametros_criterio_parada){   

            //============================================================
            // Inicializa algumas variáveis de controle do algoritmo
            //============================================================

            // Variável que contém o número do NFOB no qual deve se armazenar o f(x)
            int proximo_step = parametros_criterio_parada.step_para_obter_NFOBs;

            // Número de avaliações da função objetivo
            int NFOB = 0;

            // Melhor fitness até o momento. Como é minimização, começa com o maior valor possível
            double melhor_fx = Double.MaxValue;

            // Cria a lista que conterá os melhores f(x) a cada NFOB desejado
            List<double> melhores_NFOBs = new List<double>();

            //============================================================
            // Geração da População de Bits Inicial
            //============================================================
            
            List<bool> populacao_atual = geracao_populacao_de_bits(codificacao_binaria_para_fenotipo.bits_por_variavel_variaveis);

            //============================================================
            // Calcula o primeiro Valor Referência
            //============================================================
            
            // Inicializa o fenótipo das variáveis de projeto
            List<double> fenotipo_variaveis_projeto = calcula_fenotipos_variaveis(populacao_atual, codificacao_binaria_para_fenotipo);

            double atual_fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, parametros_problema.definicao_funcao_objetivo);
            
            // Incrementa o número de avaliações da função objetivo
            NFOB++;

            // Inicializa o melhor fx como o fx inicial.
            melhor_fx = atual_fx;

#if DEBUG_CONSOLE    
            Console.WriteLine("População de bits gerado:");
            ApresentaCromossomoBool(populacao_atual);        
            Console.WriteLine("Melhor fx: " + melhor_fx);
#endif

            
            //====================================================================
            // AGEOs
            //====================================================================
            // Inicializa o CoI(i-1) para o controle da mutaçaão do TAU no AGEO
            double CoI_1 = 1.0 / Math.Sqrt(populacao_atual.Count);

            // tau começa como 0.5 se for o AGEO
            if (tipo_AGEO == 1 || tipo_AGEO == 2){
                tau = 0.5;
            }
            //====================================================================



            //============================================================
            // Iterações
            //============================================================

            // Entra no while e avalia a condição de parada lá no fim
            while (true){

#if DEBUG_CONSOLE
                Console.WriteLine("População de Bits começo while:");
                ApresentaCromossomoBool(populacao_atual);
                Console.WriteLine("Fx = " + atual_fx);
                Console.WriteLine("Fx melhor = " + melhor_fx);
#endif

                //============================================================
                // Avalia o flip para cada bit
                //============================================================

                List<BitVerificado> lista_informacoes_mutacao = obtem_lista_fxs_flipando(populacao_atual, parametros_problema, codificacao_binaria_para_fenotipo);

                // FO foi avaliada por N vezes para cada flip. Portanto, incrementa aqui..
                for (int i=0; i<populacao_atual.Count; i++){
                    NFOB++;
                }
            

                //============================================================
                // Calcula a Chance of Improvement
                //============================================================
                
                if (tipo_AGEO == 1 || tipo_AGEO == 2){
                    List<BitVerificado> informacoes_mutacao_AGEO = new List<BitVerificado>();

                    // Conta quantas mudanças que flipando dá melhor
                    int melhoraram = 0;

                    if (tipo_AGEO == 1){
                        // Verifica quantos melhora em comparação com o MELHOR FX
                        melhoraram = lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= melhor_fx).ToList().Count;
                    }
                    else if (tipo_AGEO == 2){
                        // Verifica quantos melhora em comparação com o ATUAL FX
                        melhoraram = lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= atual_fx).ToList().Count;                 
                    }

                    // Calcula a Chance of Improvement
                    double CoI = (double) melhoraram / populacao_atual.Count;

                    // Se a CoI for zero, restarta o TAU
                    if (CoI <= 0.0 || tau > 5){
                        // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                        // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                        tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));

                    }
                    // Senão, se for menor que o CoI anterior, aumenta o TAU
                    else if(CoI <= CoI_1){
                        tau += (0.5 + CoI) * random.NextDouble();
                    }
                    
#if DEBUG_CONSOLE
                    Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_atual.Count, melhoraram);
                    Console.WriteLine("Valor TAU: {0}", tau);
#endif

                    // Atualiza o CoI(i-1) como sendo o atual CoI(i)
                    CoI_1 = CoI;
                }


                //============================================================
                // Ordena os bits e flipa, atualizando a população
                //============================================================
                
                //GEOcanonico
                if (tipo_GEO == 0){ 
                    populacao_atual = ordena_tudo_e_perturba_um_bit(populacao_atual, lista_informacoes_mutacao, tau);
                }
                //GEOvar
                else if (tipo_GEO == 1){
                    populacao_atual = ordena_por_var_e_perturba_por_var(populacao_atual, lista_informacoes_mutacao, codificacao_binaria_para_fenotipo.bits_por_variavel_variaveis, tau);
                }


                //============================================================
                // Atualiza, se possível, o valor (Valor Refrência)
                //============================================================

                // Calcula a fitness da nova população de bits
                fenotipo_variaveis_projeto = calcula_fenotipos_variaveis(populacao_atual, codificacao_binaria_para_fenotipo);

                atual_fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, parametros_problema.definicao_funcao_objetivo);
                
                // Incrementa o número de avaliações da função objetivo
                NFOB++;

                // Se essa fitness for a menor que a melhor...
                if (atual_fx < melhor_fx){
                    // Atualiza o melhor da história
                    melhor_fx = atual_fx;

#if DEBUG_NOVO_MELHOR_FX
                    Console.WriteLine("------------------");
                    Console.WriteLine("Atualizou o melhor "+melhor_fx+" em NFOB "+NFOB);
                    Console.WriteLine("------------------");
#endif
                }


                //============================================================
                // Verifica se armazena o valor f(x) no atual NFOB
                //============================================================

                // Se o critério for de NFOBs, verifica o NFOB para armazenar
                if (parametros_criterio_parada.tipo_criterio_parada == 0){
                    // Se for a hora de guardar o valor f(x) nesse NFOB, guarda
                    if (NFOB > proximo_step){
                        // Incrementa o step para verificação de NFOB
                        proximo_step += parametros_criterio_parada.step_para_obter_NFOBs;
                            
                        // Adiciona o f(x) na lista de NFOBs
                        melhores_NFOBs.Add(melhor_fx);
                    }
                }


                //============================================================
                // Verifica o critério de parada
                //============================================================
                
                bool parada = Verifica_Criterio_Parada(parametros_criterio_parada, NFOB, melhor_fx);
                
                if (parada){
                    break;
                }
            }
            
            //============================================================
            // Retorna o resultado da execução
            //============================================================

            RetornoGEOs retorno = new RetornoGEOs();
            retorno.NFOB = NFOB;
            retorno.melhores_NFOBs = melhores_NFOBs;
            retorno.melhor_fx = melhor_fx;
            
            return retorno;
        }
    }
}