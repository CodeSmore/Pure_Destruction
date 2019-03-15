using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferBase {
	public Destruction2D destructible = null;
	public Camera renderCamera;
	
	static private Material eraseMaterial = null;

	public static Material GetEraseMaterial() {
		if (eraseMaterial == null) {
			eraseMaterial = new Material (Shader.Find ("SmartDestruction2D/RemoveShader"));
		}
		return(eraseMaterial);
	}

	public void DrawSelf (Material materialm) {
		float sizeY = renderCamera.orthographicSize / renderCamera.transform.localScale.x;
		Vector2D size = new Vector2D(sizeY * ((float)destructible.outputTexture.width / (float)destructible.outputTexture.height), sizeY);
		Max2D.DrawImage(renderCamera.transform, destructible.originalSpriteMaterial, new Vector2D(0, 0), size, 0.1f);
	}

	public void SaveRenderTextureToSprite() {
		RenderTexture.active = destructible.renderTexture;

		destructible.outputTexture.filterMode = FilterMode.Point;
		destructible.outputTexture.ReadPixels(new Rect(0, 0, destructible.renderTexture.width, destructible.renderTexture.height), 0, 0);
		destructible.outputTexture.Apply();

		destructible.outputSprite = Sprite.Create(destructible.outputTexture, new Rect(0.0f, 0.0f, destructible.renderTexture.width, destructible.renderTexture.height), new Vector2(0.5f, 0.5f), 100.0f, 2, SpriteMeshType.FullRect);
		destructible.spriteRenderer.sprite = destructible.outputSprite;
		destructible.outputMaterial.mainTexture = destructible.outputTexture;
	}
}
