<h2 align="center">
  STUDY TO INCREASE THE PERFORMANCE OF THE ADAPTIVE VERSION OF THE GEO ALGORITHM AND ITS APPLICATION IN THE CONCEPTUAL PROJECT OF SPACE SYSTEMS
</h2>

<h4 align="center"><a href="http://inpe.br/">National Institute for Space Research - INPE</a></h4>
<h4 align="center"><a href="http://www.inpe.br/posgraduacao/ete/">Engineering and Space Technology - CSE/ETE</a></h4>

This repository contains the algorithms developed during a master's thesis. The main focus of the work was to improve the evolutionary algorithm A-GEO, which was presented in the paper [A new Adaptive Evolutionary Algorithm for Design Optimization (Barroca, 2019)](http://mtc-m21c.sid.inpe.br/col/sid.inpe.br/mtc-m21c/2019/05.15.23.22/doc/publicacao.pdf).




<p align="center">
<a href="https://www.repostatus.org/#active"><img src="https://www.repostatus.org/badges/latest/active.svg" alt="Project Status: Active."></a>
<a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/license-MIT-green" alt="License"></a>
<a href="https://www.tidyverse.org/lifecycle/#maturing"><img src="https://lifecycle.r-lib.org/articles/figures/lifecycle-experimental.svg" alt="lifecycle"></a>
<br>
</p>

<p align="center">  
  • <a href="#objectives">Objectives</a> &nbsp;
  • <a href="#algorithms">Algorithms</a> &nbsp;
</p>

&nbsp;


## Objectives
- Implement the parameter control mechanism of A-GEO in GEOvar, a variant of GEO, verifying this implementation using a set of test functions;
- Change the encoding of the A-GEO design variables from binary to real, no longer requiring the definition of the number of bits encoding each variable;
- Study how the design variables are perturbed in the real encoding, as well as how to perform several mutations on each variable in a single iteration;
- Investigate the control mechanism for the parameter τ proposed for A-GEO, which may indicate other ways to control the parameter during search;
- Explore ways to make A-GEOreal fully adaptive, without the need for parameter tuning;
- Apply the improved algorithm in the search for automating the creation of solutions in a conceptual design of space systems;


&nbsp;
## Algorithms
### Binary Encoding:
```r
- GEO
- GEOvar
- A-GEO
- A-GEOvar
- A-GEO2var_5
```
### Real Encoding:
```r
- GEOreal1 (GEOreal1_M, GEOreal1_P, GEOreal1_A)
- GEOreal2 (GEOreal2_M_VO, GEOreal2_P_VO, GEOreal2_A_VO, GEOreal2_M_DS, GEOreal2_P_DS, GEOreal2_A_DS)
- GEOreal2_P_DS_UNI
- A-GEO2real1_AA
- A-GEO2real2_AA0
- A-GEO2real2_AA1
- A-GEO2real2_AA2
- A-GEO2real2_AA3
```
