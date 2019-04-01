using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLabelController : MonoBehaviour {

	[SerializeField] private PlanetManager planet;

	private void Awake() {
		transform.localScale = planet.transform.localScale;
		transform.localPosition = planet.transform.localPosition;
	}

	private void Update() {
		if(transform.localScale != planet.transform.localScale) {
			transform.localScale = planet.transform.localScale;
		}
		if(transform.localPosition != planet.transform.localPosition) {
			transform.localPosition = planet.transform.localPosition;
		}
	}
}
