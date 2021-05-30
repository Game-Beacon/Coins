using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandCardsUI : MonoBehaviour,CardsUI
{
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private HorizontalLayoutGroup grid;
    private LinkedList<CardUI> cardUIs = new LinkedList<CardUI>();
    private Queue<CardUI> notUseCards = new Queue<CardUI>();
    public int SetCardAndReturnUniqueID(Card card, CardUIInfo info)
    {
        var useCard = ReturnNotUseCard();
        var id = useCard.SetCard(card, info);
        useCard.gameObject.SetActive(true);
        return id;
    }
    public CardUI ReturnNotUseCard()
    {
        CardUI cardUI;
        if (notUseCards.Count>0)
        {
            cardUI = notUseCards.Dequeue();
            cardUIs.AddLast(cardUI.node);
        }
        else
        {
            cardUI=Instantiate(cardPrefab,grid.transform);
            var node=cardUIs.AddLast(cardUI);
            cardUI.Initialize(this,node);
        }
        return cardUI;
    }

    public void RecycleCard(CardUI cardUI)
    {
        cardUI.gameObject.SetActive(false);
        cardUIs.Remove(cardUI.node);
        notUseCards.Enqueue(cardUI);
    }

}
