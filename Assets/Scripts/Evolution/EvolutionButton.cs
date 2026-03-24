using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EvolutionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public Evolution evolution;
    public EvolutionMenu menu;
    public GameObject description;
    public EvolutionButton requiresButton;
    public bool purchased;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(() => menu.ClickEvolution(evolution));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        description.SetActive(false);
    }

    public void Check()
    {
        if (requiresButton)
        {
            Debug.Log(requiresButton.purchased);
            //button.interactable = evolution.requiresButton.purchased;
        }
    }

    private void OnEnable()
    {
        description.SetActive(false);
    }
}
