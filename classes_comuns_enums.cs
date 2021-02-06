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
        public int NFOB { get; set; }
        public double melhor_fx { get; set; }
        public List<double> melhores_NFOBs { get; set; }
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
        public bool rodar_GEOreal2 { get; set; }
        public bool rodar_AGEO1real1 { get; set; }
        public bool rodar_AGEO2real1 { get; set; }
        public bool rodar_AGEO1real2 { get; set; }
        public bool rodar_AGEO2real2 { get; set; }
    }


    public class ParametrosDaFuncao {
        public int definicao_funcao_objetivo { get; set; }
        public int n_variaveis_projeto { get; set; }
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
        public bool mostrar_media_NFE_atingido {get; set;}
        public bool mostrar_media_melhor_fx {get; set;}
    }

    enum EnumNomesFuncoesObjetivo{
        enum_griwangk,
        enum_rosenbrock,
        enum_dejong3,
        enum_spacecraft,
        enum_rastringin,
        enum_schwefel,
        enum_ackley,
        enum_F09,
        enum_F10
    }

    enum EnumNomesAlgoritmos{
        GEO_can,
        GEO_var,
        AGEO1,
        AGEO2,
        AGEO1var,
        AGEO2var,
        GEOreal1,
        GEOreal2,
        AGEO1real1,
        AGEO2real1,
        AGEO1real2,
        AGEO2real2,
    }

    enum EnumTipoCriterioParada{
        parada_por_NFOB,
        parada_por_PRECISAO,
        parada_por_PRECISAOouNFOB
    }
}