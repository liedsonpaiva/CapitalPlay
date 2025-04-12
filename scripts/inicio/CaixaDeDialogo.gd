extends MarginContainer

# Referências aos nós da cena
@onready var text_label: Label = $label_margin/text_label
@onready var letter_timer_display: Timer = $letter_timer_display

# Largura máxima permitida para o texto
const MAX_WIDTH = 256

# Variáveis de controle do texto e índice da letra
var text = ""
var letter_index = 0

# Tempos de exibição de caracteres
var letter_display_timer := 0.07
var space_display_timer := 0.05
var punctuaction_display_timer := 0.02

# Sinal emitido quando todo o texto foi exibido
signal text_display_finished()

# Função para iniciar a exibição do texto
func display_text(text_to_display: String):
	text = text_to_display
	text_label.text = text_to_display

	await resized  # Aguarda a atualização do tamanho do label

	# Define o tamanho mínimo da área do texto
	custom_minimum_size.x = min(size.x, MAX_WIDTH)

	# Se o texto for maior que a largura máxima, ativa a quebra automática
	if size.x > MAX_WIDTH:
		text_label.autowrap_mode = TextServer.AUTOWRAP_WORD
		await resized
		await resized  # Aguarda ajustes no tamanho da interface
		custom_minimum_size.y = size.y

	# Ajusta a posição do texto na tela
	global_position.x -= size.x / 2
	global_position.x -= size.y + 24

	# Limpa o texto e inicia a exibição gradual
	text_label.text = ""
	display_letter()

# Exibe as letras do texto gradualmente
func display_letter():
	text_label.text += text[letter_index]
	letter_index += 1

	# Se todas as letras foram exibidas, emite o sinal de conclusão
	if letter_index >= text.length():
		text_display_finished.emit()
		return

	# Define o tempo de espera com base no tipo de caractere
	match text[letter_index]:
		"!", "?", ",", ".":
			letter_timer_display.start(punctuaction_display_timer)
		" ":
			letter_timer_display.start(space_display_timer)
		_:
			letter_timer_display.start(letter_display_timer)

# Callback chamado quando o temporizador termina
func _on_letter_timer_display_timeout() -> void:
	display_letter()
