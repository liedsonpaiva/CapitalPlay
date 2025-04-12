using Godot;
using System;
using System.Threading.Tasks;

public partial class CaminhadaNaChuva : Node2D 
{
    private AnimationPlayer _animationPlayer;

    private Label _label;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _label = GetNode<Label>("MarginContainer/HBoxContainer/VBoxContainer/Label");

        GD.Print(_label.Text); // Só pra testar se está acessando
        _label.Visible = true;

        // Conectar o sinal de animação finalizada
        _animationPlayer.AnimationFinished += OnAnimationFinished;

        // Inicia a animação da cutscene
        _animationPlayer.Play("CaminhadaNaChuva");
    }

    private async void OnAnimationFinished(StringName animName)
    {
        if (animName == "CaminhadaNaChuva")
        {
            // Espera 0.1 segundo antes de trocar de cena
            await ToSignal(GetTree().CreateTimer(0.1), "timeout");
            TrocarDeCena();
        }
    }

    private void TrocarDeCena()
    {
        GetTree().ChangeSceneToFile("res://Cenas/Atos/CenaDeTransicaoDaMaeChamandoFilho.tscn");
    }
}
