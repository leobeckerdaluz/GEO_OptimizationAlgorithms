
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


namespace MecanismoAGEO
{
    public class MecanismoAGEO {
       
        
        public double calcula_fx_referencia(
            int tipo_AGEO, 
            double fx_melhor, 
            double fx_atual)
        {
            if (tipo_AGEO == 1){
                return fx_melhor;
            }
            else{
                return fx_atual;
            }
        }



        public double calcula_CoI_bin(
            List<BitVerificado> lista_informacoes_mutacao,
            double fx_referencia,
            int tamanho_populacao)
        {
            // Verifica quantos melhora em comparação com a população de referência
            int melhoraram = lista_informacoes_mutacao.Where(p => p.funcao_objetivo_flipando < fx_referencia).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / tamanho_populacao;

            return CoI;
        }



        public double calcula_CoI_real(
            List<Perturbacao> perturbacoes_da_iteracao,
            double fx_referencia,
            int tamanho_populacao)
        {
            // Verifica quantos melhora em comparação com a população de referência
            int melhoraram = perturbacoes_da_iteracao.Where(p => p.fx_depois_da_perturbacao < fx_referencia).ToList().Count;

            // Calcula a Chance of Improvement
            double CoI = (double) melhoraram / perturbacoes_da_iteracao.Count;

            return CoI;
        }



        public double obtem_novo_tau(
            int tipo_AGEO,
            double tau,
            double CoI,
            double CoI_1,
            int tamanho_populacao)
        {
            Random random = new Random();

            double tau_resetado = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Sqrt((double)tamanho_populacao)) );
            double tau_incremento = (0.5 + CoI) * random.NextDouble();

            if ((tipo_AGEO==1) || (tipo_AGEO==2))
            {
                if (CoI == 0.0)
                    return tau_resetado;
                else if(CoI <= CoI_1)
                    return tau+tau_incremento;
                else
                    return tau;
            }
            
            else if (tipo_AGEO==3)
            {
                if (CoI == 0.0)
                    return tau_resetado;
                else if(CoI <= CoI_1)
                    return tau+tau_incremento;
                else if(CoI > CoI_1)
                    return tau-tau_incremento;
                else
                    return tau;
            }

            else if (tipo_AGEO==4)
            {
                if (CoI == 0.0)
                    return tau_resetado;
                else
                    return tau+tau_incremento;
            }
            
            else
            {
                return tau;
            }
                
        }


        public double obtem_novo_tau_AGEOvarnovo(
            int tipo_AGEO,
            double tau,
            double CoI,
            double CoI_1,
            int tamanho_populacao,
            double tau_maior_que)
        {
            Random random = new Random();

            double tau_resetado = 0.5 * MathNet.Numerics.Distributions.LogNormal.Sample(0, (1.0 / Math.Sqrt((double)tamanho_populacao)) );
            double tau_incremento = (0.5 + CoI) * random.NextDouble();

            if ((tipo_AGEO==1) || (tipo_AGEO==2))
            {
                if (CoI == 0.0 || tau > tau_maior_que)
                    return tau_resetado;
                else if(CoI <= CoI_1)
                    return tau+tau_incremento;
                else
                    return tau;
            }
            
            else
            {
                return tau;
            }
                
        }

    }
}