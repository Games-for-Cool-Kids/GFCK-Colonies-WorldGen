using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MapGeneration.Flags;

namespace MapGeneration
{
    public class MapGenerator : MonoBehaviour
    {
        public Delaunator delaunator = null;

        public MapPointsSampler pointsSampler = null;
        public List<IPoint> points = new();

        public delegate void MapGeneratorEventHandler(object sender);
        public event MapGeneratorEventHandler Cleared;
        public event MapGeneratorEventHandler VoronoiGenerated;
        public event MapGeneratorEventHandler VoronoiUpdated;


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

            VoronoiUpdated?.Invoke(this);
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
}

