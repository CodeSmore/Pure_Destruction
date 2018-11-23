using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderLineRenderer2D : MonoBehaviour {
	public Max2D.LineMode lineMode = Max2D.LineMode.Smooth;
	public Color color = Color.white;
	public float lineWidth = 1;

	private bool connectedLine = true; // For Edge Collider

	private float lineOffset = -0.01f;
	private Polygon2D polygon = null;
	private Mesh mesh = null;
	private float lineWidthSet = 1;

	Material material;

	public Polygon2D GetPolygon() {
		if (polygon == null) {
			polygon = Polygon2DList.CreateFromGameObject (gameObject)[0];
		}
		return(polygon);
	}

	public void GenerateMesh() {
		lineWidthSet = lineWidth;

		mesh =  Max2DMesh.GeneratePolygon2DMesh(transform, GetPolygon(), lineOffset, lineWidth, connectedLine);
	}

	public void Start() {
		if (GetComponent<EdgeCollider2D>() != null) {
			connectedLine = false;
		}
		
		Max2D.Check();
		material = new Material(Max2D.lineMaterial);

		GenerateMesh();
		Draw();
	}

	public void Draw() {
		material.SetColor ("_Emission", color);
		
		Max2DMesh.Draw(mesh, transform, material);
	}

	public void Update() {
		if (lineWidth != lineWidthSet) {
			GenerateMesh();
		}

		Draw();
	}
}