using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class AGEO2real2_AA0 : AGEO2real2
    {
        public double porcentagem {get; set;}
        
        public AGEO2real2_AA0(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            bool integer_population) : base(
                populacao_inicial,
                n_variaveis_projeto,
                function_id,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                false,
                9999,
                9999,
                (int)EnumTipoPerturbacao.perturbacao_porcentagem,
                9999,
                9999,
                integer_population)
        {
            this.P = 10;
            this.porcentagem = new MathNet.Numerics.Distributions.LogNormal(1, 0.67).Sample();
            
            // this.std = 9999;
            // this.tau = 9999;
        }
          


        public override void verifica_perturbacoes()
        {
            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();
            
            // Lista com todas as variáveis de projeto (incluindo porcentagem de índice 999)
            List<int> indices_variaveis = Enumerable.Range(0, n_variaveis_projeto).ToList();
            indices_variaveis.Add(999);
            
            // PARA CADA P...
            for(int j=0; j<this.P; j++){

                // PARA CADA VARIÁVEL...
                foreach (int i in indices_variaveis){

                    // SE A VARIÁVEL É A 999 (p), ENTÃO PERTURBA TODAS VARIÁVEIS AO MESMO TEMPO COM ESSE p' E CALCULA FX
                    if(i == 999){
                        // GERA UM p' com maioria entre 0% e 10%
                        double rand_lognormal = new MathNet.Numerics.Distributions.LogNormal(1, 0.67).Sample();
                        double porcentagem_linha = rand_lognormal;

                        // Cria uma cópia da população atual para perturbar todas as variáveis
                        List<double> populacao_copia = new List<double>(populacao_atual);
                        // Perturba todas as variáveis com aquela porcentagem perturlinha
                        for(int k=0; k<populacao_atual.Count; k++){
                            // Calcula o invervalo de variação dessa variável
                            double intervalo_variacao_variavel = upper_bounds[k] - lower_bounds[k];
                            // Perturba a variável
                            populacao_copia[k] = perturba_variavel(
                                populacao_atual[k],
                                porcentagem_linha,
                                this.tipo_perturbacao,
                                intervalo_variacao_variavel,
                                integer_population
                            );
                        }
                        
                        // Cria e adiciona essa info da perturbação da porcentagem na lista de perturbações
                        perturbacoes_da_iteracao.Add(
                            new Perturbacao(){
                                xi_antes_da_perturbacao = this.porcentagem,
                                xi_depois_da_perturbacao = porcentagem_linha,
                                fx_depois_da_perturbacao = calcula_valor_funcao_objetivo(populacao_copia, true),
                                feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_copia, upper_bounds, lower_bounds),
                                populacao_depois_da_perturbacao = new List<double>(populacao_copia),
                                indice_variavel_projeto = 999,
                            }
                        );
                        // ------------------------------------------------------------------------------------
                    }
                    
                    // SENÃO, PERTURBA A VARIÁVEL DE PROJETO COM O p'
                    else{
                        // Cria uma população cópia
                        List<double> populacao_para_perturbar = new List<double>(populacao_atual);
                        // Obtém o intervalo de variação dela
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
                                fx_depois_da_perturbacao = calcula_valor_funcao_objetivo(populacao_para_perturbar, true),
                                populacao_depois_da_perturbacao = new List<double>(populacao_para_perturbar),
                                feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_para_perturbar, upper_bounds, lower_bounds),
                                indice_variavel_projeto = i,
                            }
                        );
                    }
                }
            }

            // Foram geradas P perturbações para todas as (N+1) variáveis de projeto (porcentagem incluida)
        }





        public override void ordena_e_perturba()
        {
            // Para cada variável, escolhe uma das P perturbações para confirmar


            // Para cada variável, confirma uma perturbação
            List<int> indices_variaveis = Enumerable.Range(0, n_variaveis_projeto).ToList();
            indices_variaveis.Add(999);
            foreach(int i in indices_variaveis)
            {
                // Obtem somente as perturbações realizadas naquela variável
                List<Perturbacao> perturbacoes_da_variavel = new List<Perturbacao>();
                perturbacoes_da_variavel = perturbacoes_da_iteracao.Where(p => p.indice_variavel_projeto == i).ToList();
               
                // Ordena as perturbações com base no f(x)
                perturbacoes_da_variavel.Sort(
                    delegate(Perturbacao b1, Perturbacao b2) { 
                        return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); 
                    }
                );

                // Verifica as probabilidades até que uma das perturbações dessa variável seja aceita
                while (true)
                {
                    // Gera um número aleatório com distribuição uniforme entre 0 e 1
                    double ALE = random.NextDouble();

                    // Determina a posição do ranking escolhida, entre 1 e o número de variáveis. +1 é 
                    // ...porque tem que ser de 1 até menor que o 2º parámetro de .Next()
                    int k = random.Next(1, perturbacoes_da_variavel.Count+1);
                    
                    // Probabilidade Pk => k^(-tau)
                    double Pk = Math.Pow(k, -tau);

                    // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                    k -= 1;

                    // Se o Pk é maior ou igual ao aleatório, então confirma a perturbação
                    if (Pk >= ALE)
                    {
                        // Obtém o índice da perturbação escolhida pra aceitar
                        int indice = perturbacoes_da_variavel[k].indice_variavel_projeto;
                        // Obtém o valor da variável depois de perturbar
                        double xii_depois_perturbar = perturbacoes_da_variavel[k].xi_depois_da_perturbacao;


                        
                        // Se indice é 999, atualiza porcentagem
                        if (indice == 999){
                            this.porcentagem = xii_depois_perturbar;
                        }
                        // Senão, atualiza valor da variável de projeto
                        else{
                            populacao_atual[indice] = xii_depois_perturbar;
                        }



                        // Sai do laço while
                        break;
                    }
                }
            }

            // Depois que aceitou uma perturbação de cada variável, precisa calcular o fx_atual novamente
            fx_atual = calcula_valor_funcao_objetivo(this.populacao_atual, true);
        }
    }
}
