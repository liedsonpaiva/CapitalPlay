extends Control
class_name tabuleiroclass

@onready var botao_proximo_turno: Button = $BotaoProximoTurno
@onready var lucro: Sprite2D = $Lucro
@onready var label_saldo_valor: Label = $Sprite2D/SaldoValor

var _current_carta_sprite: Sprite2D = null
var quantiAcaoAlim: int = 0
var quantiAcaoSider: int = 0
var quantiAcaoTecno: int = 0
var quantiAcaoTrans: int = 0
var quantiAcaoSau: int = 0
var carta_anterior: carta_informacao = null
var acoes_lista: Array = []  
var saldo: saldos
var menu: MenuDeAcoes

func D6() -> int:
	return randi_range(1, 6)

func D66() -> int:
	return floor(randf_range(-6, 6))

func D8() -> int:
	return floor(randf_range(4, 8))

func adicionar_acao(nicho: String, preco: float):
	var nova_acao = acoes.new(nicho, preco)
	acoes_lista.append(nova_acao)

func inicializar_acoes():
	adicionar_acao("Transporte", D8())
	adicionar_acao("Siderúrgica", D8())
	adicionar_acao("Tecnologia", D8())
	adicionar_acao("Saúde", D8())
	adicionar_acao("Alimentação", D8())
	atualizar_valores_acoes()

func atualizar_valores_acoes():
	$"SituaçAçãoTrans/ValorAçãoTrans".text = "R$ %.2f" % acoes_lista[0].preco
	$"SituaçAçãoSider/ValorAçãoSider".text = "R$ %.2f" % acoes_lista[1].preco
	$"SituaçAçãoTecno/ValorAçãoTecno".text = "R$ %.2f" % acoes_lista[2].preco
	$"SituaçAçãoSaúde/ValorAçãoSau".text = "R$ %.2f" % acoes_lista[3].preco
	$"SituaçAçãoAlimen/ValorAçãoAlime".text = "R$ %.2f" % acoes_lista[4].preco

func ajustar_preco_minimo(acao: acoes) -> void:
	if acao.preco < 1.0:
		acao.preco = 1.0

func subida(acao: acoes) -> void:
	acao.preco += D6()

func queda(acao: acoes) -> void:
	acao.preco -= D6()
	ajustar_preco_minimo(acao)

func volatil(acao: acoes) -> void:
	acao.preco += D66()
	ajustar_preco_minimo(acao)

func mudar_valor_acoes(carta: carta_informacao):
	var acao1 = _buscar_acao_por_nicho(carta.nicho)
	if not acao1:
		print("Erro: Ação não encontrada para nicho '%s'" % carta.nicho)
		return

	if carta.informacao:
		print("Subida para o nicho '%s'" % acao1.nicho)
		subida(acao1)  # Aumenta o valor da ação
	else:
		print("Queda para o nicho '%s'" % acao1.nicho)
		queda(acao1)  # Reduz o valor da ação
		
	print("Preço após impacto direto: R$ %.2f" % acao1.preco)

	# Volatilidade nas outras ações
	for acao in acoes_lista:
		if acao.nicho != acao1.nicho:
			print("Volatilidade para o nicho '%s'" % acao.nicho)
			volatil(acao)
			print("Novo preço do nicho '%s': R$ %.2f" % [acao.nicho, acao.preco])

	# Atualiza os valores na interface
	atualizar_valores_acoes()

func _buscar_acao_por_nicho(nicho: String) -> acoes:
	for acao in acoes_lista:
		if acao.nicho == nicho:
			return acao
	return null

func nova_rodada():
	# Remove a carta exibida anteriormente, se houver
	if _current_carta_sprite:
		remove_child(_current_carta_sprite)
		_current_carta_sprite.queue_free()
		_current_carta_sprite = null
	# Aplica o impacto da carta anterior (se houver)
	if carta_anterior != null:
		print("Aplicando impacto da carta anterior: %s" % carta_anterior.nicho)
		mudar_valor_acoes(carta_anterior)
	# Sorteia uma nova carta e exibe sem aplicar impacto ainda
	var cartas_sorteadas = sortear_cartas(1)
	carta_anterior = cartas_sorteadas[0]
	print("Nova carta sorteada: %s" % carta_anterior.nicho)
	exibir_carta(carta_anterior)

func sortear_cartas(quantidade: int) -> Array:
	var cartas = []
	var nichos_possiveis = ["Transporte", "Siderúrgica", "Tecnologia", "Saúde", "Alimentação"]
	for i in range(quantidade):
		var nicho_aleatorio = nichos_possiveis[randi() % nichos_possiveis.size()]
		var informacao_aleatoria = randf() > 0.5
		cartas.append(carta_informacao.new(nicho_aleatorio, informacao_aleatoria))
	return cartas

func exibir_carta(carta: carta_informacao):
	var sprite = Sprite2D.new()
	match carta.nicho:
		"Transporte":
			if carta.informacao:
				sprite.texture = preload("res://assets/cartas/carta_alta_transporte.png")
			else:
				sprite.texture = preload("res://assets/cartas/carta_baixa_transporte.png")
		"Siderúrgica":
			if carta.informacao:
				sprite.texture = preload("res://assets/cartas/carta_alta_siderurgica.png")
			else:
				sprite.texture = preload("res://assets/cartas/carta_baixa_siderurgica.png")
		"Tecnologia":
			if carta.informacao:
				sprite.texture = preload("res://assets/cartas/carta_alta_tecnologia.png")
			else:
				sprite.texture = preload("res://assets/cartas/carta_baixa_tecnologia.png")
		"Saúde":
			if carta.informacao:
				sprite.texture = preload("res://assets/cartas/carta_alta_saude.png")
			else:
				sprite.texture = preload("res://assets/cartas/carta_baixa_saude.png")
		"Alimentação":
			if carta.informacao:
				sprite.texture = preload("res://assets/cartas/carta_alta_alimentacao.png")
			else:
				sprite.texture = preload("res://assets/cartas/carta_baixa_alimentacao.png")

	var janela_tamanho = get_viewport().get_visible_rect().size
	sprite.position = janela_tamanho - Vector2(sprite.texture.get_size().x * 0.5 + 20, sprite.texture.get_size().y * 0.55 + 20)
	sprite.scale = Vector2(0.8, 0.8)
	add_child(sprite)
	_current_carta_sprite = sprite

# Função para atualizar o lucro
func atualizar_lucro():
	var lucro_atual = saldo.puxar_saldo() - 100.0
	var label_lucro: Label = $Lucro/LucroAtual
	label_lucro.text = "R$ %.2f" % lucro_atual

func _ready() -> void:
	botao_proximo_turno.connect("pressed", Callable(self, "nova_rodada"))
	saldo = saldos.new(100.0)
	label_saldo_valor.text = "R$ %.2f" % saldo.puxar_saldo()
	menu = MenuDeAcoes.new()
	menu.configurar(self, saldo)
	inicializar_acoes()
	var label_Lucro_Atual: Label = $Lucro/LucroAtual
	atualizar_lucro()  # Atualiza o lucro inicial
	
	# Inicializa valores dos investimentos
	$ticketAlimentacao/valorInvestido.text = "R$ 0,00"
	$ticketSiderurgica/valorInvestido.text = "R$ 0,00"
	$ticketTecnologia/valorInvestido.text = "R$ 0,00"
	$ticketTransporte/valorInvestido.text = "R$ 0,00"
	$ticketSaude/valorInvestido.text = "R$ 0,00"

	print("Ações iniciais:")
	for acao in acoes_lista:
		print("Nicho: %s, Preço: %f" % [acao.nicho, acao.preco])
		
	# Inicia o jogo com a primeira rodada
	nova_rodada()
	print("Inicialização do Tabuleiro.")

# Função para verificar a vitória e mudar para a cena de vitória
# Função de verificar vitória
func verificar_vitoria():
	if saldo.puxar_saldo() >= 200:
		get_tree().change_scene_to_file("res://cenas/cena_vitoria/cena_vitoria.tscn")

# Função genérica de compra
func comprar_acao(tipo: String, quantidade: int, label_valor: Label):
	menu.comprar_acao(tipo, quantidade)
	label_valor.text = "R$ %.2f" % menu.calcular_preco_total(tipo, quantidade)
	$Sprite2D/SaldoValor.text = "R$ %.2f" % saldo.puxar_saldo()
	atualizar_lucro()
	verificar_vitoria()

# Função genérica de venda
func vender_acao(tipo: String, quantidade_ref: Dictionary, label_valor: Label):
	if quantidade_ref.value == 0:
		label_valor.text = "R$ 0.00"
		return
	menu.vender_acao(tipo, 1)
	quantidade_ref.value -= 1
	if quantidade_ref.value == 0:
		label_valor.text = "R$ 0.00"
	else:
		label_valor.text = "R$ %.2f" % menu.calcular_preco_total(tipo, quantidade_ref.value)
	$Sprite2D/SaldoValor.text = "R$ %.2f" % saldo.puxar_saldo()
	atualizar_lucro()
	verificar_vitoria()

# Botões de compra
func OnBtnComprarAlimPressed() -> void:
	comprar_acao("Alimentação", quantiAcaoAlim, $ticketAlimentacao/valorInvestido)

func OnBtnComprarSidePressed() -> void:
	comprar_acao("Siderúrgica", quantiAcaoSider, $ticketSiderurgica/valorInvestido)

func OnBtnComprarTecnoPressed() -> void:
	comprar_acao("Tecnologia", quantiAcaoTecno, $ticketTecnologia/valorInvestido)

func OnBtnComprarTransPressed() -> void:
	comprar_acao("Transporte", quantiAcaoTrans, $ticketTransporte/valorInvestido)

func OnBtnComprarSauPressed() -> void:
	comprar_acao("Saúde", quantiAcaoSau, $ticketSaude/valorInvestido)

# Botões de venda
func OnBtnVenderAlimPressed() -> void:
	vender_acao("Alimentação", get_ref("quantiAcaoAlim"), $ticketAlimentacao/valorInvestido)

func OnBtnVenderSidePressed() -> void:
	vender_acao("Siderúrgica", get_ref("quantiAcaoSider"), $ticketSiderurgica/valorInvestido)

func OnBtnVenderTecnoPressed() -> void:
	vender_acao("Tecnologia", get_ref("quantiAcaoTecno"), $ticketTecnologia/valorInvestido)

func OnBtnVenderTransPressed() -> void:
	vender_acao("Transporte", get_ref("quantiAcaoTrans"), $ticketTransporte/valorInvestido)

func OnBtnVenderSauPressed() -> void:
	vender_acao("Saúde", get_ref("quantiAcaoSau"), $ticketSaude/valorInvestido)

# Função auxiliar para pegar referência mutável
func get_ref(var_name: String) -> Dictionary:
	return { "value": get(var_name) }
