// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION_OVERLIMIT
// #define NOVO_MELHOR_FX

using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    public class GEO_real1
    {
        public Random random = new Random();

        public double tau {get; set;}
        public int n_variaveis_projeto {get; set;}
        public int definicao_funcao_objetivo {get; set;}
        public List<RestricoesLaterais> restricoes_laterais_variaveis {get; set;}
        public int step_obter_NFOBs {get; set;}
        public double std {get; set;}
        public int tipo_perturbacao_original_ou_SDdireto {get; set;}

        public int NFOB {get; set;}
        public double fx_atual {get; set;}
        public double fx_melhor {get; set;}
        public List<double> populacao_atual {get; set;}
        public List<double> populacao_melhor {get; set;}
        public List<double> melhores_NFOBs {get; set;}
        public List<Perturbacao> perturbacoes_da_iteracao {get; set;}


        public GEO_real1(List<double> populacao_inicial, double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais_variaveis, int step_obter_NFOBs, double std, int tipo_perturbacao_original_ou_SDdireto){
            this.tau = tau;
            this.n_variaveis_projeto = n_variaveis_projeto;
            this.definicao_funcao_objetivo = definicao_funcao_objetivo;
            this.restricoes_laterais_variaveis = restricoes_laterais_variaveis;
            this.step_obter_NFOBs = step_obter_NFOBs;
            this.std = std;
            this.tipo_perturbacao_original_ou_SDdireto = tipo_perturbacao_original_ou_SDdireto;
            
            this.NFOB = 0;
            this.populacao_atual = new List<double>(populacao_inicial);
            this.populacao_melhor = new List<double>(populacao_inicial);
            this.fx_atual = calcula_valor_funcao_objetivo(populacao_inicial);
            this.fx_melhor = this.fx_atual;
            this.melhores_NFOBs = new List<double>();
            this.perturbacoes_da_iteracao = new List<Perturbacao>();
        }


        public virtual void add_NFOB(){
            NFOB++;

            #if DEBUG_CONSOLE
                Console.WriteLine("NFOB incrementado = {0}", NFOB);
            #endif

            if (NFOB % step_obter_NFOBs == 0){
                melhores_NFOBs.Add(fx_melhor);
                #if DEBUG_CONSOLE    
                    Console.WriteLine("melhor NFOB {0} = {1}", NFOB, fx_melhor);
                #endif
            }
        }


        public virtual double calcula_valor_funcao_objetivo(List<double> fenotipos){
            // Calcula o valor da função objetivo com o fenótipo desejado
            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipos, this.definicao_funcao_objetivo);
            
            add_NFOB();
            
            double penalidade = 0;

            const double grau_penalidade = 2;

            // Verifica a penalidade para cada variável do fenótipo desejado
            for(int i=0; i<fenotipos.Count; i++){
                double limite_inferior = restricoes_laterais_variaveis[i].limite_inferior_variavel;
                double limite_superior = restricoes_laterais_variaveis[i].limite_superior_variavel;
                double xi = fenotipos[i];

                // Verifica se a variável está fora dos limites
                if (xi < limite_inferior){
                    double penalidade_inferior = grau_penalidade * Math.Pow(xi - limite_inferior, 2);
                    
                    penalidade += penalidade_inferior;

                    #if DEBUG_FUNCTION_OVERLIMIT
                        Console.WriteLine("Fora do limite inferior! xi = {0} e limite inferior = {1}. Penalidade = {2}", xi, limite_inferior, penalidade_inferior);
                    #endif
                }
                else if (xi > limite_superior){
                    double penalidade_superior = grau_penalidade * Math.Pow(xi - limite_superior, 2);
                    
                    penalidade += penalidade_superior;

                    #if DEBUG_FUNCTION_OVERLIMIT
                        Console.WriteLine("Fora do limite superior! xi = {0} e limite superior = {1}. Penalidade = {2}", xi, limite_superior, penalidade_superior);
                    #endif
                }
            }

            double penalidade_aplicada = fx + penalidade;

            #if DEBUG_FUNCTION
            Console.WriteLine("Penalidade total = {0} aplicada na fx = {1} transforma em {2}", penalidade, fx, penalidade_aplicada);
            #endif

            return penalidade_aplicada;
        }
        
       
        public virtual void verifica_perturbacoes(){

            #if DEBUG_CONSOLE
                Console.WriteLine("===================================================");
            #endif

            // Inicia a lista de perturbações zerada
            List<Perturbacao> perturbacoes = new List<Perturbacao>();
            
            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++){
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                double xi = populacao_para_perturbar[i];
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, this.std);
                
                double xii = xi;
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

                // Avalia se a perturbação é a melhor de todas
                if (fx < fx_melhor){

                    #if NOVO_MELHOR_FX
                        Console.WriteLine("AVALIAÇÃO: fx = {0} < fx_melhor = {1}", fx, fx_melhor);
                    #endif

                    fx_melhor = fx;
                    populacao_melhor = populacao_para_perturbar;
                }


                // Cria o objeto perturbação
                Perturbacao perturbacao = new Perturbacao();
                perturbacao.xi_antes_da_perturbacao = xi;
                perturbacao.xi_depois_da_perturbacao = xii;
                perturbacao.fx_depois_da_perturbacao = fx;
                perturbacao.indice_variavel_projeto = i;

                // Adiciona na lista de perturbações
                perturbacoes.Add(perturbacao);
            }

            // Atualiza a lista de perturbações da iteração
            perturbacoes_da_iteracao = perturbacoes;
        }


        public virtual void mutacao_do_tau_AGEOs(){}



        public virtual void ordena_e_perturba(){
            
            #if DEBUG_CONSOLE
                Console.WriteLine("Agora que criou as perturbações, mostra as perturbações:");
                foreach(Perturbacao p in perturbacoes_da_iteracao){
                    Console.WriteLine("Perturbação na var {0} com novo fx = {1} , xi antes = {2} e xi depois = {3}", p.indice_variavel_projeto, p.fx_depois_da_perturbacao, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao);
                }
            #endif

            // Ordena as perturbações com base no f(x)
            perturbacoes_da_iteracao.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); });

            #if DEBUG_CONSOLE
                Console.WriteLine("Perturbações depois de ordenar:");
                foreach(Perturbacao p in perturbacoes_da_iteracao){
                    Console.WriteLine("Perturbação na var {0} com novo fx = {1} , xi antes = {2} e xi depois = {3}", p.indice_variavel_projeto, p.fx_depois_da_perturbacao, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao);
                }
            #endif


            // Verifica as probabilidades até que uma variável seja perturbada
            while (true){

                // Gera um número aleatório com distribuição uniforme
                double ALE = random.NextDouble();

                // k é o índice da população de bits ordenada
                int k = random.Next(1, populacao_atual.Count+1);
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -tau);

                // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                k -= 1;

                #if DEBUG_CONSOLE
                    Console.WriteLine("Gerou ALE = {0} e Pk = {1} para k = {2}", ALE, Pk, k+1);
                #endif

                // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                if (Pk >= ALE){

                    // Lá na variável k, coloca o novo xi
                    populacao_atual[ perturbacoes_da_iteracao[k].indice_variavel_projeto ] = perturbacoes_da_iteracao[k].xi_depois_da_perturbacao;

                    #if DEBUG_CONSOLE
                        Console.WriteLine("Perturbou populacao no indice {0} para o valor {1}", perturbacoes_da_iteracao[k].indice_variavel_projeto, perturbacoes_da_iteracao[k].xi_depois_da_perturbacao);
                        Console.WriteLine("Novo f(x) tem que dar {0}", perturbacoes_da_iteracao[k].fx_depois_da_perturbacao);
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
        

        public virtual RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada){
            while(true){
                verifica_perturbacoes();
                mutacao_do_tau_AGEOs();
                ordena_e_perturba();

                // Verifica o critério de parada
                bool parada_por_precisao = ( Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado)) <= parametros_criterio_parada.PRECISAO_criterio_parada );
                bool parada_por_NFOB = (NFOB >= parametros_criterio_parada.NFOB_criterio_parada);
                
                // Antes de verificar a parada, começa com falso.
                bool parada = false;

                // Se o critério for por NFOB...
                if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB){
                    if (parada_por_NFOB){
                        parada = true;
                    }
                }
                // Se o critério for por precisão...
                else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAO){
                    if (parada_por_precisao){
                        parada = true;
                    }
                }
                // Se o critério for por precisão ou por NFOB...
                else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB){
                    if (parada_por_NFOB || parada_por_precisao){
                        parada = true;
                    }
                }

                // Se a parada foi atingida, retorna
                if (parada){
                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFOB = this.NFOB;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFOBs = this.melhores_NFOBs;
                    retorno.populacao_final = this.populacao_melhor;
                    
                    return retorno;
                }
            }
        } 
    }
}