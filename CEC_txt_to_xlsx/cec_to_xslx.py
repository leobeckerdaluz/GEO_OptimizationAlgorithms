



from posixpath import split


lines = []
with open('novo__CEC1to30_50runs.txt') as f:
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
df = pd.DataFrame(data = dataframe)


# Nomes algoritmos concatenados
nome_sheet = nomes_algoritmos[0] + ";" + nomes_algoritmos[1]





# Create a Pandas Excel writer using XlsxWriter as the engine.
writer = pd.ExcelWriter("pandas_column_formats.xlsx", engine='xlsxwriter')

print("Escrevendo xlsx...")
df.to_excel(writer, index=False, startrow=0, sheet_name=nome_sheet)
print("xlsx escrito!")

workbook  = writer.book
worksheet = writer.sheets[nome_sheet]



BOLD_FORMAT = workbook.add_format({'bold': True}) # 'valign': 'top'

format1 = workbook.add_format({'num_format': '#,##0.00'})
format2 = workbook.add_format({'num_format': '0%'})



COLUNA_MELHORFX1 = 1
COLUNA_MELHORFX2 = 7
for i in range(0,len(lista_numeros_funcoes)):
    
    melhorFX1_str = melhorFX_algoritmo1[i]
    melhorFX2_str = melhorFX_algoritmo2[i]
    if (float(melhorFX1_str) < float(melhorFX2_str)):
        worksheet.write(i+1, COLUNA_MELHORFX1, melhorFX1_str, BOLD_FORMAT)
    elif (float(melhorFX2_str) < float(melhorFX1_str)):
        worksheet.write(i+1, COLUNA_MELHORFX2, melhorFX2_str, BOLD_FORMAT)
    else:
        print("IGUALLL!")



# # Set the column width and format.
# worksheet.set_column(1, 1, 18, format1)
# # Set the format but not the column width.
# worksheet.set_column(2, 2, None, format2)

writer.save()