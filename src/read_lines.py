filename = "tuningGEOreal2_schwefel_completo_50ex.txt"

with open(filename) as f:
    content = f.readlines()

valores = []
for line in content:
    # print(line)
    if ("meanFX;0" in line):
        s = line.split(";")
        valor = float( s[1].replace(",",".") )
        valores.append(valor)

print(valores)

print( min(valores) )