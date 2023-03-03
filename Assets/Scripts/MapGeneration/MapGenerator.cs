using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MapGeneration.Flags;

public class MapGenerator : MonoBehaviour
{
    public Delaunator delaunator = null;

    public MapPointsSampler pointsSampler = null;
    public List<IPoint> points = new();

    public delegate void MapGeneratorEventHandler(object sender);
    public event MapGeneratorEventHandler Cleared;
    public event MapGeneratorEventHandler VoronoiGenerated;
    public event MapGeneratorEventHandler VoronoiUpdated;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Clear();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Clear();
            GenerateAsync();
        }

        if (Input.GetMouseButtonDown(0))
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            points.Add(new Point(target.x, target.y));

            GenerateAsync();
        }
    }

    public async void GenerateAsync()
    {
        var generationType = points.Count == 0 
            ? GenerationType.Clean 
            : GenerationType.Update;


        await Task.Run(() =>
        {
            GenerateInternal(generationType);
        });


        if (generationType == GenerationType.Clean)
            VoronoiGenerated?.Invoke(this);
        else if (generationType == GenerationType.Update)
            VoronoiUpdated?.Invoke(this);
    }
    private void GenerateInternal(MapGeneration.Flags.GenerationType generationType)
    {
        if (generationType == MapGeneration.Flags.GenerationType.Clean)
            SamplePoints();

        CreateDelauneyVoronoi();
    }

    public void Clear()
    {
        delaunator = null;
        points.Clear();

        Cleared?.Invoke(this);
    }

    public void RelaxPoints()
    {
        points = delaunator.GetRellaxedPoints().ToList();

        CreateDelauneyVoronoi();
    }


    // Private Functions
    private void SamplePoints()
    {
        points = pointsSampler.Sample();
        Debug.Log($"Generated Points Count {points.Count}");
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

}
