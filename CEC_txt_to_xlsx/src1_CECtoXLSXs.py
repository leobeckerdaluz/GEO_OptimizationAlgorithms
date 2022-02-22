



from posixpath import split
from statistics import mean



# TXT_FILEPATH = 'novo__CEC1to30_50runs.txt'
TXT_FILEPATH = 'CEC_teste_3algoritmos.txt'
XLSX_OUT_FILEPATH = "saida.xlsx"


# Abre o txt
lines = []
with open(TXT_FILEPATH) as f:
    lines = f.readlines()



# Joga todos os valores de cada métrica em uma lista geral
ListaGeral_nomes_algoritmos = []
jah_viu_algoritmos = False
ListaGeral_melhorFX = []
ListaGeral_piorFX = []
ListaGeral_medianFX = []
ListaGeral_meanFX = []
ListaGeral_sdFX = []

for line in lines:
    line = line.replace(',', '.')
    # print(line)
    spl = line.split(';')

    # os comandos pop(0) e pop(-1) servem pra remover a primeira label e o \n do fim da linha
    
    if (("parameter" in line) and not(jah_viu_algoritmos)):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_nomes_algoritmos.append(value)
        jah_viu_algoritmos = True

    if ("melhorFX" in line):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_melhorFX.append(value)

    if ("piorFX" in line):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_piorFX.append(value)

    if ("medianFX" in line):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_medianFX.append(value)

    if ("meanFX" in line):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_meanFX.append(value)

    if ("sdFX" in line):
        spl.pop(0)
        spl.pop(-1)
        for value in spl:
            ListaGeral_sdFX.append(value)



# Essas listas aqui abaixo terão sempre QUANTIDADE_ALGORITMOS listas e cada listinha terá 30 valores
# Inicialmente, cria as listinhas vazias
QUANTIDADE_ALGORITMOS = 3

melhorFX_algoritmos = []
piorFX_algoritmos = []
medianFX_algoritmos = []
meanFX_algoritmos = []
sdFX_algoritmos = []
for i in range(0,QUANTIDADE_ALGORITMOS):
    melhorFX_algoritmos.append([])
    piorFX_algoritmos.append([])
    medianFX_algoritmos.append([])
    meanFX_algoritmos.append([])
    sdFX_algoritmos.append([])




# Quebra as listas com 29*QUANTIDADE_ALGORITMOS valores em QUANTIDADE_ALGORITMOS listinhas dentro da lista de listinhas
count = 0
while(count < len(ListaGeral_melhorFX)):
    # Para a quantidade de algoritmos
    for j in range(0,QUANTIDADE_ALGORITMOS):
        (melhorFX_algoritmos[j]).append(ListaGeral_melhorFX[count])
        (piorFX_algoritmos[j]).append(ListaGeral_piorFX[count])
        (medianFX_algoritmos[j]).append(ListaGeral_medianFX[count])
        (meanFX_algoritmos[j]).append(ListaGeral_meanFX[count])
        (sdFX_algoritmos[j]).append(ListaGeral_sdFX[count])
        count += 1
    


# Cria a lista de ids das 29 funções --> lista de 1 a 30 sem a 2.
lista_ids_funcoes = []
lista_ids_funcoes = [i for i in range(1,31)]
lista_ids_funcoes.remove(2)









# Monta um dataframe pra cada algoritmo e salva em xlsx
FOLDER_NAME = './folder3/'
# Cria a pasta se ela não existe
import os
if (not(os.path.exists(FOLDER_NAME))):
    os.makedirs(FOLDER_NAME)

import pandas as pd
for i in range(0,QUANTIDADE_ALGORITMOS):
    df = pd.DataFrame()

    df['No'] = lista_ids_funcoes
    df['Best'] = melhorFX_algoritmos[i]
    df['Worst'] = piorFX_algoritmos[i]
    df['Median'] = medianFX_algoritmos[i]
    df['Mean'] = meanFX_algoritmos[i]
    df['Std'] = sdFX_algoritmos[i]

    nome_algoritmo = ListaGeral_nomes_algoritmos[i]
    filepath = FOLDER_NAME + nome_algoritmo + '.xlsx'
    # df.to_excel(filepath, index=False, sheet_name=nome_algoritmo)
    df.to_excel(filepath, index=False)