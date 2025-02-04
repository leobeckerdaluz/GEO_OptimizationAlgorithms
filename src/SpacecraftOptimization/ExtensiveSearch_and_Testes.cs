
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


namespace ExtensiveSearch_and_Testes
{
    public class ExtensiveSearch_and_Testes {
       
        public static void ExtensiveSearch_SpacecraftOptimization()
        {
            double menor_fx_historia = Double.MaxValue;
            double menor_i_historia = Double.MaxValue;
            double menor_n_historia = Double.MaxValue;
            double menor_d_historia = Double.MaxValue;

            for (int i = 13; i <= 15; i++)
            {
                for (int d = 1; d <= 60; d++)
                {
                    for (int n = 1; n <= d; n++)
                    {
                        
                        // Se o espaço for viável, executa
                        if( (n < d) && ((double)n%d != 0) ) { //&& (Satellite.Payload.FOV >= 1.05*FovMin);
                            
                            // Monta a lista de fenótipos
                            List<double> fenotipo_variaveis_projeto = new List<double>(){i,d,n};
                            
                            // Instancia a spacecraft
                            SpacecraftFunction spacecraft_model = new SpacecraftFunction(fenotipo_variaveis_projeto);
                            double fx = spacecraft_model.fx_calculada;

                            // Executa diretamente a função objetivo
                            // double fx = SpaceDesignTeste.SpacecraftFunction.ObjectiveFunction(fenotipo_variaveis_projeto);
                            // Console.WriteLine("Espaço válido! i="+i+"; n="+n+"; d:"+d+"; fx="+fx);

                            // Verifica se essa execução é a melhor da história
                            if (fx < menor_fx_historia)
                            {
                                Console.WriteLine("Atualiza o melhor fx para {0} e restrição nesse é {1}", fx, spacecraft_model.valid_solution);
                                menor_fx_historia = fx;
                                menor_i_historia = i;
                                menor_n_historia = n;
                                menor_d_historia = d;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Menor fx história: " + menor_fx_historia);
            Console.WriteLine("Menor i história: " + menor_i_historia);
            Console.WriteLine("Menor n história: " + menor_n_historia);
            Console.WriteLine("Menor d história: " + menor_d_historia);
        }

        public static void Teste_FuncoesObjetivo_SpacecraftOptimization()
        {    
            // Define qual função chamar e o fenótipo
            int function_id = (int)EnumNomesFuncoesObjetivo.spacecraft;
            List<double> fenotipo_variaveis_projeto = new List<double>(){14,60,59};
            // fenotipo_variaveis_projeto = new List<double>(){79,378,348};
            
            // =========================================================
            // Calcula a função objetivo com a rotina de FOs
            // =========================================================
            
            double melhor_fx = ObjectiveFunctions.Methods.funcao_objetivo(fenotipo_variaveis_projeto, function_id);

            Console.WriteLine("Melhor fx função switch case: {0}", melhor_fx);


            // =========================================================
            // Calcula a função objetivo diretamente
            // =========================================================

            SpaceDesignTeste.SpacecraftFunction spacecraft_model = new SpaceDesignTeste.SpacecraftFunction(fenotipo_variaveis_projeto);

            double fx = spacecraft_model.fx_calculada;

            Console.WriteLine("Fx Final Função diretamente: {0}", fx);
        }

    }
}