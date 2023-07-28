﻿using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using MapGeneration.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.Assertions;

namespace MapGeneration.Biomes
{
	public class BiomeLogic
	{
		public static List<BiomeData> CreateBiomeDataFrom(Delaunator delaunator)
		{
			List<BiomeData> biomes = new();

			var cells = delaunator.GetVoronoiCells();

			foreach (var cell in cells)
			{
				var biome = new BiomeData();
				biome.Id = cell.Index;
				biome.Points = cell.Points.ToVectors2();
				biome.Type = BiomeType.LAND;
				biomes.Add(biome);
			}

			LinkBiomesWithNeighbors(biomes);

			return biomes;
		}

		private static void LinkBiomesWithNeighbors(List<BiomeData> biomes)
		{
			// Init
			List<Tuple<BiomeData, List<BiomeData>>> biomesWithNeighbors = new();
			foreach (var biome in biomes)
				biomesWithNeighbors.Add(new Tuple<BiomeData, List<BiomeData>>(biome, new()));

			// Link
			foreach (var biome in biomesWithNeighbors)
			{
				foreach (var otherBiome in biomesWithNeighbors)
				{
					if (biome == otherBiome)
						continue;

					if (IsNewNeighbor(biome, otherBiome))
					{
						biome.Item2.Add(otherBiome.Item1);
						otherBiome.Item2.Add(biome.Item1);
					}
				}
			}

			// Store array[]
			foreach (var biome in biomesWithNeighbors)
				biome.Item1.Neighbors = biome.Item2.ToArray();
		}

		private static bool IsNewNeighbor(
			Tuple<BiomeData, List<BiomeData>> biome,
			Tuple<BiomeData, List<BiomeData>> otherBiome)
		{
			if (biome.Item2.Contains(otherBiome.Item1))
			{
				//Assert(otherBiome.Item2.Contains(biome.Item1));
				return false;
			}

			foreach (var point in biome.Item1.Points)
				foreach (var otherPoint in otherBiome.Item1.Points)
					if (otherPoint == point)
						return true;

			return false;
		}
	}
}
