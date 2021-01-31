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
        public int step_para_obter_NFOBs { get; set; }
        public int NFOB_criterio_parada { get; set; }
        public double PRECISAO_criterio_parada { get; set; }
        public double fx_esperado { get; set; }
    }
    // 'parada_por_NFOB_atingido';
    // 'parada_por_Precisao_atingida';
    // 'parada_por_Precisao_E_NFOB_atingidos';


    public class QuaisAlgoritmosRodar {
        public bool rodar_GEO { get; set; }
        public bool rodar_GEOvar { get; set; }
        public bool rodar_AGEO1 { get; set; }
        public bool rodar_AGEO2 { get; set; }
        public bool rodar_AGEO1var { get; set; }
        public bool rodar_AGEO2var { get; set; }
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
}