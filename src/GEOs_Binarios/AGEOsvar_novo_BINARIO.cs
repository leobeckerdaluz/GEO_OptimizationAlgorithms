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
            // Obtém o tamanho da população de bits
            int tamanho_populacao = this.populacao_atual.Count;
            
            // Instancia as funções úteis do mecanismo A-GEO
            MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();
            
            // Calcula o f(x) de referência
            double fx_referencia = mecanismo.calcula_fx_referencia(tipo_AGEO, fx_melhor, fx_atual);
            
            // Calcula o CoI
            double CoI = mecanismo.calcula_CoI_bin(lista_informacoes_mutacao, fx_referencia, tamanho_populacao);
            
            
            
            // NOVA VERSÃO DE ATUALIZAR O TAU
            // Atualiza o tau
            tau = mecanismo.obtem_novo_tau_AGEOvarnovo(tipo_AGEO, tau, CoI, CoI_1, tamanho_populacao, tau_maior_que);
            
           
                
            // Armazena o CoI atual para ser usado como o anterior na próxima iteração
            this.CoI_1 = CoI;
        }
    }
}