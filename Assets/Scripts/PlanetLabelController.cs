using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLabelController : MonoBehaviour {

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
	public int Index {
		get {
			return index;
		}
		set {
			index = value;
			if(planetLabel == null) {
				SetupPlanetLabel();
			}
			planetLabel.Index = index;
			GetPlanet();
		}
	}

	public float DistanceToSun {
		get {
			return distanceToSun;
		}
		set {
			distanceToSun = value;
			transform.localPosition = new Vector3(
				distanceToSun,
				transform.localPosition.y,
				transform.localPosition.z
			);
		}
	}

	private int index;
	private float radius;
	private float distanceToSun;
	private PlanetManager planet;
	private PlanetLabel planetLabel;

	


	private void GetPlanet() {
		planet = SolarSystemController.Instance.Planets[index];

		transform.localScale = planet.transform.localScale;
		transform.localPosition = planet.transform.localPosition;
	}

	private void SetupPlanetLabel() {
		GameObject labelGameObject = new GameObject();
		labelGameObject.name = "Label";
		planetLabel = labelGameObject.AddComponent<PlanetLabel>();
		planetLabel.transform.SetParent(transform);
	}

	public void SetLabelSize(State currentState) {
		planetLabel.SetSize(currentState);
	}
	
}
