
#define CONSOLE_OUT_FILE

using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.IO;

namespace Execucoes
{
    public class Execucoes_GEO
    {
        public void Execucoes()
        {
            // Cria uma lista contendo as funções a serem executadas
            List<int> function_values = new List<int>()
            {
                // (int)EnumNomesFuncoesObjetivo.griewangk,
                // (int)EnumNomesFuncoesObjetivo.rastringin,
                // (int)EnumNomesFuncoesObjetivo.rosenbrock,
                // (int)EnumNomesFuncoesObjetivo.schwefel,
                (int)EnumNomesFuncoesObjetivo.ackley,
                // (int)EnumNomesFuncoesObjetivo.beale,
                
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
            parametros_execucao.quantidade_execucoes = 40;
            // parametros_execucao.quantidade_execucoes = 28;
            parametros_execucao.parametros_criterio_parada = new ParametrosCriterioParada()
            {
                // // // EXECUÇÃO NORMAL
                // tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE,
                // NFE_criterio_parada = 100000,
                // lista_NFEs_desejados = new List<int>(){500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000}

                tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE,
                NFE_criterio_parada = 5000,
                lista_NFEs_desejados = new List<int>(){50,100,200,300,400,500,600,700,800,900,1000,1250,1500,1750,2000,2500,3000,4000,5000}

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


            // Para cada função, executa os algoritmos...
            foreach (int definicao_funcao_objetivo in function_values)
            {
                // =======================================================================================
                // Define os parâmetros do problema
                ParametrosProblema parametros_problema = FunctionParameters.FunctionParameters.get_function_parameters(definicao_funcao_objetivo);



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
                parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2 = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3 = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4 = true;
                parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9 = true;

                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3var = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4var = true;
                // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9var = true;

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
                List<RetornoGEOs> todas_execucoes_algoritmos = ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                
                // Organiza os resultados de todas as excuções por algoritmo
                List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                
                // Apresenta os resultados finais
                ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema);
                // ======================================================================================================







                
                
                
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

            }
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

            // ExtensiveSearch_and_Testes.ExtensiveSearch_and_Testes.ExtensiveSearch_SpacecraftOptimization();
            // ExtensiveSearch_and_Testes.ExtensiveSearch_and_Testes.Teste_FuncoesObjetivo_SpacecraftOptimization();

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