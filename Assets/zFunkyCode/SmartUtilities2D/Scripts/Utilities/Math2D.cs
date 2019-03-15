using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Math2D {

	public static Rect GetBounds(List<Vector2D> pointsList)
	{
		double rMinX = 1e+10f;
		double rMinY = 1e+10f;
		double rMaxX = -1e+10f;
		double rMaxY = -1e+10f;

		foreach (Vector2D id in pointsList) {
			rMinX = System.Math.Min (rMinX, id.x);
			rMinY = System.Math.Min (rMinY, id.y);
			rMaxX = System.Math.Max (rMaxX, id.x);
			rMaxY = System.Math.Max (rMaxY, id.y);
		}

		return(new Rect((float)rMinX, (float)rMinY, (float)System.Math.Abs(rMinX - rMaxX), (float)System.Math.Abs(rMinY - rMaxY))); 
	}

	public static Rect GetBounds(Pair2D pair)
	{
		double rMinX = 1e+10f;
		double rMinY = 1e+10f;
		double rMaxX = -1e+10f;
		double rMaxY = -1e+10f;

		Vector2D id = pair.A;
		rMinX = System.Math.Min (rMinX, id.x);
		rMinY = System.Math.Min (rMinY, id.y);
		rMaxX = System.Math.Max (rMaxX, id.x);
		rMaxY = System.Math.Max (rMaxY, id.y);
	
		id = pair.B;
		rMinX = System.Math.Min (rMinX, id.x);
		rMinY = System.Math.Min (rMinY, id.y);
		rMaxX = System.Math.Max (rMaxX, id.x);
		rMaxY = System.Math.Max (rMaxY, id.y);

		return(new Rect((float)rMinX, (float)rMinY, (float)System.Math.Abs(rMinX - rMaxX), (float)System.Math.Abs(rMinY - rMaxY))); 
	}


	public static bool PolyInPoly(Polygon2D polyA, Polygon2D polyB)
	{
		foreach (Pair2D p in Pair2D.GetList(polyB.pointsList)) {
			if (PointInPoly (p.A, polyA) == false) {
				return(false);
			}
		}

		if (PolyIntersectPoly (polyA, polyB) == true) {
			return(false);
		}
		
		return(true);
	}

	// Is it not finished?
	public static bool PolyCollidePoly(Polygon2D polyA, Polygon2D polyB)
	{
		if (PolyIntersectPoly (polyA, polyB) == true) {
			return(true);
		}

		if (PolyInPoly (polyA, polyB) == true) {
			return(true);
		}

		if (PolyInPoly (polyB, polyA) == true) {
			return(true);
		}
		
		return(false);
	}

	public static bool PolyIntersectPoly(Polygon2D polyA, Polygon2D polyB)
	{
		foreach (Pair2D a in Pair2D.GetList(polyA.pointsList)) {
			foreach (Pair2D b in Pair2D.GetList(polyB.pointsList)) {
				if (LineIntersectLine (a, b)) {
					return(true);
				}
			}
		}

		return(false);
	}

	public static bool SliceIntersectPoly(List <Vector2D> slice, Polygon2D poly)
	{
		Pair2D pairA = new Pair2D(null,  null);
		foreach (Vector2D pointA in slice) {
			pairA.B = pointA;
			
			if (pairA.A != null && pairA.B != null) {
				Pair2D pairB = new Pair2D(new Vector2D(poly.pointsList.Last()),  null);
				foreach (Vector2D pointB in poly.pointsList) {
					pairB.B = pointB;

					if (LineIntersectLine (pairA, pairB)) {
						return(true);
					}

					pairB.A = pointB;
				}
			}

			pairA.A = pointA;
		}

		return(false);
	}

	public static bool SliceIntersectSlice(List <Vector2D> sliceA, List <Vector2D> sliceB)
	{
		Pair2D pairA = new Pair2D(null,  null);
		foreach (Vector2D pointA in sliceA) {
			pairA.B = pointA;

			if (pairA.A != null && pairA.B != null) {

				Pair2D pairB = new Pair2D(null,  null);
				foreach (Vector2D pointB in sliceB) {
					pairB.B = pointB;

					if (pairB.A != null && pairB.B != null) {
						if (LineIntersectLine (pairA, pairB)) {
							return(true);
						}
					}

					pairB.A = pointB;
				}
			}

			pairA.A = pointA;
		}

		return(false);
	}
		
	public static bool LineIntersectPoly(Pair2D line, Polygon2D poly)
	{
		Pair2D pair = new Pair2D(new Vector2D(poly.pointsList.Last()), new Vector2D(Vector2.zero));
		foreach (Vector2D point in poly.pointsList) {
			pair.B = point;

			if (LineIntersectLine (line, pair)) {
				return(true);
			}
			
			pair.A = point;
		}
		
		return(false);
	}

	public static bool LineIntersectLine(Pair2D lineA, Pair2D lineB)
	{
		if (GetPointLineIntersectLine (lineA, lineB) != null) {
			return(true);
		}

		return(false);
	}

	public static bool SliceIntersectItself(List<Vector2D> slice)
	{
		Pair2D pairA = new Pair2D(null,  null);
		foreach (Vector2D va in slice) {
			pairA.B = va;

			if (pairA.A != null && pairA.B != null) {

				Pair2D pairB = new Pair2D(null,  null);
				foreach (Vector2D vb in slice) {
					pairB.B = vb;

					if (pairB.A != null && pairB.B != null) {
						if (GetPointLineIntersectLine (pairA, pairB) != null) {
							if (pairA.A != pairB.A && pairA.B != pairB.B && pairA.A != pairB.B && pairA.B != pairB.A) {
								return(true);
							}
						}
					}
					pairB.A = vb;
				}
			}
			
			pairA.A = va;
		}
		
		return(false);
	}

	public static Vector2D GetPointLineIntersectLine(Pair2D lineA, Pair2D lineB)
	{
		double ay_cy, ax_cx, px, py;
		double dx_cx = lineB.B.x - lineB.A.x;
		double dy_cy = lineB.B.y - lineB.A.y;
		double bx_ax = lineA.B.x - lineA.A.x;
		double by_ay = lineA.B.y - lineA.A.y;
		double de = bx_ax * dy_cy - by_ay * dx_cx;
		double tor = 1E-10;

		if (System.Math.Abs(de) < 0.0001d) {
			return(null);
		}	

		if (de > - tor && de < tor) {
			return(null);
		}

		ax_cx = lineA.A.x - lineB.A.x;
		ay_cy = lineA.A.y - lineB.A.y;

		double r = (ay_cy * dx_cx - ax_cx * dy_cy) / de;
		double s = (ay_cy * bx_ax - ax_cx * by_ay) / de;

		if ((r < 0) || (r > 1) || (s < 0)|| (s > 1)) {
			return(null);
		}

		px = lineA.A.x + r * bx_ax;
		py = lineA.A.y + r * by_ay;

		return(new Vector2D (px, py));
	}

	public static bool PointInPoly(Vector2D point, Polygon2D poly)
	{
		if (poly.pointsList.Count < 3) {
			return(false);
		}

		int total = 0;
		int diff = 0;

		Pair2D id = new Pair2D(new Vector2D(poly.pointsList.Last()), null);
		foreach (Vector2D p in poly.pointsList) {
			id.B = p;

			diff = (GetQuad (point, id.A) - GetQuad (point, id.B));

			switch (diff) {
				case -2: case 2:
					if ((id.B.x - (((id.B.y - point.y) * (id.A.x - id.B.x)) / (id.A.y - id.B.y))) < point.x)
						diff = -diff;

					break;

				case 3:
					diff = -1;
					break;

				case -3:
					diff = 1;
					break;

				default:
					break;   
			}

			total += diff;

			id.A = id.B;
		}

		return(Mathf.Abs(total) == 4);
	}

	private static int GetQuad(Vector2D axis, Vector2D vert)
	{
		if (vert.x < axis.x) {
			if (vert.y < axis.y) {
				return(1);
			}
			return(4);
		}
		if (vert.y < axis.y) {
			return(2);
		}
		return(3);
	}
		
	// Getting List is Slower
	public static List <Vector2D> GetListLineIntersectPoly(Pair2D line, Polygon2D poly)
	{
		List <Vector2D> result = new List <Vector2D>() ;

		Pair2D pair = new Pair2D(new Vector2D(poly.pointsList.Last()),  null);
		foreach (Vector2D point in poly.pointsList) {
			pair.B = point;

			Vector2D intersection = GetPointLineIntersectLine (line, pair);
			if (intersection != null) {
				result.Add(intersection);
			}

			pair.A = point;
		}
		return(result);
	}

	public static List<Vector2D> GetListLineIntersectSlice(Pair2D pair, List<Vector2D> slice)
	{
		List<Vector2D> resultList = new List<Vector2D> ();
		
		Pair2D id = new Pair2D(null,  null);
		foreach (Vector2D point in slice) {
			id.B = point;

			if (id.A != null && id.B != null) {
				Vector2D result = GetPointLineIntersectLine(id, pair);
				if (result != null) {
					resultList.Add(result);
				}
			}

			id.A = point;
		}
		return(resultList);
	}

	public static Vector2 ReflectAngle(Vector2 v, float wallAngle)
	{
		//normal vector to the wall
		Vector2 n = new Vector2(Mathf.Cos(wallAngle + Mathf.PI / 2), Mathf.Sin(wallAngle + Mathf.PI / 2));

		// p is the projection of V onto the normal
		float dotproduct = v.x * n.x + v.y * n.y;

		// the velocity after hitting the wall is V - 2p, so just subtract 2*p from V
		return(new Vector2(v.x - 2f * (dotproduct * n.x), v.y - 2f * (dotproduct * n.y)));
	}

	static public bool PolygonIntersectCircle(Polygon2D poly, Vector2D circle, float radius) {
		foreach (Pair2D id in Pair2D.GetList(poly.pointsList)) {
			if (LineIntersectCircle(id, circle, radius) == true) {
				return(true);
			}
		}
		return(false);
	}
	
	static public bool SliceIntersectCircle(List<Vector2D> points, Vector2D circle, float radius) {
		foreach (Pair2D id in Pair2D.GetList(points, false)) {
			if (LineIntersectCircle(id, circle, radius) == true) {
				return(true);
			}
		}
		return(false);
	}

	static public bool LineIntersectCircle(Pair2D line, Vector2D circle, float radius)
	{
		double sx = line.B.x - line.A.x;
		double sy = line.B.y - line.A.y;

		double q = ((circle.x - line.A.x) * (line.B.x - line.A.x) + (circle.y - line.A.y) * (line.B.y - line.A.y)) / (sx * sx + sy * sy);
			
		if (q < 0.0f) {
			q = 0.0f;
		} else if (q > 1.0) {
			q = 1.0f;
		}

		double dx = circle.x - ((1.0f - q) * line.A.x + q * line.B.x);
		double dy = circle.y - ((1.0f - q) * line.A.y + q * line.B.y);

		if (dx * dx + dy * dy < radius * radius) {
			return(true);
		} else {
			return(false);
		}
	}
}
