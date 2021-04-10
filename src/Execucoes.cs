using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Funcoes_Definidas;
using SpaceDesignTeste;

using GEOs_REAIS;
using GEOs_BINARIOS;

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


        public static RetornoGEOs Executa_Algoritmo_por_N_vezes(int qual_algoritmo_executar, int quantidade_execucoes, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais_por_variavel, int step_para_obter_NFOBs, List<int> bits_por_variavel_variaveis, ParametrosCriterioParada parametros_criterio_parada, double tau, double std, int P, int tipo_perturbacao_original_ou_SDdireto, bool printar_status_executando){
                
            // Inicializa a lista de todos os resultados
            List<List<double>> NFOBs_todas_execucoes = new List<List<double>>();
            // Inicializa o somatório de NFOBs
            int somatorio_NFOBs = 0;
            // Inicializa o somatório de melhor f(x)
            double somatorio_melhorFX = 0;
            // Inicializa a lista que irá conter os melhores f(x) das N execuções
            List<double> lista_melhores_fx = new List<double>();
            
            // Executa os algoritmos por N vezes
            for(int i=0; i<quantidade_execucoes; i++){
                if (printar_status_executando){
                    Console.Write((i+1) +"...");
                }
            
                RetornoGEOs retorno_algoritmo = new RetornoGEOs();

                switch(qual_algoritmo_executar){
                    
                    case (int)EnumNomesAlgoritmos.GEO_can:
                        GEO_BINARIO geo = new GEO_BINARIO(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = geo.executar(parametros_criterio_parada);
                        
                        break;

                    case (int)EnumNomesAlgoritmos.GEO_var:
                        GEOvar_BINARIO geo_var = new GEOvar_BINARIO(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = geo_var.executar(parametros_criterio_parada);
                        
                        break;

                    case (int)EnumNomesAlgoritmos.AGEO1:
                        AGEOs_BINARIO ageo1 = new AGEOs_BINARIO(1, tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = ageo1.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.AGEO2:
                        AGEOs_BINARIO ageo2 = new AGEOs_BINARIO(2, tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = ageo2.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.AGEO1var:
                        AGEOsvar_BINARIO ageo1_var = new AGEOsvar_BINARIO(1, tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = ageo1_var.executar(parametros_criterio_parada);

                        break;
                    
                    case (int)EnumNomesAlgoritmos.AGEO2var:
                        AGEOsvar_BINARIO ageo2_var = new AGEOsvar_BINARIO(2, tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, bits_por_variavel_variaveis);

                        retorno_algoritmo = ageo2_var.executar(parametros_criterio_parada); 

                        break;
                    
                    case (int)EnumNomesAlgoritmos.GEOreal1:
                        GEO_real1 geo_real1 = new GEO_real1(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, tipo_perturbacao_original_ou_SDdireto);
                        
                        retorno_algoritmo = geo_real1.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.AGEO1real1:
                        AGEOs_REAL1 AGEO1real1 = new AGEOs_REAL1(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 1, tipo_perturbacao_original_ou_SDdireto);
                        
                        retorno_algoritmo = AGEO1real1.executar(parametros_criterio_parada);

                        break;
                        
                    case (int)EnumNomesAlgoritmos.AGEO2real1:
                        AGEOs_REAL1 AGEO2real1 = new AGEOs_REAL1(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 2, tipo_perturbacao_original_ou_SDdireto);
                        
                        retorno_algoritmo = AGEO2real1.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.GEOreal2:
                        GEO_real2 geo_real2 = new GEO_real2(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, tipo_perturbacao_original_ou_SDdireto, P);
                        
                        retorno_algoritmo = geo_real2.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.AGEO1real2:
                        AGEOs_REAL2 AGEO1real2 = new AGEOs_REAL2(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 1, tipo_perturbacao_original_ou_SDdireto, P);
                        
                        retorno_algoritmo = AGEO1real2.executar(parametros_criterio_parada);

                        break;
                        
                    case (int)EnumNomesAlgoritmos.AGEO2real2:
                        AGEOs_REAL2 AGEO2real2 = new AGEOs_REAL2(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 2, tipo_perturbacao_original_ou_SDdireto, P);
                        
                        retorno_algoritmo = AGEO2real2.executar(parametros_criterio_parada);

                        break;
                    
                    case (int)EnumNomesAlgoritmos.AGEO1_ASTD:
                        AGEOs_ASTD AGEO1_ASTD = new AGEOs_ASTD(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 1, tipo_perturbacao_original_ou_SDdireto);
                        
                        retorno_algoritmo = AGEO1_ASTD.executar(parametros_criterio_parada);

                        break;

                    case (int)EnumNomesAlgoritmos.AGEO2_ASTD:
                        AGEOs_ASTD AGEO2_ASTD = new AGEOs_ASTD(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_por_variavel, step_para_obter_NFOBs, std, 2, tipo_perturbacao_original_ou_SDdireto);
                        
                        retorno_algoritmo = AGEO2_ASTD.executar(parametros_criterio_parada);

                        break;
                }

                // Adiciona a lista de fxs em cada NFOBs da execução em uma lista geral
                NFOBs_todas_execucoes.Add( retorno_algoritmo.melhores_NFOBs );
                // Somatório de NFOBs para posterior média
                somatorio_NFOBs += retorno_algoritmo.NFOB;
                // Somatório de melhorFX para posterior média
                somatorio_melhorFX += retorno_algoritmo.melhor_fx;
                // Adiciona o melhor f(x) na lista dos melhores f(x)s
                lista_melhores_fx.Add( retorno_algoritmo.melhor_fx );
            }

            // Lista irá conter o f(x) médio para cada NFOB desejado
            List<double> lista_fxs_medios_cada_NFOB_desejado = new List<double>();

            // Obtém a quantidade de NFOBs armazenados
            int quantidade_NFOBs = NFOBs_todas_execucoes[0].Count;

            // Se o critério de parada é por NFOB, calcula a média das N execuções para cada NFOB desejado.
            if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB){
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
            }

            // Calcula a média dos NFOBs
            int media_NFOBs = (int) (somatorio_NFOBs / quantidade_execucoes);

            // Calcula a média dos NFOBs
            double media_melhor_fx = (double) somatorio_melhorFX / quantidade_execucoes;

            // Calcula o desvio padrão final com base no melhor f(x) das execuções
            double somatorio_sd = 0;
            foreach (double melhor_fx in lista_melhores_fx){
                somatorio_sd += Math.Pow((melhor_fx - media_melhor_fx), 2);
            }
            int n = lista_melhores_fx.Count - 1;
            double SD_melhor_fx = Math.Sqrt(somatorio_sd / n);

            // Cria um objeto de retorno contendo a média dos valores para as N execuções
            RetornoGEOs media_das_execucoes = new RetornoGEOs();
            media_das_execucoes.melhores_NFOBs = lista_fxs_medios_cada_NFOB_desejado;
            media_das_execucoes.melhor_fx = media_melhor_fx;
            media_das_execucoes.NFOB = media_NFOBs;
            media_das_execucoes.lista_melhores_fxs = lista_melhores_fx;
            media_das_execucoes.SD_melhor_fx = SD_melhor_fx;
            
            // Retorna o objeto com a média das execuções
            return media_das_execucoes;
        }
        

        public static void Executa_Os_Algoritmos_Por_N_Vezes(int quantidade_execucoes, double tau_GEO, double tau_GEOvar, double tau_GEOreal1, double tau_GEOreal2, double tau_minimo_AGEOs, double std_GEOreal1, double std_GEOreal2, double std_AGEO1real1, double std_AGEO2real1, double std_AGEO1real2, double std_AGEO2real2, int P_GEOreal2, int P_AGEO1real2, int P_AGEO2real2, int tipo_perturbacao_original_ou_SDdireto, ParametrosDaFuncao parametros_problema, List<int> bits_por_variavel_variaveis, List<RestricoesLaterais> restricoes_laterais_variaveis, ParametrosCriterioParada parametros_criterio_parada, int step_para_obter_NFOBs, QuaisAlgoritmosRodar quais_algoritmos_rodar, OQueInteressaPrintar o_que_interessa_printar, bool printar_status_executando){

            // ===========================================================
            // Executa os algoritmos
            // ===========================================================

            // Inicializa os retornos da média das execuções para cada algoritmo
            RetornoGEOs retorno_GEO = new RetornoGEOs();
            RetornoGEOs retorno_GEOvar = new RetornoGEOs();
            RetornoGEOs retorno_AGEO1 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO2 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO1var = new RetornoGEOs();
            RetornoGEOs retorno_AGEO2var = new RetornoGEOs();
            RetornoGEOs retorno_GEOreal1 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO1real1 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO2real1 = new RetornoGEOs();
            RetornoGEOs retorno_GEOreal2 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO1real2 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO2real2 = new RetornoGEOs();
            RetornoGEOs retorno_AGEO1_ASTD = new RetornoGEOs();
            RetornoGEOs retorno_AGEO2_ASTD = new RetornoGEOs();
            

            // Inicializa a quantidade de NFOBs
            int quantidade_NFOBs = 0;
            
            // GEO
            if (quais_algoritmos_rodar.rodar_GEO){
                Console.Write("\nGEO...");

                retorno_GEO = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.GEO_can, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_GEO, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_GEO.melhores_NFOBs.Count;
            }

            // GEOvar
            if (quais_algoritmos_rodar.rodar_GEOvar){
                Console.Write("\nGEOvar...");

                retorno_GEOvar = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.GEO_var, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_GEOvar, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_GEOvar.melhores_NFOBs.Count;
            }

            // AGEO1
            if (quais_algoritmos_rodar.rodar_AGEO1){
                Console.Write("\nAGEO1...");

                retorno_AGEO1 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO1, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO1.melhores_NFOBs.Count;
            }

            // AGEO2
            if (quais_algoritmos_rodar.rodar_AGEO2){
                Console.Write("\nAGEO2...");
                
                retorno_AGEO2 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO2, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO2.melhores_NFOBs.Count;
            }

            // AGEO1var
            if (quais_algoritmos_rodar.rodar_AGEO1var){
                Console.Write("\nAGEO1var...");
                
                retorno_AGEO1var = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO1var, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO1var.melhores_NFOBs.Count;
            }

            // AGEO2var
            if (quais_algoritmos_rodar.rodar_AGEO2var){
                Console.Write("\nAGEO2var...");

                retorno_AGEO2var = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO2var, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, 0, 0, 0, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO2var.melhores_NFOBs.Count;
            }

            // GEOreal1
            if (quais_algoritmos_rodar.rodar_GEOreal1){
                Console.Write("\nGEOreal1...");

                retorno_GEOreal1 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.GEOreal1, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_GEOreal1, std_GEOreal1, 0, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_GEOreal1.melhores_NFOBs.Count;
            }

            // GEOreal2
            if (quais_algoritmos_rodar.rodar_GEOreal2){
                Console.Write("\nGEOreal2...");

                retorno_GEOreal2 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.GEOreal2, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_GEOreal2, std_GEOreal2, P_GEOreal2, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_GEOreal2.melhores_NFOBs.Count;
            }

            // AGEO1real1
            if (quais_algoritmos_rodar.rodar_AGEO1real1){
                Console.Write("\nAGEO1real1...");

                retorno_AGEO1real1 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO1real1, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_AGEO1real1, 0, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO1real1.melhores_NFOBs.Count;
            }

            // AGEO2real1
            if (quais_algoritmos_rodar.rodar_AGEO2real1){
                Console.Write("\nAGEO2real1...");

                retorno_AGEO2real1 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO2real1, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_AGEO2real1, 0, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO2real1.melhores_NFOBs.Count;
            }

            // AGEO1real2
            if (quais_algoritmos_rodar.rodar_AGEO1real2){
                Console.Write("\nAGEO1real2...");

                retorno_AGEO1real2 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO1real2, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_AGEO1real2, P_AGEO1real2, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO1real2.melhores_NFOBs.Count;
            }

            // AGEO2real2
            if (quais_algoritmos_rodar.rodar_AGEO2real2){
                Console.Write("\nAGEO2real2...");

                retorno_AGEO2real2 = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO2real2, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_AGEO2real2, P_AGEO2real2, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

                quantidade_NFOBs = retorno_AGEO2real2.melhores_NFOBs.Count;
            }

            // // AGEO1_ASTD
            // if (quais_algoritmos_rodar.rodar_AGEO1_ASTD){
            //     Console.Write("\nAGEO2real2...");

            //     retorno_AGEO1_ASTD = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO1_ASTD, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_minimo_AGEOsASTD, 0, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

            //     quantidade_NFOBs = retorno_AGEO1_ASTD.melhores_NFOBs.Count;
            // }

            // // AGEO2_ASTD
            // if (quais_algoritmos_rodar.rodar_AGEO2_ASTD){
            //     Console.Write("\nAGEO2real2...");

            //     retorno_AGEO2_ASTD = Executa_Algoritmo_por_N_vezes( (int)EnumNomesAlgoritmos.AGEO2_ASTD, quantidade_execucoes, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, restricoes_laterais_variaveis, step_para_obter_NFOBs, bits_por_variavel_variaveis, parametros_criterio_parada, tau_minimo_AGEOs, std_minimo_AGEOsASTD, 0, tipo_perturbacao_original_ou_SDdireto, printar_status_executando);

            //     quantidade_NFOBs = retorno_AGEO2_ASTD.melhores_NFOBs.Count;
            // }


            // ===========================================================
            // Mostra a média dos f(x) nas execuções em cada NFOB
            // ===========================================================

            string algoritmos_executados = "";
            algoritmos_executados += (quais_algoritmos_rodar.rodar_GEO ? "GEO;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_GEOvar ? "GEOvar;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO1 ? "AGEO1;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO2 ? "AGEO2;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO1var ? "AGEO1var;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO2var ? "AGEO2var;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_GEOreal1 ? "GEOreal1;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_GEOreal2 ? "GEOreal2;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO1real1 ? "AGEO1real1;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO2real1 ? "AGEO2real1;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO1real2 ? "AGEO1real2;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO2real2 ? "AGEO2real2;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO1_ASTD ? "AGEO1_ASTD;" : "");
            algoritmos_executados += (quais_algoritmos_rodar.rodar_AGEO2_ASTD ? "AGEO2_ASTD;" : "");
            

            if (o_que_interessa_printar.mostrar_melhores_NFOB){
                Console.WriteLine("");
                Console.WriteLine("===> Médias para cada NFOB:");
                Console.WriteLine("NFOB;" + algoritmos_executados);
                
                // Apresenta a média para cada NFOB
                int NFOB_atual = step_para_obter_NFOBs;
                for(int i=0; i<quantidade_NFOBs; i++){
                    string fxs_naquele_NFOB = "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO1_ASTD) ? (retorno_AGEO1_ASTD.melhores_NFOBs[i].ToString()+';') : "";
                    fxs_naquele_NFOB += (quais_algoritmos_rodar.rodar_AGEO2_ASTD) ? (retorno_AGEO2_ASTD.melhores_NFOBs[i].ToString()+';') : "";

                    Console.WriteLine(NFOB_atual + ";" + fxs_naquele_NFOB.Replace('.',','));

                    NFOB_atual += step_para_obter_NFOBs;
                }
            }
            if (o_que_interessa_printar.mostrar_media_NFE_atingido){
                Console.WriteLine("");
                Console.WriteLine("===> Média NFEs atingidos:");
                Console.WriteLine(algoritmos_executados);
                string media_do_NFE_atingido_nas_execucoes = "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1_ASTD) ? (retorno_AGEO1_ASTD.NFOB.ToString()+';') : "";
                media_do_NFE_atingido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2_ASTD) ? (retorno_AGEO2_ASTD.NFOB.ToString()+';') : "";

                // Substitui os pontos por vírgulas e printa
                Console.WriteLine( media_do_NFE_atingido_nas_execucoes.Replace('.',',') );
            }
            if (o_que_interessa_printar.mostrar_media_melhor_fx){
                Console.WriteLine("");
                Console.WriteLine("===> AVG do melhor f(x) nas N execuções:");
                Console.WriteLine(algoritmos_executados);
                string media_do_melhor_FX_obtido_nas_execucoes = "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1_ASTD) ? (retorno_AGEO1_ASTD.melhor_fx.ToString()+';') : "";
                media_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2_ASTD) ? (retorno_AGEO2_ASTD.melhor_fx.ToString()+';') : "";

                // Substitui os pontos por vírgulas e printa
                Console.WriteLine(media_do_melhor_FX_obtido_nas_execucoes.Replace('.',','));
            }
            if (o_que_interessa_printar.mostrar_SD_melhor_fx){
                Console.WriteLine("");
                Console.WriteLine("===> SD do melhor f(x) nas N execuções:");
                Console.WriteLine(algoritmos_executados);
                string SD_do_melhor_FX_obtido_nas_execucoes = "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1_ASTD) ? (retorno_AGEO1_ASTD.SD_melhor_fx.ToString()+';') : "";
                SD_do_melhor_FX_obtido_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2_ASTD) ? (retorno_AGEO2_ASTD.SD_melhor_fx.ToString()+';') : "";

                // Substitui os pontos por vírgulas e printa
                Console.WriteLine(SD_do_melhor_FX_obtido_nas_execucoes.Replace('.',','));
            }
            if (o_que_interessa_printar.mostrar_melhores_fx_cada_execucao){
                Console.WriteLine("");
                Console.WriteLine("===> Melhor f(x) para cada uma das N execuções:");
                Console.WriteLine(algoritmos_executados);
                
                for(int n=0; n<quantidade_execucoes; n++){
                    string melhor_fx_nas_execucoes = "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO1_ASTD) ? (retorno_AGEO1_ASTD.lista_melhores_fxs[n].ToString()+';') : "";
                    melhor_fx_nas_execucoes += (quais_algoritmos_rodar.rodar_AGEO2_ASTD) ? (retorno_AGEO2_ASTD.lista_melhores_fxs[n].ToString()+';') : "";

                    // Substitui os pontos por vírgulas e printa
                    Console.WriteLine(melhor_fx_nas_execucoes.Replace('.',','));
                }
            }
        }
            

        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public static void Execucoes(){
            
            // Variáveis da função a ser executada
            double tau_GEO;
            double tau_GEOvar;
            double tau_GEOreal1;
            double tau_GEOreal2;
            double tau_minimo_AGEOs;
            double std_GEOreal1;
            double std_GEOreal2;
            double std_AGEO1real1;
            double std_AGEO2real1;
            double std_AGEO1real2;
            double std_AGEO2real2;
            int P_GEOreal2;
            int P_AGEO1real2;
            int P_AGEO2real2;
            double fx_esperado_funcao;
            ParametrosDaFuncao parametros_problema;
            List<int> bits_por_variavel_variaveis;
            List<RestricoesLaterais> restricoes_laterais_por_variavel;
            List<double> valores_TAU;


            // Definições do usuário
            int quantidade_execucoes = 50;
            // int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rastringin;
            // int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_griwangk;
            int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_schwefel;


            switch(definicao_funcao_objetivo){
                // RASTRINGIN
                case (int)EnumNomesFuncoesObjetivo.enum_rastringin:
                    tau_GEO = 1.00;
                    tau_GEOvar = 1.75;
                    tau_GEOreal1 = 1.75;
                    tau_GEOreal2 = 2.25;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 8.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 8.0;
                    std_AGEO2real2 = 8.0;
                    P_GEOreal2 = 8;
                    P_AGEO1real2 = 8;
                    P_AGEO2real2 = 8;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 20;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rastringin;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12} , parametros_problema.n_variaveis_projeto).ToList();
                    
                    valores_TAU = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
                break;

                // GRIEWANGK
                case (int)EnumNomesFuncoesObjetivo.enum_griwangk:
                    tau_GEO = 1.25;
                    tau_GEOvar = 3.0;
                    tau_GEOreal1 = 1.25;
                    tau_GEOreal2 = 1.75;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 8.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 8.0;
                    std_AGEO2real2 = 8.0;
                    P_GEOreal2 = 16;
                    P_AGEO1real2 = 16;
                    P_AGEO2real2 = 16;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_griwangk;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0} , parametros_problema.n_variaveis_projeto).ToList();
                    
                    valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
                break;

                // ROSENBROCK
                case (int)EnumNomesFuncoesObjetivo.enum_rosenbrock:
                    tau_GEO = 1.00;
                    tau_GEOvar = 1.25;
                    tau_GEOreal1 = 1.00;
                    tau_GEOreal2 = 1.25;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 1.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 1.0;
                    std_AGEO2real2 = 1.0;
                    P_GEOreal2 = 0;
                    P_AGEO1real2 = 0;
                    P_AGEO2real2 = 0;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rosenbrock;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-2.048, limite_superior_variavel=2.048} , parametros_problema.n_variaveis_projeto).ToList();
                    
                    valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};
                break;

                // SCHWEFEL
                case (int)EnumNomesFuncoesObjetivo.enum_schwefel:
                    tau_GEO = 1.00;
                    tau_GEOvar = 1.75;
                    tau_GEOreal1 = 6.25;
                    tau_GEOreal2 = 9;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 8.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 8.0;
                    std_AGEO2real2 = 8.0;
                    P_GEOreal2 = 16;
                    P_AGEO1real2 = 16;
                    P_AGEO2real2 = 16;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_schwefel;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0} , parametros_problema.n_variaveis_projeto).ToList();
                break;

                // ACKLEY
                case (int)EnumNomesFuncoesObjetivo.enum_ackley:
                    tau_GEO = 2.25;
                    tau_GEOvar = 2.50;
                    tau_GEOreal1 = 2.25;
                    tau_GEOreal2 = 2.50;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 1.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 1.0;
                    std_AGEO2real2 = 1.0;
                    P_GEOreal2 = 0;
                    P_AGEO1real2 = 0;
                    P_AGEO2real2 = 0;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 30;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_ackley;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-30.0, limite_superior_variavel=30.0} , parametros_problema.n_variaveis_projeto).ToList();
                break;

                // F9 TESE
                case (int)EnumNomesFuncoesObjetivo.enum_F09:
                    tau_GEO = 1.50;
                    tau_GEOvar = 1.75;
                    tau_GEOreal1 = 1.50;
                    tau_GEOreal2 = 1.75;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 1.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 1.0;
                    std_AGEO2real2 = 1.0;
                    P_GEOreal2 = 0;
                    P_AGEO1real2 = 0;
                    P_AGEO2real2 = 0;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_F09;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-10.0, limite_superior_variavel=10.0} , parametros_problema.n_variaveis_projeto).ToList();
                break;

                // DEJONG3
                case (int)EnumNomesFuncoesObjetivo.enum_dejong3:
                    tau_GEO = 3.0;
                    tau_GEOvar = 8.0;
                    tau_GEOreal1 = 3.0;
                    tau_GEOreal2 = 8.0;
                    tau_minimo_AGEOs = 0.5;
                    std_GEOreal1 = 1.0;
                    std_GEOreal2 = 1.0;
                    std_AGEO1real1 = 1.0;
                    std_AGEO2real1 = 1.0;
                    std_AGEO1real2 = 1.0;
                    std_AGEO2real2 = 1.0;
                    P_GEOreal2 = 0;
                    P_AGEO1real2 = 0;
                    P_AGEO2real2 = 0;
                    fx_esperado_funcao = 0.0;
                    
                    parametros_problema = new ParametrosDaFuncao();
                    parametros_problema.n_variaveis_projeto = 5;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_dejong3;
                    
                    bits_por_variavel_variaveis = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    
                    restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12} , parametros_problema.n_variaveis_projeto).ToList();
                break;

                default:
                    Console.WriteLine("DEFAULT PARAMETERS!");
                    tau_GEO = 0.0;
                    tau_GEOvar = 0.0;
                    tau_GEOreal1 = 0.0;
                    tau_GEOreal2 = 0.0;
                    tau_minimo_AGEOs = 0.0;
                    std_GEOreal1 = 0.0;
                    std_GEOreal2 = 0.0;
                    std_AGEO1real1 = 0.0;
                    std_AGEO2real1 = 0.0;
                    std_AGEO1real2 = 0.0;
                    std_AGEO2real2 = 0.0;
                    P_GEOreal2 = 0;
                    P_AGEO1real2 = 0;
                    P_AGEO2real2 = 0;
                    fx_esperado_funcao = 0.0;
                    parametros_problema = new ParametrosDaFuncao();
                    bits_por_variavel_variaveis = new List<int>();
                    restricoes_laterais_por_variavel = new List<RestricoesLaterais>();
                break;
            }
            

            // =========================================
            // EXECUÇÕES POR NFOB
            // =========================================
            ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB;
            parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            const int step_para_obter_NFOBs = 500;
            parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            parametros_criterio_parada.NFOB_criterio_parada = 100000;
            parametros_criterio_parada.fx_esperado = fx_esperado_funcao;
            
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO        = false;
            quais_algoritmos_rodar.rodar_GEOvar     = false;
            quais_algoritmos_rodar.rodar_AGEO1      = false;
            quais_algoritmos_rodar.rodar_AGEO2      = false;
            quais_algoritmos_rodar.rodar_AGEO1var   = false;
            quais_algoritmos_rodar.rodar_AGEO2var   = false;
            quais_algoritmos_rodar.rodar_GEOreal1   = true;
            quais_algoritmos_rodar.rodar_GEOreal2   = true;
            quais_algoritmos_rodar.rodar_AGEO1real1 = true;
            quais_algoritmos_rodar.rodar_AGEO2real1 = true;
            quais_algoritmos_rodar.rodar_AGEO1real2 = true;
            quais_algoritmos_rodar.rodar_AGEO2real2 = true;
            quais_algoritmos_rodar.rodar_AGEO1_ASTD = false;
            quais_algoritmos_rodar.rodar_AGEO2_ASTD = false;

            OQueInteressaPrintar o_que_interessa_printar = new OQueInteressaPrintar();
            o_que_interessa_printar.mostrar_melhores_NFOB = true;
            o_que_interessa_printar.mostrar_media_NFE_atingido = false;
            o_que_interessa_printar.mostrar_media_melhor_fx = true;
            o_que_interessa_printar.mostrar_SD_melhor_fx = true;
            o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = false;

            const bool printar_status_executando = true;
            const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;

            Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, tau_GEOreal2, tau_minimo_AGEOs, std_GEOreal1, std_GEOreal2, std_AGEO1real1, std_AGEO2real1, std_AGEO1real2, std_AGEO2real2, P_GEOreal2, P_AGEO1real2, P_AGEO2real2, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, printar_status_executando);

            
            // // =========================================
            // // VARIANDO TAU
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAO;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0.001;
            // const int step_para_obter_NFOBs = 10000;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            // parametros_criterio_parada.NFOB_criterio_parada = (int)1e6;
            // parametros_criterio_parada.fx_esperado = fx_esperado_funcao;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = true;
            // quais_algoritmos_rodar.rodar_GEOvar     = true;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // quais_algoritmos_rodar.rodar_AGEO2      = false;
            // quais_algoritmos_rodar.rodar_AGEO1var   = false;
            // quais_algoritmos_rodar.rodar_AGEO2var   = false;
            // quais_algoritmos_rodar.rodar_GEOreal1   = false;
            // quais_algoritmos_rodar.rodar_GEOreal2   = false;
            // quais_algoritmos_rodar.rodar_AGEO1real1 = false;
            // quais_algoritmos_rodar.rodar_AGEO2real1 = false;
            // quais_algoritmos_rodar.rodar_AGEO1real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO2real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO1_ASTD = false;
            // quais_algoritmos_rodar.rodar_AGEO2_ASTD = false;

            // OQueInteressaPrintar o_que_interessa_printar = new OQueInteressaPrintar();
            // o_que_interessa_printar.mostrar_melhores_NFOB = false;
            // o_que_interessa_printar.mostrar_media_NFE_atingido = true;
            // o_que_interessa_printar.mostrar_media_melhor_fx = false;

            // const bool printar_status_executando = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;
            
            // foreach(double tau in valores_TAU){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("{0}", tau.ToString().Replace('.',','));
            
            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau, tau, tau, 0, tau_minimo_AGEOs, std_GEOreal1, 0, std_AGEO1real1, std_AGEO2real1, 0, 0, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, printar_status_executando);
            // }

            
            // // =========================================
            // // EXECUÇÕES VARIANDO PORCENTAGEM
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // const int step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            // parametros_criterio_parada.NFOB_criterio_parada = 100000;
            // parametros_criterio_parada.fx_esperado = fx_esperado_funcao;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = false;
            // quais_algoritmos_rodar.rodar_GEOvar     = false;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // quais_algoritmos_rodar.rodar_AGEO2      = false;
            // quais_algoritmos_rodar.rodar_AGEO1var   = false;
            // quais_algoritmos_rodar.rodar_AGEO2var   = false;
            // quais_algoritmos_rodar.rodar_GEOreal1   = true;
            // quais_algoritmos_rodar.rodar_GEOreal2   = false;
            // quais_algoritmos_rodar.rodar_AGEO1real1 = true;
            // quais_algoritmos_rodar.rodar_AGEO2real1 = true;
            // quais_algoritmos_rodar.rodar_AGEO1real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO2real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO1_ASTD = false;
            // quais_algoritmos_rodar.rodar_AGEO2_ASTD = false;

            // OQueInteressaPrintar o_que_interessa_printar = new OQueInteressaPrintar();
            // o_que_interessa_printar.mostrar_melhores_NFOB = false;
            // o_que_interessa_printar.mostrar_media_NFE_atingido = true;
            // o_que_interessa_printar.mostrar_media_melhor_fx = true;

            // const bool printar_status_executando = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_SDdireto;
            

            // List<double> valores_porcentagem_do_total_p_perturbar = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
            
            // double total_intervalo_variacao_variaveis = (restricoes_laterais_por_variavel[0].limite_superior_variavel - restricoes_laterais_por_variavel[0].limite_inferior_variavel);
            
            // foreach(double porcentagem in valores_porcentagem_do_total_p_perturbar){
            //     double std = porcentagem/100.0 * total_intervalo_variacao_variaveis;

            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("porcentagem;std");
            //     Console.WriteLine( String.Format("{0};{1}", porcentagem, std).Replace('.',',') );

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, printar_status_executando);
            // }


            // // =========================================
            // // EXECUÇÕES VARIANDO STD
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // const int step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            // parametros_criterio_parada.NFOB_criterio_parada = 100000;
            // parametros_criterio_parada.fx_esperado = fx_esperado_funcao;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = false;
            // quais_algoritmos_rodar.rodar_GEOvar     = false;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // quais_algoritmos_rodar.rodar_AGEO2      = false;
            // quais_algoritmos_rodar.rodar_AGEO1var   = false;
            // quais_algoritmos_rodar.rodar_AGEO2var   = false;
            // quais_algoritmos_rodar.rodar_GEOreal1   = true;
            // quais_algoritmos_rodar.rodar_GEOreal2   = false;
            // quais_algoritmos_rodar.rodar_AGEO1real1 = true;
            // quais_algoritmos_rodar.rodar_AGEO2real1 = true;
            // quais_algoritmos_rodar.rodar_AGEO1real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO2real2 = false;
            // quais_algoritmos_rodar.rodar_AGEO1_ASTD = false;
            // quais_algoritmos_rodar.rodar_AGEO2_ASTD = false;

            // OQueInteressaPrintar o_que_interessa_printar = new OQueInteressaPrintar();
            // o_que_interessa_printar.mostrar_melhores_NFOB = false;
            // o_que_interessa_printar.mostrar_media_NFE_atingido = true;
            // o_que_interessa_printar.mostrar_media_melhor_fx = true;

            // const bool printar_status_executando = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;

            // List<double> valores_std = new List<double>(){0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2};
            
            // foreach(double std in valores_std){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("std");
            //     Console.WriteLine( String.Format("{0}", std).Replace('.',',') );

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, printar_status_executando);
            // }
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
            int n_variaveis_projeto = 3;

            ParametrosDaFuncao parametros_problema = new ParametrosDaFuncao();
            parametros_problema.n_variaveis_projeto = n_variaveis_projeto;
            parametros_problema.definicao_funcao_objetivo = 3;

            CodificacaoBinariaParaFenotipo codificacao_binaria_para_fenotipo = new CodificacaoBinariaParaFenotipo();
            codificacao_binaria_para_fenotipo.bits_por_variavel_variaveis = new List<int>(){2,6,6};
            codificacao_binaria_para_fenotipo.limites_inferiores_variaveis = new List<double>(){13,1,0};
            codificacao_binaria_para_fenotipo.limites_superiores_variaveis = new List<double>(){15,60,59};
           
            // Define a quantidade de execuções
            // int quantidade_execucoes = 50;


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
            parametros_criterio_parada.fx_esperado = 196.949433192159;

            // Define quais algoritmos executar
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            quais_algoritmos_rodar.rodar_GEO = true;
            quais_algoritmos_rodar.rodar_GEOvar = true;
            quais_algoritmos_rodar.rodar_AGEO1 = false;
            quais_algoritmos_rodar.rodar_AGEO2 = false;
            quais_algoritmos_rodar.rodar_AGEO1var = false;
            quais_algoritmos_rodar.rodar_AGEO2var = false;

            // int NFE_ou_MELHORFX = 1;

            foreach(double tau in valores_TAU){
                Console.WriteLine("\n==========================================");
                Console.WriteLine("TAU = {0}", tau);
                // Executa_Todos_Algoritmos_Por_NFE_OU_MELHORFX(NFE_ou_MELHORFX, quantidade_execucoes, tau, tau, parametros_problema, codificacao_binaria_para_fenotipo, parametros_criterio_parada, quais_algoritmos_rodar);
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

            Execucoes();

            // ExtensiveSearch_SpacecraftOptimization();
            // Teste_FuncoesObjetivo_SpacecraftOptimization();
            // Execucoes_SpacecraftOptimization();
        }
    }
}