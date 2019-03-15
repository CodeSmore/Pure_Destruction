using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Destruction2DController : MonoBehaviour {
	public enum DestructionType {LinearCut, ComplexCut, Polygon, Modifier, PolygonBrush, ComplexBrush}
	public static Destruction2DController instance;

	public bool drawVisuals = true;
	public float visualScale = 1f;
	public float lineWidth = 1.0f;
	
	public float minVertsDistance = 0.1f;
	public float cutSize = 0.25f;
	public float zPosition = 0f; //???
	public Color color = Color.white;

	public DestructionType destructionType = DestructionType.LinearCut;
	public Destruction2DLayer destructionLayer = Destruction2DLayer.Create();

	public static LinearCut linearCutLine = new LinearCut();
	public static ComplexCut complexCutLine = new ComplexCut();
	public Polygon2D.PolygonType polygonType = Polygon2D.PolygonType.Pentagon;

	Polygon2D slicePolygon = null;
	public float polygonSize = 5f;
	public int polygonEdgeCount = 15;

	static Pair2D linearPair = Pair2D.Zero();
	static List<Vector2D> complexSlicerPointsList = new List<Vector2D>();

	bool mouseDown = false;

	public Vector2 modifierSize = new Vector2(0, 0);
	public float modifierRotation = 0;
	public bool randomRotation = true;
	public int modifierID = 0;
	public bool randomModifierID = false;
	public Texture2D[] modifierTextures = null;
	
	void Awake () {
		instance = this;
	}

	public void SetLayerType(int type)
	{
		if (type == 0) {
			destructionLayer.SetLayerType((Destruction2DLayerType)0);
		} else {
			destructionLayer.SetLayerType((Destruction2DLayerType)1);
			destructionLayer.DisableLayers ();
			destructionLayer.SetLayer (type - 1, true);
		}
	}

	public static Vector2 GetMousePosition() {
		return(Camera.main.ScreenToWorldPoint (Input.mousePosition));
	}

	public void LateUpdate() {
		if (UnityEngine.EventSystems.EventSystem.current != null && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		Vector2D pos = new Vector2D(GetMousePosition());

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		switch (destructionType) {
			case DestructionType.LinearCut:
			case DestructionType.ComplexCut:
				float newCutSize = cutSize + scroll;
				if (newCutSize > 0.05f) {
					cutSize = newCutSize;
				}

				break;

			case DestructionType.Polygon:
			case DestructionType.PolygonBrush:
				float newPolygonSize = polygonSize + scroll;
				if (newPolygonSize > 0.05f) {
					polygonSize = newPolygonSize;
				}
				break;
		}

		float newModifierSizeX = modifierSize.x + scroll * 2;
		if (newModifierSizeX > 0.05f) {
			modifierSize.x = newModifierSizeX;
		}
		float newModifierSizeY = modifierSize.y + scroll * 2;
		if (newModifierSizeY > 0.05f) {
			modifierSize.y = newModifierSizeY;
		}

		switch(destructionType) {
			case DestructionType.LinearCut:
				UpdateLinearCut (pos);
				break;

			case DestructionType.ComplexCut:
				UpdateComplexCut (pos);
				break;

			case DestructionType.Polygon:
				UpdatePolygon(pos);
				break;
			
			case DestructionType.PolygonBrush:
				UpdatePolygonBrush(pos);
				break;

			case DestructionType.Modifier:
				UpdateModifier(pos);
				break;

			case DestructionType.ComplexBrush:
				UpdateComplexBrush(pos);
				break;
		}
	}

	private void UpdateModifier(Vector2D pos) {
		mouseDown = true;

        // I just don't want destruction by mouse press for this game
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (modifierTextures.Length > 0)
        //    {
        //        Destruction2D.AddModifierAll(modifierTextures[modifierID], pos.ToVector2(), modifierSize, modifierRotation, destructionLayer);

        //        if (randomRotation)
        //        {
        //            modifierRotation = Random.Range(0, 360);
        //        }

        //        if (randomModifierID)
        //        {
        //            modifierID = Random.Range(0, modifierTextures.Length - 1);
        //        }
        //    }
        //}
    }

	private void UpdatePolygon(Vector2D pos) {
		mouseDown = true;

        // No mouse-press destruction pls
		//if (Input.GetMouseButtonDown (0)) {
		//	Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
		//	slicePolygon = Polygon2D.Create (polygonType, polygonSize);

		//	Polygon2D polygon = new Polygon2D();
		//	polygon.pointsList = new List<Vector2D>(slicePolygon.pointsList);
		//	polygon.ToOffset(pos);
		
		//	Destruction2D.DestroyByPolygonAll(polygon, destructionLayer);

		//	UpdateModifier(pos);
		//}
	}

	private void UpdatePolygonBrush(Vector2D pos) {


        //if (Input.GetMouseButton(0))
        //{
        //    mouseDown = true;
        //    Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
        //    slicePolygon = Polygon2D.Create(polygonType, polygonSize);

        //    Polygon2D polygon = new Polygon2D();
        //    polygon.pointsList = new List<Vector2D>(slicePolygon.pointsList);
        //    polygon.ToOffset(pos);

        //    Destruction2D.DestroyByPolygonAll(polygon, destructionLayer);
        //}
        //else
        //{
        //    mouseDown = false;
        //}
    }

	private void UpdateLinearCut(Vector2D pos) {

        // No mouse destruction. Is bad (for this game)
		//if (Input.GetMouseButtonDown (0)) {
		//	linearPair.A.Set (pos);
		//}

		//if (Input.GetMouseButton (0)) {
		//	linearPair.B.Set (pos);
		//	mouseDown = true;
		//}

		//if (mouseDown == true && Input.GetMouseButton (0) == false) {
		//	mouseDown = false;

		//	Destruction2D.DestroyByLinearCutAll(linearCutLine, destructionLayer);
		//}
	}

	private void UpdateComplexCut(Vector2D pos) {


		//if (Input.GetMouseButtonDown (0)) {
		//	complexSlicerPointsList.Clear ();

		//	complexSlicerPointsList.Add (pos);
		//	mouseDown = true;
		//}

		//if (complexSlicerPointsList.Count < 1) {
		//	return;
		//}
		
		//if (Input.GetMouseButton (0)) {
		//	Vector2D posMove = new Vector2D (complexSlicerPointsList.Last ());

		//	while ((Vector2D.Distance (posMove, pos) > minVertsDistance * visualScale)) {
		//		float direction = (float)Vector2D.Atan2 (pos, posMove);
		//		posMove.Push (direction, minVertsDistance * visualScale);

		//		complexSlicerPointsList.Add (new Vector2D (posMove));
		//	}
		//}

		//if (mouseDown == true && Input.GetMouseButton (0) == false) {
		//	mouseDown = false;

		//	Destruction2D.DestroyByComplexCutAll(complexCutLine, destructionLayer);

		//	complexSlicerPointsList.Clear ();
		//}
	}

	private void UpdateComplexBrush(Vector2D pos) {
        return; 



		if (Input.GetMouseButtonDown (0)) {
			linearPair.A = pos;
		}

		if (Input.GetMouseButton (0)) {
			mouseDown = true;
			Vector2D posMove = linearPair.A;

			linearPair.B = pos;

			if ((Vector2D.Distance (posMove, pos) > minVertsDistance * visualScale)) {
				float direction = (float)Vector2D.Atan2 (pos, posMove);
				
				linearCutLine = LinearCut.Create(linearPair, cutSize);

				Destruction2D.DestroyByLinearCutAll(linearCutLine, destructionLayer);

				linearPair.A = pos;
			}
		} else {
			mouseDown = false;
		}
	}

    public void OnRenderObject() {
        // Would draw circle guide for mouse
        // Skip


		//if (drawVisuals == false) {
		//	return;
		//}

		//Vector2 pos = GetMousePosition ();

		//Max2D.SetBorder (true);
		//Max2D.SetLineMode(Max2D.LineMode.Smooth);
		//Max2D.SetLineWidth (lineWidth * .5f);
		//Max2D.SetScale(visualScale);

		//switch(destructionType) {

		//	case DestructionType.LinearCut:
		//		if (mouseDown) {
		//			linearCutLine = LinearCut.Create(linearPair, cutSize);
		//			Max2D.DrawStrippedLine (linearCutLine.GetPointsList(), 0, zPosition, true);
		//		}
		//		break;

		//	case DestructionType.ComplexCut:
		//		if (mouseDown) {
		//			complexCutLine = ComplexCut.Create(complexSlicerPointsList, cutSize);
		//			Max2D.DrawStrippedLine (complexCutLine.GetPointsList(), 0, zPosition, true);
		//		}
		//		break;

		//	case DestructionType.Polygon:
		//		Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
		//		slicePolygon = Polygon2D.Create (polygonType, polygonSize);
		//		Max2D.DrawStrippedLine (slicePolygon.pointsList, minVertsDistance, zPosition, true, new Vector2D(pos));
		//		break;

		//	case DestructionType.PolygonBrush:
		//		Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
		//		slicePolygon = Polygon2D.Create (polygonType, polygonSize);
		//		Max2D.DrawStrippedLine (slicePolygon.pointsList, minVertsDistance, zPosition, true, new Vector2D(pos));
		//		break;

		//	case DestructionType.ComplexBrush:
		//		if (mouseDown) {
		//			linearCutLine = LinearCut.Create(linearPair, cutSize);
		//			Max2D.DrawStrippedLine (linearCutLine.GetPointsList(), 0, zPosition, true);
		//		}
				
		//		break;

		//	case DestructionType.Modifier:
		//		Material material = new Material (Shader.Find ("SmartDestruction2D/ModifierShader")); 
		//		material.mainTexture = modifierTextures[modifierID];
		
		//		Max2D.DrawImage(material, pos, modifierSize, modifierRotation, zPosition);
			
		//		break;

		//}
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");

        Vector2D pos = new Vector2D(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
        
        Polygon2D.defaultCircleVerticesCount = polygonEdgeCount;
        slicePolygon = Polygon2D.Create(polygonType, polygonSize);

        Polygon2D polygon = new Polygon2D();
        polygon.pointsList = new List<Vector2D>(slicePolygon.pointsList);
        polygon.ToOffset(pos);

        Destruction2D.DestroyByPolygonAll(polygon, destructionLayer);

        UpdateModifier(pos);
    }
}