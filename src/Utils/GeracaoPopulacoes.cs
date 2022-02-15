
#define CONSOLE_OUT_FILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SpaceDesignTeste;

using GEOs_REAIS;
using GEOs_BINARIOS;

using Classes_e_Enums;

using SpaceConceptOptimizer.Models;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Settings;
using SpaceConceptOptimizer.Utilities;


using System.IO;
using System.Threading.Tasks;


namespace GeracaoPopulacoes
{
    public class GeracaoPopulacoes {
       
        public static List<double> geracao_populacao_real(List<double> lower_bounds, List<double> upper_bounds, int seed)
        {
            // Quantidade de variáveis de projeto
            double size = lower_bounds.Count;
            
            List<double> population = new List<double>();
            Random rnd = new Random(seed);

            for(int i=0; i<size; i++)
            {
                double lower = lower_bounds[i];
                double upper = upper_bounds[i];
                
                double rand = rnd.NextDouble();

                double xi = lower + ((upper - lower) * rand);

                population.Add(xi);
            }

            // Retorna a população criada
            return population;
        }


        public static List<bool> geracao_populacao_binaria(List<int> bits_por_variavel_variaveis, int seed)
        {
            List<bool> population = new List<bool>();

            // Soma os bits por variável de projeto para saber o tamanho da população
            int tamanho_populacao_bits = 0;
            foreach(int bits_da_variavel in bits_por_variavel_variaveis)
            {
                tamanho_populacao_bits += bits_da_variavel;
            }
            
            // Gera um bit para cada posição da população de bits
            Random rnd = new Random(seed);
            for (int i=0; i<tamanho_populacao_bits; ++i)
            {
                population.Add( (rnd.Next(0, 2)==1) ? true : false );
            }
            
            return population;
        }

    }
}