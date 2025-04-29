using System;
using System.Collections.Generic;
using Godot;
public partial class Cartas : Node
{
    private Random random = new Random();

    public List<CartaInformacao> SortearCartas(int quantidade)
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

    public void ExibirCarta(CartaInformacao carta, Node parent)
    {
        var sprite2D = new Sprite2D();
        string caminho = "";

        switch (carta.Nicho)
        {
            case "Transporte":
                caminho = carta.Informacao ? "res://assets/cartas/carta_alta_transporte.png" : "res://assets/cartas/carta_baixa_transporte.png";
                break;
            case "Siderúrgica":
                caminho = carta.Informacao ? "res://assets/cartas/carta_alta_siderurgica.png" : "res://assets/cartas/carta_baixa_siderurgica.png";
                break;
            case "Tecnologia":
                caminho = carta.Informacao ? "res://assets/cartas/carta_alta_tecnologia.png" : "res://assets/cartas/carta_baixa_tecnologia.png";
                break;
            case "Saúde":
                caminho = carta.Informacao ? "res://assets/cartas/carta_alta_saude.png" : "res://assets/cartas/carta_baixa_saude.png";
                break;
            case "Alimentação":
                caminho = carta.Informacao ? "res://assets/cartas/carta_alta_alimentacao.png" : "res://assets/cartas/carta_baixa_alimentacao.png";
                break;
            default:
                GD.PrintErr("Nicho de carta desconhecido: " + carta.Nicho);
                return;
        }

        var textura = GD.Load<Texture2D>(caminho);
        if (textura == null)
        {
            GD.PrintErr("Falha ao carregar a textura da carta: " + caminho);
            return;
        }

        sprite2D.Texture = textura;

        Vector2 janelaTamanho = parent.GetViewport().GetVisibleRect().Size;
        sprite2D.Position = janelaTamanho - new Vector2(sprite2D.Texture.GetSize().X * 0.5f + 20, sprite2D.Texture.GetSize().Y * 0.55f + 20);
        sprite2D.Scale = new Vector2(0.8f, 0.8f);

        parent.AddChild(sprite2D);
    }

}
