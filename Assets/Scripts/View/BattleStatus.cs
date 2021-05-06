using UnityEngine;
using UnityEngine.UI;

public class BattleStatus : IUserInterface
{
    [SerializeField] private Text hp;
    [SerializeField] private Text eneragy;
    public void SetHp(int value)
    {
        hp.text = value.ToString();
    }
    public void SetEneragy(int value)
    {
        eneragy.text = value.ToString();
    }
}