using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Classes_e_Enums
{
    enum EnumNomesFuncoesObjetivo{
        griewangk,
        rosenbrock,
        dejong3,
        spacecraft,
        rastringin,
        schwefel,
        ackley,
        F09tese,
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

        CEC2017_01,
        CEC2017_02,
        CEC2017_03,
        CEC2017_04,
        CEC2017_05,
        CEC2017_06,
        CEC2017_07,
        CEC2017_08,
        CEC2017_09,
        CEC2017_10,
        CEC2017_11,
        CEC2017_12,
        CEC2017_13,
        CEC2017_14,
        CEC2017_15,
        CEC2017_16,
        CEC2017_17,
        CEC2017_18,
        CEC2017_19,
        CEC2017_20,
        CEC2017_21,
        CEC2017_22,
        CEC2017_23,
        CEC2017_24,
        CEC2017_25,
        CEC2017_26,
        CEC2017_27,
        CEC2017_28,
        CEC2017_29,
        CEC2017_30
    }


    enum EnumOQueFazer{
        executar_algoritmos,
        tuning_GEO,
        tuning_GEOvar,
        tuning_GEOvar2,
        tuning_GEOreal1_O,
        tuning_GEOreal1_P,
        tuning_GEOreal1_N,
        tuning_GEOreal2_O_VO,
        tuning_GEOreal2_P_VO,
        tuning_GEOreal2_N_VO,
        tuning_GEOreal2_O_DS,
        tuning_GEOreal2_P_DS,
        tuning_GEOreal2_N_DS,

        tuning_AGEO2real1_P,
    }


    enum EnumNomesAlgoritmos{
        // Binários
        GEO_can,
        GEO_var,
        AGEO1,
        AGEO2,
        AGEO3,
        AGEO4,
        AGEO9,
        AGEO1var,
        AGEO2var,
        AGEO3var,
        AGEO4var,
        AGEO9var,

        AGEO1var_1,
        AGEO1var_3,
        AGEO1var_5,
        AGEO1var_7,
        AGEO1var_9,
        AGEO1var_11,
        AGEO2var_1,
        AGEO2var_3,
        AGEO2var_5,
        AGEO2var_7,
        AGEO2var_9,
        AGEO2var_11,
        
        GEO_var2,

        // GEOreal1
        GEOreal1_O,
        GEOreal1_P,
        GEOreal1_N,
        
        // GEOreal2
        GEOreal2_O_VO,
        GEOreal2_P_VO,
        GEOreal2_N_VO,
        GEOreal2_O_DS,
        GEOreal2_P_DS,
        GEOreal2_N_DS,
        
        // GEOreal2 + 1 Uniforme
        GEOreal2_P_VO_UNI,
        GEOreal2_N_VO_UNI,
        GEOreal2_P_DS_UNI,
        GEOreal2_N_DS_UNI,

        // A-GEOreal
        AGEOreal1_P,
        AGEOreal2_P_DS,
        AGEO2real1_P_AA,
        AGEO2real2_P_DS_fixo,
        AGEO2real2_P_AA_p0,
        AGEO2real2_P_AA_p1,
        AGEO2real2_P_AA_p2,
        AGEO2real2_P_AA_p3,
        AGEO2real2_P_AA_p9,
    }


    enum EnumTipoCriterioParada{
        parada_por_NFE,
        // parada_por_PRECISAO,
        parada_por_PRECISAOouNFE,
        parada_por_ITERATIONS,
        parada_SPACECRAFT
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