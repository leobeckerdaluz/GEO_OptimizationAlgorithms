using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Classes_e_Enums
{
    // Classe para armazenar as informações da mutação de um bit.
    public class BitVerificado{
        public double funcao_objetivo_flipando { get; set; }
        public int indice_bit_mutado { get; set; }
    }

    
    public class Tuning{
        public string parameters { get; set; }
        public double fx { get; set; }
        public double sd { get; set; }
        public int NFE { get; set; }
    }

    
    public class RetornoGEOs {
        public int algoritmo_utilizado { get; set; }
        public int NFE { get; set; }
        public int iteracoes { get; set; }
        public double melhor_fx { get; set; }
        public double fx_atual { get; set; }
        public List<double> melhores_NFEs { get; set; }
        public List<double> fxs_atuais_NFEs { get; set; }
        public List<double> melhores_TAUs { get; set; }
        public List<double> populacao_final { get; set; }
        public List<double> stats_TAU_per_iteration { get; set; }
        public List<double> stats_STDPORC_per_iteration { get; set; }
        public List<double> stats_Mfx_per_iteration { get; set; }
        
    }

    
    public class Retorno_N_Execucoes_GEOs {
        public int codigo_algoritmo_executado { get; set; }
        public string nome_algoritmo_executado { get; set; }
        public int NFE_medio { get; set; }
        public int ITERACOES_medio { get; set; }
        public double media_melhor_fx { get; set; }
        public double pior_fx_de_todos { get; set; }
        public double melhor_fx_de_todos { get; set; }
        public double mediana_melhor_fx { get; set; }
        public double media_fx_atual { get; set; }
        public double SD_do_melhor_fx { get; set; }
        public List<double> media_valor_FO_em_cada_NFE { get; set; }
        public List<double> media_fx_atual_em_cada_NFE { get; set; }
        public List<double> lista_melhores_fxs { get; set; }
        public List<List<double>> lista_populacao_final { get; set; }
        public List<double> lista_TAU_medio_per_iteration { get; set; }
        public List<double> lista_STDPORC_medio_per_iteration { get; set; }
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


    public class QuaisAlgoritmosRodar {
        // Binários
        public bool rodar_GEO { get; set; }
        public bool rodar_GEOvar { get; set; }
        public bool rodar_AGEO1 { get; set; }
        public bool rodar_AGEO2 { get; set; }
        public bool rodar_AGEO3 { get; set; }
        public bool rodar_AGEO4 { get; set; }
        public bool rodar_AGEO9 { get; set; }
        
        public bool rodar_AGEO1var { get; set; }
        public bool rodar_AGEO2var { get; set; }
        public bool rodar_AGEO3var { get; set; }
        public bool rodar_AGEO4var { get; set; }
        public bool rodar_AGEO9var { get; set; }

        public bool rodar_AGEO1var_1 { get; set; }
        public bool rodar_AGEO1var_3 { get; set; }
        public bool rodar_AGEO1var_5 { get; set; }
        public bool rodar_AGEO1var_7 { get; set; }
        public bool rodar_AGEO1var_9 { get; set; }
        public bool rodar_AGEO1var_11 { get; set; }
        public bool rodar_AGEO2var_1 { get; set; }
        public bool rodar_AGEO2var_3 { get; set; }
        public bool rodar_AGEO2var_5 { get; set; }
        public bool rodar_AGEO2var_7 { get; set; }
        public bool rodar_AGEO2var_9 { get; set; }
        public bool rodar_AGEO2var_11 { get; set; }
        
        public bool rodar_GEOvar2 { get; set; }
        
        // GEOreal1
        public bool rodar_GEOreal1_O { get; set; }
        public bool rodar_GEOreal1_P { get; set; }
        public bool rodar_GEOreal1_N { get; set; }

        // GEOreal2
        public bool rodar_GEOreal2_O_VO { get; set; }
        public bool rodar_GEOreal2_P_VO { get; set; }
        public bool rodar_GEOreal2_N_VO { get; set; }
        public bool rodar_GEOreal2_O_DS { get; set; }
        public bool rodar_GEOreal2_P_DS { get; set; }
        public bool rodar_GEOreal2_N_DS { get; set; }

        // GEOreal2 + 1 Uniforme
        public bool rodar_GEOreal2_P_VO_UNI { get; set; }
        public bool rodar_GEOreal2_N_VO_UNI { get; set; }
        public bool rodar_GEOreal2_P_DS_UNI { get; set; }
        public bool rodar_GEOreal2_N_DS_UNI { get; set; }
        
        // A-GEOreal
        public bool rodar_AGEO2real1_P { get; set; }
        public bool rodar_AGEO2real2_P_DS { get; set; }
        public bool rodar_AGEO2real2_P_DS_fixo { get; set; }
        public bool rodar_AGEO2real1_P_AA { get; set; }
        public bool rodar_AGEO2real2_AA0 { get; set; }
        public bool rodar_AGEO2real2_AA1 { get; set; }
        public bool rodar_AGEO2real2_AA2 { get; set; }
        public bool rodar_AGEO2real2_AA3 { get; set; }
        public bool rodar_AGEO2real2_P_AA_p9 { get; set; }
    }


    public class ParametrosProblema {
        public int function_id { get; set; }
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
        public List<double> populacao_depois_da_perturbacao {get; set;}
        public double porcentagem_usada_nessa_perturb {get; set;}
        public int indice_variavel_projeto {get; set;}
    }

    
    public class OQueInteressaPrintar{
        public bool mostrar_melhores_NFE {get; set;}
        public bool mostrar_fxs_atual_por_NFE {get; set;}
        public bool mostrar_meanNFE_meanFX_sdFX {get; set;}
        public bool mostrar_melhores_fx_cada_execucao {get; set;}
        public bool mostrar_header {get; set;}
        public bool mostrar_mean_TAU_iteracoes {get; set;}
        public bool mostrar_mean_STDPORC_iteracoes {get; set;}
        public bool mostrar_mean_Mfx_iteracoes {get; set;}
    }

    
    public class ParametrosLivreProblema{
        
        public double GEO__tau {get; set;}
        public double GEOvar__tau {get; set;}

        public double GEOvar2__tau {get; set;}
        
        // public double tau_GEOreal1 {get; set;}      // GEOreal1_igor, GEOreal1_porcentagem, GEOreal1_normal
        // public double std_GEOreal1 {get; set;}      // GEOreal1_igor, GEOreal1_normal
        // public double p1_GEOreal1 {get; set;}       // GEOreal1_porcentagem
        // public double tau_GEOreal2 {get; set;}      // GEOreal2_igor, GEOreal2_porcentagem, GEOreal2_normal
        // public double std_AGEO1real1 {get; set;}    // AGEOs real1
        // public double std_AGEO2real1 {get; set;}    // AGEOs real1
        // public double std_GEOreal2 {get; set;}      // GEOreal2_igor, GEOreal2_normal
        // public double std_AGEO1real2 {get; set;}    // AGEOs real2
        // public double std_AGEO2real2 {get; set;}    // AGEOs real2
        // public double p1_AGEO1real1 {get; set;}     // AGEOs_real1 porcentagem
        // public double p1_AGEO2real1 {get; set;}     // AGEOs_real1 porcentagem
        // public double P_GEOreal2 {get; set;}        // GEOreal2_igor, GEOreal2_porcentagem, GEOreal2_normal
        // public double P_AGEO1real2 {get; set;}      // AGEOs_real2
        // public double P_AGEO2real2 {get; set;}      // AGEOs_real2
        // public double s_GEOreal2 {get; set;}        // GEOreal2_igor, GEOreal2_porcentagem, GEOreal2_normal
        // public double s_AGEO1real2 {get; set;}      // AGEOs_real2
        // public double s_AGEO2real2 {get; set;}      // AGEOs_real2
        
        // GEOreal1_O
        public double GEOreal1_O__tau {get; set;}
        public double GEOreal1_O__std {get; set;}
        // GEOreal1_P
        public double GEOreal1_P__tau {get; set;}
        public double GEOreal1_P__porc {get; set;}
        // GEOreal1_N
        public double GEOreal1_N__tau {get; set;}
        public double GEOreal1_N__std {get; set;}

        // GEOreal2_O_VO
        public double GEOreal2_O_VO__tau {get; set;}
        public double GEOreal2_O_VO__std {get; set;}
        public double GEOreal2_O_VO__P {get; set;}
        public double GEOreal2_O_VO__s {get; set;}
        // GEOreal2_P_VO
        public double GEOreal2_P_VO__tau {get; set;}
        public double GEOreal2_P_VO__porc {get; set;}
        public double GEOreal2_P_VO__P {get; set;}
        public double GEOreal2_P_VO__s {get; set;}
        // GEOreal2_N_VO
        public double GEOreal2_N_VO__tau {get; set;}
        public double GEOreal2_N_VO__std {get; set;}
        public double GEOreal2_N_VO__P {get; set;}
        public double GEOreal2_N_VO__s {get; set;}
        // GEOreal2_O_DS
        public double GEOreal2_O_DS__tau {get; set;}
        public double GEOreal2_O_DS__std {get; set;}
        public double GEOreal2_O_DS__P {get; set;}
        public double GEOreal2_O_DS__s {get; set;}
        // GEOreal2_P_DS
        public double GEOreal2_P_DS__tau {get; set;}
        public double GEOreal2_P_DS__porc {get; set;}
        public double GEOreal2_P_DS__P {get; set;}
        public double GEOreal2_P_DS__s {get; set;}
        // GEOreal2_N_DS
        public double GEOreal2_N_DS__tau {get; set;}
        public double GEOreal2_N_DS__std {get; set;}
        public double GEOreal2_N_DS__P {get; set;}
        public double GEOreal2_N_DS__s {get; set;}

        // GEOreal2_P_VO_UNI
        public double GEOreal2_P_VO_UNI__tau {get; set;}
        public double GEOreal2_P_VO_UNI__porc {get; set;}
        public double GEOreal2_P_VO_UNI__P {get; set;}
        public double GEOreal2_P_VO_UNI__s {get; set;}
        // GEOreal2_N_VO_UNI
        public double GEOreal2_N_VO_UNI__tau {get; set;}
        public double GEOreal2_N_VO_UNI__std {get; set;}
        public double GEOreal2_N_VO_UNI__P {get; set;}
        public double GEOreal2_N_VO_UNI__s {get; set;}
        // GEOreal2_P_DS_UNI
        public double GEOreal2_P_DS_UNI__tau {get; set;}
        public double GEOreal2_P_DS_UNI__porc {get; set;}
        public double GEOreal2_P_DS_UNI__P {get; set;}
        public double GEOreal2_P_DS_UNI__s {get; set;}
        // GEOreal2_N_DS_UNI
        public double GEOreal2_N_DS_UNI__tau {get; set;}
        public double GEOreal2_N_DS_UNI__std {get; set;}
        public double GEOreal2_N_DS_UNI__P {get; set;}
        public double GEOreal2_N_DS_UNI__s {get; set;}

        // A-GEOreal
        public double AGEO2real1_P__porc {get; set;}
        public double AGEO2real2_P_DS__porc {get; set;}
        public double AGEO2real2_P_DS__P {get; set;}
        public double AGEO2real2_P_DS__s {get; set;}
    }
}