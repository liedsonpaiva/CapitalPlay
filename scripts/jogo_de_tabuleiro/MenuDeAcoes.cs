/**
 * Controla o sistema de compra e venda de ações do jogador.
 * Usa referências para o Tabuleiro (onde estão as ações) e para o Saldo (dinheiro do jogador).
 * Calcula preços, atualiza valores em tempo real e gerencia a quantidade de ações por nicho.
 * Também verifica se há saldo suficiente para comprar e se o jogador possui ações para vender.
 */

using System;
using Godot;
public partial class MenuDeAcoes : Node
{
    private Tabuleiro tabuleiroRef;
    private Saldo saldoRef;
    private int quantidadeAtual = 1;
    private string nichoAtual = "";
    private float valorDeCompra = 0f;

    public void Configurar(Tabuleiro tabuleiro, Saldo saldo)
    {
        tabuleiroRef = tabuleiro;
        saldoRef = saldo;
    }

    public float CalcularPrecoTotal(string nicho, int quantidade)
    {
        if (quantidade <= 0)
            return 0.0f;

        Acoes acao = tabuleiroRef.BuscarAcaoPorNicho(nicho);
        if (acao == null)
        {
            GD.Print("Ação não encontrada!");
            return 0.0f;
        }

        float precoAcao = Math.Max(acao.Preco, 0.01f);
        return precoAcao * quantidade;
    }

    public void AtualizarPrecoEmTempoReal(string nicho, int quantidade)
    {
        nichoAtual = nicho;
        quantidadeAtual = quantidade;
        float precoTotal = CalcularPrecoTotal(nicho, quantidade);
        GD.Print($"Preço total para comprar {quantidade} ações de {nicho}: {precoTotal}");
    }

    public float GetPreco(string nicho)
    {
        Acoes acao = tabuleiroRef.BuscarAcaoPorNicho(nicho);
        return acao != null ? Math.Max(acao.Preco, 0.01f) : 0.0f;
    }

    public void ComprarAcao(string nicho, int quantidade)
    {
        if (quantidade <= 0)
        {
            GD.Print("Quantidade inválida para compra.");
            return;
        }

        float precoTotal = CalcularPrecoTotal(nicho, quantidade);
        valorDeCompra = precoTotal;

        if (saldoRef.SubtrairSaldo(precoTotal))
        {
            GD.Print($"Compra realizada: {quantidade} ação(ões) de {nicho} por {precoTotal / quantidade} cada.");
            AumentarQuantidade(nicho, quantidade);
        }
        else
        {
            GD.Print("Compra não realizada. Saldo insuficiente.");
        }
    }

    public void VenderAcao(string nicho, int quantidade)
    {
        if (quantidade <= 0)
        {
            GD.Print("Quantidade inválida para venda.");
            return;
        }

        if (!TemAcaoParaVender(nicho, quantidade))
        {
            GD.Print("Você não tem ações suficientes para vender nesse nicho");
            return;
        }

        float valorTotal = CalcularPrecoTotal(nicho, quantidade);
        DiminuirQuantidade(nicho, quantidade);
        saldoRef.AumentarSaldo(valorTotal);
        GD.Print($"Venda realizada: {quantidade} ação(ões) de {nicho} por {valorTotal / quantidade} cada.");
    }

    public void AtualizarValorEmTempoRealVenda(string nicho, int quantidade)
    {
        nichoAtual = nicho;
        quantidadeAtual = quantidade;
        float valorTotal = CalcularPrecoTotal(nicho, quantidade);
        GD.Print($"Valor total para vender {quantidade} ações de {nicho}: {valorTotal}");
    }

    public float MostrarLucro(string nicho, int quantidade)
    {
        float precoAtual = CalcularPrecoTotal(nicho, quantidade);
        return Mathf.Abs(precoAtual - valorDeCompra);
    }

    private void AumentarQuantidade(string nicho, int quantidade)
    {
        switch (nicho)
        {
            case "Alimentação": tabuleiroRef.QuantiAcaoAlim += quantidade; break;
            case "Transporte": tabuleiroRef.QuantiAcaoTrans += quantidade; break;
            case "Tecnologia": tabuleiroRef.QuantiAcaoTecno += quantidade; break;
            case "Siderúrgica": tabuleiroRef.QuantiAcaoSider += quantidade; break;
            case "Saúde": tabuleiroRef.QuantiAcaoSau += quantidade; break;
        }
    }

    private void DiminuirQuantidade(string nicho, int quantidade)
    {
        switch (nicho)
        {
            case "Alimentação": tabuleiroRef.QuantiAcaoAlim -= quantidade; break;
            case "Transporte": tabuleiroRef.QuantiAcaoTrans -= quantidade; break;
            case "Tecnologia": tabuleiroRef.QuantiAcaoTecno -= quantidade; break;
            case "Siderúrgica": tabuleiroRef.QuantiAcaoSider -= quantidade; break;
            case "Saúde": tabuleiroRef.QuantiAcaoSau -= quantidade; break;
        }
    }

    private bool TemAcaoParaVender(string nicho, int quantidade)
    {
        return nicho switch
        {
            "Alimentação" => tabuleiroRef.QuantiAcaoAlim >= quantidade,
            "Transporte" => tabuleiroRef.QuantiAcaoTrans >= quantidade,
            "Tecnologia" => tabuleiroRef.QuantiAcaoTecno >= quantidade,
            "Siderúrgica" => tabuleiroRef.QuantiAcaoSider >= quantidade,
            "Saúde" => tabuleiroRef.QuantiAcaoSau >= quantidade,
            _ => false
        };
    }
}