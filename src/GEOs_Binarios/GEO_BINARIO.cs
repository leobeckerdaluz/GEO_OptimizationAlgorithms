// #define DEBUG_FUNCTION
// #define DEBUG_CONSOLE
// #define DEBUG_FUNCTION_OVERLIMIT
// #define NOVO_MELHOR_FX

using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;

namespace GEOs_BINARIOS
{
    public class GEO_BINARIO
    {
        public Random random = new Random();

        public double tau {get; set;}
        public int n_variaveis_projeto {get; set;}
        public int definicao_funcao_objetivo {get; set;}
        public List<double> lower_bounds {get; set;}
        public List<double> upper_bounds {get; set;}
        public List<int> lista_NFOBs_desejados {get; set;}
        public List<int> bits_por_variavel_variaveis {get; set;}

        public int NFOB {get; set;}
        public double fx_atual {get; set;}
        public double fx_melhor {get; set;}
        public List<bool> populacao_atual {get; set;}
        public List<bool> populacao_melhor {get; set;}
        public List<double> melhores_NFOBs {get; set;}
        public List<BitVerificado> lista_informacoes_mutacao {get; set;}


        public GEO_BINARIO(
            double tau,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFOBs_desejados,
            List<int> bits_por_variavel_variaveis)
        {
            this.tau = tau;
            this.n_variaveis_projeto = n_variaveis_projeto;
            this.definicao_funcao_objetivo = definicao_funcao_objetivo;
            this.lower_bounds = lower_bounds;
            this.upper_bounds = upper_bounds;
            this.lista_NFOBs_desejados = lista_NFOBs_desejados;
            this.bits_por_variavel_variaveis = bits_por_variavel_variaveis;
            this.NFOB = 0;
            this.fx_atual = Double.MaxValue;
            this.fx_melhor = Double.MaxValue;
            this.melhores_NFOBs = new List<double>();
            this.populacao_atual = new List<bool>();
            this.populacao_melhor = new List<bool>();
            this.lista_informacoes_mutacao = new List<BitVerificado>();
        }


        public virtual void add_NFOB()
        {
            NFOB++;

            #if DEBUG_CONSOLE
                Console.WriteLine("NFOB incrementado = {0}", NFOB);
            #endif

            // if (NFOB % 2000 == 0)
            // {
            //     Console.WriteLine("NFOB = {0} e melhor fx até agora = {1}", NFOB, fx_melhor);
            // }
            
            if (lista_NFOBs_desejados.Contains(NFOB))
            {
                melhores_NFOBs.Add(fx_melhor);
                #if DEBUG_CONSOLE    
                    Console.WriteLine("melhor NFOB {0} = {1}", NFOB, fx_melhor);
                #endif
            }
        }


        public virtual double calcula_valor_funcao_objetivo(List<bool> populacao_de_bits)
        {
            //============================================================
            // Calcula o fenótipo para cada variável de projeto
            //============================================================

            int n_variaveis_projeto = this.bits_por_variavel_variaveis.Count;

            // Cria a lista que irá conter o fenótipo de cada variável de projeto
            List<double> fenotipo_variaveis_projeto = new List<double>();
            
            // Transforma o genótipo de cada variável em uma string para depois converter para decimal
            int iterator = 0;
            for (int i=0; i<n_variaveis_projeto; i++)
            {
                double lower = this.lower_bounds[i];
                double upper = this.upper_bounds[i];
                double bits_variavel_projeto = this.bits_por_variavel_variaveis[i];
                // Cria string representando os bits da variável
                string fenotipo_xi = "";

                // Percorre o número de bits de cada variável de projeto
                for(int c=0; c<bits_variavel_projeto; c++)
                {
                    // Se o bit for true, concatena "1", senão, "0"
                    fenotipo_xi += (populacao_de_bits[iterator] ? "1" : "0");
                    
                    iterator++;
                }

                // Converte essa string de bits para inteiro
                int variavel_convertida = Convert.ToInt32(fenotipo_xi, 2);

                // Mapeia o inteiro entre o intervalo mínimo e máximo da função
                // 0 --------- min
                // 2^bits ---- max
                // binario --- x
                // (max-min) / (2^bits - 0) ======> Variação de valor por bit
                // min + [(max-min) / (2^bits - 0)] * binario
                
                double fenotipo_variavel_projeto = lower + ((upper - lower) * variavel_convertida / (Math.Pow(2, bits_variavel_projeto) - 1));

                #if DEBUG_FUNCTION
                    Console.WriteLine("Fenótipo " + i + ": " + fenotipo_xi);
                    Console.WriteLine("variavel_convertida : " + variavel_convertida);
                    Console.WriteLine("fenotipo_variavel_projeto : " + fenotipo_variavel_projeto);
                #endif

                // Adiciona o fenótipo da variável na lista de fenótipos
                fenotipo_variaveis_projeto.Add(fenotipo_variavel_projeto);
            }

            //============================================================
            // Calcula o valor da função objetivo
            //============================================================

            double fx = Funcoes_Definidas.Funcoes.funcao_objetivo(fenotipo_variaveis_projeto, definicao_funcao_objetivo);
            add_NFOB();

            return fx;
        }
        

        public virtual void geracao_populacao()
        {
            // Inicia a população zerada
            populacao_atual = new List<bool>();
            populacao_melhor = new List<bool>();
            
            // Soma os bits por variável de projeto para saber o tamanho da população
            int tamanho_populacao_bits = 0;
            foreach(int bits_variavel in this.bits_por_variavel_variaveis)
            {
                tamanho_populacao_bits += bits_variavel;
            }
            
            // Gera um bit para cada posição da população de bits
            for (int i=0; i<tamanho_populacao_bits; ++i)
            {
                populacao_atual.Add( (random.Next(0, 2)==1) ? true : false );
            }

            fx_atual = calcula_valor_funcao_objetivo(this.populacao_atual);
            
            // Atualiza os melhores
            fx_melhor = fx_atual;
            populacao_melhor = populacao_atual;
        }
       
       
        public virtual void verifica_perturbacoes()
        {
            this.lista_informacoes_mutacao = new List<BitVerificado>();

            // Cria uma cópia da população de bits, flipa o bit e verifica a fitness
            for (int i=0; i<this.populacao_atual.Count; i++)
            {    
                // Cria uma cópia da população de bits
                List<bool> populacao_de_bits_flipado = new List<bool>(this.populacao_atual);
                
                // Flipa o i-ésimo bit
                populacao_de_bits_flipado[i] = !populacao_de_bits_flipado[i];

                // Calcula o valor da função objetivo
                double fx = calcula_valor_funcao_objetivo(populacao_de_bits_flipado);
                
                // Avalia se a perturbação é a melhor de todas
                if (fx < fx_melhor)
                {
                    #if NOVO_MELHOR_FX
                        Console.WriteLine("AVALIAÇÃO: fx = {0} < fx_melhor = {1}", fx, fx_melhor);
                    #endif
                    fx_melhor = fx;
                    populacao_melhor = populacao_de_bits_flipado;
                }
                
                // Armazena as informações dessa mutação do bit na lista de informações
                BitVerificado informacoes_bit = new BitVerificado();
                informacoes_bit.funcao_objetivo_flipando = fx;
                informacoes_bit.indice_bit_mutado = i;
                this.lista_informacoes_mutacao.Add(informacoes_bit);
            }
        }


        public virtual void mutacao_do_tau_AGEOs()
        {}


        public virtual void ordena_e_perturba()
        {
            int tamanho_populacao_bits = this.populacao_atual.Count;

            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

            #if DEBUG_CONSOLE
                Console.WriteLine("Antes da ordenação");
                for (int i=0; i<this.lista_informacoes_mutacao.Count; i++)
                {
                    Console.WriteLine(i + ": Bit: " + this.lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + this.lista_informacoes_mutacao[i].funcao_objetivo_flipando);
                }
            #endif

            // Ordena os bits
            this.lista_informacoes_mutacao.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.funcao_objetivo_flipando.CompareTo(b2.funcao_objetivo_flipando); });
           
            #if DEBUG_CONSOLE
                Console.WriteLine("Depois da ordenação");
                for (int i=0; i<this.lista_informacoes_mutacao.Count; i++)
                {
                    Console.WriteLine(i + ": Bit: " + this.lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + this.lista_informacoes_mutacao[i].funcao_objetivo_flipando);
                }
            #endif
            
            //============================================================
            // Flipa um bit com probabilidade Pk
            //============================================================

            // Verifica as probabilidades até que um bit seja mutado
            while (true)
            {
                // Gera um número aleatório com distribuição uniforme
                double ALE = random.NextDouble();

                // k é o índice da população de bits ordenada
                int k = random.Next(1, tamanho_populacao_bits+1);
                
                // Probabilidade Pk => k^(-tau)
                double Pk = Math.Pow(k, -this.tau);

                // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                k -= 1;

                #if DEBUG_CONSOLE
                    Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+this.lista_informacoes_mutacao[k].indice_bit_mutado+" com Pk "+Pk+" >= ALE "+ALE);
                #endif

                // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                if (Pk >= ALE)
                {

                    #if DEBUG_CONSOLE
                        Console.WriteLine("Antes de flipar:");
                        // ApresentaCromossomoBool(this.populacao_atual);
                        Console.WriteLine("Flipando o indice " + k + ", que é o bit " + this.lista_informacoes_mutacao[k].indice_bit_mutado);
                    #endif

                    // Flipa o bit
                    this.populacao_atual[ this.lista_informacoes_mutacao[k].indice_bit_mutado ] = !this.populacao_atual[ this.lista_informacoes_mutacao[k].indice_bit_mutado ];

                    // Atualiza o valor da f(x) para o flipado
                    fx_atual = lista_informacoes_mutacao[k].funcao_objetivo_flipando;

                    // Sai do laço
                    break;
                }
            }
        }
        

        public virtual bool criterio_parada(ParametrosCriterioParada parametros_criterio_parada)
        {
            // Verifica o critério de parada
            bool parada_por_precisao = ( Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado)) <= parametros_criterio_parada.PRECISAO_criterio_parada );

            // Console.WriteLine("fx_melhor={0} - esperado={1} = {2}", Math.Abs(this.fx_melhor), Math.Abs(parametros_criterio_parada.fx_esperado), Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado));
            
            // Console.WriteLine("abs disso é {0} e a precisao é {1}", Math.Abs(Math.Abs(this.fx_melhor) - Math.Abs(parametros_criterio_parada.fx_esperado)), parametros_criterio_parada.PRECISAO_criterio_parada);

            bool parada_por_NFOB = (NFOB >= parametros_criterio_parada.NFOB_criterio_parada);
            
            // Antes de verificar a parada, começa com falso.
            bool parada = false;

            // Se o critério for por NFOB...
            if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_NFOB)
            {
                if (parada_por_NFOB)
                    parada = true;
            }
            
            // Se o critério for por precisão...
            else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAO)
            {
                if (parada_por_precisao)
                    parada = true;
            }
            
            // Se o critério for por precisão ou por NFOB...
            else if (parametros_criterio_parada.tipo_criterio_parada == (int)EnumTipoCriterioParada.parada_por_PRECISAOouNFOB)
            {
                if (parada_por_NFOB || parada_por_precisao)
                    parada = true;
            }

            // Retorna o status da parada
            return parada;
        }


        public virtual RetornoGEOs executar(ParametrosCriterioParada parametros_criterio_parada){
            geracao_populacao();
            
            while(true)
            {
                verifica_perturbacoes();
                mutacao_do_tau_AGEOs();
                ordena_e_perturba();

                if ( criterio_parada(parametros_criterio_parada) )
                {
                    #if DEBUG_CONSOLE
                        Console.WriteLine("==================================");
                        Console.WriteLine("==================================");
                        Console.WriteLine("CRITÉRIO DE PARADA ATINGIDO!");
                        Console.WriteLine("NFOB = {0} E MELHORFX = {1}", NFOB, fx_melhor);
                        Console.WriteLine("==================================");
                        Console.WriteLine("==================================");
                    #endif

                    RetornoGEOs retorno = new RetornoGEOs();
                    retorno.NFOB = this.NFOB;
                    retorno.melhor_fx = this.fx_melhor;
                    retorno.melhores_NFOBs = this.melhores_NFOBs;
                    // retorno.populacao_final = this.populacao_melhor;
                    
                    return retorno;
                }
            }
        } 
    }
}