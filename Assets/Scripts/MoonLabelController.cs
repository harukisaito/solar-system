using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLabelController : MonoBehaviour {

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
			if(moonLabel == null) {
				CreateMoonLabel();
			}
		}
	}

	public Planet PlanetIndex {
		get {
			return planetIndex;
		}
		set {
			planetIndex = value;
			if(moonLabel == null) {
				CreateMoonLabel();
			}
			moonLabel.PlanetIndex = planetIndex;
			moonLabel.Index = index;
		}
	}

	public float Radius {
		get {
			return radius;
		}
		set {
			radius = value;
			float diameter = radius * 2;
			transform.localScale = new Vector3(
				diameter,
				diameter,
				diameter
			);
		}
	}

	public float DistanceToPlanet {
		get {
			return distanceToPlanet;
		}
		set {
			distanceToPlanet = value;
			transform.localPosition = new Vector3(
				distanceToPlanet,
				transform.localPosition.y,
				transform.localPosition.z
			);
		}
	}
	private int index;
	private float radius;
	private float distanceToPlanet;
	private Planet planetIndex;
	private MoonLabel moonLabel;

	private void CreateMoonLabel() {
		GameObject empty = new GameObject();
		moonLabel = empty.AddComponent<MoonLabel>();
		moonLabel.name = "Moon Label";
		moonLabel.transform.SetParent(transform);
		moonLabel.transform.localPosition = new Vector3(0, 1, 0);
		moonLabel.transform.localRotation = Quaternion.identity;
	}

	public void SetupMoonLabel(MoonManager moon) {
		transform.localScale = moon.transform.localScale;
		transform.localPosition = moon.transform.localPosition;
	}

	public void Show(bool enabled) {
		if(moonLabel == null) {
			moonLabel = GetComponent<MoonLabel>();
		}
		moonLabel.Text.enabled = enabled;
	}

	public void SetSize(State currentState) {
		moonLabel.SetSize(currentState);
	}
}
