using System;
using Godot;
public static class RoladorDeDados
{
    private static Random random = new Random();

    public static int D6() => random.Next(1, 7);
    public static int D66() => random.Next(-6, 7);
    public static int D8() => random.Next(4, 9);
}
