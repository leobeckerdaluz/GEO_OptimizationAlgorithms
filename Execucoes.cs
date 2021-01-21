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

        public static double Obtem_Media_Execucoes_NFE(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tao, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){
            
            double somatorio_resultados = 0;

            // Executa o algoritmo por N vezes
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"...");

                // Executa o algoritmo desejado e recebe como retorno apenas um valor (melhor fitness da execução ou NFE atingido), conforme o critério de parada.
                List<double> retorno_algoritmo = GEO.GEO.GEO_algorithm(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                
                // Como o retorno é somente um número, obtém o índice 0
                somatorio_resultados += retorno_algoritmo[0];
            }

            // Calcula e retorna a média dos valores recebidos durante a execução
            double media_resultado = somatorio_resultados / numero_execucoes;
            return media_resultado;
        }
        
        
        public static List<double> Obtem_FxsMedios_NFOB_Execucoes(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tao, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){
            
            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"... ");

                // Executa o algoritmo
                List<double> NFOBs_results_GEO = GEO.GEO.GEO_algorithm(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

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
        

        public static void Executa_Todos_Algoritmos_Por_NFOBs(int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double taoGEO, double taoGEOvar, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado){

            // Inicializa as variáveis para cada execução
            double tao = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // O critério de parada vai ser quando atingir um NFOB
            int criterio_parada_NFOBouNFEouMELHORFX = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            Console.Write("\nGEO...");
            tao = taoGEO;
            tipo_GEO = 0;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEO = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            // GEOvar
            Console.Write("\nGEOvar...");
            tao = taoGEOvar;
            tipo_GEO = 1;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEOvar = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            // AGEO1
            Console.Write("\nAGEO1...");
            tipo_GEO = 0;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            // AGEO2
            Console.Write("\nAGEO2...");
            tipo_GEO = 0;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            // AGEO1var
            Console.Write("\nAGEO1var...");
            tipo_GEO = 1;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            // AGEO2var
            Console.Write("\nAGEO2var...");
            tipo_GEO = 1;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

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
        

        public static void Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAO(List<double> valores_TAO, int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double valor_criterio_parada, double fx_esperado, int criterio_parada_NFOBouNFEouMELHORFX){
            
            // Inicializa as variáveis para cada execução
            int tipo_GEO = 0;
            int tipo_AGEO = 0;
            
            // Não será necessário obter melhor f(x) a cada NFOB
            int step_obter_NFOBs = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            string resultados = "";

            // Para cada TAO, executa os algoritmos    
            foreach(double tao in valores_TAO){
                Console.Write("\nTAO = {0}", tao);

                // GEO
                Console.Write("\nGEO...");
                tipo_GEO = 0;
                tipo_AGEO = 0;
                double media_GEO = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                
                // GEOvar
                Console.Write("\nGEOvar...");
                tipo_GEO = 1;
                tipo_AGEO = 0;
                double media_GEOvar = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

                // ===========================================================
                // Mostra a média dos NFE nas execuções
                // ===========================================================

                string str_tao = (tao.ToString()).Replace('.',',');
                string str_media_NFE_GEO = (media_GEO.ToString()).Replace('.',',');
                string str_media_NFE_GEOvar = (media_GEOvar.ToString()).Replace('.',',');
                
                string result = String.Format("{0};{1};{2}", str_tao, str_media_NFE_GEO, str_media_NFE_GEOvar);

                resultados += result + "\n";
            }

            Console.WriteLine("");
            Console.WriteLine("===> Médias NFE:");
            Console.WriteLine("TAO;GEO;GEOvar");
            Console.WriteLine(resultados);
        }
            


        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public static void Execucoes_Griewangk(){
            // Parâmetros da função
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Define a quantidade de execuções
            int quantidade_execucoes = 10;

            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 100000;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 500;
            
            // Define os TAOs
            double taoGEO = 1.25;
            double taoGEOvar = 3;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAO
            // // ========================================

            // // Define que o algoritmo irá executar até certo NFOB e a execução irá retornar o melhor f(x)
            // const int criterio_parada_NFOBouNFEouMELHORFX = 2;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 100000;

            // // Percorre a lista de TAOs e executa os algoritmos
            // List<double> valores_TAO = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAO(valores_TAO, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
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
            const double valor_criterio_parada = 10000;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 250;

            // Define os TAOs
            double taoGEO = 1.0;
            double taoGEOvar = 1.25;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAO
            // // ========================================

            // // Define que o algoritmo irá executar até atingir certa precisão e retornará o NFE atingido
            // const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.001;

            // // Percorre a lista de TAOs e executa os algoritmos
            // List<double> valores_TAO = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};

            // Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAO(valores_TAO, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
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


            // // ========================================
            // // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // // ========================================

            // // Parâmetros de execução do algoritmo
            // const double valor_criterio_parada = 1500;
            // // Define o passo para a obtenção dos f(x) a cada NFOB
            // int step_obter_NFOBs = 50;

            // // Define os TAOs
            // double taoGEO = 3.0;
            // double taoGEOvar = 8.0;

            // // Executa todos os algoritmos
            // Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado);


            // ========================================
            // OBTÉM MELHORES F(X) VARIANDO TAO
            // ========================================

            // Define que o algoritmo irá executar até certo NFOB e a execução irá retornar o melhor f(x)
            const int criterio_parada_NFOBouNFEouMELHORFX = 1;
            // NFOB para a execução encerrar
            const double valor_criterio_parada = 0.01;

            // Percorre a lista de TAOs e executa os algoritmos
            List<double> valores_TAO = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};

            Executa_Todos_Algoritmos_NFE_OU_MELHORFXporTAO(valores_TAO, quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, valor_criterio_parada, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
        }



        // =====================================
        // SPACECRAFT OPTIMIZATION
        // =====================================
        public static void AGEO_PRECISION_SpacecraftOptimization(){
            // ================================================================
            // EXECUÇÕES para AGEO para SpacecraftOptimization
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.0000001;
            double fx_esperado = 196.949433192159;
            const int criterio_parada_NFOBouNFEouMELHORFX = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;
            
            // Define o critério de parada com o número de avaliações da NFOBs
            // List<int> NFOBs_desejados = new List<int>(){15000};
            int step_obter_NFOBs = 0;

            const double tao = 1.0;

            int numero_execucoes = 50;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;


            Settings.SolarModes = new List<SolarModeModel>();
            Settings.SolarModes.Add(new SolarModeModel(SolarMode.High, 1));
            Settings.SolarModes.Add(new SolarModeModel(SolarMode.MediumHigh, 3));

            Settings.LaunchAltitudeError = 20000;
            Settings.LaunchInclinationError = (0.15).DegreesToRadians();
            // Settings.LaunchInclinationError = (0.015).DegreesToRadians();
            Settings.ISP = 225;
            Settings.PrecisionPropulsion = 10e-5;

            // Settings.AreaPercentOverSectionalArea = 4.37;
            Settings.AreaPercentOverSectionalArea = 1.8;

            Settings.V0 = 6.778309031049825E+03;
            Settings.MissionResolution = 20;



            double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

            string string_media_nro_avaliacoes = (media_nro_avaliacoes.ToString()).Replace('.',',');
            
            Console.WriteLine("");
            Console.WriteLine(string_media_nro_avaliacoes);
        }

        public static void GEOs_variandoTAO_SpacecraftOptimization(){

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.0000001;
            double fx_esperado = 196.949433192159;
            const int criterio_parada_NFOBouNFEouMELHORFX = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<double> valores_TAO = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5};
            // List<int> NFOBs_desejados = new List<int>(){400000};
            int step_obter_NFOBs = 0;
            
            const int numero_execucoes = 10;
            const int tipo_GEO = 0;
            const int tipo_AGEO = 0;

            foreach (double tao in valores_TAO){
                Console.WriteLine("===> TAO: " + tao);

                double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);

                string string_media_nro_avaliacoes = (media_nro_avaliacoes.ToString()).Replace('.',',');
                
                Console.WriteLine("");
                Console.WriteLine(string_media_nro_avaliacoes);
            }
        }

        public static void SpacecraftOptimization_ExtensiveSearch(){
            double menor_fx_historia = Double.MaxValue;
            double menor_i_historia = Double.MaxValue;
            double menor_n_historia = Double.MaxValue;
            double menor_d_historia = Double.MaxValue;

            for (int i = 13; i <= 15; i++){
                for (int d = 1; d <= 60; d++){
                    for (int n = 1; n <= 59; n++){
                        // Se a condição for válida, executa
                        if( (n < d) && ((double)n%d != 0) ) { //&& (Satellite.Payload.FOV >= 1.05*FovMin);
                            // Monta a lista de fenótipos
                            List<double> fenotipo_variaveis_projeto = new List<double>(){i,n,d};
                            // Executa diretamente a função objetivo
                            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
                            // Console.WriteLine("Fx Final: " + fx);
                            // Console.WriteLine("Espaço válido! i="+i+"; n="+n+"; d:"+d+"; fx="+fx);

                            if (fx < menor_fx_historia){
                                menor_fx_historia = fx;
                                menor_i_historia = i;
                                menor_n_historia = n;
                                menor_d_historia = d;
                            }
                        }
                        else{
                            // Console.WriteLine("Espaço inválido! i="+i+"; n="+n+"; d:"+d+"; fx="+fx);
                        }
                    }
                }
            }

            Console.WriteLine("Menor fx história: " + menor_fx_historia);
            Console.WriteLine("Menor i história: " + menor_i_historia);
            Console.WriteLine("Menor n história: " + menor_n_historia);
            Console.WriteLine("Menor d história: " + menor_d_historia);
        }

        public static void TesteFuncaoObjetivoPopulacao_SpacecraftOptimization(){
            
            // Define qual função chamar e o fenótipo
            int definicao_funcao_objetivo = 3;
            List<double> fenotipo_variaveis_projeto = new List<double>(){14,60,59};
            
            // =========================================================
            // Calcula a função objetivo utilizando a populacao
            // =========================================================
            
            double melhor_fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, definicao_funcao_objetivo);

            Console.WriteLine("Melhor fx função switch case: {0}", melhor_fx);


            // =========================================================
            // Calcula a função objetivo diretamente com o fenótipo
            // =========================================================

            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);

            Console.WriteLine("Fx Final Função diretamente: {0}", fx);
        }

        public static void GEOcan_GEOvar_SpaceDesign(){
            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};

            // Se o TAO é alto, é mais determinístico. Se o TAO é baixo, é mais estocástico
            const double tao = 1;
            // Define o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 
            const int tipo_GEO = 0; 
            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 1000000;
            double fx_esperado = 196.949;
            const int criterio_parada_NFOBouNFEouMELHORFX = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};
            
            int step_obter_NFOBs = 250;
            // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            //============================================================
            // Execução do algoritmo
            //============================================================

            for(int i=0; i<100; i++){
                // Executa o GEO e recebe como retorno a melhor fitness da execução
                List<double> melhores_NFOBs = GEO.GEO.GEO_algorithm(tipo_GEO, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, criterio_parada_NFOBouNFEouMELHORFX);
                
                // melhores_NFOBs

                foreach(double melhor_NFOB in melhores_NFOBs){
                    string melhor_NFOB_virgula = (melhor_NFOB.ToString()).Replace('.',',');
                    Console.WriteLine(melhor_NFOB_virgula);
                }
            }
        }


        // =====================================
        // MAIN
        // =====================================

        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            // Execucoes_Griewangk();
            // Execucoes_Rosenbrock();
            Execucoes_DeJong3();

            // SpacecraftOptimization_ExtensiveSearch();
            // GEOs_variandoTAO_SpacecraftOptimization();
            // TesteFuncaoObjetivoPopulacao_SpacecraftOptimization();
        }
    }
}