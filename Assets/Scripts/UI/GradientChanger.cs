using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GradientChanger : MonoBehaviour
{
    [SerializeField] private float _animationTime = 2;
    [SerializeField] private float _animationSpeed = 1;

    private List<Color> _vertexColors;
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();

        _vertexColors = new List<Color>();
        _vertexColors.Add(_text.colorGradient.topLeft);
        _vertexColors.Add(_text.colorGradient.topRight);
        _vertexColors.Add(_text.colorGradient.bottomLeft);
        _vertexColors.Add(_text.colorGradient.bottomRight);

        // _text.colorGradient = new VertexGradient(Color.black, Color.black, Color.black, Color.black);

        StartCoroutine(ColorChanging());
    }

    private IEnumerator ColorChanging()
    {
        while (true)
        {
            Color[] colors = new Color[_vertexColors.Count];

            for (int i = 0; i < colors.Length; i++)
                colors[i] = _vertexColors[i];

            for (int i = 0; i < _vertexColors.Count; i++)
            {
                if (i == _vertexColors.Count - 1)
                {
                    _vertexColors[i] = colors[0];
                    continue;
                }

                _vertexColors[i] = colors[i + 1];

            }


            float timer = 0;

            while (timer < _animationTime)
            {
                _text.colorGradient = new VertexGradient(
                    Color.Lerp(_text.colorGradient.topLeft, _vertexColors[0], Time.deltaTime * _animationSpeed), 
                    Color.Lerp(_text.colorGradient.topRight, _vertexColors[1], Time.deltaTime * _animationSpeed), 
                    Color.Lerp(_text.colorGradient.bottomLeft, _vertexColors[2], Time.deltaTime * _animationSpeed), 
                    Color.Lerp(_text.colorGradient.bottomRight, _vertexColors[3], Time.deltaTime * _animationSpeed)
                    );

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}