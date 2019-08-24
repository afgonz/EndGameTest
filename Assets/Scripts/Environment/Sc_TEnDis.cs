using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_TEnDis : MonoBehaviour
{
    [SerializeField]
    private GameObject go_object2Disable, go_obstacle = null;
    [SerializeField]
    private Animator a_anim;

    public void OnStart()
    {
        go_object2Disable.SetActive(true);
        go_obstacle.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (other.GetComponent<Player>().b_gotKey)
            {
                if (a_anim != null)
                {
                    go_obstacle.SetActive(false);
                    a_anim.SetBool("Open", true);
                }
                else
                    go_object2Disable.SetActive(false);
            }
            else
            {
                go_object2Disable.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            go_object2Disable.SetActive(true);
        }
    }
}
