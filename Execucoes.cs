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
        public static double Obtem_Media_Execucoes_NFE(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tao, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int CRITERIO_PARADA_NFOBouPRECISAO){
            // Executa o SGA por N vezes
            double somatorio_nro_avaliacoes_FO_encontrar_global_AGEO = 0;

            // Executa o AGEO
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"... ");

                // Executa o AGEO e recebe como retorno a melhor fitness da execução
                List<double> retorno_AGEO = GEO.GEO.GEO_algorithm(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                
                // Como o retorno é somente o número de avaliações médio, então obtém o número
                somatorio_nro_avaliacoes_FO_encontrar_global_AGEO += retorno_AGEO[0];
            }

            // Calcula a média dos melhores f(x) pra esse TAO
            double media_nro_avaliacoes_AGEO = somatorio_nro_avaliacoes_FO_encontrar_global_AGEO / numero_execucoes;

            // Retorna a média de NFE atingido na execução
            return media_nro_avaliacoes_AGEO;
        }
        
        
        public static List<double> Obtem_FxsMedios_NFOB_Execucoes(int numero_execucoes, int tipo_GEO, int tipo_AGEO, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double tao, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int CRITERIO_PARADA_NFOBouPRECISAO){
            
            List<List<double>> NFOBs_all_results_GEO = new List<List<double>>();
            
            // Executa o GEO e o GEOvar por 50 vezes
            for(int i=0; i<numero_execucoes; i++){
                Console.Write((i+1) +"... ");

                // Executa o algoritmo
                List<double> NFOBs_results_GEO = GEO.GEO.GEO_algorithm(tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

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
        
        
        public static void Executa_Todos_Algoritmos_Por_NFOBs(int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double taoGEO, double taoGEOvar, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int CRITERIO_PARADA_NFOBouPRECISAO){

            // Inicializa as variáveis para cada execução
            double tao = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            Console.Write("\nGEO...");
            tao = taoGEO;
            tipo_GEO = 0;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEO = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // GEOvar
            Console.Write("\nGEOvar...");
            tao = taoGEOvar;
            tipo_GEO = 1;
            tipo_AGEO = 0;
            List<double> NFOBs_all_results_GEOvar = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO1
            Console.Write("\nAGEO1...");
            tipo_GEO = 0;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO2
            Console.Write("\nAGEO2...");
            tipo_GEO = 0;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO1var
            Console.Write("\nAGEO1var...");
            tipo_GEO = 1;
            tipo_AGEO = 1;
            List<double> NFOBs_all_results_AGEO1var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO2var
            Console.Write("\nAGEO2var...");
            tipo_GEO = 1;
            tipo_AGEO = 2;
            List<double> NFOBs_all_results_AGEO2var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

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
        
        
        public static void Executa_Todos_Algoritmos_Por_NFE(int quantidade_execucoes, int n_variaveis_projeto, List<int> bits_por_variavel_variaveis, int definicao_funcao_objetivo, List<double> limites_inferiores_variaveis, List<double> limites_superiores_variaveis, double taoGEO, double taoGEOvar, double valor_criterio_parada, int step_obter_NFOBs, double fx_esperado, int CRITERIO_PARADA_NFOBouPRECISAO){
            
            // Inicializa as variáveis para cada execução
            double tao = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            Console.Write("\nGEO...");
            tao = taoGEO;
            tipo_GEO = 0;
            tipo_AGEO = 0;
            double media_nro_avaliacoes_GEO = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
            
            // GEOvar
            Console.Write("\nGEOvar...");
            tao = taoGEOvar;
            tipo_GEO = 1;
            tipo_AGEO = 0;
            double media_nro_avaliacoes_GEOvar = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO1
            Console.Write("\nAGEO1...");
            tipo_GEO = 0;
            tipo_AGEO = 1;
            double media_nro_avaliacoes_AGEO1 = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO2
            Console.Write("\nAGEO2...");
            tipo_GEO = 0;
            tipo_AGEO = 2;
            double media_nro_avaliacoes_AGEO2 = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO1var
            Console.Write("\nAGEO1var...");
            tipo_GEO = 1;
            tipo_AGEO = 1;
            double media_nro_avaliacoes_AGEO1var = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // AGEO2var
            Console.Write("\nAGEO2var...");
            tipo_GEO = 1;
            tipo_AGEO = 2;
            double media_nro_avaliacoes_AGEO2var = Obtem_Media_Execucoes_NFE(quantidade_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

            // ===========================================================
            // Mostra a média dos NFE nas execuções
            // ===========================================================

            string str_media_NFE_GEO = (media_nro_avaliacoes_GEO.ToString()).Replace('.',',');
            string str_media_NFE_GEOvar = (media_nro_avaliacoes_GEOvar.ToString()).Replace('.',',');
            string str_media_NFE_AGEO1 = (media_nro_avaliacoes_AGEO1.ToString()).Replace('.',',');
            string str_media_NFE_AGEO2 = (media_nro_avaliacoes_AGEO2.ToString()).Replace('.',',');
            string str_media_NFE_AGEO1var = (media_nro_avaliacoes_AGEO1var.ToString()).Replace('.',',');
            string str_media_NFE_AGEO2var = (media_nro_avaliacoes_AGEO2var.ToString()).Replace('.',',');
            
            Console.WriteLine("");
            Console.WriteLine("===> Médias NFE:");
            Console.WriteLine("GEO;GEOvar;AGEO1;AGEO2;AGEO1var;AGEO2var");
            Console.WriteLine("{0};{1};{2};{3};{4};{5}", str_media_NFE_GEO, str_media_NFE_GEOvar, str_media_NFE_AGEO1, str_media_NFE_AGEO2, str_media_NFE_AGEO1var, str_media_NFE_AGEO2var);
        }
            
        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================
            
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

            // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};

            int step_obter_NFOBs = 250;
            // List<int> NFOBs_desejados = new List<int>(){10,20,30,40,50,60,70,80,90,100,120,140,160,180,200,250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000,199999};
            

            //============================================================
            // Execução do algoritmo
            //============================================================

            // Executa o GEO e recebe como retorno a melhor fitness da execução
            List<double> melhores_NFOBs = GEO.GEO.GEO_algorithm(tipo_GEO, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
            
            foreach(double melhor_NFOB in melhores_NFOBs){
                string melhor_NFOB_virgula = (melhor_NFOB.ToString()).Replace('.',',');
                Console.WriteLine(melhor_NFOB_virgula);
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
            // List<int> NFOBs_desejados = new List<int>(){400000};
            int step_obter_NFOBs = 0;
            
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
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
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

            // Define a lista de TAOs para as execuções
            List<double> valores_TAO_F3 = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};
            
            // Define o passo para a obtenção do f(x) a cada NFOB
            int step_obter_NFOBs = 0;
            
            // Executa os algoritmos para cada valor de TAO
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
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_nro_avaliacoes_FO_encontrar_global_GEO += retorno_GEO[0];

                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
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
            // List<int> NFOBs_desejados = new List<int>(){250,500,750,1000,1500,2000,3000,4000,5000,6000,7000,8000,9000,10000,15000,20000,25000,30000,40000,50000,60000,70000,80000,90000,100000};
            int step_obter_NFOBs = 250;
            
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
                    List<double> retorno_GEO = GEO.GEO.GEO_algorithm(0, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                    // Como o retorno é somente o número de avaliações médio, então obtém o número
                    somatorio_melhores_GEO += retorno_GEO[retorno_GEO.Count-1];


                    // Executa o GEOvar e recebe como retorno o número de avaliações médio
                    List<double> retorno_GEOvar = GEO.GEO.GEO_algorithm(1, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
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



        public static void Execucoes_NFOBs_Rosenbrock(){
            // Parâmetros da função
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 1;

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1000000;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 25000;
            // Define a quantidade de execuções
            int quantidade_execucoes = 5;

            // Define os TAOs
            double taoGEO = 1.0;
            double taoGEOvar = 1.25;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
        }

        public static void Execucoes_NFOBs_Griewangk(){
            // Parâmetros da função
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 1000000;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 25000;
            // Define a quantidade de execuções
            int quantidade_execucoes = 5;

            // Define os TAOs
            double taoGEO = 1.25;
            double taoGEOvar = 3;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
        }

        public static void Execucoes_NFOBs_DeJong3(){
            // Parâmetros da função
            const int n_variaveis_projeto = 5;
            List<int> bits_por_variavel_variaveis = new List<int>(){11,11,11,11,11};
            List<double> limites_inferiores_variaveis = new List<double>(){-5.12,-5.12,-5.12,-5.12,-5.12};
            List<double> limites_superiores_variaveis = new List<double>(){5.12,5.12,5.12,5.12,5.12};
            double fx_esperado = -25;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 2;

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 2201;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 25000;
            // Define a quantidade de execuções
            int quantidade_execucoes = 5;

            // Define os TAOs
            double taoGEO = 3.0;
            double taoGEOvar = 8.0;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
        }

        public static void Execucoes_NFE_Precisao_Rosenbrock(){
            // Parâmetros da função
            const int n_variaveis_projeto = 2;
            List<int> bits_por_variavel_variaveis = new List<int>(){13,13};
            List<double> limites_inferiores_variaveis = new List<double>(){-2.048,-2.048};
            List<double> limites_superiores_variaveis = new List<double>(){2.048, 2.048};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 0.001;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 0;
            // Define a quantidade de execuções
            int quantidade_execucoes = 5;

            // Define os TAOs
            double taoGEO = 1.0;
            double taoGEOvar = 1.25;

            Executa_Todos_Algoritmos_Por_NFE(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
        }

        public static void Execucoes_NFE_Precisao_Griewangk(){
            // Parâmetros da função
            const int n_variaveis_projeto = 10;
            List<int> bits_por_variavel_variaveis = new List<int>(){14,14,14,14,14,14,14,14,14,14};
            List<double> limites_inferiores_variaveis = new List<double>(){-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0,-600.0};
            List<double> limites_superiores_variaveis = new List<double>(){600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0,600.0};
            double fx_esperado = 0;
            // 0 - Griewangk | 1 - Rosenbrock | 2 - DeJong3 | 3 - Spacecraft Design
            int definicao_funcao_objetivo = 0;

            // Parâmetros de execução do algoritmo
            const double valor_criterio_parada = 0.5;
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;
            // Define o passo para a obtenção dos f(x) a cada NFOB
            int step_obter_NFOBs = 0;
            // Define a quantidade de execuções
            int quantidade_execucoes = 5;

            // Define os TAOs
            double taoGEO = 1.25;
            double taoGEOvar = 3;

            Executa_Todos_Algoritmos_Por_NFE(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, taoGEO, taoGEOvar, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
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
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

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



            double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

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
            const int CRITERIO_PARADA_NFOBouPRECISAO = 1;

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

                double media_nro_avaliacoes = Obtem_Media_Execucoes_NFE(numero_execucoes, tipo_GEO, tipo_AGEO, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);

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
            const int CRITERIO_PARADA_NFOBouPRECISAO = 0;

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
                List<double> melhores_NFOBs = GEO.GEO.GEO_algorithm(tipo_GEO, 0, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tao, valor_criterio_parada, step_obter_NFOBs, fx_esperado, CRITERIO_PARADA_NFOBouPRECISAO);
                
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

            // GEOcan_GEOvar_Griewangk();
            // GEOcan_GEOvar_SpaceDesign();
            
            // GEOs_variandoTAO_F2_Rosenbrock();
            // GEOs_variandoTAO_F3_DeJong3();
            // GEOs_variandoTAO_F5_Griewangk();

            Execucoes_NFOBs_Rosenbrock();
            // Execucoes_NFOBs_Griewangk();
            // Execucoes_NFOBs_DeJong3();


            // Execucoes_NFE_Precisao_Griewangk();



            // AGEO_PRECISION_F2_Rosenbrock();

            // AGEO_PRECISION_F3_Griewangk();

            // AGEO_PRECISION_SpacecraftOptimization();


            // AGEO_NFOBs_F3_Griewangk();
            // AGEO_NFOBs_F2_Rosenbrock();


            // SpacecraftOptimization_ExtensiveSearch();

            // GEOs_variandoTAO_SpacecraftOptimization();
            
            // TesteFuncaoObjetivoPopulacao_SpacecraftOptimization();

        }
    }
}