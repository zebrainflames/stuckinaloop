using Godot;

namespace stuckinaloop
{
    public class Astronaut : Area2D
    {
        public Planet ClosestPlanet { get; set; }
        public bool Rescued { get; set; } = false;
        
        private const float RotationSpeed = 2.3f;
        private float rotationSpeedScale;

        // TODO: randomize
        private float orbitalSpeed = 0.001f;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // TODO: randomize
            rotationSpeedScale = 1.0f;
            
            Connect("body_entered", this, nameof(OnBodyEntered));
        }

        
        
        public override void _Process(float delta)
        {
            Rotation += RotationSpeed * rotationSpeedScale * delta;
            
            var point = ClosestPlanet.Position;
            var orbitAngle = (point).AngleTo(point);
            orbitAngle += orbitalSpeed;
            Position = point + (Position - point).Rotated(orbitAngle);
        }

        //
        public void OnBodyEntered(PhysicsBody2D body)
        {
            Rescued = true;
        }
    }
}
