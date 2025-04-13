using Godot;
using System;

public partial class Creditos : Control
{
   public override void _Ready()
    {
    var botao = GetNode<Button>("marginContainer/HBoxContainer/VBoxContainer/voltarMenu");
    botao.Pressed += OnVoltarMenuPressed;
    }

    private void OnVoltarMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://cenas/inicio/inicio.tscn"); 
    }
}