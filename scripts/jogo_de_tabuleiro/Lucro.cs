using System;
using Godot;
public partial class Lucro : Node
{
    private Saldo saldo;

    public Lucro(Saldo saldo)
    {
        this.saldo = saldo;
    }

    public float CalcularLucro()
    {
        return saldo.PuxarSaldo() - 100.0f;
    }

    public void AtualizarLucro(Label labelLucro)
    {
        float lucroAtual = CalcularLucro();
        labelLucro.Text = $"R$ {lucroAtual:0.00}";
    }
}
