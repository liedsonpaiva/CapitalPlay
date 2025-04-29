/** 
 * Representa uma carta que contém uma informação positiva ou negativa sobre um nicho de mercado.
 * Pode ser usada para influenciar o valor das ações relacionadas a esse nicho.
 */

using System;
using Godot;
public partial class CartaInformacao : Node
{
    public string Nicho { get; set; }
    public bool Informacao { get; set; }

    public CartaInformacao(string nicho, bool informacao)
    {
        Nicho = nicho;
        Informacao = informacao;
    }
}
