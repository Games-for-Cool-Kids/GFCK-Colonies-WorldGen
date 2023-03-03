using UnityEngine;
using MapGeneration;
using DelaunatorSharp;

public class MapGeneratorController : MonoBehaviour
{
    private MapGenerator generator = null;

    private void OnEnable()
    {
        generator = FindObjectOfType<MapGenerator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            generator.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            generator.Clear();
            generator.GenerateAsync();
        }

        if (Input.GetMouseButtonDown(0))
        {
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            generator.points.Add(new Point(target.x, target.y));

            generator.GenerateAsync();
        }
    }
}
