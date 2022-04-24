using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;
using CheckFeasibility;

namespace GEOs_REAIS
{
    public class AGEO2real1_P_AA : AGEO2real1
    {
        public double porcentagem {get; set;}
        
        public AGEO2real1_P_AA(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            double std_ou_porcentagem,
            int tipo_perturbacao,
            bool integer_population) : base(
                populacao_inicial,
                n_variaveis_projeto,
                function_id,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                std_ou_porcentagem,
                tipo_perturbacao,
                integer_population)
        {
            

            this.porcentagem = new MathNet.Numerics.Distributions.LogNormal(1, 0.67).Sample();
            // this.alfa = 1 / Math.Sqrt(n_variaveis_projeto);
            
            // this.sigma = 1;
            // this.porcentagem = 600;

        }
          


        public override void verifica_perturbacoes()
        {
            // Inicia a lista de perturbações zerada
            perturbacoes_da_iteracao = new List<Perturbacao>();
            



            // ------------------------------------------------------------------------------------
            // Antes de perturbar as variáveis, perturba o sigma

            // // Porcentagem atual
            // double porcentagem_atual = this.std;
            // double intervalo_variaveis =  
            
            // double alfa = 1.0 / Math.Sqrt(n_variaveis_projeto);
            // // MathNet.Numerics.Distributions.LogNormal lognormal_dist = new MathNet.Numerics.Distributions.LogNormal(0, alfa);
            // MathNet.Numerics.Distributions.LogNormal lognormal_dist = new MathNet.Numerics.Distributions.LogNormal(1, 0.67);
            // double rand_lognormal = lognormal_dist.Sample();
            // double porcentagem_linha = this.porcentagem * rand_lognormal;



            
            
            
            // ------------------------------------------------------------------------------------------------------------
            // Calcula a adaptabilidade do porcentagem linha. 
            
            double rand_lognormal = new MathNet.Numerics.Distributions.LogNormal(1, 0.67).Sample();
            double porcentagem_linha = rand_lognormal;
            
            // Cria uma cópia da população atual para perturbar todas as variáveis
            List<double> populacao_copia = new List<double>(populacao_atual);
            // Perturba todas as variáveis com aquela porcentagem perturlinha
            for(int i=0; i<populacao_atual.Count; i++){
                // Calcula o invervalo de variação dessa variável
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];

                // Pertuaba a variável
                populacao_copia[i] = perturba_variavel(
                    populacao_atual[i],
                    porcentagem_linha,
                    this.tipo_perturbacao,
                    intervalo_variacao_variavel,
                    integer_population
                );
            }
            
            // Cria as informações da perturbação da porcentagem e adiciona ela na lista de perturbações
            perturbacoes_da_iteracao.Add(
                new Perturbacao(){
                    xi_antes_da_perturbacao = porcentagem,
                    xi_depois_da_perturbacao = porcentagem_linha,
                    fx_depois_da_perturbacao = calcula_valor_funcao_objetivo(populacao_copia, true),
                    feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_copia, upper_bounds, lower_bounds),
                    populacao_depois_da_perturbacao = new List<double>(populacao_copia),
                    indice_variavel_projeto = 999,
                }
            );
            // ------------------------------------------------------------------------------------






            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++)
            {
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);
                
                // Obtém o intervalo de variação da variável
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];

                // Perturba a variável
                populacao_para_perturbar[i] = perturba_variavel(
                    populacao_atual[i],
                    this.porcentagem,
                    this.tipo_perturbacao,
                    intervalo_variacao_variavel,
                    integer_population
                );
                
                // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                perturbacoes_da_iteracao.Add(
                    new Perturbacao(){
                        xi_antes_da_perturbacao = populacao_atual[i],
                        xi_depois_da_perturbacao = populacao_para_perturbar[i],
                        feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_para_perturbar, upper_bounds, lower_bounds),
                        populacao_depois_da_perturbacao = new List<double>(populacao_para_perturbar),
                        fx_depois_da_perturbacao = calcula_valor_funcao_objetivo(populacao_para_perturbar, true),
                        indice_variavel_projeto = i
                    }
                );
            }
        }




        public override void ordena_e_perturba()
        {    
            // Ordena as perturbações com base no f(x)
            perturbacoes_da_iteracao.Sort(
                delegate(Perturbacao b1, Perturbacao b2) { 
                    return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); 
                }
            );

            // Verifica as probabilidades até que uma variável seja perturbada
            while (true)
            {
                // Gera um número aleatório com distribuição uniforme entre 0 e 1
                double ALE = random.NextDouble();
                
                // Determina a posição do ranking escolhida, entre 1 e o número de variáveis. +1 é 
                // ...porque tem que ser de 1 até menor que o 2º parámetro de .Next()
                int k = random.Next(1, perturbacoes_da_iteracao.Count+1   );
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -tau);

                // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                k -= 1;

                // Se o Pk é maior ou igual ao aleatório, então confirma a perturbação
                if (Pk >= ALE)
                {
                    // Obtém o índice da perturbação escolhida pra aceitar
                    int indice = perturbacoes_da_iteracao[k].indice_variavel_projeto;
                    // Obtém o valor da variável depois de perturbar
                    double xii_depois_perturbar = perturbacoes_da_iteracao[k].xi_depois_da_perturbacao;
                    // Obtém o f(x) da população com aquela variável perturbada
                    double fx_depois_perturbar = perturbacoes_da_iteracao[k].fx_depois_da_perturbacao;
                    // Obtém a população com a variável perturbada
                    List<double> populacao_depois_perturbar = new List<double>(perturbacoes_da_iteracao[k].populacao_depois_da_perturbacao);

                    // Atualiza com a população de variáveis escolhida
                    populacao_atual = new List<double>(populacao_depois_perturbar);
                    
                    // Se o índice escolhido para ser perturbado é o da porcentagem, atualiza a porcentagem
                    if(indice == 999){
                        // porcentagem foi auto-adaptada
                        this.porcentagem = xii_depois_perturbar;
                    }

                    // Atualiza o f(x) atual com o após perturbado
                    fx_atual = fx_depois_perturbar;

                    // Sai do laço while
                    break;
                }
            }
        }
    }
}
