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
        public List<int> lista_NFEs_desejados {get; set;}
        public double std {get; set;}
        public int tipo_perturbacao {get; set;}
        public int NFE {get; set;}
        public double fx_atual {get; set;}
        public double fx_melhor {get; set;}
        public double fx_atual_comeco_it {get; set;}
        public List<double> populacao_atual {get; set;}
        public List<double> populacao_melhor {get; set;}
        public List<double> melhores_NFEs {get; set;}
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
            List<int> lista_NFEs_desejados,
            int tipo_perturbacao,
            double tau,
            double std)
        {
            this.tau = tau;
            this.n_variaveis_projeto = n_variaveis_projeto;
            this.definicao_funcao_objetivo = definicao_funcao_objetivo;
            this.lower_bounds = lower_bounds;
            this.upper_bounds = upper_bounds;
            this.lista_NFEs_desejados = lista_NFEs_desejados;
            this.std = std;
            this.tipo_perturbacao = tipo_perturbacao;
            
            this.NFE = 0;
            this.populacao_atual = new List<double>(populacao_inicial);
            this.populacao_melhor = new List<double>(populacao_inicial);
            this.fx_atual = calcula_valor_funcao_objetivo(populacao_inicial);
            this.fx_melhor = this.fx_atual;
            this.fx_atual_comeco_it = this.fx_atual;
            this.melhores_NFEs = new List<double>();
            this.perturbacoes_da_iteracao = new List<Perturbacao>();
            this.iterations = 0;
            this.melhoras_nas_iteracoes = new List<int>();

            this.stats_TAU_per_iteration = new List<double>();
            this.stats_STD_per_iteration = new List<double>();
            this.stats_Mfx_per_iteration = new List<double>();
        }


        public virtual void add_NFE()
        {
            NFE++;

            // Se está no NFE a armazenar info, armazena
            if (lista_NFEs_desejados.Contains(NFE))
            {
                melhores_NFEs.Add(fx_melhor);
            }
        }


        public virtual double calcula_valor_funcao_objetivo(List<double> fenotipos)
        {
            // Calcula o valor da função objetivo com o fenótipo desejado
            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipos, this.definicao_funcao_objetivo);
            
            // Incrementa o NFE
            add_NFE();
            
            // Para cada variável de projeto, computa a penalidade se ela está fora dos limites
            double penalidade = 0;
            const double grau_penalidade = 2;

            for(int i=0; i<fenotipos.Count; i++)
            {
                double lower = lower_bounds[i];
                double upper = upper_bounds[i];
                double xi = fenotipos[i];

                if (xi < lower)
                {
                    double penalidade_inferior = grau_penalidade * Math.Pow((xi-lower), 2);
                    penalidade += penalidade_inferior;
                }
                else if (xi > upper)
                {
                    double penalidade_superior = grau_penalidade * Math.Pow((xi-upper), 2);
                    penalidade += penalidade_superior;
                }
            }

            // Aplica a penalidade em f(x)
            double fx_final = fx + penalidade;
            
            // Sempre depois de calcular o f(x), verifica se é o melhor de todos
            if (fx < fx_melhor)
            {
                fx_melhor = fx_final;
                populacao_melhor = fenotipos;
            }

            return fx_final;
        }
        
        
        public double perturba_variavel(double xi, double std_or_p, int tipo_perturbacao, double intervalo_variacao_variavel)
        {
            // Cria a variável perturbada como cópia da original
            double xii = xi;

            // Caso seja a perturbação igor...
            if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_igor)
            {
                // Distribuição normal com desvio padrão = std
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_or_p);
                xii = xi + (normalDist.Sample() * xi);
            }
            
            // Caso seja a perturbação porcentagem...
            else if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_porcentagem)
            {
                // Distribuição normal onde desvio padrão é porcentagem
                double intervalo_porcentado = intervalo_variacao_variavel * std_or_p / 100.0;
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, intervalo_porcentado);
                xii = xi + normalDist.Sample();
            }
            
            // Caso seja a perturbação normal...
            else if (tipo_perturbacao == (int)EnumTipoPerturbacao.perturbacao_normal)
            {
                // Distribuição normal com desvio padrão = std
                MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, std_or_p);
                xii = xi + normalDist.Sample();
            }


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

                // Perturba a variável
                double xi = populacao_para_perturbar[i];
                double intervalo_variacao_variavel = upper_bounds[i] - lower_bounds[i];
                double xii = perturba_variavel(xi, this.std, this.tipo_perturbacao, intervalo_variacao_variavel);

                // Atribui a variável perturbada na população cópia
                populacao_para_perturbar[i] = xii;

                // Calcula f(x) com a variável perturbada
                double fx = calcula_valor_funcao_objetivo(populacao_para_perturbar);
                

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


        public virtual void mutacao_do_tau_AGEOs(){}


        public virtual void ordena_e_perturba()
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
                int k = random.Next(1, populacao_atual.Count+1);
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -tau);

                // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                k -= 1;

                // Se o Pk é maior ou igual ao aleatório, então confirma a perturbação
                if (Pk >= ALE)
                {
                    // Coloca o novo xi lá na variável escolhida do ranking
                    populacao_atual[perturbacoes_da_iteracao[k].indice_variavel_projeto] = perturbacoes_da_iteracao[k].xi_depois_da_perturbacao;

                    // Atualiza o f(x) atual com o perturbado
                    fx_atual = perturbacoes_da_iteracao[k].fx_depois_da_perturbacao;

                    // Sai do laço while
                    break;
                }
            }
        }


        public virtual bool criterio_parada(ParametrosCriterioParada parametros_criterio_parada)
        {
            // Verifica cada possível critério de parada (por precisão, por NFE ou por nro de iterações)
            bool stop = false;
            
            // POR PRECISÃO - Se a precisão é menor que a definida
            double precisao_obtida = Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado));
            bool parada_por_precisao = (precisao_obtida <= parametros_criterio_parada.PRECISAO_criterio_parada);

            // POR NFE - Se o NFE é superior ao definido
            bool parada_por_NFE = (NFE >= parametros_criterio_parada.NFE_criterio_parada);
            
            // POR NRO ITERAÇÕES - Se o nro de iterações é maior que o definido
            bool parada_por_ITERATIONS = (iterations >= parametros_criterio_parada.ITERATIONS_criterio_parada);
            

            // Se o critério for por NFE...
            if ((parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFE)
            && parada_por_NFE)
            {
                stop = true;
            }

            // Se o critério for por ITERACOES...
            if ((parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_ITERATIONS) 
            && parada_por_ITERATIONS)
            {
                stop = true;
            }

            // Se o critério for por precisão...
            else if ((parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAO) 
            && parada_por_precisao)
            {
                stop = true;
            }
            
            // Se o critério for por precisão ou por NFE...
            else if ((parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFE) 
            && (parada_por_NFE || parada_por_precisao))
            {
                stop = true;
            }

            // Retorna o status da parada
            return stop;
        }
        

        public virtual RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada)
        {
            while(true)
            {
                // Armazena o valor da função no início da iteração
                fx_atual_comeco_it = fx_atual;
                
                verifica_perturbacoes();    // Realiza todas as perturbações nas variáveis
                mutacao_do_tau_AGEOs();     // Muta o tau se necessário
                ordena_e_perturba();        // Escolhe as perturbações a serem confirmadas

                // Armazena os dados da iteração
                iterations++;
                melhoras_nas_iteracoes.Add((fx_atual < fx_atual_comeco_it) ? 1 : 0 );
                stats_TAU_per_iteration.Add(tau);
                stats_STD_per_iteration.Add(std);
                stats_Mfx_per_iteration.Add(fx_melhor);

                // Se o critério de parada for atingido, retorna as informações da execução
                if ( criterio_parada(parametros_criterio_parada) )
                {
                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFE = this.NFE;
                    retorno.iteracoes = this.iterations;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFEs = this.melhores_NFEs;
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