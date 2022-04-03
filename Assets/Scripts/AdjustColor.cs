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
        _renderer.material.EnableKeyword("_EMISSION");
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
            _renderer.material.SetColor("_EmissionColor", color);
            SetEmissionIntensity(_renderer.material, 2);

        }
        else
        {
            Color from, to;
            if (_index % 2 == 0)
            {// first->second
                from = _firstColor;
                to = _secondColor;
            }
            else
            {//second->first
                from = _secondColor;
                to = _firstColor;
            }

            if (_timeLeft < _duration / 3)
            {// blue change
                float ratio = 1 - _timeLeft / (_duration / 3); // 0 ~ 1
                float b = from.b + (to.b - from.b) * ratio;
                color = new Color(to.r, to.g, b);
                Debug.Log("B change: " + b + color);
            }
            else if (_timeLeft < _duration * 2 / 3)
            {// green change
                float ratio = 1 - (_timeLeft - _duration / 3) / (_duration / 3); // 0 ~ 1
                float g = from.g + (to.g - from.g) * ratio;
                color = new Color(to.r, g, from.b);
                Debug.Log("G change: " + g + color);
            }
            else
            {// red change
                float ratio = 1 - (_timeLeft - _duration * 2 / 3) / (_duration / 3); // 0 ~ 1
                float r = from.r + (to.r - from.r) * ratio;
                color = new Color(r, from.g, from.b);
                Debug.Log("R change: " + r + color);
            }

            _renderer.material.SetColor("_Color", color);
            _renderer.material.SetColor("_EmissionColor", color);
            SetEmissionIntensity(_renderer.material, 1);

            _timeLeft -= Time.deltaTime;
        }
    }
    //https://forum.unity.com/threads/setting-material-emission-intensity-in-script-to-match-ui-value.661624/
    private void SetEmissionIntensity(Material mat, float intensity)
    {
        Color color = mat.GetColor("_Color");

        // for some reason, the desired intensity value (set in the UI slider) needs to be modified slightly for proper internal consumption
        float adjustedIntensity = intensity - (0.4169F);

        // redefine the color with intensity factored in - this should result in the UI slider matching the desired value
        color *= Mathf.Pow(2.0F, adjustedIntensity);
        mat.SetColor("_EmissionColor", color);
        //Debug.Log("<b>Set custom emission intensity of " + intensity + " (" + adjustedIntensity + " internally) on Material: </b>" + mat);
    }
}
