using System;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public static EventManager Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Subscribe() {
        foreach(var planet in SolarSystemController.Instance.Planets) {
            UIManager.Instance.SettingSpeed += planet.OnSettingSpeed;
        }
    }
}