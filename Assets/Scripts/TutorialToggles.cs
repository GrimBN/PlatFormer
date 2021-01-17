using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialToggles : MonoBehaviour
{

    [SerializeField] Toggle tutorialTextToggle, toolTextToggle;

    public void ToggleTheToggle()
    {
        tutorialTextToggle.isOn = !tutorialTextToggle.isOn;
        toolTextToggle.isOn = !toolTextToggle.isOn;
    }
 
}
