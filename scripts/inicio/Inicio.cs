using Godot;
using System;

public partial class Inicio : Control
{
   public override void _Ready()
    {
    GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/InicioBotao").Pressed += OnInicioBotaoPressed;
    GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/CreditosBotao").Pressed += OnCreditosBotaoPressed;
    GetNode<Button>("MarginContainer/HBoxContainer/VBoxContainer/SairBotao").Pressed += OnSairButtonPressed;
    }
    
    // Botão de início pressionado
    private void OnInicioBotaoPressed()
    {
        GetTree().ChangeSceneToFile("res://cenas/inicio/caminhada_na_chuva.tscn");
    }

    // Botão de créditos pressionado
    private void OnCreditosBotaoPressed()
    {
        GetTree().ChangeSceneToFile("res://cenas/creditos_e_vitoria/creditos.tscn");
    }

    // Botão de sair pressionado
    private void OnSairButtonPressed()
    {
        GetTree().Quit();
    }
}
