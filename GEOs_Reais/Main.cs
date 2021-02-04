
using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;

namespace GEOs_REAIS
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Rodando...");

            // //======================================================
            // // SCHWEFEL
            // //======================================================
            // const double tau = 6.25;
            // const int n_variaveis_projeto = 10;
            // const int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_schwefel;
            // List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            // for(int i=0; i<n_variaveis_projeto; i++){
            //     restricoes_laterais_variaveis.Add( 
            //         new RestricoesLaterais(){limite_inferior_variavel=-500.0, limite_superior_variavel=500.0}
            //     );
            // }


            //======================================================
            // GRIEWANGK
            //======================================================
            const int n_variaveis_projeto = 10;
            const int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_griwangk;
            List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            for(int i=0; i<n_variaveis_projeto; i++){
                restricoes_laterais_variaveis.Add( 
                    new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0}
                );
            }
            const double tau_GEOreal1 = 1.25;
            const double tau_GEOreal2 = 1.75;
            // const double std = 1;
            const double std1 = 2.0;
            const int P = 16;


            // //======================================================
            // // RASTRINGIN
            // //======================================================
            // const int n_variaveis_projeto = 20;
            // const int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rastringin;
            // List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            // for(int i=0; i<n_variaveis_projeto; i++){
            //     restricoes_laterais_variaveis.Add( 
            //         new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12}
            //     );
            // }
            // const double tau_GEOreal1 = 1.75;
            // const double tau_GEOreal2 = 2.25;
            // // const double std = 1;
            // const double std1 = 8;
            // const int P = 8;


            List<double> stds = new List<double>{0.1, 0.2, 0.5, 0.8, 1, 1.2, 1.5};

            const int step_obter_NFOBs = 250;
            const int NFOB_criterio_parada = 25000;
            const int quantidade_execucoes = 50;

            foreach (double std in stds){

                List<RetornoGEOs> retornos_execucoes_GEOreal1 = new List<RetornoGEOs>();
                List<RetornoGEOs> retornos_execucoes_GEOreal2 = new List<RetornoGEOs>();
                List<RetornoGEOs> retornos_execucoes_AGEOreal1 = new List<RetornoGEOs>();
                List<RetornoGEOs> retornos_execucoes_AGEOreal2 = new List<RetornoGEOs>();
                
                for (int i=0; i<quantidade_execucoes; i++){
                    Console.Write("{0}...", i);

                    GEOsBASE_REAL geo_real1 = new GEOsBASE_REAL(
                        tau_GEOreal1,
                        n_variaveis_projeto,
                        definicao_funcao_objetivo,
                        restricoes_laterais_variaveis,
                        step_obter_NFOBs,
                        std);

                    RetornoGEOs retorno_GEOreal1 = geo_real1.rodar_por_NFOB(NFOB_criterio_parada);
                    retornos_execucoes_GEOreal1.Add(retorno_GEOreal1);


                    // GEOvar_REAL geo_real2 = new GEOvar_REAL(
                    //     tau_GEOreal2,
                    //     n_variaveis_projeto,
                    //     definicao_funcao_objetivo,
                    //     restricoes_laterais_variaveis,
                    //     step_obter_NFOBs,
                    //     std,
                    //     std1,
                    //     P);
                    
                    // RetornoGEOs retorno_GEOreal2 = geo_real2.rodar_por_NFOB(NFOB_criterio_parada);
                    // retornos_execucoes_GEOreal2.Add(retorno_GEOreal2);


                    AGEOs_REAL AGEOreal1 = new AGEOs_REAL(
                        tau_GEOreal1,
                        n_variaveis_projeto,
                        definicao_funcao_objetivo,
                        restricoes_laterais_variaveis,
                        step_obter_NFOBs,
                        std,
                        1);

                    RetornoGEOs retorno_AGEOreal1 = AGEOreal1.rodar_por_NFOB(NFOB_criterio_parada);
                    retornos_execucoes_AGEOreal1.Add(retorno_AGEOreal1);


                    AGEOs_REAL AGEOreal2 = new AGEOs_REAL(
                        tau_GEOreal1,
                        n_variaveis_projeto,
                        definicao_funcao_objetivo,
                        restricoes_laterais_variaveis,
                        step_obter_NFOBs,
                        std,
                        2);

                    RetornoGEOs retorno_AGEOreal2 = AGEOreal2.rodar_por_NFOB(NFOB_criterio_parada);
                    retornos_execucoes_AGEOreal2.Add(retorno_AGEOreal2);
                }

                // Para cada NFOB, avalia as execuções
                int quantidade_NFOBs = retornos_execucoes[0].melhores_NFOBs.Count;
                Console.WriteLine("");
                for(int i=0; i<quantidade_NFOBs; i++){
                    double sum = 0.0;
                    foreach(RetornoGEOs ret in retornos_execucoes){
                        sum += ret.melhores_NFOBs[i];
                    }
                    double media_NFOBs = sum / quantidade_execucoes;
                    string media_str = (media_NFOBs.ToString()).Replace('.',',');
                    Console.WriteLine(media_str);
                }

            }
           


            // double xi = 600;
            // for(int i=0; i<50; i++){
            //     MathNet.Numerics.Distributions.Normal normalDist = new MathNet.Numerics.Distributions.Normal(0, 1);

            //     double sample = normalDist.Sample();
            //     Console.WriteLine(sample);
                
            //     // double xii = xi + sample * xi;
            //     // Console.WriteLine("Era {0} e perturbou para {1}", xi, xii);
            // }
        }
    }
}
