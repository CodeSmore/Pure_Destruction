using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction2DManager : MonoBehaviour {
	private static Destruction2DManager instance = null;
	public float bufferMaxIdle = 1;

	public static void Initialize() {
		if (instance != null) {
			return;
		}

		GameObject manager = new GameObject();
		manager.name = "Destruction 2D Manager";

		instance = manager.AddComponent<Destruction2DManager>();
	}
	
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Debug.LogError("Destruction2D: Multiple Managers Detected!");
			Destroy(this);
		}
	}
	
	static public void RequestBufferEvent(Destruction2D destructible) {
		Destruction2DBuffer buffer = GetBuffer(destructible);

		buffer.Set(destructible);
	}

	static public Destruction2DBuffer GetBuffer(Destruction2D destructible) {
		foreach(Destruction2DBuffer buffer in Destruction2DBuffer.GetList()) {
			if (buffer.destructible == destructible) {
				return(buffer);
			}
		}

		foreach(Destruction2DBuffer buffer in Destruction2DBuffer.GetList()) {
			if (buffer.destructible == null) {
				return(buffer);
			}
		}
		return(CreateBuffer());
	}

	public static Destruction2DBuffer CreateBuffer () {
		GameObject gameObject = new GameObject();
		gameObject.name = "Buffer (" + (Destruction2DBuffer.count + 1) + ")";
		gameObject.transform.parent = instance.transform;
		gameObject.transform.position = new Vector3(Destruction2DBuffer.count * 200 + 5, 0, -1000);

		Camera renderCamera = gameObject.AddComponent<Camera>();
		renderCamera.orthographic = true;
		renderCamera.orthographicSize = 1f; 
		renderCamera.allowHDR = false;
		renderCamera.allowMSAA = false;
		renderCamera.clearFlags = CameraClearFlags.SolidColor;
		renderCamera.nearClipPlane = 0;
		renderCamera.farClipPlane = 0.5f;

		Destruction2DBuffer buffer = gameObject.AddComponent<Destruction2DBuffer>();
		buffer.Init(renderCamera);

		buffer.idleTime = instance.bufferMaxIdle;

		return(buffer);
	}

}
