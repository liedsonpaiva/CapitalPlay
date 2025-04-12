using Godot;
using System;

public partial class Creditos : Control
{
    private Button _btnVoltar;

    public override void _Ready()
    {
        _btnVoltar = GetNode<Button>("voltaMenu");
        _btnVoltar.Pressed += OnVoltaMenuPressed;
    }

    private void OnVoltaMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://Cenas/Menu/Menu.tscn");
    }
}
