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
        public float Fuel { get; set; }
        
        private float inputRotation;
        private float inputThrust;
        
        private Vector2 velocity = Vector2.Zero;
        private const float Friction = 0.87f;
        private const float FuelBurnRate = 100.0f / 6.0f; // -> units of fuel / seconds to burn tank 

        //TODO: remove this or enable collision detection by part
        private CollisionPolygon2D legs;
        private CollisionShape2D body;

        private AnimatedSprite thrusterAnimation;
        
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
            thrusterAnimation = GetNode<AnimatedSprite>("Animation");
        }

        public override void _Process(float delta)
        {
            ProcessInput();
            if (inputThrust > 0.0f)
            {
                thrusterAnimation.Show();
                thrusterAnimation.Play();
            }
            else
            {
                thrusterAnimation.Stop();
                thrusterAnimation.Hide();
            }
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
            
            // burn fuel
            if (inputThrust > 0.0f)
            {
                Fuel -= FuelBurnRate * delta;
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
