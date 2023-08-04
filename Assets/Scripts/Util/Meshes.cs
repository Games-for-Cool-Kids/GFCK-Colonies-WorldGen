using UnityEngine;

public static class Meshes
{
	public static int[] TriangulateIndicesConvexPolygon(int points)
	{
		if (points < 3)
		{
			Debug.LogError($"Polygon cannot have less than 3 points.");
			return null;
		}

		int triangleCount = points - 2;
		int[] triangleIndices = new int[triangleCount * 3];

		int triangleIndex = 2;
		for (int i = 2; i < points; i++)
		{
			triangleIndices[triangleIndex - 2] = 0;
			triangleIndices[triangleIndex - 1] = i;
			triangleIndices[triangleIndex] = i - 1;

			triangleIndex += 3;
		}

		return triangleIndices;
	}
}
