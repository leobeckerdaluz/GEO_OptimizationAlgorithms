// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE

using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_REAIS
{
    public class GEOvar_REAL : GEOsBASE_REAL
    {
        public double std1 {get; set;}
        public int P {get; set;}
        
        public GEOvar_REAL(double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais, int step_obter_NFOBs, double std, double std1, int P) : base(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais, step_obter_NFOBs, std){
            this.std1 = std1;
            this.P = P;
        }


        public override void verifica_perturbacoes()
        {
            // Inicializa a lista de perturbações zerada
            perturbacoes_da_iteracao = new List<Perturbacao>();

            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++){
                double std3 = this.std1;// / (2*1);

                for(int j=0; j<this.P; j++){
                    // Cria uma população cópia
                    List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                    double xi = populacao_para_perturbar[i];
                    MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std3);
                    double xii = xi + normalDist.Sample() * xi;

#if DEBUG_CONSOLE
                    Console.WriteLine("Verificando a variável {0}", i);
                    Console.WriteLine("xi vale {0} e perturbando vai para {1}", xi, xii);
#endif

                    // Atribui a variável perturbada
                    populacao_para_perturbar[i] = xii;

#if DEBUG_CONSOLE
                    Console.WriteLine("População perturbada para calcular fx:");
                    foreach (double ind in populacao_para_perturbar){
                        Console.Write(ind + " | ");
                    }
                    Console.WriteLine("");
#endif

                    // Calcula f(x) com a variável perturbada
                    double fx = funcao_objetivo_aplicando_penalidade(populacao_para_perturbar);
                    add_NFOB();


                    // Avalia se a perturbação gera o melhor f(x) da história
                    if (fx < fx_melhor){
                        fx_melhor = fx;
                        populacao_melhor = populacao_para_perturbar;
                    }

                    // Cria a perturbação e adiciona ela na lista de perturbações da iteração
                    Perturbacao perturbacao = new Perturbacao();
                    perturbacao.xi_antes_da_perturbacao = xi;
                    perturbacao.xi_depois_da_perturbacao = xii;
                    perturbacao.fx_depois_da_perturbacao = fx;
                    perturbacao.indice_variavel_projeto = i;

                    perturbacoes_da_iteracao.Add(perturbacao);
                
                    // Atualiza o novo std
                    std3 /= (2*(j+1));
                }

#if DEBUG_CONSOLE
                foreach( Perturbacao p in perturbacoes_da_iteracao ){
                    Console.WriteLine("{0}: xi = {1} | xii = {2} | fx = {3}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao, p.fx_depois_da_perturbacao);
                }
#endif
            }
        }


        public override void ordena_e_perturba(){
            
            for(int i=0; i<this.n_variaveis_projeto; i++){
                // Obtem somente as perturbações da variável
                List<Perturbacao> perturbacoes_da_variavel = new List<Perturbacao>();
                perturbacoes_da_variavel = perturbacoes_da_iteracao.Where(p => p.indice_variavel_projeto == i).ToList();

                // List<Perturbacao> perturbacoes_da_variavel = new List<Perturbacao>(perturbacoes_da_iteracao);
                
#if DEBUG_CONSOLE
                Console.WriteLine("PERTURBAÇÕES DA VARIÁVEL:");
                foreach( Perturbacao p in perturbacoes_da_variavel ){
                    Console.WriteLine("{0}: xi = {1} | xii = {2} | fx = {3}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao, p.fx_depois_da_perturbacao);
                }
#endif

                // Ordena elas
                perturbacoes_da_variavel.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); });

#if DEBUG_CONSOLE
                Console.WriteLine("PERTURBAÇÕES DA VARIÁVEL ORDENADO:");
                foreach( Perturbacao p in perturbacoes_da_variavel ){
                    Console.WriteLine("{0}: xi = {1} | xii = {2} | fx = {3}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao, p.fx_depois_da_perturbacao);
                }
#endif

                // Perturba a variável
                while (true){

                    // Gera um número aleatório com distribuição uniforme
                    double ALE = random.NextDouble();

                    // k é o índice da população de bits ordenada
                    int k = random.Next(1, perturbacoes_da_variavel.Count+1);
                    
                    // Probabilidade Pk => k^(-tau)
                    double Pk = Math.Pow(k, -tau);

                    // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                    k -= 1;

#if DEBUG_CONSOLE
                    Console.WriteLine("Gerou ALE = {0} e Pk = {1} para k = {2}", ALE, Pk, k+1);
#endif

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){

                        // Lá na variável i, coloca o novo xi
                        populacao_atual[ perturbacoes_da_variavel[k].indice_variavel_projeto ] = perturbacoes_da_variavel[k].xi_depois_da_perturbacao;

#if DEBUG_CONSOLE
                        Console.WriteLine("Perturbou populacao no indice {0} para o valor {1}", perturbacoes_da_variavel[k].indice_variavel_projeto, perturbacoes_da_variavel[k].xi_depois_da_perturbacao);
                        Console.WriteLine("Novo f(x) tem que dar {0}", perturbacoes_da_variavel[k].fx_depois_da_perturbacao);
#endif

                        // Atualiza o f(x) atual com o perturbado
                        fx_atual = perturbacoes_da_iteracao[k].fx_depois_da_perturbacao;

                        // Sai do laço
                        break;
                    }
                }

#if DEBUG_CONSOLE
                Console.WriteLine("População depois de perturbar tudo:");
                foreach(double ind in populacao_atual){
                    Console.WriteLine(ind);
                }
#endif
            }
        }
    }
}
