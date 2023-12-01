using System;
using System.Collections;
using System.Collections.Generic;
using Abstractions;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyCounterController : MonoBehaviour
{
    [SerializeField]
    private DressableCharacter playerChar;
    
    [SerializeField]
    private TMP_Text moneyCounter;

    [SerializeField]
    private Color earningColor, spendingColor;

    [SerializeField]
    private float tweenDuration;

    private void Awake()
    {
        moneyCounter.text = $"${playerChar.money}";

        playerChar.OnMoneyChanged += AnimateMoneyCounter;
    }

    private void OnDestroy()
    {
        playerChar.OnMoneyChanged -= AnimateMoneyCounter;
    }

    private void AnimateMoneyCounter(int val)
    {
        int oldValue = playerChar.money;

        moneyCounter.color = val > 0 ? earningColor : spendingColor;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(moneyCounter.DOColor(Color.white, tweenDuration));
        seq.Join(DOTween.To(() => oldValue, x => oldValue = x, oldValue + val, tweenDuration)
            .OnUpdate(() =>
            {
                moneyCounter.text = $"${oldValue}";
            }));
        seq.Play();
    }
}
