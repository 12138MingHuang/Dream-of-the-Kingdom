using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    private VisualElement bacground;

    private void Awake()
    {
        bacground = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Background");
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0, 1, duration, value =>
        {
            bacground.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value =>
        {
            bacground.style.opacity = value;
        }).SetEase(Ease.InQuad);
    }
}
