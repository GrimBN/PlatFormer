using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private Button buttonComponent;
    [SerializeField] private TextMeshProUGUI buttonText;

    private void OnGUI() 
    {
        buttonComponent = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        int levelNumber = transform.GetSiblingIndex() + 1;
        buttonText.text = levelNumber.ToString();

    }

    private void Start()
    {
        LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

        buttonComponent.onClick.AddListener(() => levelLoader.LoadGivenScene(transform.GetSiblingIndex() + 1));
        
    }
}
