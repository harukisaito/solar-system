using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

	[SerializeField] private Button[] buttons; 
	[SerializeField] private Button[] planetButtons; 
	[SerializeField] private Text[] texts;

	public Button[] PlanetButtons {
		get { return planetButtons; }
		set { planetButtons = value; }
	}

	public Text[] Texts {
		get { return texts; }
		set { texts = value; }
	}

	private void Awake() {
		buttons[2].interactable = false;
	}

	public void EnableButtons(bool enabled) {
		EnableFor(
			buttons,
			enabled, 
			startNumber: 0, 
			endNumber: buttons.Length);
	}

	public void EnableAllPlanetButtons(bool enabled) {
		EnableFor(
			planetButtons,
			enabled,
			0, 
			buttons.Length
		);
	}

	public void EnableMoveButton(bool enabled) {
		buttons[2].interactable = enabled;
	}

	private void EnableFor(Button[] buttons, bool enabled, int startNumber, int endNumber) {
		GetComponent();
		for(int i = startNumber; i < endNumber; i++) {
			buttons[i].interactable = enabled;
		}
	}

	private void GetComponent() {
		for(int i = 0; i < buttons.Length; i++) {
			if(buttons[i] == null) {
				buttons[i] = GetComponent<Button>();
			}
		}
	}
}
