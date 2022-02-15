using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace GEOs_REAIS
{
    public class AGEO2real2_P_AA_p0 : AGEO2real2
    {
        public double porcentagem {get; set;}
        
        public AGEO2real2_P_AA_p0(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados) : base(
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
                9999)
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
                            // Calcula o sigma que será utilizado na distribuição normal
                            double sigma = porcentagem_linha/100.0 * intervalo_variacao_variavel;
                            // Obtém um valor aleatório da dist normal
                            double rand_normal = new MathNet.Numerics.Distributions.Normal(0, sigma).Sample();
                            // Obtém o valor atual dessa variável
                            double xi = populacao_atual[k];
                            // A perturbação vai ser a variável atual mais um valor da distribuição normal
                            double xi_perturbado = xi + rand_normal;
                            // Atualiza o valor dessa variável na população cópia
                            populacao_copia[k] = xi_perturbado;
                        }
                        // Depois de toda população perturbada, calcula o f(x) que é a adaptabilidade do porcentagem linha
                        double fx_adaptabilidade_porcentagem = calcula_valor_funcao_objetivo(populacao_copia, true);
                        // Cria as informações da perturbação da porcentagem
                        Perturbacao info_perturbacao_porcent = new Perturbacao();
                        info_perturbacao_porcent.xi_antes_da_perturbacao = this.porcentagem;
                        info_perturbacao_porcent.xi_depois_da_perturbacao = porcentagem_linha;
                        info_perturbacao_porcent.fx_depois_da_perturbacao = fx_adaptabilidade_porcentagem;
                        info_perturbacao_porcent.indice_variavel_projeto = 999;
                        // Adiciona essa info da perturbação da porcentagem na lista de perturbações
                        perturbacoes_da_iteracao.Add(info_perturbacao_porcent);
                        // ------------------------------------------------------------------------------------
                    }
                    
                    // SENÃO, PERTURBA A VARIÁVEL DE PROJETO COM O p'
                    else{
                        // Cria uma população cópia
                        List<double> populacao_para_perturbar = new List<double>(populacao_atual);
                        // Obtém o valor da variável atual e o intervalo de variação dela
                        double xi = populacao_atual[i];
                        double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                        // Calcula o sigma com a porcentagem linha e perturba a variável
                        double sigma = this.porcentagem/100.0 * intervalo_variacao_variavel;
                        double xi_perturbado = xi + new MathNet.Numerics.Distributions.Normal(0, sigma).Sample();
                        // Atribui a variável perturbada na população cópia
                        populacao_para_perturbar[i] = xi_perturbado;
                        // Calcula f(x) com a variável perturbada
                        double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);
                        // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                        Perturbacao perturbacao = new Perturbacao();
                        perturbacao.xi_antes_da_perturbacao = xi;
                        perturbacao.xi_depois_da_perturbacao = xi_perturbado;
                        perturbacao.fx_depois_da_perturbacao = fx;
                        perturbacao.indice_variavel_projeto = i;
                        // Adiciona na lista de perturbações
                        perturbacoes_da_iteracao.Add(perturbacao);
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
            fx_atual = calcula_valor_funcao_objetivo(this.populacao_atual, false);
        }
    }
}
