using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace stuckinaloop
{
    public enum GameState
    {
        Playing,
        Won,
        OutOfFuel,
    }
    
    public class Main : Node2D
    {
        // Game state objects...
        private Lander lander;
        private LevelBase level;
        private GameState state = GameState.Playing;
        private LevelLoader ll;
        //TODO: figure out why timers didn't work
        private Timer groundTimer;
        private float groundTime = 1.0f;
        
        //ui
        private ProgressBar fuelBar;
        private RichTextLabel restartText;
        private Camera2D camera;

        
        public override void _Ready()
        {
            // TODO: proper level loading..?
            lander = GetNode<Lander>("Lander");
            groundTimer = GetNode<Timer>("GroundTimer");
            fuelBar = GetNode<ProgressBar>("UI/FuelBar");
            restartText = GetNode<RichTextLabel>("UI/ResetText");
            restartText.Hide();
            camera = GetNode<Camera2D>("Camera");
            
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

        private void RestartLevel()
        {
            //TODO
            level.QueueFree();
            state = GameState.Playing;
            restartText.Hide();
            SetCurrentLevel(ll.CurrentLevel());
        }
        
        private void ChangeLevel()
        {
            // TODO: how to cleanup previous level..?
            level.QueueFree();
            var lvl = ll.NextLevel();
            SetCurrentLevel(lvl);
        }

        public override void _Process(float delta)
        {
            if (state == GameState.OutOfFuel)
            {
                restartText.Show();
                if (Input.IsActionPressed("restart"))
                {
                    RestartLevel();
                }
            }

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            // calculate rect to cover all necessary objects (assume player and center screen is enough)
            var playerPos = lander.Position;
            var viewRect = GetViewportRect();
            var center = viewRect.Size / 2f;
            //TODO: this paddingg only works to positive axis directions
            const float padding = 1.1f;
            playerPos *= padding;
            
            if (viewRect.HasPoint(playerPos))
            {
                camera.Zoom = Vector2.One;
                return;
            }

            
            var dir = (playerPos - center);
            var maxCoord = Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            var factor = maxCoord / center.x; // assuming square viewport
            camera.Zoom = new Vector2(factor, factor);
        }

        // TODO: refactor state handling, this is rather horrible
        public override void _PhysicsProcess(float delta)
        {
            if (!level.LevelComplete && lander.Fuel <= 0.0f)
            {
                state = GameState.OutOfFuel;
            }
            
            
            
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
