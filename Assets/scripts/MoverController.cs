using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MoverController : MonoBehaviour
{

	public TileEffect[] movers;
    public Elevator elevator;

	private List<int[]> patterns = new List<int[]>{
		new int[] { 0, 1, 2, 3, 4, 74, 68, 62, 56, 50, 44, 38 },
        new int[] { 19, 12, 20, 11, 21, 10, 50, 49, 48, 47, 46 },
        new int[] { 12, 11, 10, 9, 8, 7, 27, 74, 28, 73, 29, 72, 30, 71, 31, 70 },
        new int[] { 22, 10, 6, 25, 7, 39, 62, 44, 57, 40, 61, 43, 58 },
        new int[] { 19, 20, 21, 14, 15, 16, 18, 72, 73, 67, 66, 60 },
        new int[] { 5, 4, 3, 2, 1, 69, 63, 57, 51, 45, 39, 33 },
	};
	private float delayTime;
	private float lastTime;
	private int[] currentPattern;
	private int currentPatternIndex = 0;
	private string mode = "gap"; //gap, moving, stopping
    private int level = 1;

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

                    if (currentPattern[currentPatternIndex] >= 27) delayTime = 1.5f; 
                    else delayTime = 3f;

                    currentPatternIndex++;
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
                level++;
                if (level == 2) elevator.OpenElevator();
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
