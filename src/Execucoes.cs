
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
        // =========================================================================
        // ====================== GERAÇÃO DAS POPULAÇÕES ===========================
        // =========================================================================
       
        public static List<double> geracao_populacao_real(List<double> lower_bounds, List<double> upper_bounds, int seed)
        {
            // Quantidade de variáveis de projeto
            double size = lower_bounds.Count;
            
            List<double> population = new List<double>();
            Random rnd = new Random(seed);

            for(int i=0; i<size; i++)
            {
                double lower = lower_bounds[i];
                double upper = upper_bounds[i];
                
                double rand = rnd.NextDouble();

                double xi = lower + ((upper - lower) * rand);

                population.Add(xi);
            }

            // Retorna a população criada
            return population;
        }


        public static List<bool> geracao_populacao_binaria(List<int> bits_por_variavel_variaveis, int seed)
        {
            List<bool> population = new List<bool>();

            // Soma os bits por variável de projeto para saber o tamanho da população
            int tamanho_populacao_bits = 0;
            foreach(int bits_da_variavel in bits_por_variavel_variaveis)
            {
                tamanho_populacao_bits += bits_da_variavel;
            }
            
            // Gera um bit para cada posição da população de bits
            Random rnd = new Random(seed);
            for (int i=0; i<tamanho_populacao_bits; ++i)
            {
                population.Add( (rnd.Next(0, 2)==1) ? true : false );
            }
            
            return population;
        }




        // =========================================================================
        // ====================== PROCESSAMENTO EXECUÇÕES ==========================
        // =========================================================================
        
        public List<Retorno_N_Execucoes_GEOs> organiza_os_resultados_de_cada_execucao(List<RetornoGEOs> todas_execucoes, ParametrosExecucao parametros_execucao)
        {
            // Cria uma lista que irá conter as estatísticas para cada algoritmo que foi executado
            List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos = new List<Retorno_N_Execucoes_GEOs>();
            

            // Obtém uma lista de int com o código dos algoritmos que foram executados
            List<int> algoritmos_executados = new List<int>();
            foreach (RetornoGEOs ret in todas_execucoes){
                algoritmos_executados.Add(ret.algoritmo_utilizado);
            }
            algoritmos_executados = algoritmos_executados.Distinct().ToList();            


            // Para cada algoritmo executado, processa as execuções
            // --> NFE: Calcula a média de NFE obtido em todas as execuções
            // --> iteracoes: Calcula a média de NFE obtido em todas as execuções
            // --> melhor_fx: Calcula a média de melhor f(x) obtido em todas as execuções
            // --> melhores_NFEs: Calcula a média do valor da função naquele NFE 
            // --> stats_TAU_per_iteration: Calcula a média do valor de tau por iteração
            // --> stats_Mfx_per_iteration: Calcula a média do valor de f(x) por iteração
            
            for (int i=0; i<algoritmos_executados.Count; i++)
            {
                int codigo_algoritmo_executado = algoritmos_executados[i];

                // Filtra todas as execuções obtendo somente as execuções deste algoritmo
                List<RetornoGEOs> execucoes_algoritmo_executado = todas_execucoes.Where(p => p.algoritmo_utilizado == codigo_algoritmo_executado).ToList();
                
                // Listas temporárias de processamento
                //-------------------------------------------------------------------------------------------
                // Lista que irá conter os melhores f(x) por NFE em cada execução
                List<List<double>> stats_melhorFX_por_NFE = new List<List<double>>();   
                // Lista que irá conter os tau por iteração em cada execução
                List<List<double>> stats_TAU_per_iteration = new List<List<double>>();
                // Lista que irá conter os melhores f(x) por iteração em cada execução
                List<List<double>> stats_Mfx_per_iteration = new List<List<double>>();
                // Lista utilizada para armazenar o NFE de cada execução para posterior média
                List<int> NFEs_para_posterior_media = new List<int>();
                // Lista utilizada para armazenar a qtde. iterações de cada execução para posterior média
                List<int> ITEs_para_posterior_media = new List<int>();
                // Lista utilizada para armazenar o melhor f(x) de cada exeução para posterior média
                List<double> MelhoresFXs_para_posterior_media = new List<double>();
                
                
                // Para cada execução, vai acumulando os resultados
                for (int j=0; j<execucoes_algoritmo_executado.Count; j++)
                {
                    // Obtém o retorno do algoritmo naquela execução
                    RetornoGEOs ret = execucoes_algoritmo_executado[j];

                    // Armazena o melhor f(x) obtido na execução
                    MelhoresFXs_para_posterior_media.Add(ret.melhor_fx);
                    // Armazena a lista contendo os fxs em cada NFE nessa execução
                    stats_melhorFX_por_NFE.Add( ret.melhores_NFEs );
                    // Armazena o NFE final obtido na execução
                    NFEs_para_posterior_media.Add(ret.NFE);
                    // Armazena o nro. de iterações final obtido na execução
                    ITEs_para_posterior_media.Add(ret.iteracoes);
                    // Armazena a lista contendo os tau em cada iteração nessa execução
                    stats_TAU_per_iteration.Add(ret.stats_TAU_per_iteration);
                    // Armazena a lista contendo os f(x) em cada iteração nessa execução
                    stats_Mfx_per_iteration.Add(ret.stats_Mfx_per_iteration);
                }

                // Calcula a média de NFEs com base em todas execuções
                int media_NFEs = (int) NFEs_para_posterior_media.Average();
                // Calcula a média de qtde de iterações com base em todas execuções
                int media_iteracoes = (int) ITEs_para_posterior_media.Average();
                // Calcula a média de melhor f(x) com base em todas execuções
                double media_melhor_fx = (double) MelhoresFXs_para_posterior_media.Average();
                // Calcula o desvio padrão final com base nos melhores f(x) das execuções
                double somatorio_sd = 0;
                foreach (double melhor_fx in MelhoresFXs_para_posterior_media)
                {
                    somatorio_sd += Math.Pow((melhor_fx - media_melhor_fx), 2);
                }
                int n = MelhoresFXs_para_posterior_media.Count - 1;
                double SD_melhor_fx = Math.Sqrt(somatorio_sd / n);


                // Listas com os resultados finais
                //-------------------------------------------------------------------------------------------
                // Lista irá conter o tau médio para cada iteração. Essa lista só poderá
                // ...ser utilizada quando o critério de parada for por número de iterações, visto que só
                // ...assim que todasas execuções terão a mesma quantidade de iterações
                List<double> lista_TAU_medio_per_iteration = new List<double>();
                // Lista irá conter o melhor f(x) médio para cada iteração. Essa lista só poderá
                // ...ser utilizada quando o critério de parada for por número de iterações, visto que só
                // ...assim que todasas execuções terão a mesma quantidade de iterações
                List<double> lista_Mfx_medio_per_iteration = new List<double>();
                // Lista irá conter o f(x) médio para cada NFE desejado. Essa lista só poderá
                // ...ser utilizada quando o critério de parada por por NFE, visto que só assim
                // ...todas as execuções terão a mesma quantidade de f(x) por NFE
                List<double> lista_MelhoresFX_por_NFE = new List<double>();


                // Se o critério de parada for por NFE, calcula o f(x) médio para cada NFE desejado
                if (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFE)
                {
                    // Obtém a quantidade de NFEs desejados
                    int quantidade_NFEs = parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados.Count;
                    
                    // Para cada NFE, percorre as execuções pra fazer a média do melhor fx
                    for(int u=0; u<quantidade_NFEs; u++)
                    {
                        List<double> fxs_no_NFE_desejado = new List<double>();
                        for(int o=0; o<parametros_execucao.quantidade_execucoes; o++)
                        {
                            // Adiciona o valor de FX de cada execução naquele NFE
                            fxs_no_NFE_desejado.Add( stats_melhorFX_por_NFE[o][u] );
                        }
                    
                        // Gera a média de f(x) naquele NFE
                        double avg_naquele_NFE = fxs_no_NFE_desejado.Average();
                        
                        // Adiciona essa média na lista de fxs médios
                        lista_MelhoresFX_por_NFE.Add(avg_naquele_NFE);
                    }
                }


                // Se o critério de parada for por ITERAÇÕES, calcula o tau médio e o f(x) médio em cada iteração
                if (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_ITERATIONS)
                {
                    // Computa os TAU e Mfx médio por cada iteração
                    for(int it=0; it<media_iteracoes; it++)
                    {
                        double sum_TAU = 0;
                        double sum_Mfx = 0;

                        for(int execs=0; execs<stats_TAU_per_iteration.Count; execs++)
                        {
                            sum_TAU += stats_TAU_per_iteration[execs][it];
                            sum_Mfx += stats_Mfx_per_iteration[execs][it];
                        }

                        double media1 = sum_TAU / stats_TAU_per_iteration.Count;
                        lista_TAU_medio_per_iteration.Add(media1);

                        double media3 = sum_Mfx / stats_Mfx_per_iteration.Count;
                        lista_Mfx_medio_per_iteration.Add(media3);
                    }
                }


                // Cria um objeto contendo os resultados finais do processamento desse
                // ... algoritmo executado para a N execuções
                Retorno_N_Execucoes_GEOs media_das_execucoes = new Retorno_N_Execucoes_GEOs();
                media_das_execucoes.codigo_algoritmo_executado = codigo_algoritmo_executado;
                media_das_execucoes.nome_algoritmo_executado = Enum.GetName(typeof(EnumNomesAlgoritmos), codigo_algoritmo_executado);
                media_das_execucoes.NFE_medio = media_NFEs;
                media_das_execucoes.ITERACOES_medio = media_iteracoes;
                media_das_execucoes.media_melhor_fx = media_melhor_fx;
                media_das_execucoes.SD_do_melhor_fx = SD_melhor_fx;
                media_das_execucoes.media_valor_FO_em_cada_NFE = lista_MelhoresFX_por_NFE;
                media_das_execucoes.lista_melhores_fxs = MelhoresFXs_para_posterior_media;
                media_das_execucoes.lista_TAU_medio_per_iteration = lista_TAU_medio_per_iteration;  
                media_das_execucoes.lista_Mfx_medio_per_iteration = lista_Mfx_medio_per_iteration;  

                // Adiciona essa estrutura na lista geral que contém uma estatística por algoritmo
                estatisticas_algoritmos.Add(media_das_execucoes);
            }

            return estatisticas_algoritmos;
        }


        // Essa função tem por objetivo apresentar as estatísticas das execuções por algoritmo. Aqui, as 
        // ...informações são apresentadas na tela, como melhor valor da função médio obtido 
        public void apresenta_resultados_finais(OQueInteressaPrintar o_que_interessa_printar, List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos, ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema)
        {
            // Concatena o nome dos algoritmos executados
            string string_algoritmos_executados = "";
            foreach(Retorno_N_Execucoes_GEOs ret_processado in estatisticas_algoritmos)
            {
                string_algoritmos_executados += ret_processado.nome_algoritmo_executado + ";";
            }

            
            // Se desejado, apresenta as informações de header
            if (o_que_interessa_printar.mostrar_header)
            {
                Console.WriteLine("\n\n==========================================================");
                Console.WriteLine("Apresenta todas estatísticas");
                Console.WriteLine("==========================================================\n");
                Console.WriteLine(String.Format("Função objetivo utilizada: {0} - {1}", parametros_problema.definicao_funcao_objetivo, parametros_problema.nome_funcao));
                Console.WriteLine(String.Format("Número de variáveis de projeto: {0}", parametros_problema.n_variaveis_projeto));
                Console.WriteLine(String.Format("Quantidade de execuções: {0}", parametros_execucao.quantidade_execucoes));
                Console.WriteLine(String.Format("Tipo de critério de parada: {0}", Enum.GetName(typeof(EnumTipoCriterioParada), parametros_execucao.parametros_criterio_parada.tipo_criterio_parada)));
                Console.WriteLine(String.Format("NFE limite para execução: {0}", parametros_execucao.parametros_criterio_parada.NFE_criterio_parada));
                Console.WriteLine("");
            }


            // Se desejado, apresenta as médias de NFE atingido, f(x) final e desvio padrão 
            if (parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX)
            {
                string resultados_finais = "parameter;" + string_algoritmos_executados + '\n';
                string media_do_NFE_atingido_nas_execucoes = "meanNFE;";
                string media_da_fx_nas_execucoes = "meanFX;";
                string sd_dos_fx_finais_nas_execucoes = "sdFX;";

                // Obtém os valores de cada algoritmo
                for (int i=0; i<estatisticas_algoritmos.Count; i++)
                {
                    int media_NFE = estatisticas_algoritmos[i].NFE_medio;
                    double media_fx = estatisticas_algoritmos[i].media_melhor_fx;
                    double sd_melhores_fx = estatisticas_algoritmos[i].SD_do_melhor_fx;

                    media_do_NFE_atingido_nas_execucoes += media_NFE.ToString() + ';';
                    media_da_fx_nas_execucoes += media_fx.ToString() + ';';
                    sd_dos_fx_finais_nas_execucoes += sd_melhores_fx.ToString() + ';';
                }

                // Substitui os pontos por vírgulas e printa
                resultados_finais += media_do_NFE_atingido_nas_execucoes + '\n';
                resultados_finais += media_da_fx_nas_execucoes + '\n';
                resultados_finais += sd_dos_fx_finais_nas_execucoes;
                resultados_finais = resultados_finais.Replace('.',',');
                
                // Printa essa linha processada
                Console.WriteLine(resultados_finais);
            }


            // Se desejado, apresenta os f(x) médio obtido em cada NFE
            if (o_que_interessa_printar.mostrar_melhores_NFE)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> f(x) médio para cada NFE:");
                Console.WriteLine("NFE;" + string_algoritmos_executados);
                
                // Obtém a quantidade de NFEs
                int quantidade_NFEs = parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados.Count;
                
                // Para cada NFE, apresenta o f(x) médio obtido em cada algoritmo
                for (int i=0; i<quantidade_NFEs; i++)
                {    
                    double NFE_atual = parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados[i];
                    string fxs_string = NFE_atual.ToString() + ';';
                    
                    // Para cada algoritmo, concatena o f(x) médio
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        // Obtém o valor do f(x) médio naquele NFE para esse algoritmo
                        double fx_naquele_NFE = estatisticas_algoritmos[j].media_valor_FO_em_cada_NFE[i];
                        // Concatena
                        fxs_string += fx_naquele_NFE.ToString() + ';';
                    }

                    // Apresenta a linha concatenada
                    Console.WriteLine( fxs_string.Replace('.',',') );
                }
                Console.WriteLine("");
            }


            // Se desejado, apresenta os f(x) obtidos em cada execução
            if (o_que_interessa_printar.mostrar_melhores_fx_cada_execucao)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Melhores f(x) para cada execução:");
                Console.WriteLine("execucao;" + string_algoritmos_executados);
                
                // Para cada execução dos algoritmos
                for (int i=0; i<parametros_execucao.quantidade_execucoes; i++)
                {    
                    string execucao_string = (i+1).ToString() + ';';
                    string melhores_fx_algoritmos = execucao_string;
                    
                    // Para cada algoritmo, concatena o f(x) final médio
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        // Obtém o f(x) final médio
                        double melhor_fx_da_execucao = estatisticas_algoritmos[j].lista_melhores_fxs[i];
                        // Concatena
                        melhores_fx_algoritmos += melhor_fx_da_execucao.ToString() + ';';
                    }

                    // Apresenta a linha concatenada
                    Console.WriteLine( melhores_fx_algoritmos.Replace('.',',') );
                }
                Console.WriteLine("");
            }


            // Se desejado, apresenta os tau obtidos por iteração
            // Somente usado quando o critério de parada for por qtde de iterações
            if (o_que_interessa_printar.mostrar_mean_TAU_iteracoes)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> TAU para cada iteração:");
                Console.WriteLine("iteracao;" + string_algoritmos_executados);
                
                int quantidade_iteracoes = parametros_execucao.parametros_criterio_parada.ITERATIONS_criterio_parada;
                
                // Concatena o tau por iteração para cada algoritmo executado
                for (int i=0; i<quantidade_iteracoes; i++)
                {    
                    string iteracao_string = (i+1).ToString();
                    string TAUs_naquela_iteracao = iteracao_string + ';';
                    
                    // Para cada algoritmo, concatena o tau por iteração
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        double TAU_naquela_iteracao = estatisticas_algoritmos[j].lista_TAU_medio_per_iteration[i];
                        TAUs_naquela_iteracao += TAU_naquela_iteracao.ToString() + ';';
                    }
                    Console.WriteLine( TAUs_naquela_iteracao.Replace('.',',') );
                }
                Console.WriteLine("");
            }


            // Se desejado, apresenta os f(x) obtidos por iteração
            // Somente usado quando o critério de parada for por qtde de iterações
            if (o_que_interessa_printar.mostrar_mean_Mfx_iteracoes)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Mfx para cada iteração:");
                Console.WriteLine("iteracao;" + string_algoritmos_executados);
                
                int quantidade_iteracoes = parametros_execucao.parametros_criterio_parada.ITERATIONS_criterio_parada;
                
                // Concatena o f(x) por iteração para cada algoritmo executado
                for (int i=0; i<quantidade_iteracoes; i++)
                {    
                    string iteracao_string = (i+1).ToString();
                    string Mfxs_naquela_iteracao = iteracao_string + ';';
                    
                    // Para cada algoritmo, concatena o f(x) por iteração
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        double fx = estatisticas_algoritmos[j].lista_Mfx_medio_per_iteration[i];
                        Mfxs_naquela_iteracao += fx.ToString() + ';';
                    }
                    Console.WriteLine( Mfxs_naquela_iteracao.Replace('.',',') );
                }
                Console.WriteLine("");
            }
        }


        public List<RetornoGEOs> executa_algoritmos_n_vezes(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema)
        {    
            // Essa lista irá conter todas as N execuções de cada um dos 
            // ...algoritmos a serem executados. Posteriormente, essa lista
            // ...será proecssada pela função organiza_os_resultados_de_cada_execucao.
            List<RetornoGEOs> todas_execucoes = new List<RetornoGEOs>();

            // Executa os algoritmos por N vezes
            for(int i=0; i<parametros_execucao.quantidade_execucoes; i++)
            {
                // Seta o seed pra gerar a população
                int seed = i;

                // Para cada execução, gera uma nova população inicial com base nos limites de cada variável
                
                // População Real
                List<double> populacao_real_gerada = geracao_populacao_real(parametros_problema.lower_bounds, parametros_problema.upper_bounds, seed);
                parametros_problema.populacao_inicial_real = new List<double>(populacao_real_gerada);

                // População Binária
                List<bool> populacao_binaria_gerada = geracao_populacao_binaria(parametros_problema.bits_por_variavel, seed);
                parametros_problema.populacao_inicial_binaria = new List<bool>(populacao_binaria_gerada);

                // ---------------------------------------------------------------------------
                
                // GEO
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEO)
                {
                    GEO_BINARIO geo = new GEO_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        parametros_problema.parametros_livres.GEO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can;

                    todas_execucoes.Add(ret);
                }

                // GEOvar
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar)
                {
                    GEOvar_BINARIO geo_var = new GEOvar_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        parametros_problema.parametros_livres.GEOvar__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var;
                    
                    todas_execucoes.Add(ret);
                }

                // A-GEO1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1)
                {
                    AGEOs_BINARIO ageo1 = new AGEOs_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1;
                        
                    todas_execucoes.Add(ret);
                }

                // A-GEO2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2)
                {
                    AGEOs_BINARIO ageo2 = new AGEOs_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2;
                        
                    todas_execucoes.Add(ret);
                }

                // A-GEO3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3)
                {
                    AGEOs_BINARIO ageo3 = new AGEOs_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        3, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO3;
                        
                    todas_execucoes.Add(ret);
                }


                // AGEO1var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var)
                {
                    AGEOsvar_BINARIO ageo1_var = new AGEOsvar_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo1_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var)
                {
                    AGEOsvar_BINARIO ageo2_var = new AGEOsvar_BINARIO(
                        parametros_problema.populacao_inicial_binaria,
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2_var.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal1 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_igor)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_GEOreal1, 
                        parametros_problema.parametros_livres.std_GEOreal1);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1_igor;

                    todas_execucoes.Add(ret);
                }

                // AGEO1real1 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1_igor)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    AGEOs_REAL1 AGEO1real1 = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.std_AGEO1real1, 
                        1, 
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO1real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real1_igor;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real1 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_igor)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    AGEOs_REAL1 AGEO2real1 = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.std_AGEO2real1, 
                        2, 
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO2real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real1_igor;
                        
                    todas_execucoes.Add(ret);
                }





                // GEOreal1_O  =  GEOreal1 + PERTURBAÇÃO IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.GEOreal1_O__tau, 
                        parametros_problema.parametros_livres.GEOreal1_O__std);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1_O;

                    todas_execucoes.Add(ret);
                }

                // GEOreal1_P  =  GEOreal1 + PERTURBAÇÃO PORCENTAGEM
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_P)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.GEOreal1_P__tau, 
                        parametros_problema.parametros_livres.GEOreal1_P__porc);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1_P;

                    todas_execucoes.Add(ret);
                }

                // GEOreal1_N  =  GEOreal1 + PERTURBAÇÃO DISTNORMAL
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_N)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.GEOreal1_N__tau, 
                        parametros_problema.parametros_livres.GEOreal1_N__std);
                        
                    RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1_N;

                    todas_execucoes.Add(ret);
                }



                // GEOreal2_O_VO  =  GEOreal2 + PERTURBAÇÃO IGOR + VARIAÇÃO ORIGINAL
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_VO)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_O_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_O_VO__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_O_VO__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_O_VO__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_O_VO;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_P_VO  =  GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO ORIGINAL
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_VO__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_VO;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_N_VO  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO ORIGINAL
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_VO__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_VO;
                        
                    todas_execucoes.Add(ret);
                }



                // GEOreal2_O_DS  =  GEOreal2 + PERTURBAÇÃO IGOR + VARIAÇÃO DIVIDE POR S
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_DS)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_O_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_O_DS__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_O_DS__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_O_DS__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_O_DS;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_P_DS  =  GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO DIVIDE POR S
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_DS__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_DS;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_N_DS  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO DIVIDE POR S
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_DS__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_DS;
                        
                    todas_execucoes.Add(ret);
                }




                // GEOreal2_P_VO_UNI  =  GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO ORIGINAL + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO_UNI)
                {
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_VO_UNI;
                        
                    todas_execucoes.Add(ret);
                }
                
                // GEOreal2_N_VO_UNI  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO ORIGINAL + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO_UNI)
                {
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_VO_UNI;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_P_DS_UNI  =  GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO DIVIDE POR S + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS_UNI)
                {
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_DS_UNI;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_N_DS_UNI  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO DIVIDE POR S + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS_UNI)
                {
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_DS_UNI;
                        
                    todas_execucoes.Add(ret);
                }




                // AGEO1real2 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real2_igor)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    AGEOs_REAL2 AGEO1real2 = new AGEOs_REAL2(
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.std_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.P_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.s_AGEO1real2,
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO1real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real2_igor;

                    todas_execucoes.Add(ret);
                }

                // AGEO2real2 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_igor)
                {
                    bool ultima_perturbacao_random_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    AGEOs_REAL2 AGEO2real2 = new AGEOs_REAL2(
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.std_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.P_AGEO1real2, 
                        (int)parametros_problema.parametros_livres.s_AGEO1real2,
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO2real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_igor;

                    todas_execucoes.Add(ret);
                }
                
                
                
                // ASGEO2real1_1 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_1)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    ASGEO2_REAL1_1 ASGEO2real1_1 = new ASGEO2_REAL1_1(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_1, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_1,
                        parametros_problema.parametros_livres.q_one_fifth_rule,
                        parametros_problema.parametros_livres.c_one_fifth_rule,
                        parametros_problema.parametros_livres.stdmin_one_fifth_rule);
                        
                    RetornoGEOs ret = ASGEO2real1_1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_1;
                        
                    todas_execucoes.Add(ret);
                }
                
                // ASGEO2real1_2 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_2)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    ASGEO2_REAL1_2 ASGEO2real1_2 = new ASGEO2_REAL1_2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_2, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_2,
                        parametros_problema.parametros_livres.q_one_fifth_rule,
                        parametros_problema.parametros_livres.c_one_fifth_rule,
                        parametros_problema.parametros_livres.stdmin_one_fifth_rule);
                        
                    RetornoGEOs ret = ASGEO2real1_2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_2;
                        
                    todas_execucoes.Add(ret);
                }
                
                // ASGEO2real1_3 - IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real1_3)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    ASGEO2_REAL1_3 ASGEO2real1_3 = new ASGEO2_REAL1_3(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        tipo_perturbacao,
                        parametros_problema.parametros_livres.tau_ASGEO2_REAL1_3, 
                        parametros_problema.parametros_livres.std_ASGEO2_REAL1_3);
                        
                    RetornoGEOs ret = ASGEO2real1_3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real1_3;
                        
                    todas_execucoes.Add(ret);
                }

                // ASGEO2real2_1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_ASGEO2real2_1)
                {
                    ASGEO2_REAL2_1 ASGEO2real2_1 = new ASGEO2_REAL2_1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.ASGEO2_REAL2_1_P,
                        parametros_problema.parametros_livres.ASGEO2_REAL2_1_std1,
                        parametros_problema.parametros_livres.ASGEO2_REAL2_1_s,
                        (int)EnumTipoPerturbacao.perturbacao_porcentagem);
                        
                    RetornoGEOs ret = ASGEO2real2_1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.ASGEO2real2_1;

                    todas_execucoes.Add(ret);
                }

                
                
                // AGEO2_REAL1_igor
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2_REAL1_igor)
                {
                    AGEOs_REAL1 AGEO2_REAL1_igor = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.std_AGEO2real1, 
                        2, 
                        (int)EnumTipoPerturbacao.perturbacao_igor);
                        
                    RetornoGEOs ret = AGEO2_REAL1_igor.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2_REAL1_igor;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2_REAL1_porcentagem
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2_REAL1_porcentagem)
                {
                    AGEOs_REAL1 AGEO2_REAL1_porcentagem = new AGEOs_REAL1(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.p1_AGEO2real1, 
                        2, 
                        (int)EnumTipoPerturbacao.perturbacao_porcentagem);
                        
                    RetornoGEOs ret = AGEO2_REAL1_porcentagem.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2_REAL1_porcentagem;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2_REAL2_igor
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2_REAL2_igor)
                {
                    int P = 6;
                    double std = 50;
                    int s = 10;
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;

                    AGEOs_REAL2 AGEO2_REAL2_igor = new AGEOs_REAL2(
                        2,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        std,
                        P,
                        s,
                        (int)EnumTipoPerturbacao.perturbacao_igor);
                        
                    RetornoGEOs ret = AGEO2_REAL2_igor.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2_REAL2_igor;

                    todas_execucoes.Add(ret);
                }

                // AGEO2_REAL2_porcentagem
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2_REAL2_porcentagem)
                {
                    int P = 6;
                    double std = 10;
                    int s = 10;
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;

                    AGEOs_REAL2 AGEO2_REAL2_porcentagem = new AGEOs_REAL2(
                        2,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        std,
                        P,
                        s,
                        (int)EnumTipoPerturbacao.perturbacao_porcentagem);
                        
                    RetornoGEOs ret = AGEO2_REAL2_porcentagem.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2_REAL2_porcentagem;

                    todas_execucoes.Add(ret);
                }

                // AGEO2_REAL2_normal
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2_REAL2_normal)
                {
                    int P = 6;
                    double std = 10;
                    int s = 10;
                    bool ultima_perturbacao_random_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;

                    AGEOs_REAL2 AGEO2_REAL2_normal = new AGEOs_REAL2(
                        2,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.definicao_funcao_objetivo, 
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados,
                        ultima_perturbacao_random_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        std,
                        P,
                        s,
                        (int)EnumTipoPerturbacao.perturbacao_normal);
                        
                    RetornoGEOs ret = AGEO2_REAL2_normal.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2_REAL2_normal;

                    todas_execucoes.Add(ret);
                }
            }

            // Retorna a lista com todas as N execuções para os algoritmos a serem executados
            return todas_execucoes;
        }
        


       


        // =========================================================================
        // ============================= TUNINGS ===================================
        // =========================================================================

        // =========================================================================
        // Ordena o tuning e apresenta
        
        public static void ordena_e_apresenta_resultados_tuning(List<Tuning> tuning_results)
        {
            // Ordena os resultados do tuning com base no f(x)
            List<Tuning> sortedList = tuning_results.OrderBy(i => i.fx).ThenBy(i => i.NFE).ToList();
            
            // Apresenta os resultados linha por linha
            foreach (Tuning tun in sortedList){
                string str_NFE = String.Format("{0,7}", tun.NFE);
                string str_fx = String.Format("{0:#.000000000000000E+00}", tun.fx);
                string str_sd = String.Format("{0:#.000000000000000E+00}", tun.sd);
                // string str_parameters = "";
                string str_parameters = tun.parameters;
                string final = String.Format("{0}  -->  NFE: {1}  -->  fx: {2}  -->  sd:{3}", str_parameters, str_NFE, str_fx, str_sd);
                Console.WriteLine(final.Replace('.',','));
            }
        }

        public static string formata_string_parametros_tuning(double P, double s, double std1, double tau, double porc, bool std_or_porc)
        {
            string str_P            = String.Format("{0,2}", P);
            string str_s            = String.Format("{0,2}", s);
            string str_tau          = String.Format("{0,5:0.00}", tau);
            string parameters       = "";
            if (!std_or_porc){
                string str_std1 = String.Format("{0,5:0.00}", std1);
                parameters = String.Format("P = {0};   s = {1};   std1 = {2};   tau = {3}", str_P, str_s, str_std1, str_tau);
            }
            else{
                string str_porc = String.Format("{0,5:0.00}", porc);
                parameters = String.Format("P = {0};   s = {1};   porc = {2};   tau = {3}", str_P, str_s, str_porc, str_tau);
            }

            return parameters;
        }

        public static string formata_string_parametros_tuning_GEOreal1(double tau, double std, double porc, bool std_or_porc)
        {
            string str_tau          = String.Format("{0,5:0.00}", tau);
            string parameters       = "";
            if (!std_or_porc){
                string str_std = String.Format("{0,5:0.00}", std);
                parameters = String.Format("tau = {0};   std = {1};", str_tau, str_std);
            }
            else{
                string str_porc = String.Format("{0,5:0.00}", porc);
                parameters = String.Format("tau = {0};   porc = {1};", str_tau, str_porc);
            }

            // string str_tau = String.Format("{0:00.00}", tau);
                        
            return parameters;
        }

        public static string formata_string_parametros_tuning_GEOeGEOvar(double tau)
        {
            string str_tau = String.Format("{0,5:0.00}", tau);
            string parameters = String.Format("tau = {0};", str_tau);
                        
            return parameters;
        }



        // =========================================================================
        // Tuning GEO e GEOvar

        public List<Tuning> tuning_GEO_GEOvar(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, bool GEOorGEOvar)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            if (GEOorGEOvar)        parametros_execucao.quais_algoritmos_rodar.rodar_GEO = true;
            else                    parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar = true;
            
            // Itera cada tau
            foreach (double tau in valores_tau){
                parametros_problema.parametros_livres.GEO__tau = tau; 
                parametros_problema.parametros_livres.GEOvar__tau = tau; 

                // Executa cada algoritmo por N vezes e obtém todas as execuções
                List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                
                // Organiza os resultados de todas as excuções por algoritmo
                List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                tuning_results.Add(new Tuning(){
                    parameters = formata_string_parametros_tuning_GEOeGEOvar(tau),
                    fx = resultados_por_algoritmo[0].media_melhor_fx,
                    sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                    NFE = resultados_por_algoritmo[0].NFE_medio,
                });
            }

            return tuning_results;
        }



        // =========================================================================
        // Tuning GEOreal1

        public List<Tuning> tuning_GEOreal1_O(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O = true;
            
            // Itera cada std e cada tau
            foreach (double tau in valores_tau){
                foreach (double std in valores_std){
                    parametros_problema.parametros_livres.GEOreal1_O__tau = tau; 
                    parametros_problema.parametros_livres.GEOreal1_O__std = std; 

                    // Executa cada algoritmo por N vezes e obtém todas as execuções
                    List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // Organiza os resultados de todas as excuções por algoritmo
                    List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                    // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                    tuning_results.Add(new Tuning(){
                        parameters = formata_string_parametros_tuning_GEOreal1(tau, std, 0, false),
                        fx = resultados_por_algoritmo[0].media_melhor_fx,
                        sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                        NFE = resultados_por_algoritmo[0].NFE_medio,
                    });
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal1_P(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_porcent)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_P = true;
            
            // Itera cada std e cada tau
            foreach (double tau in valores_tau){
                foreach (double porc in valores_porcent){
                    parametros_problema.parametros_livres.GEOreal1_P__tau = tau; 
                    parametros_problema.parametros_livres.GEOreal1_P__porc = porc; 

                    // Executa cada algoritmo por N vezes e obtém todas as execuções
                    List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // Organiza os resultados de todas as excuções por algoritmo
                    List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                    // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                    tuning_results.Add(new Tuning(){
                        parameters = formata_string_parametros_tuning_GEOreal1(tau, 0, porc, true),
                        fx = resultados_por_algoritmo[0].media_melhor_fx,
                        sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                        NFE = resultados_por_algoritmo[0].NFE_medio,
                    });
                }
            }

            return tuning_results;
        }
        
        public List<Tuning> tuning_GEOreal1_N(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_N = true;
            
            // Itera cada std e cada tau
            foreach (double tau in valores_tau){
                foreach (double std in valores_std){
                    parametros_problema.parametros_livres.GEOreal1_N__tau = tau; 
                    parametros_problema.parametros_livres.GEOreal1_N__std = std; 

                    // Executa cada algoritmo por N vezes e obtém todas as execuções
                    List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // Organiza os resultados de todas as excuções por algoritmo
                    List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                    // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                    tuning_results.Add(new Tuning(){
                        parameters = formata_string_parametros_tuning_GEOreal1(tau, std, 0, false),
                        fx = resultados_por_algoritmo[0].media_melhor_fx,
                        sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                        NFE = resultados_por_algoritmo[0].NFE_medio,
                    });
                }
            }

            return tuning_results;
        }

        
        // =========================================================================
        // Tuning GEOreal2

        public List<Tuning> tuning_GEOreal2_O_VO(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_VO = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_O_VO__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_O_VO__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_O_VO__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_O_VO__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_P_VO(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_porcentagem, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double porcent in valores_porcentagem){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_P_VO__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO__porc = porcent; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, 0, tau, porcent, true),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_N_VO(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_N_VO__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_O_DS(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_DS = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_O_DS__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_O_DS__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_O_DS__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_O_DS__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_P_DS(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_porcentagem, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double porcent in valores_porcentagem){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_P_DS__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS__porc = porcent; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, 0, tau, porcent, true),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_N_DS(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_N_DS__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }
        

        // =========================================================================
        // Tuning GEOreal2_UNI

        public List<Tuning> tuning_GEOreal2_P_VO_UNI(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_porcentagem, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO_UNI = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double porcent in valores_porcentagem){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__porc = porcent; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_P_VO_UNI__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, 0, tau, porcent, true),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_N_VO_UNI(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO_UNI = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_N_VO_UNI__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_P_DS_UNI(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_porcentagem, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS_UNI = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double porcent in valores_porcentagem){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__porc = porcent; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_P_DS_UNI__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, 0, tau, porcent, true),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }

        public List<Tuning> tuning_GEOreal2_N_DS_UNI(ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, List<double> valores_tau, List<double> valores_std1, List<double> valores_P, List<double> valores_s)
        {
            List<Tuning> tuning_results = new List<Tuning>();

            parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS_UNI = true;
            
            // Itera cada std e cada tau
            foreach (double P in valores_P){
                foreach (double s in valores_s){
                    foreach (double std1 in valores_std1){
                        foreach (double tau in valores_tau){
                            parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__tau = tau; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__std = std1; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__P = P; 
                            parametros_problema.parametros_livres.GEOreal2_N_DS_UNI__s = s; 
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);

                            // Para essa combinação de parâmetros, armazena o melhor valor de f(x)
                            tuning_results.Add(new Tuning(){
                                parameters = formata_string_parametros_tuning(P, s, std1, tau, 0, false),
                                fx = resultados_por_algoritmo[0].media_melhor_fx,
                                sd = resultados_por_algoritmo[0].SD_do_melhor_fx,
                                NFE = resultados_por_algoritmo[0].NFE_medio,
                            });
                        }
                    }
                }
            }

            return tuning_results;
        }
        




        // =========================================================================
        // ==================== Parâmetros dos Problemas ===========================
        // =========================================================================

        public ParametrosProblema get_function_parameters(int definicao_funcao_objetivo)
        {
            ParametrosProblema parametros_problema;
            switch(definicao_funcao_objetivo)
            {
                // GRIEWANGK
                case (int)EnumNomesFuncoesObjetivo.griewangk:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Griewangk";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 2.75,
                        
                        GEOreal1_O__std = 1,
                        GEOreal1_O__tau = 1.5,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 2,
                        GEOreal1_N__tau = 1.5,

                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 2.5,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 10,
                        GEOreal2_P_VO__tau = 3,

                        GEOreal2_N_VO__P = 4,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 2.5,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 5,
                        GEOreal2_P_DS__tau = 3,

                        // 8/2/10/...
                        // 4/10/10/...
                        GEOreal2_N_DS__P = 4,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 5,
                        GEOreal2_N_DS__tau = 4,


                        
                        
                        GEOreal2_P_VO_UNI__P = 12,
                        GEOreal2_P_VO_UNI__s = 1,
                        GEOreal2_P_VO_UNI__porc = 1,
                        GEOreal2_P_VO_UNI__tau = 2.5,

                        GEOreal2_N_VO_UNI__P = 4,
                        GEOreal2_N_VO_UNI__s = 2,
                        GEOreal2_N_VO_UNI__std = 2,
                        GEOreal2_N_VO_UNI__tau = 2.5,

                        GEOreal2_P_DS_UNI__P = 4,
                        GEOreal2_P_DS_UNI__s = 10,
                        GEOreal2_P_DS_UNI__porc = 1,
                        GEOreal2_P_DS_UNI__tau = 4,

                        GEOreal2_N_DS_UNI__P = 4,
                        GEOreal2_N_DS_UNI__s = 10,
                        GEOreal2_N_DS_UNI__std = 10,
                        GEOreal2_N_DS_UNI__tau = 4,




                        std_AGEO1real1 = 0.8,
                        std_AGEO2real1 = 1.2,
                        p1_AGEO1real1 = 0.2,
                        p1_AGEO2real1 = 0.2,
                        
                        std_AGEO1real2 = 1,
                        P_AGEO1real2 = 4,
                        s_AGEO1real2 = 1,
                        
                        std_AGEO2real2 = 1,
                        P_AGEO2real2 = 4,
                        s_AGEO2real2 = 1,
                        
                        tau_ASGEO2_REAL1_1 = 1.5,
                        std_ASGEO2_REAL1_1 = 1.2,
                        tau_ASGEO2_REAL1_2 = 1.5,
                        std_ASGEO2_REAL1_2 = 1.2,
                        tau_ASGEO2_REAL1_3 = 1.5,
                        std_ASGEO2_REAL1_3 = 1.2,
                    };
                break;
                }

                // RASTRINGIN
                case (int)EnumNomesFuncoesObjetivo.rastringin:
                {
                    int n_variaveis = 20;

                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Rastringin";
                    parametros_problema.n_variaveis_projeto = n_variaveis;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1,
                        GEOvar__tau = 1.75,

                        GEOreal1_O__std = 1.8,
                        GEOreal1_O__tau = 2,

                        GEOreal1_P__porc = 3,
                        GEOreal1_P__tau = 5,

                        GEOreal1_N__std = 0.4,
                        GEOreal1_N__tau = 6,
                        
                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 4.5,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 2,
                        GEOreal2_P_VO__porc = 10,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 12,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 1,
                        GEOreal2_N_VO__tau = 5,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 16,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 5,




                        GEOreal2_P_VO_UNI__P = 12,
                        GEOreal2_P_VO_UNI__s = 2,
                        GEOreal2_P_VO_UNI__porc = 10,
                        GEOreal2_P_VO_UNI__tau = 5,

                        GEOreal2_N_VO_UNI__P = 12,
                        GEOreal2_N_VO_UNI__s = 2,
                        GEOreal2_N_VO_UNI__std = 2,
                        GEOreal2_N_VO_UNI__tau = 5,

                        GEOreal2_P_DS_UNI__P = 12,
                        GEOreal2_P_DS_UNI__s = 10,
                        GEOreal2_P_DS_UNI__porc = 10,
                        GEOreal2_P_DS_UNI__tau = 5,

                        GEOreal2_N_DS_UNI__P = 12,
                        GEOreal2_N_DS_UNI__s = 10,
                        GEOreal2_N_DS_UNI__std = 1,
                        GEOreal2_N_DS_UNI__tau = 5,



                        std_AGEO1real1 = 0.8,
                        std_AGEO2real1 = 1,
                        p1_AGEO1real1 = 5.2,
                        p1_AGEO2real1 = 4.8,
                        
                        std_AGEO1real2 = 1,
                        P_AGEO1real2 = 4,
                        s_AGEO1real2 = 1,
                        
                        std_AGEO2real2 = 1,
                        P_AGEO2real2 = 4,
                        s_AGEO2real2 = 1,
                        
                        tau_ASGEO2_REAL1_1 = 1.5,
                        std_ASGEO2_REAL1_1 = 1,
                        tau_ASGEO2_REAL1_2 = 1.5,
                        std_ASGEO2_REAL1_2 = 1,
                        tau_ASGEO2_REAL1_3 = 1.5,
                        std_ASGEO2_REAL1_3 = 1,
                    };
                break;
                }

                // ROSENBROCK
                case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Rosenbrock";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 2.5,
                        GEOreal1_O__tau = 6,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 5,
                        
                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 5,

                        GEOreal2_P_VO__P = 4,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 0.1,
                        GEOreal2_P_VO__tau = 4,

                        GEOreal2_N_VO__P = 4,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 0.5,
                        GEOreal2_N_VO__tau = 5,

                        // 8/2/5/...
                        // 4/10/10/...
                        GEOreal2_O_DS__P = 12,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 4,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 0.1,
                        GEOreal2_P_DS__tau = 5,

                        // 4/10/1/...
                        // 8/2/1/...
                        GEOreal2_N_DS__P = 8,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 1,
                        GEOreal2_N_DS__tau = 5,




                        GEOreal2_P_VO_UNI__P = 4,
                        GEOreal2_P_VO_UNI__s = 1,
                        GEOreal2_P_VO_UNI__porc = 0.1,
                        GEOreal2_P_VO_UNI__tau = 4,

                        GEOreal2_N_VO_UNI__P = 4,
                        GEOreal2_N_VO_UNI__s = 2,
                        GEOreal2_N_VO_UNI__std = 0.5,
                        GEOreal2_N_VO_UNI__tau = 5,

                        GEOreal2_P_DS_UNI__P = 4,
                        GEOreal2_P_DS_UNI__s = 2,
                        GEOreal2_P_DS_UNI__porc = 0.1,
                        GEOreal2_P_DS_UNI__tau = 4,

                        GEOreal2_N_DS_UNI__P = 4,
                        GEOreal2_N_DS_UNI__s = 10,
                        GEOreal2_N_DS_UNI__std = 1,
                        GEOreal2_N_DS_UNI__tau = 4,




                        std_AGEO1real1 = 1.6,
                        std_AGEO2real1 = 1.8,
                        p1_AGEO1real1 = 0.4,
                        p1_AGEO2real1 = 0.2,
                        
                        std_AGEO1real2 = 2,
                        P_AGEO1real2 = 8,
                        s_AGEO1real2 = 1,
                        
                        std_AGEO2real2 = 2,
                        P_AGEO2real2 = 4,
                        s_AGEO2real2 = 2,
                        
                        tau_ASGEO2_REAL1_1 = 4,
                        std_ASGEO2_REAL1_1 = 1.8,
                        tau_ASGEO2_REAL1_2 = 4,
                        std_ASGEO2_REAL1_2 = 1.8,
                        tau_ASGEO2_REAL1_3 = 4,
                        std_ASGEO2_REAL1_3 = 1.8,
                    };
                break;
                }

                // SCHWEFEL
                case (int)EnumNomesFuncoesObjetivo.schwefel:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Schwefel";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 0.75,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 1.8,
                        GEOreal1_O__tau = 6,

                        GEOreal1_P__porc = 8,
                        GEOreal1_P__tau = 2.5,

                        GEOreal1_N__std = 1.4,
                        GEOreal1_N__tau = 1.5,

                        GEOreal2_O_VO__P = 16,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 8,
                        GEOreal2_O_VO__tau = 4,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 2,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 4,
                        GEOreal2_N_VO__s = 1,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 4,

                        GEOreal2_O_DS__P = 12,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 50,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 4,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 0.5,




                        GEOreal2_P_VO_UNI__P = 12,
                        GEOreal2_P_VO_UNI__s = 2,
                        GEOreal2_P_VO_UNI__porc = 50,
                        GEOreal2_P_VO_UNI__tau = 5,

                        GEOreal2_N_VO_UNI__P = 12,
                        GEOreal2_N_VO_UNI__s = 2,
                        GEOreal2_N_VO_UNI__std = 2,
                        GEOreal2_N_VO_UNI__tau = 4,

                        GEOreal2_P_DS_UNI__P = 12,
                        GEOreal2_P_DS_UNI__s = 10,
                        GEOreal2_P_DS_UNI__porc = 50,
                        GEOreal2_P_DS_UNI__tau = 5,

                        GEOreal2_N_DS_UNI__P = 4,
                        GEOreal2_N_DS_UNI__s = 2,
                        GEOreal2_N_DS_UNI__std = 10,
                        GEOreal2_N_DS_UNI__tau = 0.5,



                        std_AGEO1real1 = 1.8,
                        std_AGEO2real1 = 2.4,
                        p1_AGEO1real1 = 5,
                        p1_AGEO2real1 = 8.4,
                        
                        std_AGEO1real2 = 2,
                        P_AGEO1real2 = 16,
                        s_AGEO1real2 = 1,
                        
                        std_AGEO2real2 = 8,
                        P_AGEO2real2 = 16,
                        s_AGEO2real2 = 1,
                        
                        tau_ASGEO2_REAL1_1 = 5,
                        std_ASGEO2_REAL1_1 = 2.4,
                        tau_ASGEO2_REAL1_2 = 5,
                        std_ASGEO2_REAL1_2 = 2.4,
                        tau_ASGEO2_REAL1_3 = 5,
                        std_ASGEO2_REAL1_3 = 2.4,
                    };
                break;
                }

                // ACKLEY
                case (int)EnumNomesFuncoesObjetivo.ackley:
                {
                    int n_variaveis_projeto = 30;

                    parametros_problema = new ParametrosProblema()
                    {
                        nome_funcao = "Ackley",
                        n_variaveis_projeto = n_variaveis_projeto,
                        definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.ackley,
                        bits_por_variavel = Enumerable.Repeat(16, n_variaveis_projeto).ToList(),
                        lower_bounds = Enumerable.Repeat(-30.0, n_variaveis_projeto).ToList(),
                        upper_bounds = Enumerable.Repeat(30.0, n_variaveis_projeto).ToList(),
                        fx_esperado = 0.0,
                        parametros_livres = new ParametrosLivreProblema()
                        {
                            GEO__tau = 3.25,
                            GEOvar__tau = 2.25,

                            GEOreal1_O__std = 0.8,
                            GEOreal1_O__tau = 1,

                            GEOreal1_P__porc = 1,
                            GEOreal1_P__tau = 5,

                            GEOreal1_N__std = 0.6,
                            GEOreal1_N__tau = 6,

                            GEOreal2_O_VO__P = 4,
                            GEOreal2_O_VO__s = 1,
                            GEOreal2_O_VO__std = 1,
                            GEOreal2_O_VO__tau = 5,

                            GEOreal2_P_VO__P = 12,
                            GEOreal2_P_VO__s = 2,
                            GEOreal2_P_VO__porc = 10,
                            GEOreal2_P_VO__tau = 5,

                            GEOreal2_N_VO__P = 12,
                            GEOreal2_N_VO__s = 2,
                            GEOreal2_N_VO__std = 2,
                            GEOreal2_N_VO__tau = 4.5,

                            GEOreal2_O_DS__P = 4,
                            GEOreal2_O_DS__s = 2,
                            GEOreal2_O_DS__std = 1,
                            GEOreal2_O_DS__tau = 5,

                            GEOreal2_P_DS__P = 12,
                            GEOreal2_P_DS__s = 10,
                            GEOreal2_P_DS__porc = 5,
                            GEOreal2_P_DS__tau = 5,

                            GEOreal2_N_DS__P = 16,
                            GEOreal2_N_DS__s = 10,
                            GEOreal2_N_DS__std = 5,
                            GEOreal2_N_DS__tau = 5,




                            GEOreal2_P_VO_UNI__P = 12,
                            GEOreal2_P_VO_UNI__s = 2,
                            GEOreal2_P_VO_UNI__porc = 10,
                            GEOreal2_P_VO_UNI__tau = 5,

                            GEOreal2_N_VO_UNI__P = 12,
                            GEOreal2_N_VO_UNI__s = 2,
                            GEOreal2_N_VO_UNI__std = 2,
                            GEOreal2_N_VO_UNI__tau = 5,

                            GEOreal2_P_DS_UNI__P = 12,
                            GEOreal2_P_DS_UNI__s = 10,
                            GEOreal2_P_DS_UNI__porc = 10,
                            GEOreal2_P_DS_UNI__tau = 5,

                            GEOreal2_N_DS_UNI__P = 12,
                            GEOreal2_N_DS_UNI__s = 10,
                            GEOreal2_N_DS_UNI__std = 5,
                            GEOreal2_N_DS_UNI__tau = 5,
                            


                            std_AGEO1real1 = 0.8,
                            std_AGEO2real1 = 0.8,
                            p1_AGEO1real1 = 3.6,
                            p1_AGEO2real1 = 1,
                            
                            std_AGEO1real2 = 1,
                            P_AGEO1real2 = 4,
                            s_AGEO1real2 = 1,
                            
                            std_AGEO2real2 = 1,
                            P_AGEO2real2 = 4,
                            s_AGEO2real2 = 1,
                            
                            tau_ASGEO2_REAL1_1 = 1,
                            std_ASGEO2_REAL1_1 = 0.8,
                            tau_ASGEO2_REAL1_2 = 1,
                            std_ASGEO2_REAL1_2 = 0.8,
                            tau_ASGEO2_REAL1_3 = 1,
                            std_ASGEO2_REAL1_3 = 0.8,
                        }
                    };
                break;
                }

                // BEALE
                case (int)EnumNomesFuncoesObjetivo.beale:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Beale";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 2,
                        GEOreal1_O__tau = 5,

                        GEOreal1_P__porc = 1.5,
                        GEOreal1_P__tau = 1.5,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 2,

                        GEOreal2_O_VO__P = 8,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 4,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 3.5,

                        GEOreal2_N_VO__P = 12,
                        GEOreal2_N_VO__s = 1,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 2.5,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 1,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 50,
                        GEOreal2_P_DS__tau = 3,

                        GEOreal2_N_DS__P = 16,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 5,
                        GEOreal2_N_DS__tau = 2.5,




                        GEOreal2_P_VO_UNI__P = 12,
                        GEOreal2_P_VO_UNI__s = 1,
                        GEOreal2_P_VO_UNI__porc = 50,
                        GEOreal2_P_VO_UNI__tau = 3,

                        GEOreal2_N_VO_UNI__P = 12,
                        GEOreal2_N_VO_UNI__s = 1,
                        GEOreal2_N_VO_UNI__std = 2,
                        GEOreal2_N_VO_UNI__tau = 2.5,

                        GEOreal2_P_DS_UNI__P = 12,
                        GEOreal2_P_DS_UNI__s = 2,
                        GEOreal2_P_DS_UNI__porc = 50,
                        GEOreal2_P_DS_UNI__tau = 3.5,

                        GEOreal2_N_DS_UNI__P = 12,
                        GEOreal2_N_DS_UNI__s = 2,
                        GEOreal2_N_DS_UNI__std = 5,
                        GEOreal2_N_DS_UNI__tau = 3.5,


                        
                        std_AGEO1real1 = 1.8,
                        std_AGEO2real1 = 1.8,
                        p1_AGEO1real1 = 1.2,
                        p1_AGEO2real1 = 1.4,
                        
                        // std_AGEO1real2 = 2,
                        // P_AGEO1real2 = 8,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 2,
                        // P_AGEO2real2 = 4,
                        // s_AGEO2real2 = 2,
                        
                        tau_ASGEO2_REAL1_1 = 5,
                        std_ASGEO2_REAL1_1 = 1.8,
                        tau_ASGEO2_REAL1_2 = 5,
                        std_ASGEO2_REAL1_2 = 1.8,
                        tau_ASGEO2_REAL1_3 = 5,
                        std_ASGEO2_REAL1_3 = 1.8,
                    };
                break;
                }
                
                
                
                
                // LEVY13
                case (int)EnumNomesFuncoesObjetivo.levy13:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Levy13";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
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
                        
                        // tau_ASGEO2_REAL1_1 = 5,
                        // std_ASGEO2_REAL1_1 = 2,
                        // tau_ASGEO2_REAL1_2 = 5,
                        // std_ASGEO2_REAL1_2 = 2,
                        // tau_ASGEO2_REAL1_3 = 5,
                        // std_ASGEO2_REAL1_3 = 2,
                    };
                break;
                }

                // PAVIANI
                case (int)EnumNomesFuncoesObjetivo.paviani:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Paviani";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(10, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(2.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // SALOMON
                case (int)EnumNomesFuncoesObjetivo.salomon:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Salomon";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // SCHAFFER 2
                case (int)EnumNomesFuncoesObjetivo.schaffer_2:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Schaffer 2";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BARTELS CONN
                case (int)EnumNomesFuncoesObjetivo.bartels_conn:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bartels Conn";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(17, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BIRD
                case (int)EnumNomesFuncoesObjetivo.bird:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bird";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BOHACHEVSKY 1
                case (int)EnumNomesFuncoesObjetivo.bohachevsky_1:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bohachevsky 1";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }





                // F9 TESE
                case (int)EnumNomesFuncoesObjetivo.F09:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "F9";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.50,
                        // tau_GEOvar = 1.75,
                        tau_GEOreal1 = 1.50,
                        tau_GEOreal2 = 1.75,
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
                }

                // DEJONG3
                case (int)EnumNomesFuncoesObjetivo.dejong3:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "DeJong#3";
                    parametros_problema.n_variaveis_projeto = 5;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        
                        // tau_GEO = 3.0,
                        // tau_GEOvar = 8.0,
                        tau_GEOreal1 = 3.0,
                        tau_GEOreal2 = 8.0,
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
                }

                // SPACECRAFT
                case (int)EnumNomesFuncoesObjetivo.spacecraft:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "SPACECRAFT";
                    parametros_problema.n_variaveis_projeto = 3;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = new List<int>(){2,6,6};
                    parametros_problema.lower_bounds = new List<double>(){13,1,0};
                    parametros_problema.upper_bounds = new List<double>(){15,60,59};
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 3.0,
                        // tau_GEOvar = 8.0,

                        tau_GEOreal1 = 1.0,
                        tau_GEOreal2 = 1.0,
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
                }

                // DEFAULT
                default:
                {
                    Console.WriteLine("DEFAULT PARAMETERS!");
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "DEFAULT";
                    parametros_problema.n_variaveis_projeto = 0;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = new List<int>();
                    parametros_problema.lower_bounds = new List<double>();
                    parametros_problema.upper_bounds = new List<double>();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                    parametros_problema.populacao_inicial_real = new List<double>();
                    parametros_problema.populacao_inicial_binaria = new List<bool>();
                break;
                }
            }

            return parametros_problema;
        }





        // =========================================================================
        // =============================== MAIN ====================================
        // =========================================================================

        public void Execucoes()
        {
            // Cria uma lista contendo as funções a serem executadas
            List<int> function_values = new List<int>()
            {
                (int)EnumNomesFuncoesObjetivo.griewangk,
                (int)EnumNomesFuncoesObjetivo.rastringin,
                (int)EnumNomesFuncoesObjetivo.rosenbrock,
                (int)EnumNomesFuncoesObjetivo.schwefel,
                (int)EnumNomesFuncoesObjetivo.ackley,
                (int)EnumNomesFuncoesObjetivo.beale,
                
                // (int)EnumNomesFuncoesObjetivo.paviani,
                // (int)EnumNomesFuncoesObjetivo.salomon,
                // (int)EnumNomesFuncoesObjetivo.schaffer_2,
                // (int)EnumNomesFuncoesObjetivo.bartels_conn,
                // (int)EnumNomesFuncoesObjetivo.bird,
                // (int)EnumNomesFuncoesObjetivo.bohachevsky_1,
                
                // (int)EnumNomesFuncoesObjetivo.cosine_mixture,
                // (int)EnumNomesFuncoesObjetivo.mccormick,
                // (int)EnumNomesFuncoesObjetivo.alpine01,
                // (int)EnumNomesFuncoesObjetivo.adjiman,
                // (int)EnumNomesFuncoesObjetivo.levy13,
                // (int)EnumNomesFuncoesObjetivo.nova,
                // (int)EnumNomesFuncoesObjetivo.spacecraft,
            };


            // =======================================================================================
            // Define os parâmetros de execução
            ParametrosExecucao parametros_execucao = new ParametrosExecucao();
            parametros_execucao.quantidade_execucoes = 30;
            // parametros_execucao.quantidade_execucoes = 28;
            parametros_execucao.parametros_criterio_parada = new ParametrosCriterioParada()
            {
                // EXECUÇÃO NORMAL
                tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE,
                NFE_criterio_parada = 100000,
                lista_NFEs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000}

                // // PARADA POR ITERAÇÕES
                // tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_ITERATIONS,
                // ITERATIONS_criterio_parada = 500,
                // lista_NFEs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000}

            
                
                // // TUNING FUNÇÕES TESTE
                // tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFE,
                // PRECISAO_criterio_parada = 1e-16,
                // NFE_criterio_parada = 100000,
                // lista_NFEs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000},
                // fx_esperado = 0.0,

                // Lista a cada 500
                // lista_NFEs_desejados = Enumerable.Range(1, 200).Select(i => (i*500)).ToList()

                // f(x) esperado Spacecraft
                // fx_esperado = 196.949433192159
            };
        
            parametros_execucao.o_que_interessa_printar = new OQueInteressaPrintar();
            // parametros_execucao.o_que_interessa_printar.mostrar_header                      = true;
            parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX         = true;
            // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFE                = true;
            // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao   = true;
            // parametros_execucao.o_que_interessa_printar.mostrar_mean_TAU_iteracoes          = true;
            // parametros_execucao.o_que_interessa_printar.mostrar_mean_STD_iteracoes          = true;
            // parametros_execucao.o_que_interessa_printar.mostrar_mean_Mfx_iteracoes          = true;


            // Realiza vários tunings em uma mesma saída
            for(int tuning_cd=0; tuning_cd<1; tuning_cd++){
                // Console.WriteLine("\n\n\n\n\n\n\n");
                // Console.WriteLine("=======================================================");
                // Console.WriteLine("Tuning: {0}", tuning_cd);
                // Console.WriteLine("=======================================================");
                


                // Para cada função, executa os algoritmos...
                foreach (int definicao_funcao_objetivo in function_values)
                {
                    // =======================================================================================
                    // Define os parâmetros do problema
                    ParametrosProblema parametros_problema = get_function_parameters(definicao_funcao_objetivo);



                    Console.WriteLine("\n\n\n\n");
                    Console.WriteLine("============================================================================");
                    Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                    Console.WriteLine("============================================================================");
                    // Após definir os parâmetros do problema e de execução, executa os algoritmos desejados
                    // =======================================================================================








                    // ======================================================================================================
                    // Quais algoritmos executar
                    parametros_execucao.quais_algoritmos_rodar = new QuaisAlgoritmosRodar();
                    // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1 = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2 = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3 = true;
                    parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var = true;
                    parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var = true;

                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_P = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_N = true;

                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_VO = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO = true;

                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_DS = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS = true;

                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO_UNI = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO_UNI = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS_UNI = true;
                    // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS_UNI = true;
                    
                    // O que interessa printar no arquivo de saída
                    parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFE = true;
                    parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;

                    // Executa cada algoritmo por N vezes e obtém todas as execuções
                    List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // Organiza os resultados de todas as excuções por algoritmo
                    List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                    // Apresenta os resultados finais
                    apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                    // ======================================================================================================












                    
                    
                    
                    // // ======================================================================================================
                    // // Tuning do GEOreal2_O_VO
                    // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_std1 = new List<double>(){1, 2, 4, 8};
                    // List<double> valores_P = new List<double>(){4, 8, 16};
                    // List<double> valores_s = new List<double>(){1, 2};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_O_VO(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOreal2_P_VO
                    // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_porcent = new List<double>(){0.1, 1, 10, 50};
                    // List<double> valores_P = new List<double>(){4, 12};
                    // List<double> valores_s = new List<double>(){1, 2};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_P_VO(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOreal2_N_VO                
                    // List<double>valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_std1 = new List<double>(){0.5, 1, 2};
                    // List<double>valores_P = new List<double>(){4, 12};
                    // List<double>valores_s = new List<double>(){1, 2};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_N_VO(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOreal2_O_DS
                    // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_std1 = new List<double>(){1, 5, 10};
                    // List<double> valores_P = new List<double>(){4, 12};
                    // List<double> valores_s = new List<double>(){2, 10};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_O_DS(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOreal2_P_DS
                    // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_porcent = new List<double>(){0.1, 1.0, 5.0, 10, 50};
                    // List<double> valores_P = new List<double>(){4, 12};
                    // List<double> valores_s = new List<double>(){2, 10};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_P_DS(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOreal2_N_DS                
                    // List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    // List<double> valores_std1 = new List<double>(){1, 5, 10};
                    // List<double> valores_P = new List<double>(){4, 8, 16};
                    // List<double> valores_s = new List<double>(){2, 10};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal2_N_DS(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================






                    // if (tuning_cd == 0){
                    //     // ======================================================================================================
                    //     // Tuning do GEOreal2_P_VO_UNI
                    //     List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    //     List<double> valores_porcent = new List<double>(){0.1, 1, 10, 50};
                    //     List<double> valores_P = new List<double>(){4, 12};
                    //     List<double> valores_s = new List<double>(){1, 2};
                        
                    //     List<Tuning> resultados_tuning = tuning_GEOreal2_P_VO_UNI(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                    //     ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    //     // ======================================================================================================
                    // }
                    
                    // else if (tuning_cd == 1){
                    //     // ======================================================================================================
                    //     // Tuning do GEOreal2_N_VO_UNI              
                    //     List<double>valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    //     List<double> valores_std1 = new List<double>(){0.5, 1, 2};
                    //     List<double>valores_P = new List<double>(){4, 12};
                    //     List<double>valores_s = new List<double>(){1, 2};
                        
                    //     List<Tuning> resultados_tuning = tuning_GEOreal2_N_VO_UNI(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    //     ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    //     // ======================================================================================================
                    // }

                    // else if (tuning_cd == 2){
                    //     // ======================================================================================================
                    //     // Tuning do GEOreal2_P_DS_UNI
                    //     List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    //     List<double> valores_porcent = new List<double>(){0.1, 1.0, 10, 50};
                    //     List<double> valores_P = new List<double>(){4, 12};
                    //     List<double> valores_s = new List<double>(){2, 10};
                        
                    //     List<Tuning> resultados_tuning = tuning_GEOreal2_P_DS_UNI(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                    //     ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    //     // ======================================================================================================
                    // }
                    // else if (tuning_cd == 3){
                    //     // ======================================================================================================
                    //     // Tuning do GEOreal2_N_DS_UNI             
                    //     List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                    //     List<double> valores_std1 = new List<double>(){1, 5, 10};
                    //     List<double> valores_P = new List<double>(){4, 12};
                    //     List<double> valores_s = new List<double>(){2, 10};
                        
                    //     List<Tuning> resultados_tuning = tuning_GEOreal2_N_DS_UNI(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                    //     ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    //     // ======================================================================================================
                    // }
                





                    // // ======================================================================================================
                    // // Tuning do GEO
                    // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4.0}; 
                    
                    // List<Tuning> resultados_tuning = tuning_GEO_GEOvar(parametros_execucao, parametros_problema, valores_tau, true);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    // // ======================================================================================================
                    // // Tuning do GEOvar
                    // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4.0, 4.25, 4.5, 4.75, 5}; 
                    
                    // List<Tuning> resultados_tuning = tuning_GEO_GEOvar(parametros_execucao, parametros_problema, valores_tau, false);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                















                    // // ======================================================================================================
                    // // Tuning do GEOreal1 IGOR - parâmetros tau e std
                    
                    // List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 6};
                    // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};

                    // List<Tuning> resultados_tuning = tuning_GEOreal1_O(parametros_execucao, parametros_problema, valores_tau, valores_std);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================

                    

                    // // ======================================================================================================
                    // // Tuning do GEOreal1 PORCENTAGEM - parâmetros tau e porcentagem
                    
                    // List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 5, 6};
                    // List<double> valores_porcentagem = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8};
                    
                    // List<Tuning> resultados_tuning = tuning_GEOreal1_P(parametros_execucao, parametros_problema, valores_tau, valores_porcentagem);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================



                    // // ======================================================================================================
                    // // Tuning do GEOreal1 NORMAL - parâmetros tau e std
                    
                    // List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 5, 6};
                    // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4};

                    // List<Tuning> resultados_tuning = tuning_GEOreal1_N(parametros_execucao, parametros_problema, valores_tau, valores_std);
                    // ordena_e_apresenta_resultados_tuning(resultados_tuning);
                    // // ======================================================================================================








                    
                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ================================== ASGEO e 1/5 rule ==================================================
                    // // ======================================================================================================
                    // // ======================================================================================================
                    
                    // // ====================================================================================
                    // // ASGEO2 REAL2 1
                    
                    /////// parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;

                    // // parametros_execucao.o_que_interessa_printar.mostrar_header = false;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX = true;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFE = true;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;

                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_P = 5;
                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_std1 = 10;
                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_s = 10;


                    // // List<int> valores_P = new List<int>(){4,8,12,16};
                    // // List<double> valores_p1 = new List<double>(){5, 10, 50, 100};
                    // // List<int> valores_s = new List<int>(){2, 10};

                    // // List<int> valores_P = new List<int>(){5};
                    // // List<double> valores_p1 = new List<double>(){10};
                    // // List<int> valores_s = new List<int>(){10};

                    // // BOM
                    // // List<int> valores_P = new List<int>(){8};
                    // // List<double> valores_p1 = new List<double>(){10};
                    // // List<int> valores_s = new List<int>(){2};

                    // List<int> valores_P = new List<int>(){4, 6, 8, 12};
                    // List<double> valores_p1 = new List<double>(){10, 20, 50};
                    // List<int> valores_s = new List<int>(){2, 10};
                    
                    // foreach (int P in valores_P){
                    //     foreach (double p1 in valores_p1){
                    //         foreach (int s in valores_s){
                    //             Console.WriteLine("=====================================================");
                    //             Console.WriteLine("P = {0} | p1 = {1} | s = {2}", P, p1, s);
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_P = P;
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_std1 = p1;
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_s = s;
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
                    // // ASGEO2 REAL2 1 - PERTURBAÇÃO ORIGINAL
                    // // ====================================================================================

                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_P = 5;
                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_std1 = 10;
                    // // parametros_problema.parametros_livres.ASGEO2_REAL2_1_s = 10;

                    // // List<int> valores_P = new List<int>(){4,8,12,16};
                    // List<int> valores_P = new List<int>(){2,4,8};
                    // List<double> valores_std1 = new List<double>(){1,2,3};
                    // // List<double> valores_std1 = new List<double>(){10};
                    // List<int> valores_s = new List<int>(){2,5,10};
                    // // List<int> valores_s = new List<int>(){10};
                    
                    // foreach (int P in valores_P){
                    //     foreach (double std1 in valores_std1){
                    //         foreach (int s in valores_s){
                    //             Console.WriteLine("=====================================================");
                    //             Console.WriteLine("P = {0} | std1 = {1} | s = {2}", P, std1, s);
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_P = P;
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_std1 = std1;
                    //             parametros_problema.parametros_livres.ASGEO2_REAL2_1_s = s;
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
                    // // Execução 1/5 rule tunado
                    // // ====================================================================================
                    
                    /////// parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;

                    // parametros_problema.parametros_livres.stdmin_one_fifth_rule = 0.2; 
                    // parametros_problema.parametros_livres.q_one_fifth_rule = 200; 
                    // parametros_problema.parametros_livres.c_one_fifth_rule = 0.9;

                    // // parametros_execucao.o_que_interessa_printar.mostrar_header = false;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX = true;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFE = true;
                    // // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;
                
                    // // Executa cada algoritmo por N vezes e obtém todas as execuções
                    // List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                    
                    // // Organiza os resultados de todas as excuções por algoritmo
                    // List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                    
                    // // Apresenta os resultados finais
                    // apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);

                    

                    // // ====================================================================================
                    // // Tuning do c e q para 1/5 rule dos A-GEOsreal1
                    // // ====================================================================================

                    /////// parametros_execucao.tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_SDdireto;
                    
                    // // Executa o algoritmo variando o std
                    // List<double> valores_stdmin = new List<double>(){0.1, 0.2, 0.5, 1};
                    // // List<int> valores_q = new List<int>(){50, 100, 200, 500};
                    // List<int> valores_q = new List<int>(){100, 200, 500};
                    // // List<double> valores_c = new List<double>(){0.85, 0.9, 0.95};
                    // List<double> valores_c = new List<double>(){0.9, 0.95};
                    
                    // // Itera cada std e cada tau
                    // foreach (double stdmin in valores_stdmin){
                    //     foreach (int q in valores_q){
                    //         foreach (double c in valores_c){
                    //             parametros_problema.parametros_livres.stdmin_one_fifth_rule = stdmin; 
                    //             parametros_problema.parametros_livres.q_one_fifth_rule = q; 
                    //             parametros_problema.parametros_livres.c_one_fifth_rule = c; 

                    //             Console.WriteLine("========================================================");
                    //             Console.WriteLine("stdmin = {0} | q = {1} | c = {2}", stdmin, q, c);

                    //             // Executa cada algoritmo por N vezes e obtém todas as execuções
                    //             List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                                
                    //             // Organiza os resultados de todas as excuções por algoritmo
                    //             List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                                
                    //             // Apresenta os resultados finais
                    //             apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                    //         }
                    //     }
                    // }
                    
                    
                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ======================================================================================================



                    
                    








                    // // ======================================================================================================
                    // // ============================== A-GEOs REAL1 igor e porcentagem =======================================
                    // // ======================================================================================================


                    // // ====================================================================================
                    // // Tuning do std para os A-GEOsreal1

                    // List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};
                    // // Itera cada std
                    // foreach (double std in valores_std){
                    //     parametros_problema.parametros_livres.std_AGEO1real1 = std;
                    //     parametros_problema.parametros_livres.std_AGEO2real1 = std;
                        
                    //     Console.WriteLine("========================================================");
                    //     Console.WriteLine("std = {0}", std);

                    //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                    //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                        
                    //     // Organiza os resultados de todas as excuções por algoritmo
                    //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                        
                    //     // Apresenta os resultados finais
                    //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                    // }
                    // // ====================================================================================



                    // // ====================================================================================
                    // // Tuning da porcentagem nos A-GEO real1 com poerturbação porcentagem

                    // // Executa o algoritmo variando a porcentagem do intervalo
                    // List<double> valores_porcentagem = new List<double>(){0.5,1,1.5,2,2.5,3,3.5,4,4.5,5,6,7,8,9,10};
                    // // List<double> valores_porcentagem = new List<double>(){0.2,0.4,0.6,0.8,1.0,1.2,1.4,1.6,1.8,2.0,2.2,2.4,2.6,2.8,3.0,3.2,3.4,3.6,3.8,4.0,4.2,4.4,4.6,4.8,5.0};
                    // // List<double> valores_porcentagem = new List<double>(){5.2,5.4,5.6,5.8,6.0,6.2,6.4,6.6,6.8,7.0,7.2,7.4,7.6,7.8,8.0,8.2,8.4,8.6,8.8,9.0,9.2,9.4,9.6,9.8,10.0};
                    
                    // // Itera cada porcentagem
                    // foreach (double porcentagem in valores_porcentagem){
                    //     parametros_problema.parametros_livres.p1_AGEO1real1 = porcentagem;
                    //     parametros_problema.parametros_livres.p1_AGEO2real1 = porcentagem;
                        
                    //     Console.WriteLine("========================================================");
                    //     Console.WriteLine("porcentagem = {0}", porcentagem);

                    //     // Executa cada algoritmo por N vezes e obtém todas as execuções
                    //     List<RetornoGEOs> todas_execucoes_algoritmos = executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                        
                    //     // Organiza os resultados de todas as excuções por algoritmo
                    //     List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                        
                    //     // Apresenta os resultados finais
                    //     apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                    // }
                    // // ====================================================================================

                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ======================================================================================================


                




                    // // ======================================================================================================
                    // // ============================== A-GEOs REAL2 igor =====================================================
                    // // ======================================================================================================
                    

                    // // ====================================================================================
                    // // Tuning do std para os A-GEOs REAL2

                    // List<int> valores_s = new List<int>(){1, 2};
                    // List<int> valores_P = new List<int>(){4, 8, 16};
                    // List<double> valores_std1 = new List<double>(){1, 2, 4, 8};
                    
                    // // Realiza todas as combinações possíveis
                    // foreach (int P in valores_P){
                    //     foreach (int s in valores_s){
                    //         foreach (double std in valores_std1){
                    //             parametros_problema.parametros_livres.std_AGEO1real2 = std;
                    //             parametros_problema.parametros_livres.std_AGEO2real2 = std;
                    //             parametros_problema.parametros_livres.P_AGEO1real2 = P;
                    //             parametros_problema.parametros_livres.P_AGEO2real2 = P;
                    //             parametros_problema.parametros_livres.s_AGEO1real2 = s;
                    //             parametros_problema.parametros_livres.s_AGEO2real2 = s;

                    //             Console.WriteLine("=====================================================");
                    //             Console.WriteLine("P = {0} | s = {1} | std1 = {2}", P, s, std);

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

                    // // ======================================================================================================
                    // // ======================================================================================================
                    // // ======================================================================================================





                
                    



                    
                    
                }
            }
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
                string filename = "./SaidaRedirect.txt";

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
                    Console.WriteLine ("Cannot open SaidaRedirect.txt for writing");
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

            // ex.ExtensiveSearch_SpacecraftOptimization();
            // ex.Teste_FuncoesObjetivo_SpacecraftOptimization();

            watch.Stop();
            Console.WriteLine($"\n\nExecution Time: {watch.ElapsedMilliseconds} ms");





            // ============================================================================
            // ============================================================================
            // Volta para o normal o console
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