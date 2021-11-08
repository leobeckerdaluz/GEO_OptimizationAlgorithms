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
        public List<double> lower_bounds {get; set;}
        public List<double> upper_bounds {get; set;}
        public List<int> lista_NFOBs_desejados {get; set;}
        public double std {get; set;}
        public int tipo_perturbacao {get; set;}
        public int NFOB {get; set;}
        public double fx_atual {get; set;}
        public double fx_melhor {get; set;}
        public double fx_atual_comeco_it {get; set;}
        public List<double> populacao_atual {get; set;}
        public List<double> populacao_melhor {get; set;}
        public List<double> melhores_NFOBs {get; set;}
        public List<double> melhores_TAUs {get; set;}
        public List<Perturbacao> perturbacoes_da_iteracao {get; set;}
        public int iterations {get; set;}
        public List<int> melhoras_nas_iteracoes {get; set;}
        public List<double> stats_TAU_per_iteration {get; set;}
        public List<double> stats_STD_per_iteration {get; set;}
        public List<double> stats_Mfx_per_iteration {get; set;}


        public GEO_real1(
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> populacao_inicial,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFOBs_desejados,
            int tipo_perturbacao,
            double tau,
            double std)
        {
            this.tau = tau;
            this.n_variaveis_projeto = n_variaveis_projeto;
            this.definicao_funcao_objetivo = definicao_funcao_objetivo;
            this.lower_bounds = lower_bounds;
            this.upper_bounds = upper_bounds;
            this.lista_NFOBs_desejados = lista_NFOBs_desejados;
            this.std = std;
            this.tipo_perturbacao = tipo_perturbacao;
            
            this.NFOB = 0;
            this.populacao_atual = new List<double>(populacao_inicial);
            this.populacao_melhor = new List<double>(populacao_inicial);
            this.fx_atual = calcula_valor_funcao_objetivo(populacao_inicial);
            this.fx_melhor = this.fx_atual;
            this.fx_atual_comeco_it = this.fx_atual;
            this.melhores_NFOBs = new List<double>();
            this.perturbacoes_da_iteracao = new List<Perturbacao>();
            this.iterations = 0;
            this.melhoras_nas_iteracoes = new List<int>();

            this.stats_TAU_per_iteration = new List<double>();
            this.stats_STD_per_iteration = new List<double>();
            this.stats_Mfx_per_iteration = new List<double>();
        }


        public virtual void add_NFOB()
        {
            NFOB++;

            if (lista_NFOBs_desejados.Contains(NFOB))
            {
                
                // Atualiza os stats
                
                melhores_NFOBs.Add(fx_melhor);
                // stats_TAU_per_iteration.Add(tau);
                // stats_STD_per_iteration.Add(std);
                stats_Mfx_per_iteration.Add(fx_melhor);
            }
        }


        public virtual double calcula_valor_funcao_objetivo(List<double> fenotipos)
        {
            // Calcula o valor da função objetivo com o fenótipo desejado
            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipos, this.definicao_funcao_objetivo);
            
            add_NFOB();
            
            double penalidade = 0;

            const double grau_penalidade = 2;

            // Verifica a penalidade para cada variável do fenótipo desejado
            for(int i=0; i<fenotipos.Count; i++)
            {
                double lower = lower_bounds[i];
                double upper = upper_bounds[i];
                double xi = fenotipos[i];

                // Verifica se a variável está fora dos limites
                if (xi < lower)
                {
                    double penalidade_inferior = grau_penalidade * Math.Pow(xi - lower, 2);
                    
                    penalidade += penalidade_inferior;
                }
                else if (xi > upper)
                {
                    double penalidade_superior = grau_penalidade * Math.Pow(xi - upper, 2);
                    
                    penalidade += penalidade_superior;
                }
            }

            double penalidade_aplicada = fx + penalidade;

            return penalidade_aplicada;
        }
        
        
        public double perturba_variavel(double xi, double std_atual, int tipo_perturbacao, double intervalo_variacao_variavel)
        {
            // Varíavel perturbada
            double xii = xi;

            // Perturba a variável dentro dos limites
            if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_igor)
            {
                // Distribuição normal com desvio padrão = std
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_atual);
                xii = xi + (normalDist.Sample() * xi);
            }
            else if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_porcentagem)
            {
                // Distribuição normal onde desvio padrão é porcentagem.
                double intervalo_porcentado = intervalo_variacao_variavel * std_atual / 100.0;
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, intervalo_porcentado);
                xii = xi + normalDist.Sample();
            }
            else if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_normal)
            {
                // Distribuição normal com desvio padrão = std
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_atual);
                xii = xi + normalDist.Sample();
            }




            // MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_atual);
            // MathNet.Numerics.Distributions.ContinuousUniform uniformContDist = new MathNet.Numerics.Distributions.ContinuousUniform();
            // MathNet.Numerics.Distributions.Exponential expDist = new MathNet.Numerics.Distributions.Exponential(std_atual);
            
            // xii = xi + normalDist.Sample();
            // // xii = xi + uniformContDist.Sample();
            
            // // double exp = expDist.Sample();
            // // double unif = uniformContDist.Sample();
            // // xii = xi + ((unif >= 0.5) ? exp : -exp);


            // Retorna a variável perturbada
            return xii;
        }
        
        
        public virtual void verifica_perturbacoes()
        {
            // Inicia a lista de perturbações zerada
            List<Perturbacao> perturbacoes = new List<Perturbacao>();
            
            // Perturba cada variável
            for(int i=0; i<this.n_variaveis_projeto; i++)
            {
                // Cria uma população cópia
                List<double> populacao_para_perturbar = new List<double>(populacao_atual);

                double xi = populacao_para_perturbar[i];

                // Perturba a variável
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                double xii = perturba_variavel(xi, this.std, this.tipo_perturbacao, intervalo_variacao_variavel);

                // Atribui a variável perturbada
                populacao_para_perturbar[i] = xii;

                // Calcula f(x) com a variável perturbada
                double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar);

                // Avalia se a perturbação é a melhor de todas
                if (fx < fx_melhor)
                {
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


        public virtual void ordena_e_perturba()
        {    
            // Ordena as perturbações com base no f(x)
            perturbacoes_da_iteracao.Sort(delegate(Perturbacao b1, Perturbacao b2) { return b1.fx_depois_da_perturbacao.CompareTo(b2.fx_depois_da_perturbacao); });

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

                // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                if (Pk >= ALE)
                {
                    // Lá na variável k, coloca o novo xi
                    populacao_atual[ perturbacoes_da_iteracao[k].indice_variavel_projeto ] = perturbacoes_da_iteracao[k].xi_depois_da_perturbacao;

                    // Atualiza o f(x) atual com o perturbado
                    fx_atual = perturbacoes_da_iteracao[k].fx_depois_da_perturbacao;

                    // Sai do laço
                    break;
                }
            }
        }


        public virtual bool criterio_parada(ParametrosCriterioParada parametros_criterio_parada)
        {
            // Verifica o critério de parada
            bool parada_por_precisao = ( Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado)) <= parametros_criterio_parada.PRECISAO_criterio_parada );

            bool parada_por_NFOB = (NFOB >= parametros_criterio_parada.NFOB_criterio_parada);
            
            bool parada_por_ITERATIONS = (iterations >= parametros_criterio_parada.ITERATIONS_criterio_parada);
            
            // Antes de verificar a parada, começa com falso.
            bool parada = false;

            // Se o critério for por NFOB...
            if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB)
            {
                if (parada_por_NFOB)
                {
                    parada = true;
                }
            }

            // Se o critério for por ITERACOES...
            if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_ITERATIONS)
            {
                if (parada_por_ITERATIONS)
                {
                    parada = true;
                }
            }

            // Se o critério for por precisão...
            else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAO)
            {
                if (parada_por_precisao)
                {
                    parada = true;
                }
            }
            
            // Se o critério for por precisão ou por NFOB...
            else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB)
            {
                if (parada_por_NFOB || parada_por_precisao)
                {
                    parada = true;
                }
            }

            // Retorna o status da parada
            return parada;
        }
        

        public virtual RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada)
        {
            while(true)
            {
                fx_atual_comeco_it = fx_atual;
                
                verifica_perturbacoes();
                mutacao_do_tau_AGEOs();
                ordena_e_perturba();

                iterations++;
                melhoras_nas_iteracoes.Add( (fx_atual < fx_atual_comeco_it) ? 1 : 0 );
                stats_TAU_per_iteration.Add(tau);
                stats_STD_per_iteration.Add(std);
                stats_Mfx_per_iteration.Add(fx_melhor);

                if ( criterio_parada(parametros_criterio_parada) )
                {
                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFOB = this.NFOB;
                    retorno.iteracoes = this.iterations;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFOBs = this.melhores_NFOBs;
                    retorno.populacao_final = this.populacao_melhor;
                    retorno.stats_TAU_per_iteration = this.stats_TAU_per_iteration;
                    retorno.stats_STD_per_iteration = this.stats_STD_per_iteration;
                    retorno.stats_Mfx_per_iteration = this.stats_Mfx_per_iteration;
                    
                    return retorno;
                }
            }
        } 
    }
}