using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector]
    public bool pushed = false;
    [SerializeField]
    private Image i_jsBack, i_jsBut;
    private RectTransform r_jsRect;
    private Vector3 v_screenSize;
    [HideInInspector]
    public Vector3 v_jsVect;
    [SerializeField]
    private Vector3 v_posCorrection;
    public delegate void PushJS();
    public event PushJS OnPush, OnRelease;

    private void Start()
    {
        r_jsRect = i_jsBut.GetComponent<RectTransform>();
        v_screenSize = new Vector2(1920.0f / Screen.width, 1080.0f / Screen.height);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 inputJS = (eventData.position * v_screenSize - new Vector2(v_posCorrection.x, v_posCorrection.y)) / 150.0f;
        v_jsVect = (inputJS.magnitude > 1.0f) ? inputJS.normalized : inputJS;
        i_jsBut.rectTransform.localPosition = v_jsVect * 100.0f + new Vector3(i_jsBack.rectTransform.pivot.x == 0? 150 : -150, v_posCorrection.y / 2, 0.0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnPush != null)
            OnPush();
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(OnRelease != null)
            OnRelease();
        v_jsVect = Vector3.zero;
        i_jsBut.rectTransform.anchoredPosition = Vector3.zero;
    }
}
