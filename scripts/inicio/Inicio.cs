using Godot;
using System;

public partial class Inicio : Control
{
    [Export] private Button InicioBotao;
    [Export] private Button CreditosBotao;
    [Export] private Button SairBotao;

    public override void _Ready()
    {
        InicioBotao.Pressed += OnInicioBotaoPressed;
        CreditosBotao.Pressed += OnCreditosBotaoPressed;
        SairBotao.Pressed += OnSairButtonPressed; 
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
