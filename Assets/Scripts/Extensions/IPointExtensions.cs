using DelaunatorSharp;

namespace UnityEngine
{
    public static class IPointExtensions
    {
        public static Vector2 ToVector2(this IPoint point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }

        public static float DistanceTo(this IPoint point1, IPoint point2)
        {
            return Vector2.Distance(point1.ToVector2(), point2.ToVector2());
        }
    }
}
