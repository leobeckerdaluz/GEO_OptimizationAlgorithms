using System;
using System.Collections.Generic;
using Classes_e_Enums;
using System.Linq;

namespace GEOs_REAIS
{
    public class GEO_real2 : GEO_real1
    {
        public int P {get; set;}
        public int s {get; set;}
        public int tipo_variacao_std_nas_P_perturbacoes {get; set;}
        public bool primeira_das_P_perturbacoes_uniforme {get; set;}
        
        public GEO_real2(
            List<double> populacao_inicial,
            double tau,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            bool primeira_das_P_perturbacoes_uniforme,
            int tipo_variacao_std_nas_P_perturbacoes,
            double std,
            int tipo_perturbacao,
            int P,
            int s,
            bool round_current_population_every_it) : base(
                n_variaveis_projeto,
                function_id,
                populacao_inicial,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                tipo_perturbacao,
                tau,
                std,
                round_current_population_every_it)
        {
            this.P = P;
            this.s = s;
            this.tipo_variacao_std_nas_P_perturbacoes = tipo_variacao_std_nas_P_perturbacoes;
            this.primeira_das_P_perturbacoes_uniforme = primeira_das_P_perturbacoes_uniforme;
        }

        public override void verifica_perturbacoes()
        {
            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();

            // Perturba cada variável
            for(int i=0; i<n_variaveis_projeto; i++)
            {
                // Inicia a lista de perturbações dessa variável zerada
                List<Perturbacao> perturbacoes = new List<Perturbacao>();

                // Define o desvio padrão inicial
                double std_atual = this.std;

                // Realiza P perturbações na variável com diferentes desvios padrão
                for(int j=0; j<this.P; j++)
                {
                    // Cria uma população cópia
                    List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                    // ----------------------------------------------------------------------------
                    // Perturba a variável
                    double xi = populacao_para_perturbar[i];
                    double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                    double xii = xi;
                    
                    // Verifica se quer que a 1ª das P pertuabções seja perturbação uniforme
                    if(primeira_das_P_perturbacoes_uniforme && j==0)
                    {
                        // Perturbação será um valor qualquer no intervalo de variação da variável
                        MathNet.Numerics.Distributions.ContinuousUniform UniDist = new MathNet.Numerics.Distributions.ContinuousUniform();
                        xii = lower_bounds[i] + intervalo_variacao_variavel*UniDist.Sample();
                    }
                    else
                    {
                        xii = perturba_variavel(xi, std_atual, this.tipo_perturbacao, intervalo_variacao_variavel);
                    }
                    // ----------------------------------------------------------------------------

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

                    // Só atualiza o std se não quiser que uma das P perturbações seja uniforme ou caso queira, que a iteração não seja a 1ª
                    if (!primeira_das_P_perturbacoes_uniforme || j!=0){
                        // Atualiza o std pra próxima perturbação da variável
                        if (this.tipo_variacao_std_nas_P_perturbacoes == (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_real_original){
                            // std(i+1) = std(i) / (s*i) ===> originalmente, s=2
                            std_atual = std_atual / ((this.s)*(j+1)) ;
                        }
                        else if (this.tipo_variacao_std_nas_P_perturbacoes == (int)EnumTipoVariacaoStdNasPPerturbacoes.variacao_divide_por_s){
                            // std(i+1) = std(i) / s
                            std_atual = std_atual / this.s;
                        }
                    }
                }
                
                // Adiciona cada perturbação dessa variável na lista geral de perturbacoes da iteração
                foreach (Perturbacao p in perturbacoes)
                {
                    perturbacoes_da_iteracao.Add(p);
                }
            }
        }


        public override void ordena_e_perturba()
        {
            // Para cada variável, confirma uma perturbação
            for(int i=0; i<n_variaveis_projeto; i++)
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
                        // Coloca o novo xi lá na variável escolhida do ranking
                        populacao_atual[perturbacoes_da_variavel[k].indice_variavel_projeto] = perturbacoes_da_variavel[k].xi_depois_da_perturbacao;

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
