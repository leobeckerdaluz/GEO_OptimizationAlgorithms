using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_BINARIOS
{
    public class AGEOsvar_BINARIO: GEOvar_BINARIO
    {
        public double CoI_1 {get;set;}
        public int tipo_AGEO {get;set;}
        
        public AGEOsvar_BINARIO(
            int tipo_AGEO,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFOBs_desejados,
            List<int> bits_por_variavel_variaveis) : base(
                0.5,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFOBs_desejados,
                bits_por_variavel_variaveis)
        {
            this.CoI_1 = 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tipo_AGEO = tipo_AGEO;
        }


        public override void mutacao_do_tau_AGEOs()
        {
            // Conta quantas mudanças que flipando dá melhor
            int melhoraram = 0;

            if (this.tipo_AGEO == 1)
            {
                // Verifica quantos melhora em comparação com o MELHOR FX
                melhoraram = this.lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= this.fx_melhor).ToList().Count;
            }
            else if (this.tipo_AGEO == 2)
            {
                // Verifica quantos melhora em comparação com o ATUAL FX
                melhoraram = this.lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando <= this.fx_atual).ToList().Count;                 
            }

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / this.populacao_atual.Count;

            // Se a CoI for zero, restarta o TAU
            if (CoI <= 0.0 || tau > 5)
            {
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0/Math.Sqrt(populacao_atual.Count)) );
                // tau = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Pow((populacao_atual.Count), 1.0/2.0)));
                tau = 0.5 * Math.Exp(this.random.NextDouble() * (1.0 / Math.Pow( (this.populacao_atual.Count), 1.0/2.0 )));

            }
            // Senão, se for menor que o CoI anterior, aumenta o TAU
            else if(CoI <= this.CoI_1)
            {
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