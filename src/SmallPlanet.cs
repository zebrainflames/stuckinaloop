namespace stuckinaloop
{
    public class SmallPlanet : Planet
    {
        public override void _Ready()
        {
            Size = PlanetSize.Small;
            base._Ready();
        }

    }
}
