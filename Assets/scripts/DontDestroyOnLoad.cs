using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
	public static DontDestroyOnLoad instance;

	void Start () {
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
