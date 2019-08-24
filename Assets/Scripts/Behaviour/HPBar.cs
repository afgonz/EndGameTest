using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Transform t_HPindicator;
    [SerializeField]
    private TextMeshPro tx_HPText;
    [SerializeField]
    private float f_HPValue = 1000.0f;

    public float SetHPValue { set { f_HPValue = value; UpdateValue(); } get { return f_HPValue; } }
    public float ChangeHPValue
    {
        set
        {
            f_HPValue -= value;
            if(f_HPValue < 0)
            {
                f_HPValue = 0;
            }
            UpdateValue();
        }
    }

    private void UpdateValue()
    {
        tx_HPText.text = f_HPValue.ToString();
        t_HPindicator.localScale = new Vector3(f_HPValue / 1000.0f, 1.0f, 1.0f);
    }
}
