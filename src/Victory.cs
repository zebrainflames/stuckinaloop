using Godot;

namespace stuckinaloop
{
    public class Victory : Node2D
    {
        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("restart"))
            {
                Reload();   
            }
        }

        private void Reload()
        {
            var ps = ResourceLoader.Load<PackedScene>("res://scenes/Main.tscn");
            GetTree().ChangeSceneTo(ps);
        }
    }
}
