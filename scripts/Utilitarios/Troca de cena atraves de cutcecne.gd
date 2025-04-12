extends Node2D  # Substitua por Control ou o tipo do nó principal da sua cena, se necessário

@onready var animation_player = $AnimationPlayer  # Certifique-se de que o caminho está correto

# Função que será chamada quando a animação termina
func _on_animation_finished(animation_name):
	# Verifica se a animação que terminou é a esperada
	if animation_name == "Cena 1 Caminhada na chuva":
		# Espera 0.1 segundo antes de trocar de cena
		await get_tree().create_timer(0.1).timeout
		trocar_de_cena()

# Função para trocar de cena
func trocar_de_cena():
	# Usa o caminho diretamente como String para change_scene_to_file
	get_tree().change_scene_to_file("res://Cenas/Atos/cena_de_transição_mãe_chamando_filho.tscn")

func _ready():
	# Conecta o sinal "animation_finished" do AnimationPlayer à função "_on_animation_finished"
	animation_player.animation_finished.connect(_on_animation_finished)
	
	# Inicia a animação da cutscene
	animation_player.play("Cena 1 Caminhada na chuva")
