using Godot;
using System;

public partial class Transicao : CanvasLayer
{
    private string _scene;

    public void FadeToScene(string newScene)
    {
        _scene = newScene;
        GetNode<AnimationPlayer>("Animation").Play("fade_out");
    }

    public void ChangeScene()
    {
        GetTree().ChangeSceneToFile(_scene);
    }
}
