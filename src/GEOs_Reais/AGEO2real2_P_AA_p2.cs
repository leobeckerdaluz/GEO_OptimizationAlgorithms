using System;
using System.Collections.Generic;
using System.Linq;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class AGEO2real2_P_AA_p2 : AGEO2real2
    {
        public double p1 {get; set;}
        
        // Escolhe uma perturbação por variável

        public AGEO2real2_P_AA_p2(
            List<double> populacao_inicial,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados) : base(
                populacao_inicial,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
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
            this.s = 10;
            this.std = 99999;
            this.tau = 99999;
            
            this.p1 = new MathNet.Numerics.Distributions.LogNormal(4, 0.2).Sample();
            
            // double alfa = 1 / Math.Sqrt(n_variaveis_projeto);
        }
          


        public override void verifica_perturbacoes()
        {
            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();

            double p = this.p1;



            // Perturba cada variável
            for(int i=0; i<n_variaveis_projeto; i++)
            {
                // Define a p1 inicial
                p = this.p1;

                // Realiza P perturbações na variável com diferentes desvios padrão
                for(int j=0; j<this.P; j++)
                {
                    // Cria uma população cópia
                    List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                    // ----------------------------------------------------------------------------
                    // Perturba a variável
                    double xi = populacao_para_perturbar[i];
                    double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                    // double xii = xi;
                    
                    // // Verifica se quer que a 1ª das P pertuabções seja perturbação uniforme
                    // if(primeira_das_P_perturbacoes_uniforme && j==0)
                    // {
                    //     // Perturbação será um valor qualquer no intervalo de variação da variável
                    //     MathNet.Numerics.Distributions.ContinuousUniform UniDist = new MathNet.Numerics.Distributions.ContinuousUniform();
                    //     xii = lower_bounds[i] + intervalo_variacao_variavel*UniDist.Sample();
                    // }
                    // else
                    // {
                    //     xii = perturba_variavel(xi, p, (int)EnumTipoPerturbacao.perturbacao_porcentagem, intervalo_variacao_variavel);
                        
                    double sigma = p/100.0 * intervalo_variacao_variavel;
                    MathNet.Numerics.Distributions.Normal DistNormal = new MathNet.Numerics.Distributions.Normal(0, sigma);
                    double xii = xi + DistNormal.Sample();
                    // }
                    // ----------------------------------------------------------------------------

                    // Atribui a variável perturbada na população cópia
                    populacao_para_perturbar[i] = xii;

                    // Calcula f(x) com a variável perturbada
                    double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar, true);

                    // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                    Perturbacao perturbacao = new Perturbacao();
                    perturbacao.xi_antes_da_perturbacao = xi;
                    perturbacao.xi_depois_da_perturbacao = xii;
                    perturbacao.populacao_depois_da_perturbacao = new List<double>(populacao_para_perturbar);
                    // perturbacao.porcentagem_usada_nessa_perturb = p1;
                    perturbacao.fx_depois_da_perturbacao = fx;
                    perturbacao.indice_variavel_projeto = i;

                    // Adiciona na lista de perturbações
                    perturbacoes_da_iteracao.Add(perturbacao);
                    
                    p = p / this.s;   
                }

            }





            // --------------------------------------------------------------------------------------------------------
            // Calcula a adaptabilidade do p1'

            double rand_lognormal = new MathNet.Numerics.Distributions.LogNormal(4, 0.2).Sample();
            double p1_linha = rand_lognormal;
        
            p = p1_linha;
            
            List<double> populacao_copia = new List<double>(populacao_atual);

            for(int j=0; j<this.P; j++){
                // Pra cada P, perturba todas variáveis de projeto de uma só vez
                for(int i=0; i<n_variaveis_projeto; i++){
                    
                    double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                    
                    double sigma = p/100.0 * intervalo_variacao_variavel;
                    
                    MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, sigma);
                    
                    double rand_normal = normalDist.Sample();
                    
                    double xi = populacao_atual[i];
                    
                    double xi_perturbado = xi + rand_normal;
                    
                    populacao_copia[i] = xi_perturbado;
                }

                // Depois de toda população perturbada com esse p que está sendo dividido por s, calcula o f(x)
                // ... que é a adaptabilidade do p1'
                double fx_adaptabilidade_porcentagem = calcula_valor_funcao_objetivo(populacao_copia, true);

                // Cria as informações da perturbação da porcentagem
                Perturbacao info_perturbacao_porcent = new Perturbacao();
                info_perturbacao_porcent.xi_antes_da_perturbacao = this.p1;
                info_perturbacao_porcent.xi_depois_da_perturbacao = p1_linha;
                info_perturbacao_porcent.populacao_depois_da_perturbacao = new List<double>(populacao_copia);
                // info_perturbacao_porcent.porcentagem_usada_nessa_perturb = porcentagem_linha;
                info_perturbacao_porcent.fx_depois_da_perturbacao = fx_adaptabilidade_porcentagem;
                info_perturbacao_porcent.indice_variavel_projeto = 999;

                
                // Adiciona na lista de perturbações
                perturbacoes_da_iteracao.Add(info_perturbacao_porcent);
                
                
                p = p / this.s;
            }
        }





        public override void ordena_e_perturba()
        {
            
            // Cria uma lista contendo todas as variáveis de projeto e a p1
            List<int> indices_variaveis = Enumerable.Range(0,n_variaveis_projeto).ToList();
            indices_variaveis.Add(999);



            // Para cada variável, confirma uma perturbação
            // for(int i=0; i<n_variaveis_projeto; i++)
            foreach (int i in indices_variaveis)
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
                        int indice = perturbacoes_da_variavel[k].indice_variavel_projeto;
                        
                        double xii_depois_perturbar = perturbacoes_da_variavel[k].xi_depois_da_perturbacao;
                        
                        List<double> populacao_depois_perturbar = new List<double>(perturbacoes_da_variavel[k].populacao_depois_da_perturbacao);

                        
                        // Se o índice escolhido para ser perturbado é o da porcentagem, atualiza a porcentagem
                        // porcentagem foi auto-adaptada
                        if(indice == 999){
                            this.p1 = xii_depois_perturbar;
                        }
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
