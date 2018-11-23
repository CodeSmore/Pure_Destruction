﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineTriangle {
	public List<Vector2> uv = new List<Vector2>();
	public List<Vector3> vertices = new List<Vector3>();
}

public class Max2DMesh {
	const float pi = Mathf.PI;
	const float pi2 = pi / 2;
	const float uv0 = 1f / 32;
	const float uv1 = 1f - uv0;

	static public void Draw(Mesh mesh, Transform transform, Material material) {
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		Vector3 scale = transform.lossyScale;
		Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

		Graphics.DrawMesh(mesh, matrix, material, 0);
	}

	static public void Draw(Mesh mesh, Material material) {
		Vector3 position = Vector3.zero;
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		Vector3 scale = new Vector3(1, 1, 1);
		Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, scale);

		Graphics.DrawMesh(mesh, matrix, material, 0);
	}


	static public LineTriangle CreateLine(Pair2D pair, Transform transform, float lineWidth, float z = 0f) {
		LineTriangle result = new LineTriangle();

		float size = lineWidth / 6;
		float rot = (float)Vector2D.Atan2 (pair.A, pair.B);

		Vector2D A1 = new Vector2D (pair.A);
		Vector2D A2 = new Vector2D (pair.A);
		Vector2D B1 = new Vector2D (pair.B);
		Vector2D B2 = new Vector2D (pair.B);

		Vector2 scale = new Vector2(1f / transform.localScale.x, 1f / transform.localScale.y);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);
		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		result.uv.Add(new Vector2(0.5f + uv0, 0));
		result.vertices.Add(new Vector3((float)B1.x, (float)B1.y, z));
		result.uv.Add(new Vector2(uv1, 0));
		result.vertices.Add(new Vector3((float)A1.x, (float)A1.y, z));
		result.uv.Add(new Vector2(uv1, 1));
		result.vertices.Add(new Vector3((float)A2.x, (float)A2.y, z));
		result.uv.Add(new Vector2(0.5f + uv0, 1));
		result.vertices.Add(new Vector3((float)B2.x, (float)B2.y, z));
	
		A1 = new Vector2D (pair.A);
		A2 = new Vector2D (pair.A);
		Vector2D A3 = new Vector2D (pair.A);
		Vector2D A4 = new Vector2D (pair.A);

		A1.Push (rot + pi2, size, scale);
		A2.Push (rot - pi2, size, scale);

		A3.Push (rot + pi2, size, scale);
		A4.Push (rot - pi2, size, scale);
		A3.Push (rot + pi, -size, scale);
		A4.Push (rot + pi, -size, scale);

		result.uv.Add(new Vector2(uv0, 0));
		result.vertices.Add(new Vector3((float)A3.x, (float)A3.y, z));
		result.uv.Add(new Vector2(uv0, 1));
		result.vertices.Add(new Vector3((float)A4.x, (float)A4.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 1));
		result.vertices.Add(new Vector3((float)A2.x, (float)A2.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 0));
		result.vertices.Add(new Vector3((float)A1.x, (float)A1.y, z));

		B1 = new Vector2D (pair.B);
		B2 = new Vector2D (pair.B);
		Vector2D B3 = new Vector2D (pair.B);
		Vector2D B4 = new Vector2D (pair.B);

		B1.Push (rot + pi2, size, scale);
		B2.Push (rot - pi2, size, scale);

		B3.Push (rot + pi2, size, scale);
		B4.Push (rot - pi2, size, scale);
		B3.Push (rot + pi, size, scale);
		B4.Push (rot + pi , size, scale);

		result.uv.Add(new Vector2(uv0, 0));
		result.vertices.Add(new Vector3((float)B4.x, (float)B4.y, z));
		result.uv.Add(new Vector2(uv0, 1));
		result.vertices.Add(new Vector3((float)B3.x, (float)B3.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 1));
		result.vertices.Add(new Vector3((float)B1.x, (float)B1.y, z));
		result.uv.Add(new Vector2(0.5f - uv0, 0));
		result.vertices.Add(new Vector3((float)B2.x, (float)B2.y, z));

		return(result);
	}

	static public Mesh ExportMesh(List<LineTriangle> trianglesList) {
		Mesh mesh = new Mesh();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
		List<int> triangles = new List<int>();

		int count = 0;
		foreach(LineTriangle triangle in trianglesList) {
			foreach(Vector3 v in triangle.vertices) {
				vertices.Add(v);
			}
			foreach(Vector2 u in triangle.uv) {
				uv.Add(u);
			}
			
			triangles.Add(count + 0);
			triangles.Add(count + 1);
			triangles.Add(count + 2);

			triangles.Add(count + 4);
			triangles.Add(count + 5);
			triangles.Add(count + 6);

			triangles.Add(count + 8);
			triangles.Add(count + 9);
			triangles.Add(count + 10);

			triangles.Add(count + 3);
			triangles.Add(count + 0);
			triangles.Add(count + 2);

			triangles.Add(count + 7);
			triangles.Add(count + 4);
			triangles.Add(count + 6);

			triangles.Add(count + 11);
			triangles.Add(count + 8);
			triangles.Add(count + 10);
			
			count += 12;
		}

		mesh.vertices = vertices.ToArray();
		mesh.uv = uv.ToArray();
		mesh.triangles = triangles.ToArray();

		return(mesh);
	}

	static public Mesh GeneratePolygon2DMesh(Transform transform, Polygon2D polygon, float lineOffset, float lineWidth, bool connectedLine) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		Polygon2D poly = polygon;

		foreach(Pair2D p in Pair2D.GetList(poly.pointsList, connectedLine)) {
			trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
		}

		foreach(Polygon2D hole in poly.holesList) {
			foreach(Pair2D p in Pair2D.GetList(hole.pointsList, connectedLine)) {
				trianglesList.Add(CreateLine(p, transform, lineWidth, lineOffset));
			}
		}
		
		return(ExportMesh(trianglesList));
	}

	static public Mesh GenerateLinearMesh(Pair2D linearPair, Transform transform, float lineWidth, float zPosition) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		float size = 0.5f;

		trianglesList.Add(CreateLine(linearPair, transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineWidth, zPosition));
	
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineWidth, zPosition));
	
		return(ExportMesh(trianglesList));
	}

	
	static public Mesh GenerateComplexMesh(List<Vector2D> complexSlicerPointsList, Transform transform, float lineWidth, float minVertsDistance, float zPosition) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		float size = 0.5f;
		
		Vector2D vA, vB;
		foreach(Pair2D pair in Pair2D.GetList(complexSlicerPointsList, false)) {
			vA = new Vector2D (pair.A);
			vB = new Vector2D (pair.B);

			vA.Push (Vector2D.Atan2 (pair.A, pair.B), -minVertsDistance / 5);
			vB.Push (Vector2D.Atan2 (pair.A, pair.B), minVertsDistance / 5);

			trianglesList.Add(CreateLine(new Pair2D(vA, vB), transform, lineWidth, zPosition));
		}

		Pair2D linearPair = Pair2D.Zero();
		linearPair.A = new Vector2D(complexSlicerPointsList.First());
		linearPair.B = new Vector2D(complexSlicerPointsList.Last());

		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x - size, linearPair.A.y - size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x + size, linearPair.A.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.A.x + size, linearPair.A.y + size), new Vector2D(linearPair.A.x - size, linearPair.A.y + size)), transform, lineWidth, zPosition));
	
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x - size, linearPair.B.y - size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x + size, linearPair.B.y - size)), transform, lineWidth, zPosition));
		trianglesList.Add(CreateLine(new Pair2D(new Vector2D(linearPair.B.x + size, linearPair.B.y + size), new Vector2D(linearPair.B.x - size, linearPair.B.y + size)), transform, lineWidth, zPosition));
	
		return(ExportMesh(trianglesList));
	}



	static public Mesh GenerateLinearCutMesh(Pair2D linearPair, float cutSize, Transform transform, float lineWidth, float zPosition) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		LinearCut linearCutLine = LinearCut.Create(linearPair, cutSize);
		foreach(Pair2D pair in Pair2D.GetList(linearCutLine.GetPointsList(), true)) {
			trianglesList.Add(CreateLine(pair, transform, lineWidth, zPosition));
		}

		return(ExportMesh(trianglesList));
	}

	static public Mesh GenerateComplexCutMesh(List<Vector2D> complexSlicerPointsList, float cutSize, Transform transform, float lineWidth, float zPosition) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		ComplexCut complexCutLine = ComplexCut.Create(complexSlicerPointsList, cutSize);
		foreach(Pair2D pair in Pair2D.GetList(complexCutLine.GetPointsList(), true)) {
			trianglesList.Add(CreateLine(pair, transform, lineWidth, zPosition));
		}

		return(ExportMesh(trianglesList));
	}

	static public Mesh GeneratePolygonMesh(Vector2D pos, Polygon2D.PolygonType polygonType, float polygonSize, float minVertsDistance, Transform transform, float lineWidth, float zPosition) {
		List<LineTriangle> trianglesList = new List<LineTriangle>();

		Polygon2D slicePolygon = Polygon2D.Create (polygonType, polygonSize).ToOffset(pos);

		Vector2D vA, vB;
		foreach(Pair2D pair in Pair2D.GetList(slicePolygon.pointsList, true)) {
			vA = new Vector2D (pair.A);
			vB = new Vector2D (pair.B);

			vA.Push (Vector2D.Atan2 (pair.A, pair.B), -minVertsDistance / 5);
			vB.Push (Vector2D.Atan2 (pair.A, pair.B), minVertsDistance / 5);

			trianglesList.Add(CreateLine(new Pair2D(vA, vB), transform, lineWidth, zPosition));
		}

		return(ExportMesh(trianglesList));
	}
}
