using Godot;

namespace stuckinaloop
{
    public class Astronaut : Area2D
    {
        public bool Rescued { get; set; } = false;
        
        private const float RotationSpeed = 2.3f;
        private float rotationSpeedScale;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // TODO: randomize
            rotationSpeedScale = 1.0f;
            
            this.Connect("body_entered", this, nameof(OnBodyEntered));
        }

        public override void _Process(float delta)
        {
            Rotation += RotationSpeed * rotationSpeedScale * delta;
        }

        //
        public void OnBodyEntered(PhysicsBody2D body)
        {
            //GD.Print($"{body}");
            Rescued = true;
        }
    }
}
