


#define SCIENTIFIC_STRING_FORMAT
// #define DECIMAL_STRING_FORMAT


using System;
using System.Collections.Generic;
using System.Linq;
using GEOs_REAIS;
using GEOs_BINARIOS;
using Classes_e_Enums;


namespace ExecutaOrganizaApresenta
{
    public class ExecutaOrganizaApresenta {
       
        public static List<Retorno_N_Execucoes_GEOs> organiza_os_resultados_de_cada_execucao(List<RetornoGEOs> todas_execucoes, ParametrosExecucao parametros_execucao)
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
                // Lista que irá conter os f(x) atual por NFE em cada execução
                List<List<double>> stats_FX_atual_por_NFE = new List<List<double>>();   
                // Lista que irá conter os tau por iteração em cada execução
                List<List<double>> stats_TAU_per_iteration = new List<List<double>>();
                // Lista que irá conter os std/porcentagens por iteração em cada execução
                List<List<double>> stats_STDPORC_per_iteration = new List<List<double>>();
                // Lista que irá conter os melhores f(x) por iteração em cada execução
                List<List<double>> stats_Mfx_per_iteration = new List<List<double>>();
                // Lista utilizada para armazenar o NFE de cada execução para posterior média
                List<int> NFEs_para_posterior_media = new List<int>();
                // Lista utilizada para armazenar a qtde. iterações de cada execução para posterior média
                List<int> ITEs_para_posterior_media = new List<int>();
                // Lista utilizada para armazenar o melhor f(x) de cada exeução para posterior média
                List<double> TodosValoresFinaisDeFX = new List<double>();
                // Lista utilizada para armazenar o f(x) atual de cada exeução para posterior média
                List<double> FXsAtual_para_posterior_media = new List<double>();
                
                
                // Para cada execução, vai acumulando os resultados
                for (int j=0; j<execucoes_algoritmo_executado.Count; j++)
                {
                    // Obtém o retorno do algoritmo naquela execução
                    RetornoGEOs ret = execucoes_algoritmo_executado[j];



                    // if (ret.melhor_fx > 1E+300){
                    //     continue;
                    // }


                    
                    // Armazena o melhor f(x) obtido na execução
                    TodosValoresFinaisDeFX.Add(ret.melhor_fx);
                    // Armazena o f(x) da população atual
                    FXsAtual_para_posterior_media.Add(ret.melhor_fx);
                    // Armazena a lista contendo os fxs em cada NFE nessa execução
                    stats_melhorFX_por_NFE.Add( ret.melhores_NFEs );
                    // Armazena a lista contendo os fxs atual em cada NFE nessa execução
                    stats_FX_atual_por_NFE.Add( ret.fxs_atuais_NFEs );
                    // Armazena o NFE final obtido na execução
                    NFEs_para_posterior_media.Add(ret.NFE);
                    // Armazena o nro. de iterações final obtido na execução
                    ITEs_para_posterior_media.Add(ret.iteracoes);
                    // Armazena a lista contendo os tau em cada iteração nessa execução
                    stats_TAU_per_iteration.Add(ret.stats_TAU_per_iteration);
                    // Armazena a lista contendo os std/porcentagens em cada iteração nessa execução
                    stats_STDPORC_per_iteration.Add(ret.stats_STDPORC_per_iteration);
                    // Armazena a lista contendo os f(x) em cada iteração nessa execução
                    stats_Mfx_per_iteration.Add(ret.stats_Mfx_per_iteration);
                }

                // Calcula a média de NFEs com base em todas execuções
                int media_NFEs = (int) NFEs_para_posterior_media.Average();
                // Calcula a média de qtde de iterações com base em todas execuções
                int media_iteracoes = (int) ITEs_para_posterior_media.Average();
                // Calcula a média de melhor f(x) com base em todas execuções
                double media_melhor_fx = (double) TodosValoresFinaisDeFX.Average();
                // Calcula o menor (melhor) valor de f(x) com base em todas execuções
                double melhor_fx_de_todos = (double) TodosValoresFinaisDeFX.Min();
                // Calcula o pior (maior) valor de f(x) com base em todas execuções
                double pior_fx_de_todos = (double) TodosValoresFinaisDeFX.Max();
                // Calcula a mediana de melhor f(x) com base em todas execuções
                List<double> lista = new List<double>(TodosValoresFinaisDeFX);
                lista.Sort(delegate(double x, double y){return x.CompareTo(y);});
		        double mediana_melhor_fx = lista[(lista.Count / 2)];
                // Calcula o desvio padrão final com base nos melhores f(x) das execuções
                double somatorio_sd = 0;
                foreach (double melhor_fx in TodosValoresFinaisDeFX)
                {
                    somatorio_sd += Math.Pow((melhor_fx - media_melhor_fx), 2);
                }
                int n = TodosValoresFinaisDeFX.Count - 1;
                double SD_melhor_fx = Math.Sqrt(somatorio_sd / n);


                // Listas com os resultados finais
                //-------------------------------------------------------------------------------------------
                // Lista irá conter o tau médio para cada iteração. Essa lista só poderá
                // ...ser utilizada quando o critério de parada for por número de iterações, visto que só
                // ...assim que todasas execuções terão a mesma quantidade de iterações
                List<double> lista_TAU_medio_per_iteration = new List<double>();
                // Lista irá conter o std/porcentagem média para cada iteração. Essa lista só poderá
                // ...ser utilizada quando o critério de parada for por número de iterações, visto que só
                // ...assim que todasas execuções terão a mesma quantidade de iterações
                List<double> lista_STDPORC_medio_per_iteration = new List<double>();
                // Lista irá conter o melhor f(x) médio para cada iteração. Essa lista só poderá
                // ...ser utilizada quando o critério de parada for por número de iterações, visto que só
                // ...assim que todasas execuções terão a mesma quantidade de iterações
                List<double> lista_Mfx_medio_per_iteration = new List<double>();
                // Lista irá conter o f(x) médio para cada NFE desejado. Essa lista só poderá
                // ...ser utilizada quando o critério de parada por por NFE, visto que só assim
                // ...todas as execuções terão a mesma quantidade de f(x) por NFE
                List<double> lista_MelhoresFX_por_NFE = new List<double>();
                // Lista irá conter o f(x) atual para cada NFE desejado. Essa lista só poderá
                // ...ser utilizada quando o critério de parada por por NFE, visto que só assim
                // ...todas as execuções terão a mesma quantidade de f(x) por NFE
                List<double> lista_FXatual_por_NFE = new List<double>();


                // Se o critério de parada for por NFE, calcula o f(x) médio para cada NFE desejado
                if (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFE)
                {
                    // Obtém a quantidade de NFEs desejados
                    int quantidade_NFEs = parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados.Count;
                    
                    // Para cada NFE, percorre as execuções pra fazer a média do melhor fx
                    for(int u=0; u<quantidade_NFEs; u++)
                    {
                        List<double> fxs_no_NFE_desejado = new List<double>();
                        List<double> fx_atual_no_NFE_desejado = new List<double>();
                        
                        for(int o=0; o<parametros_execucao.quantidade_execucoes; o++)
                        {
                            // Adiciona o valor do melhor FX de cada execução naquele NFE
                            fxs_no_NFE_desejado.Add( stats_melhorFX_por_NFE[o][u] );
                            // Adiciona o valor do atual FX de cada execução naquele NFE
                            fx_atual_no_NFE_desejado.Add( stats_FX_atual_por_NFE[o][u] );
                        }
                    
                        // Gera a média d melhor f(x) naquele NFE
                        double avg_naquele_NFE = fxs_no_NFE_desejado.Average();
                        // Gera a média do f(x) atual naquele NFE
                        double avg_fx_atual_naquele_NFE = fx_atual_no_NFE_desejado.Average();
                        
                        
                        // Adiciona essa média na lista de fxs médios
                        lista_MelhoresFX_por_NFE.Add(avg_naquele_NFE);
                        lista_FXatual_por_NFE.Add(avg_fx_atual_naquele_NFE);
                    }
                }


                // Se o critério de parada for por ITERAÇÕES, calcula o tau médio e o f(x) médio em cada iteração
                if ((parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_ITERATIONS) || 
                (parametros_execucao.parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFE))
                {
                    // Computa os TAU e Mfx médio por cada iteração
                    for(int it=0; it<media_iteracoes; it++)
                    {
                        double sum_TAU = 0;
                        double sum_STDPORC = 0;
                        double sum_Mfx = 0;

                        for(int execs=0; execs<stats_TAU_per_iteration.Count; execs++)
                        {
                            sum_TAU += stats_TAU_per_iteration[execs][it];
                            sum_STDPORC += stats_STDPORC_per_iteration[execs][it];
                            sum_Mfx += stats_Mfx_per_iteration[execs][it];
                        }

                        double media1 = sum_TAU / stats_TAU_per_iteration.Count;
                        lista_TAU_medio_per_iteration.Add(media1);

                        double media2 = sum_STDPORC / stats_STDPORC_per_iteration.Count;
                        lista_STDPORC_medio_per_iteration.Add(media2);

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
                media_das_execucoes.pior_fx_de_todos = pior_fx_de_todos;
                media_das_execucoes.melhor_fx_de_todos = melhor_fx_de_todos;
                media_das_execucoes.mediana_melhor_fx = mediana_melhor_fx;
                media_das_execucoes.SD_do_melhor_fx = SD_melhor_fx;
                media_das_execucoes.media_valor_FO_em_cada_NFE = lista_MelhoresFX_por_NFE;
                media_das_execucoes.media_fx_atual_em_cada_NFE = lista_FXatual_por_NFE;
                media_das_execucoes.lista_melhores_fxs = TodosValoresFinaisDeFX;
                media_das_execucoes.lista_TAU_medio_per_iteration = lista_TAU_medio_per_iteration;  
                media_das_execucoes.lista_STDPORC_medio_per_iteration = lista_STDPORC_medio_per_iteration;  
                media_das_execucoes.lista_Mfx_medio_per_iteration = lista_Mfx_medio_per_iteration;  

                // Adiciona essa estrutura na lista geral que contém uma estatística por algoritmo
                estatisticas_algoritmos.Add(media_das_execucoes);
            }

            return estatisticas_algoritmos;
        }


        // Essa função tem por objetivo apresentar as estatísticas das execuções por algoritmo. Aqui, as 
        // ...informações são apresentadas na tela, como melhor valor da função médio obtido 
        public static void apresenta_resultados_finais(OQueInteressaPrintar o_que_interessa_printar, List<Retorno_N_Execucoes_GEOs> estatisticas_algoritmos, ParametrosExecucao parametros_execucao, ParametrosProblema parametros_problema, int scientific_or_decimal_str_format)
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
                Console.WriteLine(String.Format("Função objetivo utilizada: {0} - {1}", parametros_problema.function_id, parametros_problema.nome_funcao));
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
                string media_de_fx_nas_execucoes = "meanFX;";
                string melhor_fx_nas_execucoes = "melhorFX;";
                string pior_fx_nas_execucoes = "piorFX;";
                string mediana_de_fx_nas_execucoes = "medianFX;";
                string sd_dos_fx_finais_nas_execucoes = "sdFX;";

                // Obtém os valores de cada algoritmo
                for (int i=0; i<estatisticas_algoritmos.Count; i++)
                {
                    int media_NFE           = estatisticas_algoritmos[i].NFE_medio;
                    double media_fx         = estatisticas_algoritmos[i].media_melhor_fx;
                    double melhor_fx        = estatisticas_algoritmos[i].melhor_fx_de_todos;
                    double pior_fx          = estatisticas_algoritmos[i].pior_fx_de_todos;
                    double mediana_fx       = estatisticas_algoritmos[i].mediana_melhor_fx;
                    double sd_melhores_fx   = estatisticas_algoritmos[i].SD_do_melhor_fx;

                    // Seta formato vazio para decimal
                    string StrFormat = "";
                    if (scientific_or_decimal_str_format == 0)
                        // Seta formato scientific
                        StrFormat = "0.00E+0";


                    // string StrFormat = "";
                    media_do_NFE_atingido_nas_execucoes += media_NFE.ToString(StrFormat) + ';';
                    media_de_fx_nas_execucoes           += media_fx.ToString(StrFormat) + ';';
                    melhor_fx_nas_execucoes             += melhor_fx.ToString(StrFormat) + ';';
                    pior_fx_nas_execucoes               += pior_fx.ToString(StrFormat) + ';';
                    mediana_de_fx_nas_execucoes         += mediana_fx.ToString(StrFormat) + ';';
                    sd_dos_fx_finais_nas_execucoes      += sd_melhores_fx.ToString(StrFormat) + ';';
                }

                // Substitui os pontos por vírgulas e printa
                resultados_finais += media_do_NFE_atingido_nas_execucoes + '\n';
                resultados_finais += media_de_fx_nas_execucoes + '\n';
                resultados_finais += melhor_fx_nas_execucoes + '\n';
                resultados_finais += pior_fx_nas_execucoes + '\n';
                resultados_finais += mediana_de_fx_nas_execucoes + '\n';
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


            // Se desejado, apresenta o f(x) atual obtido em cada NFE
            if (o_que_interessa_printar.mostrar_fxs_atual_por_NFE)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> f(x) atual para cada NFE:");
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
                        // // Obtém o valor do f(x) médio naquele NFE para esse algoritmo
                        double fx_atual_naquele_NFE = estatisticas_algoritmos[j].media_fx_atual_em_cada_NFE[i];
                        // Concatena
                        fxs_string += fx_atual_naquele_NFE.ToString() + ';';
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
            // Somente usado quando o critério de parada for por qtde de iterações ou por NFE máximo
            if (o_que_interessa_printar.mostrar_mean_TAU_iteracoes)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> TAU para cada iteração:");
                Console.WriteLine("iteracao;" + string_algoritmos_executados);
                
                int quantidade_iteracoes = estatisticas_algoritmos[0].lista_TAU_medio_per_iteration.Count;
                
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


            // Se desejado, apresenta os tau obtidos por iteração
            // Somente usado quando o critério de parada for por qtde de iterações ou por NFE máximo
            if (o_que_interessa_printar.mostrar_mean_STDPORC_iteracoes)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> STDPORC para cada iteração:");
                Console.WriteLine("iteracao;" + string_algoritmos_executados);
                
                int quantidade_iteracoes = estatisticas_algoritmos[0].lista_STDPORC_medio_per_iteration.Count;
                
                // Concatena o std/porcentagem por iteração para cada algoritmo executado
                for (int i=0; i<quantidade_iteracoes; i++)
                {    
                    string iteracao_string = (i+1).ToString();
                    string STDPORCs_naquela_iteracao = iteracao_string + ';';
                    
                    // Para cada algoritmo, concatena o std/porcentagem por iteração
                    for (int j=0; j<estatisticas_algoritmos.Count; j++)
                    {
                        double STDPORC_naquela_iteracao = estatisticas_algoritmos[j].lista_STDPORC_medio_per_iteration[i];
                        STDPORCs_naquela_iteracao += STDPORC_naquela_iteracao.ToString() + ';';
                    }
                    Console.WriteLine( STDPORCs_naquela_iteracao.Replace('.',',') );
                }
                Console.WriteLine("");
            }


            // Se desejado, apresenta os f(x) obtidos por iteração
            // Somente usado quando o critério de parada for por qtde de iterações ou por NFE máximo
            if (o_que_interessa_printar.mostrar_mean_Mfx_iteracoes)
            {
                Console.WriteLine("==========================================================");
                Console.WriteLine("===> Mfx para cada iteração:");
                Console.WriteLine("iteracao;" + string_algoritmos_executados);
                
                int quantidade_iteracoes = estatisticas_algoritmos[0].lista_Mfx_medio_per_iteration.Count;
                
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
                Random r = new Random();
                int seed = r.Next();

                // Para cada execução, gera uma nova população inicial com base nos limites de cada variável
                
                // População Real
                List<double> populacao_real_gerada = GeracaoPopulacoes.GeracaoPopulacoes.geracao_populacao_real(parametros_problema.lower_bounds, parametros_problema.upper_bounds, seed);
                parametros_problema.populacao_inicial_real = new List<double>(populacao_real_gerada);

                // População Binária
                List<bool> populacao_binaria_gerada = GeracaoPopulacoes.GeracaoPopulacoes.geracao_populacao_binaria(parametros_problema.bits_por_variavel, seed);
                parametros_problema.populacao_inicial_binaria = new List<bool>(populacao_binaria_gerada);

                // ---------------------------------------------------------------------------
                
                // GEO
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEO)
                {
                    GEO_BINARIO geo = new GEO_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        parametros_problema.parametros_livres.GEO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
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
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        parametros_problema.parametros_livres.GEOvar__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo_var.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var;
                    
                    todas_execucoes.Add(ret);
                }






                // GEOvar2
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar2)
                {
                    GEOvar2_BINARIO geo_var2 = new GEOvar2_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        parametros_problema.parametros_livres.GEOvar__tau, 
                        // parametros_problema.parametros_livres.GEOvar2__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds, 
                        parametros_problema.upper_bounds, 
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = geo_var2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var2;
                    
                    todas_execucoes.Add(ret);
                }






                // A-GEO1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1)
                {
                    AGEOs_BINARIO ageo1 = new AGEOs_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
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
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
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
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        3, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO3;
                        
                    todas_execucoes.Add(ret);
                }

                // A-GEO4
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4)
                {
                    AGEOs_BINARIO ageo4 = new AGEOs_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        4, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo4.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO4;
                        
                    todas_execucoes.Add(ret);
                }

                // A-GEO9
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9)
                {
                    AGEOs_BINARIO ageo9 = new AGEOs_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        9, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret2 = ageo9.executar(parametros_execucao.parametros_criterio_parada);
                    ret2.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO9;
                        
                    todas_execucoes.Add(ret2);
                }




                // AGEO1var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var)
                {
                    AGEOsvar_BINARIO ageo1_var = new AGEOsvar_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
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
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = ageo2_var.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var;
                        
                    todas_execucoes.Add(ret);
                }


                // AGEO3var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3var)
                {
                    AGEOsvar_BINARIO alg = new AGEOsvar_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        3, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO3var;
                        
                    todas_execucoes.Add(ret);
                }


                // AGEO4var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4var)
                {
                    AGEOsvar_BINARIO alg = new AGEOsvar_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        4, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO4var;
                        
                    todas_execucoes.Add(ret);
                }


                // AGEO9var
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9var)
                {
                    AGEOsvar_BINARIO alg = new AGEOsvar_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        9, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada); 
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO9var;
                        
                    todas_execucoes.Add(ret);
                }






                // AGEO1var_1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_1)
                {
                    double tau_maior_que = 1;
                    
                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_1;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var_3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_3)
                {
                    double tau_maior_que = 3;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_3;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var_5
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_5)
                {
                    double tau_maior_que = 5;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_5;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var_7
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_7)
                {
                    double tau_maior_que = 7;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_7;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var_9
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_9)
                {
                    double tau_maior_que = 9;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_9;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO1var_11
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_11)
                {
                    double tau_maior_que = 11;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        1, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1var_11;
                        
                    todas_execucoes.Add(ret);
                }






                // AGEO2var_1
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_1)
                {
                    double tau_maior_que = 1;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_1;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var_3
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_3)
                {
                    double tau_maior_que = 3;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_3;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var_5
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_5)
                {
                    double tau_maior_que = 5;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_5;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var_7
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_7)
                {
                    double tau_maior_que = 7;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_7;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var_9
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_9)
                {
                    double tau_maior_que = 9;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_9;
                        
                    todas_execucoes.Add(ret);
                }

                // AGEO2var_11
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_11)
                {
                    double tau_maior_que = 11;

                    AGEOvar_novo_BINARIO alg = new AGEOvar_novo_BINARIO(
                        new List<bool>(parametros_problema.populacao_inicial_binaria),
                        2, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.bits_por_variavel,
                        tau_maior_que);

                    RetornoGEOs ret = alg.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2var_11;
                        
                    todas_execucoes.Add(ret);
                }






                // // GEOreal1 - IGOR
                // if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_igor)
                // {
                //     int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                //     GEO_real1 geo_real1 = new GEO_real1(
                //         parametros_problema.n_variaveis_projeto, 
                //         parametros_problema.function_id, 
                //         parametros_problema.populacao_inicial_real, 
                //         parametros_problema.lower_bounds,
                //         parametros_problema.upper_bounds,
                //         parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                //         tipo_perturbacao,
                //         parametros_problema.parametros_livres.tau_GEOreal1, 
                //         parametros_problema.parametros_livres.std_GEOreal1);
                        
                //     RetornoGEOs ret = geo_real1.executar(parametros_execucao.parametros_criterio_parada);
                //     ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal1_igor;

                //     todas_execucoes.Add(ret);
                // }

                // // AGEO1real1 - IGOR
                // if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1real1_igor)
                // {
                //     int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                //     AGEOs_REAL1 AGEO1real1 = new AGEOs_REAL1(
                //         parametros_problema.populacao_inicial_real, 
                //         parametros_problema.n_variaveis_projeto, 
                //         parametros_problema.function_id, 
                //         parametros_problema.lower_bounds,
                //         parametros_problema.upper_bounds,
                //         parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                //         parametros_problema.parametros_livres.std_AGEO1real1, 
                //         1, 
                //         tipo_perturbacao);
                        
                //     RetornoGEOs ret = AGEO1real1.executar(parametros_execucao.parametros_criterio_parada);
                //     ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO1real1_igor;

                //     todas_execucoes.Add(ret);
                // }

                // // AGEO2real1 - IGOR
                // if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_igor)
                // {
                //     int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                //     AGEOs_REAL1 AGEO2real1 = new AGEOs_REAL1(
                //         parametros_problema.populacao_inicial_real, 
                //         parametros_problema.n_variaveis_projeto, 
                //         parametros_problema.function_id, 
                //         parametros_problema.lower_bounds,
                //         parametros_problema.upper_bounds,
                //         parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                //         parametros_problema.parametros_livres.std_AGEO2real1, 
                //         2, 
                //         tipo_perturbacao);
                        
                //     RetornoGEOs ret = AGEO2real1.executar(parametros_execucao.parametros_criterio_parada);
                //     ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real1_igor;
                        
                //     todas_execucoes.Add(ret);
                // }





                // GEOreal1_O  =  GEOreal1 + PERTURBAÇÃO IGOR
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;

                    GEO_real1 geo_real1 = new GEO_real1(
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
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
                        parametros_problema.function_id, 
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
                        parametros_problema.function_id, 
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_O_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_igor;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_O_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
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
                    bool primeira_das_P_perturbacoes_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_VO__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_VO__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_VO_UNI;
                        
                    todas_execucoes.Add(ret);
                }
                
                // GEOreal2_N_VO_UNI  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO ORIGINAL + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO_UNI)
                {
                    bool primeira_das_P_perturbacoes_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_VO__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_VO__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_VO__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_VO_UNI;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_P_DS_UNI  =  GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO DIVIDE POR S + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS_UNI)
                {
                    bool primeira_das_P_perturbacoes_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_P_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_P_DS__porc, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_P_DS__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_P_DS_UNI;
                        
                    todas_execucoes.Add(ret);
                }

                // GEOreal2_N_DS_UNI  =  GEOreal2 + PERTURBAÇÃO NORMAL + VARIAÇÃO DIVIDE POR S + 1 Uniforme
                if (parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS_UNI)
                {
                    bool primeira_das_P_perturbacoes_uniforme = true;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_normal;
                    
                    GEO_real2 geo_real2 = new GEO_real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.parametros_livres.GEOreal2_N_DS__tau, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.GEOreal2_N_DS__std, 
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS__P, 
                        (int)parametros_problema.parametros_livres.GEOreal2_N_DS__s);
                        
                    RetornoGEOs ret = geo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEOreal2_N_DS_UNI;
                        
                    todas_execucoes.Add(ret);
                }


                
                // ----------------------------------------------------------------
                // tau adaptativo
                // ----------------------------------------------------------------

                // AGEOreal1_P  =  Adaptativo + GEOreal1 + PERTURBAÇÃO PORCENTAGEM
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_P)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    AGEO2real1 AGEOreal1_P = new AGEO2real1(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.AGEO2real1_P__porc,
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEOreal1_P.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEOreal1_P;

                    todas_execucoes.Add(ret);
                }
                

                // A-GEOreal2_P_DS  =  Adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO DIVIDE POR S
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_DS)
                {
                    bool primeira_das_P_perturbacoes_uniforme = false;
                    int tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    AGEO2real2 ageo_real2 = new AGEO2real2(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        primeira_das_P_perturbacoes_uniforme,
                        tipo_variacao_std_nas_P_perturbacoes,
                        parametros_problema.parametros_livres.AGEO2real2_P_DS__porc,
                        tipo_perturbacao, 
                        (int)parametros_problema.parametros_livres.AGEO2real2_P_DS__P, 
                        (int)parametros_problema.parametros_livres.AGEO2real2_P_DS__s);
                        
                    RetornoGEOs ret = ageo_real2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEOreal2_P_DS;
                        
                    todas_execucoes.Add(ret);
                }



                // ----------------------------------------------------------------
                // FIXO
                // ----------------------------------------------------------------

                // AGEO2real2_P_DS_fixo  =  Adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + VARIAÇÃO DIVIDE POR S
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_DS_fixo)
                {
                    AGEO2real2_P_DS_fixo AGEO2real2_P_DS_fixo = new AGEO2real2_P_DS_fixo(
                        parametros_problema.populacao_inicial_real, 
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_DS_fixo.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_DS_fixo;
                        
                    todas_execucoes.Add(ret);
                }



                // ----------------------------------------------------------------
                // Auto-Adaptativo
                // ----------------------------------------------------------------
                
                // AGEOreal1_P_AA  =  tau adaptativo + GEOreal1 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_P_AA)
                {
                    int tipo_perturbacao = (int)EnumTipoPerturbacao.perturbacao_porcentagem;
                    
                    AGEO2real1_P_AA AGEO2real1_P_AA = new AGEO2real1_P_AA(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados, 
                        parametros_problema.parametros_livres.AGEO2real1_P__porc,
                        tipo_perturbacao);
                        
                    RetornoGEOs ret = AGEO2real1_P_AA.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real1_P_AA;

                    todas_execucoes.Add(ret);
                }


                // AGEOreal2_P_AA_p0  =  tau adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p0)
                {
                    AGEO2real2_P_AA_p0 AGEO2real2_P_AA_p0 = new AGEO2real2_P_AA_p0(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_AA_p0.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_AA_p0;

                    todas_execucoes.Add(ret);
                }
                
                
                // AGEOreal2_P_AA_p1  =  tau adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p1)
                {
                    AGEO2real2_P_AA_p1 AGEO2real2_P_AA_p1 = new AGEO2real2_P_AA_p1(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_AA_p1.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_AA_p1;

                    todas_execucoes.Add(ret);
                }


                // AGEOreal2_P_AA_p2  =  tau adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p2)
                {
                    AGEO2real2_P_AA_p2 AGEO2real2_P_AA_p2 = new AGEO2real2_P_AA_p2(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_AA_p2.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_AA_p2;

                    todas_execucoes.Add(ret);
                }


                // AGEOreal2_P_AA_p 3  =  tau adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p3)
                {
                    AGEO2real2_P_AA_p3 AGEO2real2_P_AA_p3 = new AGEO2real2_P_AA_p3(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_AA_p3.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_AA_p3;

                    todas_execucoes.Add(ret);
                }


                // AGEOreal2_P_AA_p9  =  tau adaptativo + GEOreal2 + PERTURBAÇÃO PORCENTAGEM + porcent autoadaptativo
                if (parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p9)
                {
                    AGEO2real2_P_AA_p9 AGEO2real2_P_AA_p9 = new AGEO2real2_P_AA_p9(
                        parametros_problema.populacao_inicial_real,
                        parametros_problema.n_variaveis_projeto, 
                        parametros_problema.function_id, 
                        parametros_problema.lower_bounds,
                        parametros_problema.upper_bounds,
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados);
                        
                    RetornoGEOs ret = AGEO2real2_P_AA_p9.executar(parametros_execucao.parametros_criterio_parada);
                    ret.algoritmo_utilizado = (int)EnumNomesAlgoritmos.AGEO2real2_P_AA_p9;

                    todas_execucoes.Add(ret);
                }
            }

            // Retorna a lista com todas as N execuções para os algoritmos a serem executados
            return todas_execucoes;
        }
        
    }
}