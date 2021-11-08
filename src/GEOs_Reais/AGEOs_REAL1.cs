using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class AGEOs_REAL1 : GEO_real1
    {
        public int tipo_AGEO {get; set;}
        public double CoI_1 {get; set;}

        
        public AGEOs_REAL1(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFOBs_desejados,
            double std,
            int tipo_AGEO,
            int tipo_perturbacao) : base(
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                lista_NFOBs_desejados,
                tipo_perturbacao,
                0.5,
                std)
        {
            this.tipo_AGEO = tipo_AGEO;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;
            // Define o valor de referência
            double valor_ref = (this.tipo_AGEO == 1) ? fx_melhor : fx_atual;
            
            // Verifica quantos melhora em comparação com o valor de referência
            melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao < valor_ref).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / populacao_atual.Count;
            // Armazena o tau a ser alterado
            double tau_antigo = tau;

            // Se a CoI for zero, restarta o TAU
            if (CoI == 0.0)// || tau > 5)
            {
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1)
            {
                tau += (0.5 + CoI) * random.NextDouble();
            }
            
            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;
        }
    }
}
