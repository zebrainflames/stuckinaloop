using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace stuckinaloop
{
    public class Main : Node2D
    {

        private Lander lander;
        private List<Planet> planets;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // TODO: proper level loading..?
            lander = GetNode<Lander>("Lander");
            //bodies = new Array<DestructibleBody>(GetNode<Node2D>("Bodies").GetChildren());
            planets = new Array<Planet>(GetNode<Node2D>("Levels/level/planets").GetChildren()).ToList();
            GD.Print($"Got {planets.Count} planets");
        }

        public override void _PhysicsProcess(float delta)
        {
            lander.Gravity = -planets.Aggregate(new Vector2(), 
                (sum, p) => sum += p.GravityForPosition(lander.Position)
            );
            
        }
    }
}
