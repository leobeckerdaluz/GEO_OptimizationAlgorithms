

import pandas as pd

FOLDER_NAME = './folder3/'

FILE1 = 'AGEO2.xlsx'
# FILE1 = 'AGEO2var_5.xlsx'
# FILE1 = 'AGEO2real2_P_DS_AA3.xlsx'

# FILE2 = 'AGEO2.xlsx'
FILE2 = 'AGEO2var_5.xlsx'
# FILE2 = 'AGEO2real2_P_DS_AA3.xlsx'

FILE_OUTPUT = "saida.xlsx"

FILEPATH1 = FOLDER_NAME + FILE1
FILEPATH2 = FOLDER_NAME + FILE2
OUTPUT_FILEPATH = FOLDER_NAME + FILE_OUTPUT

file1 = pd.read_excel(FILEPATH1)
file2 = pd.read_excel(FILEPATH2)

df_geral = pd.DataFrame()
df_geral['No1']     = file1['No']
df_geral['Best1']   = file1['Best']
df_geral['Worst1']  = file1['Worst']
df_geral['Median1'] = file1['Median']
df_geral['Mean1']   = file1['Mean']
df_geral['Std1']    = file1['Std']
df_geral['No2']     = file2['No']
df_geral['Best2']   = file2['Best']
df_geral['Worst2']  = file2['Worst']
df_geral['Median2'] = file2['Median']
df_geral['Mean2']   = file2['Mean']
df_geral['Std2']    = file2['Std']













NOME_SHEET = 'CEC2017'

writer = pd.ExcelWriter(OUTPUT_FILEPATH, engine='xlsxwriter')
print("Escrevendo xlsx...")
df_geral.to_excel(writer, index=False, sheet_name=NOME_SHEET)
print("xlsx escrito!")
workbook  = writer.book
worksheet = writer.sheets[NOME_SHEET]




CENTER_FORMAT = workbook.add_format({'align':'center'})
CENTERBOLD_FORMAT = workbook.add_format({'bold':True, 'align':'center'})
CENTER_NUMBER_FORMAT = workbook.add_format({'num_format':'0.00E+00', 'align':'center'})
CENTERBOLD_NUMBER_FORMAT = workbook.add_format({'num_format':'0.00E+00', 'bold':True, 'align':'center'})

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

df_length = len(df_geral['Median2'])
for i in range(0, df_length):
    
    melhorFX1_str = df_geral['Best1'][i]
    melhorFX2_str = df_geral['Best2'][i]
    if (float(melhorFX1_str) < float(melhorFX2_str)):
        worksheet.write(i, COLUNA_MELHORFX1, melhorFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(melhorFX2_str) < float(melhorFX1_str)):
        worksheet.write(i, COLUNA_MELHORFX2, melhorFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("IGUALLL melhorFX na função {}".format(df_geral['No1'][i]))

    piorFX1_str = df_geral['Worst1'][i]
    piorFX2_str = df_geral['Worst2'][i]
    if (float(piorFX1_str) < float(piorFX2_str)):
        worksheet.write(i, COLUNA_PIORFX1, piorFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(piorFX2_str) < float(piorFX1_str)):
        worksheet.write(i, COLUNA_PIORFX2, piorFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("IGUALLL piorFX na função {}".format(df_geral['No1'][i]))

    medianFX1_str = df_geral['Median1'][i]
    medianFX2_str = df_geral['Median2'][i]
    if (float(medianFX1_str) < float(medianFX2_str)):
        worksheet.write(i, COLUNA_MEDIANFX1, medianFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(medianFX2_str) < float(medianFX1_str)):
        worksheet.write(i, COLUNA_MEDIANFX2, medianFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("IGUALLL medianFX na função {}".format(df_geral['No1'][i]))

    meanFX1_str = df_geral['Mean1'][i]
    meanFX2_str = df_geral['Mean2'][i]
    if (float(meanFX1_str) < float(meanFX2_str)):
        worksheet.write(i, COLUNA_MEANFX1, meanFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(meanFX2_str) < float(meanFX1_str)):
        worksheet.write(i, COLUNA_MEANFX2, meanFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:   
        print("IGUALLL meanFX na função {}".format(df_geral['No1'][i]))

    sdFX1_str = df_geral['Std1'][i]
    sdFX2_str = df_geral['Std2'][i]
    if (float(sdFX1_str) < float(sdFX2_str)):
        worksheet.write(i, COLUNA_SDFX1, sdFX1_str, CENTERBOLD_NUMBER_FORMAT)
    elif (float(sdFX2_str) < float(sdFX1_str)):
        worksheet.write(i, COLUNA_SDFX2, sdFX2_str, CENTERBOLD_NUMBER_FORMAT)
    else:
        print("IGUALLL sdFX na função {}".format(df_geral['No1'][i]))


writer.save()