using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunatorSharp
{
    public static class DelaunatorExtensions
    {
        public static IPoint[] GetRelaxedPoints(this Delaunator delaunator, float maxRelaxation)
        {
            List<IPoint> result = new List<IPoint>();

            foreach(var linkedPoint in delaunator.GetLinkedPoints())
            {
                var originalPoint = linkedPoint.Item1.ToVector2();
                var centroid = linkedPoint.Item2.ToVector2();

                var move = (centroid - originalPoint).normalized;
                float moveDist = Vector2.Distance(originalPoint, centroid);

                if (moveDist > maxRelaxation)
                    moveDist = maxRelaxation;

                var newP = originalPoint + move * moveDist;

                result.Add(newP.ToPoint());
            }

            return result.ToArray();
        }

        static private List<Tuple<IPoint, IPoint>> GetLinkedPoints(this Delaunator delaunator)
        {
            var result = new List<Tuple<IPoint, IPoint>>();

            var openPoints = delaunator.Points.ToList();
            var openCells = delaunator.GetVoronoiCellsBasedOnCircumcenters().ToList();
            var closedCells = delaunator.GetVoronoiCellsBasedOnCircumcenters().ToList();

            // Link originalPoint closest inside voronoi cell to centroid.
            foreach (var cell in openCells)
            {
                var pointsInCell = new List<IPoint>();
                foreach (var point in openPoints)
                    if (IsInsideCell(cell, point))
                        pointsInCell.Add(point);

                if (pointsInCell.Count() == 0)
                    continue;

                var centroid = Delaunator.GetCentroid(cell.Points);

                IPoint closestPoint = pointsInCell.First();
                foreach (var point in pointsInCell)
                    if (point.DistanceTo(centroid) < closestPoint.DistanceTo(centroid))
                        closestPoint = point;

                result.Add(new Tuple<IPoint, IPoint>(closestPoint, centroid));
                openPoints.Remove(closestPoint);
                closedCells.Add(cell);
            }

            foreach (var closedCell in closedCells)
                openCells.Remove(closedCell);

            // Any leftover openPoints are linked to closest centroid.
            foreach (var cell in openCells)
            {
                var centroid = Delaunator.GetCentroid(cell.Points);

                IPoint closestPoint = openPoints.First();
                foreach (var point in openPoints)
                    if (point.DistanceTo(centroid) < closestPoint.DistanceTo(centroid))
                        closestPoint = point;

                result.Add(new Tuple<IPoint, IPoint>(closestPoint, centroid));
                openPoints.Remove(closestPoint);
            }

            return result;
        }

        public static bool IsInsideCell(IVoronoiCell cell, IPoint point)
        {
            foreach (var triangle in GetTriangles(cell))
                if (PointInTriangle(point, triangle))
                    return true;

            return false;
        }

        public static bool PointInTriangle(IPoint p, ITriangle triangle)
        {
            var p0 = triangle.Points.ElementAt(0);
            var p1 = triangle.Points.ElementAt(1);
            var p2 = triangle.Points.ElementAt(2);

            var s = (p0.X - p2.X) * (p.Y - p2.Y) - (p0.Y - p2.Y) * (p.X - p2.X);
            var t = (p1.X - p0.X) * (p.Y - p0.Y) - (p1.Y - p0.Y) * (p.X - p0.X);

            if ((s < 0) != (t < 0) && s != 0 && t != 0)
                return false;

            var d = (p2.X - p1.X) * (p.Y - p1.Y) - (p2.Y - p1.Y) * (p.X - p1.X);
            return d == 0 || (d < 0) == (s + t <= 0);
        }

        public static List<ITriangle> GetTriangles(IVoronoiCell cell)
        {
            var triangles = new List<ITriangle>();

            var centroid = Delaunator.GetCentroid(cell.Points);
            for (int i = 0; i < cell.Points.Count(); i++)
            {
                var p1 = cell.Points.ElementAt(i);
                var p2 = cell.Points.ElementAt((i + 1) % cell.Points.Count());

                triangles.Add(new Triangle(triangles.Count, new List<IPoint>()
                {
                    p1, p2, centroid
                }));
            }

            return triangles;
        }
    }
}
