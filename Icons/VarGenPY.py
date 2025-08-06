import os

declaracoes = []
atribuicoes = []

for arquivo in os.listdir():
    nome, ext = os.path.splitext(arquivo)
    if ext.lower() in [".png", ".jpg", ".jpeg"]:  # filtra imagens
        nome_var = nome.replace(" ", "_")
        declaracoes.append(f"public static Sprite {nome_var};")
        atribuicoes.append(f'{nome_var} = _assetBundle.LoadAsset<Sprite>("{nome}");')

print("// --- Declarações ---")
print("\n".join(declaracoes))
print("// --- Atribuições ---")
print("\n".join(atribuicoes))

input("\nPressione Enter para sair...")
