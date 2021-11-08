// #define DEBUG_CONSOLE
// #define DEBUG_MUTACAO_TAU

using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class AGEO2_REAL2 : GEO_real2
    {
        public int tipo_AGEO {get; set;}
        public double CoI_1 {get; set;}
        public bool primeira_perturbacao_random_uniforme {get; set;}

         public AGEO2_REAL2(
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> populacao_inicial,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFOBs_desejados,
            bool primeira_perturbacao_random_uniforme,
            int P,
            int s,
            double std,
            int tipo_perturbacao_variavel) : base(
                populacao_inicial,
                0.5,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFOBs_desejados,
                std,
                tipo_perturbacao_variavel,
                P,
                s)
        {
            this.tipo_AGEO = 2;
            this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tau = 0.5;
            this.primeira_perturbacao_random_uniforme = primeira_perturbacao_random_uniforme;
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

                    // Perturbação uniforme caso seja necessária
                    if (primeira_perturbacao_random_uniforme )
                    {
                        if (j==0)
                        {
                            Random r = new Random();
                            xii = lower_bounds[i] + r.NextDouble()*intervalo_variacao_variavel;
                        }

                    }

                    // Atribui a variável perturbada
                    populacao_para_perturbar[i] = xii;

                    // Calcula f(x) com a variável perturbada
                    double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar);
                    add_NFOB();

                    // Avalia se a perturbação gera o melhor f(x) da história
                    if (fx < fx_melhor)
                    {
                        fx_melhor = fx;
                        populacao_melhor = populacao_para_perturbar;
                    }

                    // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                    Perturbacao perturbacao = new Perturbacao();
                    perturbacao.xi_antes_da_perturbacao = xi;
                    perturbacao.xi_depois_da_perturbacao = xii;
                    perturbacao.fx_depois_da_perturbacao = fx;
                    perturbacao.indice_variavel_projeto = i;

                    perturbacoes.Add(perturbacao);

                    // Só não atualiza o sigma se tiver a primeira perturbação uniforme ativada e j==0
                    if (!(primeira_perturbacao_random_uniforme && j==0))
                    {
                        std_atual = std_atual / this.s;
                    }
                }

                #if DEBUG_CONSOLE
                    Console.WriteLine("Motrando as perturbacoes:");
                    foreach(Perturbacao p in perturbacoes)
                    {
                        Console.WriteLine("{0}: xi = {1} | xii = {2} | fx = {3}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao, p.fx_depois_da_perturbacao);
                    }
                #endif

                
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
            // if (CoI == 0.0 || tau > 5)
            if (CoI == 0.0)// || tau > 5)
            {
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                
                tau = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Pow( (populacao_atual.Count), 1.0/2.0 )));
                // tau = 0.5;

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= CoI_1)
            {
                tau += (0.5 + CoI) * random.NextDouble();
                // tau = 10;
            }
            
            #if DEBUG_MUTACAO_TAU
                // Console.WriteLine("perturbações da iteração count: {0}", perturbacoes_da_iteracao.Count);
                Console.WriteLine("NFOB = {0} | melhoraram {1}/{2} | tau era {3} e virou {4} | fx={5}", this.NFOB, melhoraram,perturbacoes_da_iteracao.Count, tau_antigo, tau, fx_melhor);
                // Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_atual.Count, melhoraram);
                // Console.WriteLine("Valor TAU era {0} e virou {1}", tau_antigo, tau);
            #endif

            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            CoI_1 = CoI;
        }
    }
}
