using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAssigner : MonoBehaviour
{
    public static MaterialAssigner Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Material[] planetMaterials;

    public Material[] PlanetMaterials {
        get {
            return planetMaterials;
        }
    }
}
