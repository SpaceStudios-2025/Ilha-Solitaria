using System.Collections.Generic;

public interface ICrafting
{
    public void SelecionarFabricavel(ItemFabricavel item);    
    public bool GenerateItem(ref ItemInventory item,int qtd);

    public List<ReceitaItem> Receitas();
}
