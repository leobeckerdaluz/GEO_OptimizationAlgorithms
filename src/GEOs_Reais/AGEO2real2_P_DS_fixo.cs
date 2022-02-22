using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;


namespace GEOs_REAIS
{
    public class AGEO2real2_P_DS_fixo : AGEO2real2
    {
        public AGEO2real2_P_DS_fixo(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados) : base(
                new List<double>(populacao_inicial),
                n_variaveis_projeto,
                function_id,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                false,
                9999,
                9999,
                (int)EnumTipoPerturbacao.perturbacao_porcentagem,
                9999,
                9999)
        {
            // Fixa o 'P' e o 's' em 10
            this.P = 10;
            this.s = 10;
            this.tipo_variacao_std_nas_P_perturbacoes = (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s;
            this.primeira_das_P_perturbacoes_uniforme = false;
        }





        public override RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada)
        {
            while(true)
            {
                // Armazena o valor da função no início da iteração
                fx_atual_comeco_it = fx_atual;
                
                verifica_perturbacoes();    // Realiza todas as perturbações nas variáveis
                mutacao_do_tau_AGEOs();     // Muda o tau se necessário
                ordena_e_perturba();        // Escolhe as perturbações a serem confirmadas

                // Armazena os dados da iteração
                iterations++;
                melhoras_nas_iteracoes.Add((fx_atual < fx_atual_comeco_it) ? 1 : 0 );
                stats_TAU_per_iteration.Add(tau);
                stats_STDPORC_per_iteration.Add(std);
                stats_Mfx_per_iteration.Add(fx_melhor);

                // Se o critério de parada for atingido, retorna as informações da execução
                if ( criterio_parada(parametros_criterio_parada) )
                {
                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFE = this.NFE;
                    retorno.iteracoes = this.iterations;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFEs = this.melhores_NFEs;
                    retorno.fxs_atuais_NFEs = this.fxs_atuais_NFEs;
                    retorno.populacao_final = this.populacao_melhor;
                    retorno.stats_TAU_per_iteration = this.stats_TAU_per_iteration;
                    retorno.stats_STDPORC_per_iteration = this.stats_STDPORC_per_iteration;
                    retorno.stats_Mfx_per_iteration = this.stats_Mfx_per_iteration;

                    return retorno;
                }




                // No fim de cada iteração, gera um novo p1
                this.std = new MathNet.Numerics.Distributions.LogNormal(4, 0.2).Sample();
            }
        } 
    }
}
