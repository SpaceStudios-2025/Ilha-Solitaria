using System.Collections.Generic;

public interface ICrafting
{
    public void SelecionarFabricavel(ItemFabricavel item);    
    public bool GenerateItem(ref ItemInventory item);

    public List<ReceitaItem> Receitas();
}
