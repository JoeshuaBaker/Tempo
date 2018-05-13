using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour {

	//variables to control the properties of our circle. 
	private float radius = 3.0f;			//the circle will be the size of the note when this radius = 0
	private const float minScale = 0.27f;	//this is the smallest the scale value should ever go
	private const float minRadius = 1.0f;	//this radius gets added to the actual radius such that 0 = rad of note;

	//variables that change the granularity of our note circle (how many vertices)
	private int Size;
	private float ThetaScale = 0.04f;
	private float Theta = 0f;

	//lastRadius tracks whether or not we should redraw our circle when the radius is changed
	private float lastRadius = 0.0f;

	//variables to interpolate the scale of the circle based on the countdown time
	private float lerp = 0.0f;
	private float originalTime = 0.0f;
	private float remainingTime = 0.0f;
	private bool countdownActive = false;

	//variables to fade the object in and out
	private bool fading = false;
	private bool fadeIn = false;
	private float fadeLerp = 0.0f;
	private float originalFadeTime = 0.0f;
	private float remainingFadeTime = 0.0f;

	//the LineRenderer Component needed to draw the circle. Init in start()
	private LineRenderer LineDrawer;
	private NoteProperties np;
	private Transform circleTrans;

	void Awake ()
	{       
		LineDrawer = GetComponentInChildren<LineRenderer>();
		np = GetComponent<NoteProperties> ();

		circleTrans = transform.Find ("TimeCircle");
	}

	void Update ()
	{   
		//only redraw the line if we've changed our radius since the last frame
		if (radius != lastRadius) {
			drawCircle ();
		}
		lastRadius = radius;

		if (countdownActive) {
			remainingTime -= Time.deltaTime;
			if (remainingTime > 0.0f) {
				lerp = remainingTime / originalTime;
				setScale (lerp);
			} else {
				lerp = 0.0f;
				remainingTime = 0.0f;
				originalTime = 0.0f;
				countdownActive = false;
			}

		}

		if (fading) {
			remainingFadeTime -= Time.deltaTime;
			fadeLerp = remainingFadeTime / originalFadeTime;
			if (fadeIn) {
				np.setTransparency (1f - fadeLerp);
			} else {
				np.setTransparency (fadeLerp);
			}

			if (remainingFadeTime < 0f) {
				fading = false;
				remainingFadeTime = 0.0f;
				originalFadeTime = 0.0f;
				fadeLerp = 0.0f;
			}
		}
	}



	/// <summary>
	///basically does a for loop that iterates around a unit circle and mutliplies the coordinates by sin/cos
	///and the radius to get each vertex of the circle.
	/// </summary>
	private void drawCircle()
	{
		Theta = 0f;
		Size = (int)((1f / ThetaScale) + 1f);
		LineDrawer.positionCount = Size + 1; 
		for(int i = 0; i < Size; i++){          
			Theta += (2.0f * Mathf.PI * ThetaScale);         
			float x = (minRadius + radius) * Mathf.Cos(Theta);
			float y = (minRadius + radius) * Mathf.Sin(Theta);          
			LineDrawer.SetPosition(i, new Vector3(x, 0, y));
		}

		Theta = 2.0f * Mathf.PI * ThetaScale;
		LineDrawer.SetPosition(Size, new Vector3((minRadius + radius)*Mathf.Cos(Theta), 0, (minRadius + radius)*Mathf.Sin(Theta)));
	}

	/// <summary>
	/// Translates an incoming scale of 0-1f to 0.25-1f, such that a 0f represents
	/// the minimum radius of our note ring
	/// </summary>
	/// <param name="f">F.</param>
	private void setScale(float f)
	{
		float transf = ((minScale * (1 - f)) + f);
		Vector3 newScale = new Vector3 (transf, transf, transf);

		circleTrans.localScale = newScale;
	}

	public void setFade(bool _fadeIn, float timeInSeconds)
	{
		fadeIn = _fadeIn;
		originalFadeTime = timeInSeconds;
		remainingFadeTime = timeInSeconds;
		fading = true;
		fadeLerp = 1.0f;

	}

	public void setCountdown(float timeInSeconds)
	{
		countdownActive = true;
		originalTime = timeInSeconds;
		remainingTime = timeInSeconds;
		lerp = 1.0f;
	}
}
