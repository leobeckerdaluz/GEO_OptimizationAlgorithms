using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;


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
            int function_id,
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
                function_id,
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
    }
}
