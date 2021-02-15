using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_BINARIOS
{
    public class AGEOs_BINARIO: GEO_BINARIO
    {
        public double CoI_1 {get;set;}
        public int tipo_AGEO {get;set;}
        
        public AGEOs_BINARIO(int tipo_AGEO, double tau_minimo, int n_variaveis_projeto, int definicao_funcao_objetivo, List<RestricoesLaterais> restricoes_laterais_variaveis, int step_obter_NFOBs, List<int> bits_por_variavel_variaveis): base(tau_minimo, n_variaveis_projeto, definicao_funcao_objetivo, restricoes_laterais_variaveis, step_obter_NFOBs, bits_por_variavel_variaveis){
            this.CoI_1 = 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tau = tau_minimo;
            this.tipo_AGEO = tipo_AGEO;
        }


        public override void mutacao_do_tau_AGEOs(){
            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;

            if (this.tipo_AGEO == 1){
                // Verifica quantos melhora em comparação com o MELHOR FX
                melhoraram = this.lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= this.fx_melhor).ToList().Count;
            }
            else if (this.tipo_AGEO == 2){
                // Verifica quantos melhora em comparação com o ATUAL FX
                melhoraram = this.lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= this.fx_atual).ToList().Count;                 
            }

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / this.populacao_atual.Count;

            // Se a CoI for zero, restarta o TAU
            if (CoI <= 0.0 || tau > 5){
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                tau = 0.5 * Math.Exp(this.random.NextDouble() * (1.0 / Math.Pow( (this.populacao_atual.Count), 1.0/2.0 )));

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= this.CoI_1){
                tau += (0.5 + CoI) * this.random.NextDouble();
            }
            
#if DEBUG_CONSOLE
            Console.WriteLine("Dos {0}, apenas {1} são melhores!", populacao_atual.Count, melhoraram);
            Console.WriteLine("Valor TAU: {0}", tau);
#endif

            // Atualiza o CoI(i-1) como sendo o atual CoI(i)
            this.CoI_1 = CoI;
        }
    }
}