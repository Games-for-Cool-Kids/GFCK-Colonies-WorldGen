using MapGeneration.Data;
using MapGeneration.Steps.Water;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration.Steps.Water
{
	[CreateAssetMenu(menuName = "MapGeneration/Water/LakeGenerator")]
	public class LakeGenerator : WaterGenerator
	{
		public int LakeMinCount = 3;
		public int LakeMaxCount = 3;
		public int LakeMinSize = 3;
		public int LakeMaxSize = 6;

		public override void Generate(MapData map)
		{
			var count = StaticRandom.Random(LakeMinCount, LakeMaxCount);
			for (int i = 0; i < count; i++)
			{
				GenerateLake(map.Biomes);
			}
		}

		private void GenerateLake(BiomeData[] biomes)
		{
			var lakeStart = PickRandomLakeStart(biomes);
			//Debug.Log("Make water at " + lakeStart.Id);
			lakeStart.Type = BiomeType.WATER;
			if (lakeStart == null)
				return;

			List<BiomeData> lake = new List<BiomeData> { lakeStart };

			var size = StaticRandom.Random(LakeMinSize, LakeMaxSize);
			for (int i = 0; i < size; i++)
			{
				var nextLakeSeed = StaticRandom.Random(0, lake.Count);
				var randomNeighbor = PickRandomLandNeighbor(lake[nextLakeSeed]);
				if (randomNeighbor != null)
				{
					//Debug.Log("Make water at " + randomNeighbor.Id);
					randomNeighbor.Type = BiomeType.WATER;
					lake.Add(randomNeighbor);
				}
			}
		}

		private BiomeData PickRandomLakeStart(BiomeData[] biomes)
		{
			int maxTries = 10;
			int tries = 0;
			while (tries < maxTries)
			{
				var randIndex = StaticRandom.Random(0, biomes.Length);
				var randBiome = biomes[randIndex];

				if (!NeighborsWater(randBiome))
					return randBiome;

				++tries;
			}
			return null;
		}

		private BiomeData PickRandomLandNeighbor(BiomeData biome)
		{
			int maxTries = 10;
			int tries = 0;
			while (tries < maxTries)
			{
				var randIndex = StaticRandom.Random(0, biome.Neighbors.Length);
				var randNeighbor = biome.Neighbors[randIndex];

				if (randNeighbor.Type != BiomeType.WATER)
					return randNeighbor;

				++tries;
			}
			return null;
		}

		private bool NeighborsWater(BiomeData biome)
		{
			foreach (var neighbor in biome.Neighbors)
				if (neighbor.Type == BiomeType.WATER)
					return true;

			return false;
		}
	}
}
