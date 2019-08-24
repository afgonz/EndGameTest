using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private G_Manager m_manager;
    [SerializeField]
    private Transform t_FireOrigin, t_targetPos, t_HPBar;
    [SerializeField]
    private NavMeshAgent n_agent;
    [SerializeField]
    private Animator a_anim;
    [SerializeField]
    private ParticleSystem p_system;
    [SerializeField]
    private float f_aspd = 2.0f;
    private bool b_firing;
    private Coroutine co_fire;
    public HPBar hpBar;

    public G_Manager SetManager { set { m_manager = value; } }

    // Set the new AI target to move or attack
    public Transform SetTarget
    {
        get { return t_targetPos; }
        set
        {
            if (t_targetPos.tag != "Player")
                Destroy(t_targetPos.gameObject);
            t_targetPos = value;
            n_agent.SetDestination(t_targetPos.position);
        }
    }

    // Initialize the enemies properties
    public void StartEnemy()
    {
        p_system.Stop();
        b_firing = false;
        StopAllCoroutines();
        if (co_fire != null)
            StopCoroutine(co_fire);
        if (t_targetPos == null)
            t_targetPos = new GameObject().transform;
        hpBar.SetHPValue = 1000.0f;
        transform.position = new Vector3(UnityEngine.Random.Range(-11.5f, 11.5f), 0, UnityEngine.Random.Range(-8.0f, 5.0f));
    }

    // Get a new target
    public void GetNewPos()
    {
        if (t_targetPos.tag == "Player")
            t_targetPos = new GameObject().transform;
        t_targetPos.position = new Vector3(UnityEngine.Random.Range(-11.5f, 11.5f), 0, UnityEngine.Random.Range(-8.0f, 5.0f));
        n_agent.SetDestination(t_targetPos.position);
    }

    // Function to move the character and determine if must attack
    public void MoveEnemy()
    {
        if (hpBar.SetHPValue < 0)
            gameObject.SetActive(false);
        if (Vector3.Distance(transform.position, t_targetPos.position) < 2 || (Vector3.Distance(transform.position, t_targetPos.position) > 8 && t_targetPos.tag == "Player"))
        {
            a_anim.SetFloat("Speed", 0);
            GetNewPos();
        }
        else
        {
            a_anim.SetFloat("Speed", (t_targetPos.tag == "Player" && Vector3.Distance(transform.position, t_targetPos.position) <= 5) ? 0 : 1);
        }
        if (t_targetPos.tag == "Player" && t_targetPos.gameObject.activeSelf)
        {
            n_agent.stoppingDistance = 5;
            if (!b_firing)
            {
                b_firing = true;
                if (co_fire != null)
                    StopCoroutine(co_fire);
                StartCoroutine(ShootTime());
                a_anim.SetBool("Shoot", true);
                p_system.Play();
            }
        }
        else
        {
            n_agent.stoppingDistance = 2;
            if (co_fire != null)
                StopCoroutine(co_fire);
            b_firing = false;
            a_anim.SetBool("Shoot", false);
            p_system.Stop();
        }
        a_anim.transform.LookAt(t_targetPos);
        t_HPBar.eulerAngles = new Vector3(60, -transform.rotation.y, 0);
    }

    // Shooting function with firing speed
    private IEnumerator ShootTime()
    {
        while (b_firing)
        {
            m_manager.ShootTo(t_FireOrigin, transform);
            yield return new WaitForSeconds(1 / f_aspd);
        }
    }

    // Check if the character is taking damage or colliding with some objec
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            hpBar.ChangeHPValue = 20.0f;
        }
        if (hpBar.SetHPValue <= 0)
        {
            n_agent.ResetPath();
            n_agent.isStopped = true;
            if (other.GetComponent<Bullet>() != null)
                if (other.GetComponent<Bullet>().t_owner == m_manager.player.transform)
                    m_manager.AddScore();
            gameObject.SetActive(false);
        }
    }
}
