using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class ASGEO2_REAL2_1 : GEO_real2
    {
        public int tipo_AGEO {get; set;}
        public double CoI_1 {get; set;}

         public ASGEO2_REAL2_1(
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> populacao_inicial,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            int P,
            double std1,
            int s,
            int tipo_perturbacao) : base(
                populacao_inicial,
                0.5,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                false,
                (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original,
                std1,
                tipo_perturbacao,
                P,
                s)
        {
            this.tipo_AGEO = 2;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
            // this.P = 5;
            // this.s = 10;
            // this.std = 10;
            this.tau = 0.5;
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
                    double xii = perturba_variavel(xi, std_atual, this.tipo_perturbacao, intervalo_variacao_variavel);


                    
                    
                    
                    
                    // Na primeira iteração, perturba de forma diferente
                    if (j==0){
                        Random r = new Random();
                        
                        // 0 ----- min
                        // 1 ----- max
                        // r ----- xii

                        // xii = r * intervalo
                        xii = r.NextDouble() * intervalo_variacao_variavel;
                    }
                    



                    

                    // Atribui a variável perturbada
                    populacao_para_perturbar[i] = xii;

                    // Calcula f(x) com a variável perturbada
                    double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);

                    // // Avalia se a perturbação gera o melhor f(x) da história
                    // if (fx < fx_melhor)
                    // {
                    //     fx_melhor = fx;
                    //     populacao_melhor = populacao_para_perturbar;
                    // }

                    // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                    Perturbacao perturbacao = new Perturbacao();
                    perturbacao.xi_antes_da_perturbacao = xi;
                    perturbacao.xi_depois_da_perturbacao = xii;
                    perturbacao.fx_depois_da_perturbacao = fx;
                    perturbacao.indice_variavel_projeto = i;

                    perturbacoes.Add(perturbacao);

                    // Atualiza o novo std ===> std(i+1) = std(i) / (s*i)
                    // Onde i = 1,2...P e s é arbitrário e vale 2.
                    std_atual = std_atual / this.s;
                }
                
                // Adiciona cada perturbação na lista geral de perturbacoes
                // Console.WriteLine("perturbações.Count = {0}", perturbacoes.Count);
                foreach (Perturbacao p in perturbacoes)
                {
                    perturbacoes_da_iteracao.Add(p);
                }
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
