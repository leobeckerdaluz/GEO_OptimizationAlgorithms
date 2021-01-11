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
using static GEO.GEO;

namespace Execucoes
{
    public class Execucoes_GEO
    {
        public static void GEOvar_Griewangk(){
            // Parâmetros de execução do algoritmo
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};

            const int n_variaveis_projeto = 10;
            
            // const double function_min = -600.0;
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};

            // const double function_max = 600.0;
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};

            // Se o TAO é alto, é mais determinístico. Se o TAO é baixo, é mais estocástico
            const double tao = 0.5;
            // Define o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 
            const int tipo_GEO = 1; 
            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 200000;
            const double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            const int definicao_funcao_objetivo = 0;

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            

            //============================================================
            // Execução do algoritmo
            //============================================================

            // Executa o GEO e recebe como retorno a melhor fitness da execução
            List<double> melhores_NFOBs = GEO_algorithm(tipo_GEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
            
            foreach(double melhor_NFOB in melhores_NFOBs){
                Console.WriteLine(melhor_NFOB);
            }
        }


        public static void GEOvar_SpaceDesign(){
            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};

            // Se o TAO é alto, é mais determinístico. Se o TAO é baixo, é mais estocástico
            const double tao = 1;
            // Define o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 
            const int tipo_GEO = 1; 
            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.1;
            double fx_esperado = 196.949;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            
            // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            //============================================================
            // Execução do algoritmo
            //============================================================

            // Executa o GEO e recebe como retorno a melhor fitness da execução
            List<double> melhores_NFOBs = GEO_algorithm(tipo_GEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
            
            foreach(double melhor_NFOB in melhores_NFOBs){
                Console.WriteLine(melhor_NFOB);
            }
        }


        public static void GEOs_variandoTAO_F2_Rosenbrock(){
            // ================================================================
            // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F2
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 000.1;
            double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 1;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<double> valores_TAO_F2 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};
            List<int> NFOBs_desejados = new List<int>(){400000};
            
            foreach (double tao in valores_TAO_F2){
                Console.WriteLine("===> TAO: " + tao + "   |   GEOcan / GEOvar");
                
                // Executa o SGA por 50 vezes
                double somatorio_nro_avaliacoes_FO_encontrar_global_GEO = 0;
                double somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar = 0;

                // Executa o GEO e o GEOvar por 5 vezes
                for(int i=0; i<50; i++){
                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
                double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

                string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
                string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine(string_media_nro_avaliacoes_GEO);
                Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            }
        }
        

        public static void GEOs_variandoTAO_F3_DeJong3(){
            // ================================================================
            // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F3
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 5;
            List<int> bits_por_variavel_variaveis = new List<int>(){11,11,11,11,11};
            List<double> limites_inferiores_variaveis = new List<double>(){-5.12,-5.12,-5.12,-5.12,-5.12};
            List<double> limites_superiores_variaveis = new List<double>(){5.12,5.12,5.12,5.12,5.12};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.01;
            double fx_esperado = -25;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 2;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<double> valores_TAO_F3 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};
            List<int> NFOBs_desejados = new List<int>(){400000};
            
            foreach (double tao in valores_TAO_F3){
                Console.WriteLine("===> TAO: " + tao + "   |   GEOcan / GEOvar");
                
                // Executa o SGA por 50 vezes
                double somatorio_nro_avaliacoes_FO_encontrar_global_GEO = 0;
                double somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar = 0;

                // Executa o GEO e o GEOvar por 5 vezes
                for(int i=0; i<50; i++){
                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
                double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

                string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
                string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine(string_media_nro_avaliacoes_GEO);
                Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            }
        }


        public static void GEOs_variandoTAO_F5_Griewangk(){
            // ================================================================
            // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F5
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 5;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 100000;
            double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<double> valores_TAO_F5 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
            List<int> NFOBs_desejados = new List<int>(){400000};
            
            foreach (double tao in valores_TAO_F3){
                Console.WriteLine("===> TAO: " + tao + "   |   GEOcan / GEOvar");
                
                // Executa o SGA por 50 vezes
                double somatorio_melhores_GEO = 0;
                double somatorio_melhores_GEOvar = 0;

                // Executa o GEO e o GEOvar por 5 vezes
                for(int i=0; i<50; i++){
                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_melhores_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_melhores_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_melhor_fx_GEO = somatorio_melhores_GEO / 50;
                double media_melhor_fx_GEOvar = somatorio_melhores_GEOvar / 50;

                string string_media_melhor_fx_GEO = (media_melhor_fx_GEO.ToString()).Replace('.',',');
                string string_media_melhor_fx_GEOvar = (media_melhor_fx_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine(string_media_melhor_fx_GEO);
                Console.WriteLine(string_media_melhor_fx_GEOvar);
            }
        }

        
        public static void GEO_TAO1_e_GEOvar_TAO125_F2_Rosenbrock(){
            // // ================================================================
            // // 50 EXECUÇÕES para TAO=1 GEO e TAO=1,25 GEOvar para F2
            // // ================================================================

            // // Parâmetros de execução do algoritmo
            // const int bits_por_variavel_projeto = 13;
            // const int n_variaveis_projeto = 2;
            // const double fx_esperado = 0;
            // const double function_min = -2.048;
            // const double function_max = 2.048;
            // // Define o critério de parada com o número de avaliações da NFOBs
            // const double valor_criterio_parada = 15000;
            // List<int> NFOBs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000,10500,11000,11500,12000,12500,13000,13500,14000,14500,15000};
            
            // const double tao_GEO = 1.0;
            // const double tao_GEOvar = 1.25;
            // Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            // List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            // List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // // Executa o GEO e o GEOvar por 5 vezes
            // for(int i=0; i<50; i++){
            //     // Console.WriteLine("Rodando " + i);
            //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            //     List<double> NFOBs_results_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            //     NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

            //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            //     List<double> NFOBs_results_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado);
                
            //     NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            // }

            // // Para cada NFOB desejado, calcula a média das N execuções
            // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            // for(int i=0; i<NFOBs_desejados.Count; i++){
            //     double sum = 0;
            //     // Percorre a lista de cada execução para fazer o somatório
            //     foreach(List<double> execution in NFOBs_all_results_GEO){
            //         sum += execution[i];
            //     }
            //     double media = sum / (double)NFOBs_all_results_GEO.Count;

            //     string string_media = (media.ToString()).Replace('.',',');
            //     Console.WriteLine(string_media);
            // }

            // // Para cada NFOB desejado, calcula a média das N execuções
            // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            // for(int i=0; i<NFOBs_desejados.Count; i++){
            //     double sum = 0;
            //     // Percorre a lista de cada execução para fazer o somatório
            //     foreach(List<double> execution in NFOBs_all_results_GEOvar){
            //         sum += execution[i];
            //     }
            //     double media = sum / (double)NFOBs_all_results_GEOvar.Count;

            //     string string_media = (media.ToString()).Replace('.',',');
            //     Console.WriteLine(string_media);
            // }
        }


        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            List<double> fenotipo_variaveis_projeto = new List<double>(){13,50,60};
            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
            
            Console.WriteLine("Fx Final: " + fx);




            


            

            // // Cria lista para armazenar os valores fitness a cada NFOB desejado
            // List<List<double>> todas_execucoes_SGA_NFOB = new List<List<double>>();

            // // Apresenta o melhor resultado
            // Console.WriteLine("Execução " + i + ": " + SGA_bests_NFOB[SGA_bests_NFOB.Count - 1]);
            // // Adiciona na lista de execuções a execução atual
            // todas_execucoes_SGA_NFOB.Add(SGA_bests_NFOB);

            // List<double> valores_TAO_F3 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};
            



           






            






            






            // // // ================================================================
            // // // 50 EXECUÇÕES para TAO=3 GEO e TAO=8 GEOvar para F3
            // // // ================================================================

            // // // Parâmetros de execução do algoritmo
            // // const int bits_por_variavel_projeto = 11;
            // // const int n_variaveis_projeto = 5;
            // // const double fx_esperado = -25;
            // // const double function_min = -5.12;
            // // const double function_max = 5.12;
            // // // Define o critério de parada com o número de avaliações da NFOBs
            
            // // // const double valor_criterio_parada = 15000;
            // // // List<int> NFOBs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000,10500,11000,11500,12000,12500,13000,13500,14000,14500,15000};
            // // const double valor_criterio_parada = 2201;
            // // List<int> NFOBs_desejados = new List<int>(){100,200,300,400,500,600,700,800,900,1000,1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200};

            // // const double tao_GEO = 3.0;
            // // const double tao_GEOvar = 8.0;
            // // Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            // // List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            // // List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // // // Executa o GEO e o GEOvar por 5 vezes
            // // for(int i=0; i<50; i++){
            // //     // Console.WriteLine("Rodando " + i);
            // //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            // //     List<double> NFOBs_results_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            // //     NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

            // //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            // //     List<double> NFOBs_results_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado);
                
            // //     NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            // // }

            // // // Para cada NFOB desejado, calcula a média das N execuções
            // // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            // // for(int i=0; i<NFOBs_desejados.Count; i++){
            // //     double sum = 0;
            // //     // Percorre a lista de cada execução para fazer o somatório
            // //     foreach(List<double> execution in NFOBs_all_results_GEO){
            // //         sum += execution[i];
            // //     }
            // //     double media = sum / (double)NFOBs_all_results_GEO.Count;

            // //     string string_media = (media.ToString()).Replace('.',',');
            // //     Console.WriteLine(string_media);
            // // }

            // // // Para cada NFOB desejado, calcula a média das N execuções
            // // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            // // for(int i=0; i<NFOBs_desejados.Count; i++){
            // //     double sum = 0;
            // //     // Percorre a lista de cada execução para fazer o somatório
            // //     foreach(List<double> execution in NFOBs_all_results_GEOvar){
            // //         sum += execution[i];
            // //     }
            // //     double media = sum / (double)NFOBs_all_results_GEOvar.Count;

            // //     string string_media = (media.ToString()).Replace('.',',');
            // //     Console.WriteLine(string_media);
            // // }






            // // // ================================================================
            // // // 50 EXECUÇÕES para TAO=1,25 GEO e TAO=3 GEOvar para F5
            // // // ================================================================

            // // // Parâmetros de execução do algoritmo
            // // const int bits_por_variavel_projeto = 14;
            // // const int n_variaveis_projeto = 10;
            // // const double fx_esperado = 0;
            // // const double function_min = -600.0;
            // // const double function_max = 600.0;
            // // // Define o critério de parada com o número de avaliações da NFOBs
            // // const double valor_criterio_parada = 100000;
            // // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};

            // // const double tao_GEO = 1.25;
            // // const double tao_GEOvar = 3.0;
            // // Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            // // List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            // // List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // // // Executa o GEO e o GEOvar por 5 vezes
            // // for(int i=0; i<50; i++){
            // //     Console.WriteLine("Rodando " + i);

            // //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            // //     List<double> NFOBs_results_GEO = GEO_algorithm(0, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado);

            // //     NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

            // //     // Executa o GEO e recebe como retorno a melhor fitness da execução
            // //     List<double> NFOBs_results_GEOvar = GEO_algorithm(1, n_variaveis_projeto, bits_por_variavel_projeto, definicao_funcao_objetivo, function_min, function_max, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado);
                
            // //     NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            // // }

            // // // Para cada NFOB desejado, calcula a média das N execuções
            // // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            // // for(int i=0; i<NFOBs_desejados.Count; i++){
            // //     double sum = 0;
            // //     // Percorre a lista de cada execução para fazer o somatório
            // //     foreach(List<double> execution in NFOBs_all_results_GEO){
            // //         sum += execution[i];
            // //     }
            // //     double media = sum / (double)NFOBs_all_results_GEO.Count;

            // //     string string_media = (media.ToString()).Replace('.',',');
            // //     Console.WriteLine(string_media);
            // // }

            // // // Para cada NFOB desejado, calcula a média das N execuções
            // // Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            // // for(int i=0; i<NFOBs_desejados.Count; i++){
            // //     double sum = 0;
            // //     // Percorre a lista de cada execução para fazer o somatório
            // //     foreach(List<double> execution in NFOBs_all_results_GEOvar){
            // //         sum += execution[i];
            // //     }
            // //     double media = sum / (double)NFOBs_all_results_GEOvar.Count;

            // //     string string_media = (media.ToString()).Replace('.',',');
            // //     Console.WriteLine(string_media);
            // // }
        }
    }
}