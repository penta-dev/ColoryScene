using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustColor : MonoBehaviour
{
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    [SerializeField] private float _duration;

    private int _index;
    private Renderer _renderer;
    private float _timeLeft;
    void Start()
    {
        _index = 0;
        _timeLeft = _duration;
        _renderer = gameObject.GetComponent<Renderer>();
    }

    
    void Update()
    {
        Color color;
        if (_timeLeft < Time.deltaTime)
        {
            _index++;
            _timeLeft = _duration;

            color = _index % 2 == 0 ? _firstColor : _secondColor;
            _renderer.material.SetColor("_Color", color);
            _renderer.material.SetColor("_EmissionColor", color *2);

        }
        else
        {
            color = Color.Lerp(_renderer.material.color, _index % 2 == 0 ? _secondColor : _firstColor, Time.deltaTime / _timeLeft);
            _renderer.material.SetColor("_Color", color);
            _renderer.material.SetColor("_EmissionColor", color *2);

            _timeLeft -= Time.deltaTime;
        }

        Debug.Log(color);
    }
}
