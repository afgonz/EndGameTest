using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public Joystick m_joystick, a_joystick;
    public GameObject go_key;
    public Animation a_score;
    public TextMeshPro tx_score;

    public void SetScore(int score)
    {
        a_score.Play();
        tx_score.text = "x" + score.ToString();
    }
}
