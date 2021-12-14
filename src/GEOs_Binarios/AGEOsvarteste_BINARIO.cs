using System;
using System.Collections.Generic;
using Classes_Comuns_Enums;
using System.Linq;

namespace GEOs_BINARIOS
{
    public class AGEOvar_novo_BINARIO: GEOvar_BINARIO
    {
        public double CoI_1 {get;set;}
        public int tipo_AGEO {get;set;}
        public double tau_maior_que {get;set;}
        
        public AGEOvar_novo_BINARIO(
            List<bool> populacao_inicial_binaria,
            int tipo_AGEO,
            int n_variaveis_projeto,
            int definicao_funcao_objetivo,
            List<double> lower_bounds,
            List<double> upper_bounds,
            List<int> lista_NFEs_desejados,
            List<int> bits_por_variavel_variaveis,
            double tau_maior_que) : base(
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
            this.tau_maior_que = tau_maior_que;
        }



        public override void mutacao_do_tau_AGEOs()
        {
            int tamanho_populacao = this.populacao_atual.Count;
            double fx_referencia = (this.tipo_AGEO==1 ? fx_melhor : fx_atual);
            
            MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();            
            
            double CoI = mecanismo.calcula_CoI_bin(lista_informacoes_mutacao, fx_referencia, tamanho_populacao);
            

            Random random = new Random();
            double tau_incremento = 0.5 * Math.Exp(random.NextDouble() * (1.0 / Math.Sqrt( (double)tamanho_populacao )));
            double tau_resetado = (0.5 + CoI) * random.NextDouble();

            if (CoI == 0.0 || tau > tau_maior_que)
                tau = tau_resetado;
            else if(CoI <= CoI_1)
                tau += tau_incremento;



            this.CoI_1 = CoI;
        }
    }
}