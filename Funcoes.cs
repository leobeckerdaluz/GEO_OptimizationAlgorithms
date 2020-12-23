// // Diretivas de compilação para controle de partes do código
// // #define DEBUG_CONSOLE

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.InteropServices;

// namespace Funcoes_Definidas
// {
//     public class Funcoes
//     {
//         /*
//             A função DeJong#3 recebe como parâmetro a lista contendo o fenótipo de cada
//             variável de projeto e calcula o valor da função.
//         */
//         public static double funcao_DeJong3_inteiro(List<double> fenotipo_variaveis_projeto){
//             double laco_somatorio = 0;

//             // Laço para os somatórios
//             for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
//                 // Arredonda para o inteiro mais próximo
//                 laco_somatorio += Math.Round(fenotipo_variaveis_projeto[i], 0);
//             }

//             // Retorna o valor de f(x), que é o somatório
//             return laco_somatorio;
//         }
        
        
//         /*
//             A função de rosenbrock recebe como parâmetro a lista contendo o fenótipo de cada
//             variável de projeto e calcula o valor da função.
//         */
//         public static double funcao_rosenbrock(List<double> fenotipo_variaveis_projeto){
//             double laco_somatorio = 0;

//             // Laço para os somatórios
//             for(int i=0; i<fenotipo_variaveis_projeto.Count-1; i++){
//                 // Obtém o valor da variável atual
//                 double Xi = fenotipo_variaveis_projeto[i];
//                 // Obtém o valor da próxima variável
//                 double Xi1 = fenotipo_variaveis_projeto[i+1];
                
//                 double primeira_parte = 100 * Math.Pow( (Math.Pow(Xi,2) - Xi1), 2 );
//                 // double primeira_parte = 100 * Math.Pow( (Xi1 - Math.Pow(Xi,2)), 2 );
//                 // double segunda_parte = Math.Pow( (1 + Xi), 2 );
//                 double segunda_parte = Math.Pow( (1 - Xi), 2 );
//                 double colchetes = primeira_parte + segunda_parte;
//                 laco_somatorio += colchetes;
//             }

//             // Retorna o valor de f(x), que é o somatório
//             return laco_somatorio;
//         }
        
        
//         /*
//             A função de griewank recebe como parâmetro a lista contendo o fenótipo de cada
//             variável de projeto e calcula o valor da função.
//         */
//         public static double funcao_griewank(List<double> fenotipo_variaveis_projeto){
//             double laco_somatorio = 0;
//             double laco_produto = 1;

//             // Laço para os somatórios e pi
//             for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
//                 laco_somatorio += Math.Pow(fenotipo_variaveis_projeto[i], 2);
//                 // laco_produto *= Math.Cos( Math.PI * fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
//                 laco_produto *= Math.Cos( fenotipo_variaveis_projeto[i] / Math.Sqrt(i+1) );
//             }

//             // Expressão final de f(x)
//             double fx = (1 + laco_somatorio/4000.0 - laco_produto);

//             // Retorna o valor de f(x)
//             return fx;
//         }



        
        
//         public static double funcao_custom_spacecraft_orbit(List<double> fenotipo_variaveis_projeto){
//             int I = fenotipo_variaveis_projeto[0];
//             int Q = fenotipo_variaveis_projeto[1];
//             int D = fenotipo_variaveis_projeto[2];
            
//             // double rev = I + Q/D;

//             // // Earth equatorial radius
//             // double R0 = 6378137;
//             // // semi-major axis
//             // double a
//             // // eccentricity
//             // double e
//             // // Earth standard gravitational parameter
//             // double mi0 = 3.986004418 * Math.Pow(10,14);
//             // // non-perturbed orbit angular velocity
//             // double n0 = Math.sqrt( mi0 / Math.Pow(a,3));
//             // // Earth second gravitational zonal harmonic
//             // double J2 = 1.08262617385222 * Math.Pow(10,-3);
//             // // orbit inclination
//             // double i

//             // double omega = -3/2 * Math.Pow(R0,2)/( Math.Pow(a,2)*(1-Math.Pow(e,2)) ) * n0 * J2 * Math.cos(i)



//             double md = Wp/0.31;
            
//             // Propellant specific impulse
//             double Isp = 225;
//             // gravity acceleration at Earth’s surface
//             double g0 = 9.80665
//             double expoente_mp = -DELTAv / g0*Isp
//             double mp = Sm * (1 - Math.Pow(e, ))

//             double Sm = md + mp;






//             return value;
//         }









        
//     }
// }