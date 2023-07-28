using UnityEngine;
using System.Collections.Generic;

namespace MapGeneration.Data
{
	public class BiomeData
	{
		public int Id { get; set; }

		public Vector2 Center { get; set; }
		public Vector2[] Points { get; set; }

		public BiomeType Type { get; set; }

		public BiomeData[] Neighbors { get; set; }
	}
}
