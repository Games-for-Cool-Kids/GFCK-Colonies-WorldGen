using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity;
using DelaunatorSharp.Unity.Extensions;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "MapGeneration/RandomRectMapPointsSampler")]
public class RandomRectMapPointsSampler : MapPointsSampler
{
    public Vector2 size = Vector2.one;
    public int points = 100;

    public override List<IPoint> Sample()
    {
        List<Vector2> pointList = new();
        for(int i = 0; i < points; i++)
        {
            var p = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
            p = p * size - size / 2;
            pointList.Add(p);
        }

        return pointList.ToPoints().ToList();
    }
}
