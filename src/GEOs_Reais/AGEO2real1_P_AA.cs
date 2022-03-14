using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

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
            bool round_current_population_every_it) : base(
                populacao_inicial,
                n_variaveis_projeto,
                function_id,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                std_ou_porcentagem,
                tipo_perturbacao,
                round_current_population_every_it)
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
                // Calcula o sigma que será utilizado na distribuição normal
                double sigma = porcentagem_linha/100.0 * intervalo_variacao_variavel;
                // Obtém um valor da distribuição
                double rand_normal = new MathNet.Numerics.Distributions.Normal(0, sigma).Sample();
                // Obtém o valor atual dessa variável
                double xi = populacao_atual[i];
                // A perturbação vai ser a variável atual mais um valor da distribuição normal
                double xi_perturbado = xi + rand_normal;
                // Atualiza o valor dessa variável na população cópia
                populacao_copia[i] = xi_perturbado;
            }
            // Depois de toda população perturbada, calcula o f(x) que é a adaptabilidade do porcentagem linha
            double fx_adaptabilidade_porcentagem = calcula_valor_funcao_objetivo(populacao_copia, true);
            // Cria as informações da perturbação da porcentagem
            Perturbacao info_perturbacao_porcent = new Perturbacao();
            info_perturbacao_porcent.xi_antes_da_perturbacao = porcentagem;
            info_perturbacao_porcent.xi_depois_da_perturbacao = porcentagem_linha;
            info_perturbacao_porcent.populacao_depois_da_perturbacao = new List<double>(populacao_copia);
            info_perturbacao_porcent.fx_depois_da_perturbacao = fx_adaptabilidade_porcentagem;
            info_perturbacao_porcent.indice_variavel_projeto = 999;
            // Adiciona essa info da perturbação da porcentagem na lista de perturbações
            perturbacoes_da_iteracao.Add(info_perturbacao_porcent);
            // ------------------------------------------------------------------------------------






            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++)
            {
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);
                // Perturba a variável
                double xi = populacao_para_perturbar[i];
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                // Calcula a perturbação
                double sigma = this.porcentagem/100.0 * intervalo_variacao_variavel;
                MathNet.Numerics.Distributions.Normal DistNormal = new MathNet.Numerics.Distributions.Normal(0, sigma);
                double xii = xi + DistNormal.Sample();
                // Atribui a variável perturbada na população cópia
                populacao_para_perturbar[i] = xii;
                // Calcula f(x) com a variável perturbada
                double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);
                // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                Perturbacao perturbacao = new Perturbacao();
                perturbacao.xi_antes_da_perturbacao = xi;
                perturbacao.xi_depois_da_perturbacao = xii;
                perturbacao.populacao_depois_da_perturbacao = new List<double>(populacao_para_perturbar);
                perturbacao.fx_depois_da_perturbacao = fx;
                perturbacao.indice_variavel_projeto = i;
                // Adiciona na lista de perturbações
                perturbacoes_da_iteracao.Add(perturbacao);
            }



           

            // // Todas essas perturbações realizadas na população são as perturbações da iteração a 
            // // ...serem processadas posteriormente.
            // perturbacoes_da_iteracao = perturbacoes;
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
                    // else{
                    //     populacao_atual[indice] = xii_depois_perturbar
                    // }
                    

                    


                    // Atualiza o f(x) atual com o após perturbado
                    fx_atual = fx_depois_perturbar;

                    // Sai do laço while
                    break;
                }
            }
        }
    }
}
