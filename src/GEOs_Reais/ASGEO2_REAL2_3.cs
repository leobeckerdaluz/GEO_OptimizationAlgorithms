using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL2_3 : GEO_real1
    {
        public int tipo_AGEO {get; set;}
        public double CoI_1 {get; set;}
        public int P {get; set;}
        public double s {get; set;}

         public ASGEO2_REAL2_3(
            int n_variaveis_projeto,
            int function_id,
            List<double> populacao_inicial,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados) : base(
                n_variaveis_projeto,
                function_id,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                1,
                0.5,
                1)
        {
            this.tipo_AGEO = 2;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tau = 0.5;
            
            this.P = 5;
            this.s = 10;
            this.std = 50;

            // // EXP
            // this.P = 4;
            // this.s = 10;
            // this.std = 10;
        }


        public override void verifica_perturbacoes()
        {
            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();

            // Verifica a perturbação para cada variável
            for(int i=0; i<n_variaveis_projeto; i++)
            {
                // Inicia a lista de perturbações zerada
                List<Perturbacao> perturbacoes = new List<Perturbacao>();

                double std_atual = this.std;

                // Para cada desvio padrão diferente, calcula as perturbações
                for(int j=0; j<this.P; j++)
                {
                    // Cria uma população cópia
                    List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                    double xi = populacao_para_perturbar[i];




                    // Perturba a variável
                    double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                    double xii = xi;

                    MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_atual);
                    MathNet.Numerics.Distributions.ContinuousUniform uniformContDist = new MathNet.Numerics.Distributions.ContinuousUniform();
                    MathNet.Numerics.Distributions.Exponential expDist = new MathNet.Numerics.Distributions.Exponential(std_atual);
                    
                    xii = xi + normalDist.Sample();
                    // xii = xi + uniformContDist.Sample();
                    
                    // double exp = expDist.Sample();
                    // double unif = uniformContDist.Sample();
                    // xii = xi + ((unif >= 0.5) ? exp : -exp);


                    
                    
                    
                    
                    // Na primeira iteração, perturba de forma diferente
                    if (j==0){
                        Random r = new Random();
                        
                        // 0 ----- min
                        // 1 ----- max
                        // r ----- xii

                        // xii = r.NextDouble() * intervalo_variacao_variavel;
                        xii = lower_bounds[i] + r.NextDouble()*intervalo_variacao_variavel;
                    }
                    



                    

                    // Atribui a variável perturbada
                    populacao_para_perturbar[i] = xii;

                    // Calcula f(x) com a variável perturbada
                    double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);

                    // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                    Perturbacao perturbacao = new Perturbacao();
                    perturbacao.xi_antes_da_perturbacao = xi;
                    perturbacao.xi_depois_da_perturbacao = xii;
                    perturbacao.fx_depois_da_perturbacao = fx;
                    perturbacao.indice_variavel_projeto = i;

                    perturbacoes.Add(perturbacao);





                    // Atualiza o novo std ===> std(i+1) = std(i) / (s*i)
                    if (j>0){
                        std_atual = std_atual / this.s;
                    }
                }

                // Ordena elas
                perturbacoes.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); });


                perturbacoes_da_iteracao.Add(perturbacoes[0]);

                
                // // Adiciona cada perturbação na lista geral de perturbacoes
                // // Console.WriteLine("perturbações.Count = {0}", perturbacoes.Count);
                // foreach (Perturbacao p in perturbacoes)
                // {
                //     perturbacoes_da_iteracao.Add(p);
                // }
            }
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
