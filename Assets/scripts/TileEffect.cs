using UnityEngine;
using System.Collections;
using System;

public abstract class TileEffect : MonoBehaviour
{
	public delegate void EffectFinished(TileEffect effect);
	public EffectFinished Finished;

	private bool triggered = false;
	private bool finished = false;

	public virtual void Trigger()
	{
		triggered = true;
	}

	protected void MarkFinished()
	{
		finished = true;
		var finishedCallback = Finished;
		if(finishedCallback != null)
		{
			Finished(this);
		}
	}

	internal bool IsFinished()
	{
		return finished;
	}

	internal bool IsTriggered()
	{
		return triggered;
	}
}
