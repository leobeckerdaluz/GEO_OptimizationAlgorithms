using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class AGEO2real1_autoadap_s : AGEO2real1
    {
        public double sigma {get; set;}
        
        
        public AGEO2real1_autoadap_s(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            double std_ou_porcentagem,
            int tipo_perturbacao) : base(
                populacao_inicial,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                std_ou_porcentagem,
                tipo_perturbacao)
        {
            

            this.sigma = 1;
            

        }
          


        public override void verifica_perturbacoes()
        {
            // Inicia a lista de perturbações zerada
            List<Perturbacao> perturbacoes = new List<Perturbacao>();
            



            // ------------------------------------------------------------------------------------
            // Antes de perturbar as variáveis, perturba o sigma

            // // Porcentagem atual
            // double porcentagem_atual = this.std;
            // double intervalo_variaveis =  
            
            double alfa = 1.0 / Math.Sqrt(n_variaveis_projeto);
            
            
            MathNet.Numerics.Distributions.LogNormal lognormal_dist = new MathNet.Numerics.Distributions.LogNormal(0, alfa);
            double rand_lognormal = lognormal_dist.Sample();
            
            double sigma_linha = sigma * rand_lognormal;
            
            MathNet.Numerics.Distributions.Normal normalDist_com_sigmalinha = new MathNet.Numerics.Distributions.Normal(0, sigma_linha);
            
            // Calcula a adaptabilidade do sigma. Para isso, cria uma cópia da população atual para perturbar todas as variáveis
            List<double> populacao_copia = new List<double>(populacao_atual);
            // Perturba todas as variáveis com aquele sigma perturbado
            for(int i=0; i<populacao_atual.Count; i++){
                double xi = populacao_atual[i];
                double xi_perturbado = xi + normalDist_com_sigmalinha.Sample();
                populacao_copia[i] = xi_perturbado;
            }
            double fx_adaptabilidade_sigma = calcula_valor_funcao_objetivo(populacao_copia, false);

            // Cria as informações da perturbação do sigma
            Perturbacao info_perturbacao_sigma = new Perturbacao();
            info_perturbacao_sigma.xi_antes_da_perturbacao = sigma;
            info_perturbacao_sigma.xi_depois_da_perturbacao = sigma_linha;
            info_perturbacao_sigma.fx_depois_da_perturbacao = fx_adaptabilidade_sigma;
            info_perturbacao_sigma.indice_variavel_projeto = 999;

            // Adiciona essa info da perturbação do sigma na lista de perturbações
            perturbacoes.Add(info_perturbacao_sigma);
            // ------------------------------------------------------------------------------------






            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++)
            {
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                // Perturba a variável
                double xi = populacao_para_perturbar[i];
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                
                

                
                // double porcent = 5;
                // double sigma_normal = porcent/100.0 * intervalo_variacao_variavel;
                MathNet.Numerics.Distributions.Normal normal = new MathNet.Numerics.Distributions.Normal(0, sigma);
                double xii = xi + normal.Sample();
                // double xii = perturba_variavel(xi, this.std, this.tipo_perturbacao, intervalo_variacao_variavel);





                // Atribui a variável perturbada na população cópia
                populacao_para_perturbar[i] = xii;

                // Calcula f(x) com a variável perturbada
                double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);
                

                // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                Perturbacao perturbacao = new Perturbacao();
                perturbacao.xi_antes_da_perturbacao = xi;
                perturbacao.xi_depois_da_perturbacao = xii;
                perturbacao.fx_depois_da_perturbacao = fx;
                perturbacao.indice_variavel_projeto = i;

                // Adiciona na lista de perturbações
                perturbacoes.Add(perturbacao);
            }



           

            // Todas essas perturbações realizadas na população são as perturbações da iteração a 
            // ...serem processadas posteriormente.
            perturbacoes_da_iteracao = perturbacoes;
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
                int k = random.Next(1, populacao_atual.Count+1    +1    );
                
                
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -tau);

                // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                k -= 1;

                // Se o Pk é maior ou igual ao aleatório, então confirma a perturbação
                if (Pk >= ALE)
                {
                    
                    // Obtém o índice da perturbação escolhida pra aceitar
                    int indice = perturbacoes_da_iteracao[k].indice_variavel_projeto;
                    double xii_depois_perturbar = perturbacoes_da_iteracao[k].xi_depois_da_perturbacao;
                    double fx_depois_perturbar = perturbacoes_da_iteracao[k].fx_depois_da_perturbacao;

                    

                    // Se o índice é 999, muda o sigma, senão muda na população
                    if (indice == 999){
                        sigma = xii_depois_perturbar;
                    }
                    else{
                        populacao_atual[indice] = xii_depois_perturbar;
                    }
                    



                    // Atualiza o f(x) atual com o perturbado
                    fx_atual = fx_depois_perturbar;

                    // Sai do laço while
                    break;
                }
            }
        }
        
        
        
        
        public override void mutacao_do_tau_AGEOs()
        {
            // Obtém o tamanho da população de bits
            int tamanho_populacao = this.populacao_atual.Count;
            
            // Instancia as funções úteis do mecanismo A-GEO
            MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();
            
            // Calcula o f(x) de referência
            double fx_referencia = mecanismo.calcula_fx_referencia(tipo_AGEO, fx_melhor, fx_atual);
            
            // Calcula o CoI
            double CoI = mecanismo.calcula_CoI_real(perturbacoes_da_iteracao, fx_referencia, tamanho_populacao);
            
            
            
            // VERSÃO ORIGINAL DE ATUALIZAR O TAU
            // Atualiza o tau
            tau = mecanismo.obtem_novo_tau(this.tipo_AGEO, this.tau, CoI, this.CoI_1, tamanho_populacao);
            
            
                
            // Armazena o CoI atual para ser usado como o anterior na próxima iteração
            this.CoI_1 = CoI;
        }
    }
}
