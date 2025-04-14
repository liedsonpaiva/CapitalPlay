/** 
 * Representa uma ação de mercado com um nicho específico e um preço atual.
 * Pode ser usada para simular ações de diferentes setores (ex: tecnologia, saúde).
 */

using System;
using Godot;
using System.Collections.Generic;

public partial class Acoes
{
    public string Nicho { get; set; }
    public float Preco { get; set; }
    
    public Acoes(string nicho = "", float preco = 0.0f)
    {
        Nicho = nicho;
        // Garante que o preço nunca seja negativo
        Preco = Math.Max(preco, 0.01f);
    }

    public void SubirPreco(float valor)
    {
        Preco += valor;
        Preco = Math.Max(Preco, 0.01f);
    }

    public void CairPreco(float valor)
    {
        Preco -= valor;
        Preco = Math.Max(Preco, 0.01f);
    }

    public void VariarPreco(float valor)
    {
        // Alternativa: você poderia usar um valor aleatório aqui
        Preco += valor;
        Preco = Math.Max(Preco, 0.01f);
    }
}