using Godot;
using System;

public partial class CenaDeTransicaoDaMaeChamandoFilho : Control // Substitua pelo nome adequado do script
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        // Conecta o sinal de animação finalizada
        _animationPlayer.AnimationFinished += OnAnimationFinished;

        // Inicia a animação da cutscene
        _animationPlayer.Play("Cutsene Ato 1 Cena 2");
    }

    private async void OnAnimationFinished(StringName animationName)
    {
        if (animationName == "Cutsene Ato 1 Cena 2")
        {
            await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
            TrocarDeCena();
        }
    }

    private void TrocarDeCena()
    {
        GetTree().ChangeSceneToFile("res://Cenas/Atos/Cena 2 A casa.tscn");
    }
}
