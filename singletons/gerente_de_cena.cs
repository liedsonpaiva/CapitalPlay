using Godot;
using System;

public partial class gerente_de_cena : Node
{
    public void TrocarPara(string nomeCena)
    {
        var caminho = $"res://cenas/{nomeCena}.tscn";

        if (ResourceLoader.Exists(caminho))
        {
            GetTree().ChangeSceneToFile(caminho);
        }
        else
        {
            GD.PrintErr("Cena não encontrada: ", caminho);
        }
    }
}
