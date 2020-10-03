using Godot;

namespace stuckinaloop
{
    public static class PlanetExtensions
    {
        private const float PixelsPerUnit = 64f;
        
        public static float SizeToMass(this PlanetSize size, float density)
        {
            switch (size)
            {
                // TODO: Planet sizes (floats) should be defined somewhere else
                case PlanetSize.Small:
                    return Mass(Area(128f / PixelsPerUnit));
                case PlanetSize.Medium:
                    return Mass(Area(192f / PixelsPerUnit));
                case PlanetSize.Large:
                    return Mass(Area(244f / PixelsPerUnit));
                default:
                    return 0f;
            }

            float Area(float radius) => radius * radius * Mathf.Pi;
            float Mass(float area) => area * density;
        }
    }
    
    public enum PlanetSize
    {
        Small,
        Medium,
        Large
        
    }
    
    
    
    public class Planet : StaticBody2D
    {
        protected PlanetSize Size { get; set; }

        protected float Density { get; set; } = 1.0f;
        
        public override void _Ready()
        {
                
        }

        public Vector2 GravityForPosition(Vector2 pos)
        {
            var dir = (pos - this.Position);
            var r2 = dir.Length();
            var mass = Size.SizeToMass(Density);
            
            // 2d gravity: F = -2G * ( m1 * m2) / r
            // Assume m2 = 1 for simplicity
            // Gravitational constant G to scale to suitable ranges
            var G = 0.5f;
            var mag = (-2 * G * mass) / r2;
            return dir * mag;
        }
    }
    
}