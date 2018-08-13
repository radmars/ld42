using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MoverController : MonoBehaviour
{

	public TileEffect[] movers;

	private List<int[]> patterns = new List<int[]>{
		new int[] { 0, 1, 2, 3, 30, 31, 32, 33 },
        new int[] { 3, 2, 1, 0, 30, 31, 32, 33 },
        new int[] { 6, 8, 10, 12, 30, 31, 32, 33 },
	};
	private float delayTime;
	private float lastTime;
	private int[] currentPattern;
	private int currentPatternIndex = 0;
	private string mode = "gap"; //gap, moving, stopping

	void Start()
	{
		delayTime = 5f;
	}

	void Update()
	{

		float timeDiff = Time.time - lastTime;
		delayTime -= timeDiff;
		lastTime = Time.time;

		if (delayTime <= 0)
		{
			if (mode == "gap")
			{
				TriggerPattern();
			}
			else if (mode == "moving")
			{
				if (currentPatternIndex == currentPattern.Length)
				{
					mode = "stopping";
					delayTime = 5f;
				}
				else
				{
					movers[currentPattern[currentPatternIndex]].Trigger();
					currentPatternIndex++;
					delayTime = 3f;
				}
			}
			else if (mode == "stopping")
			{
				foreach (var item in currentPattern)
				{
					movers[item].Retract();
				}
				mode = "gap";
				delayTime = 5f;
			}
		}
	}

	void TriggerPattern()
	{
		int index = UnityEngine.Random.Range(0, patterns.Count);
		print(index + " index");

		currentPattern = patterns[index];
		currentPatternIndex = 0;
		print(currentPattern);
		mode = "moving";
	}

	//choose a pattern randomly
	//iterate through the pattern, triggering each block separated by 3 seconds
	//when pattern ends, pause, then retract
	//repeat

	/*
    List<int> list1 = new List<int>();
    var list2 = new List<int>();
    var list3 = new List<int> { 1, 2, 3 };  // Initialize the list with 3 elements.
    list3.Add(4); // Add a new element, list3.Count is now 4.
     * */

}
