using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {

	private NoteProperties prop;

	private bool active = false;
	private bool disabled = false;
	private float timeToHit = -1.0f;
	private int difficulty = -1;
	public enum scoreRating {Perfect = 100, Good = 70, Bad = 40, Missed = 0, Unset = -1};
	private scoreRating result = scoreRating.Unset;



	// Use this for initialization
	void Awake () {
		prop = this.GetComponent<NoteProperties> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			timeToHit -= Time.deltaTime;
			if (timeToHit < spillover ()) {
				setScore ();
				disable ();
			}
		}

		if (disabled) {
			timeToHit -= Time.deltaTime;
			if (timeToHit < -4.0f) {
				Destroy (gameObject);
			}
		}
	}

	void OnMouseDown()
	{
		if(active)
		{
			setScore ();
			Debug.Log ("Score: " + result + ", with time remaining: " + timeToHit);
			GetComponentInParent<PatternLibrary> ().reportScore ((int)getScore());
			disable ();
		}
	}

	/// <summary>
	/// Note initialization function.
	/// <param name="c">Color of the note.</param>
	/// <param name="pos">Position of the note in x,z coordinates.</param>
	/// <param name="numeral">Numeral on the face of the note.</param>
	/// <param name="time">Time until note should be struck in seconds.</param>
	/// <param name="diff">Difficulty on a scale from 1 - 5. Affects precision required.</param>
	/// 
	public void setup(Color c, Vector3 pos, int numeral, float time, int diff = 3)
	{
		prop.setColor (c);
		prop.setFade (true, time/3.0f);
		prop.setCountdown (time);
		prop.setNumeral (numeral);
		setActive (time, diff);
		prop.setPosition (pos);
	}

	private void setActive(float time, int diff)
	{
		active = true;
		timeToHit = time;
		difficulty = diff;
	}

	private void disable()
	{
		active = false;
		disabled = true;
		timeToHit = -1.0f;
		difficulty = -1;
		prop.setFade (false, 0.3f);
	}

	/// <summary>
	/// Determines time a player can still hit a note after its perfect hit window has passed
	/// </summary>
	private float spillover()
	{
		return -0.080f - (0.010f*difficulty);
	}

	/// <summary>
	/// Formula for determining what counts as a "bad" hit score.
	/// </summary>
	/// <returns>The window in seconds.</returns>
	private float badWindow()
	{
		return (0.25f + (0.05f * (5 - difficulty)/5));
	}

	/// <summary>
	/// Formula for determining what counts as a "good" hit score.
	/// </summary>
	/// <returns>The window in seconds.</returns>
	private float goodWindow()
	{
		return (0.175f + (0.04f * (5 - difficulty)/5));
	}

	/// <summary>
	/// Formula for determining what counts as a "perfect" hit score.
	/// </summary>
	/// <returns>The window.</returns>
	private float perfectWindow()
	{
		return (0.1f + (0.03f * (5 - difficulty)/5));
	}

	/// <summary>
	/// Calculates the score.
	/// </summary>
	/// <returns>The score.</returns>
	private scoreRating calcScore()
	{
		
		if (timeToHit < 0  && timeToHit > spillover()) {
			return scoreRating.Good;
		}

		if (timeToHit < perfectWindow ()) {
			return scoreRating.Perfect;
		} else if (timeToHit < goodWindow ()) {
			return scoreRating.Good;
		} else if (timeToHit < badWindow ()) {
			return scoreRating.Bad;
		} else {
			return scoreRating.Missed;
		}
	}

	/// <summary>
	/// Helper function to update the score based on the calculation of the current score
	/// </summary>
	private void setScore()
	{
		result = calcScore ();
	}

	public scoreRating getScore()
	{
		return result;
	}


}
