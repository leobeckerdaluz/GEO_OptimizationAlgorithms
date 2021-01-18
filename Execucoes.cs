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
        public static double Obtem_Media_Execucoes_NFE(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tao, double valor_criterio_parada, List<int> NFOBs_desejados, double fx_esperado, int CRITERIO_PARADA_NFOBouPRECISAO){
            // Executa o SGA por N vezes
            double somatorio_nro_avaliacoes_FO_encontrar_global_AGEO = 0;

            // Executa o AGEO
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"... ");

                // Executa o AGEO e recebe como retorno a melhor fitness da execução
                List<double> retorno_AGEO = GEO.GEO.GEO_algorithm(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                
                // Como o retorno é somente o número de avaliações médio, então obtém o número
                somatorio_nro_avaliacoes_FO_encontrar_global_AGEO += retorno_AGEO[0];
            }

            // Calcula a média dos melhores f(x) pra esse TAO
            double media_nro_avaliacoes_AGEO = somatorio_nro_avaliacoes_FO_encontrar_global_AGEO / numero_execucoes;

            // Retorna a média de NFE atingido na execução
            return media_nro_avaliacoes_AGEO;
        }
        
        
        public static void GEOcan_GEOvar_Griewangk(){
            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};

            // Se o TAO é alto, é mais determinístico. Se o TAO é baixo, é mais estocástico
            const double tao = 5;
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
            List<double> melhores_NFOBs = GEO.GEO.GEO_algorithm(tipo_GEO, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
            
            foreach(double melhor_NFOB in melhores_NFOBs){
                string melhor_NFOB_virgula = (melhor_NFOB.ToString()).Replace('.',',');
                Console.WriteLine(melhor_NFOB_virgula);
            }
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
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Essa lista contém todos os pontos NFOB onde se deseja saber o valor fitness do algoritmo. 
            // O algoritmo mostra o melhor fitness até o momento assim que o NFOB atinge cada um destes valores.

            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};
            
            // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            //============================================================
            // Execução do algoritmo
            //============================================================

            for(int i=0; i<100; i++){
                // Executa o GEO e recebe como retorno a melhor fitness da execução
                List<double> melhores_NFOBs = GEO.GEO.GEO_algorithm(tipo_GEO, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                
                // melhores_NFOBs

                foreach(double melhor_NFOB in melhores_NFOBs){
                    string melhor_NFOB_virgula = (melhor_NFOB.ToString()).Replace('.',',');
                    Console.WriteLine(melhor_NFOB_virgula);
                }
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
            const double valor_criterio_parada = 0.001;
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
                    Console.Write((i+1) +"... ");

                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
                double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

                string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
                string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine("");
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
                    Console.Write((i+1) +"... ");

                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / 50;
                double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / 50;

                string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
                string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine("");
                Console.WriteLine(string_media_nro_avaliacoes_GEO);
                Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            }
        }


        public static void GEOs_variandoTAO_F5_Griewangk(){
            // ================================================================
            // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na F5
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 10;
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
            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};
            
            foreach (double tao in valores_TAO_F5){
                Console.WriteLine("===> TAO: " + tao + "   |   GEOcan / GEOvar");
                
                // Executa o SGA por 50 vezes
                double somatorio_melhores_GEO = 0;
                double somatorio_melhores_GEOvar = 0;

                // Executa o GEO e o GEOvar por 5 vezes
                for(int i=0; i<50; i++){
                    Console.Write((i+1) +"... ");

                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_melhores_GEO += retorno_GEO[retorno_GEO.Count-1];


                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_melhores_GEOvar += retorno_GEOvar[retorno_GEOvar.Count-1];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_melhor_fx_GEO = somatorio_melhores_GEO / 50;
                double media_melhor_fx_GEOvar = somatorio_melhores_GEOvar / 50;

                string string_media_melhor_fx_GEO = (media_melhor_fx_GEO.ToString()).Replace('.',',');
                string string_media_melhor_fx_GEOvar = (media_melhor_fx_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine("");
                Console.WriteLine(string_media_melhor_fx_GEO);
                Console.WriteLine(string_media_melhor_fx_GEOvar);
            }
        }

        
        public static void GEO_TAO1_e_GEOvar_TAO125_F2_Rosenbrock(){
            // ================================================================
            // 50 EXECUÇÕES para TAO=1 GEO e TAO=1,25 GEOvar para F2
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 15000;
            double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 1;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<int> NFOBs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,5500,6000,6500,7000,7500,8000,8500,9000,9500,10000,10500,11000,11500,12000,12500,13000,13500,14000,14500,15000};

            // Define os valores de tao para as execuções
            const double tao_GEO = 1.0;
            const double tao_GEOvar = 1.25;
            Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<50; i++){
                Console.Write((i+1) +"... ");

                // Executa o GEO
                List<double> NFOBs_results_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

                // Executa o GEOvar
                List<double> NFOBs_results_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            }
            Console.WriteLine("");

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEO){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEO.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEOvar){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEOvar.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }
        }


        public static void GEO_TAO3_e_GEOvar_TAO8_F3_DeJong3(){
            // ================================================================
            // 50 EXECUÇÕES para TAO=3 GEO e TAO=8 GEOvar para F3
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 5;
            List<int> bits_por_variavel_variaveis = new List<int>(){11,11,11,11,11};
            List<double> limites_inferiores_variaveis = new List<double>(){-5.12,-5.12,-5.12,-5.12,-5.12};
            List<double> limites_superiores_variaveis = new List<double>(){5.12,5.12,5.12,5.12,5.12};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 2201;
            double fx_esperado = -25;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 2;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<int> NFOBs_desejados = new List<int>(){100,200,300,400,500,600,700,800,900,1000,1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200};

            const double tao_GEO = 3.0;
            const double tao_GEOvar = 8.0;
            Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<50; i++){
                Console.Write((i+1) +"... ");

                // Executa o GEO
                List<double> NFOBs_results_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

                // Executa o GEOvar
                List<double> NFOBs_results_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            }
            Console.WriteLine("");

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEO){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEO.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEOvar){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEOvar.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }
        }


        public static void GEO_TAO125_e_GEOvar_TAO3_F5_Griewangk(){
            // ================================================================
            // 50 EXECUÇÕES para TAO=1,25 GEO e TAO=3 GEOvar para F5
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 10;
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
            List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};

            const double tao_GEO = 1.25;
            const double tao_GEOvar = 3.0;
            Console.WriteLine("===> taoGEO: "+tao_GEO+" | taoGEOvar: "+tao_GEOvar);

            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            List<List<double>> NFOBs_all_results_GEOvar = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<50; i++){
                Console.Write((i+1) +"... ");

                // Executa o GEO
                List<double> NFOBs_results_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEO, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEO.Add(NFOBs_results_GEO);

                // Executa o GEOvar
                List<double> NFOBs_results_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao_GEOvar, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

                NFOBs_all_results_GEOvar.Add(NFOBs_results_GEOvar);
            }
            Console.WriteLine("");

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEO:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEO){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEO.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }

            // Para cada NFOB desejado, calcula a média das N execuções
            Console.WriteLine("===> Médias das 50 execuções para cada NFOB desejado no GEOvar:");
            for(int i=0; i<NFOBs_desejados.Count; i++){
                double sum = 0;
                // Percorre a lista de cada execução para fazer o somatório
                foreach(List<double> execution in NFOBs_all_results_GEOvar){
                    sum += execution[i];
                }
                double media = sum / (double)NFOBs_all_results_GEOvar.Count;

                string string_media = (media.ToString()).Replace('.',',');
                Console.WriteLine(string_media);
            }
        }


        public static void TesteFuncaoObjetivoPopulacao_SpacecraftOptimization(){
            
            // =========================================================
            // Calcula a função objetivo utilizando a populacao
            // =========================================================

            int definicao_funcao_objetivo = 3;
            List<bool> populacao_de_bits = new List<bool>(){
                true, false, 
                true, true, true, true, true, true, 
                true, true, true, true, true, true 
            };
            int n_variaveis_projeto = 3;
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            
            double melhor_fx = GEO.GEO.funcao_objetivo(definicao_funcao_objetivo, populacao_de_bits, n_variaveis_projeto, limites_inferiores_variaveis, limites_superiores_variaveis, bits_por_variavel_variaveis);
            Console.WriteLine("Melhor fx: {0}", melhor_fx);


            // =========================================================
            // Calcula a função objetivo diretamente com o fenótipo
            // =========================================================

            List<double> fenotipo_variaveis_projeto = new List<double>(){14,60,59};
            double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
            Console.WriteLine("Fx Final: {0}", fx);
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


        public static void GEOs_variandoTAO_SpacecraftOptimization(){
            // ================================================================
            // 50 EXECUÇÕES GEOcanonico e GEOvar para diferentes TAO na SpacecraftOptimization
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 3;
            List<int> bits_por_variavel_variaveis = new List<int>(){2,6,6};
            List<double> limites_inferiores_variaveis = new List<double>(){13,1,1};
            List<double> limites_superiores_variaveis = new List<double>(){15,60,59};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.0000001;
            double fx_esperado = 196.949433192159;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<double> valores_TAO = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5};
            List<int> NFOBs_desejados = new List<int>(){400000};
            
            foreach (double tao in valores_TAO){
                Console.WriteLine("===> TAO: " + tao + "   |   GEOcan / GEOvar");
                
                // Executa o SGA por 50 vezes
                double somatorio_nro_avaliacoes_FO_encontrar_global_GEO = 0;
                // double somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar = 0;

                // Executa o GEO e o GEOvar por 5 vezes
                int quantidade_execucoes = 50;
                for(int i=0; i<quantidade_execucoes; i++){
                    Console.Write((i+1) +"... ");

                    // O primeiro parâmetro é o tipo do GEO ==> 0-GEOcanonico | 1-GEOvar 

                    // Executa o GEO e recebe como retorno a melhor fitness da execução
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    // List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // // Como o retorno é somente o número de avaliações médio, então obtém o número
                    // somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar += retorno_GEOvar[0];
                }

                // Calcula a média dos melhores f(x) pra esse TAO
                double media_nro_avaliacoes_GEO = somatorio_nro_avaliacoes_FO_encontrar_global_GEO / quantidade_execucoes;
                // double media_nro_avaliacoes_GEOvar = somatorio_nro_avaliacoes_FO_encontrar_global_GEOvar / quantidade_execucoes;

                string string_media_nro_avaliacoes_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
                // string string_media_nro_avaliacoes_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
		        
                Console.WriteLine("");
                Console.WriteLine(string_media_nro_avaliacoes_GEO);
                // Console.WriteLine(string_media_nro_avaliacoes_GEOvar);
            }
        }


        public static void AGEO_PRECISION_F2_Rosenbrock(){
            // ================================================================
            // EXECUÇÕES para AGEO para F2
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.001;
            double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 1;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<int> NFOBs_desejados = new List<int>(){0,100000,200000,300000,400000,500000,600000,700000,800000,900000,1000000};

            // double tao = 0.5;
            double tao = 1;

            int numero_execucoes = 100;
            int tipo_GEO = 0;
            int tipo_AGEO = 2;

            double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            string string_media_nro_avaliacoes = (media_nro_avaliacoes.ToString()).Replace('.',',');
            
            Console.WriteLine("");
            Console.WriteLine(string_media_nro_avaliacoes);
        }


        public static void AGEO_PRECISION_F3_Griewangk(){
            // ================================================================
            // EXECUÇÕES para AGEO para F3
            // ================================================================

            // Parâmetros de execução do algoritmo
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};

            // Define o critério de parada com o número de avaliações da NFOBs
            const double valor_criterio_parada = 0.5;
            double fx_esperado = 0;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Define o critério de parada com o número de avaliações da NFOBs
            List<int> NFOBs_desejados = new List<int>(){15000};

            const double tao = 1.25;

            int numero_execucoes = 50;
            int tipo_GEO = 0;
            int tipo_AGEO = 2;

            double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            string string_media_nro_avaliacoes = (media_nro_avaliacoes.ToString()).Replace('.',',');
            
            Console.WriteLine("");
            Console.WriteLine(string_media_nro_avaliacoes);
        }


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
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

            // Definicao_funcao_objetivo
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 3;
            
            // Define o critério de parada com o número de avaliações da NFOBs
            List<int> NFOBs_desejados = new List<int>(){15000};

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



            double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, NFOBs_desejados, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            string string_media_nro_avaliacoes = (media_nro_avaliacoes.ToString()).Replace('.',',');
            
            Console.WriteLine("");
            Console.WriteLine(string_media_nro_avaliacoes);
        }


        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            // GEOcan_GEOvar_Griewangk();
            // GEOcan_GEOvar_SpaceDesign();
            
            // GEOs_variandoTAO_F2_Rosenbrock();
            // GEOs_variandoTAO_F3_DeJong3();
            // GEOs_variandoTAO_F5_Griewangk();
            // GEO_TAO1_e_GEOvar_TAO125_F2_Rosenbrock();
            // GEO_TAO3_e_GEOvar_TAO8_F3_DeJong3();
            // GEO_TAO125_e_GEOvar_TAO3_F5_Griewangk();



            // AGEO_PRECISION_F2_Rosenbrock();

            AGEO_PRECISION_F3_Griewangk();

            // AGEO_PRECISION_SpacecraftOptimization();



            // SpacecraftOptimization_ExtensiveSearch();

            // GEOs_variandoTAO_SpacecraftOptimization();

            // TesteFuncaoObjetivoPopulacao_SpacecraftOptimization();


            // List<double> fenotipo_variaveis_projeto = new List<double>(){14,59,60};
            // double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
            // Console.WriteLine("Fx Final: " + fx);
        }
    }
}