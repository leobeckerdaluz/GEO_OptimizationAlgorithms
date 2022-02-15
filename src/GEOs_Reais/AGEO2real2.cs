using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;


namespace GEOs_REAIS
{
    public class AGEO2real2 : GEO_real2
    {
        public int tipo_AGEO {get; set;}
        public double CoI_1 {get; set;}
        public bool reiniciou_o_tau {get; set;}
        public int qtde_resets_tau {get; set;}
        
        
        public AGEO2real2(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            bool primeira_das_P_perturbacoes_uniforme,
            int tipo_variacao_std_nas_P_perturbacoes,
            double std,
            int tipo_perturbacao,
            int P,
            int s) : base(
                new List<double>(populacao_inicial),
                0.5,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                primeira_das_P_perturbacoes_uniforme,
                tipo_variacao_std_nas_P_perturbacoes,
                std,
                tipo_perturbacao,
                P,
                s)
        {
            this.tipo_AGEO = 2;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);

            this.P = P;
            this.s = s;
            this.tipo_variacao_std_nas_P_perturbacoes = tipo_variacao_std_nas_P_perturbacoes;
            this.primeira_das_P_perturbacoes_uniforme = primeira_das_P_perturbacoes_uniforme;

            this.reiniciou_o_tau = false;
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Obtém o tamanho da população de bits
            int tamanho_populacao = this.populacao_atual.Count;
            
            // Instancia as funções úteis do mecanismo A-GEO
            MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();
            
            // Calcula o f(x) de referência
            double fx_referencia = mecanismo.calcula_fx_referencia(tipo_AGEO, fx_melhor, fx_atual);
            
            // Calcula o CoI
            double CoI = mecanismo.calcula_CoI_real(perturbacoes_da_iteracao, fx_referencia, tamanho_populacao);
            
            
            
            // VERSÃO ORIGINAL DE ATUALIZAR O TAU
            // Atualiza o tau
            tau = mecanismo.obtem_novo_tau(this.tipo_AGEO, this.tau, CoI, this.CoI_1, tamanho_populacao);
            

            
            
            // Se o CoI é 0, marca que reiniciou o tau
            if (CoI == 0){
                reiniciou_o_tau = true;
                qtde_resets_tau++;
            }




                
            // Armazena o CoI atual para ser usado como o anterior na próxima iteração
            this.CoI_1 = CoI;
        }



        // public override RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada)
        // {
        //     while(true)
        //     {
        //         // Armazena o valor da função no início da iteração
        //         fx_atual_comeco_it = fx_atual;
                
        //         verifica_perturbacoes();    // Realiza todas as perturbações nas variáveis
        //         mutacao_do_tau_AGEOs();     // Muta o tau se necessário
        //         ordena_e_perturba();        // Escolhe as perturbações a serem confirmadas

        //         // Armazena os dados da iteração
        //         iterations++;
        //         melhoras_nas_iteracoes.Add((fx_atual < fx_atual_comeco_it) ? 1 : 0 );
        //         stats_TAU_per_iteration.Add(tau);
        //         stats_STDPORC_per_iteration.Add(std);
        //         stats_Mfx_per_iteration.Add(fx_melhor);

        //         // Se o critério de parada for atingido, retorna as informações da execução
        //         if ( criterio_parada(parametros_criterio_parada) )
        //         {
        //             RetornoGEOs retorno = new RetornoGEOs();
        //             retorno.NFE = this.NFE;
        //             retorno.iteracoes = this.iterations;
        //             retorno.melhor_fx = this.fx_melhor;
        //             retorno.melhores_NFEs = this.melhores_NFEs;
        //             retorno.fxs_atuais_NFEs = this.fxs_atuais_NFEs;
        //             retorno.populacao_final = this.populacao_melhor;
        //             retorno.stats_TAU_per_iteration = this.stats_TAU_per_iteration;
        //             retorno.stats_STDPORC_per_iteration = this.stats_STDPORC_per_iteration;
        //             retorno.stats_Mfx_per_iteration = this.stats_Mfx_per_iteration;

                    
        //             Console.WriteLine("qtde_resets_tau: " + this.qtde_resets_tau);
                    

        //             return retorno;
        //         }
        //     }
        // } 
    }
}
