using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;

public abstract class MapPointsSampler : ScriptableObject
{
    public abstract List<IPoint> Sample();
}
