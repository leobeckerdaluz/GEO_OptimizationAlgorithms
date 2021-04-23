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
        public Random random = new Random();

        // =====================================
        // ROTINAS PARA EXECUÇÃO
        // =====================================

        public string populacao_real_em_string(List<double> populacao){
            string populacao_str = populacao[0].ToString();
            for(int i=1; i<populacao.Count; i++){
                populacao_str += "," + populacao[i].ToString();
            }
            return populacao_str;
        }


        public List<double> geracao_populacao_real(int n_variaveis_projeto, List<RestricoesLaterais> restricoes_laterais_variaveis){
            
            List<double> nova_populacao = new List<double>();

            for(int i=0; i<n_variaveis_projeto; i++){
                double limite_inferior_variavel = restricoes_laterais_variaveis[i].limite_inferior_variavel;
                double limite_superior_variavel = restricoes_laterais_variaveis[i].limite_superior_variavel;
                double rand = random.NextDouble();

                double xi = limite_inferior_variavel + ((limite_superior_variavel - limite_inferior_variavel) * rand);

                #if DEBUG_CONSOLE
                    Console.WriteLine("xi gerado = {0}", xi);
                #endif

                nova_populacao.Add(xi);
            }

            #if DEBUG_CONSOLE
                Console.WriteLine("População gerada:");
                foreach(double ind in populacao_atual){
                    Console.WriteLine("individuo = {0}", ind);
                }
            #endif

            // Retorna a população criada
            return nova_populacao;
        }


        public void apresenta_resultados_finais(OQueInteressaPrintar o_que_interessa_printar, List<RetornoGEOs> todas_execucoes, ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, QuaisAlgoritmosRodar quais_algoritmos_rodar){

            Console.WriteLine("Apresenta todas estatísticas");

            // Obtém a lista de algoritmos que foram executados
            List<int> algoritmos_executados = new List<int>();
            if (quais_algoritmos_rodar.rodar_GEO)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEO_can );
            if (quais_algoritmos_rodar.rodar_GEOvar)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEO_var );
            if (quais_algoritmos_rodar.rodar_AGEO1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1 );
            if (quais_algoritmos_rodar.rodar_AGEO2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2 );
            if (quais_algoritmos_rodar.rodar_AGEO1var)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1var );
            if (quais_algoritmos_rodar.rodar_AGEO2var)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2var );
            if (quais_algoritmos_rodar.rodar_GEOreal1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEOreal1 );
            if (quais_algoritmos_rodar.rodar_AGEO1real1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1real1 );
            if (quais_algoritmos_rodar.rodar_AGEO2real1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2real1 );
            if (quais_algoritmos_rodar.rodar_GEOreal2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEOreal2 );
            if (quais_algoritmos_rodar.rodar_AGEO1real2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1real2 );
            if (quais_algoritmos_rodar.rodar_AGEO2real2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2real2 );
            if (quais_algoritmos_rodar.rodar_GEOreal3)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEOreal3 );
            if (quais_algoritmos_rodar.rodar_AGEO1real3)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1real3 );
            if (quais_algoritmos_rodar.rodar_AGEO2real3)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2real3 );

            // Cria uma lista que irá conter as estatísticas para cada algoritmo que foi executado
            List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos = new List<Retorno_N_Execucoes_GEOs>();

            Console.WriteLine("Chegou aqui!! algoritmos_executados Count: {0}", algoritmos_executados.Count);
            foreach (var item in algoritmos_executados){
                Console.WriteLine(item);
            }

            // Para cada algoritmo executado, processa as execuções
            for (int i=0; i<algoritmos_executados.Count; i++){
                int algoritmo_executado = algoritmos_executados[i];

                Console.WriteLine("Chegou aqui!! todas_execucoes Count: {0}", todas_execucoes.Count);

                // Filtra todas as execuções obtendo somente as execuções deste algoritmo
                List<RetornoGEOs> execucoes_algoritmo_executado = todas_execucoes.Where(p => p.algoritmo_utilizado == algoritmo_executado).ToList();

                Console.WriteLine("Chegou aqui2!! execucoes_algoritmo_executado Count: {0}", execucoes_algoritmo_executado.Count);

                // Inicializa a lista que irá conter os f(x) em cada NFOB
                List<List<double>> NFOBs_todas_execucoes = new List<List<double>>();
                // Inicializa o somatório dos NFOBs finais
                int somatorio_NFOBs = 0;
                // Inicializa o somatório dos melhores f(x)
                double somatorio_melhorFX = 0;
                // Inicializa a lista que irá conter os melhores f(x) para cada execução
                List<double> lista_melhores_fx = new List<double>();
                // Inicializa a lista que irá conter a população final para cada execução
                List<List<double>> lista_populacoes_finais = new List<List<double>>();

                Console.WriteLine("Chegou aqui!!");

                // Para cada execução, vai acumulando os resultados
                for (int j=0; j<execucoes_algoritmo_executado.Count; j++){
                    RetornoGEOs ret = execucoes_algoritmo_executado[j];

                    // Adiciona a lista de fxs em cada NFOBs da execução em uma lista geral
                    NFOBs_todas_execucoes.Add( ret.melhores_NFOBs );
                    // Somatório de NFOBs para posterior média
                    somatorio_NFOBs += ret.NFOB;
                    // Somatório de melhorFX para posterior média
                    somatorio_melhorFX += ret.melhor_fx;
                    // Adiciona o melhor f(x) na lista dos melhores f(x)s
                    lista_melhores_fx.Add( ret.melhor_fx );
                    // Adiciona a população da execução na lista de populações das execuções
                    lista_populacoes_finais.Add( ret.populacao_final );
                }

                Console.WriteLine("Chegou aqui2!! NFOBs_todas_execucoes Count: {0}", NFOBs_todas_execucoes.Count);

                // Lista irá conter o f(x) médio para cada NFOB desejado
                List<double> lista_fxs_medios_cada_NFOB_desejado = new List<double>();

                // Obtém a quantidade de NFOBs armazenados
                int quantidade_NFOBs = NFOBs_todas_execucoes[0].Count;

                Console.WriteLine("Chegou aqui3!!");
                
                // Se o critério de parada é por NFOB, calcula a média das N execuções para cada NFOB desejado.
                if (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB){
                    for(int k=0; k<quantidade_NFOBs; k++){
                        

                        // Percorre aquele NFOB em cada execução para calcular a média de f(x) naquele NFOB
                        double sum = 0;
                        foreach(List<double> execution in NFOBs_todas_execucoes){
                            sum += execution[k];
                        }
                        double media = sum / NFOBs_todas_execucoes.Count;
                    
                        // Adiciona o f(x) médio do NFOB na lista
                        lista_fxs_medios_cada_NFOB_desejado.Add(media);
                    }
                }

                // Calcula a média dos NFOBs
                int media_NFOBs = (int) (somatorio_NFOBs / parametros_execucao.quantidade_execucoes);

                // Calcula a média dos melhores f(x)
                double media_melhor_fx = (double) somatorio_melhorFX / parametros_execucao.quantidade_execucoes;

                // Calcula o desvio padrão final com base nos melhores f(x) das execuções
                double somatorio_sd = 0;
                foreach (double melhor_fx in lista_melhores_fx){
                    somatorio_sd += Math.Pow((melhor_fx - media_melhor_fx), 2);
                }
                int n = lista_melhores_fx.Count - 1;
                double SD_melhor_fx = Math.Sqrt(somatorio_sd / n);

                // Cria um objeto contendo os resultados finais do processamento desse algoritmo
                // ... executado para a N execuções
                Retorno_N_Execucoes_GEOs media_das_execucoes = new Retorno_N_Execucoes_GEOs();
                media_das_execucoes.algoritmo_utilizado = algoritmo_executado;
                media_das_execucoes.NFOB_medio = media_NFOBs;
                media_das_execucoes.media_melhor_fx = media_melhor_fx;
                media_das_execucoes.SD_do_melhor_fx = SD_melhor_fx;
                media_das_execucoes.media_valor_FO_em_cada_NFOB = lista_fxs_medios_cada_NFOB_desejado;
                media_das_execucoes.lista_melhores_fxs = lista_melhores_fx;
                media_das_execucoes.lista_populacao_final = lista_populacoes_finais;  

                // Adiciona essa estatística da execução na lista geral de estatísticas
                estatisticas_algoritmos.Add(media_das_execucoes);
            }

            // =======================================================================================

            // Para cada algoritmo da lista de estatísticas dos algoritmos, apresenta os resultados






            // // ===========================================================
            // // Mostra a média dos f(x) nas execuções em cada NFOB
            // // ===========================================================

            // string algoritmos_executados = "";
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO ? "GEO;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar ? "GEOvar;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1 ? "AGEO1;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2 ? "AGEO2;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var ? "AGEO1var;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var ? "AGEO2var;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1 ? "GEOreal1;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2 ? "GEOreal2;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1 ? "AGEO1real1;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1 ? "AGEO2real1;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2 ? "AGEO1real2;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2 ? "AGEO2real2;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3 ? "GEOreal3;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3 ? "AGEO1real3;" : "");
            // algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3 ? "AGEO2real3;" : "");
            

            // if (parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFOB){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> Médias para cada NFOB:");
            //     Console.WriteLine("NFOB;" + algoritmos_executados);
                
            //     // Apresenta a média para cada NFOB
            //     int NFOB_atual = parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs;
            //     for(int i=0; i<quantidade_NFOBs; i++){
            //         string fxs_naquele_NFOB = "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (retorno_GEOreal3.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (retorno_AGEO1real3.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";
            //         fxs_naquele_NFOB += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (retorno_AGEO2real3.media_valor_FO_em_cada_NFOB[i].ToString()+';') : "";

            //         Console.WriteLine(NFOB_atual + ";" + fxs_naquele_NFOB.Replace('.',','));

            //         NFOB_atual += parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs;
            //     }
            // }
            // if (parametros_execucao.o_que_interessa_printar.mostrar_media_NFE_atingido){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> Média NFEs atingidos:");
            //     Console.WriteLine(algoritmos_executados);
            //     string media_do_NFE_atingido_nas_execucoes = "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (retorno_GEOreal3.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (retorno_AGEO1real3.NFOB_medio.ToString()+';') : "";
            //     media_do_NFE_atingido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (retorno_AGEO2real3.NFOB_medio.ToString()+';') : "";

            //     // Substitui os pontos por vírgulas e printa
            //     Console.WriteLine( media_do_NFE_atingido_nas_execucoes.Replace('.',',') );
            // }
            // if (parametros_execucao.o_que_interessa_printar.mostrar_media_melhor_fx){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> AVG do melhor f(x) nas N execuções:");
            //     Console.WriteLine(algoritmos_executados);
            //     string media_do_melhor_FX_obtido_nas_execucoes = "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (retorno_GEOreal3.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (retorno_AGEO1real3.media_melhor_fx.ToString()+';') : "";
            //     media_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (retorno_AGEO2real3.media_melhor_fx.ToString()+';') : "";

            //     // Substitui os pontos por vírgulas e printa
            //     Console.WriteLine(media_do_melhor_FX_obtido_nas_execucoes.Replace('.',','));
            // }
            // if (parametros_execucao.o_que_interessa_printar.mostrar_SD_melhor_fx){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> SD do melhor f(x) nas N execuções:");
            //     Console.WriteLine(algoritmos_executados);
            //     string SD_do_melhor_FX_obtido_nas_execucoes = "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (retorno_GEOreal3.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (retorno_AGEO1real3.SD_do_melhor_fx.ToString()+';') : "";
            //     SD_do_melhor_FX_obtido_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (retorno_AGEO2real3.SD_do_melhor_fx.ToString()+';') : "";

            //     // Substitui os pontos por vírgulas e printa
            //     Console.WriteLine(SD_do_melhor_FX_obtido_nas_execucoes.Replace('.',','));
            // }
            // if (parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> Melhor f(x) para cada uma das N execuções:");
            //     Console.WriteLine(algoritmos_executados);
                
            //     for(int n=0; n<parametros_execucao.quantidade_execucoes; n++){
            //         string melhor_fx_nas_execucoes = "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (retorno_GEO.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (retorno_GEOvar.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (retorno_AGEO1.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (retorno_AGEO2.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (retorno_AGEO1var.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (retorno_AGEO2var.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (retorno_GEOreal1.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (retorno_GEOreal2.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (retorno_AGEO1real1.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (retorno_AGEO2real1.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (retorno_AGEO1real2.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (retorno_AGEO2real2.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (retorno_GEOreal3.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (retorno_AGEO1real3.lista_melhores_fxs[n].ToString()+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (retorno_AGEO2real3.lista_melhores_fxs[n].ToString()+';') : "";

            //         // Substitui os pontos por vírgulas e printa
            //         Console.WriteLine(melhor_fx_nas_execucoes.Replace('.',','));
            //     }
            // }
            // if (parametros_execucao.o_que_interessa_printar.mostrar_populacao_cada_execucao){
            //     Console.WriteLine("");
            //     Console.WriteLine("===> População para cada uma das N execuções:");
            //     Console.WriteLine(algoritmos_executados);
                
            //     for(int n=0; n<parametros_execucao.quantidade_execucoes; n++){
            //         string melhor_fx_nas_execucoes = "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO) ? (populacao_real_em_string(retorno_GEO.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar) ? (populacao_real_em_string(retorno_GEOvar.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1) ? (populacao_real_em_string(retorno_AGEO1.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2) ? (populacao_real_em_string(retorno_AGEO2.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var) ? (populacao_real_em_string(retorno_AGEO1var.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var) ? (populacao_real_em_string(retorno_AGEO2var.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1) ? (populacao_real_em_string(retorno_GEOreal1.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2) ? (populacao_real_em_string(retorno_GEOreal2.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1) ? (populacao_real_em_string(retorno_AGEO1real1.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1) ? (populacao_real_em_string(retorno_AGEO2real1.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2) ? (populacao_real_em_string(retorno_AGEO1real2.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2) ? (populacao_real_em_string(retorno_AGEO2real2.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3) ? (populacao_real_em_string(retorno_GEOreal3.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3) ? (populacao_real_em_string(retorno_AGEO1real3.populacao[n])+';') : "";
            //         melhor_fx_nas_execucoes += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3) ? (populacao_real_em_string(retorno_AGEO2real3.populacao[n])+';') : "";

            //         // Substitui os pontos por vírgulas e printa
            //         Console.WriteLine(melhor_fx_nas_execucoes.Replace('.',','));
            //     }
            // }
        }
            
            
        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public void Execucoes(){
            
            // ======================================================================
            // DEFINE OS PARÂMETROS DO PROBLEMA
            // ======================================================================
            int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.schwefel;

            // Define os parâmetros do problema conforme cada função
            ParametrosProblema parametros_problema;

            switch(definicao_funcao_objetivo){
                // RASTRINGIN
                case (int)EnumNomesFuncoesObjetivo.rastringin:
                    int n_variaveis = 20;

                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = n_variaveis;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.rastringin;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 1.00,
                        tau_GEOvar = 1.75,
                        tau_GEOreal1 = 1.75,
                        tau_GEOreal2 = 2.25,
                        tau_GEOreal3 = 2.25,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 1.0,
                        std_AGEO1real1 = 1.0,
                        std_AGEO2real1 = 1.0,
                        std_GEOreal2 = 8.0,
                        std_AGEO1real2 = 8.0,
                        std_AGEO2real2 = 8.0,
                        std_GEOreal3 = 8.0,
                        std_AGEO1real3 = 8.0,
                        std_AGEO2real3 = 8.0,
                        P_GEOreal2 = 8,
                        P_AGEO1real2 = 8,
                        P_AGEO2real2 = 8,
                        P_GEOreal3 = 8,
                        P_AGEO1real3 = 8,
                        P_AGEO2real3 = 8
                    };
                    // valores_TAU = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
                break;

                // GRIEWANGK
                case (int)EnumNomesFuncoesObjetivo.griewangk:
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.griewangk;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 1.25,
                        tau_GEOvar = 3.0,
                        tau_GEOreal1 = 1.25,
                        tau_GEOreal2 = 1.75,
                        tau_GEOreal3 = 1.75,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 1.0,
                        std_AGEO1real1 = 1.0,
                        std_AGEO2real1 = 1.0,
                        std_GEOreal2 = 8.0,
                        std_AGEO1real2 = 8.0,
                        std_AGEO2real2 = 8.0,
                        std_GEOreal3 = 8.0,
                        std_AGEO1real3 = 8.0,
                        std_AGEO2real3 = 8.0,
                        P_GEOreal2 = 16,
                        P_AGEO1real2 = 16,
                        P_AGEO2real2 = 16,
                        P_GEOreal3 = 16,
                        P_AGEO1real3 = 16,
                        P_AGEO2real3 = 16
                    };
                    // valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4};
                break;

                // ROSENBROCK
                case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.rosenbrock;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-2.048, limite_superior_variavel=2.048} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 1.00,
                        tau_GEOvar = 1.25,
                        tau_GEOreal1 = 1.00,
                        tau_GEOreal2 = 1.25,
                        tau_GEOreal3 = 1.25,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 2.0,
                        std_AGEO1real1 = 2.0,
                        std_AGEO2real1 = 2.0,
                        std_GEOreal2 = 1.0,
                        std_AGEO1real2 = 1.0,
                        std_AGEO2real2 = 1.0,
                        std_GEOreal3 = 1.0,
                        std_AGEO1real3 = 1.0,
                        std_AGEO2real3 = 1.0,
                        P_GEOreal2 = 2,
                        P_AGEO1real2 = 2,
                        P_AGEO2real2 = 2,
                        P_GEOreal3 = 2,
                        P_AGEO1real3 = 2,
                        P_AGEO2real3 = 2
                    };
                    // valores_TAU = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3};
                break;

                // SCHWEFEL
                case (int)EnumNomesFuncoesObjetivo.schwefel:
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.schwefel;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 1.00,
                        tau_GEOvar = 1.75,
                        tau_GEOreal1 = 6.25,
                        tau_GEOreal2 = 9,
                        tau_GEOreal3 = 9,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 1.0,
                        std_AGEO1real1 = 1.0,
                        std_AGEO2real1 = 1.0,
                        std_GEOreal2 = 8.0,
                        std_AGEO1real2 = 8.0,
                        std_AGEO2real2 = 8.0,
                        std_GEOreal3 = 8.0,
                        std_AGEO1real3 = 8.0,
                        std_AGEO2real3 = 8.0,
                        P_GEOreal2 = 16,
                        P_AGEO1real2 = 16,
                        P_AGEO2real2 = 16,
                        P_GEOreal3 = 16,
                        P_AGEO1real3 = 16,
                        P_AGEO2real3 = 16
                    };
                break;

                // ACKLEY
                case (int)EnumNomesFuncoesObjetivo.ackley:

                    int n_variaveis_projeto = 30;

                    parametros_problema = new ParametrosProblema(){
                        n_variaveis_projeto = n_variaveis_projeto,
                        definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.ackley,
                        bits_por_variavel = Enumerable.Repeat(16, n_variaveis_projeto).ToList(),
                        restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-30.0, limite_superior_variavel=30.0} , n_variaveis_projeto).ToList(),
                        parametros_livres = new ParametrosLivreProblema(){
                            tau_GEO = 2.25,
                            tau_GEOvar = 2.50,
                            tau_GEOreal1 = 2.25,
                            tau_GEOreal2 = 2.50,
                            tau_GEOreal3 = 2.50,
                            tau_minimo_AGEOs = 0.5,
                            std_GEOreal1 = 1.0,
                            std_AGEO1real1 = 1.0,
                            std_AGEO2real1 = 1.0,
                            std_GEOreal2 = 1.0,
                            std_AGEO1real2 = 1.0,
                            std_AGEO2real2 = 1.0,
                            std_GEOreal3 = 1.0,
                            std_AGEO1real3 = 1.0,
                            std_AGEO2real3 = 1.0,
                            P_GEOreal2 = 8,
                            P_AGEO1real2 = 8,
                            P_AGEO2real2 = 8,
                            P_GEOreal3 = 8,
                            P_AGEO1real3 = 8,
                            P_AGEO2real3 = 8
                        }
                    };
                break;

                // F9 TESE
                case (int)EnumNomesFuncoesObjetivo.F09:
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.F09;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-10.0, limite_superior_variavel=10.0} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 1.50,
                        tau_GEOvar = 1.75,
                        tau_GEOreal1 = 1.50,
                        tau_GEOreal2 = 1.75,
                        tau_GEOreal3 = 1.75,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 1.0,
                        std_AGEO1real1 = 1.0,
                        std_AGEO2real1 = 1.0,
                        std_GEOreal2 = 1.0,
                        std_AGEO1real2 = 1.0,
                        std_AGEO2real2 = 1.0,
                        std_GEOreal3 = 1.0,
                        std_AGEO1real3 = 1.0,
                        std_AGEO2real3 = 1.0,
                        P_GEOreal2 = 8,
                        P_AGEO1real2 = 8,
                        P_AGEO2real2 = 8,
                        P_GEOreal3 = 8,
                        P_AGEO1real3 = 8,
                        P_AGEO2real3 = 8,
                    };
                break;

                // DEJONG3
                case (int)EnumNomesFuncoesObjetivo.dejong3:
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 5;
                    parametros_problema.definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.dejong3;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.restricoes_laterais_por_variavel = Enumerable.Repeat( new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12} , parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                        tau_GEO = 3.0,
                        tau_GEOvar = 8.0,
                        tau_GEOreal1 = 3.0,
                        tau_GEOreal2 = 8.0,
                        tau_GEOreal3 = 8.0,
                        tau_minimo_AGEOs = 0.5,
                        std_GEOreal1 = 1.0,
                        std_AGEO1real1 = 1.0,
                        std_AGEO2real1 = 1.0,
                        std_GEOreal2 = 1.0,
                        std_AGEO1real2 = 1.0,
                        std_AGEO2real2 = 1.0,
                        std_GEOreal3 = 1.0,
                        std_AGEO1real3 = 1.0,
                        std_AGEO2real3 = 1.0,
                        P_GEOreal2 = 8,
                        P_AGEO1real2 = 8,
                        P_AGEO2real2 = 8,
                        P_GEOreal3 = 8,
                        P_AGEO1real3 = 8,
                        P_AGEO2real3 = 8
                    };
                break;

                // DEFAULT
                default:
                    Console.WriteLine("DEFAULT PARAMETERS!");
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.n_variaveis_projeto = 0;
                    parametros_problema.definicao_funcao_objetivo = 0;
                    parametros_problema.bits_por_variavel = new List<int>();
                    parametros_problema.restricoes_laterais_por_variavel = new List<RestricoesLaterais>();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                    parametros_problema.populacao_inicial = new List<double>();
                break;
            }

            // ======================================================================
            // DEFINE OS PARÂMETROS DA EXECUÇÃO
            // ======================================================================
            ParametrosExecucao parametros_execucao = new ParametrosExecucao(){
                quantidade_execucoes = 2,
                parametros_criterio_parada = new ParametrosCriterioParada(){
                    tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB,
                    PRECISAO_criterio_parada = 0,
                    step_para_obter_NFOBs = 50,
                    NFOB_criterio_parada = 1000,
                    fx_esperado = 0.0
                },
                quais_algoritmos_rodar = new QuaisAlgoritmosRodar(){
                    rodar_GEO           = false,
                    rodar_GEOvar        = false,
                    rodar_AGEO1         = false,
                    rodar_AGEO2         = false,
                    rodar_AGEO1var      = false,
                    rodar_AGEO2var      = false,
                    rodar_GEOreal1      = true,
                    rodar_AGEO1real1    = false,
                    rodar_AGEO2real1    = false,
                    rodar_GEOreal2      = true,
                    rodar_AGEO1real2    = false,
                    rodar_AGEO2real2    = false,
                    rodar_GEOreal3      = false,
                    rodar_AGEO1real3    = false,
                    rodar_AGEO2real3    = false
                },
                o_que_interessa_printar = new OQueInteressaPrintar(){
                    mostrar_melhores_NFOB = true,
                    mostrar_media_NFE_atingido = false,
                    mostrar_media_melhor_fx = true,
                    mostrar_SD_melhor_fx = true,
                    mostrar_melhores_fx_cada_execucao = true,
                    mostrar_populacao_cada_execucao = false
                }
            };

            // Lista armazenará o retorno de todas as execuções
            List<RetornoGEOs> todas_execucoes = new List<RetornoGEOs>();

            // Executa os algoritmos por N vezes
            for(int i=0; i<parametros_execucao.quantidade_execucoes; i++){
                Console.Write((i+1) +"...");

                // Para cada execução, gera uma nova população inicial com base nos limites de cada variável
                parametros_problema.populacao_inicial = new List<double>(geracao_populacao_real(parametros_problema.n_variaveis_projeto, parametros_problema.restricoes_laterais_por_variavel));

                Console.WriteLine("População inicial GERADA: {0}", populacao_real_em_string(parametros_problema.populacao_inicial) );
            
                // GEO
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEO){
                    Console.Write("\nGEO...");

                    GEO_BINARIO geo = new GEO_BINARIO(parametros_problema.parametros_livres.tau_GEO, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can;

                    todas_execucoes.Add(ret);
                }

                // GEOvar
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar){
                    Console.Write("\nGEOvar...");

                    GEOvar_BINARIO geo_var = new GEOvar_BINARIO(parametros_problema.parametros_livres.tau_GEOvar, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var;
                     
                    todas_execucoes.Add(ret);
                }

                // AGEO1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1){
                    Console.Write("\nAGEO1...");

                    AGEOs_BINARIO ageo1 = new AGEOs_BINARIO(1, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2){
                    Console.Write("\nAGEO2...");
                    
                    AGEOs_BINARIO ageo2 = new AGEOs_BINARIO(2, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var){
                    Console.Write("\nAGEO1var...");
                    
                    AGEOsvar_BINARIO ageo1_var = new AGEOsvar_BINARIO(1, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var){
                    Console.Write("\nAGEO2var...");

                    AGEOsvar_BINARIO ageo2_var = new AGEOsvar_BINARIO(2, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2_var.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1){
                    Console.Write("\nGEOreal1...");

                    GEO_real1 geo_real1 = new GEO_real1(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_GEOreal1, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_GEOreal1, 0);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1;

                    todas_execucoes.Add(ret);
                }

                // GEOreal2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2){
                    Console.Write("\nGEOreal2...");

                    GEO_real2 geo_real2 = new GEO_real2(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_GEOreal2, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_GEOreal2, 0, (int)parametros_problema.parametros_livres.P_GEOreal2);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1real1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1){
                    Console.Write("\nAGEO1real1...");

                    AGEOs_REAL1 AGEO1real1 = new AGEOs_REAL1(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO1real1, 1, 0);
                        
                    RetornoGEOs ret = AGEO1real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real1;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1){
                    Console.Write("\nAGEO2real1...");

                    AGEOs_REAL1 AGEO2real1 = new AGEOs_REAL1(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO2real1, 2, 0);
                        
                    RetornoGEOs ret = AGEO2real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real1;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1real2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2){
                    Console.Write("\nAGEO1real2...");

                    AGEOs_REAL2 AGEO1real2 = new AGEOs_REAL2(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO1real2, 1, 0, (int)parametros_problema.parametros_livres.P_AGEO1real2);
                        
                    RetornoGEOs ret = AGEO1real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real2;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2){
                    Console.Write("\nAGEO2real2...");

                    AGEOs_REAL2 AGEO2real2 = new AGEOs_REAL2(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO2real2, 2, 0, (int)parametros_problema.parametros_livres.std_AGEO2real2);
                        
                    RetornoGEOs ret = AGEO2real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2;

                    todas_execucoes.Add(ret);
                }

                // GEOreal3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal3){
                    Console.Write("\nGEOreal3...");

                    GEO_real3 geo_real3 = new GEO_real3(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_GEOreal3, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_GEOreal3, 0, (int)parametros_problema.parametros_livres.P_GEOreal3);
                        
                    RetornoGEOs ret = geo_real3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal3;

                    todas_execucoes.Add(ret);
                }
                
                // AGEO1real3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real3){
                    Console.Write("\nAGEO1real3...");

                    AGEOs_REAL2 AGEO1real3 = new AGEOs_REAL2(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO1real3, 1, 0, (int)parametros_problema.parametros_livres.P_AGEO1real3);
                        
                    RetornoGEOs ret = AGEO1real3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real3;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real3){
                    Console.Write("\nAGEO2real3...");

                    AGEOs_REAL2 AGEO2real3 = new AGEOs_REAL2(parametros_problema.populacao_inicial, parametros_problema.parametros_livres.tau_minimo_AGEOs, parametros_problema.n_variaveis_projeto, parametros_problema.definicao_funcao_objetivo, parametros_problema.restricoes_laterais_por_variavel, parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, parametros_problema.parametros_livres.std_AGEO2real3, 2, 0, (int)parametros_problema.parametros_livres.std_AGEO2real3);
                        
                    RetornoGEOs ret = AGEO2real3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real3;

                    todas_execucoes.Add(ret);
                }
            }


            apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, todas_execucoes, parametros_execucao, parametros_problema, parametros_execucao.quais_algoritmos_rodar);








            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;
            
            // // =========================================
            // // VARIANDO TAU
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAO;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0.001;
            // const int step_para_obter_NFOBs = 10000;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = true;
            // quais_algoritmos_rodar.rodar_GEOvar     = true;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // parametros_criterio_parada.fx_esperado = ;
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

            // const bool  = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;
            
            // foreach(double tau in valores_TAU){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("{0}", tau.ToString().Replace('.',','));
            
            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau, tau, tau, 0, tau_minimo_AGEOs, std_GEOreal1, 0, std_AGEO1real1, std_AGEO2real1, 0, 0, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, );
            // }

            
            // // =========================================
            // // EXECUÇÕES VARIANDO PORCENTAGEM
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // const int step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = false;
            // quais_algoritmos_rodar.rodar_GEOvar     = false;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // parametros_criterio_parada.fx_esperado = ;
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

            // const bool  = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_SDdireto;
            

            // List<double> valores_porcentagem_do_total_p_perturbar = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
            
            // double total_intervalo_variacao_variaveis = (restricoes_laterais_por_variavel[0].limite_superior_variavel - restricoes_laterais_por_variavel[0].limite_inferior_variavel);
            
            // foreach(double porcentagem in valores_porcentagem_do_total_p_perturbar){
            //     double std = porcentagem/100.0 * total_intervalo_variacao_variaveis;

            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("porcentagem;std");
            //     Console.WriteLine( String.Format("{0};{1}", porcentagem, std).Replace('.',',') );

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar);
            // }


            // // =========================================
            // // EXECUÇÕES VARIANDO STD
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // const int step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            
            // QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            // quais_algoritmos_rodar.rodar_GEO        = false;
            // quais_algoritmos_rodar.rodar_GEOvar     = false;
            // quais_algoritmos_rodar.rodar_AGEO1      = false;
            // parametros_criterio_parada.fx_esperado = ;
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

            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_original;

            // List<double> valores_std = new List<double>(){0.4, 0.6, 0.8, 1, 1.2, 1.4, 1.6, 1.8, 2};
            
            // foreach(double std in valores_std){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("std");
            //     Console.WriteLine( String.Format("{0}", std).Replace('.',',') );

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, restricoes_laterais_por_variavel, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar);
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

            ParametrosProblema parametros_problema = new ParametrosProblema();
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
    }

    public class GEO {
        // =====================================
        // MAIN
        // =====================================

        public static void Main(string[] args){
            Console.WriteLine("Rodando!");

            Execucoes_GEO ex = new Execucoes_GEO();
            ex.Execucoes();

            // ExtensiveSearch_SpacecraftOptimization();
            // Teste_FuncoesObjetivo_SpacecraftOptimization();
            // Execucoes_SpacecraftOptimization();
        }
    }
}