extends Control
class_name tabuleiroclass

@onready var botao_proximo_turno: Button = $BotaoProximoTurno
@onready var lucro: Sprite2D = $Lucro
@onready var label_saldo_valor: Label = $Sprite2D/SaldoValor

var _current_carta_sprite: Sprite2D = null
var quantiAcaoAlim := 0
var quantiAcaoSider := 0
var quantiAcaoTecno := 0
var quantiAcaoTrans := 0
var quantiAcaoSau := 0
var carta_anterior: carta_informacao = null
var acoes_lista: Array = []
var saldo: saldos
var menu: MenuDeAcoes

func D6() -> int:
	return randi_range(1, 6)

func D66() -> int:
	return randi_range(-6, 6)

func D8() -> int:
	return randi_range(4, 8)

func adicionar_acao(nicho: String, preco: float) -> void:
	var nova_acao = acoes.new(nicho, preco)
	acoes_lista.append(nova_acao)

func inicializar_acoes() -> void:
	adicionar_acao("Transporte", D8())
	adicionar_acao("Siderúrgica", D8())
	adicionar_acao("Tecnologia", D8())
	adicionar_acao("Saúde", D8())
	adicionar_acao("Alimentação", D8())
	atualizar_valores_acoes()

func atualizar_valores_acoes() -> void:
	$"SituaçAçãoTrans/ValorAçãoTrans".text = "R$ %.2f" % max(acoes_lista[0].preco, 0.01)
	$"SituaçAçãoSider/ValorAçãoSider".text = "R$ %.2f" % max(acoes_lista[1].preco, 0.01)
	$"SituaçAçãoTecno/ValorAçãoTecno".text = "R$ %.2f" % max(acoes_lista[2].preco, 0.01)
	$"SituaçAçãoSaúde/ValorAçãoSau".text = "R$ %.2f" % max(acoes_lista[3].preco, 0.01)
	$"SituaçAçãoAlimen/ValorAçãoAlime".text = "R$ %.2f" % max(acoes_lista[4].preco, 0.01)

func subida(acao: acoes) -> void:
	acao.preco += D6()
	acao.preco = max(acao.preco, 0.01)

func queda(acao: acoes) -> void:
	acao.preco -= D6()
	acao.preco = max(acao.preco, 0.01)

func volatil(acao: acoes) -> void:
	acao.preco += D66()
	acao.preco = max(acao.preco, 0.01)

func mudar_valor_acoes(carta: carta_informacao) -> void:
	var acao1 = _buscar_acao_por_nicho(carta.nicho)
	if acao1 == null:
		print("Erro: Ação não encontrada para nicho '%s'" % carta.nicho)
		return

	if carta.informacao:
		print("Subida para o nicho '%s'" % acao1.nicho)
		subida(acao1)
	else:
		print("Queda para o nicho '%s'" % acao1.nicho)
		queda(acao1)

	print("Preço após impacto direto: R$ %.2f" % acao1.preco)

	for acao in acoes_lista:
		if acao.nicho != acao1.nicho:
			print("Volatilidade para o nicho '%s'" % acao.nicho)
			volatil(acao)
			print("Novo preço do nicho '%s': R$ %.2f" % [acao.nicho, acao.preco])

	atualizar_valores_acoes()

func _buscar_acao_por_nicho(nicho: String) -> acoes:
	for acao in acoes_lista:
		if acao.nicho == nicho:
			return acao
	return null

func nova_rodada() -> void:
	if _current_carta_sprite:
		remove_child(_current_carta_sprite)
		_current_carta_sprite.queue_free()
		_current_carta_sprite = null

	if carta_anterior != null:
		print("Aplicando impacto da carta anterior: %s" % carta_anterior.nicho)
		mudar_valor_acoes(carta_anterior)

	var cartas_sorteadas = sortear_cartas(1)
	carta_anterior = cartas_sorteadas[0]
	print("Nova carta sorteada: %s" % carta_anterior.nicho)
	exibir_carta(carta_anterior)

func sortear_cartas(quantidade: int) -> Array:
	var cartas := []
	var nichos_possiveis = ["Transporte", "Siderúrgica", "Tecnologia", "Saúde", "Alimentação"]
	for i in range(quantidade):
		var nicho_aleatorio = nichos_possiveis[randi() % nichos_possiveis.size()]
		var informacao_aleatoria = randf() > 0.5
		cartas.append(carta_informacao.new(nicho_aleatorio, informacao_aleatoria))
	return cartas

func exibir_carta(carta: carta_informacao) -> void:
	var sprite := Sprite2D.new()
	var caminho := ""

	match carta.nicho:
		"Transporte":
			caminho = "res://assets/cartas/carta_alta_transporte.png" if carta.informacao else "res://assets/cartas/carta_baixa_transporte.png"
		"Siderúrgica":
			caminho = "res://assets/cartas/carta_alta_siderurgica.png" if carta.informacao else "res://assets/cartas/carta_baixa_siderurgica.png"
		"Tecnologia":
			caminho = "res://assets/cartas/carta_alta_tecnologia.png" if carta.informacao else "res://assets/cartas/carta_baixa_tecnologia.png"
		"Saúde":
			caminho = "res://assets/cartas/carta_alta_saude.png" if carta.informacao else "res://assets/cartas/carta_baixa_saude.png"
		"Alimentação":
			caminho = "res://assets/cartas/carta_alta_alimentacao.png" if carta.informacao else "res://assets/cartas/carta_baixa_alimentacao.png"

	sprite.texture = load(caminho)
	var janela_tamanho = get_viewport().get_visible_rect().size
	sprite.position = janela_tamanho - Vector2(sprite.texture.get_size().x * 0.5 + 20, sprite.texture.get_size().y * 0.55 + 20)
	sprite.scale = Vector2(0.8, 0.8)
	add_child(sprite)
	_current_carta_sprite = sprite

func calcular_preco_total(nicho: String, quantidade: int) -> float:
	if quantidade <= 0:
		return 0.00
	
	var preco_acao = menu.calcular_preco_acao(nicho)
	
	if preco_acao <= 0.0:
		preco_acao = 0.01
	
	return preco_acao * quantidade

func atualizar_lucro() -> void:
	var lucro_atual = saldo.puxar_saldo() - 100.0
	var label_lucro: Label = $Lucro/LucroAtual
	label_lucro.text = "R$ %.2f" % lucro_atual

func _ready() -> void:
	botao_proximo_turno.pressed.connect(nova_rodada)
	saldo = saldos.new(100.0)
	label_saldo_valor.text = "R$ %.2f" % saldo.puxar_saldo()

	menu = MenuDeAcoes.new()
	menu.configurar(self, saldo)

	inicializar_acoes()
	atualizar_lucro()

	# Inicializa valores dos investimentos
	$ticketAlimentacao/valorInvestido.text = "R$ 0,00"
	$ticketSiderurgica/valorInvestido.text = "R$ 0,00"
	$ticketTecnologia/valorInvestido.text = "R$ 0,00"
	$ticketTransporte/valorInvestido.text = "R$ 0,00"
	$ticketSaude/valorInvestido.text = "R$ 0,00"

	print("Ações iniciais:")
	for acao in acoes_lista:
		print("Nicho: %s, Preço: %f" % [acao.nicho, acao.preco])

	nova_rodada()
	print("Inicialização do Tabuleiro.")


# Parte 3: Métodos de Realizar compras e vendas

# Verifica se o jogador venceu
func verificar_vitoria():
	if saldo.puxar_saldo() >= 500:
		get_tree().change_scene_to_file("res://cenas/cena_vitoria/cena_vitoria.tscn")

# Função genérica para compras
func realizar_compra(nome_acao: String, label_node: Label, quantidade_ref: String):
	var quantidade = self.get(quantidade_ref)

	label_node.text = "R$ %.2f" % menu.calcular_preco_total(nome_acao, quantidade)
	menu.comprar_acao(nome_acao, quantidade)
	quantidade += 1
	self.set(quantidade_ref, quantidade)

	label_node.text = "R$ %.2f" % menu.calcular_preco_total(nome_acao, quantidade)
	$Sprite2D/SaldoValor.text = "R$ %.2f" % saldo.puxar_saldo()
	atualizar_lucro()
	verificar_vitoria()

func realizar_venda(nome_acao: String, label_node: Label, quantidade_ref: String):
	var quantidade = self.get(quantidade_ref)

	if quantidade <= 0:
		label_node.text = "R$ 0.00"
		return

	menu.vender_acao(nome_acao, 1)
	quantidade -= 1
	self.set(quantidade_ref, quantidade)

	label_node.text = "R$ %.2f" % menu.calcular_preco_total(nome_acao, quantidade)
	$Sprite2D/SaldoValor.text = "R$ %.2f" % saldo.puxar_saldo()
	atualizar_lucro()

	
func _on_btn_comprar_alim_pressed():
	realizar_compra("Alimentação", $ticketAlimentacao/valorInvestido, "quantiAcaoAlim")

func _on_btn_comprar_side_pressed():
	realizar_compra("Siderúrgica", $ticketSiderurgica/valorInvestido, "quantiAcaoSider")

func _on_btn_comprar_tecno_pressed():
	realizar_compra("Tecnologia", $ticketTecnologia/valorInvestido, "quantiAcaoTecno")

func _on_btn_comprar_trans_pressed():
	realizar_compra("Transporte", $ticketTransporte/valorInvestido, "quantiAcaoTrans")

func _on_btn_comprar_sau_pressed():
	realizar_compra("Saúde", $ticketSaude/valorInvestido, "quantiAcaoSau")

func _on_btn_vender_alim_pressed():
	realizar_venda("Alimentação", $ticketAlimentacao/valorInvestido, "quantiAcaoAlim")

func _on_btn_vender_side_pressed():
	realizar_venda("Siderúrgica", $ticketSiderurgica/valorInvestido, "quantiAcaoSider")

func _on_btn_vender_tecno_pressed():
	realizar_venda("Tecnologia", $ticketTecnologia/valorInvestido, "quantiAcaoTecno")

func _on_btn_vender_trans_pressed():
	realizar_venda("Transporte", $ticketTransporte/valorInvestido, "quantiAcaoTrans")

func _on_btn_vender_sau_pressed():
	realizar_venda("Saúde", $ticketSaude/valorInvestido, "quantiAcaoSau")
