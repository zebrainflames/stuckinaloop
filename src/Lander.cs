using System.Linq;
using Godot;

namespace stuckinaloop
{
    public class Lander : KinematicBody2D
    {

        [Export] public float RotationSpeed = 3.5f;
        [Export] public float ThrusterPower = 100.0f;
        
        public Vector2 Gravity { get; set; }
        public bool Grounded { get; private set; }
        
        private float inputRotation;
        private float inputThrust;
        
        //private Vector2 acceleration = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        
        private const float Friction = 0.87f;

        private CollisionPolygon2D legs;
        private CollisionShape2D body;
        
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
    
        public override void _Ready()
        {
            legs = GetNode<CollisionPolygon2D>("LegCollider");
            body = GetNode<CollisionShape2D>("BodyCollider");
            
            // Connect signals for collisions...

        }

        public override void _Process(float delta)
        {
            ProcessInput();
        }

        public override void _PhysicsProcess(float delta)
        {
            //Rotate lander
            Rotate(inputRotation * RotationSpeed * delta);


            var acceleration = -Gravity;
            
            // Apply thrust
            //velocity += Transform.y * ThrusterPower * -inputThrust;
            acceleration += Transform.y * ThrusterPower * -inputThrust;
            
            velocity += acceleration * delta;
            velocity = MoveAndSlide(velocity);

            Grounded = false;
            var slideCount = GetSlideCount();
            if (slideCount > 0)
            {
                velocity *= Friction;

                //var col = GetSlideCollision(0);
                Grounded = true;
                //GD.Print($"{body} {legs} {col}");
            }
            // TODO: react to collisions?
        }

        public void ResetToPos(Vector2 pos)
        {
            velocity = Vector2.Zero;
            Rotation = 0f;
            inputRotation = 0f;
            inputThrust = 0f;
            Position = pos;
        }
    }
}
