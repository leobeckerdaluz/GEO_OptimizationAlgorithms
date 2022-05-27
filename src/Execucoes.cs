
#define CONSOLE_OUT_FILE

using System;
using System.Collections.Generic;
using Classes_e_Enums;
using System.IO;
using System.Linq;

namespace Execucoes
{
    public class Execucoes_GEO
    {
        public void Execucoes()
        {
            // =======================================================================================
            // Define qual modo o usuário deseja operar (executar ou realizar tuning)
            // =======================================================================================
            List<int> o_que_fazer = new List<int>(){ 
                (int)EnumOQueFazer.executar_algoritmos,
                // (int)EnumOQueFazer.tuning_GEO,
                // (int)EnumOQueFazer.tuning_GEOvar,
                // (int)EnumOQueFazer.tuning_GEOreal1_O,
                // (int)EnumOQueFazer.tuning_GEOreal1_P,
                // (int)EnumOQueFazer.tuning_GEOreal1_N,
                // (int)EnumOQueFazer.tuning_GEOreal2_O_VO,
                // (int)EnumOQueFazer.tuning_GEOreal2_P_VO,
                // (int)EnumOQueFazer.tuning_GEOreal2_N_VO,
                // (int)EnumOQueFazer.tuning_GEOreal2_O_DS,
                // (int)EnumOQueFazer.tuning_GEOreal2_P_DS,
                // (int)EnumOQueFazer.tuning_GEOreal2_N_DS,
                // (int)EnumOQueFazer.tuning_AGEO2real1_P,
            };
            


            // =======================================================================================
            // Define a lista de funções a serem executadas
            // =======================================================================================
            List<int> function_values = new List<int>()
            {
                // (int)EnumNomesFuncoesObjetivo.griewangk,
                // (int)EnumNomesFuncoesObjetivo.rastringin,
                // (int)EnumNomesFuncoesObjetivo.rosenbrock,
                // (int)EnumNomesFuncoesObjetivo.schwefel,
                // (int)EnumNomesFuncoesObjetivo.ackley,
                // (int)EnumNomesFuncoesObjetivo.beale,
                
                // (int)EnumNomesFuncoesObjetivo.CEC2017_01,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_03,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_04,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_05,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_06,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_07,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_08,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_09,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_10,
                
                // (int)EnumNomesFuncoesObjetivo.CEC2017_11,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_12,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_13,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_14,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_15,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_16,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_17,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_18,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_19,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_20,
                
                // (int)EnumNomesFuncoesObjetivo.CEC2017_21,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_22,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_23,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_24,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_25,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_26,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_27,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_28,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_29,
                // (int)EnumNomesFuncoesObjetivo.CEC2017_30,
                
                (int)EnumNomesFuncoesObjetivo.spacecraft,
                
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


            

            int qtde_execucoes = 50;

            // int float_str_format = (int)EnumFloatStrFormat.scientific_notation;
            int float_str_format = (int)EnumFloatStrFormat.decimal_double;

            // int criterio_parada = (int)EnumCriteriosParada.execution_1e5;
            // int criterio_parada = (int)EnumCriteriosParada.execution_3e5;
            // int criterio_parada = (int)EnumCriteriosParada.execution_1e6;
            int criterio_parada = (int)EnumCriteriosParada.execution_spacecraft;
            // int criterio_parada = (int)EnumCriteriosParada.tuning_1e16_10e5;

            bool integer_population = true;
            // bool integer_population = false;
            
            QuaisAlgoritmosRodar quais_algoritmos_rodar = new QuaisAlgoritmosRodar(){
                // rodar_GEO = true,
                // rodar_GEOvar = true,
                // rodar_AGEO2 = true,
                // rodar_AGEO2var = true,
                // rodar_AGEO2var_5 = true,
                // rodar_AGEO2real2_AA3 = true,
                // rodar_AGEO2real2_AA2 = true,
                rodar_AGEO2real1_P_AA = true
            };

            OQueInteressaPrintar o_que_printar = new OQueInteressaPrintar(){
                // mostrar_melhores_NFE = true,
                mostrar_melhores_fx_cada_execucao = true,
            };









            foreach (int opcao in o_que_fazer){
                Console.WriteLine("\n\n\n\n");
                Console.WriteLine("============================================================================");
                Console.WriteLine("O que irá executar a seguir: {0}", Enum.GetName(typeof(EnumOQueFazer), opcao));
                Console.WriteLine("============================================================================");
                Console.WriteLine("\n\n");

                // =======================================================================================
                // Define os parâmetros de execução
                ParametrosExecucao parametros_execucao = new ParametrosExecucao();
                
                // Quantidade de execuções
                parametros_execucao.quantidade_execucoes = qtde_execucoes;

                // Por default, printa somente as médias na saída
                parametros_execucao.o_que_interessa_printar = o_que_printar;
                parametros_execucao.o_que_interessa_printar.mostrar_meanNFE_meanFX_sdFX = true;
                parametros_execucao.quais_algoritmos_rodar = quais_algoritmos_rodar;
                parametros_execucao.integer_population = integer_population;
                

                // Define os critérios de parada
                parametros_execucao.parametros_criterio_parada = new ParametrosCriterioParada();
                switch(criterio_parada){
                    case (int)EnumCriteriosParada.tuning_1e16_10e5:{
                        parametros_execucao.parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFE;
                        parametros_execucao.parametros_criterio_parada.PRECISAO_criterio_parada = 1e-16;
                        parametros_execucao.parametros_criterio_parada.NFE_criterio_parada = 100000;
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados = new List<int>(){100000};
                    break;
                    }

                    case (int)EnumCriteriosParada.execution_1e5:{
                        parametros_execucao.parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE;
                        parametros_execucao.parametros_criterio_parada.NFE_criterio_parada = 100000;
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados = new List<int>(){5,50,100,500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000};
                    break;
                    }

                    case (int)EnumCriteriosParada.execution_3e5:{
                        parametros_execucao.parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE;
                        parametros_execucao.parametros_criterio_parada.NFE_criterio_parada = 300000;
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados = new List<int>(){5,50,100,500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000,150000,200000,250000,300000};
                    break;
                    }

                    case (int)EnumCriteriosParada.execution_1e6:{
                        parametros_execucao.parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE;
                        parametros_execucao.parametros_criterio_parada.NFE_criterio_parada = 1000000;
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados = new List<int>(){5,50,100,500,1000,1500,2000,2500,3000,3500,4000,4500,5000,6000,7000,8000,9000,10000,12000,14000,16000,18000,20000,25000,30000,35000,40000,45000,50000,55000,60000,65000,70000,75000,80000,85000,90000,95000,100000,150000,200000,250000,300000,350000,400000,450000,500000,550000,600000,650000,700000,750000,800000,850000,900000,950000,1000000};
                    break;
                    }

                    case (int)EnumCriteriosParada.execution_spacecraft:{
                        parametros_execucao.parametros_criterio_parada.tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFE;
                        parametros_execucao.parametros_criterio_parada.NFE_criterio_parada = 100000;
                        parametros_execucao.parametros_criterio_parada.lista_NFEs_desejados = new List<int>(){5};
                        parametros_execucao.parametros_criterio_parada.PRECISAO_criterio_parada = 196.94943319216; //196.94943319215918;
                    break;
                    }
                }
                // =======================================================================================

                

                

                

                // Para cada função, executa os algoritmos ou os tunings para cada função.
                foreach (int function_id in function_values)
                {
                    // =======================================================================================
                    // Define os parâmetros do problema
                    // =======================================================================================
                    ParametrosProblema parametros_problema = ObjectiveFunctions.Parameters.get_function_parameters(function_id);

                    Console.WriteLine("\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("============================================================================");
                    Console.WriteLine("Função: {0}", parametros_problema.nome_funcao);
                    Console.WriteLine("============================================================================");


                    // =======================================================================================
                    // Após definir os parâmetros do problema e de execução, executa os algoritmos desejados
                    switch(opcao){
                        case (int)EnumOQueFazer.executar_algoritmos:
                        {
                            // ======================================================================================================
                            // O que interessa printar no arquivo de saída ao 'executar algoritmos'
                            // parametros_execucao.o_que_interessa_printar.mostrar_melhores_NFE = true;
                            
                            // parametros_execucao.o_que_interessa_printar.mostrar_fxs_atual_por_NFE = true;
                            // parametros_execucao.o_que_interessa_printar.mostrar_mean_TAU_iteracoes = true; //alg. sozinho
                            // parametros_execucao.o_que_interessa_printar.mostrar_mean_STDPORC_iteracoes = true;
                            
                            // parametros_execucao.o_que_interessa_printar.mostrar_melhores_fx_cada_execucao = true;
                            // parametros_execucao.o_que_interessa_printar.mostrar_mean_Mfx_iteracoes = true;
                            // ======================================================================================================



                            // ======================================================================================================
                            // Quais algoritmos executar
                            
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEO = true;                    // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar = true;                 // Possui parâmetros livres
                            
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9 = true;

                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO3var = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO4var = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO9var = true;
                            
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_3 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_5 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_7 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_9 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO1var_11 = true;
                            
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_3 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_5 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_7 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_9 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2var_11 = true;

                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O = true;             // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_P = true;             // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_N = true;             // Possui parâmetros livres

                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_VO = true;          // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO = true;          // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO = true;          // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_DS = true;          // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS = true;          // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS = true;          // Possui parâmetros livres

                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO_UNI = true;      // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO_UNI = true;      // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS_UNI = true;      // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS_UNI = true;      // Possui parâmetros livres

                            // Poucos parâmetros
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_P = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_DS = true;        // Possui parâmetros livres
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_DS_fixo = true;
                            
                            // Auto-adaptativos
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_P_AA = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_AA0 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_AA1 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_AA2 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_AA3 = true;
                            // parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real2_P_AA_p9 = true;
                            

                            // ======================================================================================================
                            
                            
                            // ======================================================================================================
                            // Executa os algoritmos
                            ExecutaOrganizaApresenta.ExecutaOrganizaApresenta exec = new ExecutaOrganizaApresenta.ExecutaOrganizaApresenta();
                            
                            // Executa cada algoritmo por N vezes e obtém todas as execuções
                            List<RetornoGEOs> todas_execucoes_algoritmos = exec.executa_algoritmos_n_vezes(parametros_execucao, parametros_problema);
                            
                            // Organiza os resultados de todas as excuções por algoritmo
                            List<Retorno_N_Execucoes_GEOs> resultados_por_algoritmo = ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.organiza_os_resultados_de_cada_execucao(todas_execucoes_algoritmos, parametros_execucao);
                            
                            // Apresenta os resultados finais
                            ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.apresenta_resultados_finais(parametros_execucao.o_que_interessa_printar, resultados_por_algoritmo, parametros_execucao, parametros_problema, float_str_format);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEO:
                        {
                            // ======================================================================================================
                            // Tuning do GEO
                            // List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4.0}; 
                            List<double> valores_tau = new List<double>(){0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5}; 
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEO = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEO_GEOvar(parametros_execucao, parametros_problema, valores_tau);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOvar:
                        {
                            // ======================================================================================================
                            // Tuning do GEOvar
                            List<double> valores_tau = new List<double>(){0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4.0, 4.25, 4.5, 4.75, 5}; 
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOvar = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();

                            List<Tuning> resultados_tuning = tunings.tuning_GEO_GEOvar(parametros_execucao, parametros_problema, valores_tau);
                           
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal1_O:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal1_O
                            
                            List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6};
                            List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};

                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_O = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal1_O(parametros_execucao, parametros_problema, valores_tau, valores_std);
                         
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal1_P:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal1_P
                            
                            List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6};
                            List<double> valores_porcentagem = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10};
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_P = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal1_P(parametros_execucao, parametros_problema, valores_tau, valores_porcentagem);
                          
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal1_N:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal1_N
                            
                            List<double> valores_tau = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6};
                            List<double> valores_std = new List<double>(){0.2, 0.4, 0.6, 0.8, 1.0, 1.2, 1.4, 1.6, 1.8, 2.0, 2.2, 2.4, 2.6, 2.8, 3.0};

                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal1_N = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal1_N(parametros_execucao, parametros_problema, valores_tau, valores_std);
                           
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_O_VO:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_O_VO
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_std1 = new List<double>(){0.5, 1, 2, 10};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){1, 2, 4};
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_VO = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_O_VO(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_P_VO:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_P_VO
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_porcent = new List<double>(){0.1, 1, 10, 50};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){1, 2, 4};
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_VO = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_P_VO(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_N_VO:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_N_VO                
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_std1 = new List<double>(){0.5, 1, 2, 10};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){1, 2, 4};

                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_VO = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_N_VO(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_O_DS:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_O_DS
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_std1 = new List<double>(){0.5, 1, 2, 10};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){2, 10};;

                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_O_DS = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_O_DS(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_P_DS:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_P_DS
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_porcent = new List<double>(){0.1, 1, 10, 50};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){2, 10};

                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_P_DS = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_P_DS(parametros_execucao, parametros_problema, valores_tau, valores_porcent, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_GEOreal2_N_DS:
                        {
                            // ======================================================================================================
                            // Tuning do GEOreal2_N_DS                
                            List<double> valores_tau = new List<double>(){0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5, 5.0};
                            List<double> valores_std1 = new List<double>(){0.5, 1, 2, 10};
                            List<double> valores_P = new List<double>(){5, 10};
                            List<double> valores_s = new List<double>(){2, 10};
                            
                            parametros_execucao.quais_algoritmos_rodar.rodar_GEOreal2_N_DS = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_GEOreal2_N_DS(parametros_execucao, parametros_problema, valores_tau, valores_std1, valores_P, valores_s);
                            
                            bool ordenar = true;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        case (int)EnumOQueFazer.tuning_AGEO2real1_P:
                        {
                            // ======================================================================================================
                            // Tuning do AGEO2real1_P
                            
                            
                            // List<double> valores_porcent = new List<double>(){0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10, 10.5, 11, 11.5, 12};
                            // List<double> valores_porcent = new List<double>(){0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 50, 100};
                            List<double> valores_porcent = Enumerable.Range(1, 50).Select(i => i*2.0).ToList();
                            

                            parametros_execucao.quais_algoritmos_rodar.rodar_AGEO2real1_P = true;
                            
                            Tunings.Tunings tunings = new Tunings.Tunings();
                            
                            List<Tuning> resultados_tuning = tunings.tuning_AGEO2real1_P(parametros_execucao, parametros_problema, valores_porcent);
                          
                            bool ordenar = false;
                            tunings.ordena_e_apresenta_resultados_tuning(resultados_tuning, ordenar);
                            // ======================================================================================================
                        }
                        break;


                        default:
                            Console.WriteLine("Tipo de execução não conhecido!");
                        break;
                    }



                



                    
                    // // ======================================================================================================
                    // // ================================== ASGEO e 1/5 rule ==================================================

                    // // ====================================================================================
                    // // Execução 1/5 rule tunado
                    // parametros_problema.parametros_livres.stdmin_one_fifth_rule = 0.2; 
                    // parametros_problema.parametros_livres.q_one_fifth_rule = 200; 
                    // parametros_problema.parametros_livres.c_one_fifth_rule = 0.9;

                    // // ====================================================================================
                    // // Tuning do c e q para 1/5 rule dos A-GEOsreal1
                    // List<double> valores_stdmin = new List<double>(){0.1, 0.2, 0.5, 1};
                    // // List<int> valores_q = new List<int>(){50, 100, 200, 500};
                    // List<int> valores_q = new List<int>(){100, 200, 500};
                    // // List<double> valores_c = new List<double>(){0.85, 0.9, 0.95};
                    // List<double> valores_c = new List<double>(){0.9, 0.95};
                    // // ======================================================================================================

                }
            }
        }
    
        public void Verify_Final_Stats(){

            ParametrosExecucao parametros_execucao = new ParametrosExecucao();
            parametros_execucao.quantidade_execucoes = 2;
            parametros_execucao.parametros_criterio_parada = new ParametrosCriterioParada()
            {
                tipo_criterio_parada = (int)EnumTipoCriterioParada.parada_por_NFE,
                NFE_criterio_parada = 5000,
                lista_NFEs_desejados = new List<int>(){100,200,300,400,500,600,700,800,900,1000}
            };


            // List<RetornoGEOs> todas_execucoes = new List<RetornoGEOs>()
            // {
            //     new RetornoGEOs(){
            //         algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can,
            //         iteracoes = 60,
            //         melhor_fx = 10,
            //         melhores_NFEs = new List<double> (){10,20,30,40,50,60,70,80,90,100},
            //         melhores_TAUs = new List<double> (){1,2,3,4,5,6,7,8,9,10},
            //         NFE = 100000
            //     },
            //     new RetornoGEOs(){
            //         algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can,
            //         iteracoes = 30,
            //         melhor_fx = 20,
            //         melhores_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
            //         melhores_TAUs = new List<double> (){2,4,6,8,10,12,14,16,18,20},
            //         NFE = 90000
            //     },
            //     new RetornoGEOs(){
            //         algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var,
            //         iteracoes = 60,
            //         melhor_fx = 10,
            //         melhores_NFEs = new List<double> (){10,20,30,40,50,60,70,80,90,100},
            //         melhores_TAUs = new List<double> (){1,2,3,4,5,6,7,8,9,10},
            //         NFE = 100000
            //     },
            //     new RetornoGEOs(){
            //         algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var,
            //         iteracoes = 30,
            //         melhor_fx = 20,
            //         melhores_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
            //         melhores_TAUs = new List<double> (){2,4,6,8,10,12,14,16,18,20},
            //         NFE = 90000
            //     }
            // };

            List<RetornoGEOs> todas_execucoes = new List<RetornoGEOs>()
            {
                new RetornoGEOs(){
                    algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can,
                    iteracoes = 5,
                    melhor_fx = 10,
                    melhores_NFEs = new List<double> (){10,20},
                    fxs_atuais_NFEs = new List<double> (){10,20},
                    melhores_TAUs = new List<double> (){1,2,3,4,5},
                    NFE = 2000
                },
                new RetornoGEOs(){
                    algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_can,
                    iteracoes = 30,
                    melhor_fx = 20,
                    melhores_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
                    fxs_atuais_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
                    melhores_TAUs = new List<double> (){2,4,6,8,10,12,14,16,18,20},
                    NFE = 90000
                },
                new RetornoGEOs(){
                    algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var,
                    iteracoes = 12,
                    melhor_fx = 10,
                    melhores_NFEs = new List<double> (){10,20,30,40,50,60,70,80,90,100},
                    fxs_atuais_NFEs = new List<double> (){10,20,30,40,50,60,70,80,90,100},
                    melhores_TAUs = new List<double> (){1,2,3,4,5,6,7,8,9,10},
                    NFE = 100000
                },
                new RetornoGEOs(){
                    algoritmo_utilizado = (int)EnumNomesAlgoritmos.GEO_var,
                    iteracoes = 30,
                    melhor_fx = 20,
                    melhores_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
                    fxs_atuais_NFEs = new List<double> (){5,15,25,35,45,55,65,75,85,95},
                    melhores_TAUs = new List<double> (){2,4,6,8,10,12,14,16,18,20},
                    NFE = 90000
                }
            };

            List<Retorno_N_Execucoes_GEOs> ret = ExecutaOrganizaApresenta.ExecutaOrganizaApresenta.organiza_os_resultados_de_cada_execucao(todas_execucoes, parametros_execucao);
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
            // If desired, set console output to file
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
                    Console.WriteLine (e.Message);
                    return;
                }
                Console.SetOut (writer);
            #endif
            // ============================================================================





            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();



            // Execuções
            Execucoes_GEO ex = new Execucoes_GEO();
            ex.Execucoes();
            
            // ex.Verify_Final_Stats();

            // ExtensiveSearch_and_Testes.ExtensiveSearch_and_Testes.ExtensiveSearch_SpacecraftOptimization();
            // ExtensiveSearch_and_Testes.ExtensiveSearch_and_Testes.Teste_FuncoesObjetivo_SpacecraftOptimization();



            watch.Stop();
            Console.WriteLine("\n\nExecution Time: {0} s", watch.ElapsedMilliseconds/1000.0);





            // ============================================================================
            // Set console output back to terminal
            // ============================================================================
            #if CONSOLE_OUT_FILE
                Console.SetOut (oldOut);
                writer.Close();
                ostrm.Close();
            #endif
            // ============================================================================

            Console.WriteLine ("Done");
        }
    }
}