using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL1_2 : GEO_real1
    {
        public double CoI_1 {get; set;}
        public int q_OF_rule {get; set;}
        public double c_OF_rule {get; set;}
        public double std_minimo_OF_rule {get; set;}

        
        public ASGEO2_REAL1_2(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            int tipo_perturbacao,
            double tau,
            double std,
            int q,
            double c,
            double std_minimo) : base(
                n_variaveis_projeto,
                function_id,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
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
            if (CoI == 0.0)// || tau > 5)
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1)
                tau += (0.5 + CoI) * random.NextDouble();
            
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
                // Pega os últimos melhores NFEs
                List<int> ultimas_melhorias_iteracoes = melhoras_nas_iteracoes.GetRange(melhoras_nas_iteracoes.Count - q, q);
                int melhoraram_its = ultimas_melhorias_iteracoes.Count(i => i == 1);
                
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