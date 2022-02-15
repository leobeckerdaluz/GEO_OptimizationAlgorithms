using System;
using System.Collections.Generic;
using System.Linq;
using Classes_e_Enums;

namespace ObjectiveFunctions
{
    public class Parameters
    {
        // =========================================================================
        // Set All Functions Parameters
        // =========================================================================
        public static ParametrosProblema get_function_parameters(int function_id)
        {
            // Generate initial problem parameters
            ParametrosProblema parametros_problema = new ParametrosProblema(){
                nome_funcao = Enum.GetName(typeof(EnumNomesFuncoesObjetivo), function_id),
                function_id = function_id
            };


            switch(function_id)
            {
                // --------------------------------------------------------------------------------
                // Benchmark functions
                // --------------------------------------------------------------------------------
                
                // GRIEWANGK
                case (int)EnumNomesFuncoesObjetivo.griewangk:
                {
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(14, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(600.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 1.25,
                        GEOvar__tau = 2.75,
                        GEOvar2__tau = 6.5,
                       
                        // GEOreal1
                        GEOreal1_O__std = 1.2,
                        GEOreal1_O__tau = 1.5,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 5.5,

                        GEOreal1_N__std = 2,
                        GEOreal1_N__tau = 1.5,

                        // GEOreal2
                        GEOreal2_O_VO__P = 5,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 2.5,

                        GEOreal2_P_VO__P = 5,
                        GEOreal2_P_VO__s = 4,
                        GEOreal2_P_VO__porc = 1,
                        GEOreal2_P_VO__tau = 3.5,

                        GEOreal2_N_VO__P = 5,
                        GEOreal2_N_VO__s = 4,
                        GEOreal2_N_VO__std = 10,
                        GEOreal2_N_VO__tau = 3.5,

                        GEOreal2_O_DS__P = 5,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 2,
                        GEOreal2_O_DS__tau = 3,

                        GEOreal2_P_DS__P = 10,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 1,
                        GEOreal2_P_DS__tau = 4,

                        GEOreal2_N_DS__P = 10,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 4,


                        AGEO2real1_P__porc = 0.5,

                        AGEO2real2_P_DS__P = 10,
                        AGEO2real2_P_DS__s = 10,
                        AGEO2real2_P_DS__porc = 1,
                    };
                break;
                }

                // RASTRINGIN
                case (int)EnumNomesFuncoesObjetivo.rastringin:
                {
                    parametros_problema.n_variaveis_projeto = 20;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 1,
                        GEOvar__tau = 1.75,
                        GEOvar2__tau = 6,

                        // GEOreal1
                        GEOreal1_O__std = 1,
                        GEOreal1_O__tau = 1.5,

                        GEOreal1_P__porc = 3,
                        GEOreal1_P__tau = 5.5,

                        GEOreal1_N__std = 0.4,
                        GEOreal1_N__tau = 5,
                        
                        // GEOreal2
                        GEOreal2_O_VO__P = 5,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 5,

                        GEOreal2_P_VO__P = 10,
                        GEOreal2_P_VO__s = 4,
                        GEOreal2_P_VO__porc = 10,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 10,
                        GEOreal2_N_VO__s = 4,
                        GEOreal2_N_VO__std = 0.5,
                        GEOreal2_N_VO__tau = 5,

                        GEOreal2_O_DS__P = 5,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 2,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 10,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 5,

                        GEOreal2_N_DS__P = 10,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 0.5,
                        GEOreal2_N_DS__tau = 5,


                        AGEO2real1_P__porc = 3,

                        AGEO2real2_P_DS__P = 10,
                        AGEO2real2_P_DS__s = 10,
                        AGEO2real2_P_DS__porc = 10,
                    };
                break;
                }

                // ROSENBROCK
                case (int)EnumNomesFuncoesObjetivo.rosenbrock:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(2.048, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        GEOvar2__tau = 6.50,
                        
                        // GEOreal1
                        GEOreal1_O__std = 2.4,
                        GEOreal1_O__tau = 5.5,

                        GEOreal1_P__porc = 0.5,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 5.5,
                        
                        // GEOreal2
                        GEOreal2_O_VO__P = 5,
                        GEOreal2_O_VO__s = 2,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 4,

                        GEOreal2_P_VO__P = 5,
                        GEOreal2_P_VO__s = 1,
                        GEOreal2_P_VO__porc = 0.1,
                        GEOreal2_P_VO__tau = 4.5,

                        GEOreal2_N_VO__P = 5,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 0.5,
                        GEOreal2_N_VO__tau = 4,

                        GEOreal2_O_DS__P = 10,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 2,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 5,
                        GEOreal2_P_DS__s = 2,
                        GEOreal2_P_DS__porc = 0.1,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 10,
                        GEOreal2_N_DS__s = 2,
                        GEOreal2_N_DS__std = 0.5,
                        GEOreal2_N_DS__tau = 5,


                        AGEO2real1_P__porc = 0.5,

                        AGEO2real2_P_DS__P = 5,
                        AGEO2real2_P_DS__s = 2,
                        AGEO2real2_P_DS__porc = 0.1,
                    };
                break;
                }

                // SCHWEFEL
                case (int)EnumNomesFuncoesObjetivo.schwefel:
                {
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 0.75,
                        GEOvar__tau = 1.5,
                        GEOvar2__tau = 7,
                      
                        // GEOreal1
                        GEOreal1_O__std = 1.8,
                        GEOreal1_O__tau = 5.5,

                        GEOreal1_P__porc = 9,
                        GEOreal1_P__tau = 3.5,

                        GEOreal1_N__std = 1,
                        GEOreal1_N__tau = 1.5,

                        // GEOreal2
                        GEOreal2_O_VO__P = 10,
                        GEOreal2_O_VO__s = 4,
                        GEOreal2_O_VO__std = 2,
                        GEOreal2_O_VO__tau = 5,

                        GEOreal2_P_VO__P = 10,
                        GEOreal2_P_VO__s = 4,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 10,
                        GEOreal2_N_VO__s = 2,
                        GEOreal2_N_VO__std = 1,
                        GEOreal2_N_VO__tau = 3.5,

                        GEOreal2_O_DS__P = 10,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 2,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 10,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 50,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 10,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 1,
                        GEOreal2_N_DS__tau = 5,



                        AGEO2real1_P__porc = 9,

                        AGEO2real2_P_DS__P = 10,
                        AGEO2real2_P_DS__s = 10,
                        AGEO2real2_P_DS__porc = 50,
                    };
                break;
                }

                // ACKLEY
                case (int)EnumNomesFuncoesObjetivo.ackley:
                {
                    parametros_problema.n_variaveis_projeto = 30;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(16, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-30.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(30.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 3.25,
                        GEOvar__tau = 2.25,
                        GEOvar2__tau = 6.25,

                        // GEOreal1
                        GEOreal1_O__std = 0.8,
                        GEOreal1_O__tau = 1,

                        GEOreal1_P__porc = 1,
                        GEOreal1_P__tau = 6,

                        GEOreal1_N__std = 0.4,
                        GEOreal1_N__tau = 6,

                        // GEOreal2
                        GEOreal2_O_VO__P = 5,
                        GEOreal2_O_VO__s = 1,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 4.5,

                        GEOreal2_P_VO__P = 10,
                        GEOreal2_P_VO__s = 4,
                        GEOreal2_P_VO__porc = 10,
                        GEOreal2_P_VO__tau = 5,

                        GEOreal2_N_VO__P = 10,
                        GEOreal2_N_VO__s = 4,
                        GEOreal2_N_VO__std = 2,
                        GEOreal2_N_VO__tau = 5,

                        GEOreal2_O_DS__P = 5,
                        GEOreal2_O_DS__s = 2,
                        GEOreal2_O_DS__std = 1,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 10,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 10,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 10,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 2,
                        GEOreal2_N_DS__tau = 5,


                        AGEO2real1_P__porc = 1,

                        AGEO2real2_P_DS__P = 10,
                        AGEO2real2_P_DS__s = 10,
                        AGEO2real2_P_DS__porc = 10,
                    };
                break;
                }

                // BEALE
                case (int)EnumNomesFuncoesObjetivo.beale:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(4.5, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema()
                    {
                        // Binários
                        GEO__tau = 1.25,
                        GEOvar__tau = 1.5,
                        GEOvar2__tau = 6.5,
                        
                        // GEOreal1
                        GEOreal1_O__std = 2,
                        GEOreal1_O__tau = 6,

                        // GEOreal1_P__porc = 2.5,
                        // GEOreal1_P__tau = 4,
                        GEOreal1_P__porc = 1.5,
                        GEOreal1_P__tau = 1.5,

                        GEOreal1_N__std = 0.2,
                        GEOreal1_N__tau = 2.5,

                        // GEOreal2
                        GEOreal2_O_VO__P = 10,
                        GEOreal2_O_VO__s = 4,
                        GEOreal2_O_VO__std = 1,
                        GEOreal2_O_VO__tau = 4.5,

                        GEOreal2_P_VO__P = 10,
                        GEOreal2_P_VO__s = 4,
                        GEOreal2_P_VO__porc = 50,
                        GEOreal2_P_VO__tau = 4,

                        GEOreal2_N_VO__P = 10,
                        GEOreal2_N_VO__s = 4,
                        GEOreal2_N_VO__std = 10,
                        GEOreal2_N_VO__tau = 3.5,

                        GEOreal2_O_DS__P = 5,
                        GEOreal2_O_DS__s = 10,
                        GEOreal2_O_DS__std = 1,
                        GEOreal2_O_DS__tau = 4.5,

                        GEOreal2_P_DS__P = 5,
                        GEOreal2_P_DS__s = 10,
                        GEOreal2_P_DS__porc = 50,
                        GEOreal2_P_DS__tau = 4.5,

                        GEOreal2_N_DS__P = 5,
                        GEOreal2_N_DS__s = 10,
                        GEOreal2_N_DS__std = 10,
                        GEOreal2_N_DS__tau = 4,



                        AGEO2real1_P__porc = 1.5,
                        AGEO2real2_P_DS__P = 5,
                        AGEO2real2_P_DS__s = 10,
                        AGEO2real2_P_DS__porc = 50,
                    };
                break;
                }
                
                
                
                // --------------------------------------------------------------------------------
                // CEC2017 functions
                // --------------------------------------------------------------------------------
                
                case (int)EnumNomesFuncoesObjetivo.CEC2017_01:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_03:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_04:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_05:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_06:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_07:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_08:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_09:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_10:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_11:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_12:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_13:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_14:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_15:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_16:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_17:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_18:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_19:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_20:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_21:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_22:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_23:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_24:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_25:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_26:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_27:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_28:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_29:
                case (int)EnumNomesFuncoesObjetivo.CEC2017_30:
                {
                    const int bits_per_variable = 16;
                    const int n_design_variables = 10;
                    
                    // dimension, bits per variable and bounds are fixed.
                    // The only variable per function is its name.
                    parametros_problema.n_variaveis_projeto = n_design_variables;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(bits_per_variable, n_design_variables).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, n_design_variables).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(+100.0, n_design_variables).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }



                // --------------------------------------------------------------------------------
                // Spacecraft Function
                // --------------------------------------------------------------------------------
                
                case (int)EnumNomesFuncoesObjetivo.spacecraft:
                {
                    parametros_problema.n_variaveis_projeto = 3;
                    parametros_problema.bits_por_variavel = new List<int>(){2,6,6};
                    parametros_problema.lower_bounds = new List<double>(){13,1,1};
                    parametros_problema.upper_bounds = new List<double>(){15,60,59};
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }



                // --------------------------------------------------------------------------------
                // Other Functions
                // --------------------------------------------------------------------------------
                
                // LEVY13
                case (int)EnumNomesFuncoesObjetivo.levy13:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(13, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // PAVIANI
                case (int)EnumNomesFuncoesObjetivo.paviani:
                {
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(10, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(2.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // SALOMON
                case (int)EnumNomesFuncoesObjetivo.salomon:
                {
                    parametros_problema.n_variaveis_projeto = 10;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // SCHAFFER 2
                case (int)EnumNomesFuncoesObjetivo.schaffer_2:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // BARTELS CONN
                case (int)EnumNomesFuncoesObjetivo.bartels_conn:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(17, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(500.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // BIRD
                case (int)EnumNomesFuncoesObjetivo.bird:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  2*Math.PI, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // BOHACHEVSKY 1
                case (int)EnumNomesFuncoesObjetivo.bohachevsky_1:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(15, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat( -100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(  100.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // F9 TESE
                case (int)EnumNomesFuncoesObjetivo.F09tese:
                {
                    parametros_problema.n_variaveis_projeto = 2;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(18, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(10.0, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // DEJONG3
                case (int)EnumNomesFuncoesObjetivo.dejong3:
                {
                    parametros_problema.n_variaveis_projeto = 5;
                    parametros_problema.bits_por_variavel = Enumerable.Repeat(11, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.lower_bounds = Enumerable.Repeat(-5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.upper_bounds = Enumerable.Repeat(5.12, parametros_problema.n_variaveis_projeto).ToList();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }

                // DEFAULT
                default:
                {
                    Console.WriteLine("DEFAULT PARAMETERS!");
                    parametros_problema = new ParametrosProblema();
                    parametros_problema.nome_funcao = "DEFAULT";
                    parametros_problema.n_variaveis_projeto = 0;
                    parametros_problema.function_id = function_id;
                    parametros_problema.bits_por_variavel = new List<int>();
                    parametros_problema.lower_bounds = new List<double>();
                    parametros_problema.upper_bounds = new List<double>();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }
            }

            return parametros_problema;
        }
    }
}