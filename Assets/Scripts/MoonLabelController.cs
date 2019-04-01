using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLabelController : MonoBehaviour {
	[SerializeField] private MoonManager moon;

	private void Awake() {
		transform.localScale = moon.transform.localScale;
		transform.localPosition = moon.transform.localPosition;
	}

	private void Update() {
		if(transform.localScale != moon.transform.localScale) {
			transform.localScale = moon.transform.localScale;
		}
		if(transform.localPosition != moon.transform.localPosition) {
			transform.localPosition = moon.transform.localPosition;
		}
	}
}
