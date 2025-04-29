using Godot;
using System;
using System.Collections.Generic;

public partial class GerenciadorDeCartas : Node
{
    private Sprite2D _currentCartaSprite2D;
    private CartaInformacao cartaAnterior;
    private Random random = new Random();

    private void NovaRodada()
    {
        // Remove a carta atual se existir
        if (_currentCartaSprite2D != null)
        {
            RemoveChild(_currentCartaSprite2D);
            _currentCartaSprite2D.QueueFree();
            _currentCartaSprite2D = null;
        }

        // Aplica impacto da carta anterior
        if (cartaAnterior != null)
        {
            GD.Print($"Aplicando impacto da carta anterior: {cartaAnterior.Nicho}");
            MudarValorAcoes(cartaAnterior);
        }

        // Sorteia e exibe nova carta
        var cartasSorteadas = SortearCartas(1);
        cartaAnterior = cartasSorteadas[0];
        GD.Print($"Nova carta sorteada: {cartaAnterior.Nicho}");
        ExibirCarta(cartaAnterior);
    }

    private List<CartaInformacao> SortearCartas(int quantidade)
    {
        var cartas = new List<CartaInformacao>();
        var nichosPossiveis = new List<string> { "Transporte", "Siderúrgica", "Tecnologia", "Saúde", "Alimentação" };

        for (int i = 0; i < quantidade; i++)
        {
            string nichoAleatorio = nichosPossiveis[random.Next(nichosPossiveis.Count)];
            bool informacaoAleatoria = random.NextDouble() > 0.5;
            cartas.Add(new CartaInformacao(nichoAleatorio, informacaoAleatoria));
        }

        return cartas;
    }

    private void ExibirCarta(CartaInformacao carta)
    {
        // Implementação para exibir a carta visualmente
        _currentCartaSprite2D = new Sprite2D();
        // Configure a textura e posição do sprite2D aqui
        AddChild(_currentCartaSprite2D);
    }

    private void MudarValorAcoes(CartaInformacao carta)
    {
        // Implementação para modificar os valores das ações baseado na carta
        // Exemplo: aumentar/diminuir preços conforme a informação da carta
    }
}