using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class AGEO2real2_AA3 : AGEO2real2
    {
        public double p_inicial_peq {get; set;}
        
        public AGEO2real2_AA3(
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
                9999,
                9999,
                9999,
                integer_population)
        {
            this.P = 9;
            this.s = 10;
            this.std = 99999;
            this.tau = 0.5;
            
            // this.p_inicial_peq = new MathNet.Numerics.Distributions.LogNormal(1, 0.67).Sample()/1000;
            this.p_inicial_peq = 1E-3 + (1E-2 - 1E-3)*new MathNet.Numerics.Distributions.ContinuousUniform().Sample();
        }



        private bool populacao_viavel(List<double> populacao_para_perturbar, List<double> lower_bounds, List<double> upper_bounds)
        {
            for (int i=0; i<populacao_para_perturbar.Count; i++){
                double xi = populacao_para_perturbar[i];
                double ul = upper_bounds[i];
                double ll = lower_bounds[i];

                if (xi<ll || xi>ul){
                    return false;
                }
            }
            return true;
        }
          



        public override void verifica_perturbacoes()
        {
            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();
            
            // Lista com todas as variáveis de projeto (incluindo porcentagem de índice 999)
            List<int> indices_variaveis = Enumerable.Range(0, n_variaveis_projeto).ToList();
            indices_variaveis.Add(999);


            // Gera a lista de p_linhas para as P iterações
            List<double> p_linhas = new List<double>();
            p_linhas.Add(this.p_inicial_peq * 10000);
            p_linhas.Add(this.p_inicial_peq * 1000);
            p_linhas.Add(this.p_inicial_peq * 100);
            p_linhas.Add(this.p_inicial_peq * 10);
            p_linhas.Add(this.p_inicial_peq * 1);
            p_linhas.Add(this.p_inicial_peq / 10);
            p_linhas.Add(this.p_inicial_peq / 100);
            p_linhas.Add(this.p_inicial_peq / 1000);
            p_linhas.Add(this.p_inicial_peq / 10000);

            
            // PARA CADA P...
            for(int j=0; j<this.P; j++){
                
                // Valor da porcentagem p a ser utilizada:
                double p = p_linhas[j];
                
                // PARA CADA VARIÁVEL...
                foreach (int i in indices_variaveis){

                    // SE A VARIÁVEL É A PORCENTAGEM (id=999), ENTÃO PERTURBA TODAS VARIÁVEIS AO MESMO TEMPO E CALCULA FX
                    if(i == 999){
                        // Obtém um valor aleatório entre 10^-4 e 10^-1
                        const double lim_inf = 1E-3;
                        const double lim_sup = 1E-2;

                        double rand_uniform = new MathNet.Numerics.Distributions.ContinuousUniform().Sample();
                        double ro_linha = lim_inf + rand_uniform*(lim_sup - lim_inf);
                        
                        // Cria uma cópia da população atual para perturbar todas as variáveis
                        List<double> populacao_copia = new List<double>(populacao_atual);
                        // Perturba todas as variáveis com aquela porcentagem ro_linha
                        for(int k=0; k<populacao_atual.Count; k++){
                            // Calcula o invervalo de variação dessa variável
                            double intervalo_variacao_variavel = Math.Abs(upper_bounds[k] - lower_bounds[k]);
                            // Perturba a variável
                            populacao_copia[k] = perturba_variavel(
                                populacao_atual[k], 
                                ro_linha, 
                                this.tipo_perturbacao,
                                intervalo_variacao_variavel, 
                                integer_population
                            );
                        }

                        // Cria e adiciona essa info da perturbação da porcentagem na lista de perturbações
                        perturbacoes_da_iteracao.Add(
                            new Perturbacao(){
                                xi_antes_da_perturbacao = p_inicial_peq,
                                xi_depois_da_perturbacao = ro_linha,
                                fx_depois_da_perturbacao = calcula_valor_funcao_objetivo(populacao_copia, true),
                                populacao_depois_da_perturbacao = populacao_copia,
                                feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_copia, upper_bounds, lower_bounds),
                                indice_variavel_projeto = 999
                            }
                        );
                        // ------------------------------------------------------------------------------------
                    }
                    
                    // SE FOR OUTRA VARIÁVEL (DE PROJETO), PERTURBA ELA COM O p
                    else{
                        // Cria uma população cópia
                        List<double> populacao_para_perturbar = new List<double>(populacao_atual);
                        // Obtém o valor da variável atual e o intervalo de variação dela
                        double intervalo_variacao_variavel = Math.Abs(upper_bounds[i] - lower_bounds[i]);
                        
                        // Perturba a variável
                        populacao_para_perturbar[i] = perturba_variavel(
                            populacao_atual[i],
                            p,
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
                                populacao_depois_da_perturbacao = populacao_para_perturbar,
                                feasible_solution = CheckFeasibility.CheckFeasibility.check_feasibility(populacao_para_perturbar, upper_bounds, lower_bounds),
                                indice_variavel_projeto = i
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
                perturbacoes_da_variavel.Sort(delegate(Perturbacao b1, Perturbacao b2) { 
                    return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); 
                });
                
                // Se nenhuma perturbação for viável, deixa essa população mesmo
                bool at_least_one_valid = perturbacoes_da_variavel.Any(x => x.feasible_solution == true);
                if (!at_least_one_valid)
                    continue;
                
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

                    // Se o Pk é maior ou igual ao aleatório, então confirma a perturbação
                    if (Pk >= ALE)
                    {
                        // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                        Perturbacao perturbacao_escolhida = perturbacoes_da_variavel[k-1];

                        if (!perturbacao_escolhida.feasible_solution)
                            continue;

                        // Obtém o índice da perturbação escolhida pra aceitar
                        int indice = perturbacao_escolhida.indice_variavel_projeto;
                        // Obtém o valor da variável depois de perturbar
                        double xii_depois_perturbar = perturbacao_escolhida.xi_depois_da_perturbacao;
                        
                        // Se indice é 999, atualiza porcentagem
                        if (indice == 999){
                            this.p_inicial_peq = xii_depois_perturbar;
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

            // Armazena o novo p' na variável std do GEOreal1 a fim de gerar gráficos de std
            this.std = this.p_inicial_peq;
        }
    }
}
