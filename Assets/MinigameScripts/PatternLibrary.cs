using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternLibrary : MonoBehaviour {

	public GameObject round;
	//temp value--BPM = 60/BPM. In this case, BPM = 100
	private float bpm = .6f;
	private float timer;
	private int numeralCounter = 1;

	public const float xBound = 11.0f;
	public const float zBound = 4.25f;

	//temp canvas for simple score display, will be removed later
	public Text temp;
	private int score;

	// Use this for initialization
	void Start () {
		timer = bpm;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			timer += (bpm / 4.0f) * (Random.Range (4, 12));
			singleRound(new Color(Random.value, Random.value, Random.value), 
						new Vector3(Random.Range(-xBound, xBound), 0, Random.Range(-zBound, zBound)), 
						numeralCounter++%10,
						bpm*3);
		}
	}

	public void singleRound(Color c, Vector3 pos, int numeral, float time, int diff = 3)
	{
		GameObject newRound = Instantiate(round, this.transform);
		NoteController nc = newRound.GetComponent<NoteController>();

		nc.setup (c, pos, numeral, time, diff);
	}

	public void reportScore(int _score)
	{
		score += _score;
		temp.text = "Score : " + score;
	}
		
}
