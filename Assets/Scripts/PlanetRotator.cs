using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour {

	public float orbitalPeriodInSeconds;
	private float inclinationAngle;

	public float InclinationAngle {
		get {
			return inclinationAngle;
		}
		set {
			inclinationAngle = value;
			transform.localRotation = 
				Quaternion.Euler(
					transform.localEulerAngles.x,
					transform.localEulerAngles.y, 
					inclinationAngle
				);
		}
	}

	private void Awake() {
		transform.SetParent(SolarSystemController.Instance.transform);
	}

	private void Update () {
		float rotation = (360 / orbitalPeriodInSeconds) * Time.deltaTime * SolarMetrics.speed;
		transform.rotation *= Quaternion.Euler(0, rotation, 0);
		// timePassed += (360 / orbitalPeriodInSeconds) * Time.deltaTime * speed;
		// if(timePassed % 365 >= 1 && gameObject.name == "Earth") {
		// 	timePassed -= 365;
		// 	yearCounter ++;
		// 	SolarSystemController.Instance.ButtonController.Texts[0].text = "Year Counter: " + yearCounter;
		// }
	}
}
