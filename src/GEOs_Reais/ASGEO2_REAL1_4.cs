// #define DEBUG_CONSOLE
// #define DEBUG_MUTACAO_TAU

using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL1_4 : GEO_real1
    {
        public double CoI_1 {get; set;}
        public int q_OF_rule {get; set;}
        public double c_OF_rule {get; set;}
        public double std_minimo_OF_rule {get; set;}

        
        public ASGEO2_REAL1_4(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            int step_obter_NFOBs,
            int tipo_perturbacao,
            double tau,
            double std,
            int q,
            double c,
            double std_minimo) : base(
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                step_obter_NFOBs,
                tipo_perturbacao,
                tau,
                std)
        {
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tau = 0.5;
            this.std = 0.5;
            this.q_OF_rule = q;
            this.c_OF_rule = c;
            this.std_minimo_OF_rule = std_minimo;
            this.std = std_minimo;
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Armazena o tau a ser alterado
            double tau_antigo = tau;
            // Armazena o sigma a ser alterado
            double std_antigo = std;
            
            
            // ====================================================================================
            // TAU CoI

            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;
            // Define o valor de referência (AGEO2)
            double valor_ref = fx_atual;
            
            // Verifica quantos melhora em comparação com o valor de referência
            melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao < valor_ref).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / populacao_atual.Count;
            

            // Se a CoI for zero, restarta o TAU
            if (CoI == 0.0){
                
                // Reseta o tau
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));

                // Reseta a população
                Random rnd1 = new Random();
                populacao_atual = Execucoes.Execucoes_GEO.geracao_populacao_real(lower_bounds, upper_bounds, rnd1.Next());

                // Atualiza a população atual
                fx_atual = calcula_valor_funcao_objetivo(populacao_atual);

                std = 5;
            }

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
            int q = q_OF_rule;
            double c = c_OF_rule;
            double std_minimo = std_minimo_OF_rule;
            
            // A cada q iterações, verifica
            if ((melhoras_nas_iteracoes.Count > 0) && ((melhoras_nas_iteracoes.Count % q) == 0))
            {
                // Pega os últimos melhores NFOBs
                List<bool> ultimas_melhorias_iteracoes = melhoras_nas_iteracoes.GetRange(melhoras_nas_iteracoes.Count - q, q);

                int melhoraram_its = ultimas_melhorias_iteracoes.Count(i => i == true);
                
                double razao = (double)melhoraram_its / q;

                if (razao < 0.2)
                    std = std * c;
                else if (razao > 0.2)
                    std = std / c;
                else
                    std = std;

                // Controla o std mínimo
                if (std <= std_minimo)
                    std = std_minimo;
            }
        }
    }
}