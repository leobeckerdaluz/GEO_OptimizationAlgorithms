using System;
using System.Linq;
using System.Collections.Generic;
using Classes_e_Enums;

namespace GEOs_BINARIOS
{
    public class GEOvar_BINARIO: GEO_BINARIO
    {
        public GEOvar_BINARIO(
            List<bool> populacao_inicial_binaria,
            double tau,
            int n_variaveis_projeto,
            int function_id,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            List<int> bits_por_variavel_variaveis,
            bool integer_population) : base(
                populacao_inicial_binaria,
                tau,
                n_variaveis_projeto,
                function_id,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                bits_por_variavel_variaveis,
                integer_population)
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
                    lista_informacoes_bits_variavel.Add( this.lista_informacoes_mutacao[iterador] );
                    iterador++;
                }

                // Ordena esses bits da variável
                lista_informacoes_bits_variavel.Sort(delegate(BitVerificado b1, BitVerificado b2) { 
                    return b1.funcao_objetivo_flipando.CompareTo(b2.funcao_objetivo_flipando); 
                });

                // //---------------------------------------------------------------------------------------
                // // Se nenhuma perturbação for viável, deixa essa população mesmo
                // bool at_least_one_valid = lista_informacoes_bits_variavel.Any(x => x.feasible_solution == true);
                // if (!at_least_one_valid)
                //     return;
                
                // if (! lista_informacoes_bits_variavel.All(x => x.feasible_solution == true)){
                //     return;
                // }
                // //---------------------------------------------------------------------------------------

                // Verifica as probabilidades até que um bit por variável seja mutado
                while (true){

                    // Gera um número aleatório com distribuição uniforme
                    double ALE = this.random.NextDouble();

                    // k é o índice da população de bits ordenada
                    int k = this.random.Next(1, bits_variavel_projeto+1);
                    
                    // Probabilidade Pk => k^(-tau)
                    double Pk = Math.Pow(k, -tau);

                    // Se o Pk é maior ou igual ao aleatório, então flipa o bit
                    if (Pk >= ALE){
                        // k foi de 1 a N, mas no array o índice começa em 0, então subtrai 1
                        BitVerificado perturbacao_escolhida = lista_informacoes_bits_variavel[k-1];

                        // //----------------------------------------------------
                        // // Do not set an unfeasible solution as new population
                        // if (!perturbacao_escolhida.feasible_solution)
                        //     continue;
                        // //----------------------------------------------------

                        // Flipa o bit
                        this.populacao_atual[ perturbacao_escolhida.indice_bit_mutado ] = !this.populacao_atual[ perturbacao_escolhida.indice_bit_mutado ];

                        // Sai do laço
                        break;
                    }
                }
            }

            // Depois que flipou um bit de cada variável, precisa calcular o fx_atual novamente
            populacao_atual_double = convert_boolpop_to_listdouble(populacao_atual, this.integer_population);
            fx_atual = calcula_valor_funcao_objetivo(populacao_atual_double, true);
        }
    }
}