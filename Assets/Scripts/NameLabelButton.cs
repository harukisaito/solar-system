using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameLabelButton : MonoBehaviour {

	[SerializeField] private Button planetButton;
	[SerializeField] private Text planetText;

	private Vector3 originalFontSize;

	public float FontSize {
		set {
			planetButton.transform.localScale = originalFontSize * value;
		}
	}

	public Text Text {
		get { return planetText; }
		set { planetText = value; } 
	}
	

	private void Start() {
		planetButton.transform.localScale = planetButton.transform.localScale * 0.1f;
		originalFontSize = planetButton.transform.localScale;
	}

	private void Update() {
		planetButton.transform.position = transform.position;
		planetButton.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	}
}
