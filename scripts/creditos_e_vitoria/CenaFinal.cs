using Godot;
using System;
using System.Threading.Tasks;

public partial class CenaFinal : Control
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("Final");

        // Conecta o sinal de término da animação
        _animationPlayer.AnimationFinished += OnAnimationFinished;

        // Inicia a animação
        _animationPlayer.Play("Final");
    }

    private async void OnAnimationFinished(StringName animationName)
    {
        if (animationName == "Final")
        {
            // Aguarda 0.1 segundo antes de trocar a cena
            await ToSignal(GetTree().CreateTimer(0.1), "timeout");
            TrocarDeCena();
        }
    }

    private void TrocarDeCena()
    {
        GetTree().ChangeSceneToFile("res://cenas/creditos_e_vitoria/cena_final_caminhada_ensolarada.tscn");
    }
}
