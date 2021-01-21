// Diretivas de compilação para controle de partes do código
// #define DEBUG_CONSOLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace Funcoes_Definidas
{
    public class Funcoes
    {
        public static double funcao_objetivo(List<double> fenotipo_variaveis_projeto, int definicao_funcao_objetivo){
            double fx = 99999;

            // Executa a função objetivo conforme o código dela
            switch(definicao_funcao_objetivo){

                // Griewangk
                case 0:
                    return funcao_griewank(fenotipo_variaveis_projeto);

                // Rosenbrock
                case 1:
                    return funcao_rosenbrock(fenotipo_variaveis_projeto);

                // DeJong3
                case 2:
                    return funcao_DeJong3_inteiro(fenotipo_variaveis_projeto);

                // Custom Spacecraft Orbit Function
                case 3:
                    int D = (int)fenotipo_variaveis_projeto[1];
                    int N = (int)fenotipo_variaveis_projeto[2];
                    
                    if ( (N < D) && ((double)N%D != 0) ){ //&& (Satellite.Payload.FOV >= 1.05*FovMin);
                        // Critério aceito. Espaço viável.
                        return SpaceDesignTeste.TesteOptimizer.ObjectiveFunction(fenotipo_variaveis_projeto);
                    }
                    else{
                        // Espaço inviável. Retorna o máximo
                        return Double.MaxValue;
                    }
                
                // F6
                case 6:
                    return funcao_Rastringin(fenotipo_variaveis_projeto);

                // F7
                case 7:
                    return funcao_Schwefel(fenotipo_variaveis_projeto);

                // F8
                case 8:
                    return funcao_Ackley(fenotipo_variaveis_projeto);

                // F9
                case 9:
                    return 2;

                // F10
                case 10:
                    return 2;

                // F11
                case 11:
                    return 2;

                // F12
                case 12:
                    return 2;

            }

            return fx;
        }


        public static double funcao_DeJong3_inteiro(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;

            // Laço para o somatório
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                // Arredonda para o inteiro mais próximo
                laco_somatorio += Math.Round(fenotipo_variaveis_projeto[i], 0);
            }

            // Retorna o valor de f(x), que é o somatório
            return laco_somatorio;
        }


        public static double funcao_rosenbrock(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;

            // Laço para o somatório
            for(int i=0; i<fenotipo_variaveis_projeto.Count-1; i++){
                // Obtém o valor da variável atual
                double Xi = fenotipo_variaveis_projeto[i];
                // Obtém o valor da próxima variável
                double Xi1 = fenotipo_variaveis_projeto[i+1];
                
                double primeira_parte = 100 * Math.Pow( (Math.Pow(Xi,2) - Xi1), 2 );
                // double primeira_parte = 100 * Math.Pow( (Xi1 - Math.Pow(Xi,2)), 2 );
                // double segunda_parte = Math.Pow( (1 + Xi), 2 );
                double segunda_parte = Math.Pow( (1 - Xi), 2 );
                double colchetes = primeira_parte + segunda_parte;
                laco_somatorio += colchetes;
            }

            // Retorna o valor de f(x), que é o somatório
            return laco_somatorio;
        }

        
        public static double funcao_griewank(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0;
            double laco_produto = 1;

            // Laço para o somatório e produtório
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                laco_somatorio += Math.Pow(fenotipo_variaveis_projeto[i], 2);
                // laco_produto *= Math.Cos( Math.PI * fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
                laco_produto *= Math.Cos( fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
            }

            // Expressão final de f(x)
            double fx = (1 + laco_somatorio/4000.0 - laco_produto);

            // Retorna o valor de f(x)
            return fx;
        }


        public static double funcao_Rastringin(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0.0;

            // Laço para o somatório
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];

                double quadrado = Math.Pow(xi, 2);
                double cosseno = 10.0*Math.Cos(2.0 * Math.PI * xi);
                
                laco_somatorio += quadrado - cosseno;
            }

            // Expressão final de f(x)
            double fx = 10.0*fenotipo_variaveis_projeto.Count + laco_somatorio;

            // Retorna o valor de f(x)
            return fx;
        }


        public static double funcao_Schwefel(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0.0;

            // Laço para o somatório
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];
                laco_somatorio += xi * Math.Sin(Math.Sqrt(Math.Abs(xi)));
            }

            // Expressão final de f(x)
            double fx = 418.9829 * fenotipo_variaveis_projeto.Count - laco_somatorio;

            // Retorna o valor de f(x)
            return fx;
        }


        public static double funcao_Ackley(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio_1 = 0.0;
            double laco_somatorio_2 = 0.0;

            // Laço para os somatórios
            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];

                laco_somatorio_1 += Math.Pow(xi, 2.0);
                laco_somatorio_2 += Math.Cos(2.0 * Math.PI * xi);
            }

            // Obtém o número total de variáveis
            int N = fenotipo_variaveis_projeto.Count;

            // Calculaos Exp antes para depois juntar
            double Exp1 = Math.Exp(-0.2 * Math.Sqrt(1.0/N * laco_somatorio_1 ));
            double Exp2 = Math.Exp(1.0/N * laco_somatorio_2 );

            // Expressão final de f(x)
            double fx = 20.0 + Math.Exp(1.0) - 20.0*Exp1 - Exp2;

            // Retorna o valor de f(x)
            return fx;
        }
    }
}