using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL1_3 : GEO_real1
    {
        public double CoI_1 {get; set;}

        
        public ASGEO2_REAL1_3(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            int tipo_perturbacao,
            double tau,
            double std,
            bool round_current_population_every_it) : base(
                n_variaveis_projeto,
                function_id,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                tipo_perturbacao,
                tau,
                std,
                round_current_population_every_it)
        {
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tau = 0.5;
            this.std = 0.5;
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Armazena o tau a ser alterado
            double tau_antigo = tau;
            // Armazena o sigma a ser alterado
            double std_antigo = std;
            
            

            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;
            // Define o valor de referência (AGEO2)
            double valor_ref = fx_atual;
            
            // Verifica quantos melhora em comparação com o valor de referência
            melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao < valor_ref).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / populacao_atual.Count;
            

            // ====================================================================================
            // TAU CoI
            // Se a CoI for zero, restarta o TAU
            if (CoI == 0.0)// || tau > 5)
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1)
                tau += (0.5 + CoI) * random.NextDouble();
            
            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;
            


            // ====================================================================================
            // SIGMA 1/5
            double std_minimo = 0.2;
            
            if (CoI == 0.0){
                // Reinicia o sigma
                std = 2;
            }
            else if(CoI <= CoI_1){
                // Diminui sigma
                std = std * 0.90;
            }

            // Controla o std mínimo
            if (std <= std_minimo)
                std = std_minimo;
        }
    }
}
