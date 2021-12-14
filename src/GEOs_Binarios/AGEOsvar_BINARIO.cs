using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_BINARIOS
{
    public class AGEOsvar_BINARIO: GEOvar_BINARIO
    {
        public double CoI_1 {get;set;}
        public int tipo_AGEO {get;set;}
        
        public AGEOsvar_BINARIO(
            List<bool> populacao_inicial_binaria,
            int tipo_AGEO,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            List<int> bits_por_variavel_variaveis) : base(
                populacao_inicial_binaria,
                0.5,
                n_variaveis_projeto,
                definicao_funcao_objetivo,
                lower_bounds,
                upper_bounds,
                lista_NFEs_desejados,
                bits_por_variavel_variaveis)
        {
            this.CoI_1 = 1.0 / Math.Sqrt(n_variaveis_projeto);
            this.tipo_AGEO = tipo_AGEO;
        }



        public override void mutacao_do_tau_AGEOs()
        {
            int tamanho_populacao = this.populacao_atual.Count;
            double fx_referencia = (this.tipo_AGEO==1 ? fx_melhor : fx_atual);
            
            MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();            
            double CoI = mecanismo.calcula_CoI_bin(lista_informacoes_mutacao, fx_referencia, tamanho_populacao);
            tau = mecanismo.obtem_novo_tau(this.tipo_AGEO, this.tau, CoI, this.CoI_1, 0, tamanho_populacao);

            this.CoI_1 = CoI;
        }
    }
}