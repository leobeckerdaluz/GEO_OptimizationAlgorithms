// Diretivas de compilação para controle de partes do código
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION

// #define CRITERIO_PARADA_NFOB
#define CRITERIO_PARADA_PRECISAO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Funcoes_Definidas;
using SpaceDesignTeste;



using SpaceConceptOptimizer.Models;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Settings;
using SpaceConceptOptimizer.Utilities;

namespace Execucoes
{
    public class Execucoes_GEO
    {
        // =====================================
        // ROTINAS PARA EXECUÇÃO
        // =====================================

        public static double Obtem_Media_Execucoes_NFE(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tau, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){
            
            double somatorio_resultados = 0;

            //------------------------------------------------
            //int contador_deubom = 0;
            //------------------------------------------------

            // Executa o algoritmo por N vezes
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"...");

                // Executa o algoritmo desejado e recebe como retorno apenas um valor (melhor fitness da execução ou NFE atingido), conforme o critério de parada.
                List<double> retorno_algoritmo = GEO.GEO.GEOs_algorithms(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                
                //------------------------------------------------
                // if (retorno_algoritmo[0] >= 100000){
                //     Console.WriteLine("Não contabilizando! NFOB = {0}", retorno_algoritmo[0]);
                //     continue;
                // }
                // Console.WriteLine("Passou reto o if. NFOB = {0}", retorno_algoritmo[0]);
                // contador_deubom++;
                //------------------------------------------------

                // Como o retorno é somente um número, obtém o índice 0
                somatorio_resultados += retorno_algoritmo[0];
            }

            // Calcula e retorna a média dos valores recebidos durante a execução
            double media_resultado = somatorio_resultados / numero_execucoes;
            Console.WriteLine("Media / total execucoes: {0}", media_resultado);

            //------------------------------------------------
            // double media_deubom = somatorio_resultados / contador_deubom;
            // Console.WriteLine("Media / deubom: {0}", media_deubom);
            //------------------------------------------------

            return media_resultado;
        }
        
        
        public static List<double> Obtem_FxsMedios_NFOB_Execucoes(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tau, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado){
            
            // Como o critério de parada é por NFOBs, então já define:
            int criterio_parada_NFOBouNFEouMELHORFX = 0;

            // Inicializa a lista de todos os resultados
            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"... ");

                // Executa o algoritmo
                List<double> NFOBs_results_GEO = GEO.GEO.GEOs_algorithms(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

                // Adiciona a lista de NFOBs em uma lista geral
                NFOBs_all_results_GEO.Add(NFOBs_results_GEO);
            }
            // Console.WriteLine("");

            // Obtém a quantidade de NFOBs armazenados
            int quantidade_NFOBs = NFOBs_all_results_GEO[0].Count;

            // Lista irá conter o f(x) médio para cada NFOB desejado
            List<double> fx_medio_cada_NFOB_desejado = new List<double>();
            
            // Para cada NFOB desejado, calcula a média das N execuções
            for(int i=0; i<quantidade_NFOBs; i++){
                double sum = 0;

                // Percorre aquele NFOB em cada execução para calcular a média de f(x) naquele NFOB
                foreach(List<double> execution in NFOBs_all_results_GEO){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEO.Count;
                
                // Adiciona o f(x) médio do NFOB na lista
                fx_medio_cada_NFOB_desejado.Add(media);
            }

            // Retorna a lista contendo o f(x) médio para cada NFOB
            return fx_medio_cada_NFOB_desejado;
        }
        

        public static void Executa_Todos_Algoritmos_Por_NFOBs(int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tauGEO, double tauGEOvar, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado){

            // Inicializa as variáveis para cada execução
            double tau = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            Console.Write("\nGEO...");
            tau = tauGEO;
            tipo_GEO = 0;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEO = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // GEOvar
            Console.Write("\nGEOvar...");
            tau = tauGEOvar;
            tipo_GEO = 1;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEOvar = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // AGEO1
            Console.Write("\nAGEO1...");
            tipo_GEO = 0;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // AGEO2
            Console.Write("\nAGEO2...");
            tipo_GEO = 0;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // AGEO1var
            Console.Write("\nAGEO1var...");
            tipo_GEO = 1;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // AGEO2var
            Console.Write("\nAGEO2var...");
            tipo_GEO = 1;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado);

            // ===========================================================
            // Mostra a média dos f(x) nas execuções em cada NFOB
            // ===========================================================

            Console.WriteLine("");
            Console.WriteLine("===> Médias para cada NFOB:");
            Console.WriteLine("NFOB;GEO;GEOvar;AGEO1;AGEO2;AGEO1var;AGEO2var");
            
            // Obtém a quantidade de NFOBs
            int quantidade_NFOBs = NFOBs_all_results_GEO.Count;

            // Apresenta a média para cada NFOB
            int NFOB_atual = step_obter_NFOBs;
            for(int i=0; i<quantidade_NFOBs; i++){
                string str_media_GEO = (NFOBs_all_results_GEO[i].ToString()).Replace('.',',');
                string str_media_GEOvar = (NFOBs_all_results_GEOvar[i].ToString()).Replace('.',',');
                string str_media_AGEO1 = (NFOBs_all_results_AGEO1[i].ToString()).Replace('.',',');
                string str_media_AGEO2 = (NFOBs_all_results_AGEO2[i].ToString()).Replace('.',',');
                string str_media_AGEO1var = (NFOBs_all_results_AGEO1var[i].ToString()).Replace('.',',');
                string str_media_AGEO2var = (NFOBs_all_results_AGEO2var[i].ToString()).Replace('.',',');
                
                
                Console.WriteLine("{0};{1};{2};{3};{4};{5};{6}", NFOB_atual, str_media_GEO, str_media_GEOvar, str_media_AGEO1, str_media_AGEO2, str_media_AGEO1var, str_media_AGEO2var);

                NFOB_atual += step_obter_NFOBs;
            }
        }
        

        public static void Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(List<double> valores_TAU, int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double valor_criterio_parada, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){
            
            // Inicializa as variáveis para cada execução
            int tipo_GEO = 0;
            int tipo_AGEO = 0;
            
            // Não será necessário obter melhor f(x) a cada NFOB
            int step_obter_NFOBs = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            string resultados = "";

            // Para cada TAU, executa os algoritmos    
            foreach(double tau in valores_TAU){
                Console.Write("\nTAU = {0}", tau);

                // GEO
                Console.Write("\nGEO...");
                tipo_GEO = 0;
                tipo_AGEO = 0;
                double media_GEO = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                Console.WriteLine("NFE médio GEO: {0}", media_GEO);
                
                // GEOvar
                Console.Write("\nGEOvar...");
                tipo_GEO = 1;
                tipo_AGEO = 0;
                double media_GEOvar = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                Console.WriteLine("NFE médio GEOvar: {0}", media_GEOvar);

                // ===========================================================
                // Mostra a média dos NFE nas execuções
                // ===========================================================

                string str_tau = (tau.ToString()).Replace('.',',');
                string str_media_NFE_GEO = (media_GEO.ToString()).Replace('.',',');
                string str_media_NFE_GEOvar = (media_GEOvar.ToString()).Replace('.',',');
                
                string result = String.Format("{0};{1};{2}", str_tau, str_media_NFE_GEO, str_media_NFE_GEOvar);

                resultados += result + "\n";
            }

            Console.WriteLine("");
            Console.WriteLine("===> Médias NFE:");
            Console.WriteLine("TAU;GEO;GEOvar");
            Console.WriteLine(resultados);
        }


        public static void Executa_Todos_Algoritmos_Por_NFE(int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tauGEO, double tauGEOvar, double valor_criterio_parada, double fx_esperado){

            // Inicializa as variáveis para cada execução
            double tau = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // Não será necessário obter melhor f(x) a cada NFOB
            int step_obter_NFOBs = 0;

            // O critério de parada vai ser quando atingir a precisão
            int criterio_parada_NFOBouNFEouMELHORFX = 1;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            Console.Write("\nGEO...");
            tau = tauGEO;
            tipo_GEO = 0;
            tipo_AGEO = 0;
            double media_GEO = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio GEO: {0}", media_GEO);

            // GEOvar
            Console.Write("\nGEOvar...");
            tau = tauGEOvar;
            tipo_GEO = 1;
            tipo_AGEO = 0;
            double media_GEOvar = 2;//Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio GEOvar: {0}", media_GEOvar);

            // AGEO1
            Console.Write("\nAGEO1...");
            tipo_GEO = 0;
            tipo_AGEO = 1;
            double media_AGEO1 = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio AGEO1: {0}", media_AGEO1);

            // AGEO2
            Console.Write("\nAGEO2...");
            tipo_GEO = 0;
            tipo_AGEO = 2;
            double media_AGEO2 = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio AGEO2: {0}", media_AGEO2);

            // AGEO1var
            Console.Write("\nAGEO1var...");
            tipo_GEO = 1;
            tipo_AGEO = 1;
            double media_AGEO1var = 2;//Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio AGEO1var: {0}", media_AGEO1var);

            // AGEO2var
            Console.Write("\nAGEO2var...");
            tipo_GEO = 1;
            tipo_AGEO = 2;
            double media_AGEO2var = 2;//Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tau, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
            Console.WriteLine("NFE médio AGEO2var: {0}", media_AGEO2var);

            // ===========================================================
            // Mostra a média dos NFE nas execuções
            // ===========================================================

            string str_media_NFE_GEO = (media_GEO.ToString()).Replace('.',',');
            string str_media_NFE_GEOvar = (media_GEOvar.ToString()).Replace('.',',');
            string str_media_NFE_AGEO1 = (media_AGEO1.ToString()).Replace('.',',');
            string str_media_NFE_AGEO2 = (media_AGEO2.ToString()).Replace('.',',');
            string str_media_NFE_AGEO1var = (media_AGEO1var.ToString()).Replace('.',',');
            string str_media_NFE_AGEO2var = (media_AGEO2var.ToString()).Replace('.',',');
            
            string result = String.Format("{0};{1};{2};{3};{4};{5}", str_media_NFE_GEO, str_media_NFE_GEOvar, str_media_NFE_AGEO1, str_media_NFE_AGEO2, str_media_NFE_AGEO1var, str_media_NFE_AGEO2var);

            Console.WriteLine("");
            Console.WriteLine("===> Médias NFE:");
            Console.WriteLine("NFOB;GEO;GEOvar;AGEO1;AGEO2;AGEO1var;AGEO2var");
            Console.WriteLine(result);
        }
            


        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public static void Execucoes_Griewangk(){
            // Parâmetros da função
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = Enumerable.Repeat(14, n_variaveis_projeto).ToList();
            List<double> limites_inferiores_variaveis = Enumerable.Repeat(-600.0, n_variaveis_projeto).ToList();
            List<double> limites_superiores_variaveis = Enumerable.Repeat(600.0, n_variaveis_projeto).ToList();
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1E6;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;
            
            // Define os TAUs
            double tauGEO = 1.25;
            double tauGEOvar = 3;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // Define que o algoritmo irá executar até certo NFOB e a execução irá retornar o melhor f(x)
            // const int criterio_parada_NFOBouNFEouMELHORFX = 2;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 100000;

            // // Percorre a lista de TAUs e executa os algoritmos
            // List<double> valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(valores_TAU, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
        }


        public static void Execucoes_Rosenbrock(){
            // Parâmetros da função
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 1;

            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

          
            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1e6;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;

            // Define os TAUs
            double tauGEO = 1.0;
            double tauGEOvar = 1.25;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // Define que o algoritmo irá executar até atingir certa precisão e retornará o NFE atingido
            // const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.001;

            // // Percorre a lista de TAUs e executa os algoritmos
            // List<double> valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(valores_TAU, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
        }
        

        public static void Execucoes_DeJong3(){
            // Parâmetros da função
            const int n_variaveis_projeto = 5;
            List<int> bits_por_variavel_variaveis = new List<int>(){11,11,11,11,11};
            List<double> limites_inferiores_variaveis = new List<double>(){-5.12,-5.12,-5.12,-5.12,-5.12};
            List<double> limites_superiores_variaveis = new List<double>(){5.12,5.12,5.12,5.12,5.12};
            double fx_esperado = -25;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 2;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 50000;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 250;

            // Define os TAUs
            double tauGEO = 3.0;
            double tauGEOvar = 8.0;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // Define que o algoritmo irá executar até certo NFOB e a execução irá retornar o melhor f(x)
            // const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.001;

            // // Percorre a lista de TAUs e executa os algoritmos
            // List<double> valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(valores_TAU, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
        }


        public static void Execucoes_Rastringin(){
            // Parâmetros da função
            const int n_variaveis_projeto = 20;
            List<int> bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            List<double> limites_inferiores_variaveis = Enumerable.Repeat(-5.12, n_variaveis_projeto).ToList();
            List<double> limites_superiores_variaveis = Enumerable.Repeat(5.12, n_variaveis_projeto).ToList();
            double fx_esperado = 0.0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 4;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1e6;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;

            // Define os TAUs
            double tauGEO = 1.0;
            double tauGEOvar = 1.75;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);
        }


        public static void Execucoes_Schwefel(){
            // Parâmetros da função
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            List<double> limites_inferiores_variaveis = Enumerable.Repeat(-500.0, n_variaveis_projeto).ToList();
            List<double> limites_superiores_variaveis = Enumerable.Repeat(500.0, n_variaveis_projeto).ToList();
            double fx_esperado = 0.0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 5;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1e6;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;

            // Define os TAUs
            double tauGEO = 1.00;
            double tauGEOvar = 1.75;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);
        }


        public static void Execucoes_Ackley(){
            // Parâmetros da função
            const int n_variaveis_projeto = 30;
            List<int> bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            List<double> limites_inferiores_variaveis = Enumerable.Repeat(-30.0, n_variaveis_projeto).ToList();
            List<double> limites_superiores_variaveis = Enumerable.Repeat(30.0, n_variaveis_projeto).ToList();
            double fx_esperado = 0.0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 6;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1e6+1;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;

            // Define os TAUs
            double tauGEO = 2.25;
            double tauGEOvar = 2.50;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);
        }


        public static void Execucoes_F9(){
            // Parâmetros da função
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = Enumerable.Repeat(18, n_variaveis_projeto).ToList();
            List<double> limites_inferiores_variaveis = Enumerable.Repeat(-10.0, n_variaveis_projeto).ToList();
            List<double> limites_superiores_variaveis = Enumerable.Repeat(10.0, n_variaveis_projeto).ToList();
            double fx_esperado = 1.0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 7;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1e6;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 10000;

            // Define os TAUs
            double tauGEO = 1.5;
            double tauGEOvar = 1.75;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // Define que o algoritmo irá executar até atingir certa precisão e retornará o NFE atingido
            // const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.0001;

            // // Percorre a lista de TAUs e executa os algoritmos
            // List<double> valores_TAU = new List<double>(){1.5, 1.75};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(valores_TAU, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
        }

        // =====================================
        // SPACECRAFT OPTIMIZATION
        // =====================================

        public static void ExtensiveSearch_SpacecraftOptimization(){
            double menor_fx_historia = Double.MaxValue;
            double menor_i_historia = Double.MaxValue;
            double menor_n_historia = Double.MaxValue;
            double menor_d_historia = Double.MaxValue;

            for (int i = 13; i <= 15; i++){
                for (int d = 1; d <= 60; d++){
                    for (int n = 1; n <= d; n++){
                        
                        // Se o espaço for viável, executa
                        if( (n < d) && ((double)n%d != 0) ) { //&& (Satellite.Payload.FOV >= 1.05*FovMin);
                            
                            // Monta a lista de fenótipos
                            List<double> fenotipo_variaveis_projeto = new List<double>(){i,d,n};
                            
                            // Executa diretamente a função objetivo
                            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
                            // Console.WriteLine("Espaço válido! i="+i+"; n="+n+"; d:"+d+"; fx="+fx);

                            // Verifica se essa execução é a melhor da história
                            if (fx < menor_fx_historia){
                                menor_fx_historia = fx;
                                menor_i_historia = i;
                                menor_n_historia = n;
                                menor_d_historia = d;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Menor fx história: " + menor_fx_historia);
            Console.WriteLine("Menor i história: " + menor_i_historia);
            Console.WriteLine("Menor n história: " + menor_n_historia);
            Console.WriteLine("Menor d história: " + menor_d_historia);
        }

        public static void Teste_FuncoesObjetivo_SpacecraftOptimization(){
            
            // Define qual função chamar e o fenótipo
            int definicao_funcao_objetivo = 3;
            List<double> fenotipo_variaveis_projeto = new List<double>(){14,60,59};
            
            // =========================================================
            // Calcula a função objetivo com a rotina de FOs
            // =========================================================
            
            double melhor_fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, definicao_funcao_objetivo);

            Console.WriteLine("Melhor fx função switch case: {0}", melhor_fx);


            // =========================================================
            // Calcula a função objetivo diretamente
            // =========================================================

            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);

            Console.WriteLine("Fx Final Função diretamente: {0}", fx);
        }

        public static void Execucoes_SpacecraftOptimization(){
            
            // Parâmetros da função
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,0};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};
            double fx_esperado = 196.949433192159;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // // Parâmetros de execução do algoritmo
            // const double valor_criterio_parada = 1500;
            // // Define o passo para a obtenção dos f(x) a cada NFOB
            // int step_obter_NFOBs = 50;

            // // Define os TAUs
            // double tauGEO = 3.0;
            // double tauGEOvar = 8.0;

            // // Executa todos os algoritmos
            // Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // Define que o algoritmo irá executar até certo NFOB e a execução irá retornar o melhor f(x)
            // const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.0000001;

            // // Percorre a lista de TAUs e executa os algoritmos
            // // Define o critério de parada com o número de avaliações da NFOBs
            // // List<double> valores_TAU = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5};
            // List<double> valores_TAU = new List<double>(){0.75, 1, 1.25, 1.5};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAU(valores_TAU, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);


            // ========================================
            // OBTÉM MELHORES F(X) VARIANDO TAU
            // ========================================

            // NFOB para a execução encerrar
            const double valor_criterio_parada = 0.0000001;

            // Define os TAUs
            double tauGEO = 1.0;
            double tauGEOvar = 1.0;

            Executa_Todos_Algoritmos_Por_NFE(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, fx_esperado);
        }
        
        

        // =====================================
        // MAIN
        // =====================================

        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            // Execucoes_Griewangk();
            // Execucoes_Rosenbrock();
            // Execucoes_DeJong3();
            // Execucoes_Rastringin();
            // Execucoes_Schwefel();
            // Execucoes_Ackley();
            // Execucoes_F9();

            // ExtensiveSearch_SpacecraftOptimization();
            // Teste_FuncoesObjetivo_SpacecraftOptimization();
            Execucoes_SpacecraftOptimization();
        }
    }
}