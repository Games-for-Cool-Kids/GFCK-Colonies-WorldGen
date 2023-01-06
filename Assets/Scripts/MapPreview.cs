using UnityEngine;
using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;

public partial class MapPreview : MonoBehaviour
{
    public GameObject trianglePointPrefab;
    public GameObject voronoiPointPrefab;

    public float voronoiEdgeWidth = .01f;
    public float triangleEdgeWidth = .01f;
    public float hullEdgeWith = .01f;

    public Color triangleEdgeColor = Color.black;
    public Color hullColor = Color.magenta;
    public Color voronoiColor = Color.white;

    public Material meshMaterial;
    public Material lineMaterial;


    public bool drawTrianglePoints = true;
    public bool drawTriangleEdges = true;
    public bool drawVoronoiPoints = true;
    public bool drawVoronoiEdges = true;
    public bool drawHull = true;
    public bool createMesh = true;

    private MapGenerator generator = null;

    private GameObject meshObject;

    private Transform PointsContainer;
    private Transform HullContainer;
    private Transform VoronoiContainer;
    private Transform TrianglesContainer;


    private void Start()
    {
        Clear();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (generator != null)
            {
                generator.Clear();
                generator = null;
            }

            Clear();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Clear();

            generator = GetComponent<MapGenerator>();
            generator.Clear();
            generator.Generate();

            CreatePreview();

            CenterCamera();

            return;
        }

        if (Input.GetMouseButtonDown(0)
         && generator)
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            generator.points.Add(new Point(target.x, target.y));

            generator.Generate();

            CreatePreview();
        }
    }

    private void CreatePreview()
    {
        if (generator.points.Count < 3)
            return;

        Clear();

        CreateMesh();
        CreateTriangle();
        CreateHull();
        CreateVoronoi();
    }

    private void Clear()
    {
        CreateNewContainers();

        if (meshObject != null)
        {
            Destroy(meshObject);
        }
    }

    private void CreateTriangle()
    {
        if (generator.delaunator == null) return;

        generator.delaunator.ForEachTriangleEdge(edge =>
        {
            if (drawTriangleEdges)
            {
                CreateLine(TrianglesContainer, $"TriangleEdge - {edge.Index}", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, triangleEdgeColor, triangleEdgeWidth, 0);
            }

            if (drawTrianglePoints)
            {
                var pointGameObject = Instantiate(trianglePointPrefab, PointsContainer);
                pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
            }
        });
    }
    private void CreateHull()
    {
        if (!drawHull) return;
        if (generator.delaunator == null) return;

        CreateNewHullContainer();

        foreach (var edge in generator.delaunator.GetHullEdges())
        {
            CreateLine(HullContainer, $"Hull Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, hullColor, hullEdgeWith, 3);
        }
    }
    private void CreateVoronoi()
    {
        if (generator.delaunator == null) return;

        generator.delaunator.ForEachVoronoiEdge(edge =>
        {
            if (drawVoronoiEdges)
            {
                CreateLine(VoronoiContainer, $"Voronoi Edge", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, voronoiColor, voronoiEdgeWidth, 2);
            }
            if (drawVoronoiPoints)
            {
                var pointGameObject = Instantiate(voronoiPointPrefab, PointsContainer);
                pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
            }
        });
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
    private void CreateMesh()
    {
        if (!createMesh) return;

        if (meshObject != null)
        {
            Destroy(meshObject);
        }

        var mesh = new Mesh
        {
            vertices = generator.delaunator.Points.ToVectors3(),
            triangles = generator.delaunator.Triangles
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshObject = new GameObject("DelaunatorMesh");
        meshObject.transform.parent = transform;
        var meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = meshMaterial ?? new Material(Shader.Find("Standard"));
        var meshFilter = meshObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void CreateNewContainers()
    {
        CreateNewPointsContainer();
        CreateNewTrianglesContainer();
        CreateNewVoronoiContainer();
        CreateNewHullContainer();
    }
    private void CreateNewPointsContainer()
    {
        if (PointsContainer != null)
        {
            Destroy(PointsContainer.gameObject);
        }

        PointsContainer = new GameObject(nameof(PointsContainer)).transform;
        PointsContainer.transform.parent = transform;
    }
    private void CreateNewTrianglesContainer()
    {
        if (TrianglesContainer != null)
        {
            Destroy(TrianglesContainer.gameObject);
        }

        TrianglesContainer = new GameObject(nameof(TrianglesContainer)).transform;
        TrianglesContainer.transform.parent = transform;
    }
    private void CreateNewHullContainer()
    {
        if (HullContainer != null)
        {
            Destroy(HullContainer.gameObject);
        }

        HullContainer = new GameObject(nameof(HullContainer)).transform;
        HullContainer.transform.parent = transform;
    }
    private void CreateNewVoronoiContainer()
    {
        if (VoronoiContainer != null)
        {
            Destroy(VoronoiContainer.gameObject);
        }

        VoronoiContainer = new GameObject(nameof(VoronoiContainer)).transform;
        VoronoiContainer.transform.parent = transform;
    }

    private void CenterCamera()
    {
        var target = meshObject.GetComponent<Renderer>().bounds.center;
        var camera = GameObject.Find("Main Camera");

        camera.transform.position = new Vector3(target.x, target.y, camera.transform.position.z);
    }
}
