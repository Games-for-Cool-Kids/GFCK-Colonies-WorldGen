using DelaunatorSharp;

namespace UnityEngine
{
    public static class Vector2Extensions
    {
        public static IPoint ToPoint(this Vector2 point)
        {
            return new Point((double)point.x, (double)point.y);
        }
    }
}
