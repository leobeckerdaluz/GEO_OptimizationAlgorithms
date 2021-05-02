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
        public int NFOB { get; set; }
        public double melhor_fx { get; set; }
        public List<double> melhores_NFOBs { get; set; }
        public List<double> populacao_final { get; set; }
    }

    public class Retorno_N_Execucoes_GEOs {
        public int algoritmo_utilizado { get; set; }
        public int NFOB_medio { get; set; }
        public double media_melhor_fx { get; set; }
        public double SD_do_melhor_fx { get; set; }
        public List<double> media_valor_FO_em_cada_NFOB { get; set; }
        public List<double> lista_melhores_fxs { get; set; }
        public List<List<double>> lista_populacao_final { get; set; }
    }

    public class ParametrosCriterioParada {
        public int tipo_criterio_parada { get; set; }
        public int NFOB_criterio_parada { get; set; }
        
        public int step_para_obter_NFOBs { get; set; }
        
        public double PRECISAO_criterio_parada { get; set; }
        public double fx_esperado { get; set; }
    }
    // 'parada_por_NFOB_atingido';
    // 'parada_por_Precisao_atingida';
    // 'parada_por_Precisao_ou_NFOB_atingidos';


    public class QuaisAlgoritmosRodar {
        public bool rodar_GEO { get; set; }
        public bool rodar_GEOvar { get; set; }
        public bool rodar_AGEO1 { get; set; }
        public bool rodar_AGEO2 { get; set; }
        public bool rodar_AGEO1var { get; set; }
        public bool rodar_AGEO2var { get; set; }
        public bool rodar_GEOreal1 { get; set; }
        public bool rodar_AGEO1real1 { get; set; }
        public bool rodar_AGEO2real1 { get; set; }
        public bool rodar_GEOreal2 { get; set; }
        public bool rodar_AGEO1real2 { get; set; }
        public bool rodar_AGEO2real2 { get; set; }
        public bool rodar_GEOreal3 { get; set; }
        public bool rodar_AGEO1real3 { get; set; }
        public bool rodar_AGEO2real3 { get; set; }
    }


    public class ParametrosProblema {
        public int definicao_funcao_objetivo { get; set; }
        public string nome_funcao { get; set; }
        public int n_variaveis_projeto { get; set; }
        public List<int> bits_por_variavel { get; set; }
        public List<RestricoesLaterais> restricoes_laterais_por_variavel { get; set; }
        public ParametrosLivreProblema parametros_livres { get; set; }
        public List<double> populacao_inicial { get; set; }
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
        public bool mostrar_melhores_NFOB {get; set;}
        public bool mostrar_meanNFE_meanFX_sdFX {get; set;}
        public bool mostrar_melhores_fx_cada_execucao {get; set;}
        public bool mostrar_header {get; set;}
    }

    public class ParametrosLivreProblema{
        public double tau_GEO {get; set;}
        public double tau_GEOvar {get; set;}
        public double tau_GEOreal1 {get; set;}
        public double tau_GEOreal2 {get; set;}
        public double tau_GEOreal3 {get; set;}
        public double tau_minimo_AGEOs {get; set;}
        public double std_GEOreal1 {get; set;}
        public double std_AGEO1real1 {get; set;}
        public double std_AGEO2real1 {get; set;}
        public double std_GEOreal2 {get; set;}
        public double std_AGEO1real2 {get; set;}
        public double std_AGEO2real2 {get; set;}
        public double std_GEOreal3 {get; set;}
        public double std_AGEO1real3 {get; set;}
        public double std_AGEO2real3 {get; set;}
        public double P_GEOreal2 {get; set;}
        public double P_AGEO1real2 {get; set;}
        public double P_AGEO2real2 {get; set;}
        public double P_GEOreal3 {get; set;}
        public double P_AGEO1real3 {get; set;}
        public double P_AGEO2real3 {get; set;}
        public double s_GEOreal2 {get; set;}
        public double s_AGEO1real2 {get; set;}
        public double s_AGEO2real2 {get; set;}
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
        F10
    }

    enum EnumNomesAlgoritmos{
        GEO_can,
        GEO_var,
        AGEO1,
        AGEO2,
        AGEO1var,
        AGEO2var,
        GEOreal1,
        AGEO1real1,
        AGEO2real1,
        GEOreal2,
        AGEO1real2,
        AGEO2real2,
        GEOreal3,
        AGEO1real3,
        AGEO2real3,
    }

    enum EnumTipoCriterioParada{
        parada_por_NFOB,
        parada_por_PRECISAO,
        parada_por_PRECISAOouNFOB
    }

    enum EnumTipoPerturbacao{
        perturbacao_original,
        perturbacao_SDdireto
    }
}