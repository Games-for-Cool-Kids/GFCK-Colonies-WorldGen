﻿using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "MapGeneration/PointsSampling/UniformPoissonRectMapPointsSampler")]
    public class UniformPoissonRectMapPointsSampler : MapPointsSampler
    {
        public Vector2 size = Vector2.one;
        public float minDistance = .2f;
        public int pointsPerIteration = 100;

        public override List<IPoint> Sample()
        {
            var sampler = MapGeneration.UniformPoissonDiskSampler.SampleRectangle(-size / 2.0f, size / 2.0f, minDistance, pointsPerIteration);
            return sampler.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();
        }
    }
}