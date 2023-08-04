using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class UnityMathExtensions
{
	public static Vector3 ToVector3(this Vector2 point) => new Vector3(point.x, point.y);
	public static Vector3[] ToVectors3(this IEnumerable<Vector2> points) => points.Select(point => point.ToVector3()).ToArray();
}
