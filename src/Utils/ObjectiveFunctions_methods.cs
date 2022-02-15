using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SpaceDesignTeste;
using Classes_Comuns_Enums;

using SpaceConceptOptimizer.Models;
using MathModelsDomain.Utilities;
using SpaceConceptOptimizer.Settings;
using SpaceConceptOptimizer.Utilities;
using System.IO;
using System.Threading.Tasks;


namespace ObjectiveFunctions
{
    public class Methods
    {
        /*
        * =========================================================================
        * Description of the functions of this file
        * =========================================================================
        *
        * // Convert List<double> phenotype to double[] for CEC2017 Functions
        * private static double[] convert_listdouble_to_doublearray(List<double> fenotipo_variaveis_projeto);
        *
        * // Set All Functions Parameters
        * public static ParametrosProblema get_function_parameters(int function_id);
        *
        * // Invoke Available Objective Functions 
        * public static double funcao_objetivo(List<double> fenotipo_variaveis_projeto, int definicao_funcao_objetivo);
        *
        * // Functions Definitions
        * public static double .....function_name.....(List<double> fenotipo_variaveis_projeto);
        * =========================================================================
        */
        



        // =========================================================================
        // Convert List<double> phenotype to double[] for CEC2017 Functions
        // =========================================================================
        private static double[] convert_listdouble_to_doublearray(List<double> fenotipo_variaveis_projeto)
        {
            int N = fenotipo_variaveis_projeto.Count;
            double[] phenotype = new double[N];
            
            for(int i=0; i<N; i++){
                phenotype[i] = fenotipo_variaveis_projeto[i];
            }
            
            return phenotype;
        }


        
        // =========================================================================
        // Set All Functions Parameters
        // =========================================================================
        public static ParametrosProblema get_function_parameters(int function_id)
        {
            // Generate initial problem parameters
            ParametrosProblema parametros_problema = new ParametrosProblema(){
                nome_funcao = Enum.GetName(typeof(EnumNomesFuncoesObjetivo), function_id),
                definicao_funcao_objetivo = function_id
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
                    parametros_problema.definicao_funcao_objetivo = function_id;
                    parametros_problema.bits_por_variavel = new List<int>();
                    parametros_problema.lower_bounds = new List<double>();
                    parametros_problema.upper_bounds = new List<double>();
                    parametros_problema.parametros_livres = new ParametrosLivreProblema();
                break;
                }
            }

            return parametros_problema;
        }
        
        
        
        
        // =========================================================================
        // Invoke Available Objective Functions 
        // =========================================================================
        public static double funcao_objetivo(List<double> fenotipo_variaveis_projeto, int definicao_funcao_objetivo){
            double fx = Double.MaxValue;

            // Executa a função objetivo conforme o código dela
            switch(definicao_funcao_objetivo){

                // ------------------------------------------------------------
                // Benchmark Functions
                // ------------------------------------------------------------
                
                // Griewangk
                case (int)EnumNomesFuncoesObjetivo.griewangk:{
                    return funcao_griewank(fenotipo_variaveis_projeto);
                }

                // Rastrigin
                case (int)EnumNomesFuncoesObjetivo.rastringin:{
                    return funcao_Rastringin(fenotipo_variaveis_projeto);
                }

                // Rosenbrock
                case (int)EnumNomesFuncoesObjetivo.rosenbrock:{
                    return funcao_rosenbrock(fenotipo_variaveis_projeto);
                }

                // Schwefel
                case (int)EnumNomesFuncoesObjetivo.schwefel:{
                    return funcao_Schwefel(fenotipo_variaveis_projeto);
                }

                // Ackley
                case (int)EnumNomesFuncoesObjetivo.ackley:{
                    return funcao_Ackley(fenotipo_variaveis_projeto);
                }

                // Beale
                case (int)EnumNomesFuncoesObjetivo.beale:{
                    return funcao_Beale(fenotipo_variaveis_projeto);
                }



                // ------------------------------------------------------------
                // CEC2017
                // ------------------------------------------------------------

                case (int)EnumNomesFuncoesObjetivo.CEC2017_01:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 1, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_03:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 3, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_04:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 4, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_05:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 5, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_06:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 6, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_07:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 7, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_08:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 8, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_09:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 9, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_10:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 10, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_11:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 11, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_12:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 12, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_13:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 13, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_14:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 14, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_15:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 15, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_16:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 16, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_17:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 17, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_18:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 18, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_19:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 19, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_20:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 20, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_21:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 21, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_22:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 22, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_23:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 23, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_24:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 24, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_25:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 25, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_26:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 26, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_27:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 27, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_28:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 28, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_29:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 29, fenotipo_variaveis_projeto.Count);
                }

                case (int)EnumNomesFuncoesObjetivo.CEC2017_30:{
                    double [] phenotype = convert_listdouble_to_doublearray(fenotipo_variaveis_projeto);
                    return CppBind.CEC2017.cec2017_run_function(phenotype, 30, fenotipo_variaveis_projeto.Count);
                }



                // ------------------------------------------------------------
                // Spacecraft Function
                // ------------------------------------------------------------

                case (int)EnumNomesFuncoesObjetivo.spacecraft:{

                    // Caso não ocorra de Q<D, já nem calcula. Caso calcule, ainda pode ter restrições lá dentro
                    // ...da função, mas essa já é uma etapa pra poupar processamento.
                    int I = (int)fenotipo_variaveis_projeto[0];
                    int D = (int)fenotipo_variaveis_projeto[1];
                    int Q = (int)fenotipo_variaveis_projeto[2];

                    
                    if (D<=Q || I<13 || I>15 || D<1 || D>60 || Q<1){
                        // Console.WriteLine("Já saiu antes!");
                        return Double.MaxValue;
                    }

                    int a=1;
                    
                    // Calcula f(x). Se a solução não é viável (FOV...), retorna MaxValue
                    SpaceDesignTeste.TesteOptimizer spacecraft_model = new SpaceDesignTeste.TesteOptimizer(fenotipo_variaveis_projeto);
                    if (spacecraft_model.valid_solution)
                        return spacecraft_model.fx_calculada;
                    else
                        return Double.MaxValue;

                }



                // ------------------------------------------------------------
                // Functions Not Tested
                // ------------------------------------------------------------

                // DeJong3
                case (int)EnumNomesFuncoesObjetivo.dejong3:{
                    return funcao_DeJong3_inteiro(fenotipo_variaveis_projeto);
                }

                // F9
                case (int)EnumNomesFuncoesObjetivo.F09tese:{
                    return funcao_F9(fenotipo_variaveis_projeto);
                }
                    
                // Levy13
                case (int)EnumNomesFuncoesObjetivo.levy13:{
                    return funcao_Levy13(fenotipo_variaveis_projeto);
                }

                // F10
                case (int)EnumNomesFuncoesObjetivo.F10:{
                    double xi1 = fenotipo_variaveis_projeto[0];
                    double xi2 = fenotipo_variaveis_projeto[1];
                    
                    if (xi1+xi2 < 4){
                        // Espaço inviável
                        return Double.MaxValue;
                    }
                    else{
                        return funcao_F10(fenotipo_variaveis_projeto);
                    }
                }

                // Cosine Mixture
                case (int)EnumNomesFuncoesObjetivo.cosine_mixture:{
                    return funcao_cosine_mixture(fenotipo_variaveis_projeto);
                }

                // McCormick
                case (int)EnumNomesFuncoesObjetivo.mccormick:{
                    return funcao_mccormick(fenotipo_variaveis_projeto);
                }

                // Paviani
                case (int)EnumNomesFuncoesObjetivo.paviani:{
                    return funcao_paviani(fenotipo_variaveis_projeto);
                }

                // Salomon
                case (int)EnumNomesFuncoesObjetivo.salomon:{
                    return funcao_salomon(fenotipo_variaveis_projeto);
                }

                // Schaffer 2
                case (int)EnumNomesFuncoesObjetivo.schaffer_2:{
                    return funcao_schaffer_2(fenotipo_variaveis_projeto);
                }

                // Adjiman
                case (int)EnumNomesFuncoesObjetivo.adjiman:{
                    return funcao_adjiman(fenotipo_variaveis_projeto);
                }

                // Alpine 1
                case (int)EnumNomesFuncoesObjetivo.alpine01:{
                    return funcao_alpine01(fenotipo_variaveis_projeto);
                }

                // Bartels Conn
                case (int)EnumNomesFuncoesObjetivo.bartels_conn:{
                    return funcao_bartels_conn(fenotipo_variaveis_projeto);
                }

                // Bird
                case (int)EnumNomesFuncoesObjetivo.bird:{
                    return funcao_bird(fenotipo_variaveis_projeto);
                }

                // Bohachevsky 1
                case (int)EnumNomesFuncoesObjetivo.bohachevsky_1:{
                    return funcao_bohachevsky_1(fenotipo_variaveis_projeto);
                }
            }

            return fx;
        }



        // =========================================================================
        // Functions Definitions
        // =========================================================================
        public static double funcao_Levy13(List<double> fenotipo_variaveis_projeto){
            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];
            
            double f1 = Math.Pow(Math.Sin(3*Math.PI*x1), 2);
            double f2 = Math.Pow((x1 - 1), 2) * (1 + Math.Pow(Math.Sin(3*Math.PI*x2), 2));
            double f3 = Math.Pow((x2 - 1), 2) * (1 + Math.Pow(Math.Sin(2*Math.PI*x2), 2));
            double fx = f1 + f2 + f3;
            
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
                double xi = fenotipo_variaveis_projeto[i];
                laco_somatorio += Math.Pow(xi, 2);
                // laco_produto *= Math.Cos( Math.PI * xi / Math.Sqrt(i+1) );
                laco_produto *= Math.Cos( xi / Math.Sqrt(i+1) );
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
                double cosseno = 3.0*Math.Cos(2.0 * Math.PI * xi);
                
                laco_somatorio += quadrado - cosseno;
            }

            // Expressão final de f(x)
            double fx = 3.0*fenotipo_variaveis_projeto.Count + laco_somatorio;

            // Retorna o valor de f(x)
            return fx;
        }

        public static double funcao_Schwefel(List<double> fenotipo_variaveis_projeto){
            double laco_somatorio = 0.0;

            int n = fenotipo_variaveis_projeto.Count;

            for(int i=0; i<n; i++){
                double xi = fenotipo_variaveis_projeto[i];
                laco_somatorio += xi * Math.Sin(Math.Sqrt(Math.Abs(xi)));
            }

            double fx = 418.9829*n - laco_somatorio;

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

            // Calcula os Exp antes para depois juntar
            double Exp1 = Math.Exp(-0.2 * Math.Sqrt( (1.0/N) * laco_somatorio_1 ));
            double Exp2 = Math.Exp( (1.0/N) * laco_somatorio_2 );

            // Expressão final de f(x)
            // double fx = 20.0 + Math.Exp(1.0) - 20.0*Exp1 - Exp2;
            double fx = -20.0*Exp1 - Exp2 + Math.E + 20.0;

            // Retorna o valor de f(x)
            return fx;
        }

        public static double funcao_Beale(List<double> fenotipo_variaveis_projeto){
            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];
            
            double f1 = Math.Pow( (1.5 - x1 + x1*x2), 2);
            double f2 = Math.Pow( (2.25 - x1 + x1*Math.Pow(x2,2)), 2);
            double f3 = Math.Pow( (2.625 - x1 + x1*Math.Pow(x2,3)), 2);
            double fx = f1 + f2 + f3;
            
            return fx;
        }

        public static double funcao_F9(List<double> fenotipo_variaveis_projeto){
            double xi1 = fenotipo_variaveis_projeto[0];
            double xi2 = fenotipo_variaveis_projeto[1];

            double soma_quadrados = Math.Pow(xi1, 2.0) + Math.Pow(xi2, 2.0);
            double cosseno1 = Math.Cos(20.0 * Math.PI * xi1);
            double cosseno2 = Math.Cos(20.0 * Math.PI * xi2);

            // Expressão final de f(x)
            double fx = 1.0/2.0 * soma_quadrados - cosseno1*cosseno2 + 2.0;

            // Retorna o valor de f(x)
            return fx;
        }

        public static double funcao_F10(List<double> fenotipo_variaveis_projeto){
            double xi1 = fenotipo_variaveis_projeto[0];
            double xi2 = fenotipo_variaveis_projeto[1];

            // Expressão final de f(x)
            double fx = 0.25*Math.Pow(xi1,4) - 3*Math.Pow(xi1,3) + 11*Math.Pow(xi1,2) - 13*xi1 + 0.25*Math.Pow(xi2,4) - 3*Math.Pow(xi2,3) + 11*Math.Pow(xi2,2) - 13*xi1;

            // Retorna o valor de f(x)
            return fx;
        }

        public static double funcao_cosine_mixture(List<double> fenotipo_variaveis_projeto){
            //http://jakobbossek.github.io/smoof/reference/makeCosineMixtureFunction.html
            //http://infinity77.net/global_optimization/test_functions_nd_C.html
            
            double laco_somatorio_1 = 0.0;
            double laco_somatorio_2 = 0.0;

            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];

                laco_somatorio_1 += Math.Cos(5.0 * Math.PI * xi);
                laco_somatorio_2 += Math.Pow(xi, 2);
            }

            double fx = -0.1*laco_somatorio_1 - laco_somatorio_2;
            return fx;
        }

        public static double funcao_mccormick(List<double> fenotipo_variaveis_projeto){
            //https://al-roomi.org/benchmarks/unconstrained/2-dimensions/61-mccormick-s-function
            //https://www.sfu.ca/~ssurjano/mccorm.html
            
            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];

            double fx = Math.Sin(x1+x2) + Math.Pow((x1-x2),2) - 1.5*x1 + 2.5*x2 + 1;
            return fx;
        }

        public static double funcao_paviani(List<double> fenotipo_variaveis_projeto){
            //http://infinity77.net/global_optimization/test_functions_nd_P.html
            //http://www.geocities.ws/eadorio/mvf.pdf
            
            double laco_somatorio_1 = 0.0;
            double laco_produtorio_1 = 1;

            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];

                laco_somatorio_1 += (Math.Pow(Math.Log(xi-2), 2) + Math.Pow(Math.Log(10-xi), 2));
                laco_produtorio_1 *= xi;
            }

            double fx = laco_somatorio_1 - Math.Pow(laco_produtorio_1, 0.2);
            return fx;
        }

        public static double funcao_salomon(List<double> fenotipo_variaveis_projeto){
            //https://www.indusmic.com/post/salomon-function
            //https://al-roomi.org/benchmarks/unconstrained/n-dimensions/184-salomon-s-function
            
            double laco_somatorio_1 = 0.0;

            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];
                laco_somatorio_1 += Math.Pow(xi, 2);
            }
            double sqrt_x = Math.Sqrt(laco_somatorio_1);

            double fx = 1 - Math.Cos(2*Math.PI*sqrt_x) + 0.1*sqrt_x;
            return fx;
        }   

        public static double funcao_alpine01(List<double> fenotipo_variaveis_projeto){
            //http://infinity77.net/global_optimization/test_functions_nd_A.html
            //https://al-roomi.org/benchmarks/unconstrained/n-dimensions/162-alpine-function-no-1

            double laco_somatorio_1 = 0.0;

            for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
                double xi = fenotipo_variaveis_projeto[i];

                double res = xi*Math.Sin(xi) + 0.1*xi;
                laco_somatorio_1 += Math.Abs(res);
            }
            double fx = laco_somatorio_1;

            return fx;
        }   

        public static double funcao_schaffer_2(List<double> fenotipo_variaveis_projeto){
            //https://www.sfu.ca/~ssurjano/schaffer2.html
            //https://www.indusmic.com/post/python-implementation-of-schaffer-function

            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];
                
            double soma_quadrados = Math.Pow(x1,2) + Math.Pow(x2,2);
            double subt_quadrados = Math.Pow(x1,2) - Math.Pow(x2,2);
            double num = Math.Pow( Math.Sin(subt_quadrados), 2) - 0.5;
            double den = Math.Pow(1 + 0.001*soma_quadrados, 2);
            
            double fx = 0.5 + num/den;

            return fx;
        }

        public static double funcao_bartels_conn(List<double> fenotipo_variaveis_projeto){
            //https://al-roomi.org/benchmarks/unconstrained/2-dimensions/72-bartels-conn-s-function
            //https://www.indusmic.com/post/bartels-conn-function

            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];
                
            double termo1 = Math.Abs (Math.Pow(x1,2) + Math.Pow(x2,2) + x1*x2);
            double termo2 = Math.Abs (Math.Sin(x1));
            double termo3 = Math.Abs (Math.Cos(x2));
            
            double fx = termo1 + termo2 + termo3;   
            return fx;
        }

        public static double funcao_bird(List<double> fenotipo_variaveis_projeto){
            //https://www.indusmic.com/post/bird-function

            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];

            double exp1 = Math.Pow((1 - Math.Cos(x2)),2);
            double exp2 = Math.Pow((1 - Math.Sin(x1)),2);

            double termo1 = Math.Sin(x1)*Math.Exp(exp1);
            double termo2 = Math.Cos(x2)*Math.Exp(exp2);
            double termo3 = Math.Pow((x1-x2),2);
            
            double fx = termo1 + termo2 + termo3;   
            return fx;
        }

        public static double funcao_adjiman(List<double> fenotipo_variaveis_projeto){
            //https://al-roomi.org/benchmarks/unconstrained/2-dimensions/113-adjiman-s-function
            //https://www.indusmic.com/post/happy-cat-function
            //http://infinity77.net/global_optimization/test_functions_nd_A.html

            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];
                
            double fx = Math.Cos(x1)*Math.Sin(x2) - x1/(Math.Pow(x2,2)+1);
            return fx;
        }

        public static double funcao_bohachevsky_1(List<double> fenotipo_variaveis_projeto){
            //http://jakobbossek.github.io/smoof/reference/makeBohachevskyN1Function.html
            //https://www.sfu.ca/~ssurjano/boha.html
            
            double x1 = fenotipo_variaveis_projeto[0];
            double x2 = fenotipo_variaveis_projeto[1];

            double termo1 = Math.Pow(x1, 2);
            double termo2 = 2*Math.Pow(x2, 2);
            double termo3 = -0.3*Math.Cos(3*Math.PI*x1);
            double termo4 = -0.4*Math.Cos(4*Math.PI*x2);

            double fx = termo1 + termo2 + termo3 + termo4 + 0.7;
            return fx;
        }
    
        // public static double funcao_schaffer____(List<double> fenotipo_variaveis_projeto){
        //     //https://www.sfu.ca/~ssurjano/schaffer2.html
        //     double x1 = fenotipo_variaveis_projeto[0];
        //     double x2 = fenotipo_variaveis_projeto[1];
        //     double soma_quadrados = Math.Pow(x1,2) + Math.Pow(x2,2)
        //     double fx = Math.Pow(soma_quadrados, 0.25) * (Math.Pow(Math.Sin(50*Math.Pow(soma_quadrados,0.1)), 2) + 1); 
        //     return fx;
        // }

        // public static double funcao_exponential(List<double> fenotipo_variaveis_projeto){
        //     // ??????????
        //     double laco_somatorio_1 = 0.0;
        //     for(int i=0; i<fenotipo_variaveis_projeto.Count; i++){
        //         double xi = fenotipo_variaveis_projeto[i];
        //         laco_somatorio_1 += Math.Pow(xi, 2);
        //     }
        //     double fx = Math.Exp(-0.5*laco_somatorio_1);
        //     return fx;
        // }
    }
}