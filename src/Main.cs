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

        private Lander lander;
        // currently active level..
        private LevelBase level;
        private LevelLoader ll;
        private GameState state = GameState.Playing;
        //TODO: figure out why timers didn't work
        private Timer groundTimer;
        private float groundTime = 1.0f;
        
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // TODO: proper level loading..?
            lander = GetNode<Lander>("Lander");
            groundTimer = GetNode<Timer>("GroundTimer");
            
            ll = new LevelLoader();
            

            var lvl = ll.FirstLevel();
            GetNode<Node2D>("Levels").AddChild(lvl);
            level = lvl;
            lander.ResetToPos(level.StartPos);
        }

        private void ChangeLevel()
        {
            // TODO: how to cleanup previous level..?
            level.QueueFree();
            var lvl = ll.NextLevel();
            AddChild(lvl);
            level = lvl;
            lander.ResetToPos(level.StartPos);
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
        }

        public void GroundTimerTimeout()
        {
            
        }
    }
}
