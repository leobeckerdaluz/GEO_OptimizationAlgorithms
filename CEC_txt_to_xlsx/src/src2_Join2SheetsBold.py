

import pandas as pd


N = 'D10'
# N = 'D30'

INPUTS_FOLDER_NAME = f'../OUTPUTS/{N}/'
PREINPUT = f'CEC2017functions-{N}__'

# INPUT_FILE1 = 'AGEO2'
INPUT_FILE1 = 'AGEO2var_5'

# INPUT_FILE2 = 'AGEO2'
# INPUT_FILE2 = 'AGEO2var_5'
INPUT_FILE2 = 'AGEO2real2_AA3'

FILENAME_OUTPUT = INPUT_FILE1+"_vs_"+INPUT_FILE2

INPUT_FILENAME1 = INPUTS_FOLDER_NAME + PREINPUT+INPUT_FILE1 + ".xlsx"
INPUT_FILENAME2 = INPUTS_FOLDER_NAME + PREINPUT+INPUT_FILE2 + ".xlsx"
OUTPUT_FILEPATH = INPUTS_FOLDER_NAME + PREINPUT+FILENAME_OUTPUT + ".xlsx"

file1 = pd.read_excel(INPUT_FILENAME1)
file2 = pd.read_excel(INPUT_FILENAME2)

df_geral = pd.DataFrame({
    'No1': file1['No'],
    'Best1': file1['Best'],
    'Worst1': file1['Worst'],
    'Median1': file1['Median'],
    'Mean1': file1['Mean'],
    'Std1': file1['Std'],
    
    'No2': file2['No'],
    'Best2': file2['Best'],
    'Worst2': file2['Worst'],
    'Median2': file2['Median'],
    'Mean2': file2['Mean'],
    'Std2': file2['Std']
})













NOME_SHEET = 'CEC2017'

writer = pd.ExcelWriter(OUTPUT_FILEPATH, engine='xlsxwriter')
print("Escrevendo xlsx...")
df_geral.to_excel(writer, index=False, sheet_name=NOME_SHEET)
print("xlsx escrito!")
workbook  = writer.book
worksheet = writer.sheets[NOME_SHEET]



# Formats
CENTER_FORMAT =             workbook.add_format({'align':'center'})
CENTER_NUMBER_FORMAT =      workbook.add_format({'align':'center', 'num_format':'0.00E+00'})
CENTERBOLD_FORMAT =         workbook.add_format({'align':'center', 'bold':True, })
CENTERBOLD_NUMBER_FORMAT =  workbook.add_format({'align':'center', 'bold':True, 'num_format':'0.00E+00'})

# Seta os formatos das colunas. Centraliza só os ids e formata e centraliza os valores
worksheet.set_column('A:A', None, CENTER_FORMAT)
worksheet.set_column('B:F', None, CENTER_NUMBER_FORMAT)
worksheet.set_column('G:G', None, CENTER_FORMAT)
worksheet.set_column('H:L', None, CENTER_NUMBER_FORMAT)




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

START_ROW = 1

# Tamanho do dataframe
df_length = df_geral.shape[0]
# Para cada linha, verifica o menor e formata em negrito
for i in range(0, df_length):
    funcao_id       = df_geral['No1'][i]
    melhorFX1_str   = df_geral['Best1'][i]
    melhorFX2_str   = df_geral['Best2'][i]
    piorFX1_str     = df_geral['Worst1'][i]
    piorFX2_str     = df_geral['Worst2'][i]
    medianFX1_str   = df_geral['Median1'][i]
    medianFX2_str   = df_geral['Median2'][i]
    meanFX1_str     = df_geral['Mean1'][i]
    meanFX2_str     = df_geral['Mean2'][i]
    sdFX1_str       = df_geral['Std1'][i]
    sdFX2_str       = df_geral['Std2'][i]
    
    # Verifica o menor melhorFX
    if (float(melhorFX1_str) < float(melhorFX2_str)):
        worksheet.write(i+START_ROW, COLUNA_MELHORFX1, melhorFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(melhorFX2_str) < float(melhorFX1_str)):
        worksheet.write(i+START_ROW, COLUNA_MELHORFX2, melhorFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("WARNING ---> Igual melhorFX na função {} !".format(funcao_id))

    # Verifica o menor piorFX
    if (float(piorFX1_str) < float(piorFX2_str)):
        worksheet.write(i+START_ROW, COLUNA_PIORFX1, piorFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(piorFX2_str) < float(piorFX1_str)):
        worksheet.write(i+START_ROW, COLUNA_PIORFX2, piorFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("WARNING ---> Igual piorFX na função {} !".format(funcao_id))

    # Verifica o menor medianFX
    if (float(medianFX1_str) < float(medianFX2_str)):
        worksheet.write(i+START_ROW, COLUNA_MEDIANFX1, medianFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(medianFX2_str) < float(medianFX1_str)):
        worksheet.write(i+START_ROW, COLUNA_MEDIANFX2, medianFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("WARNING ---> Igual medianFX na função {} !".format(funcao_id))

    # Verifica o menor meanFX
    if (float(meanFX1_str) < float(meanFX2_str)):
        worksheet.write(i+START_ROW, COLUNA_MEANFX1, meanFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(meanFX2_str) < float(meanFX1_str)):
        worksheet.write(i+START_ROW, COLUNA_MEANFX2, meanFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:   
        print("WARNING ---> Igual meanFX na função {} !".format(funcao_id))

    # Verifica o menor sdFX
    if (float(sdFX1_str) < float(sdFX2_str)):
        worksheet.write(i+START_ROW, COLUNA_SDFX1, sdFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(sdFX2_str) < float(sdFX1_str)):
        worksheet.write(i+START_ROW, COLUNA_SDFX2, sdFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("WARNING ---> Igual sdFX na função {} !".format(funcao_id))


# Salva o xlsx
writer.save()