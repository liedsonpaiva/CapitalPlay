using System;
using Godot;

/// <summary>
/// Controla o saldo financeiro do jogador.
/// Permite consultar, adicionar e subtrair valores, além de verificar se há saldo suficiente para transações.
/// Também verifica se o jogador atingiu a meta financeira do jogo.
/// </summary>
public class Saldo : Object
{
    private float valor = 100.0f;

    /// <summary>
    /// Retorna o valor atual do saldo.
    /// </summary>
    public float MostrarSaldo()
    {
        return valor;
    }

    /// <summary>
    /// Retorna o valor atual do saldo (versão compatível com outras classes).
    /// </summary>
    public float PuxarSaldo()
    {
        return valor;
    }

    /// <summary>
    /// Tenta subtrair um valor do saldo.
    /// Retorna false se o saldo for insuficiente.
    /// </summary>
    public bool SubtrairSaldo(float valorSubtraido)
    {
        if (valorSubtraido > valor)
        {
            GD.Print("Saldo insuficiente!");
            return false;
        }

        valor -= valorSubtraido;
        return true;
    }

    public Saldo(float valorInicial)
    {
        valor = valorInicial;
    }


    /// <summary>
    /// Aumenta o saldo com o valor especificado.
    /// </summary>
    public bool AumentarSaldo(float valorAumentado)
    {
        valor += valorAumentado;
        return true;
    }

    /// <summary>
    /// Atualiza o saldo com um valor delta, que pode ser positivo ou negativo.
    /// </summary>
    public void AtualizarSaldo(float delta)
    {
        valor += delta;
    }

    /// <summary>
    /// Verifica se o jogador atingiu a meta financeira (R$2000).
    /// </summary>
    public bool Meta()
    {
        return valor >= 2000f;
    }
}
