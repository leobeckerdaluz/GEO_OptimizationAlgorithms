using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;

namespace GEOs_BINARIOS
{
    public class GEOvar_BINARIO: GEO_BINARIO
    {
        public GEOvar_BINARIO(
            double tau,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            int step_obter_NFOBs,
            List<int> bits_por_variavel_variaveis) : base(
                tau,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                step_obter_NFOBs,
                bits_por_variavel_variaveis)
        {}


        public override void ordena_e_perturba(){
            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

#if DEBUG_CONSOLE
            Console.WriteLine("Antes da ordenação:");
            Console.WriteLine(this.lista_informacoes_mutacao.Count);
            ApresentaCromossomoBool(this.populacao_atual);
            for (int i=0; i<this.lista_informacoes_mutacao.Count; i++){
                Console.WriteLine(i + ": Bit: " + this.lista_informacoes_mutacao[i].indice_bit_mutado + "\t| fx_flipado: " + this.lista_informacoes_mutacao[i].funcao_objetivo_flipando);
            }
#endif

            // Percorre cada variável de projeto para ordenar os bits e escolher o bit para filpar
            List<BitVerificado> depois_ordenar = new List<BitVerificado>();
            int iterador = 0;
            for (int i=0; i<this.n_variaveis_projeto; i++){
                // Obtém o número de bits dessa variável de projeto
                int bits_variavel_projeto = this.bits_por_variavel_variaveis[i];
                
                // Cria uma lista com as informações de mutação de cada bit da variável
                List<BitVerificado> lista_informacoes_bits_variavel = new List<BitVerificado>();
                
                // Percorre o número de bits de cada variável de projeto
                for(int c=0; c<bits_variavel_projeto; c++){
                    // Se o bit for true, concatena "1", senão, "0"
                    lista_informacoes_bits_variavel.Add( this.lista_informacoes_mutacao[iterador] );

                    iterador++;
                }

                // Ordena esses bits da variável
                lista_informacoes_bits_variavel.Sort(delegate(BitVerificado b1, BitVerificado b2) { return b1.funcao_objetivo_flipando.CompareTo(b2.funcao_objetivo_flipando); });

                // Coloca esses bits ordenado na lista geral
                foreach (BitVerificado bit in lista_informacoes_bits_variavel){
                    depois_ordenar.Add(bit);
                }


                //============================================================
                // Flipa um bit da variável com probabilidade Pk
                //============================================================

                // Verifica as probabilidades até que um bit por variável seja mutado
                while (true){

                    // Gera um número aleatório com distribuição uniforme
                    double ALE = this.random.NextDouble();

                    // k é o índice da população de bits ordenada
                    int k = this.random.Next(1, bits_variavel_projeto+1);
                    
                    // Probabilidade Pk => k^(-tau)
                    double Pk = Math.Pow(k, -tau);

                    // k precisa ser de 1 a N, mas aqui nos índices começa em 0
                    k -= 1;

    #if DEBUG_CONSOLE
                    Console.WriteLine("Tentando flipar o indice "+k+", que é o bit "+lista_informacoes_bits_variavel[k].indice_bit_mutado+" com Pk "+Pk+" >= ALE "+ALE);
    #endif

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){

    #if DEBUG_CONSOLE
                        Console.WriteLine("Antes de flipar:");
                        ApresentaCromossomoBool(populacao_de_bits);
                        Console.WriteLine("Flipando o indice " + k + ", que é o bit " + lista_informacoes_bits_variavel[k].indice_bit_mutado);
    #endif

                        // Flipa o bit
                        this.populacao_atual[ lista_informacoes_bits_variavel[k].indice_bit_mutado ] = !this.populacao_atual[ lista_informacoes_bits_variavel[k].indice_bit_mutado ];

                        // Sai do laço
                        break;
                    }
                }
            }

#if DEBUG_CONSOLE
            Console.WriteLine("Depois da ordenação:");
            ApresentaCromossomoBool(this.populacao_atual);
            for (int m=0; m<depois_ordenar.Count; m++){
                Console.WriteLine(m + ": Bit: " + depois_ordenar[m].indice_bit_mutado + "\t| fx_flipado: " + depois_ordenar[m].funcao_objetivo_flipando);
            }
#endif
        }
    }
}