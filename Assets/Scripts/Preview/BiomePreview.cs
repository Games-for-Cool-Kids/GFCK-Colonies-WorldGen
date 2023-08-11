using UnityEngine;
using MapGeneration;
using MapGeneration.Biomes;
using System.Collections.Generic;
using MapGeneration.Data;

public partial class BiomePreview : MonoBehaviour
{
	public float edgeWidth = .01f;

	public Color borderColor = Color.black;
	public Color waterColor = Color.blue;
	public Color landColor = Color.green;

	public Material lineMaterial;

	public Material meshMaterial;
	private Dictionary<BiomeType, Material> meshMaterials;

	public bool drawBorders = true;
	public bool createMeshes = true;

	private MapGenerator generator = null;

	private Transform BorderContainer;
	private Transform MeshesContainer;


	void Start()
	{
		Clear();
	}

	private void OnEnable()
	{
		generator = FindObjectOfType<MapGenerator>();
		generator.Cleared += (s) => Clear();
		generator.BiomesGenerated += (s) => Generate();
		generator.BiomesUpdated += (s) => Generate();

		InitializeMaterials();
	}

	private void Generate()
	{
		if (generator.Map == null ||
			generator.Map.Biomes == null ||
			generator.Map.Biomes.Length == 0)
			return;

		Clear();

		CreateBorders();
		CreateMeshes();
	}

	private void Clear()
	{
		CreateNewContainers();
	}

	private void CreateBorders()
	{
		if (!drawBorders)
			return;

		foreach (var biome in generator.Map.Biomes)
		{
			int id = 0;
			foreach (var border in BiomeLogic.GetBorders(biome))
			{
				CreateLine(BorderContainer,
					$"BorderEdge - {id}",
					new Vector3[] { border.P1.ToVector3(), border.P2.ToVector3() },
					borderColor,
					edgeWidth,
					0);
				id++;
			}
		}
	}
	private void CreateLine(Transform container, string name, Vector3[] points, Color color, float width, int order = 1)
	{
		var lineGameObject = new GameObject(name);
		lineGameObject.transform.parent = container;
		var lineRenderer = lineGameObject.AddComponent<LineRenderer>();

		lineRenderer.SetPositions(points);

		lineRenderer.material = lineMaterial ?? new Material(Shader.Find("Standard"));
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		lineRenderer.sortingOrder = order;
	}

	private Material CreateMeshMaterial(Color color)
	{
		var material = new Material(meshMaterial);
		material.color = color;
		return material;
	}

	private void InitializeMaterials()
	{
		meshMaterials = new()
		{
			{ BiomeType.WATER, CreateMeshMaterial(waterColor) },
			{ BiomeType.LAND, CreateMeshMaterial(landColor) }
		};
	}

	private Material GetBiomeMaterial(BiomeType biomeType)
	{
		if(meshMaterials.ContainsKey(biomeType)) 
			return meshMaterials[biomeType];

		return new Material(Shader.Find("Standard"));
	}

	private void CreateMeshes()
	{
		if (!createMeshes)
			return;

		int id = 0;
		foreach (var biome in generator.Map.Biomes)
		{

			var mesh = new Mesh
			{
				vertices = biome.Points.ToVectors3(),
				triangles = Meshes.TriangulateIndicesConvexPolygon(biome.Points.Length)
			};

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			var biomeMesh = new GameObject($"Biome - {id}");
			biomeMesh.transform.parent = MeshesContainer.transform;

			var meshRenderer = biomeMesh.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = GetBiomeMaterial(biome.Type);

			var meshFilter = biomeMesh.AddComponent<MeshFilter>();
			meshFilter.mesh = mesh;

			id++;
		}
	}

	private void CreateNewContainers()
	{
		CreateNewBordersContainer();
		CreateNewMeshesContainer();
	}
	private void CreateNewBordersContainer()
	{
		if (BorderContainer != null)
		{
			Destroy(BorderContainer.gameObject);
			BorderContainer = null;
		}

		BorderContainer = new GameObject(nameof(BorderContainer)).transform;
		BorderContainer.transform.parent = transform;
	}
	private void CreateNewMeshesContainer()
	{
		if (MeshesContainer != null)
		{
			Destroy(MeshesContainer.gameObject);
			MeshesContainer = null;
		}

		MeshesContainer = new GameObject(nameof(MeshesContainer)).transform;
		MeshesContainer.transform.parent = transform;
	}

	//private void CenterCamera()
	//{
	//	var target = meshObject.GetComponent<Renderer>().bounds.center;
	//	var camera = GameObject.Find("Main Camera");

	//	camera.transform.position = new Vector3(target.x, target.y, camera.transform.position.z);
	//}
}
