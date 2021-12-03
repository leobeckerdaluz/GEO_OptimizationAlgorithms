using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;

namespace GEOs_BINARIOS
{
    public class GEOvar_BINARIO: GEO_BINARIO
    {
        public GEOvar_BINARIO(
            List<bool> populacao_inicial_binaria,
            double tau,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            List<int> bits_por_variavel_variaveis) : base(
                populacao_inicial_binaria,
                tau,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                bits_por_variavel_variaveis)
        {}


        public override void ordena_e_perturba(){
            //============================================================
            // Ordena os bits conforme os indices fitness
            //============================================================

            // Percorre cada variável de projeto para ordenar os bits e escolher o bit para filpar
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

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){
                        // Flipa o bit
                        this.populacao_atual[ lista_informacoes_bits_variavel[k].indice_bit_mutado ] = !this.populacao_atual[ lista_informacoes_bits_variavel[k].indice_bit_mutado ];

                        // Sai do laço
                        break;
                    }
                }
            }

            // Depois que flipou um bit de cada variável, precisa calcular o fx_atual novamente
            fx_atual = calcula_valor_funcao_objetivo(this.populacao_atual, false);
        }
    }
}