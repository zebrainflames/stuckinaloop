using Godot;

namespace stuckinaloop
{
    public class Intro : Node2D
    {
        public override void _Ready()
        {
            OS.SetWindowTitle("Orbital Rescue");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey inputEvent && inputEvent.Pressed)
            {
                var ps = ResourceLoader.Load<PackedScene>("res://scenes/Main.tscn");
                GetTree().ChangeSceneTo(ps);
            }
        }
    }
}
