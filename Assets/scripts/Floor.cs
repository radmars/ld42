using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Floor : MonoBehaviour
{
	private FloorTile[] floorTiles;
	private List<TileEffect> effects;

	public Chairman chairman;
	public Text winLose;
	public Elevator elevator;

	// Use this for initialization
	void Start()
	{
		floorTiles = GetComponentsInChildren<FloorTile>();
		effects = floorTiles
			.Where(e => e.tileEffect != null)
			.Select(tile => { return tile.tileEffect; }).ToList();

		effects.ForEach(e =>
		{
			e.Finished += OnFinish;
		});

		if (winLose != null)
		{
			winLose.enabled = false;
		}
		chairman.OnDie += OnChairmanDeath;
		elevator.OnInElevator += InElevator;
	}

	private void InElevator()
	{
		Win();
	}

	private void OnChairmanDeath(string why)
	{
		Die(why);
	}

	private void OnFinish(TileEffect effect)
	{
		if (effects.Where(e => e.IsFinished()).Count() <= 1)
		{
		//	Win();
		}
	}

	private void Die(string why)
	{
		if (winLose != null && !winLose.enabled)
		{
			winLose.text = "YOU DIED: \n" + why.ToUpper() + "\nRESTART IN 5 SECONDS";
			winLose.enabled = true;
			StartCoroutine(ScheduleReload(5));
		}
	}

	private void Win()
	{
		if (winLose != null && !winLose.enabled)
		{
			winLose.text = "YOU WIN\nRESTART IN 5 SECONDS";
			winLose.enabled = true;
			StartCoroutine(ScheduleReload(5));
		}
	}

	private IEnumerator ScheduleReload(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		var scene = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(scene);
	}
}
