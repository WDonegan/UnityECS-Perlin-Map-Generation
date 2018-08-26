using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderElement : MonoBehaviour {

    public Slider Slider;
    public InputField Input;

    public float Value;

    void Start()
    {
        Slider = GetComponentInChildren<Slider>();
        Input = GetComponentInChildren<InputField>();

        SetValue(Slider.minValue);
    }

    public void UpdateSlider(float f)
    {
        SetValue(f);
    }

    public void UpdateInput(string s)
    {
        SetValue(float.Parse(s));
    }

    public void SetValue(float f)
    {
        Slider.value = f;
        Input.text = f.ToString();

        Value = f;
    }
}
