namespace stuckinaloop
{
    public class LargePlanet : Planet
    {
        public override void _Ready()
        {
            Size = PlanetSize.Large;
            base._Ready();
        }

    }
}
