using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandCardsUI : MonoBehaviour,CardsUI
{
    public bool player;
    [SerializeField] private CardUI cardPrefab;
    [SerializeField] private Transform dropCardPoint;
    [Range(0, 5000)] [SerializeField] private int length = 500;
    [Range(0, 360)] [SerializeField] private float defaultCardAngle;
    [Range(0, 5000)] [SerializeField] private float handCardLength;
    private LinkedList<CardUI> cardUIs = new LinkedList<CardUI>();
    private Queue<CardUI> notUseCards = new Queue<CardUI>();
    public int SetCardAndReturnUniqueID(Card card, CardUIInfo info)
    {
        var useCard = ReturnNotUseCard();
        useCard.transform.localPosition = dropCardPoint.localPosition;
        useCard.transform.localRotation = dropCardPoint.localRotation;
        useCard.transform.SetAsLastSibling();
        var uiID = useCard.SetCard(card, info);
        useCard.gameObject.SetActive(true);
        ArrangeCards();
        return uiID;
    }
    private CardUI ReturnNotUseCard()
    {
        CardUI cardUI;
        if (notUseCards.Count>0)
        {
            cardUI = notUseCards.Dequeue();
            cardUIs.AddLast(cardUI.node);
        }
        else
        {
            cardUI=Instantiate(cardPrefab,transform);
            var node=cardUIs.AddLast(cardUI);

            cardUI.Initialize(node, player);
        }
        return cardUI;
    }
    private void ArrangeCards()
    {
        float cardAngle = GetCardAngle();
        Arrange(cardAngle);
    }
    private float GetCardAngle()
    {
        float cardsCount = cardUIs.Count;
        float defaultCardAllAngles = cardsCount * defaultCardAngle;
        bool IsCardAngelBiggerthanRange = defaultCardAllAngles > MaxAngleRange;
        float cardAngle = defaultCardAngle;
        if (IsCardAngelBiggerthanRange)
        {
            if (cardsCount>1)
            {
                cardAngle = MaxAngleRange / (cardsCount - 1);
            }
            else
            {
                cardAngle = 0;
            }
        }
        return cardAngle;
    }
    private float MaxAngleRange
    {
        get
        {
            if (length <= 0 || handCardLength < 0)
            {
                return 0;
            }
            if (handCardLength > length)
            {
                return 0;
            }
            float angle = 2 * Mathf.Asin(handCardLength / length) * 180 / Mathf.PI;
            return angle;
        }
    }
    private void Arrange(float cardAngle)
    {
        if (cardAngle < 0)
        {
            Debug.LogError($"cardAngle must biggerthan 0");
            return;
        }
        int cardCount = cardUIs.Count;
        if (cardCount <= 0)
        {
            return;
        }
        float angleSum = (cardCount - 1) * cardAngle;
        float startAngle = angleSum / 2;
        float tempYaxisAngle = startAngle;
        var tempNode = cardUIs.First;
        for (int i = 0; i < cardCount; i++)
        {
            Quaternion quaternion = Quaternion.Euler(0, 0, tempYaxisAngle);
            var pos = GetCardPosition(quaternion);
            tempNode.Value.SetCardPosition(pos,quaternion);
            tempNode = tempNode.Next;
            tempYaxisAngle -= cardAngle;
        }
    }
    private Vector3 GetCardPosition(Quaternion angle)
    {
        var direction = angle * new Vector3(0, 1, 0);
        var finalPosition = direction * length - new Vector3(0, length, 0);
        return finalPosition;
    }

    public void RecycleCard(int uiID)
    {
        foreach (var item in cardUIs)
        {
            if (item.GetInstanceID()==uiID)
            {
                var cardUI = item;
                cardUI.gameObject.SetActive(false);
                cardUIs.Remove(cardUI.node);
                notUseCards.Enqueue(cardUI);
                ArrangeCards();
                break;
            }
        }
    }

}
