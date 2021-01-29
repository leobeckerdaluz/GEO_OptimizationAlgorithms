using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Funcoes_Definidas;
using SpaceDesignTeste;

using Classes_Comuns_Enums;

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

        public static double Obtem_Media_Execucoes_NFE_OU_MELHORFX(int NFE_ou_MELHORFX, int quantidade_execucoes, int tipo_GEO, int tipo_AGEO, double tau, ParametrosDaFuncao parametros_problema, ParametrosCriterioParada parametros_criterio_parada){
            //------------------------------------------------
            //int contador_deubom = 0;
            //------------------------------------------------

            // Executa o algoritmo por N vezes
            double somatorio_NFEs_OU_MELHORFX = 0;
            for(int i=0; i<quantidade_execucoes; i++){
                Console.Write((i+1) +"...");

                // Executa o algoritmo desejado e recebe como retorno apenas um valor (melhor fitness da execução ou NFE atingido), conforme o critério de parada.
                RetornoGEOs retorno_algoritmo = GEO.GEO.GEOs_algorithms(tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
                
                //------------------------------------------------
                // if (retorno_algoritmo[0] >= 100000){
                //     Console.WriteLine("Não contabilizando! NFOB = {0}", retorno_algoritmo[0]);
                //     continue;
                // }
                // Console.WriteLine("Passou reto o if. NFOB = {0}", retorno_algoritmo[0]);
                // contador_deubom++;
                //------------------------------------------------

                if (NFE_ou_MELHORFX == 0){
                    somatorio_NFEs_OU_MELHORFX += retorno_algoritmo.NFOB;
                }
                else if (NFE_ou_MELHORFX == 1){
                    somatorio_NFEs_OU_MELHORFX += retorno_algoritmo.melhor_fx;
                }
            }

            // Calcula e retorna a média dos valores recebidos durante a execução
            double media_resultado = somatorio_NFEs_OU_MELHORFX / quantidade_execucoes;
            
            // if (parametros_criterio_parada.tipo_criterio_parada == 1){
            //     Console.WriteLine("Media NFE / total execucoes: {0}", media_resultado);
            // }
            // else if (parametros_criterio_parada.tipo_criterio_parada == 2){
            //     Console.WriteLine("Media MELHORFX / total execucoes: {0}", media_resultado);
            // }

            //------------------------------------------------
            // double media_deubom = somatorio_NFEs_OU_MELHORFX / contador_deubom;
            // Console.WriteLine("Media / deubom: {0}", media_deubom);
            //------------------------------------------------

            return media_resultado;
        }
        
        
        public static List<double> Obtem_FxsMedios_NFOB_Execucoes(int quantidade_execucoes, int tipo_GEO, int tipo_AGEO, double tau, ParametrosDaFuncao parametros_problema, ParametrosCriterioParada parametros_criterio_parada){

            // Inicializa a lista de todos os resultados
            List<List<double>> NFOBs_todas_execucoes = new List<List<double>>();
            
            // Executa os algoritmos por N vezes
            for(int i=0; i<quantidade_execucoes; i++){
                Console.Write((i+1) +"...");

                // Executa o algoritmo
                RetornoGEOs retorno_algoritmo = GEO.GEO.GEOs_algorithms(tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                // Adiciona a lista de NFOBs em uma lista geral
                NFOBs_todas_execucoes.Add( retorno_algoritmo.melhores_NFOBs );
            }

            // Lista irá conter o f(x) médio para cada NFOB desejado
            List<double> lista_fxs_medios_cada_NFOB_desejado = new List<double>();
            
            // Obtém a quantidade de NFOBs armazenados
            int quantidade_NFOBs = NFOBs_todas_execucoes[0].Count;
            // Para cada NFOB desejado, calcula a média das N execuções
            for(int i=0; i<quantidade_NFOBs; i++){

                // Percorre aquele NFOB em cada execução para calcular a média de f(x) naquele NFOB
                double sum = 0;
                foreach(List<double> execution in NFOBs_todas_execucoes){
                    sum += execution[i];
                }
                double media = sum / NFOBs_todas_execucoes.Count;
                
                // Adiciona o f(x) médio do NFOB na lista
                lista_fxs_medios_cada_NFOB_desejado.Add(media);
            }

            // Retorna a lista contendo o f(x) médio para cada NFOB
            return lista_fxs_medios_cada_NFOB_desejado;
        }
        

        public static void Executa_Todos_Algoritmos_Por_NFOBs(int quantidade_execucoes, double tauGEO, double tauGEOvar, ParametrosDaFuncao parametros_problema, ParametrosCriterioParada parametros_criterio_parada, QuaisAlgoritmosRodar quais_algoritmos_rodar){

            // Inicializa as variáveis para cada execução
            double tau = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // Inicializa as listas com os resultados
            List<double> FXsMedios_Por_NFOB_GEO = new List<double>();
            List<double> FXsMedios_Por_NFOB_GEOvar = new List<double>();
            List<double> FXsMedios_Por_NFOB_AGEO1 = new List<double>();
            List<double> FXsMedios_Por_NFOB_AGEO2 = new List<double>();
            List<double> FXsMedios_Por_NFOB_AGEO1var = new List<double>();
            List<double> FXsMedios_Por_NFOB_AGEO2var = new List<double>();
            
            // GEO
            if (quais_algoritmos_rodar.rodar_GEO){
                Console.Write("\nGEO...");
                tau = tauGEO;
                tipo_GEO = 0;
                tipo_AGEO = 0;

                FXsMedios_Por_NFOB_GEO = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // GEOvar
            if (quais_algoritmos_rodar.rodar_GEOvar){
                Console.Write("\nGEOvar...");
                tau = tauGEOvar;
                tipo_GEO = 1;
                tipo_AGEO = 0;

                FXsMedios_Por_NFOB_GEOvar = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // AGEO1
            if (quais_algoritmos_rodar.rodar_AGEO1){
                Console.Write("\nAGEO1...");
                tipo_GEO = 0;
                tipo_AGEO = 1;

                FXsMedios_Por_NFOB_AGEO1 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // AGEO2
            if (quais_algoritmos_rodar.rodar_AGEO2){
                Console.Write("\nAGEO2...");
                tipo_GEO = 0;
                tipo_AGEO = 2;
                
                FXsMedios_Por_NFOB_AGEO2 = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // AGEO1var
            if (quais_algoritmos_rodar.rodar_AGEO1var){
                Console.Write("\nAGEO1var...");
                tipo_GEO = 1;
                tipo_AGEO = 1;
                
                FXsMedios_Por_NFOB_AGEO1var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // AGEO2var
            if (quais_algoritmos_rodar.rodar_AGEO2var){
                Console.Write("\nAGEO2var...");
                tipo_GEO = 1;
                tipo_AGEO = 2;
                FXsMedios_Por_NFOB_AGEO2var = Obtem_FxsMedios_NFOB_Execucoes(quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);
            }

            // ===========================================================
            // Mostra a média dos f(x) nas execuções em cada NFOB
            // ===========================================================

            Console.WriteLine("");
            Console.WriteLine("===> Médias para cada NFOB:");
            Console.WriteLine("NFOB;GEO;GEOvar;AGEO1;AGEO2;AGEO1var;AGEO2var");
            
            // Obtém a quantidade de NFOBs
            int quantidade_NFOBs = FXsMedios_Por_NFOB_GEO.Count;

            // Apresenta a média para cada NFOB
            int NFOB_atual = parametros_criterio_parada.step_para_obter_NFOBs;
            for(int i=0; i<quantidade_NFOBs; i++){
                string str_media_GEO = (quais_algoritmos_rodar.rodar_GEO) ? (FXsMedios_Por_NFOB_GEO[i].ToString()).Replace('.',',') : "0.0";

                string str_media_GEOvar = (quais_algoritmos_rodar.rodar_GEOvar) ? (FXsMedios_Por_NFOB_GEOvar[i].ToString()).Replace('.',',') : "0.0";

                string str_media_AGEO1 = (quais_algoritmos_rodar.rodar_AGEO1) ? (FXsMedios_Por_NFOB_AGEO1[i].ToString()).Replace('.',',') : "0.0";

                string str_media_AGEO2 = (quais_algoritmos_rodar.rodar_AGEO2) ? (FXsMedios_Por_NFOB_AGEO2[i].ToString()).Replace('.',',') : "0.0";

                string str_media_AGEO1var = (quais_algoritmos_rodar.rodar_AGEO1var) ? (FXsMedios_Por_NFOB_AGEO1var[i].ToString()).Replace('.',',') : "0.0";

                string str_media_AGEO2var = (quais_algoritmos_rodar.rodar_AGEO2var) ? (FXsMedios_Por_NFOB_AGEO2var[i].ToString()).Replace('.',',') : "0.0";

                Console.WriteLine("{0};{1};{2};{3};{4};{5};{6}", NFOB_atual, str_media_GEO, str_media_GEOvar, str_media_AGEO1, str_media_AGEO2, str_media_AGEO1var, str_media_AGEO2var);

                NFOB_atual += parametros_criterio_parada.step_para_obter_NFOBs;
            }
        }


        public static void Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(int NFE_ou_MELHORFX, int quantidade_execucoes, double tauGEO, double tauGEOvar, ParametrosDaFuncao parametros_problema, ParametrosCriterioParada parametros_criterio_parada, QuaisAlgoritmosRodar quais_algoritmos_rodar){
         
            // Inicializa as variáveis para cada execução
            double tau = 0.0;
            int tipo_GEO = 0;
            int tipo_AGEO = 0;

            // Inicializa as strings de resultado das médias
            string str_media_NFE_GEO = "0.0";
            string str_media_NFE_GEOvar = "0.0";
            string str_media_NFE_AGEO1 = "0.0";
            string str_media_NFE_AGEO2 = "0.0";
            string str_media_NFE_AGEO1var = "0.0";
            string str_media_NFE_AGEO2var = "0.0";

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // GEO
            if (quais_algoritmos_rodar.rodar_GEO){
                Console.Write("\nGEO...");
                tau = tauGEO;
                tipo_GEO = 0;
                tipo_AGEO = 0;

                double media_GEO = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia GEO: {0}", media_GEO);

                str_media_NFE_GEO = (media_GEO.ToString()).Replace('.',',');
            }

            // GEOvar
            if (quais_algoritmos_rodar.rodar_GEOvar){
                Console.Write("\nGEOvar...");
                tau = tauGEOvar;
                tipo_GEO = 1;
                tipo_AGEO = 0;

                double media_GEOvar = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia GEOvar: {0}", media_GEOvar);

                str_media_NFE_GEOvar = (media_GEOvar.ToString()).Replace('.',',');
            }

            // AGEO1
            if (quais_algoritmos_rodar.rodar_AGEO1){
                Console.Write("\nAGEO1...");
                tipo_GEO = 0;
                tipo_AGEO = 1;

                double media_AGEO1 = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia AGEO1: {0}", media_AGEO1);

                str_media_NFE_AGEO1 = (media_AGEO1.ToString()).Replace('.',',');
            }

            // AGEO2
            if (quais_algoritmos_rodar.rodar_AGEO2){
                Console.Write("\nAGEO2...");
                tipo_GEO = 0;
                tipo_AGEO = 2;

                double media_AGEO2 = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia AGEO2: {0}", media_AGEO2);

                str_media_NFE_AGEO2 = (media_AGEO2.ToString()).Replace('.',',');
            }

            // AGEO1var
            if (quais_algoritmos_rodar.rodar_AGEO1var){
                Console.Write("\nAGEO1var...");
                tipo_GEO = 1;
                tipo_AGEO = 1;

                double media_AGEO1var = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia AGEO1var: {0}", media_AGEO1var);

                str_media_NFE_AGEO1var = (media_AGEO1var.ToString()).Replace('.',',');
            }

            // AGEO2var
            if (quais_algoritmos_rodar.rodar_AGEO2var){
                Console.Write("\nAGEO2var...");
                tipo_GEO = 1;
                tipo_AGEO = 2;

                double media_AGEO2var = Obtem_Media_Execucoes_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tipo_GEO, tipo_AGEO, tau, parametros_problema, parametros_criterio_parada);

                Console.WriteLine("\nMédia AGEO2var: {0}", media_AGEO2var);

                str_media_NFE_AGEO2var = (media_AGEO2var.ToString()).Replace('.',',');
            }

            // ===========================================================
            // Mostra a média dos NFE nas execuções
            // ===========================================================

            string result = String.Format("{0};{1};{2};{3};{4};{5};{6}", tau, str_media_NFE_GEO, str_media_NFE_GEOvar, str_media_NFE_AGEO1, str_media_NFE_AGEO2, str_media_NFE_AGEO1var, str_media_NFE_AGEO2var);

            Console.WriteLine("");

            if (NFE_ou_MELHORFX == 0)
                Console.WriteLine("===> Médias NFE:");
            else if (NFE_ou_MELHORFX == 1)
                Console.WriteLine("===> Médias MELHORFX:");
            
            Console.WriteLine("TAU;GEO;GEOvar;AGEO1;AGEO2;AGEO1var;AGEO2var");
            Console.WriteLine(result);
        }
            


        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public static void Execucoes_Griewangk(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 10;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(14, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-600.0, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(600.0, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 0.0;
            parametros_problema.definicao_funcao_objetivo = 0;

            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Define os TAUs
            double tauGEO = 1.25;
            double tauGEOvar = 3.0;

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 0;
            parametros_criterio_parada.step_para_obter_NFOBs = 1000;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = true;
            quais_algoritmos_rodar.rodar_AGEO2 = true;
            quais_algoritmos_rodar.rodar_AGEO1var = true;
            quais_algoritmos_rodar.rodar_AGEO2var = true;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);


            // // ========================================
            // // OBTÉM MELHORES NFEs VARIANDO TAU
            // // ========================================

            // // Percorre a lista de TAUs e executa os algoritmos
            // List<double> valores_TAU = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2,75, 3, 3.25, 3.5, 3.75, 4};

            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = 0;
            // parametros_criterio_parada.step_para_obter_NFOBs = 0;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // // Define quais algoritmos executar
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO = true;
            // quais_algoritmos_rodar.rodar_GEOvar = true;
            // quais_algoritmos_rodar.rodar_AGEO1 = false;
            // quais_algoritmos_rodar.rodar_AGEO2 = false;
            // quais_algoritmos_rodar.rodar_AGEO1var = false;
            // quais_algoritmos_rodar.rodar_AGEO2var = false;

            // int NFE_ou_MELHORFX = 1;

            // foreach(double tau in valores_TAU){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("TAU = {0}", tau);
            //     Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            // }
        }


        public static void Execucoes_Rosenbrock(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 2;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(13, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-2.048, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(2.048, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 0.0;
            parametros_problema.definicao_funcao_objetivo = 1;

            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

          
            // // ========================================
            // // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // // ========================================

            // // Define os TAUs
            // double tauGEO = 1.0;
            // double tauGEOvar = 1.25;

            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = 0;
            // parametros_criterio_parada.step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // // Define quais algoritmos executar
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO = true;
            // quais_algoritmos_rodar.rodar_GEOvar = true;
            // quais_algoritmos_rodar.rodar_AGEO1 = true;
            // quais_algoritmos_rodar.rodar_AGEO2 = true;
            // quais_algoritmos_rodar.rodar_AGEO1var = true;
            // quais_algoritmos_rodar.rodar_AGEO2var = true;

            // // Executa todos os algoritmos
            // Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);


            // ========================================
            // OBTÉM MELHORES NFEs VARIANDO TAU
            // ========================================

            // Percorre a lista de TAUs e executa os algoritmos
            List<double> valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 1;
            parametros_criterio_parada.step_para_obter_NFOBs = 0;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0.001;
            parametros_criterio_parada.NFOB_criterio_parada = 0;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = false;
            quais_algoritmos_rodar.rodar_AGEO2 = false;
            quais_algoritmos_rodar.rodar_AGEO1var = false;
            quais_algoritmos_rodar.rodar_AGEO2var = false;

            int NFE_ou_MELHORFX = 0;

            foreach(double tau in valores_TAU){
                Console.WriteLine("\n==========================================");
                Console.WriteLine("TAU = {0}", tau);
                Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            }
        }
        

        public static void Execucoes_DeJong3(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 5;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(11, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-5.12, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(5.12, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = -25.0;
            parametros_problema.definicao_funcao_objetivo = 2;
            
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // // ========================================
            // // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // // ========================================

            // // Define os TAUs
            // double tauGEO = 3.0;
            // double tauGEOvar = 8.0;

            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = 0;
            // parametros_criterio_parada.step_para_obter_NFOBs = 100;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // parametros_criterio_parada.NFOB_criterio_parada = (int)2500;

            // // Define quais algoritmos executar
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO = true;
            // quais_algoritmos_rodar.rodar_GEOvar = true;
            // quais_algoritmos_rodar.rodar_AGEO1 = true;
            // quais_algoritmos_rodar.rodar_AGEO2 = true;
            // quais_algoritmos_rodar.rodar_AGEO1var = true;
            // quais_algoritmos_rodar.rodar_AGEO2var = true;

            // // Executa todos os algoritmos
            // Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            

            // ========================================
            // OBTÉM MELHORES NFEs VARIANDO TAU
            // ========================================

            // Percorre a lista de TAUs e executa os algoritmos
            List<double> valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.5, 4, 4.5, 5, 6, 7, 8, 9, 10};

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 2;
            parametros_criterio_parada.step_para_obter_NFOBs = 0;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0.001;
            parametros_criterio_parada.NFOB_criterio_parada = 100000;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = false;
            quais_algoritmos_rodar.rodar_AGEO2 = false;
            quais_algoritmos_rodar.rodar_AGEO1var = false;
            quais_algoritmos_rodar.rodar_AGEO2var = false;

            int NFE_ou_MELHORFX = 0;

            foreach(double tau in valores_TAU){
                Console.WriteLine("\n==========================================");
                Console.WriteLine("TAU = {0}", tau);
                Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            }
        }


        public static void Execucoes_Rastringin(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 20;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-5.12, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(5.12, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 0.0;
            parametros_problema.definicao_funcao_objetivo = 4;
          
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Define os TAUs
            double tauGEO = 1.0;
            double tauGEOvar = 1.75;

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 0;
            parametros_criterio_parada.step_para_obter_NFOBs = (int)1e3;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = true;
            quais_algoritmos_rodar.rodar_AGEO2 = true;
            quais_algoritmos_rodar.rodar_AGEO1var = true;
            quais_algoritmos_rodar.rodar_AGEO2var = true;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
        }


        public static void Execucoes_Schwefel(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 10;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-500.0, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(500.0, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 0.0;
            parametros_problema.definicao_funcao_objetivo = 5;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Define os TAUs
            double tauGEO = 1.0;
            double tauGEOvar = 1.75;

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 0;
            parametros_criterio_parada.step_para_obter_NFOBs = (int)1e3;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = true;
            quais_algoritmos_rodar.rodar_AGEO2 = true;
            quais_algoritmos_rodar.rodar_AGEO1var = true;
            quais_algoritmos_rodar.rodar_AGEO2var = true;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
        }


        public static void Execucoes_Ackley(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 30;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(16, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-30.0, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(30.0, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 0.0;
            parametros_problema.definicao_funcao_objetivo = 6;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;


            // ========================================
            // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // ========================================

            // Define os TAUs
            double tauGEO = 2.25;
            double tauGEOvar = 2.50;

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 0;
            parametros_criterio_parada.step_para_obter_NFOBs = (int)1e3;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            parametros_criterio_parada.NFOB_criterio_parada = (int)1e5;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = true;
            quais_algoritmos_rodar.rodar_AGEO2 = true;
            quais_algoritmos_rodar.rodar_AGEO1var = true;
            quais_algoritmos_rodar.rodar_AGEO2var = true;

            // Executa todos os algoritmos
            Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
        }


        public static void Execucoes_F9(){
            // Parâmetros da função
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 2;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = Enumerable.Repeat(18, n_variaveis_projeto).ToList();
            parametros_problema.limites_inferiores_variaveis = Enumerable.Repeat(-10.0, n_variaveis_projeto).ToList();
            parametros_problema.limites_superiores_variaveis = Enumerable.Repeat(10.0, n_variaveis_projeto).ToList();
            parametros_problema.fx_esperado = 1.0;
            parametros_problema.definicao_funcao_objetivo = 7;
           
            // Define a quantidade de execuções
            int quantidade_execucoes = 50;

            // // ========================================
            // // OBTÉM NFEs PARA CADA NFOB DESEJADO
            // // ========================================

            // // Define os TAUs
            // double tauGEO = 1.50;
            // double tauGEOvar = 1.75;

            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = 0;
            // parametros_criterio_parada.step_para_obter_NFOBs = (int)1e4;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // parametros_criterio_parada.NFOB_criterio_parada = (int)1e6;

            // // Define quais algoritmos executar
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO = true;
            // quais_algoritmos_rodar.rodar_GEOvar = true;
            // quais_algoritmos_rodar.rodar_AGEO1 = false;
            // quais_algoritmos_rodar.rodar_AGEO2 = false;
            // quais_algoritmos_rodar.rodar_AGEO1var = true;
            // quais_algoritmos_rodar.rodar_AGEO2var = false;

            // // Executa todos os algoritmos
            // Executa_Todos_Algoritmos_Por_NFOBs(quantidade_execucoes, tauGEO, tauGEOvar, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
         

            // ========================================
            // OBTÉM MELHORES NFEs VARIANDO TAU
            // ========================================

            // Percorre a lista de TAUs e executa os algoritmos
            List<double> valores_TAU = new List<double>(){1.5, 1.75};

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 1;
            parametros_criterio_parada.step_para_obter_NFOBs = 0;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0.0001;
            parametros_criterio_parada.NFOB_criterio_parada = 0;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = false;
            quais_algoritmos_rodar.rodar_AGEO2 = false;
            quais_algoritmos_rodar.rodar_AGEO1var = false;
            quais_algoritmos_rodar.rodar_AGEO2var = false;

            int NFE_ou_MELHORFX = 0;

            foreach(double tau in valores_TAU){
                Console.WriteLine("\n==========================================");
                Console.WriteLine("TAU = {0}", tau);
                Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            }
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
            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            int n_variaveis_projeto = 3;
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.bits_por_variavel_variaveis = new List<int>(){2,6,6};
            parametros_problema.limites_inferiores_variaveis = new List<double>(){13,1,0};
            parametros_problema.limites_superiores_variaveis = new List<double>(){15,60,59};
            parametros_problema.fx_esperado = 196.949433192159;
            parametros_problema.definicao_funcao_objetivo = 3;
           
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


            // ========================================
            // OBTÉM MELHORES NFEs VARIANDO TAU
            // ========================================

            // Percorre a lista de TAUs e executa os algoritmos
            List<double> valores_TAU = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5};

            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = 1;
            parametros_criterio_parada.step_para_obter_NFOBs = 0;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0.0000001;
            parametros_criterio_parada.NFOB_criterio_parada = 0;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = false;
            quais_algoritmos_rodar.rodar_AGEO2 = false;
            quais_algoritmos_rodar.rodar_AGEO1var = false;
            quais_algoritmos_rodar.rodar_AGEO2var = false;

            int NFE_ou_MELHORFX = 1;

            foreach(double tau in valores_TAU){
                Console.WriteLine("\n==========================================");
                Console.WriteLine("TAU = {0}", tau);
                Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, parametros_criterio_parada, quais_algoritmos_rodar);
            }


            // // ========================================
            // // OBTÉM MELHORES F(X) VARIANDO TAU
            // // ========================================

            // // NFOB para a execução encerrar
            // const double valor_criterio_parada = 0.0000001;

            // // Define os TAUs
            // double tauGEO = 1.0;
            // double tauGEOvar = 1.0;

            // Executa_Todos_Algoritmos_Por_NFE(quantidade_execucoes, n_variaveis_projeto, bits_por_variavel_variaveis, definicao_funcao_objetivo, limites_inferiores_variaveis, limites_superiores_variaveis, tauGEO, tauGEOvar, valor_criterio_parada, fx_esperado);
        }
        
        

        // =====================================
        // MAIN
        // =====================================

        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            Execucoes_Griewangk();
            // Execucoes_Rosenbrock();
            // Execucoes_DeJong3();
            // Execucoes_Rastringin();
            // Execucoes_Schwefel();
            // Execucoes_Ackley();
            // Execucoes_F9();

            // ExtensiveSearch_SpacecraftOptimization();
            // Teste_FuncoesObjetivo_SpacecraftOptimization();
            // Execucoes_SpacecraftOptimization();
        }
    }
}