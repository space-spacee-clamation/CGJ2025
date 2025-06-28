using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] private RoundImage _image;
    private void Update()
    {
        _image.SetProgress(TimeController.Instance.Time/TimeController.Instance.MaxTime);
    }
}
