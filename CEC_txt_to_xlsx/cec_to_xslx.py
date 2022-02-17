



from posixpath import split



TXT_FILEPATH = 'novo__CEC1to30_50runs.txt'
TXT_FILEPATH = 'teste2.txt'
XLSX_OUT_FILEPATH = "pandas_column_formats.xlsx"


lines = []
with open(TXT_FILEPATH) as f:
    lines = f.readlines()


nomes_algoritmos = []
jah_viu_algoritmos = False
melhorFX = []
piorFX = []
medianFX = []
meanFX = []
sdFX = []

for line in lines:
    line = line.replace(',', '.')
    # print(line)
    spl = line.split(';')
    
    if (("parameter" in line) and not(jah_viu_algoritmos)):
        nomes_algoritmos.append(spl[1])
        nomes_algoritmos.append(spl[2])
        jah_viu_algoritmos = True

    if ("melhorFX" in line):
        # melhorFX.append( float(spl[1]) )
        melhorFX.append(spl[1])
        melhorFX.append(spl[2])

    if ("piorFX" in line):
        piorFX.append(spl[1])
        piorFX.append(spl[2])

    if ("medianFX" in line):
        medianFX.append(spl[1])
        medianFX.append(spl[2])

    if ("meanFX" in line):
        meanFX.append(spl[1])
        meanFX.append(spl[2])

    if ("sdFX" in line):
        sdFX.append(spl[1])
        sdFX.append(spl[2])


# print(nomes_algoritmos)
# print(melhorFX)
# print(piorFX)
# print(medianFX)
# print(meanFX)
# print(sdFX)

melhorFX_algoritmo1 = []
melhorFX_algoritmo2 = []
piorFX_algoritmo1 = []
piorFX_algoritmo2 = []
medianFX_algoritmo1 = []
medianFX_algoritmo2 = []
meanFX_algoritmo1 = []
meanFX_algoritmo2 = []
sdFX_algoritmo1 = []
sdFX_algoritmo2 = []


# Separa as listas para os dois algoritmos
for i in range(0,len(melhorFX)):
    if (i%2 == 0):
        melhorFX_algoritmo1.append(melhorFX[i])
        piorFX_algoritmo1.append(piorFX[i])
        medianFX_algoritmo1.append(medianFX[i])
        meanFX_algoritmo1.append(meanFX[i])
        sdFX_algoritmo1.append(sdFX[i])
    elif (i%2 == 1):
        melhorFX_algoritmo2.append(melhorFX[i])
        piorFX_algoritmo2.append(piorFX[i])
        medianFX_algoritmo2.append(medianFX[i])
        meanFX_algoritmo2.append(meanFX[i])
        sdFX_algoritmo2.append(sdFX[i])


# Cria a lista de ids das funções
lista_numeros_funcoes = []
lista_numeros_funcoes = [i for i in range(0,31)]
lista_numeros_funcoes.remove(0)
lista_numeros_funcoes.remove(2)
# print(lista_numeros_funcoes)
# print(len(lista_numeros_funcoes))







import pandas as pd

dataframe = {
    'No1': lista_numeros_funcoes,
    'No1': lista_numeros_funcoes,
    'Best1': melhorFX_algoritmo1,
    'Worst1': piorFX_algoritmo1,
    'Median1': medianFX_algoritmo1,
    'Mean1': meanFX_algoritmo1,
    'sd1': sdFX_algoritmo1,
    'No2': lista_numeros_funcoes,
    'Best2': melhorFX_algoritmo2,
    'Worst2': piorFX_algoritmo2,
    'Median2': medianFX_algoritmo2,
    'Mean2': meanFX_algoritmo2,
    'sd2': sdFX_algoritmo2,
}
# Cria o dataframe
df = pd.DataFrame(data = dataframe)

# Renomeia as Colunas
df.columns = ['No.','Best','Worst','Median','Mean','StdDev',  'No.','Best','Worst','Median','Mean','StdDev']











NOME_SHEET = 'CEC2017'
START_ROW = 1

# Create a Pandas Excel writer using XlsxWriter as the engine.
writer = pd.ExcelWriter(XLSX_OUT_FILEPATH, engine='xlsxwriter')
print("Escrevendo xlsx...")
df.to_excel(writer, index=False, startrow=START_ROW, sheet_name=NOME_SHEET)
print("xlsx escrito!")
workbook  = writer.book
worksheet = writer.sheets[NOME_SHEET]



CENTER_FORMAT = workbook.add_format({'align':'center'})
BOLD_FORMAT = workbook.add_format({'bold':True, 'align':'center'})





# Centraliza tudo
worksheet.set_column('A:M', None, CENTER_FORMAT)
# Adiciona título
nome_sheet = nomes_algoritmos[0] + "-" + nomes_algoritmos[1]
worksheet.write(0, 0, nome_sheet, BOLD_FORMAT)



COLUNA_MELHORFX1 = 1
COLUNA_PIORFX1 = 2
COLUNA_MEDIANFX1 = 3
COLUNA_MEANFX1 = 4
COLUNA_SDFX1 = 5

COLUNA_MELHORFX2 = 7
COLUNA_PIORFX2 = 8
COLUNA_MEDIANFX2 = 9
COLUNA_MEANFX2 = 10
COLUNA_SDFX2 = 11

for i in range(0,len(lista_numeros_funcoes)):
    
    melhorFX1_str = melhorFX_algoritmo1[i]
    melhorFX2_str = melhorFX_algoritmo2[i]
    if (float(melhorFX1_str) < float(melhorFX2_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MELHORFX1, melhorFX1_str, BOLD_FORMAT)
    elif (float(melhorFX2_str) < float(melhorFX1_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MELHORFX2, melhorFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL melhorFX na linha {0}!", i)

    piorFX1_str = piorFX_algoritmo1[i]
    piorFX2_str = piorFX_algoritmo2[i]
    if (float(piorFX1_str) < float(piorFX2_str)):
        worksheet.write(i+1+START_ROW, COLUNA_PIORFX1, piorFX1_str, BOLD_FORMAT)
    elif (float(piorFX2_str) < float(piorFX1_str)):
        worksheet.write(i+1+START_ROW, COLUNA_PIORFX2, piorFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL piorFX na linha {0}!", i)

    medianFX1_str = medianFX_algoritmo1[i]
    medianFX2_str = medianFX_algoritmo2[i]
    if (float(medianFX1_str) < float(medianFX2_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MEDIANFX1, medianFX1_str, BOLD_FORMAT)
    elif (float(medianFX2_str) < float(medianFX1_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MEDIANFX2, medianFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL medianFX na linha {0}!", i)

    meanFX1_str = meanFX_algoritmo1[i]
    meanFX2_str = meanFX_algoritmo2[i]
    if (float(meanFX1_str) < float(meanFX2_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MEANFX1, meanFX1_str, BOLD_FORMAT)
    elif (float(meanFX2_str) < float(meanFX1_str)):
        worksheet.write(i+1+START_ROW, COLUNA_MEANFX2, meanFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL meanFX na linha {0}!", i)

    sdFX1_str = sdFX_algoritmo1[i]
    sdFX2_str = sdFX_algoritmo2[i]
    if (float(sdFX1_str) < float(sdFX2_str)):
        worksheet.write(i+1+START_ROW, COLUNA_SDFX1, sdFX1_str, BOLD_FORMAT)
    elif (float(sdFX2_str) < float(sdFX1_str)):
        worksheet.write(i+1+START_ROW, COLUNA_SDFX2, sdFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL sdFX na linha {0}!", i)



# # Set the column width and format.
# worksheet.set_column(1, 1, 18, format1)
# # Set the format but not the column width.
# worksheet.set_column(2, 2, None, format2)

writer.save()