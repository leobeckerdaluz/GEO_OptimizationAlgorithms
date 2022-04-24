using System.Collections.Generic;

namespace CheckFeasibility
{
    public class CheckFeasibility
    {
        public static bool check_feasibility(List<double> fenotipos, List<double> upper_bounds, List<double>lower_bounds){
            for (int i=0; i<fenotipos.Count; i++){
                if ((fenotipos[i] < lower_bounds[i]) || (fenotipos[i] > upper_bounds[i])){
                    return false;
                }
            }
            return true;
        }
    }
}