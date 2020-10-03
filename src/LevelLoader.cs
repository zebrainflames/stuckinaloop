using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace stuckinaloop
{
    public class LevelLoader
    {
        private readonly List<string> paths = new List<string>
        {
            //"res://scenes/Level.tscn", 
            "res://scenes/Level1.tscn"
        };

        public LevelBase FirstLevel()
        {
            var ps = ResourceLoader.Load<PackedScene>("res://scenes/Level.tscn");
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
            var lvl = (LevelBase) ps.Instance();
            lvl.Initialize();
            return lvl;
        }
    }
}