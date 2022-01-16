// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Classes_Comuns_Enums;

// namespace GEOs_REAIS
// {
//     public class AGEO2real1 : GEO_real1
//     {
//         public int tipo_AGEO {get; set;}
//         public double CoI_1 {get; set;}

//         public AGEO2real1(
//             List<double> populacao_inicial,
//             int n_variaveis_projeto,
//             int definicao_funcao_objetivo,
//             List<double> lower_bounds,
//             List<double> upper_bounds,
//             List<int> lista_NFEs_desejados,
//             double std,
//             int tipo_perturbacao) : base(
//                 n_variaveis_projeto,
//                 definicao_funcao_objetivo,
//                 populacao_inicial,
//                 lower_bounds,
//                 upper_bounds,
//                 lista_NFEs_desejados,
//                 tipo_perturbacao,
//                 0.5,
//                 std)
//         {
//             this.std = 0;
            
//             this.tipo_AGEO = 2;
//             this.CoI_1 = (double) 1.0 / Math.Sqrt(n_variaveis_projeto);
//         }


//         public override void mutacao_do_tau_AGEOs()
//         {
//             // Obtém o tamanho da população de bits
//             int tamanho_populacao = this.populacao_atual.Count;
            
//             // Instancia as funções úteis do mecanismo A-GEO
//             MecanismoAGEO.MecanismoAGEO mecanismo = new MecanismoAGEO.MecanismoAGEO();
            
//             // Calcula o f(x) de referência
//             double fx_referencia = mecanismo.calcula_fx_referencia(tipo_AGEO, fx_melhor, fx_atual);
            
//             // Calcula o CoI
//             double CoI = mecanismo.calcula_CoI_real(perturbacoes_da_iteracao, fx_referencia, tamanho_populacao);
            
            
            
//             // VERSÃO ORIGINAL DE ATUALIZAR O TAU
//             // Atualiza o tau
//             tau = mecanismo.obtem_novo_tau(this.tipo_AGEO, this.tau, CoI, this.CoI_1, tamanho_populacao);



//             // if (tau < 1.5)
//             //     std = 0.5;
//             // else if (tau < 3)
//             //     std = 2;
//             // else if (tau > 3)
//             //     std = 8;


//             // if (tau < 1)
//             //     std = 10;
//             // else if (tau < 3)
//             //     std = 3;
//             // else if (tau > 3)
//             //     std = 0.5;
            
                
            
//             // std = Math.Pow(2, -tau*2) * 10;
            
            
                
//             // Armazena o CoI atual para ser usado como o anterior na próxima iteração
//             this.CoI_1 = CoI;
//         }
//     }
// }
