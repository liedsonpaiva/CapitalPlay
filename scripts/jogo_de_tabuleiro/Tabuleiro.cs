using Godot;
using System;
using System.Collections.Generic;

public partial class Tabuleiro : Node
{
    [Export]
    private Sprite2D Lucro;

    [Export]
    private Label SaldoValor;

    private Sprite2D _currentCartaSprite2D = null;

    private Mercado mercado;
    private Cartas cartas;
    private Lucro lucro;
    private GerenciadorDeCartas gerenciadorDeCartas;

    public int QuantiAcaoAlim;
    public int QuantiAcaoSider;
    public int QuantiAcaoTecno;
    public int QuantiAcaoTrans;
    public int QuantiAcaoSau;

    private CartaInformacao cartaAnterior = null;
    private List<Acoes> acoesLista = new List<Acoes>();
    private List<CartaInformacao> cartasSorteadas = new List<CartaInformacao>();

    private Saldo saldo;
    private MenuDeAcoes menu;

    // Declaração dos botões
    [Export] private Button OnBotaoProximoTurno;
    [Export] private Button BtnComprarAlim;
    [Export] private Button BtnComprarSide;
    [Export] private Button BtnComprarTecno;
    [Export] private Button BtnComprarTrans;
    [Export] private Button BtnComprarSau;
    [Export] private Button BtnVenderAlim;
    [Export] private Button BtnVenderSide;
    [Export] private Button BtnVenderTecno;
    [Export] private Button BtnVenderTrans;
    [Export] private Button BtnVenderSau;


    private Random random = new Random();

    // Inicialização do tabuleiro e classes
    public override void _Ready()
    {
        saldo = new Saldo(100.0f);
        lucro = new Lucro(saldo);
        mercado = new Mercado();
        cartas = GetNode<Cartas>("AreaDaCarta");

        lucro.AtualizarLucro(GetNode<Label>("Lucro/LucroAtual"));
        mercado.InicializarAcoes();

        // Conectar os botões diretamente
        OnBotaoProximoTurno.Pressed += NovaRodada;

        BtnComprarAlim.Pressed += OnBtnComprarAlimPressed;
        BtnComprarSide.Pressed += OnBtnComprarSidePressed;
        BtnComprarTecno.Pressed += OnBtnComprarTecnoPressed;
        BtnComprarTrans.Pressed += OnBtnComprarTransPressed;
        BtnComprarSau.Pressed += OnBtnComprarSauPressed;

        BtnVenderAlim.Pressed += OnBtnVenderAlimPressed;
        BtnVenderSide.Pressed += OnBtnVenderSidePressed;
        BtnVenderTecno.Pressed += OnBtnVenderTecnoPressed;
        BtnVenderTrans.Pressed += OnBtnVenderTransPressed;
        BtnVenderSau.Pressed += OnBtnVenderSauPressed;

        SaldoValor.Text = $"R$ {saldo.PuxarSaldo():0.00}";

        menu = new MenuDeAcoes();
        menu.Configurar(this, saldo);

        InicializarValoresInvestimentos();
        MostrarAcoesIniciais();
        NovaRodada();
    }


    private void NovaRodada()
    {
        if (_currentCartaSprite2D != null)
        {
            RemoveChild(_currentCartaSprite2D);
            _currentCartaSprite2D.QueueFree();
            _currentCartaSprite2D = null;
        }

        if (cartaAnterior != null)
        {
            GD.Print($"Aplicando impacto da carta anterior: {cartaAnterior.Nicho}");
            mercado.MudarValorAcoes(cartaAnterior);
        }

        var sorteada = cartas.SortearCartas(1)[0];
        cartaAnterior = cartasSorteadas[0];
        GD.Print($"Nova carta sorteada: {cartaAnterior.Nicho}");
        cartas.ExibirCarta(sorteada, cartas); // o próprio nó Cartas (que é Sprite2D) como pai

        lucro.AtualizarLucro(GetNode<Label>("Lucro/LucroAtual"));
    }

    private void MostrarAcoesIniciais()
    {
        GD.Print("Ações iniciais:");
        foreach (var acao in mercado.GetTodasAcoes())
        {
            GD.Print($"Nicho: {acao.Nicho}, Preço: {acao.Preco}");
        }
    }

    private void InicializarValoresInvestimentos()
    {
        GetNode<Label>("ticketAlimentacao/valorInvestido").Text = "R$ 0,00";
        GetNode<Label>("ticketSiderurgica/valorInvestido").Text = "R$ 0,00";
        GetNode<Label>("ticketTecnologia/valorInvestido").Text = "R$ 0,00";
        GetNode<Label>("ticketTransporte/valorInvestido").Text = "R$ 0,00";
        GetNode<Label>("ticketSaude/valorInvestido").Text = "R$ 0,00";
    }

    // Método para realizar compra
    private void RealizarCompra(string nomeAcao, Label labelNode, ref int quantidadeAcao)
    {
        float precoTotal = menu.CalcularPrecoTotal(nomeAcao, quantidadeAcao + 1);

        if (saldo.PuxarSaldo() >= precoTotal)
        {
            labelNode.Text = $"R$ {precoTotal:0.00}";
            menu.ComprarAcao(nomeAcao, 1);
            quantidadeAcao++;
            AtualizarQuantidadeAcao(nomeAcao, quantidadeAcao);
            AtualizarSaldo();
        }
        else
        {
            GD.Print("Saldo insuficiente para comprar mais ações.");
        }
    }

    // Método para realizar venda
    private void RealizarVenda(string nomeAcao, Label labelNode, ref int quantidadeAcao)
    {
        if (quantidadeAcao > 0)
        {
            menu.VenderAcao(nomeAcao, 1);
            quantidadeAcao--;
            AtualizarQuantidadeAcao(nomeAcao, quantidadeAcao);
            labelNode.Text = $"R$ {menu.CalcularPrecoTotal(nomeAcao, quantidadeAcao):0.00}";
            AtualizarSaldo();
        }
        else
        {
            GD.Print("Não há ações suficientes para vender.");
        }
    }

    // Atualiza a quantidade de ações no display
    private void AtualizarQuantidadeAcao(string nomeAcao, int quantidade)
    {
        // Atualiza o label da quantidade de ações, pode ser adicionado conforme o nome da ação
        GetNode<Label>($"ticket{nomeAcao}/quantidadeAcoes").Text = $"{quantidade} Ações";
    }

    // Atualiza o saldo
    private void AtualizarSaldo()
    {
        SaldoValor.Text = $"R$ {saldo.PuxarSaldo():0.00}";
    }

    // Métodos dos botões de compra
    private void OnBtnComprarAlimPressed() => RealizarCompra("Alimentação", GetNode<Label>("ticketAlimentacao/valorInvestido"), ref QuantiAcaoAlim);
    private void OnBtnComprarSidePressed() => RealizarCompra("Siderúrgica", GetNode<Label>("ticketSiderurgica/valorInvestido"), ref QuantiAcaoSider);
    private void OnBtnComprarTecnoPressed() => RealizarCompra("Tecnologia", GetNode<Label>("ticketTecnologia/valorInvestido"), ref QuantiAcaoTecno);
    private void OnBtnComprarTransPressed() => RealizarCompra("Transporte", GetNode<Label>("ticketTransporte/valorInvestido"), ref QuantiAcaoTrans);
    private void OnBtnComprarSauPressed() => RealizarCompra("Saúde", GetNode<Label>("ticketSaude/valorInvestido"), ref QuantiAcaoSau);

    // Métodos dos botões de venda
    private void OnBtnVenderAlimPressed() => RealizarVenda("Alimentação", GetNode<Label>("ticketAlimentacao/valorInvestido"), ref QuantiAcaoAlim);
    private void OnBtnVenderSidePressed() => RealizarVenda("Siderúrgica", GetNode<Label>("ticketSiderurgica/valorInvestido"), ref QuantiAcaoSider);
    private void OnBtnVenderTecnoPressed() => RealizarVenda("Tecnologia", GetNode<Label>("ticketTecnologia/valorInvestido"), ref QuantiAcaoTecno);
    private void OnBtnVenderTransPressed() => RealizarVenda("Transporte", GetNode<Label>("ticketTransporte/valorInvestido"), ref QuantiAcaoTrans);
    private void OnBtnVenderSauPressed() => RealizarVenda("Saúde", GetNode<Label>("ticketSaude/valorInvestido"), ref QuantiAcaoSau);

    // Verifica a vitória (se saldo for >= 500)
    private void VerificarVitoria()
    {
        if (saldo.PuxarSaldo() >= 500)
        {
            GetTree().ChangeSceneToFile("res://cenas/cena_vitoria/cena_vitoria.tscn");
        }
    }

    internal Acoes BuscarAcaoPorNicho(string nicho)
    {
        throw new NotImplementedException();
    }

}
