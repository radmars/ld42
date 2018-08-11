using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Floor : MonoBehaviour {
	private FloorTile[] floorTiles;
	private List<TileEffect> effects;

	// Use this for initialization
	void Start () {
		floorTiles = GetComponentsInChildren<FloorTile>();
		effects = floorTiles
			.Where(e => e.tileEffect != null)
			.Select(tile => { return tile.tileEffect; }).ToList();

		effects.ForEach(e => {
			e.Finished += OnFinish;
		});
	}

	private void OnFinish(TileEffect effect)
	{
		if( effects.Where(e => e.IsFinished() ).Count() <= 1)
		{
			Debug.Log("You win?");
		}
	}
}
