using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float f_speed = 0.1f, f_lifetime = 10.0f;    

    public void OnEnable()
    {
        StartCoroutine(MoveFwd());
    }

    private IEnumerator MoveFwd()
    {
        float f_life = 0;
        while (f_life < f_lifetime)
        {
            transform.Translate(Vector3.forward * f_speed * Time.deltaTime);
            f_life += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Wall")
            gameObject.SetActive(false);
    }
}
