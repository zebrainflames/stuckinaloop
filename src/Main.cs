using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace stuckinaloop
{
    public enum GameState
    {
        Paused, 
        Playing,
        Won,
        Lost,
    }
    
    public class Main : Node2D
    {
        // Game state objects...
        private Lander lander;
        private LevelBase level;
        private ProgressBar fuelBar;
            
        // logic
        private LevelLoader ll;
        private GameState state = GameState.Playing;
        //TODO: figure out why timers didn't work
        private Timer groundTimer;
        private float groundTime = 1.0f;
        
        public override void _Ready()
        {
            // TODO: proper level loading..?
            lander = GetNode<Lander>("Lander");
            groundTimer = GetNode<Timer>("GroundTimer");
            fuelBar = GetNode<ProgressBar>("UI/FuelBar");
            
            ll = new LevelLoader();
            var lvl = ll.FirstLevel();
            SetCurrentLevel(lvl);
        }

        private void SetCurrentLevel(LevelBase lvl)
        {
            GetNode<Node2D>("Levels").AddChild(lvl);
            level = lvl;
            lander.ResetToPos(level.StartPos);
            fuelBar.Value = level.StartingFuel;
            lander.Fuel = level.StartingFuel;
        }
        
        private void ChangeLevel()
        {
            // TODO: how to cleanup previous level..?
            level.QueueFree();
            var lvl = ll.NextLevel();
            SetCurrentLevel(lvl);
        }
        
        public override void _PhysicsProcess(float delta)
        {
            if (level.LevelComplete && lander.Grounded)
            {
                groundTime -= delta;
                if (ll.LevelsLeft() && groundTime < 0f)
                {
                    ChangeLevel();
                    groundTime = 1.0f;
                    return;
                }
                state = GameState.Won;
                
                // TODO: load some sort of final screen here

            }
            lander.Gravity = level.GetGravity(lander.Position);
            
            // UI updates
            fuelBar.Value = lander.Fuel;
        }

        public void GroundTimerTimeout()
        {
            
        }
    }
}
