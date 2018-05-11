using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {

	public float time;

	private TextMeshPro mText;
	private Color mInitialColor;
	private Color mFinalColor;
	private float mInitialTime;

	void Awake () {
		mText = GetComponent<TextMeshPro>();
		General.GroupGameObjects(transform, "HUD");
		mText.color = new Color(0f, 0f, 0f, 0f);
	}

	public void Configure (string text, Color color) {
		mText.text = text;
		mInitialColor = color;
		mFinalColor = new Color(color.r, color.g, color.b, 0f);

		mInitialTime = Time.time;

		Destroy(gameObject, time + 1f);
	}
	
	void Update () {
		float t = (Time.time - mInitialTime) / time;

		mText.color = Color.Lerp(mInitialColor, mFinalColor, t);
		transform.Translate(Vector3.up * Time.deltaTime);
	}
}
