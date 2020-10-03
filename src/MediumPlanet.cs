using Godot;

namespace stuckinaloop
{
    public class MediumPlanet : Planet
    {
        public override void _Ready()
        {
            Size = PlanetSize.Medium;
            base._Ready();
        }
    }
}
