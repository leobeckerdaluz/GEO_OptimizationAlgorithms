
#define CONSOLE_OUT_FILE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Funcoes_Definidas;
using SpaceDesignTeste;

using GEOs_REAIS;
using GEOs_BINARIOS;

using Classes_Comuns_Enums;

using SpaceConceptOptimizer.Models;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Settings;
using SpaceConceptOptimizer.Utilities;


using System.IO;
using System.Threading.Tasks;


namespace FunctionParameters
{
    public class FunctionParameters {
        public static ParametrosProblema get_function_parameters(int definicao_funcao_objetivo)
        {
            ParametrosProblema parametros_problema;
            switch(definicao_funcao_objetivo)
            {
                // GRIEWANGK
                case (int)EnumNomesFuncoesObjetivo.griewangk:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Griewangk";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 2.75,
                        
                        GEOreal1_O__std = 1,
                        GEOreal1_O__tau = 1.5,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 2,
                        GEOreal1_N__tau = 1.5,

                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 2.5,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 1,
                        GEOreal2_P_VO__tau = 3,

                        GEOreal2_N_VO__P = 4,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 3,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 3,

                        GEOreal2_N_DS__P = 4,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 5,
                        GEOreal2_N_DS__tau = 3,



                        // std_AGEO1real1 = 0.8,
                        // std_AGEO2real1 = 1.2,
                        // p1_AGEO1real1 = 0.2,
                        // p1_AGEO2real1 = 0.2,
                        
                        // std_AGEO1real2 = 1,
                        // P_AGEO1real2 = 4,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 1,
                        // P_AGEO2real2 = 4,
                        // s_AGEO2real2 = 1,
                        
                        // tau_ASGEO2_REAL1_1 = 1.5,
                        // std_ASGEO2_REAL1_1 = 1.2,
                        // tau_ASGEO2_REAL1_2 = 1.5,
                        // std_ASGEO2_REAL1_2 = 1.2,
                        // tau_ASGEO2_REAL1_3 = 1.5,
                        // std_ASGEO2_REAL1_3 = 1.2,
                    };
                break;
                }

                // RASTRINGIN
                case (int)EnumNomesFuncoesObjetivo.rastringin:
                {
                    int n_variaveis = 20;

                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Rastringin";
                    parametros_problema.n_variaveis_projeto = n_variaveis;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1,
                        GEOvar__tau = 1.75,

                        GEOreal1_O__std = 1.8,
                        GEOreal1_O__tau = 2,

                        GEOreal1_P__porc = 3,
                        GEOreal1_P__tau = 5,

                        GEOreal1_N__std = 0.4,
                        GEOreal1_N__tau = 6,
                        
                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 4.5,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 2,
                        GEOreal2_P_VO__porc = 10,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 12,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 1,
                        GEOreal2_N_VO__tau = 5,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 5,

                        GEOreal2_N_DS__P = 12,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 5,



                        // std_AGEO1real1 = 0.8,
                        // std_AGEO2real1 = 1,
                        // p1_AGEO1real1 = 5.2,
                        // p1_AGEO2real1 = 4.8,
                        
                        // std_AGEO1real2 = 1,
                        // P_AGEO1real2 = 4,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 1,
                        // P_AGEO2real2 = 4,
                        // s_AGEO2real2 = 1,
                        
                        // tau_ASGEO2_REAL1_1 = 1.5,
                        // std_ASGEO2_REAL1_1 = 1,
                        // tau_ASGEO2_REAL1_2 = 1.5,
                        // std_ASGEO2_REAL1_2 = 1,
                        // tau_ASGEO2_REAL1_3 = 1.5,
                        // std_ASGEO2_REAL1_3 = 1,
                    };
                break;
                }

                // ROSENBROCK
                case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Rosenbrock";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 2.5,
                        GEOreal1_O__tau = 6,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 5,
                        
                        GEOreal2_O_VO__P = 4,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 5,

                        GEOreal2_P_VO__P = 4,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 0.1,
                        GEOreal2_P_VO__tau = 4,

                        GEOreal2_N_VO__P = 4,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 0.5,
                        GEOreal2_N_VO__tau = 4.5,

                        GEOreal2_O_DS__P = 12,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 4,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 0.1,
                        GEOreal2_P_DS__tau = 4,

                        GEOreal2_N_DS__P = 8,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 1,
                        GEOreal2_N_DS__tau = 4,



                        // std_AGEO1real1 = 1.6,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 0.4,
                        // p1_AGEO2real1 = 0.2,
                        
                        // std_AGEO1real2 = 2,
                        // P_AGEO1real2 = 8,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 2,
                        // P_AGEO2real2 = 4,
                        // s_AGEO2real2 = 2,
                        
                        // tau_ASGEO2_REAL1_1 = 4,
                        // std_ASGEO2_REAL1_1 = 1.8,
                        // tau_ASGEO2_REAL1_2 = 4,
                        // std_ASGEO2_REAL1_2 = 1.8,
                        // tau_ASGEO2_REAL1_3 = 4,
                        // std_ASGEO2_REAL1_3 = 1.8,
                    };
                break;
                }

                // SCHWEFEL
                case (int)EnumNomesFuncoesObjetivo.schwefel:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Schwefel";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 0.75,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 1.8,
                        GEOreal1_O__tau = 6,

                        GEOreal1_P__porc = 8,
                        GEOreal1_P__tau = 2.5,

                        GEOreal1_N__std = 1.4,
                        GEOreal1_N__tau = 1.5,

                        GEOreal2_O_VO__P = 16,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 8,
                        GEOreal2_O_VO__tau = 4,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 2,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 4.5,

                        GEOreal2_N_VO__P = 12,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 4,

                        GEOreal2_O_DS__P = 12,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 5,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 50,
                        GEOreal2_P_DS__tau = 4,

                        GEOreal2_N_DS__P = 4,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 0.5,



                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 2.4,
                        // p1_AGEO1real1 = 5,
                        // p1_AGEO2real1 = 8.4,
                        
                        // std_AGEO1real2 = 2,
                        // P_AGEO1real2 = 16,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 8,
                        // P_AGEO2real2 = 16,
                        // s_AGEO2real2 = 1,
                        
                        // tau_ASGEO2_REAL1_1 = 5,
                        // std_ASGEO2_REAL1_1 = 2.4,
                        // tau_ASGEO2_REAL1_2 = 5,
                        // std_ASGEO2_REAL1_2 = 2.4,
                        // tau_ASGEO2_REAL1_3 = 5,
                        // std_ASGEO2_REAL1_3 = 2.4,
                    };
                break;
                }

                // ACKLEY
                case (int)EnumNomesFuncoesObjetivo.ackley:
                {
                    int n_variaveis_projeto = 30;

                    parametros_problema = new ParametrosProblema()
                    {
                        nome_funcao = "Ackley",
                        n_variaveis_projeto = n_variaveis_projeto,
                        definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.ackley,
                        bits_por_variavel = Enumerable.Repeat(16, n_variaveis_projeto).ToList(),
                        lower_bounds = Enumerable.Repeat(-30.0, n_variaveis_projeto).ToList(),
                        upper_bounds = Enumerable.Repeat(30.0, n_variaveis_projeto).ToList(),
                        fx_esperado = 0.0,
                        parametros_livres = new ParametrosLivreProblema()
                        {
                            GEO__tau = 3.25,
                            GEOvar__tau = 2.25,

                            GEOreal1_O__std = 0.8,
                            GEOreal1_O__tau = 1,

                            GEOreal1_P__porc = 1,
                            GEOreal1_P__tau = 5,

                            GEOreal1_N__std = 0.6,
                            GEOreal1_N__tau = 6,

                            GEOreal2_O_VO__P = 4,
                            GEOreal2_O_VO__s = 1,
                            GEOreal2_O_VO__std = 1,
                            GEOreal2_O_VO__tau = 5,

                            GEOreal2_P_VO__P = 12,
                            GEOreal2_P_VO__s = 2,
                            GEOreal2_P_VO__porc = 10,
                            GEOreal2_P_VO__tau = 4.5,

                            GEOreal2_N_VO__P = 12,
                            GEOreal2_N_VO__s = 2,
                            GEOreal2_N_VO__std = 2,
                            GEOreal2_N_VO__tau = 4.5,

                            GEOreal2_O_DS__P = 4,
                            GEOreal2_O_DS__s = 2,
                            GEOreal2_O_DS__std = 1,
                            GEOreal2_O_DS__tau = 5,

                            GEOreal2_P_DS__P = 12,
                            GEOreal2_P_DS__s = 10,
                            GEOreal2_P_DS__porc = 10,
                            GEOreal2_P_DS__tau = 5,

                            GEOreal2_N_DS__P = 12,
                            GEOreal2_N_DS__s = 10,
                            GEOreal2_N_DS__std = 5,
                            GEOreal2_N_DS__tau = 4.5,



                            // std_AGEO1real1 = 0.8,
                            // std_AGEO2real1 = 0.8,
                            // p1_AGEO1real1 = 3.6,
                            // p1_AGEO2real1 = 1,
                            
                            // std_AGEO1real2 = 1,
                            // P_AGEO1real2 = 4,
                            // s_AGEO1real2 = 1,
                            
                            // std_AGEO2real2 = 1,
                            // P_AGEO2real2 = 4,
                            // s_AGEO2real2 = 1,
                            
                            // tau_ASGEO2_REAL1_1 = 1,
                            // std_ASGEO2_REAL1_1 = 0.8,
                            // tau_ASGEO2_REAL1_2 = 1,
                            // std_ASGEO2_REAL1_2 = 0.8,
                            // tau_ASGEO2_REAL1_3 = 1,
                            // std_ASGEO2_REAL1_3 = 0.8,
                        }
                    };
                break;
                }

                // BEALE
                case (int)EnumNomesFuncoesObjetivo.beale:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Beale";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 0.0;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        
                        GEOreal1_O__std = 2,
                        GEOreal1_O__tau = 5,

                        GEOreal1_P__porc = 1.5,
                        GEOreal1_P__tau = 1.5,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 2,

                        GEOreal2_O_VO__P = 8,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 4,

                        GEOreal2_P_VO__P = 12,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 3.5,

                        GEOreal2_N_VO__P = 12,
                        GEOreal2_N_VO__s = 1,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 3,

                        GEOreal2_O_DS__P = 4,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 1,
                        GEOreal2_O_DS__tau = 5,

                        GEOreal2_P_DS__P = 12,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 3.5,

                        GEOreal2_N_DS__P = 16,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 1,
                        GEOreal2_N_DS__tau = 3,


                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                        
                        // std_AGEO1real2 = 2,
                        // P_AGEO1real2 = 8,
                        // s_AGEO1real2 = 1,
                        
                        // std_AGEO2real2 = 2,
                        // P_AGEO2real2 = 4,
                        // s_AGEO2real2 = 2,
                        
                        // tau_ASGEO2_REAL1_1 = 5,
                        // std_ASGEO2_REAL1_1 = 1.8,
                        // tau_ASGEO2_REAL1_2 = 5,
                        // std_ASGEO2_REAL1_2 = 1.8,
                        // tau_ASGEO2_REAL1_3 = 5,
                        // std_ASGEO2_REAL1_3 = 1.8,
                    };
                break;
                }
                
                
                
                
                // LEVY13
                case (int)EnumNomesFuncoesObjetivo.levy13:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Levy13";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // // tau_GEO = 1.25,
                        // // tau_GEOvar = 1.5,
                        
                        // tau_GEOreal1 = 5,
                        // std_GEOreal1 = 2,
                        
                        // // tau_GEOreal2 = 5,
                        // // std_GEOreal2 = 1,
                        // // P_GEOreal2 = 4,
                        // // s_GEOreal2 = 2,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        
                        // // std_AGEO1real2 = 2,
                        // // P_AGEO1real2 = 8,
                        // // s_AGEO1real2 = 1,
                        
                        // // std_AGEO2real2 = 2,
                        // // P_AGEO2real2 = 4,
                        // // s_AGEO2real2 = 2,
                        
                        // tau_ASGEO2_REAL1_1 = 5,
                        // std_ASGEO2_REAL1_1 = 2,
                        // tau_ASGEO2_REAL1_2 = 5,
                        // std_ASGEO2_REAL1_2 = 2,
                        // tau_ASGEO2_REAL1_3 = 5,
                        // std_ASGEO2_REAL1_3 = 2,
                    };
                break;
                }

                // PAVIANI
                case (int)EnumNomesFuncoesObjetivo.paviani:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Paviani";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(10, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(2.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // SALOMON
                case (int)EnumNomesFuncoesObjetivo.salomon:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Salomon";
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // SCHAFFER 2
                case (int)EnumNomesFuncoesObjetivo.schaffer_2:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Schaffer 2";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BARTELS CONN
                case (int)EnumNomesFuncoesObjetivo.bartels_conn:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bartels Conn";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(17, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BIRD
                case (int)EnumNomesFuncoesObjetivo.bird:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bird";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }

                // BOHACHEVSKY 1
                case (int)EnumNomesFuncoesObjetivo.bohachevsky_1:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "Bohachevsky 1";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.25,
                        // tau_GEOvar = 1.5,
                        
                        // std_AGEO1real1 = 1.8,
                        // std_AGEO2real1 = 1.8,
                        // p1_AGEO1real1 = 1.2,
                        // p1_AGEO2real1 = 1.4,
                    };
                break;
                }





                // F9 TESE
                case (int)EnumNomesFuncoesObjetivo.F09:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "F9";
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 1.50,
                        // tau_GEOvar = 1.75,
                        // tau_GEOreal1 = 1.50,
                        // tau_GEOreal2 = 1.75,
                        // std_GEOreal1 = 1.0,
                        // std_AGEO1real1 = 1.0,
                        // std_AGEO2real1 = 1.0,
                        // std_GEOreal2 = 1.0,
                        // std_AGEO1real2 = 1.0,
                        // std_AGEO2real2 = 1.0,
                        // P_GEOreal2 = 8,
                        // P_AGEO1real2 = 8,
                        // P_AGEO2real2 = 8,
                        // s_GEOreal2 = 2,
                        // s_AGEO1real2 = 2,
                        // s_AGEO2real2 = 2
                    };
                break;
                }

                // DEJONG3
                case (int)EnumNomesFuncoesObjetivo.dejong3:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "DeJong#3";
                    parametros_problema.n_variaveis_projeto = 5;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        
                        // tau_GEO = 3.0,
                        // tau_GEOvar = 8.0,
                        // tau_GEOreal1 = 3.0,
                        // tau_GEOreal2 = 8.0,
                        // std_GEOreal1 = 1.0,
                        // std_AGEO1real1 = 1.0,
                        // std_AGEO2real1 = 1.0,
                        // std_GEOreal2 = 1.0,
                        // std_AGEO1real2 = 1.0,
                        // std_AGEO2real2 = 1.0,
                        // P_GEOreal2 = 8,
                        // P_AGEO1real2 = 8,
                        // P_AGEO2real2 = 8,
                        // s_GEOreal2 = 2,
                        // s_AGEO1real2 = 2,
                        // s_AGEO2real2 = 2
                    };
                break;
                }

                // SPACECRAFT
                case (int)EnumNomesFuncoesObjetivo.spacecraft:
                {
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "SPACECRAFT";
                    parametros_problema.n_variaveis_projeto = 3;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = new List<int>(){2,6,6};
                    parametros_problema.lower_bounds = new List<double>(){13,1,0};
                    parametros_problema.upper_bounds = new List<double>(){15,60,59};
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // tau_GEO = 3.0,
                        // tau_GEOvar = 8.0,

                        // tau_GEOreal1 = 1.0,
                        // tau_GEOreal2 = 1.0,
                        // std_GEOreal1 = 1.0,
                        // std_AGEO1real1 = 1.0,
                        // std_AGEO2real1 = 1.0,
                        // std_GEOreal2 = 1.0,
                        // std_AGEO1real2 = 1.0,
                        // std_AGEO2real2 = 1.0,
                        // P_GEOreal2 = 8,
                        // P_AGEO1real2 = 8,
                        // P_AGEO2real2 = 8,
                        // s_GEOreal2 = 2,
                        // s_AGEO1real2 = 2,
                        // s_AGEO2real2 = 2
                    };
                break;
                }

                // DEFAULT
                default:
                {
                    Console.WriteLine("DEFAULT PARAMETERS!");
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "DEFAULT";
                    parametros_problema.n_variaveis_projeto = 0;
                    parametros_problema.definicao_funcao_objetivo = definicao_funcao_objetivo;
                    parametros_problema.bits_por_variavel = new List<int>();
                    parametros_problema.lower_bounds = new List<double>();
                    parametros_problema.upper_bounds = new List<double>();
                    parametros_problema.fx_esperado = 9999;
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }
            }

            return parametros_problema;
        }
    }
}