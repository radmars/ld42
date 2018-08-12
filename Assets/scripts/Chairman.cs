using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chairman : MonoBehaviour
{
	private Rigidbody body;

	public delegate void DeathHanlder(string why);
	public DeathHanlder OnDie;

	private bool alive;

    public AudioSource voiceAudioSource;
    public AudioSource movementAudioSource;
    public AudioSource fallAudioSource;

    public AudioClip gag;
    public List<AudioClip> panics;
    public List<AudioClip> movement;
    public List<AudioClip> falls;
    public AudioClip crushed;
    public AudioClip stabbed;

    private List<int> movementIndices;
    private int movementIndex = 0;

    private List<int> panicIndices;
    private int panicIndex = 0;

    private bool panicking = false;
    private float lastGagTime = 0.0f;

    private List<int> fallIndices;
    private int fallIndex = 0;
    private bool fallSoundPlaying = false;

    // Use this for initialization
    void Start()
	{
		body = GetComponent<Rigidbody>();
		alive = true;

        voiceAudioSource.loop = true;
        voiceAudioSource.clip = gag;
        voiceAudioSource.Play();

        movementIndices = new List<int>(movement.Count);
        for (int i = 0; i < movement.Count; i++)
        {
            movementIndices.Add(i);
        }
        ShuffleList(movementIndices);

        panicIndices = new List<int>(panics.Count);
        for (int i = 0; i < panics.Count; i++)
        {
            panicIndices.Add(i);
        }
        ShuffleList(panicIndices);

        fallIndices = new List<int>(falls.Count);
        for (int i = 0; i < falls.Count; i++)
        {
            fallIndices.Add(i);
        }
        ShuffleList(fallIndices);
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			Force(new Vector3(0, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			Force(new Vector3(1f, 1f), transform.up);
		}
		else if (Input.GetKeyDown(KeyCode.E))
		{
			Force(new Vector3(1f, 1f), transform.up);
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			Force(new Vector3(1f, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			Force(new Vector3(-1f, -1f));
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			Force(new Vector3(0, -1f, 2), transform.forward * -1);
		}
	}

	private void Force(Vector3 offset)
	{
		Force(offset, transform.forward + Vector3.up * .1f);
	}

	private void Force(Vector3 offset, Vector3 direction)
	{
		var fwd = transform.forward;
		fwd.Normalize();

		var q = Quaternion.LookRotation(fwd);
		offset = (q * offset);

		var forcePosition = this.transform.position - fwd + offset;
		body.AddForceAtPosition(direction, forcePosition, ForceMode.Impulse);

        playMovement();
    }

    private void playMovement()
    {
        if (movement.Count == 0)
        {
            return;
        }

        movementAudioSource.clip = movement[movementIndices[movementIndex]];
        movementAudioSource.Play();

        movementIndex++;
        if (movementIndex == movement.Count)
        {
            movementIndex = 0;
            ShuffleList(movementIndices);
        }
    }

	private void OnCollisionStay(Collision collision)
	{
		if (collision.collider.tag == "Obstacle" && Mathf.Abs(collision.contacts[0].separation) > .1f)
		{
			Die("You got crushed", crushed);
        }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Fatal")
		{
			Die("Stabbed", stabbed);
        }
	}

	private void Die(string why, AudioClip clip)
	{
		if (alive)
		{
            voiceAudioSource.Stop();
            voiceAudioSource.loop = false;
            voiceAudioSource.clip = clip;
            voiceAudioSource.Play();

            alive = false;
			var handler = OnDie;
			if (handler != null)
			{
				handler(why);
			}
		}
	}

    private void ShuffleList(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Panic()
    {
        if (!panicking)
        {
            if (panics.Count == 0)
            {
                return;
            }

            panicking = true;

            lastGagTime = voiceAudioSource.time;
            voiceAudioSource.Stop();
            voiceAudioSource.loop = false;
            voiceAudioSource.clip = panics[panicIndices[panicIndex]];
            voiceAudioSource.Play();

            panicIndex++;
            if (panicIndex == panics.Count)
            {
                panicIndex = 0;
                ShuffleList(panicIndices);
            }

            StartCoroutine(CalmDown());
        }
    }

    private IEnumerator CalmDown()
    {
        yield return new WaitForSeconds(14.0f);

        panicking = false;
        voiceAudioSource.Stop();
        voiceAudioSource.loop = true;
        voiceAudioSource.clip = gag;
        voiceAudioSource.time = lastGagTime;
        voiceAudioSource.Play();
    }

    public void Fall()
    {
        if (fallSoundPlaying || !alive)
        {
            return;
        }

        voiceAudioSource.mute = true;

        fallAudioSource.clip = falls[fallIndices[fallIndex]];
        fallAudioSource.Play();
        fallSoundPlaying = true;

        fallIndex++;
        if (fallIndex == falls.Count)
        {
            fallIndex = 0;
            ShuffleList(fallIndices);
        }

        StartCoroutine(FallFinished());
    }

    private IEnumerator FallFinished()
    {
        yield return new WaitForSeconds(1.2f);
        voiceAudioSource.mute = false;
        fallSoundPlaying = false;
    }
}
