using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace stuckinaloop
{
    public class LevelBase : Node2D
    {
        public bool LevelComplete { get; set; } 
        public Vector2 StartPos { get; private set; }
        
        private List<Planet> planets;
        private List<Astronaut> astronauts;

        private int astronautCount;
        private int collected;
        
        // TODO: when is _Ready() run when a level is instanced on the fly?
        public override void _Ready()
        {
            Initialize();
        }

        public void Initialize()
        {
            planets = new Array<Planet>(GetNode<Node2D>("planets").GetChildren()).ToList();
            astronauts = new Array<Astronaut>(GetNode<Node2D>("astronauts").GetChildren()).ToList();
            astronautCount = astronauts.Count;
            collected = 0;
            StartPos = GetNode<Position2D>("StartPos").Position;
        }
        
        public Vector2 GetGravity(Vector2 position)
        {
            var gravity = Vector2.Zero;
            foreach (var planet in planets)
            {
                gravity += planet.GravityForPosition(position);
            }

            return -gravity;
        }

        public override void _PhysicsProcess(float delta)
        {
            foreach (var a in astronauts.Where(a => a.Rescued))
            {
                collected++;
                a.QueueFree();
            }
            astronauts.RemoveAll(a => a.Rescued);
            
            
            if (astronautCount <= collected)
            {
                LevelComplete = true;
            }
        }
    }
}