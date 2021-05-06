using UnityEngine.UI;
using UnityEngine;
public class CardUI : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image frame;
    [SerializeField] private Text captain;
    [SerializeField] private Text content;
    [SerializeField] private Text cost;
    public void SetCard(Card card,CardUIInfo info)
    {
        bg.sprite = info.bg;
        frame.sprite = info.card;
        captain.text = card.type.ToString();
        content.text = card.GetContent();
        cost.text = card.Cost.ToString();
    }
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        transform.position += new Vector3(0.3f,0);
    }


}

