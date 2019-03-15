using System.Collections.Generic;
using UnityEngine;

public class Destruction2DLayer {
	private Destruction2DLayerType layer = Destruction2DLayerType.All;
	private bool[] layers = new bool[10];

	static public Destruction2DLayer Create()
	{
		return(new Destruction2DLayer());
	}

	public void SetLayerType(Destruction2DLayerType type)
	{
		layer = type;
	}

	public void SetLayer(int id, bool value)
	{
		layers [id] = value;
	}

	public void DisableLayers()
	{
		layers = new bool[10];
	}

	public Destruction2DLayerType GetLayerType()
	{
		return(layer);
	}

	public bool GetLayerState(int id)
	{
		return(layers [id]);
	}

}
