using System;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
public class BuffUnit : MonoBehaviour
{

    [SerializeField] Text text;
    [SerializeField] Image image;
    [SerializeField] Text content;
    [SerializeField] LayoutElement element;
    public BuffUnit()
    {
        Guid = Guid.NewGuid();
    }
    public Guid Guid { get; }
    internal Guid Set(Buff buff)
    {
        SetBG(buff.PositiveBuff);
        SetTurn(buff.Value);
        SetContent(buff.GetContent());
        return Guid;
    }

    private void SetContent(string value)
    {
        content.text = value;
    }

    private void SetTurn(int value)
    {
        text.text = value.ToString();
    }

    internal void SetActive(bool active)
    {
        gameObject.SetActive(active);
        element.ignoreLayout = !active;
    }

    private void SetBG(bool positiveBuff)
    {
        image.color = positiveBuff? Color.green:Color.red;
    }

    internal void Invoke()
    {
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.5f).SetEase(Ease.InExpo));
        moveSequence.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo));
        moveSequence.PlayForward();
    }


    public void OnMouseEnter()
    {
        content.gameObject.SetActive(true);
    }
    public void OnMouseExit()
    {
        content.gameObject.SetActive(false);
    }
}
