// #define DEBUG_CONSOLE
// #define DEBUG_MUTACAO_TAU

using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL1_2 : GEO_real1
    {
        public double CoI_1 {get; set;}

        
        public ASGEO2_REAL1_2(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<RestricoesLaterais> restricoes_laterais,
            int step_obter_NFOBs,
            int tipo_perturbacao,
            double tau,
            double std) : base(
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                populacao_inicial,
                restricoes_laterais,
                step_obter_NFOBs,
                tipo_perturbacao,
                tau,
                std)
        {
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;
            // Define o valor de referência (AGEO2)
            double valor_ref = fx_atual;
            
            // Verifica quantos melhora em comparação com o valor de referência
            melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao < valor_ref).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / populacao_atual.Count;
            
            // Armazena o tau a ser alterado
            double tau_antigo = tau;
            // Armazena o sigma a ser alterado
            double std_antigo = std;




            // ====================================================================================
            // TAU COI

            // Se a CoI for zero, restarta o TAU
            if (CoI == 0.0)// || tau > 5)
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1)
                tau += (0.5 + CoI) * random.NextDouble();
            
            #if DEBUG_MUTACAO_TAU
                Console.WriteLine("NFOB = {0} | melhoraram {1}/{2} | tau era {3} e virou {4} | fx={5}", this.NFOB, melhoraram,populacao_atual.Count, tau_antigo, tau, fx_melhor);
            #endif

            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;



            // ====================================================================================
            // SIGMA 1/5
            double c = 0.9;
            
            if (CoI < 0.2)
                std = std*c;
            else if (CoI > 0.2)
                std = std/c;
            else
                std = std;

            // Controla o std mínimo
            if (std <= 0.2)
                std = 0.2;

                

            #if DEBUG_MUTACAO_TAU
                Console.WriteLine("NFOB = {0} | melhoraram {1}/{2} | std era {3} e virou {4} | fx={5}", this.NFOB, melhoraram,populacao_atual.Count, std_antigo, std, fx_melhor);
            #endif
        }
    }
}
