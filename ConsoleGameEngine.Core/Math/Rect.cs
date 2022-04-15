namespace ConsoleGameEngine.Core.Math
{
    public readonly struct Rect
    {
        public Vector Position { get; }
        public Vector Size { get; }
        public Vector Center => Position.Rounded + Size * 0.5f;

        public Rect(Vector position, Vector size)
        {
            Position = position;
            Size = size;
        }

        public static bool Intersect(Rect a, Rect b)
        {
            // If one rectangle is on left side of other
            if (a.Position.X >= b.Position.X + b.Size.X || b.Position.X >= a.Position.X + a.Size.X)
            {
                return false;
            }

            // If one rectangle is above other
            if (a.Position.Y + a.Size.Y <= b.Position.Y || b.Position.Y + b.Size.Y <= a.Position.Y)
            {
                return false;
            }
            return true;
        }

    }
}