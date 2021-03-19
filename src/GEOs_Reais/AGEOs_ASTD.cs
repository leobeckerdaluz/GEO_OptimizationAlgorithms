using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class AGEOs_ASTD : AGEOs_REAL1
    {
        public double std_minimo_inicial {get;set;}

        public AGEOs_ASTD(double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais, int step_obter_NFOBs, double std_minimo_inicial, int tipo_AGEO, int tipo_perturbacao_original_ou_SDdireto) : base(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais, step_obter_NFOBs, std_minimo_inicial, tipo_AGEO, tipo_perturbacao_original_ou_SDdireto){
            // this.std = std_minimo_inicial;
            this.std = 2;
            this.std_minimo_inicial = std_minimo_inicial;
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

                // RESTARTA O STD
                this.std = 1;//this.std_minimo_inicial;

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1){
                tau += (0.5 + CoI) * random.NextDouble();

                // MELHORA O STD
                std = 0.8;
                // if (std >= 0.1){
                //     std -= 0.01; 
                // }
            }
            
            // Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_atual.Count, melhoraram);
            // Console.WriteLine("Valor TAU: {0}", tau);

            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;
        }

    }
}
