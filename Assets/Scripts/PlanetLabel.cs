using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetLabel : MonoBehaviour {

	public int Index {
		get {
			return  index;
		}
		set {
			index = value;
			planetButton = UIManager.Instance.PlanetButtons[index];
		}
	}

	private int index;

	private Button planetButton;

	private Vector3 normalSize = new Vector3(0.5f, 0.5f, 0.5f);
	private Vector3 distanceSize = new Vector3(0.05f, 0.05f, 0.05f);

	private void Start() {
		transform.localPosition = transform.localPosition + Vector3.up;

	}

	private void Update() {
		if(planetButton != null) {
			planetButton.transform.position = transform.position;
			planetButton.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
		}
	}

	public void SetSize(State currentState) {
		if(currentState == State.Normal) {
			planetButton.transform.localScale = normalSize;
		}
		else if(currentState == State.RelativeDistance) {
			planetButton.transform.localScale = distanceSize;
		}
	}
}
