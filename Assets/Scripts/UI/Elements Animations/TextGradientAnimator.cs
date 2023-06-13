using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class TextGradientAnimator : AnimatedUI
{
    private TMP_Text _text;
    private Color[] _vertexColors;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _vertexColors = new Color[4];

        _vertexColors[0] = _text.colorGradient.topLeft;
        _vertexColors[1] = _text.colorGradient.topRight;
        _vertexColors[2] = _text.colorGradient.bottomLeft;
        _vertexColors[3] = _text.colorGradient.bottomRight;

        StartAnimation();
    }

    protected override IEnumerator Animating()
    {
        while (true)
        {
            _vertexColors.ShiftArray();

            float timer = 0;

            while (timer < 2)
            {
                _text.colorGradient = new VertexGradient(
                    Color.Lerp(_text.colorGradient.topLeft, _vertexColors[0], Time.unscaledDeltaTime * AnimationSpeed), 
                    Color.Lerp(_text.colorGradient.topRight, _vertexColors[1], Time.unscaledDeltaTime * AnimationSpeed), 
                    Color.Lerp(_text.colorGradient.bottomLeft, _vertexColors[2], Time.unscaledDeltaTime * AnimationSpeed), 
                    Color.Lerp(_text.colorGradient.bottomRight, _vertexColors[3], Time.unscaledDeltaTime * AnimationSpeed)
                    );

                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}