// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE

using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_REAIS
{
    public class GEO_real2 : GEO_real1
    {
        public int P {get; set;}
        
        public GEO_real2(List<double> populacao_inicial, double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais_variaveis, int step_obter_NFOBs, double std, int tipo_perturbacao_original_ou_SDdireto, int P) : base(populacao_inicial, tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_variaveis, step_obter_NFOBs, std, tipo_perturbacao_original_ou_SDdireto){
            this.P = P;
        }

        
        public override void verifica_perturbacoes(){

            // Limpa a lista com perturbações da iteração
            perturbacoes_da_iteracao = new List<Perturbacao>();

            // Verifica a perturbação para cada variável
            for(int i=0; i<n_variaveis_projeto; i++){
                // Inicia a lista de perturbações zerada
                List<Perturbacao> perturbacoes = new List<Perturbacao>();

                double std_atual = this.std;

                for(int j=0; j<this.P; j++){
                    // Cria uma população cópia
                    List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                    double xi = populacao_para_perturbar[i];
                    MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_atual);
                    
                    double xii = xi;

                    // Perturba a variável dentro dos limites
                    if (tipo_perturbacao_original_ou_SDdireto == (int)EnumTipoPerturbacao.perturbacao_original){
                        xii = xi + (normalDist.Sample() * xi);
                    }
                    else if (tipo_perturbacao_original_ou_SDdireto == (int)EnumTipoPerturbacao.perturbacao_SDdireto){
                        xii = xi + normalDist.Sample();
                    }

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
                    double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar);
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

                    perturbacoes.Add(perturbacao);
                
                    // Atualiza o novo std ===> std(i+1) = std(i) / (s*i)
                    // Onde i = 1,2...P e s é arbitrário e vale 2.
                    std_atual = std_atual / (2*(j+1));
                }

                #if DEBUG_CONSOLE
                    Console.WriteLine("Motrando as perturbacoes:");
                    foreach( Perturbacao p in perturbacoes ){
                        Console.WriteLine("{0}: xi = {1} | xii = {2} | fx = {3}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao, p.fx_depois_da_perturbacao);
                    }
                #endif

                
                // Adiciona cada perturbação na lista geral de perturbacoes
                // Console.WriteLine("perturbações.Count = {0}", perturbacoes.Count);
                for(int k=0; k<perturbacoes.Count; k++){
                    perturbacoes_da_iteracao.Add( perturbacoes[k] )    ;
                }
            }
        }


        public override void ordena_e_perturba(){
            
            for(int i=0; i<n_variaveis_projeto; i++){
                // Obtem somente as perturbações da variável
                List<Perturbacao> perturbacoes_da_variavel = new List<Perturbacao>();
                perturbacoes_da_variavel = perturbacoes_da_iteracao.Where(p => p.indice_variavel_projeto == i).ToList();
                
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

                    // Console.WriteLine("NÃO DEU PAU ATÉ AQUI!");

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){
                        // Lá na variável i, coloca o novo xi
                        populacao_atual[ i ] = perturbacoes_da_variavel[k].xi_depois_da_perturbacao;

                        #if DEBUG_CONSOLE
                            Console.WriteLine("Perturbou populacao no indice {0} para o valor {1}", perturbacoes_da_variavel[k].indice_variavel_projeto, perturbacoes_da_variavel[k].xi_depois_da_perturbacao);
                            Console.WriteLine("Novo f(x) tem que dar {0}", perturbacoes_da_variavel[k].fx_depois_da_perturbacao);
                        #endif


                        // Atualiza o f(x) atual com o perturbado
                        fx_atual = perturbacoes_da_variavel[k].fx_depois_da_perturbacao;

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
