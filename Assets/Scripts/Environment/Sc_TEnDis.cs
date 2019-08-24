using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_TEnDis : MonoBehaviour
{
    [SerializeField]
    private GameObject go_object2Disable, go_obstacle = null, go_quest = null;
    [SerializeField]
    private Animator a_anim;

    // Set the initial state of the object
    public void OnStart()
    {
        if(go_object2Disable != null)
            go_object2Disable.SetActive(true);
        if(go_obstacle != null)
            go_obstacle.SetActive(true);
        if (a_anim != null)
        {
            a_anim.SetBool("Open", false);
            a_anim.Play("Closed");
        }
    }

    // Check the collision with other objects
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
                if(go_quest != null)
                    go_quest.GetComponent<Animation>().Play();
                go_object2Disable.SetActive(true);
            }
        }
    }

    // Reset the initial state of the object if the player left
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            go_object2Disable.SetActive(true);
        }
    }
}
