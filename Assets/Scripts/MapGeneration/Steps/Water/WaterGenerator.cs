using MapGeneration.Data;
using UnityEngine;

namespace MapGeneration.Steps.Water
{
	public abstract class WaterGenerator : ScriptableObject
	{
		public abstract void Generate(MapData map);
	}
}
