using Godot;
using System;

public partial class CaixaDeDialogo : MarginContainer
{
    [Signal]
    public delegate void TextDisplayFinishedEventHandler();

    private Label _textLabel;
    private Timer _letterTimer;

    private string _text = "";
    private int _letterIndex = 0;

    private const int MaxWidth = 256;

    private double _letterDisplayTime = 0.07;
    private double _spaceDisplayTime = 0.05;
    private double _punctuationDisplayTime = 0.02;

    public override void _Ready()
    {
        _textLabel = GetNode<Label>("label_margin/text_label");
        _letterTimer = GetNode<Timer>("letter_timer_display");

        _letterTimer.Timeout += OnLetterTimerTimeout;
    }

    public async void DisplayText(string textToDisplay)
    {
        _text = textToDisplay;
        _textLabel.Text = textToDisplay;

        // Aguarda o sinal resized (simulando com await ToSignal)
        await ToSignal(this, "resized");

        // Define largura mínima
        CustomMinimumSize = new Vector2(Mathf.Min(Size.X, MaxWidth), CustomMinimumSize.Y);

        // Quebra de linha se necessário
        if (Size.X > MaxWidth)
        {
            _textLabel.AutowrapMode = TextServer.AutowrapMode.Word;
            await ToSignal(this, "resized");
            await ToSignal(this, "resized");

            CustomMinimumSize = new Vector2(CustomMinimumSize.X, Size.Y);
        }

        // Posicionamento no centro
        GlobalPosition = new Vector2(GlobalPosition.X - Size.X / 2, GlobalPosition.Y - Size.Y - 24);

        // Limpa texto para iniciar animação
        _textLabel.Text = "";
        DisplayLetter();
    }

    private void DisplayLetter()
    {
        _textLabel.Text += _text[_letterIndex];
        _letterIndex++;

        if (_letterIndex >= _text.Length)
        {
            EmitSignal(SignalName.TextDisplayFinished);
            return;
        }

        char currentChar = _text[_letterIndex];

        switch (currentChar)
        {
            case '!':
            case '?':
            case ',':
            case '.':
                _letterTimer.Start(_punctuationDisplayTime);
                break;
            case ' ':
                _letterTimer.Start(_spaceDisplayTime);
                break;
            default:
                _letterTimer.Start(_letterDisplayTime);
                break;
        }
    }

    private void OnLetterTimerTimeout()
    {
        DisplayLetter();
    }
}
