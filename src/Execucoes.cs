
#define CONSOLE_OUT_FILE

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


using System.IO;
using System.Threading.Tasks;


namespace Execucoes
{
    public class Execucoes_GEO
    {
        // public Random random = new Random();

        // =====================================
        // ROTINAS PARA EXECUÇÃO
        // =====================================

        public string populacao_real_em_string(List<double> populacao)
        {
            string populacao_str = populacao[0].ToString();
            for(int i=1; i<populacao.Count; i++)
            {
                populacao_str += "," + populacao[i].ToString();
            }
            return populacao_str;
        }


        public static List<double> geracao_populacao_real(List<double> lower_bounds, List<double> upper_bounds, int seed)
        {
            double size = lower_bounds.Count;
            
            List<double> nova_populacao = new List<double>();
            Random rnd = new Random(seed);

            for(int i=0; i<size; i++)
            {
                double lower = lower_bounds[i];
                double upper = upper_bounds[i];
                
                double rand = rnd.NextDouble();

                double xi = lower + ((upper - lower) * rand);

                nova_populacao.Add(xi);
            }

            #if DEBUG_CONSOLE
                Console.WriteLine("População gerada:");
                foreach(double ind in populacao_atual)
                {
                    Console.WriteLine("individuo = {0}", ind);
                }
            #endif
            
            // Retorna a população criada
            return nova_populacao;
        }


        public List<Retorno_N_Execucoes_GEOs> organiza_os_resultados_de_cada_execucao(List<RetornoGEOs> todas_execucoes, ParametrosExecucao parametros_execucao)
        {
            // Obtém a lista de algoritmos que foram executados
            List<int> algoritmos_executados = new List<int>();
            if (parametros_execucao.quais_algoritmos_rodar.rodar_GEO)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEO_can );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEO_var );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1var );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2var );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEOreal1 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1real1 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2real1 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.GEOreal2 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO1real2 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.AGEO2real2 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_1)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.ASGEO2real1_1 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_2)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.ASGEO2real1_2 );
            if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_3)   algoritmos_executados.Add( (int)EnumNomesAlgoritmos.ASGEO2real1_3 );

            // Cria uma lista que irá conter as estatísticas para cada algoritmo que foi executado
            List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos = new List<Retorno_N_Execucoes_GEOs>();

            // Para cada algoritmo executado, processa as execuções
            for (int i=0; i<algoritmos_executados.Count; i++)
            {
                int algoritmo_executado = algoritmos_executados[i];

                // Console.WriteLine("Chegou aqui!! todas_execucoes Count: {0}", todas_execucoes.Count);

                // Filtra todas as execuções obtendo somente as execuções deste algoritmo
                List<RetornoGEOs> execucoes_algoritmo_executado = todas_execucoes.Where(p => p.algoritmo_utilizado == algoritmo_executado).ToList();

                // Console.WriteLine("Chegou aqui2!! execucoes_algoritmo_executado Count: {0}", execucoes_algoritmo_executado.Count);

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

                // Para cada execução, vai acumulando os resultados
                for (int j=0; j<execucoes_algoritmo_executado.Count; j++)
                {
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

                // Lista irá conter o f(x) médio para cada NFOB desejado
                List<double> lista_fxs_medios_cada_NFOB_desejado = new List<double>();

                // Obtém a quantidade de NFOBs armazenados
                int quantidade_NFOBs = NFOBs_todas_execucoes[0].Count;

                // Se o critério de parada é por NFOB, calcula a média das N execuções para cada NFOB desejado.
                if (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB)
                {
                    for(int k=0; k<quantidade_NFOBs; k++)
                    {
                        // Percorre aquele NFOB em cada execução para calcular a média de f(x) naquele NFOB
                        double sum = 0;
                        foreach(List<double> execution in NFOBs_todas_execucoes)
                        {
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
                foreach (double melhor_fx in lista_melhores_fx)
                {
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

                // Adiciona essa estatística do algoritmo na lista geral de estatísticas
                estatisticas_algoritmos.Add(media_das_execucoes);
            }

            return estatisticas_algoritmos;
        }


        public void apresenta_resultados_finais(OQueInteressaPrintar o_que_interessa_printar, List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos, ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema)
        {
            // ===========================================================
            // Mostra as estatísticas das execuções
            // ===========================================================

            Console.WriteLine("");

            if (o_que_interessa_printar.mostrar_header)
            {
                Console.WriteLine("\n\n==========================================================");
                Console.WriteLine("Apresenta todas estatísticas");
                Console.WriteLine("==========================================================\n");
                Console.WriteLine(String.Format("Função objetivo utilizada: {0} - {1}", parametros_problema.definicao_funcao_objetivo, parametros_problema.nome_funcao));
                Console.WriteLine(String.Format("Número de variáveis de projeto: {0}", parametros_problema.n_variaveis_projeto));
                Console.WriteLine(String.Format("Quantidade de execuções: {0}", parametros_execucao.quantidade_execucoes));
                Console.WriteLine(String.Format("Tipo de critério de parada: {0}", parametros_execucao.parametros_criterio_parada.tipo_criterio_parada));
                Console.WriteLine(String.Format("Step para obter NFE: {0}", parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs));
                Console.WriteLine(String.Format("NFE limite para execução: {0}", parametros_execucao.parametros_criterio_parada.NFOB_criterio_parada));
                Console.WriteLine("");
            }
            
            // Cria um array com o nome dos algoritmos que foram executados
            string string_algoritmos_executados = "";
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEO ? "GEO;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar ? "GEOvar;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1 ? "AGEO1;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2 ? "AGEO2;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var ? "AGEO1var;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var ? "AGEO2var;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1 ? "GEOreal1;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1 ? "AGEO1real1;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1 ? "AGEO2real1;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2 ? "GEOreal2;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2 ? "AGEO1real2;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2 ? "AGEO2real2;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_1 ? "ASGEO2real1_1;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_2 ? "ASGEO2real1_2;" : "");
            string_algoritmos_executados += (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_3 ? "ASGEO2real1_3;" : "");
            


            
            // Apresenta a média de NFE atingido, a média de f(x) final e o desvio padrão
            // ... para cada um dos algoritmos executados.
            
            
            if (parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Média de NFE, média de f(x) e desvio padrão de f(x):");
                
                string resultados_finais = "";
                resultados_finais = "parameter;" + string_algoritmos_executados + '\n';
                string media_do_NFE_atingido_nas_execucoes = "meanNFE;";
                string media_da_fx_nas_execucoes = "meanFX;";
                string sd_dos_fx_finais_nas_execucoes = "sdFX;";

                // Para cada algoritmo executado da lista de estatísticas dos algoritmos
                for (int i=0; i<estatisticas_algoritmos.Count; i++)
                {
                    
                    int media_NFE = estatisticas_algoritmos[i].NFOB_medio;
                    double media_fx = estatisticas_algoritmos[i].media_melhor_fx;
                    double sd_melhores_fx = estatisticas_algoritmos[i].SD_do_melhor_fx;

                    media_do_NFE_atingido_nas_execucoes += media_NFE.ToString() + ';';
                    media_da_fx_nas_execucoes += media_fx.ToString() + ';';
                    sd_dos_fx_finais_nas_execucoes += sd_melhores_fx.ToString() + ';';
                }

                // Substitui os pontos por vírgulas e printa
                resultados_finais += media_do_NFE_atingido_nas_execucoes.Replace('.',',') + '\n';
                resultados_finais += media_da_fx_nas_execucoes.Replace('.',',') + '\n';
                resultados_finais += sd_dos_fx_finais_nas_execucoes.Replace('.',',') + '\n';
                
                Console.WriteLine(resultados_finais);
            }


            if (o_que_interessa_printar.mostrar_melhores_NFOB)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Médias para cada NFOB:");
                Console.WriteLine("NFOB;" + string_algoritmos_executados);
                
                // Apresenta a média para cada NFOB
                int NFOB_atual = parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs;
                
                int quantidade_NFOBs = estatisticas_algoritmos[0].media_valor_FO_em_cada_NFOB.Count;
                
                // Para cada execução dos algoritmos
                for (int i=0; i<quantidade_NFOBs; i++)
                {    
                    string fxs_naquele_NFOB = NFOB_atual.ToString() + ';';
                    
                    // Para cada algoritmo executado da lista de estatísticas dos algoritmos
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        // Obtém o valor da média f(x) naquele NFOB
                        double fx_naquele_NFOB = estatisticas_algoritmos[j].media_valor_FO_em_cada_NFOB[i];

                        fxs_naquele_NFOB += fx_naquele_NFOB.ToString() + ';';
                    }

                    Console.WriteLine( fxs_naquele_NFOB.Replace('.',',') );

                    NFOB_atual += parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs;
                }
                
                Console.WriteLine("");
            }


            if (o_que_interessa_printar.mostrar_melhores_fx_cada_execucao)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Melhores f(x) para cada execução:");
                Console.WriteLine("execucao;" + string_algoritmos_executados);
                
                // Para cada execução dos algoritmos
                for (int i=0; i<parametros_execucao.quantidade_execucoes; i++)
                {    
                    string melhores_fx_algoritmos = (i+1).ToString() + ';';
                    
                    // Para cada algoritmo executado da lista de estatísticas dos algoritmos
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        // Obtém o valor da média f(x) naquele NFOB
                        double melhor_fx_da_execucao = estatisticas_algoritmos[j].lista_melhores_fxs[i];

                        melhores_fx_algoritmos += melhor_fx_da_execucao.ToString() + ';';
                    }

                    Console.WriteLine( melhores_fx_algoritmos.Replace('.',',') );
                }

                Console.WriteLine("");
            }
        }


        public List<RetornoGEOs> executa_algoritmos_n_vezes(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema)
        {    
            // Lista armazenará o retorno de todas as execuções
            List<RetornoGEOs> todas_execucoes = new List<RetornoGEOs>();

            // Executa os algoritmos por N vezes
            Console.WriteLine("");
            for(int i=0; i<parametros_execucao.quantidade_execucoes; i++)
            {
                Console.Write((i+1) +"...");

                int seed = i;

                // Para cada execução, gera uma nova população inicial com base nos limites de cada variável
                parametros_problema.populacao_inicial = new List<double>(geracao_populacao_real(parametros_problema.lower_bounds, parametros_problema.upper_bounds, seed));

                // Console.WriteLine("População inicial GERADA: {0}", populacao_real_em_string(parametros_problema.populacao_inicial) );
            
                // GEO
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEO)
                {
                    // Console.Write("\nGEO...");

                    GEO_BINARIO geo = new GEO_BINARIO(
                        parametros_problema.parametros_livres.tau_GEO, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can;

                    todas_execucoes.Add(ret);
                }

                // GEOvar
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar)
                {
                    // Console.Write("\nGEOvar...");

                    GEOvar_BINARIO geo_var = new GEOvar_BINARIO(
                        parametros_problema.parametros_livres.tau_GEOvar, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var;
                    
                    todas_execucoes.Add(ret);
                }

                // AGEO1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1)
                {
                    // Console.Write("\nAGEO1...");

                    AGEOs_BINARIO ageo1 = new AGEOs_BINARIO(
                        1, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2)
                {
                    // Console.Write("\nAGEO2...");
                    
                    AGEOs_BINARIO ageo2 = new AGEOs_BINARIO(
                        2, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var)
                {
                    // Console.Write("\nAGEO1var...");
                    
                    AGEOsvar_BINARIO ageo1_var = new AGEOsvar_BINARIO(
                        1, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var)
                {
                    // Console.Write("\nAGEO2var...");

                    AGEOsvar_BINARIO ageo2_var = new AGEOsvar_BINARIO(
                        2, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2_var.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1)
                {
                    // Console.Write("\nGEOreal1...");

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_execucao.tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_GEOreal1, 
                        parametros_problema.parametros_livres.std_GEOreal1);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1;

                    todas_execucoes.Add(ret);
                }

               
                // AGEO1real1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1)
                {
                    // Console.Write("\nAGEO1real1...");

                    AGEOs_REAL1 AGEO1real1 = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.parametros_livres.std_AGEO1real1, 
                        1, 
                        parametros_execucao.tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO1real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real1;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1)
                {
                    // Console.Write("\nAGEO2real1...");

                    AGEOs_REAL1 AGEO2real1 = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.parametros_livres.tau_minimo_AGEOs, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.parametros_livres.std_AGEO2real1, 
                        2, 
                        parametros_execucao.tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO2real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real1;
                        
                    todas_execucoes.Add(ret);
                }

                 // GEOreal2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2)
                {
                    // Console.Write("\nGEOreal2...");

                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.parametros_livres.tau_GEOreal2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.parametros_livres.std_GEOreal2, 
                        parametros_execucao.tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.P_GEOreal2, 
                        (int)parametros_problema.parametros_livres.s_GEOreal2);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1real2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2)
                {
                    // Console.Write("\nAGEO1real2...");

                    AGEOs_REAL2 AGEO1real2 = new AGEOs_REAL2(
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.parametros_livres.std_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.P_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.s_AGEO1real2,
                        parametros_execucao.tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO1real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real2;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2)
                {
                    // Console.Write("\nAGEO2real2...");

                    AGEOs_REAL2 AGEO2real2 = new AGEOs_REAL2(
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs, 
                        parametros_problema.parametros_livres.std_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.P_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.s_AGEO1real2,
                        parametros_execucao.tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO2real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2;

                    todas_execucoes.Add(ret);
                }

                
                
                // ASGEO2real1_1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_1)
                {
                    // Console.Write("\rodar_ASGEO2real1_1...");

                    ASGEO2_REAL1_1 ASGEO2real1_1 = new ASGEO2_REAL1_1(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs,
                        parametros_execucao.tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_1, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_1);
                        
                    RetornoGEOs ret = ASGEO2real1_1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_1;
                        
                    todas_execucoes.Add(ret);
                }
                // ASGEO2real1_2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_2)
                {
                    // Console.Write("\rodar_ASGEO2real1_2...");

                    ASGEO2_REAL1_2 ASGEO2real1_2 = new ASGEO2_REAL1_2(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs,
                        parametros_execucao.tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_2, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_2);
                        
                    RetornoGEOs ret = ASGEO2real1_2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_2;
                        
                    todas_execucoes.Add(ret);
                }
                // ASGEO2real1_3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_3)
                {
                    // Console.Write("\rodar_ASGEO2real1_3...");

                    ASGEO2_REAL1_3 ASGEO2real1_3 = new ASGEO2_REAL1_3(
                        parametros_problema.populacao_inicial, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.step_para_obter_NFOBs,
                        parametros_execucao.tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_3, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_3);
                        
                    RetornoGEOs ret = ASGEO2real1_3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_3;
                        
                    todas_execucoes.Add(ret);
                }
            }

            // Retorna a lista com todas as N execuções para os algoritmos a serem executados
            return todas_execucoes;
        }
            
        // =====================================
        // FUNÇÕES CONHECIDAS
        // =====================================

        public void Execucoes()
        {
            // Cria uma lista contendo as funções a serem executadas
            List<int> function_values = new List<int>()
            {
                // (int)EnumNomesFuncoesObjetivo.griewangk,
                // (int)EnumNomesFuncoesObjetivo.rastringin,
                // (int)EnumNomesFuncoesObjetivo.rosenbrock,
                // (int)EnumNomesFuncoesObjetivo.schwefel,
                // (int)EnumNomesFuncoesObjetivo.ackley,
                (int)EnumNomesFuncoesObjetivo.beale,
                // (int)EnumNomesFuncoesObjetivo.levy13,
                // (int)EnumNomesFuncoesObjetivo.nova,
                // (int)EnumNomesFuncoesObjetivo.spacecraft,
            };

            // Para cada função, executa os algoritmos
            foreach (int definicao_funcao_objetivo in function_values)
            {    
                // ======================================================================
                // DEFINE OS PARÂMETROS DO PROBLEMA
                // ======================================================================

                // Define os parâmetros do problema conforme cada função
                ParametrosProblema parametros_problema;

                switch(definicao_funcao_objetivo)
                {
                    // GRIEWANGK
                    case (int)EnumNomesFuncoesObjetivo.griewangk:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Griewangk";
                        parametros_problema.n_variaveis_projeto = 10;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-600.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(600.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 1.25,
                            tau_GEOvar = 2.75,
                            
                            tau_GEOreal1 = 1.5,
                            std_GEOreal1 = 0.8,
                            
                            tau_GEOreal2 = 3,
                            std_GEOreal2 = 1,
                            P_GEOreal2 = 4,
                            s_GEOreal2 = 1,
                            
                            std_AGEO1real1 = 0.8,
                            std_AGEO2real1 = 1.2,
                            
                            std_AGEO1real2 = 1,
                            P_AGEO1real2 = 4,
                            s_AGEO1real2 = 1,
                            
                            std_AGEO2real2 = 1,
                            P_AGEO2real2 = 4,
                            s_AGEO2real2 = 1,
                            
                            tau_minimo_AGEOs = 0.5,

                            tau_ASGEO2_REAL1_1 = 1.5,
                            std_ASGEO2_REAL1_1 = 1.2,
                            tau_ASGEO2_REAL1_2 = 1.5,
                            std_ASGEO2_REAL1_2 = 1.2,
                            tau_ASGEO2_REAL1_3 = 1.5,
                            std_ASGEO2_REAL1_3 = 1.2,
                        };
                    break;

                    // RASTRINGIN
                    case (int)EnumNomesFuncoesObjetivo.rastringin:
                        int n_variaveis = 20;

                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Rastringin";
                        parametros_problema.n_variaveis_projeto = n_variaveis;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 1,
                            tau_GEOvar = 1.75,
                            
                            tau_GEOreal1 = 1.5,
                            std_GEOreal1 = 1.2,
                            
                            tau_GEOreal2 = 4,
                            std_GEOreal2 = 1,
                            P_GEOreal2 = 4,
                            s_GEOreal2 = 1,
                            
                            std_AGEO1real1 = 0.8,
                            std_AGEO2real1 = 1,
                            
                            std_AGEO1real2 = 1,
                            P_AGEO1real2 = 4,
                            s_AGEO1real2 = 1,
                            
                            std_AGEO2real2 = 1,
                            P_AGEO2real2 = 4,
                            s_AGEO2real2 = 1,
                            
                            tau_minimo_AGEOs = 0.5,

                            tau_ASGEO2_REAL1_1 = 1.5,
                            std_ASGEO2_REAL1_1 = 1,
                            tau_ASGEO2_REAL1_2 = 1.5,
                            std_ASGEO2_REAL1_2 = 1,
                            tau_ASGEO2_REAL1_3 = 1.5,
                            std_ASGEO2_REAL1_3 = 1,
                        };
                    break;

                    // ROSENBROCK
                    case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Rosenbrock";
                        parametros_problema.n_variaveis_projeto = 2;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-2.048, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(2.048, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 1.25,
                            tau_GEOvar = 1.5,
                            
                            tau_GEOreal1 = 4,
                            std_GEOreal1 = 2.2,
                            
                            tau_GEOreal2 = 5,
                            std_GEOreal2 = 1,
                            P_GEOreal2 = 4,
                            s_GEOreal2 = 2,
                            
                            std_AGEO1real1 = 1.6,
                            std_AGEO2real1 = 1.8,
                            
                            std_AGEO1real2 = 2,
                            P_AGEO1real2 = 8,
                            s_AGEO1real2 = 1,
                            
                            std_AGEO2real2 = 2,
                            P_AGEO2real2 = 4,
                            s_AGEO2real2 = 2,
                            
                            tau_minimo_AGEOs = 0.5,

                            tau_ASGEO2_REAL1_1 = 4,
                            std_ASGEO2_REAL1_1 = 1.8,
                            tau_ASGEO2_REAL1_2 = 4,
                            std_ASGEO2_REAL1_2 = 1.8,
                            tau_ASGEO2_REAL1_3 = 4,
                            std_ASGEO2_REAL1_3 = 1.8,
                        };
                    break;

                    // SCHWEFEL
                    case (int)EnumNomesFuncoesObjetivo.schwefel:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Schwefel";
                        parametros_problema.n_variaveis_projeto = 10;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 0.75,
                            tau_GEOvar = 1.5,
                            
                            tau_GEOreal1 = 5,
                            std_GEOreal1 = 1.8,
                            
                            tau_GEOreal2 = 5,
                            std_GEOreal2 = 4,
                            P_GEOreal2 = 16,
                            s_GEOreal2 = 2,
                            
                            std_AGEO1real1 = 1.8,
                            std_AGEO2real1 = 2.4,
                            
                            std_AGEO1real2 = 2,
                            P_AGEO1real2 = 16,
                            s_AGEO1real2 = 1,
                            
                            std_AGEO2real2 = 8,
                            P_AGEO2real2 = 16,
                            s_AGEO2real2 = 1,
                            
                            tau_minimo_AGEOs = 0.5,

                            tau_ASGEO2_REAL1_1 = 5,
                            std_ASGEO2_REAL1_1 = 2.4,
                            tau_ASGEO2_REAL1_2 = 5,
                            std_ASGEO2_REAL1_2 = 2.4,
                            tau_ASGEO2_REAL1_3 = 5,
                            std_ASGEO2_REAL1_3 = 2.4
                        };
                    break;

                    // ACKLEY
                    case (int)EnumNomesFuncoesObjetivo.ackley:

                        int n_variaveis_projeto = 30;

                        parametros_problema = new ParametrosProblema()
                        {
                            nome_funcao = "Ackley",
                            n_variaveis_projeto = n_variaveis_projeto,
                            definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.ackley,
                            bits_por_variavel = Enumerable.Repeat(16, n_variaveis_projeto).ToList(),
                            lower_bounds = Enumerable.Repeat(-30.0, n_variaveis_projeto).ToList(),
                            upper_bounds = Enumerable.Repeat(30.0, n_variaveis_projeto).ToList(),
                            parametros_livres = new ParametrosLivreProblema()
                            {
                                tau_GEO = 3.25,
                                tau_GEOvar = 2.25,
                                
                                tau_GEOreal1 = 1,
                                std_GEOreal1 = 0.8,
                                
                                tau_GEOreal2 = 5,
                                std_GEOreal2 = 1,
                                P_GEOreal2 = 4,
                                s_GEOreal2 = 1,
                                
                                std_AGEO1real1 = 0.8,
                                std_AGEO2real1 = 0.8,
                                
                                std_AGEO1real2 = 1,
                                P_AGEO1real2 = 4,
                                s_AGEO1real2 = 1,
                                
                                std_AGEO2real2 = 1,
                                P_AGEO2real2 = 4,
                                s_AGEO2real2 = 1,
                                
                                tau_minimo_AGEOs = 0.5,

                                tau_ASGEO2_REAL1_1 = 1,
                                std_ASGEO2_REAL1_1 = 0.8,
                                tau_ASGEO2_REAL1_2 = 1,
                                std_ASGEO2_REAL1_2 = 0.8,
                                tau_ASGEO2_REAL1_3 = 1,
                                std_ASGEO2_REAL1_3 = 0.8,
                            }
                        };
                    break;

                    // LEVY13
                    case (int)EnumNomesFuncoesObjetivo.levy13:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Levy13";
                        parametros_problema.n_variaveis_projeto = 2;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            // // tau_GEO = 1.25,
                            // // tau_GEOvar = 1.5,
                            
                            // tau_GEOreal1 = 5,
                            // std_GEOreal1 = 2,
                            
                            // // tau_GEOreal2 = 5,
                            // // std_GEOreal2 = 1,
                            // // P_GEOreal2 = 4,
                            // // s_GEOreal2 = 2,
                            
                            // std_AGEO1real1 = 1.8,
                            // std_AGEO2real1 = 1.8,
                            
                            // // std_AGEO1real2 = 2,
                            // // P_AGEO1real2 = 8,
                            // // s_AGEO1real2 = 1,
                            
                            // // std_AGEO2real2 = 2,
                            // // P_AGEO2real2 = 4,
                            // // s_AGEO2real2 = 2,
                            
                            // tau_minimo_AGEOs = 0.5,

                            // tau_ASGEO2_REAL1_1 = 5,
                            // std_ASGEO2_REAL1_1 = 2,
                            // tau_ASGEO2_REAL1_2 = 5,
                            // std_ASGEO2_REAL1_2 = 2,
                            // tau_ASGEO2_REAL1_3 = 5,
                            // std_ASGEO2_REAL1_3 = 2,
                        };
                    break;

                    // BEALE
                    case (int)EnumNomesFuncoesObjetivo.beale:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "Beale";
                        parametros_problema.n_variaveis_projeto = 2;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-4.5, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(4.5, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            // tau_GEO = 1.25,
                            // tau_GEOvar = 1.5,
                            
                            tau_GEOreal1 = 5,
                            std_GEOreal1 = 2,
                            
                            // tau_GEOreal2 = 5,
                            // std_GEOreal2 = 1,
                            // P_GEOreal2 = 4,
                            // s_GEOreal2 = 2,
                            
                            std_AGEO1real1 = 1.8,
                            std_AGEO2real1 = 1.8,
                            
                            // std_AGEO1real2 = 2,
                            // P_AGEO1real2 = 8,
                            // s_AGEO1real2 = 1,
                            
                            // std_AGEO2real2 = 2,
                            // P_AGEO2real2 = 4,
                            // s_AGEO2real2 = 2,
                            
                            tau_minimo_AGEOs = 0.5,

                            tau_ASGEO2_REAL1_1 = 5,
                            std_ASGEO2_REAL1_1 = 2,
                            tau_ASGEO2_REAL1_2 = 5,
                            std_ASGEO2_REAL1_2 = 2,
                            tau_ASGEO2_REAL1_3 = 5,
                            std_ASGEO2_REAL1_3 = 2,
                        };
                    break;

                    // F9 TESE
                    case (int)EnumNomesFuncoesObjetivo.F09:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "F9";
                        parametros_problema.n_variaveis_projeto = 2;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 1.50,
                            tau_GEOvar = 1.75,
                            tau_GEOreal1 = 1.50,
                            tau_GEOreal2 = 1.75,
                            tau_minimo_AGEOs = 0.5,
                            std_GEOreal1 = 1.0,
                            std_AGEO1real1 = 1.0,
                            std_AGEO2real1 = 1.0,
                            std_GEOreal2 = 1.0,
                            std_AGEO1real2 = 1.0,
                            std_AGEO2real2 = 1.0,
                            P_GEOreal2 = 8,
                            P_AGEO1real2 = 8,
                            P_AGEO2real2 = 8,
                            s_GEOreal2 = 2,
                            s_AGEO1real2 = 2,
                            s_AGEO2real2 = 2
                        };
                    break;

                    // DEJONG3
                    case (int)EnumNomesFuncoesObjetivo.dejong3:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "DeJong#3";
                        parametros_problema.n_variaveis_projeto = 5;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 3.0,
                            tau_GEOvar = 8.0,
                            tau_GEOreal1 = 3.0,
                            tau_GEOreal2 = 8.0,
                            tau_minimo_AGEOs = 0.5,
                            std_GEOreal1 = 1.0,
                            std_AGEO1real1 = 1.0,
                            std_AGEO2real1 = 1.0,
                            std_GEOreal2 = 1.0,
                            std_AGEO1real2 = 1.0,
                            std_AGEO2real2 = 1.0,
                            P_GEOreal2 = 8,
                            P_AGEO1real2 = 8,
                            P_AGEO2real2 = 8,
                            s_GEOreal2 = 2,
                            s_AGEO1real2 = 2,
                            s_AGEO2real2 = 2
                        };
                    break;

                    // SPACECRAFT
                    case (int)EnumNomesFuncoesObjetivo.spacecraft:
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "SPACECRAFT";
                        parametros_problema.n_variaveis_projeto = 3;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = new List<int>(){2,6,6};
                        parametros_problema.lower_bounds = new List<double>(){13,1,0};
                        parametros_problema.upper_bounds = new List<double>(){15,60,59};
                        parametros_problema.parametros_livres = new ParametrosLivreProblema()
                        {
                            tau_GEO = 3.0,
                            tau_GEOvar = 8.0,

                            tau_GEOreal1 = 1.0,
                            tau_GEOreal2 = 1.0,
                            tau_minimo_AGEOs = 0.5,
                            std_GEOreal1 = 1.0,
                            std_AGEO1real1 = 1.0,
                            std_AGEO2real1 = 1.0,
                            std_GEOreal2 = 1.0,
                            std_AGEO1real2 = 1.0,
                            std_AGEO2real2 = 1.0,
                            P_GEOreal2 = 8,
                            P_AGEO1real2 = 8,
                            P_AGEO2real2 = 8,
                            s_GEOreal2 = 2,
                            s_AGEO1real2 = 2,
                            s_AGEO2real2 = 2
                        };
                    break;

                    // DEFAULT
                    default:
                        Console.WriteLine("DEFAULT PARAMETERS!");
                        parametros_problema = new ParametrosProblema();
                        parametros_problema.nome_funcao = "DEFAULT";
                        parametros_problema.n_variaveis_projeto = 0;
                        parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                        parametros_problema.bits_por_variavel = new List<int>();
                        parametros_problema.lower_bounds = new List<double>();
                        parametros_problema.upper_bounds = new List<double>();
                        parametros_problema.parametros_livres = new ParametrosLivreProblema();
                        parametros_problema.populacao_inicial = new List<double>();
                    break;
                }

                
                // ======================================================================
                // DEFINE OS PARÂMETROS DA EXECUÇÃO
                // ======================================================================

                ParametrosExecucao parametros_execucao = new ParametrosExecucao()
                {
                    quantidade_execucoes = 40,
                    parametros_criterio_parada = new ParametrosCriterioParada()
                    {
                        // EXECUÇÃO NORMAL
                        tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB,
                        PRECISAO_criterio_parada = 0,
                        step_para_obter_NFOBs = 500,
                        NFOB_criterio_parada = 100000,
                        fx_esperado = 0

                        // // TUNING FUNÇÕES TESTE
                        // tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB,
                        // PRECISAO_criterio_parada = 1e-16,
                        // step_para_obter_NFOBs = 500,
                        // NFOB_criterio_parada = 100000,
                        // fx_esperado = 0.0

                        // // TUNING SPACECRAFT
                        // tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB,
                        // PRECISAO_criterio_parada = 2e-13,
                        // step_para_obter_NFOBs = 40000,
                        // NFOB_criterio_parada = 50000,
                        // fx_esperado = 196.949433192159
                    },
                    quais_algoritmos_rodar = new QuaisAlgoritmosRodar()
                    {
                        rodar_GEO           = false,
                        rodar_GEOvar        = false,
                        rodar_AGEO1         = false,
                        rodar_AGEO2         = false,
                        rodar_AGEO1var      = false,
                        rodar_AGEO2var      = false,
                        rodar_GEOreal1      = false,
                        rodar_AGEO1real1    = true,
                        rodar_AGEO2real1    = true,
                        rodar_GEOreal2      = false,
                        rodar_AGEO1real2    = false,
                        rodar_AGEO2real2    = false,
                        rodar_ASGEO2real1_1 = false,
                        rodar_ASGEO2real1_2 = false,
                        rodar_ASGEO2real1_3 = false,
                    },
                    o_que_interessa_printar = new OQueInteressaPrintar()
                    {
                        // EXECUÇÃO NORMAL
                        mostrar_header = false,
                        mostrar_meanNFE_meanFX_sdFX = true,
                        mostrar_melhores_NFOB = false,
                        mostrar_melhores_fx_cada_execucao = false

                        // // TUNING
                        // mostrar_header = false,
                        // mostrar_meanNFE_meanFX_sdFX = true,
                        // mostrar_melhores_NFOB = false,
                        // mostrar_melhores_fx_cada_execucao = false
                    },
                    tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_original
                };


               



                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");
                
                // // parametros_execucao.o_que_interessa_printar.mostrar_header = false;
                // // parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX = true;
                // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFOB = true;
                // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;
                
                // // Executa cada algoritmo por N vezes e obtém todas as execuções
                // List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                
                // // Organiza os resultados de todas as excuções por algoritmo
                // List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                
                // // Apresenta os resultados finais
                // apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);






                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");
                
                // parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;

                // switch(definicao_funcao_objetivo){
                //     case (int)EnumNomesFuncoesObjetivo.griewangk:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 0.2;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 0.2;
                //     break;
                //     case (int)EnumNomesFuncoesObjetivo.rastringin:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 5.2;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 4.8;
                //     break;
                //     case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 0.4;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 0.2;
                //     break;
                //     case (int)EnumNomesFuncoesObjetivo.schwefel:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 5;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 8.4;
                //     break;
                //     case (int)EnumNomesFuncoesObjetivo.ackley:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 3.6;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 1;
                //     break;
                //     case (int)EnumNomesFuncoesObjetivo.beale:
                //         parametros_problema.parametros_livres.std_AGEO1real1 = 1;
                //         parametros_problema.parametros_livres.std_AGEO2real1 = 1;
                //     break;
                // }

                // // parametros_execucao.o_que_interessa_printar.mostrar_header = false;
                // // parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX = true;
                // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFOB = true;
                // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;
               
                // // Executa cada algoritmo por N vezes e obtém todas as execuções
                // List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                
                // // Organiza os resultados de todas as excuções por algoritmo
                // List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                
                // // Apresenta os resultados finais
                // apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);

                
                
                







                // // ====================================================================================
                // // Tuning do GEOreal1 - parâmetros tau e std
                // // ====================================================================================
                
                // // Executa o algoritmo variando o std
                // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};
                // // List<double> valores_tau = new List<double>(){0.25,0.5,0.75,1,1.25,1.5,1.75,2,2.25,2.5,2.75,3,3.25,3.5,3.75,4,4.25,4.5,4.75,5,5.25,5.5,5.75,6};
                // List<double> valores_tau = new List<double>(){0.5,1,1.5,2,2.5,3,3.5,4,4.5,5};


                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // // Itera cada std e cada tau
                // foreach (double std in valores_std){
                //     foreach (double tau in valores_tau){
                //         parametros_problema.parametros_livres.std_GEOreal1 = std; 
                //         parametros_problema.parametros_livres.tau_GEOreal1 = tau; 

                //         Console.WriteLine("========================================================");
                //         Console.WriteLine("std = {0} | tau = {1}", std, tau);

                //         // Executa cada algoritmo por N vezes e obtém todas as execuções
                //         List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                        
                //         // Organiza os resultados de todas as excuções por algoritmo
                //         List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                        
                //         // Apresenta os resultados finais
                //         apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                //     }
                // }
                // // ====================================================================================







                // // ====================================================================================
                // // Tuning do std para os A-GEOsreal1
                // // ====================================================================================
                
                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // // Executa o algoritmo variando o std
                // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};
                // foreach (double std in valores_std){

                //     Console.WriteLine("========================================================");
                //     Console.WriteLine("std = {0}", std);

                //     // Define o valor de std
                //     parametros_problema.parametros_livres.std_AGEO1real1 = std;
                //     parametros_problema.parametros_livres.std_AGEO2real1 = std;

                //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                //     // Organiza os resultados de todas as excuções por algoritmo
                //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                //     // Apresenta os resultados finais
                //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                // }
                // // ====================================================================================






                // ====================================================================================
                // Tuning do std para PERTURBAÇÃO DIRETA os A-GEOs real1
                // ====================================================================================

                Console.WriteLine("============================================================================");
                Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                Console.WriteLine("============================================================================");
                
                parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;

                // Executa o algoritmo variando a porcentagem do intervalo
                // List<double> valores_porcentagem = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
                // List<double> valores_porcentagem = new List<double>(){1.0,1.5,2.0,2.5,3.0,3.5,4.0,4.5,5.0,5.5,6.0,6.5,7.0,7.5,8.0,8.5,9.0,9.5,10.0,10.5,11.0,11.5,12.0,12.5,13.0,13.5,14.0,14.5,15.0};
                // List<double> valores_porcentagem = new List<double>(){1,2,3,4,5,6,7,8,9,10};
                // List<double> valores_porcentagem = new List<double>(){0.5,1,1.5,2,2.5,3,3.5,4,4.5,5,6,7,8,9,10};
                List<double> valores_porcentagem = new List<double>(){0.2,0.4,0.6,0.8,1.0,1.2,1.4,1.6,1.8,2.0,2.2,2.4,2.6,2.8,3.0,3.2,3.4,3.6,3.8,4.0,4.2,4.4,4.6,4.8,5.0};
                // List<double> valores_porcentagem = new List<double>(){5.2,5.4,5.6,5.8,6.0,6.2,6.4,6.6,6.8,7.0,7.2,7.4,7.6,7.8,8.0,8.2,8.4,8.6,8.8,9.0,9.2,9.4,9.6,9.8,10.0};
                foreach (double porcentagem in valores_porcentagem){
                    // Muda o tipo de perturbação
                    
                    // O std será a porcentam do intervalo de variação das variáveis
                    double inf_limit = parametros_problema.lower_bounds[0];
                    double sup_limit = parametros_problema.upper_bounds[0];
                    double total_intervalo_variacao_variaveis = Math.Abs(sup_limit - inf_limit);
                    double std = (porcentagem/100.0) * total_intervalo_variacao_variaveis;

                    Console.WriteLine("========================================================");
                    Console.WriteLine("std | porcentagem = {0};{1}", std, porcentagem);

                    // Define o valor de std
                    parametros_problema.parametros_livres.std_AGEO1real1 = porcentagem;
                    parametros_problema.parametros_livres.std_AGEO2real1 = porcentagem;

                    // Executa cada algoritmo por N vezes e obtém todas as execuções
                    List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // Organiza os resultados de todas as excuções por algoritmo
                    List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                    // Apresenta os resultados finais
                    apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                }
                // ====================================================================================


               



                // // ====================================================================================
                // // Tuning variando só std
                // // ====================================================================================
                
                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // // Executa o algoritmo variando o std
                // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};
                // foreach (double std in valores_std){

                //     Console.WriteLine("========================================================");
                //     Console.WriteLine("std = {0}", std);

                //     // Define o valor de std
                //     parametros_problema.parametros_livres.std_GEOreal1 = std;
                //     parametros_problema.parametros_livres.std_AGEO1real1 = std;
                //     parametros_problema.parametros_livres.std_AGEO2real1 = std;
                //     parametros_problema.parametros_livres.std_GEOreal2 = std;
                //     parametros_problema.parametros_livres.std_AGEO1real2 = std;
                //     parametros_problema.parametros_livres.std_AGEO2real2 = std;

                //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                //     // Organiza os resultados de todas as excuções por algoritmo
                //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                //     // Apresenta os resultados finais
                //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                // }
                // // ====================================================================================





                // // ====================================================================================
                // // Tuning do std para os A-GEOsreal2
                // // ====================================================================================

                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // List<int> valores_s = new List<int>(){1, 2};
                // List<int> valores_P = new List<int>(){4, 8, 16};
                // List<double> valores_std1 = new List<double>(){1, 2, 4, 8};
                // // Realiza todas as combinações possíveis
                // foreach (int P in valores_P){
                //     foreach (int s in valores_s){
                //         foreach (double std in valores_std1){
                //             parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                //                 std_AGEO1real2 = std,
                //                 std_AGEO2real2 = std,
                //                 P_AGEO1real2 = P,
                //                 P_AGEO2real2 = P,
                //                 s_AGEO1real2 = s,
                //                 s_AGEO2real2 = s,
                //             };

                //             Console.WriteLine("=====================================================");
                //             Console.WriteLine("=====================================================");
                //             Console.WriteLine("P = {0} | s = {1} | std1 = {2}", P, s, std);
                //             Console.WriteLine("=====================================================");

                //             // Executa cada algoritmo por N vezes e obtém todas as execuções
                //             List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                //             // Organiza os resultados de todas as excuções por algoritmo
                //             List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                            
                //             // Apresenta os resultados finais
                //             apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                //         }
                //     }
                // }
                // // ====================================================================================




                // // ====================================================================================
                // // Tuning do tau nos binários
                // // ====================================================================================

                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // // Executa o algoritmo variando o std
                // // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75, 3.0, 3.25, 3.5, 3.75, 4.0, 4.25, 4.5, 4.75, 5.0, 5.25, 5.5, 5.75, 6.0, 6.25, 6.5, 6.75, 7.0, 7.25, 7.5, 7.75, 8, 8.25, 8.5, 8.75, 9}; 
                // // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0, 5.5, 6.0, 6.5, 7.0, 7.5, 8}; 
                // // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3}; 
                // // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4}; 
                // // List<double> valores_tau = new List<double>(){4.0, 4.25, 4.5, 4.75, 5, 5.25, 5.5, 5.75, 6.0, 6.25, 6.5, 6.75, 7.0}; 
                
                // // SPACECRAFT
                // List<double> valores_tau = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5};
                
                // foreach (double tau in valores_tau){
                //     // Atualiza o valor de tau
                //     parametros_problema.parametros_livres.tau_GEO = tau;
                //     parametros_problema.parametros_livres.tau_GEOvar = tau;

                //     Console.WriteLine("========================================================");
                //     Console.WriteLine("tau GEO e GEOvar: {0}", tau);

                //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                //     // Organiza os resultados de todas as excuções por algoritmo
                //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                //     // Apresenta os resultados finais
                //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                // }
                // // ====================================================================================
                
                
                
                
                
               

                // // ====================================================================================
                // // Varia a porcentagem do intervalo
                // // ====================================================================================

                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // // Executa o algoritmo variando a porcentagem do intervalo
                // List<double> valores_porcentagem = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
                // foreach (double porcentagem in valores_porcentagem){
                //     // Muda o tipo de perturbação
                //     parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;
                    
                //     // O std será a porcentam do intervalo de variação das variáveis
                //     double inf_limit = parametros_problema.lower_bounds[0];
                //     double sup_limit = parametros_problema.upper_bounds[0];
                //     double total_intervalo_variacao_variaveis = Math.Abs(sup_limit - inf_limit);
                //     double std = (porcentagem/100.0) * total_intervalo_variacao_variaveis;

                //     // Atualiza os parâmetros do problema com esse std gerado
                //     parametros_problema.parametros_livres.std_GEOreal1   = std; 
                //     parametros_problema.parametros_livres.std_GEOreal2   = std; 

                //     Console.WriteLine("========================================================");
                //     Console.WriteLine("std;porcentagem");
                //     Console.WriteLine( String.Format("{0};{1}", std, porcentagem).Replace('.',',') );

                //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                //     // Organiza os resultados de todas as excuções por algoritmo
                //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                //     // Apresenta os resultados finais
                //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                // }
                // // ====================================================================================






                // // ====================================================================================
                // // Tuning completo do GEOreal2
                // // ====================================================================================

                // Console.WriteLine("============================================================================");
                // Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                // Console.WriteLine("============================================================================");

                // List<int> valores_P = new List<int>(){4, 8, 16};
                // List<int> valores_s = new List<int>(){1, 2};
                // List<double> valores_std1 = new List<double>(){1, 2, 4, 8};
                // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                // // Realiza todas as combinações possíveis
                // foreach (int P in valores_P){
                //     foreach (int s in valores_s){
                //         foreach (double std in valores_std1){
                //             foreach (double tau in valores_tau){
                //                 parametros_problema.parametros_livres = new ParametrosLivreProblema(){
                //                     tau_GEOreal2 = tau,
                //                     std_GEOreal2 = std,
                //                     P_GEOreal2 = P,
                //                     s_GEOreal2 = s,
                //                 };

                //                 Console.WriteLine("=====================================================");
                //                 Console.WriteLine("=====================================================");
                //                 Console.WriteLine("P = {0} | s = {1} | std1 = {2} | tau = {3}", P, s, std, tau);
                //                 Console.WriteLine("=====================================================");

                //                 // Executa cada algoritmo por N vezes e obtém todas as execuções
                //                 List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                                
                //                 // Organiza os resultados de todas as excuções por algoritmo
                //                 List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                                
                //                 // Apresenta os resultados finais
                //                 apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                //             }
                //         }
                //     }
                // }



            }



            // // =========================================
            // // VARIANDO STD - PERTURBAÇÃO NOVA
            // // =========================================
            
            // // Altera o tipo de perturbação
            // parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;

            // // Lista contendo as porcentagens do intervalo a serem o valor de std
            // List<double> valores_porcentagem_do_total_p_perturbar = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
            
            // // Para cada porcentagem, calcula o std e executa os algoritmos
            // for(int n=0; n<valores_porcentagem_do_total_p_perturbar.Count; n++){
            //     double porcentagem = valores_porcentagem_do_total_p_perturbar[n];

            //     // O std será a porcentam do intervalo de variação das variáveis
            //     double inf_limit = parametros_problema.lower_bounds[0];
            //     double sup_limit = parametros_problema.upper_bounds[0];
            //     double total_intervalo_variacao_variaveis = Math.Abs(sup_limit - inf_limit);
            //     double std = (porcentagem/100.0) * total_intervalo_variacao_variaveis;

            //     // Atualiza os parâmetros do problema com esse std gerado
            //     parametros_problema.parametros_livres.std_GEOreal1   = std; 
            //     parametros_problema.parametros_livres.std_AGEO1real1 = std; 
            //     parametros_problema.parametros_livres.std_AGEO2real1 = std; 
            //     parametros_problema.parametros_livres.std_GEOreal2   = std; 
            //     parametros_problema.parametros_livres.std_AGEO1real2 = std; 
            //     parametros_problema.parametros_livres.std_AGEO2real2 = std; 

            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("std;porcentagem");
            //     Console.WriteLine( String.Format("{0};{1}", std, porcentagem).Replace('.',','));

            //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);

            //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, todas_execucoes_algoritmos, parametros_execucao, parametros_problema);
            // }








            // // =========================================
            // // VARIANDO TAU
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAO;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0.001;
            // const int step_para_obter_NFOBs = 10000;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
          
            
            // foreach(double tau in valores_TAU){
            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("{0}", tau.ToString().Replace('.',','));
            
            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau, tau, tau, 0, tau_minimo_AGEOs, std_GEOreal1, 0, std_AGEO1real1, std_AGEO2real1, 0, 0, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, lower_bounds, upper,bounds, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar, );
            // }

            
            // // =========================================
            // // EXECUÇÕES VARIANDO PORCENTAGEM
            // // =========================================
            // ParametrosCriterioParada parametros_criterio_parada = new ParametrosCriterioParada();
            // parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB;
            // parametros_criterio_parada.PRECISAO_criterio_parada = 0;
            // const int step_para_obter_NFOBs = 250;
            // parametros_criterio_parada.step_para_obter_NFOBs = step_para_obter_NFOBs;
            
          
            // const bool  = true;
            // const int tipo_perturbacao_original_ou_SDdireto = (int)EnumTipoPerturbacao.perturbacao_SDdireto;
            

            // List<double> valores_porcentagem_do_total_p_perturbar = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
            
            // double total_intervalo_variacao_variaveis = (upper_bounds[0] - lower_bounds[0]);
            
            // foreach(double porcentagem in valores_porcentagem_do_total_p_perturbar){
            //     double std = porcentagem/100.0 * total_intervalo_variacao_variaveis;

            //     Console.WriteLine("\n==========================================");
            //     Console.WriteLine("porcentagem;std");
            //     Console.WriteLine( String.Format("{0};{1}", porcentagem, std).Replace('.',',') );

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, lower_bounds, upper_bounds, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar);
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

            //     Executa_Os_Algoritmos_Por_N_Vezes(quantidade_execucoes, tau_GEO, tau_GEOvar, tau_GEOreal1, 0, tau_minimo_AGEOs, std, std, std, std, std, std, 0, 0, 0, tipo_perturbacao_original_ou_SDdireto, parametros_problema, bits_por_variavel_variaveis, lower_bounds, upper_bounds, parametros_criterio_parada, step_para_obter_NFOBs, quais_algoritmos_rodar, o_que_interessa_printar);
            // }
        }

        // =====================================
        // SPACECRAFT OPTIMIZATION
        // =====================================

        public void ExtensiveSearch_SpacecraftOptimization()
        {
            double menor_fx_historia = Double.MaxValue;
            double menor_i_historia = Double.MaxValue;
            double menor_n_historia = Double.MaxValue;
            double menor_d_historia = Double.MaxValue;

            for (int i = 13; i <= 15; i++)
            {
                for (int d = 1; d <= 60; d++)
                {
                    for (int n = 1; n <= d; n++)
                    {
                        
                        // Se o espaço for viável, executa
                        if( (n < d) && ((double)n%d != 0) ) { //&& (Satellite.Payload.FOV >= 1.05*FovMin);
                            
                            // Monta a lista de fenótipos
                            List<double> fenotipo_variaveis_projeto = new List<double>(){i,d,n};
                            
                            // Instancia a spacecraft
                            TesteOptimizer spacecraft_model = new TesteOptimizer(fenotipo_variaveis_projeto);
                            double fx = spacecraft_model.fx_calculada;

                            // Executa diretamente a função objetivo
                            // double fx = SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
                            // Console.WriteLine("Espaço válido! i="+i+"; n="+n+"; d:"+d+"; fx="+fx);

                            // Verifica se essa execução é a melhor da história
                            if (fx < menor_fx_historia)
                            {
                                Console.WriteLine("Atualiza o melhor fx para {0} e restrição nesse é {1}", fx, spacecraft_model.valid_solution);
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

        public void Teste_FuncoesObjetivo_SpacecraftOptimization()
        {    
            // Define qual função chamar e o fenótipo
            int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.spacecraft;
            List<double> fenotipo_variaveis_projeto = new List<double>(){14,60,59};
            
            // =========================================================
            // Calcula a função objetivo com a rotina de FOs
            // =========================================================
            
            double melhor_fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, definicao_funcao_objetivo);

            Console.WriteLine("Melhor fx função switch case: {0}", melhor_fx);


            // =========================================================
            // Calcula a função objetivo diretamente
            // =========================================================

            SpaceDesignTeste.TesteOptimizer spacecraft_model = new SpaceDesignTeste.TesteOptimizer(fenotipo_variaveis_projeto);

            double fx = spacecraft_model.fx_calculada;

            Console.WriteLine("Fx Final Função diretamente: {0}", fx);
        }

    }


    public class GEO {
        // =====================================
        // MAIN
        // =====================================

        public static void Main(string[] args)
        {    
            Console.WriteLine("Rodando!");
            
            // ============================================================================
            // ============================================================================
            // Seta a saída para o arquivo
            // ============================================================================
            // ============================================================================
            #if CONSOLE_OUT_FILE
                string filename = "./Redirect.txt";

                // Deleta o arquivo caso ele exista
                if (File.Exists(filename))  File.Delete(filename);

                // Seta a saída do Console
                FileStream ostrm;
                StreamWriter writer;
                TextWriter oldOut = Console.Out;
                try
                {
                    ostrm = new FileStream (filename, FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter (ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine ("Cannot open Redirect.txt for writing");
                    Console.WriteLine (e.Message);
                    return;
                }
                Console.SetOut (writer);
            #endif
            // ============================================================================
            // ============================================================================





            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // Execuções
            Execucoes_GEO ex = new Execucoes_GEO();
            ex.Execucoes();

            

            // // GEOreal1
            // List<double> lower_bounds = new List<double>(){-10,-10};
            // List<double> upper_bounds = new List<double>(){ 10, 10};
            // double tau = 1;
            // double std = 1;
            // GEO_real1 geo_real1 = new GEO_real1(
            //     2,
            //     (int)EnumNomesFuncoesObjetivo.griewangk,
            //     lower_bounds,
            //     upper_bounds,
            //     Execucoes_GEO.geracao_populacao_real(lower_bounds, upper_bounds, 64),
            //     500, 
            //     (int)EnumTipoPerturbacao.perturbacao_original,
            //     tau, 
            //     std
            // );
            // RetornoGEOs ret = geo_real1.executar(new ParametrosCriterioParada(){
            //     step_para_obter_NFOBs = 500,
            //     PRECISAO_criterio_parada = 0,
            //     NFOB_criterio_parada = 100000,
            //     tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFOB
            // });
                


            // ex.ExtensiveSearch_SpacecraftOptimization();
            // ex.Teste_FuncoesObjetivo_SpacecraftOptimization();

            

            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");




            // ============================================================================
            // ============================================================================
            // Volta para o normal
            // ============================================================================
            // ============================================================================
            #if CONSOLE_OUT_FILE
                Console.SetOut (oldOut);
                writer.Close();
                ostrm.Close();
            #endif
            // ============================================================================
            // ============================================================================

            Console.WriteLine ("Done");
        }
    }
}