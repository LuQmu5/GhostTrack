﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Text))]
public class TextHoverAnimator : AnimatedUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScaleSize = 1.1f;
    [SerializeField] private Color _hoverColor = Color.white;

    private Text _text;

    private Color _baseColor;
    private Vector3 _baseScale;

    protected override void OnEnable()
    {
        _text = GetComponent<Text>();

        _baseColor = _text.color;
        _baseScale = _text.transform.localScale;

        base.OnEnable();
    }

    protected override IEnumerator Animating()
    {
        while (true)
        {
            _text.color = _hoverColor;
            _text.transform.DOScale(_hoverScaleSize, 1 / AnimationSpeed);
            yield return new WaitForSecondsRealtime(1 / AnimationSpeed);

            _text.color = _hoverColor;
            _text.transform.DOScale(_baseScale, 1 / AnimationSpeed);
            yield return new WaitForSecondsRealtime(1 / AnimationSpeed);
        }
    }

    public override void StopAnimation()
    {
        base.StopAnimation();

        _text.color = _baseColor;
        _text.transform.DOScale(_baseScale, 1 / AnimationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAnimation();
    }
}