// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE

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

        
        public AGEOs_REAL1(double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais, int step_obter_NFOBs, double std, int tipo_AGEO, double porcentagem_perturbacao) : base(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais, step_obter_NFOBs, std, porcentagem_perturbacao)
        {
            this.tipo_AGEO = tipo_AGEO;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;

            if (this.tipo_AGEO == 1){
                // Verifica quantos melhora em comparação com o MELHOR FX
                melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao <= this.fx_melhor).ToList().Count;
            }
            else if (this.tipo_AGEO == 2){
                // Verifica quantos melhora em comparação com o ATUAL FX
                melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao <= this.fx_atual).ToList().Count;
            }

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / populacao_atual.Count;

            // Se a CoI for zero, restarta o TAU
            if (CoI <= 0.0 || tau > 5){
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1){
                tau += (0.5 + CoI) * random.NextDouble();
            }
            
            // Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_atual.Count, melhoraram);
            // Console.WriteLine("Valor TAU: {0}", tau);

            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;
        }
    }
}
