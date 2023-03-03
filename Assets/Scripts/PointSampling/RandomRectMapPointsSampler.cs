using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Collections.Generic;
using System.Linq;


[CreateAssetMenu(menuName = "MapGeneration/RandomRectMapPointsSampler")]
public class RandomRectMapPointsSampler : MapPointsSampler
{
    public Vector2 size = Vector2.one;
    public int initialPoints = 100;

    public float minimumSqDistance = 0.002f;

    public override List<IPoint> Sample()
    {
        List<Vector2> pointList = new(initialPoints);

        AddCorners(pointList);
        GenerateRandomPoints(pointList);
        RemoveInvalidPoints(ref pointList);

        pointList = pointList.Select(p => (p * size) - size / 2.0f).ToList();

        return pointList.ToPoints().ToList();
    }

    private void AddCorners(List<Vector2> pointList)
    {
        pointList.Add(Vector2.zero);
        pointList.Add(Vector2.up);
        pointList.Add(Vector2.one);
        pointList.Add(Vector2.right);
    }

    private void GenerateRandomPoints(List<Vector2> pointList)
    {
        for (int i = 0; i < initialPoints; i++)
        {
            var p = new Vector2(StaticRandom.RandomFloat(), StaticRandom.RandomFloat());

            pointList.Add(p);
        }
    }

    private void RemoveInvalidPoints(ref List<Vector2> pointList)
    {
        for (int i = pointList.Count - 1; i >= 0; i--)
        {
            Vector2 p1 = pointList[i];

            for (int j = 0; j < pointList.Count; j++)
            {
                if (i == j)
                    continue;

                Vector2 p2 = pointList[j];

                if ((p2 - p1).sqrMagnitude < minimumSqDistance)
                {
                    pointList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
