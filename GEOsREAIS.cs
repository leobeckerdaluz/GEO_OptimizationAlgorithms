// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE

using System;
using System.Collections.Generic;
using Funcoes_Definidas;
using System.Linq;
using System.Runtime.InteropServices;
using SpaceDesignTeste;
using Classes_Comuns_Enums;

namespace teste_abstract
{
    public class Perturbacao{
        public double xi_antes_da_perturbacao {get; set;}
        public double xi_depois_da_perturbacao {get; set;}
        public int indice_variavel_projeto {get; set;}
    }


    public class GEOsBASE_REAL
    {
        public Random random = new Random();

        public double tau {get; set;}
        public int n_variaveis_projeto {get; set;}
        public int definicao_funcao_objetivo {get; set;}
        public List<RestricoesLaterais> restricoes_laterais_variaveis {get; set;}
        public int step_obter_NFOBs {get; set;}

        public int NFOB {get; set;}
        public double fx_atual {get; set;}
        public double fx_melhor {get; set;}
        public List<double> populacao_atual {get; set;}
        public List<double> populacao_melhor {get; set;}
        public List<double> fenotipo_variaveis_projeto {get; set;}
        public List<double> melhores_NFOBs {get; set;}
        private double std {get; set;}


        public GEOsBASE_REAL(double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais_variaveis, int step_obter_NFOBs, double std){
            this.tau = tau;
            this.n_variaveis_projeto = n_variaveis_projeto;
            this.definicao_funcao_objetivo = definicao_funcao_objetivo;
            this.restricoes_laterais_variaveis = restricoes_laterais_variaveis;
            this.step_obter_NFOBs = step_obter_NFOBs;
            this.std = std;
            
            this.NFOB = 0;
            this.fx_atual = Double.MaxValue;
            this.fx_melhor = Double.MaxValue;
            this.populacao_atual = new List<double>();
            this.populacao_melhor = new List<double>();
            this.fenotipo_variaveis_projeto = Enumerable.Repeat(Double.MaxValue, n_variaveis_projeto).ToList();
            this.melhores_NFOBs = new List<double>();
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


        public virtual double funcao_objetivo_aplicando_penalidade(List<double> fenotipos, int definicao_funcao_objetivo){
            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipos, definicao_funcao_objetivo);
            
            double penalidade = 0;

            for(int i=0; i<populacao_atual.Count; i++){
                double limite_inferior = restricoes_laterais_variaveis[i].limite_inferior_variavel;
                double limite_superior = restricoes_laterais_variaveis[i].limite_superior_variavel;
                double xi = populacao_atual[i];

                // Verifica se a variável está fora dos limites
                if (xi < limite_inferior){
                    double penalidade_inferior = 2 * Math.Pow(xi - limite_inferior, 2);
                    
                    penalidade += penalidade_inferior;

#if DEBUG_FUNCTION
                    Console.WriteLine("Fora do limite inferior! xi = {0} e limite inferior = {1}. Penalidade = {2}", xi, limite_inferior, penalidade_inferior);
#endif

                    return Double.MaxValue;
                }
                else if (xi > limite_superior){
                    double penalidade_superior = 2 * Math.Pow(xi - limite_superior, 2);
                    
                    penalidade += penalidade_superior;

#if DEBUG_FUNCTION
                    Console.WriteLine("Fora do limite superior! xi = {0} e limite superior = {1}. Penalidade = {2}", xi, limite_superior, penalidade_superior);
#endif
                    return Double.MaxValue;
                }
            }

            double penalidade_aplicada = fx + penalidade;

#if DEBUG_FUNCTION
            Console.WriteLine("Penalidade total = {0} aplicada na fx = {1} transforma em {2}", penalidade, fx, penalidade_aplicada);
#endif

            return penalidade_aplicada;
        }
        

        public virtual void mutacao_do_tau_AGEOs(){}


        public virtual void geracao_populacao()
        {
            // Console.WriteLine("{0} {1} {2} {3}", restricoes_laterais_variaveis.limites_inferiores_variaveis.Count, restricoes_laterais_variaveis.limites_inferiores_variaveis[0], restricoes_laterais_variaveis.limites_superiores_variaveis.Count, restricoes_laterais_variaveis.limites_superiores_variaveis[0]);

            // Inicia a população zerada
            populacao_atual = new List<double>();
            populacao_melhor = new List<double>();

            for(int i=0; i<this.n_variaveis_projeto; i++){
                double limite_inferior_variavel = restricoes_laterais_variaveis[i].limite_inferior_variavel;
                double limite_superior_variavel = restricoes_laterais_variaveis[i].limite_superior_variavel;
                double rand = random.NextDouble();

                double xi = limite_inferior_variavel + ((limite_superior_variavel - limite_inferior_variavel) * rand);

#if DEBUG_CONSOLE
                Console.WriteLine("xi gerado = {0}", xi);
#endif

                populacao_atual.Add(xi);
            }


            Console.WriteLine("População gerada:");
            foreach(double ind in populacao_atual){
                Console.WriteLine("individuo = {0}", ind);
            }

            fx_atual = funcao_objetivo_aplicando_penalidade(this.populacao_atual, this.definicao_funcao_objetivo);
            
            add_NFOB();
            
            fx_melhor = fx_atual;
            
#if DEBUG_CONSOLE
            Console.WriteLine("fx_atual atualizado para {0}", fx_atual);
            Console.WriteLine("fx_melhor atualizado para atual = {0}", fx_melhor);
#endif
        }
       
       
        public virtual void verifica_perturbacoes_ordena_e_perturba()
        {
            // Inicia a lista de perturbações zerada
            List<Perturbacao> perturbacoes = new List<Perturbacao>();
            
            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++){
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                double xi = populacao_para_perturbar[i];
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, this.std);
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
                    Console.WriteLine(ind);
                }
#endif

                // Calcula f(x) com a variável perturbada
                double fx = funcao_objetivo_aplicando_penalidade(populacao_para_perturbar, this.definicao_funcao_objetivo);
                add_NFOB();

                // // FRAUDE -> Atualiza o melhor resultado
                // if (fx < fx_melhor){
                //     fx_melhor = fx;
                //     populacao_melhor = populacao_para_perturbar;
                // }

                // Cria o objeto perturbação
                Perturbacao perturbacao = new Perturbacao();
                perturbacao.xi_antes_da_perturbacao = xi;
                perturbacao.xi_depois_da_perturbacao = xii;
                perturbacao.indice_variavel_projeto = i;

                // Adiciona na lista de perturbações
                perturbacoes.Add(perturbacao);
            }

#if DEBUG_CONSOLE
            Console.WriteLine("Agora que criou as perturbações, mostra as perturbações:");
            foreach(Perturbacao p in perturbacoes){
                Console.WriteLine("Perturbação na var {0} com xi antes = {1} e xii depois = {2}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao);
            }
#endif

            // Ordena a população perturbada
            perturbacoes.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.xi_depois_da_perturbacao.CompareTo(b2.xi_depois_da_perturbacao); });

#if DEBUG_CONSOLE
            Console.WriteLine("Perturbações depois de ordenar:");
            foreach(Perturbacao p in perturbacoes){
                Console.WriteLine("Perturbação na var {0} com xi antes = {1} e xi depois = {2}", p.indice_variavel_projeto, p.xi_antes_da_perturbacao, p.xi_depois_da_perturbacao);
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

                    // Perturba uma variável com probabilidade Pk
                    populacao_atual[ perturbacoes[k].indice_variavel_projeto ] = perturbacoes[k].xi_depois_da_perturbacao;

#if DEBUG_CONSOLE
                    Console.WriteLine("Perturbou populacao no indice {0} para o valor {1}", perturbacoes[k].indice_variavel_projeto, perturbacoes[k].xi_depois_da_perturbacao);
#endif

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
            
            // Calcula o fx atual fa população
            fx_atual = funcao_objetivo_aplicando_penalidade(this.populacao_atual, this.definicao_funcao_objetivo);
            
#if DEBUG_CONSOLE
            Console.WriteLine("fx atual da população atual virou {0}", fx_atual);
#endif

            add_NFOB();
        }

        
        public virtual void avaliacao(){
            if (fx_atual < fx_melhor){

#if DEBUG_CONSOLE
                Console.WriteLine("AVALIAÇÃO: fx atual = {0} < fx_melhor = {1}", fx_atual, fx_melhor);
#endif

                fx_melhor = fx_atual;
                populacao_melhor = populacao_atual;
            }
        }
        

        public virtual RetornoGEOs rodar_por_NFOB(int NFOB_criterio_parada){
            geracao_populacao();

            while(true)
            {
                Console.WriteLine("===================================================");

                verifica_perturbacoes_ordena_e_perturba();
                avaliacao();
                if ( NFOB >= NFOB_criterio_parada)
                {
                    Console.WriteLine("Critério de parada atingido por NFOB = {0} >= criterio = {1}", NFOB, NFOB_criterio_parada);

                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFOB = this.NFOB;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFOBs = this.melhores_NFOBs;
                    
                    return retorno;
                }
            }

        }  


        public virtual RetornoGEOs rodar_por_PRECISAO(double PRECISAO_criterio_parada, double fx_esperado){
            geracao_populacao();
            while(true)
            {
                verifica_perturbacoes_ordena_e_perturba();
                avaliacao();
                if ( Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(fx_esperado)) <= PRECISAO_criterio_parada )
                {
                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFOB = this.NFOB;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFOBs = new List<double>();
                    
                    return retorno;
                }
            }
        }  
    }


    // public class GEOvar_REAL : GEOsBASE_REAL
    // {
    //     public List<double> stds {get; set;}
        
    //     public GEOvar_REAL(double tau, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais, int step_obter_NFOBs, double std, List<Double> stds) : base(tau, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais, step_obter_NFOBs, std){
    //         this.stds = stds;
    //     }


    //     public override void verifica_perturbacoes_ordena_e_perturba()
    //     {
    //         // Perturba cada variável
    //         for(int i=0; i<this.n_variaveis_projeto; i++){
            
    //             // Inicia a lista de perturbações zerada
    //             List<Perturbacao> perturbacoes = new List<Perturbacao>();
            
    //             foreach(double std in this.stds){
                
    //                 // Cria uma população cópia
    //                 List<double> populacao_para_perturbar = new List<double>(populacao_atual);

    //                 double xi = populacao_para_perturbar[i];
    //                 MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std);
    //                 double xii = xi + normalDist.Sample() * xi;

    //                 // Atribui a variável perturbada
    //                 populacao_para_perturbar[i] = xii;

    //                 // Calcula f(x) com a variável perturbada
    //                 double fx = funcao_objetivo_aplicando_penalidade(populacao_para_perturbar, this.definicao_funcao_objetivo);
    //                 add_NFOB();

    //                 // Cria o objeto perturbação
    //                 Perturbacao perturbacao = new Perturbacao();
    //                 perturbacao.fx_antes_da_perturbacao = fx_atual;
    //                 perturbacao.fx_depois_da_perturbacao = fx;
    //                 perturbacao.indice_variavel_projeto = i;

    //                 // Adiciona na lista de perturbações
    //                 perturbacoes.Add(perturbacao);
    //             }



    //             // Ordena a população perturbada
    //             perturbacoes.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); });


    //             // Verifica as probabilidades até que uma variável seja perturbada
    //             while (true){

    //                 // Gera um número aleatório com distribuição uniforme
    //                 double ALE = random.NextDouble();

    //                 // k é o índice da população de bits ordenada
    //                 int k = random.Next(1, perturbacoes.Count+1);
                    
    //                 // Probabilidade Pk => k^(-tau)
    //                 double Pk = Math.Pow(k, -tau);

    //                 // k precisa ser de 1 a N, mas aqui nos índices começa em 0
    //                 k -= 1;

    //                 // Se o Pk é maior ou igual ao aleatório, então flipa o bit
    //                 if (Pk >= ALE){

    //                     // Perturba uma variável com probabilidade Pk
    //                     populacao_atual[i] = perturbacoes[k].fx_depois_da_perturbacao;

    //                     // Sai do laço
    //                     break;
    //                 }
    //             }
    //         }

    //         // Atualiza o fx atual da população
    //         fx_atual = funcao_objetivo_aplicando_penalidade(this.populacao_atual, this.definicao_funcao_objetivo);
            
    //         // if (! verifica_restricoes_laterais(fx_atual) ){
    //         //     // penaliza
    //         //     fx_atual = Double.MaxValue;
    //         // }
    //         add_NFOB();
    //     }
    // }





    class Program
    {
        static void Main(string[] args)
        {
            // double tau = 1.25;
            // int n_variaveis_projeto = 3;
            // int definicao_funcao_objetivo = 0;
            // List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            // restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0});
            // restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0});
            // restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0});
            // int step_obter_NFOBs = 200;
            // double std = 1;
            
            // GEOsBASE_REAL geo_real = new GEOsBASE_REAL(
            //     tau,
            //     n_variaveis_projeto,
            //     definicao_funcao_objetivo,
            //     restricoes_laterais_variaveis,
            //     step_obter_NFOBs,
            //     std);

            // int NFOB_criterio_parada = 5000;
            // RetornoGEOs retorno = geo_real.rodar_por_NFOB(NFOB_criterio_parada);
            
            // foreach(double fx in retorno.melhores_NFOBs){
            //     string str_fx = (fx.ToString()).Replace('.',',');
            //     Console.WriteLine(str_fx);
            // }



            double tau = 6.25;
            int n_variaveis_projeto = 10;
            int definicao_funcao_objetivo = 5;
            List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            restricoes_laterais_variaveis.Add(new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0});
            int step_obter_NFOBs = 1000;
            double std = 1;
            
            GEOsBASE_REAL geo_real = new GEOsBASE_REAL(
                tau,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                restricoes_laterais_variaveis,
                step_obter_NFOBs,
                std);

            int NFOB_criterio_parada = 10000;
            RetornoGEOs retorno = geo_real.rodar_por_NFOB(NFOB_criterio_parada);

            foreach(double fx in retorno.melhores_NFOBs){
                string str_fx = (fx.ToString()).Replace('.',',');
                Console.WriteLine(str_fx);
            }




            // double xi = 600;
            // for(int i=0; i<50; i++){
            //     MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, 1);

            //     double sample = normalDist.Sample();
            //     Console.WriteLine(sample);
                
            //     // double xii = xi + sample * xi;
            //     // Console.WriteLine("Era {0} e perturbou para {1}", xi, xii);
            // }
        }
    }
}
