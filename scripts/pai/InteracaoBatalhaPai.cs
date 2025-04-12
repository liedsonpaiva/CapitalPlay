using Godot;
using System;

public partial class InteracaoBatalhaPai : Node2D
{
    private Label _label;
    private bool _playerInArea = false;
    private bool _awaitingChoice = false;

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _label.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_playerInArea && Input.IsActionJustPressed("interect"))
        {
            _label.Text = "Você quer jogar? (Sim(S))/(Não(N))";
            _awaitingChoice = true;
        }

        if (_awaitingChoice)
        {
            if (Input.IsActionJustPressed("accept")) // "S"
            {
                GoToBattle();
                _awaitingChoice = false;
            }
            else if (Input.IsActionJustPressed("cancel")) // "N"
            {
                ExitInteraction();
                _awaitingChoice = false;
            }
        }
    }

    private void GoToBattle()
    {
        _label.Text = "Indo para a batalha...";
        GetTree().ChangeSceneToFile("res://cenas/jogo_de_tabuleiro/jogo_de_tabuleiro.tscn");
    }

    private void ExitInteraction()
    {
        _label.Text = "Interação encerrada.";
        _label.Visible = false;
    }

    private void _OnArea2dBodyEntered(Node2D body)
    {
        if (body.Name == "CharacterBody2D")
        {
            _playerInArea = true;
            _label.Visible = true;
            _label.Text = "Pressione 'E' para interagir.";
        }
    }

    private void _OnArea2dBodyExited(Node2D body)
    {
        if (body.Name == "CharacterBody2D")
        {
            _playerInArea = false;
            _label.Visible = false;
            _awaitingChoice = false;
        }
    }
}
