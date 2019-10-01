using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoonLabel : MonoBehaviour {

	[SerializeField] private int index;
	[SerializeField] private Planet planetIndex;

	private Vector3 distanceSize = new Vector3(0.02f, 0.02f, 0.02f);

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
			moonLabel = UIManager.Instance.MoonLabels[planetIndex][index];
			moonLabel.enabled = false;
		}
	}

	public Planet PlanetIndex {
		get {
			return planetIndex;
		}
		set {
			planetIndex = value;
		}                
	}

	public Text Text {
		get {
			return moonLabel;
		}
		set {
			moonLabel = value;
		}
	}
	
	private Text moonLabel;

	private void Update() {
		if(moonLabel != null) {
			moonLabel.transform.position = transform.position;
			moonLabel.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
		}
	}

	public void SetSize(State currentState) {
		if(currentState == State.RelativeDistance) {
			moonLabel.transform.localScale = distanceSize;
		}
	}
}
