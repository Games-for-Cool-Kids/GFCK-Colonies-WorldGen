using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;

namespace MapGeneration
{
    public abstract class MapPointsSampler : ScriptableObject
    {
        public abstract List<IPoint> Sample();
    }
}
