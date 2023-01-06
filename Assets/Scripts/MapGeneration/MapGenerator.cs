using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public Delaunator delaunator = null;

    public MapPointsSampler pointsSampler = null;
    public List<IPoint> points = new();

    public void Generate()
    {
        if (points.Count == 0)
        {
            points = pointsSampler.Sample();
            Debug.Log($"Generated Points Count {points.Count}");
        }

        CreateDelauneyVoronoi();
    }

    private void CreateDelauneyVoronoi()
    {
        if (points.Count < 3)
        {
            Debug.LogError("Not enough points given.");
            return;
        }

        delaunator = new Delaunator(points.ToArray());
    }

    public void Clear()
    {
        delaunator = null;
        points.Clear();
    }
}
