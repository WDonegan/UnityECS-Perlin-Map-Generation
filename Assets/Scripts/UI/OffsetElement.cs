using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetElement : MonoBehaviour {


    public SliderElement xAxis, yAxis;
    private Toggle Link;

    public float X;
    public float Y;

    void Start()
    {
        Link = GetComponentInChildren<Toggle>();
    }

    public void UpdateXSlider(float f)
    {
        xAxis.SetValue(f);
        X = f;

        if (Link.isOn)
        {
            yAxis.SetValue(f);
            Y = f;
        }
    }

    public void UpdateYSlider(float f)
    {
        yAxis.SetValue(f);
        Y = f;

        if (Link.isOn)
        {
            xAxis.SetValue(f);
            X = f;
        }
    }

    public void UpdateXInput(string s)
    {
        var f = float.Parse(s);

        xAxis.SetValue(f);
        X = f;
        if (Link.isOn)
        {
            yAxis.SetValue(f);
            Y = f;
        }
    }

    public void UpdateYInput(string s)
    {
        var f = float.Parse(s);

        yAxis.SetValue(f);
        Y = f;
        if (Link.isOn)
        {
            xAxis.SetValue(f);
            X = f;
        }
    }

    public void SetValue(float x, float y)
    {
        xAxis.SetValue(x);
        X = x;

        yAxis.SetValue(y);
        Y = y;
    }
}
