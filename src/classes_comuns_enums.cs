using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Classes_Comuns_Enums
{
    /*
        Classe para armazenar as informações da mutação de um bit.
        Para cada bit, é armazenado o delta fitness caso mute e o indice do bit na população
    */
    public class BitVerificado{
        public double funcao_objetivo_flipando { get; set; }
        public int indice_bit_mutado { get; set; }
    }
    
    
    public class RetornoGEOs {
        public int algoritmo_utilizado { get; set; }
        public int NFE { get; set; }
        public int iteracoes { get; set; }
        public double melhor_fx { get; set; }
        public List<double> melhores_NFEs { get; set; }
        public List<double> melhores_TAUs { get; set; }
        public List<double> populacao_final { get; set; }
        public List<double> stats_TAU_per_iteration { get; set; }
        public List<double> stats_STD_per_iteration { get; set; }
        public List<double> stats_Mfx_per_iteration { get; set; }
        
    }

    public class Retorno_N_Execucoes_GEOs {
        public int algoritmo_utilizado { get; set; }
        public int NFE_medio { get; set; }
        public int ITERACOES_medio { get; set; }
        public double media_melhor_fx { get; set; }
        public double SD_do_melhor_fx { get; set; }
        public List<double> media_valor_FO_em_cada_NFE { get; set; }
        public List<double> lista_melhores_fxs { get; set; }
        public List<List<double>> lista_populacao_final { get; set; }
        public List<double> lista_TAU_medio_per_iteration { get; set; }
        public List<double> lista_STD_medio_per_iteration { get; set; }
        public List<double> lista_Mfx_medio_per_iteration { get; set; }
    }

    public class ParametrosCriterioParada {
        public int tipo_criterio_parada { get; set; }
        public int NFE_criterio_parada { get; set; }
        public int ITERATIONS_criterio_parada { get; set; }
        public double PRECISAO_criterio_parada { get; set; }
        public double fx_esperado { get; set; }
        public List<int> lista_NFEs_desejados { get; set; }
    }
    // 'parada_por_NFE_atingido';
    // 'parada_por_Precisao_atingida';
    // 'parada_por_Precisao_ou_NFE_atingidos';


    public class QuaisAlgoritmosRodar {
        public bool rodar_GEO { get; set; }
        public bool rodar_GEOvar { get; set; }
        public bool rodar_AGEO1 { get; set; }
        public bool rodar_AGEO2 { get; set; }
        public bool rodar_AGEO1var { get; set; }
        public bool rodar_AGEO2var { get; set; }
        public bool rodar_GEOreal1_igor { get; set; }
        public bool rodar_AGEO1real1_igor { get; set; }
        public bool rodar_AGEO2real1_igor { get; set; }
        public bool rodar_GEOreal2_igor { get; set; }
        public bool rodar_AGEO1real2_igor { get; set; }
        public bool rodar_AGEO2real2_igor { get; set; }
        public bool rodar_ASGEO2real1_1 { get; set; }
        public bool rodar_ASGEO2real1_2 { get; set; }
        public bool rodar_ASGEO2real1_3 { get; set; }
        public bool rodar_ASGEO2real1_4 { get; set; }
        public bool rodar_ASGEO2real2_1 { get; set; }
        public bool rodar_ASGEO2real2_2 { get; set; }
        
        public bool rodar_AGEO2_REAL1_igor { get; set; }
        public bool rodar_AGEO2_REAL1_porcentagem { get; set; }
        public bool rodar_AGEO2_REAL1_normal { get; set; }
        public bool rodar_AGEO2_REAL2_igor { get; set; }
        public bool rodar_AGEO2_REAL2_porcentagem { get; set; }
        public bool rodar_AGEO2_REAL2_normal { get; set; }
        
    }


    public class ParametrosProblema {
        public int definicao_funcao_objetivo { get; set; }
        public string nome_funcao { get; set; }
        public int n_variaveis_projeto { get; set; }
        public List<int> bits_por_variavel { get; set; }
        public List<double> lower_bounds { get; set; }
        public List<double> upper_bounds { get; set; }
        public ParametrosLivreProblema parametros_livres { get; set; }
        public List<double> populacao_inicial_real { get; set; }
        public List<bool> populacao_inicial_binaria { get; set; }
    }

    public class ParametrosExecucao {
        public int quantidade_execucoes { get; set; }
        public ParametrosCriterioParada parametros_criterio_parada { get; set; }
        public QuaisAlgoritmosRodar quais_algoritmos_rodar { get; set; }
        public OQueInteressaPrintar o_que_interessa_printar { get; set; }
        public int tipo_perturbacao { get; set; }
    }
    
    public class CodificacaoBinariaParaFenotipo {
        public List<int> bits_por_variavel_variaveis { get; set; }
        public List<double> limites_inferiores_variaveis { get; set; }
        public List<double>  limites_superiores_variaveis { get; set; }
    }

    public class RestricoesLaterais {
        public double limite_inferior_variavel { get; set; }
        public double limite_superior_variavel { get; set; }
    }

    public class Perturbacao{
        public double xi_antes_da_perturbacao {get; set;}
        public double xi_depois_da_perturbacao {get; set;}
        public double fx_depois_da_perturbacao {get; set;}
        public int indice_variavel_projeto {get; set;}
    }

    public class OQueInteressaPrintar{
        public bool mostrar_melhores_NFE {get; set;}
        public bool mostrar_meanNFE_meanFX_sdFX {get; set;}
        public bool mostrar_melhores_fx_cada_execucao {get; set;}
        public bool mostrar_header {get; set;}
        public bool mostrar_mean_TAU_iteracoes {get; set;}
        public bool mostrar_mean_STD_iteracoes {get; set;}
        public bool mostrar_mean_Mfx_iteracoes {get; set;}
    }

    public class ParametrosLivreProblema{
        public double tau_GEO {get; set;}
        public double tau_GEOvar {get; set;}
        public double tau_GEOreal1 {get; set;}
        public double tau_GEOreal2 {get; set;}
        public double tau_ASGEO2_REAL1_1 {get; set;}
        public double tau_ASGEO2_REAL1_2 {get; set;}
        public double tau_ASGEO2_REAL1_3 {get; set;}
        public double tau_ASGEO2_REAL1_4 {get; set;}
        public double tau_minimo_AGEOs {get; set;}
        public double std_GEOreal1 {get; set;}
        public double std_AGEO1real1 {get; set;}
        public double std_AGEO2real1 {get; set;}
        public double std_GEOreal2 {get; set;}
        public double std_AGEO1real2 {get; set;}
        public double std_AGEO2real2 {get; set;}
        public double p1_AGEO1real1 {get; set;}
        public double p1_AGEO2real1 {get; set;}
        public double std_ASGEO2_REAL1_1 {get; set;}
        public double std_ASGEO2_REAL1_2 {get; set;}
        public double std_ASGEO2_REAL1_3 {get; set;}
        public double std_ASGEO2_REAL1_4 {get; set;}
        public double P_GEOreal2 {get; set;}
        public double P_AGEO1real2 {get; set;}
        public double P_AGEO2real2 {get; set;}
        public double P_GEOreal3 {get; set;}
        public double P_AGEO1real3 {get; set;}
        public double P_AGEO2real3 {get; set;}
        public double s_GEOreal2 {get; set;}
        public double s_AGEO1real2 {get; set;}
        public double s_AGEO2real2 {get; set;}
        public int q_one_fifth_rule {get; set;}
        public double c_one_fifth_rule {get; set;}
        public double stdmin_one_fifth_rule {get; set;}
        public int ASGEO2_REAL2_1_P {get; set;}
        public double ASGEO2_REAL2_1_std1 {get; set;}
        public int ASGEO2_REAL2_1_s {get; set;}
    }

    enum EnumNomesFuncoesObjetivo{
        griewangk,
        rosenbrock,
        dejong3,
        spacecraft,
        rastringin,
        schwefel,
        ackley,
        F09,
        F10,
        levy13,
        beale,
        cosine_mixture,
        mccormick,
        paviani,
        salomon,
        schaffer_2,
        adjiman,
        alpine01,
        bartels_conn,
        bird,
        bohachevsky_1,
    }

    enum EnumNomesAlgoritmos{
        GEO_can,
        GEO_var,
        AGEO1,
        AGEO2,
        AGEO1var,
        AGEO2var,
        GEOreal1_igor,
        AGEO1real1_igor,
        AGEO2real1_igor,
        GEOreal2_igor,
        AGEO1real2_igor,
        AGEO2real2_igor,
        ASGEO2real1_1,
        ASGEO2real1_2,
        ASGEO2real1_3,
        ASGEO2real1_4,
        ASGEO2real2_1,
        ASGEO2real2_2,
        ASGEO2real2_3,

        AGEO2_REAL1_igor,
        AGEO2_REAL1_porcentagem,
        AGEO2_REAL1_normal,
        AGEO2_REAL2_igor,
        AGEO2_REAL2_porcentagem,
        AGEO2_REAL2_normal,

    }

    enum EnumTipoCriterioParada{
        parada_por_NFE,
        parada_por_PRECISAO,
        parada_por_PRECISAOouNFE,
        parada_por_ITERATIONS
    }

    enum EnumTipoPerturbacao{
        perturbacao_igor,
        perturbacao_porcentagem,
        perturbacao_normal,
    }


    enum EnumTipoVariacaoStdNasPPerturbacoes{
        variacao_real_original,
        variacao_divide_por_s,
    }
}