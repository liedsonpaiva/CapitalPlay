using Godot;
using System;

public partial class Camadas : Sprite2D
{
    public override void _Process(double delta)
    {
        Vector2 target = GetGlobalMousePosition();
        Position = Position.MoveToward(target, 200 * (float)delta);
    }
}
