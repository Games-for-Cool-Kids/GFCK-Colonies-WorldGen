using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "MapGeneration/UniformPoissonCircleMapPointsSampler")]
    public class UniformPoissonCircleMapPointsSampler : MapPointsSampler
    {
        public float size = 3;
        public float minDistance = .2f;

        public override List<IPoint> Sample()
        {
            var sampler = MapGeneration.UniformPoissonDiskSampler.SampleCircle(Vector2.zero, size, minDistance);
            return sampler.Select(point => new Vector2(point.x, point.y)).ToPoints().ToList();
        }
    }
}
