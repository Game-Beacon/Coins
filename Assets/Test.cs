using UniRx;
using UnityEngine;


public class Test : MonoBehaviour
{
    private bool mIsDead;

    void Start()
    {
        ReactiveProperty<int> hp = new IntReactiveProperty(100);

        hp.Subscribe(Next,Complete);

        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (mIsDead == false) hp.Value -= 1;
        });
    }

    void Next(int hp)
    {
        mIsDead = hp < 1;
        Debug.LogFormat("isDead:{0}", mIsDead);
    }
    void Complete()
    {
        Debug.LogFormat("Complete");
    }

}

