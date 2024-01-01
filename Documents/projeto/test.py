import random


comida = input("digite o que deseja que escolhemos \n")

if comida == "janta":
    sort = random.randint(1, 10)
    if sort == 1:
        print("Hamburguer")
    if sort == 2:
        print("Pizza")
    if sort == 3:
        print("Coxinha")
    if sort == 4:
        print("Pastel")
    if sort == 5:
        print("Crepe")
    if sort == 6:
        print("Parmegiana")
    if sort == 7:
        print("Salada")
    if sort == 8:
        print("Macarrao")
    if sort == 9:
        print("Ovo")
    if sort == 10:
        print("Pipoca")
    