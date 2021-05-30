using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class CardUI : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image frame;
    [SerializeField] private Text captain;
    [SerializeField] private Text content;
    [SerializeField] private Text cost;
    private HandCardsUI handCardsUI;
    public LinkedListNode<CardUI> node { get; private set; }

    public void Initialize(HandCardsUI handCardsUI,LinkedListNode<CardUI> linkedListNode)
    {
        this.handCardsUI = handCardsUI;
        this.node = linkedListNode;
    }
    public int SetCard(Card card,CardUIInfo info)
    {
        bg.sprite = info.bg;
        frame.sprite = info.card;
        captain.text = card.type.ToString();
        content.text = card.GetContent();
        cost.text = card.cost.ToString();
        return GetInstanceID();
    }

    

    public void OnMouseDrag()
    {
        transform.position = Input.mousePosition;
    }

    public void OnMouseUp()
    {
        UseCard();
    }
    void UseCard()
    {
        if (BattleSystem.PlayerUseCard(GetInstanceID()))
        {
            handCardsUI.RecycleCard(this);
        }
        
    }
}