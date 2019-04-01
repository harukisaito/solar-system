using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameLabel : MonoBehaviour {

	[SerializeField] private Text nameLabel;

	
	private Vector3 originalFontSize;

	public Text MoonLabel {
		get { return nameLabel; }
		set { nameLabel = value; }
	}

	public float FontSize {
		set { 
			nameLabel.transform.localScale = originalFontSize * value;
		}
	}


	private void Start() {
		nameLabel.enabled = false;
		originalFontSize = nameLabel.transform.localScale;
	}

	private void Update() {
		nameLabel.transform.position = transform.position;
		nameLabel.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	}

	
}
