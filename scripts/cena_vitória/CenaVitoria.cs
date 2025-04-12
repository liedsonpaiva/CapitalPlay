using Godot;
using System;

public partial class CenaVitoria : Node2D
{
    private Button _cenaFinalButton;

    public override void _Ready()
    {
        // Atribui corretamente o botão à variável
        _cenaFinalButton = GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/cena_final");

        // Conecta o evento
        _cenaFinalButton.Pressed += OnCenaFinalPressed;
    }

    private void OnCenaFinalPressed()
    {
        // Troca de cena quando o botão for pressionado
        GetTree().ChangeSceneToFile("res://cenas/creditos_e_vitoria/cena_final.tscn");
    }
}
