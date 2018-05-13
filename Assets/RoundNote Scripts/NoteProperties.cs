using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProperties : MonoBehaviour {

	private SpriteRenderer numeral;
	//singleton array for the numeral sprites
	private static Sprite[] numeralSprites;
	private LineRenderer circle;
	public const float xBound = 11.0f;
	public const float zBound = 4.25f;

	// Use this for initialization
	void Awake () {
		numeral = this.GetComponentInChildren<SpriteRenderer> ();
		if(numeralSprites == null) numeralSprites = Resources.LoadAll<Sprite> ("Numerals/");
		circle = this.GetComponentInChildren<LineRenderer> ();
		setTransparency (0.0f);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void setTransparency(float t)
	{
		//change transparency of the material of the note itself
		Color c = this.GetComponent<Renderer> ().material.color;
		c.a = t;
		this.GetComponent<Renderer> ().material.color = c;

		//change transparency of the numeral on the face of the note
		c = numeral.color;
		c.a = t;
		numeral.color = c;

		//change transparency of the circle element
		c = circle.material.color;
		c.a = t;
		circle.material.color = c;

	}

	public void setNumeral(int i)
	{
		if (i >= 0 && i < 10) {
			numeral.sprite = numeralSprites[i];
		} else {
			Debug.Log ("Tried to set an improper numeral!");
		}
	}

	public void setColor(float r, float g, float b)
	{
		//change color of note itself
		Color old = this.GetComponent<Renderer> ().material.color;
		old.r = r;
		old.g = g;
		old.b = b;
		this.GetComponent<Renderer> ().material.color = old;

		//change color of the circle component
		old = circle.material.color;
		old.r = r;
		old.g = g;
		old.b = b;
		circle.material.color = old;
	}

	public void setColor(Color c)
	{
		setColor (c.r, c.g, c.b);
	}

	public void setPosition(Vector3 p)
	{
		if (p.x > xBound || p.z > zBound)
			Debug.Log ("you probably placed a note wrong");
		this.transform.localPosition = p;
	}

	public void setScale(Vector3 s)
	{
		this.transform.localScale = s;
	}

	public void setFade(bool fadeIn, float timeInSeconds)
	{
		GetComponent<DrawController> ().setFade (fadeIn, timeInSeconds);
	}

	public void setCountdown(float timeInSeconds)
	{
		GetComponent<DrawController> ().setCountdown (timeInSeconds);
	}


}
