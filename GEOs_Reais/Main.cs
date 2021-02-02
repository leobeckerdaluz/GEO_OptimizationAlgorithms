
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
            // // GRIEWANGK
            // //======================================================
            // const double tau = 1.25;
            // const int n_variaveis_projeto = 10;
            // const int definicao_funcao_objetivo = 0;
            // List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            // for(int i=0; i<n_variaveis_projeto; i++){
            //     restricoes_laterais_variaveis.Add( 
            //         new RestricoesLaterais(){limite_inferior_variavel=-600.0, limite_superior_variavel=600.0}
            //     );
            // }

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

            // //======================================================
            // // RASTRINGIN
            // //======================================================
            // const double tau = 1.75;
            // const int n_variaveis_projeto = 20;
            // const int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rastringin;
            // List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            // for(int i=0; i<n_variaveis_projeto; i++){
            //     restricoes_laterais_variaveis.Add( 
            //         new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12}
            //     );
            // }

            //======================================================
            // RASTRINGIN VAR
            //======================================================
            const int n_variaveis_projeto = 20;
            const int definicao_funcao_objetivo = (int)EnumNomesFuncoesObjetivo.enum_rastringin;
            List<RestricoesLaterais> restricoes_laterais_variaveis = new List<RestricoesLaterais>();
            for(int i=0; i<n_variaveis_projeto; i++){
                restricoes_laterais_variaveis.Add( 
                    new RestricoesLaterais(){limite_inferior_variavel=-5.12, limite_superior_variavel=5.12}
                );
            }
            const double tau = 2.25;
            const double std1 = 8;
            const int P = 8;
            


            double std = 1;
            int step_obter_NFOBs = 250;
            int NFOB_criterio_parada = 25000;

            const int quantidade_execucoes = 50;
            List<RetornoGEOs> todos_retornos = new List<RetornoGEOs>();
            for (int i=0; i<quantidade_execucoes; i++){
                Console.Write("{0}...", i);


                // GEOsBASE_REAL geo_real = new GEOsBASE_REAL(
                //     tau,
                //     n_variaveis_projeto,
                //     definicao_funcao_objetivo,
                //     restricoes_laterais_variaveis,
                //     step_obter_NFOBs,
                //     std);

                // RetornoGEOs retorno = geo_real.rodar_por_NFOB(NFOB_criterio_parada);
                // todos_retornos.Add(retorno);


                GEOvar_REAL geovar_real = new GEOvar_REAL(
                    tau,
                    n_variaveis_projeto,
                    definicao_funcao_objetivo,
                    restricoes_laterais_variaveis,
                    step_obter_NFOBs,
                    std,
                    std1,
                    P);

                RetornoGEOs retorno = geovar_real.rodar_por_NFOB(NFOB_criterio_parada);
                todos_retornos.Add(retorno);


                // const int tipo_AGEO = 2;
                // AGEOs_REAL ageo1 = new AGEOs_REAL(
                //     tau,
                //     n_variaveis_projeto,
                //     definicao_funcao_objetivo,
                //     restricoes_laterais_variaveis,
                //     step_obter_NFOBs,
                //     std,
                //     tipo_AGEO);

                // RetornoGEOs retorno = ageo1.rodar_por_NFOB(NFOB_criterio_parada);
                // todos_retornos.Add(retorno);
            }

            // Para cada NFOB, avalia as execuções
            int quantidade_NFOBs = todos_retornos[0].melhores_NFOBs.Count;
            for(int i=0; i<quantidade_NFOBs; i++){
                double sum = 0.0;
                foreach(RetornoGEOs ret in todos_retornos){
                    sum += ret.melhores_NFOBs[i];
                }
                double media_NFOBs = sum / quantidade_execucoes;
                string media_str = (media_NFOBs.ToString()).Replace('.',',');
                Console.WriteLine(media_str);
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
