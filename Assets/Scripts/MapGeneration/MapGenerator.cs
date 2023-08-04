﻿using UnityEngine;
using DelaunatorSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MapGeneration.Flags;
using MapGeneration.Data;
using MapGeneration.Biomes;

namespace MapGeneration
{
	public class MapGenerator : MonoBehaviour
	{
		public Delaunator delaunator = null;

		public MapPointsSampler pointsSampler = null;
		public List<IPoint> points = new();

		public float maxRelaxation = 0.1f;

		public MapData Map = new();

		public delegate void MapGeneratorEventHandler(object sender);
		public event MapGeneratorEventHandler Cleared;
		public event MapGeneratorEventHandler VoronoiGenerated;
		public event MapGeneratorEventHandler VoronoiUpdated;
		public event MapGeneratorEventHandler BiomesGenerated;
		public event MapGeneratorEventHandler BiomesUpdated;

		public async void GenerateAsync(GenerationType generationType)
		{
			if (generationType == GenerationType.Clean)
				Clear();

			await Task.Run(() =>
			{
				GenerateInternal(generationType);
			});

			if (generationType == GenerationType.Clean)
			{
				VoronoiGenerated?.Invoke(this);
				BiomesGenerated?.Invoke(this);
			}
			else if (generationType == GenerationType.Update)
			{
				VoronoiUpdated?.Invoke(this);
				BiomesUpdated?.Invoke(this);
			}
		}
		private void GenerateInternal(GenerationType generationType)
		{
			if (generationType == GenerationType.Clean)
				SamplePoints();

			CreateDelauneyVoronoi();

			GenerateBiomesData();
		}

		public void Clear()
		{
			delaunator = null;
			points.Clear();

			Cleared?.Invoke(this);
		}

		public void RelaxPoints()
		{
			if (delaunator == null)
				return;

			points = delaunator.GetRelaxedPoints(maxRelaxation).ToList();

			CreateDelauneyVoronoi();
			GenerateBiomesData();

			VoronoiUpdated?.Invoke(this);
			BiomesUpdated?.Invoke(this);
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

		private async void GenerateBiomesData()
		{
			Map.Biomes = BiomeLogic.CreateBiomeDataFrom(delaunator).ToArray();
			BiomesGenerated?.Invoke(this);
		}

	}
}

