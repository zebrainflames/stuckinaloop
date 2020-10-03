using Godot;

namespace stuckinaloop
{
    public class Lander : KinematicBody2D
    {

        [Export] public float RotationSpeed = 3.5f;
        [Export] public float ThrusterPower = 3.5f;
        
        public Vector2 Gravity { get; set; }
        
        private float inputRotation;
        private float inputThrust;
        
        //private Vector2 acceleration = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        
        private void ProcessInput()
        {
            inputRotation = 0.0f;
            inputThrust = 0.0f;
            if (Input.IsActionPressed("left"))
            {
                inputRotation -= 1.0f;
            }

            if (Input.IsActionPressed("right"))
            {
                inputRotation += 1.0f;
            }

            if (Input.IsActionPressed("thrust"))
            {
                inputThrust += 1.0f;
            }
        }
    
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        
        }

        public override void _Process(float delta)
        {
            ProcessInput();
        }

        public override void _PhysicsProcess(float delta)
        {
            //Rotate lander
            Rotate(inputRotation * RotationSpeed * delta);
            
            // TODO: apply gravity to total velocity
            velocity -= Gravity * delta;
            
            // Apply thrust
            velocity += Transform.y * ThrusterPower * -inputThrust;
            
            
            velocity = MoveAndSlide(velocity);

            // TODO: react to collisions?
        }
    }
}
