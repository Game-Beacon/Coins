using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCardsUI : MonoBehaviour
{
    [SerializeField] private GameObject Card;
    [SerializeField] private HorizontalLayoutGroup grid;
    private List<CardUI> cardUIs = new List<CardUI>();
    public CardUI ReturnNouseCard()
    {
        return AddNewGameObject();
    }

    CardUI AddNewGameObject()
    {
        GameObject GO= Instantiate(Card,grid.transform);
        CardUI cardUI = GO.GetComponent<CardUI>();
        cardUIs.Add(cardUI);
        return cardUI;
    }
}
