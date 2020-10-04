using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace stuckinaloop
{
    public class LevelLoader
    {
        private PackedScene currentLevel;
        
        private readonly List<string> paths = new List<string>
        {
            //"res://scenes/Level.tscn", 
            "res://scenes/Level1.tscn",
            "res://scenes/Level2.tscn",
            "res://scenes/Level3.tscn",
            "res://scenes/Level4.tscn",
            "res://scenes/Level5.tscn"
        };

        public LevelBase FirstLevel()
        {
            var ps = ResourceLoader.Load<PackedScene>("res://scenes/Level.tscn");
            currentLevel = ps;
            var lvl = (LevelBase) ps.Instance();
            return lvl;
        }

        public bool LevelsLeft()
        {
            return paths.Count > 0;
        }
        
        public LevelBase NextLevel()
        {
            var str = paths.First();
            paths.RemoveAt(0);
            var ps = ResourceLoader.Load<PackedScene>(str);
            currentLevel = ps;
            var lvl = (LevelBase) ps.Instance();
            lvl.Initialize();
            return lvl;
        }

        public LevelBase CurrentLevel()
        {
            return (LevelBase)currentLevel.Instance();
        }
    }
}