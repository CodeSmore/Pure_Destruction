using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Destruction2D : MonoBehaviour {
	public enum TextureType {Sprite, Mesh};
	public enum CenterOfMass {Default, RigidbodyOnly};

	public TextureType textureType;
	public DestructionLayer destructionLayer = DestructionLayer.Layer1;

	public CenterOfMass centerOfMass = CenterOfMass.Default;

	public bool enableSplit = false;
	public bool splitLimit = false;
	public int splitCount = 0;
	public int maxSplits = 10;

	private Destruction2DBuffer buffer;
	public RenderTexture renderTexture;

	public SpriteRenderer spriteRenderer;
	public Sprite outputSprite;
	public Material outputMaterial;
	public Texture2D outputTexture;

	public Sprite originalSprite;
	public Material originalSpriteMaterial;

	public List<DestructionEvent> eraseEvents = new List<DestructionEvent>();

	public List<DestructionModifier> modifiers = new List<DestructionModifier>();

	public bool modifiersAdded = false;
	public bool replaceSprite = false;
	
	public bool initialized = false;
	
	public List<Polygon2D> erasePolygons = new List<Polygon2D>();
	public List<Mesh> eraseMeshes = new List<Mesh>();

	public Polygon2D customPolygon = null;

	static private List<Destruction2D> destructible2DList = new List<Destruction2D>();

	// Anchor Events
	public List<Polygon2D> anchorPolygons = new List<Polygon2D>();
	public List<Collider2D> anchorColliders = new List<Collider2D>();

	public delegate void Destruction2DEventFunction(Destruction2DEvent destruction);
	private event Destruction2DEventFunction destructionAnchorEvent;
	
	static public List<Destruction2D> GetList() {
		return(new List<Destruction2D>(destructible2DList));
	}

	void OnEnable() {
		destructible2DList.Add (this);
	}

	void OnDisable() {
		destructible2DList.Remove (this);
	}

	static public List<Destruction2D> GetListLayer(Destruction2DLayer layer) {
		List<Destruction2D> result = new List<Destruction2D>();

		foreach (Destruction2D id in  destructible2DList) {
			if (id.MatchLayers (layer)) {
				result.Add(id);
			}
		}
		
		return(result);
	}

	public bool MatchLayers(Destruction2DLayer sliceLayer) {
		return((sliceLayer == null || sliceLayer.GetLayerType() == Destruction2DLayerType.All) || sliceLayer.GetLayerState(GetLayerID ()));
	}

	public int GetLayerID() {
		return((int)destructionLayer);
	}

	public void AddAnchorEvent(Destruction2DEventFunction e) {
		destructionAnchorEvent += e;
	}

	public float GetOrtographicSize() {
		return((float)originalSprite.texture.height / (originalSprite.pixelsPerUnit * 2) * transform.localScale.x);
	}
	
	static public void CopyComponents(Destruction2D slicer, GameObject gObject) {
		Component[] scriptList = slicer.gameObject.GetComponents<Component>();	
		foreach (Component script in scriptList) {
			if (script.GetType().ToString() == "UnityEngine.PolygonCollider2D" || script.GetType().ToString() == "UnityEngine.BoxCollider2D" || script.GetType().ToString() == "UnityEngine.CircleCollider2D" || script.GetType().ToString() == "UnityEngine.CapsuleCollider2D") {
				continue;
			}

			if (script.GetType().ToString() == "Destruction2D") {
				continue;
			}

			if (script.GetType().ToString() != "UnityEngine.Transform") {
				gObject.AddComponent(script.GetType());
				System.Reflection.FieldInfo[] fields = script.GetType().GetFields();

				foreach (System.Reflection.FieldInfo field in fields) {
					field.SetValue(gObject.GetComponent(script.GetType()), field.GetValue(script));
				}
			}
		}

		foreach (Behaviour childCompnent in gObject.GetComponentsInChildren<Behaviour>()) {
			foreach (Behaviour child in slicer.GetComponentsInChildren<Behaviour>()) {
				if (child.GetType() == childCompnent.GetType()) {
					childCompnent.enabled = child.enabled;
					break;
				}
			}
		}
	}

	public Polygon2D GetBoundPolygon() {
		float sizeY = GetOrtographicSize() / transform.localScale.x;
		float sizeX = sizeY * ((float)outputTexture.width / (float)outputTexture.height);

		return(Polygon2D.CreateFromRect(new Vector2(sizeX, sizeY)));
	}
	
	static public void DestroyByComplexCutAll(ComplexCut complexCut, Destruction2DLayer layer) {
		foreach(Destruction2D gObject in GetListLayer(layer)) {
			gObject.DestroyByComplexCut(complexCut);
		}
	}

	static public void DestroyByLinearCutAll(LinearCut linearCut, Destruction2DLayer layer) {
		foreach(Destruction2D gObject in GetListLayer(layer)) {
			gObject.DestroyByLinearCut(linearCut);
		}
	}

	static public void DestroyByPolygonAll(Polygon2D polygon, Destruction2DLayer layer) {
		foreach(Destruction2D gObject in GetListLayer(layer)) {
			gObject.DestroyByPolygon(polygon);
		}
	}

	public void DestroyByLinearCut(LinearCut linearCut) {
		DestroyByPolygon(new Polygon2D(linearCut.GetPointsList()));
	}

	public void DestroyByComplexCut(ComplexCut complexCut) {
		DestroyByPolygon(new Polygon2D(complexCut.GetPointsList()));
	}

	public void DestroyByCollider(Collider2D collider) {
		DestroyByPolygon(Polygon2DList.CreateFromGameObject(collider.gameObject)[0].ToWorldSpace(collider.transform));
	}
	
	public void DestroyByPolygon(Polygon2D polygon) {
		if (polygon.pointsList.Count < 3) {
			return;
		}

		List<Polygon2D> polys = Polygon2DList.CreateFromGameObject(gameObject); 
		if (polys.Count > 0) {
			bool touch = false;
			bool outside = false;

			foreach(Polygon2D p in polys) {
				p.pointsList = p.ToWorldSpace(transform).pointsList;

				if (Math2D.PolyCollidePoly(polygon, p) == true) {
					touch = true;
				}

				if (polygon.PolyInPoly(p) == false) {
					outside = true;
				}
			}

			if (touch == false) {
				return;
			}

			if (outside == false) {
				Destroy(gameObject);
				return;
			}
		} else {
			Polygon2D poly = GetBoundPolygon(); 
			bool touch = false;

			poly = poly.ToWorldSpace(transform);

			if (Math2D.PolyCollidePoly(polygon, poly) == true) {
				touch = true;
			}

			if (touch == false) {
				return;
			}
		}

		eraseEvents.Add(new DestructionEvent(polygon));

		Destruction2DManager.RequestBufferEvent(this);
	}

	public void AddModifier(Texture2D texture, Vector2 position, Vector2 size, float rotation) {
		Polygon2D poly = Polygon2D.CreateFromRect(size);
		poly = poly.ToRotation(rotation * Mathf.Deg2Rad);
		poly = poly.ToOffset(new Vector2D(position));

		List<Polygon2D> polys = Polygon2DList.CreateFromGameObject(gameObject); 
		if (polys.Count > 0) {
			bool touch = false;

			foreach(Polygon2D p in polys) {
				p.pointsList = p.ToWorldSpace(transform).pointsList;

				if (Math2D.PolyCollidePoly(poly, p) == true) {
					touch = true;
				}
			}

			if (touch == false) {
				return;
			}
		} else {
			Polygon2D p = GetBoundPolygon(); 
			bool touch = false;

			p = p.ToWorldSpace(transform);

			if (Math2D.PolyCollidePoly(p, poly) == true) {
				touch = true;
			}

			if (touch == false) {
				return;
			}
		}
		Vector2 pos = transform.InverseTransformPoint(position);

		float ratioX = 1;
		
		if (transform.localScale.x != transform.localScale.y) {
			ratioX = transform.localScale.x / transform.localScale.y;
			size.y = size.y * ratioX;
		}

		//pos.x *= transform.localScale.x;
		//pos.y *= transform.localScale.y * ratioX;
		//pos.x *= transform.localScale.x;

		//pos.y *= ratioX;

		size.x /= transform.localScale.x;
		size.y /= transform.localScale.y;
		size.y /= ratioX;

		DestructionModifier modifier = new DestructionModifier(texture, pos, size, rotation);
	
		modifiers.Add(modifier);

		modifiersAdded = true; // add only if colides

		Destruction2DManager.RequestBufferEvent(this); // Check if it is already requested
	}

	static public void AddModifierAll(Texture2D texture, Vector2 position, Vector2 size, float rotation, Destruction2DLayer layer) {
		foreach(Destruction2D gObject in GetListLayer(layer)) {
			gObject.AddModifier(texture,position,size, rotation);
		}
	}

	void Start () {
		Destruction2DManager.Initialize();

		spriteRenderer = GetComponent<SpriteRenderer>();
		Sprite sprite = spriteRenderer.sprite;

		if (replaceSprite  == false) {
			originalSprite = sprite;

			originalSpriteMaterial = new Material (Shader.Find ("Sprites/Default"));
			originalSpriteMaterial.mainTexture = originalSprite.texture;
		}

		Texture2D texture = originalSprite.texture;

		// Mesh Mode Only?
		outputMaterial = new Material (Shader.Find ("Sprites/Default"));
		outputMaterial.mainTexture = texture;

		renderTexture = new RenderTexture(texture.width, texture.height, 32);
		//Debug.Log(renderTexture.width + " " + renderTexture.height);
		outputTexture =  new Texture2D(texture.width, texture.height);

		switch(textureType) {
			case TextureType.Mesh:
				Destroy(spriteRenderer);

				break;

			case TextureType.Sprite:
				break;
		}

		Destruction2DManager.RequestBufferEvent(this);
	}

	public void UpdateCollider() {
		PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();
		if (polygonCollider2D != null) {
			Destroy(polygonCollider2D);
			PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
			float scale = (float)originalSprite.texture.width / (originalSprite.pixelsPerUnit * 2);
			
			// Check If There Is No Proper Collide Generated ()
			if (collider.pathCount == 1) {
				Vector2[] standardPentagon = collider.GetPath(0);
				if (standardPentagon[0] / scale == new Vector2(0, 1) && standardPentagon[1] / scale == new Vector2(-0.9510565f, 0.309017f)) {
					Destroy(gameObject);
				}
			}

			if (enableSplit == true && (splitLimit == false || splitCount <= maxSplits)) {
				List<Polygon2D> polys = Polygon2DList.CreateFromPolygonColliderToLocalSpace(collider);

				List<GameObject> disattachedObjects = new List<GameObject>();

				if (polys.Count > 1) {
					int id = 1;

					Rigidbody2D originalRigidBody = GetComponent<Rigidbody2D>();

					foreach(Polygon2D poly in polys) {
						GameObject gObject = new GameObject();
						gObject.name = gameObject.name + " (" + id + ")";
						gObject.transform.parent = transform.parent;

						Polygon2D polygon = poly;
						polygon.CreateCollider(gObject);

						gObject.transform.position = transform.position;
						gObject.transform.rotation = transform.rotation;
						gObject.transform.localScale = transform.localScale;

						Destruction2D.CopyComponents(this, gObject);
						
						gObject.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
						
						Destruction2D r = gObject.AddComponent<Destruction2D>();
						r.replaceSprite = true;
						r.originalSprite = originalSprite;
						r.originalSpriteMaterial = originalSpriteMaterial;

						r.customPolygon = poly;

						r.modifiers = new List<DestructionModifier>(modifiers);
						r.eraseMeshes = new List<Mesh>(eraseMeshes);
						
						r.enableSplit = true;
						r.splitLimit = splitLimit;
						r.maxSplits = maxSplits;
						r.centerOfMass = centerOfMass;
						r.splitCount = splitCount + 1;

						if (anchorColliders.Count > 0) {
							if (r.Detach(collider) == true) {
								disattachedObjects.Add(gameObject);
							}
						}
						
						foreach(Polygon2D p in polys) {
							if (p != poly) {
								r.erasePolygons.Add(p);
							}
						}

						if (originalRigidBody) {
							Rigidbody2D newRigidBody = gObject.GetComponent<Rigidbody2D> ();

							newRigidBody.isKinematic = originalRigidBody.isKinematic;
							newRigidBody.velocity = originalRigidBody.velocity;
							newRigidBody.angularVelocity = originalRigidBody.angularVelocity;
							newRigidBody.angularDrag = originalRigidBody.angularDrag;
							newRigidBody.constraints = originalRigidBody.constraints;
							newRigidBody.gravityScale = originalRigidBody.gravityScale;
							newRigidBody.collisionDetectionMode = originalRigidBody.collisionDetectionMode;
							//newRigidBody.sleepMode = originalRigidBody.sleepMode;
							//newRigidBody.inertia = originalRigidBody.inertia;

							// Center of Mass : Auto / Center
							if (centerOfMass == CenterOfMass.RigidbodyOnly) {
								newRigidBody.centerOfMass = Vector2.zero;
							}
						}
						
						id++;
					}
					Destroy(gameObject);
				
			} else {
					if (anchorColliders.Count > 0) {
						if (Detach(collider) == true) {
							disattachedObjects.Add(gameObject);
						}
					}
				}

				if (disattachedObjects.Count > 0) {
					if ((destructionAnchorEvent!= null)) {
						Destruction2DEvent destruction = new Destruction2DEvent();
						destruction.gameObjects = disattachedObjects;

						destructionAnchorEvent (destruction);
					}
				}
			}
		}
	}

	public bool Detach (PolygonCollider2D collider) {
		Polygon2D polygon = Polygon2DList.CreateFromPolygonColliderToWorldSpace(collider)[0];

		bool attached = true;
		foreach(Collider2D c in anchorColliders) {
			Polygon2D p = anchorPolygons[anchorColliders.IndexOf(c)].ToWorldSpace(c.transform);

			// Fix for PolyCollidePoly!!!
			bool inHole = false;
			
			foreach(Polygon2D hole in polygon.holesList) {
				if (hole.PolyInPoly(p)) {
					inHole = true;
				}
			}

			if (inHole == false) {
				if (Math2D.PolyCollidePoly(p, polygon) == false) {
					attached = false;
				}
			} else{
				attached = false;
			}
		}
		if (attached == false) {
			return(true);
		}
		return(false);
	}
}

public class DestructionEvent {
	public Polygon2D polygon;

	public DestructionEvent(Polygon2D polygonVar) {
		polygon = polygonVar;
	}
}

public class DestructionModifier {
	public Vector2 position;
	public Material material;
	public Vector2 size;
	public float rotation;

	public DestructionModifier(Texture tex, Vector2 pos, Vector2 siz, float rot) {
		material = new Material (Shader.Find ("SmartDestruction2D/ModifierShader")); 
		material.mainTexture = tex;
		position = pos;
		size = siz;
		rotation = rot;
	}
}

	//public bool recalculateCenterPivot = false;
	//public bool createChunks = false;


	//public Vector2D customPivot = null;
	//public Vector2D customOffset = null;
	//public Vector2D customOffsetSet = null;

			//float sx = (originalSprite.texture.width / originalSprite.pixelsPerUnit) ;
			//float sy = (originalSprite.texture.height / originalSprite.pixelsPerUnit);
		
			//r.customPivot = new Vector2D(poly.GetBounds().center.x / sx, poly.GetBounds().center.y / sy);
			//r.customOffset  = new Vector2D(poly.GetBounds().center);

	//	if (customPivot != null) {
	//		//Debug.Log("Offset " + customPivot.ToVector2());
	//		pos.x += (float)customPivot.x * 5;
	//		pos.y += (float)customPivot.y * 5;
	//	}
	