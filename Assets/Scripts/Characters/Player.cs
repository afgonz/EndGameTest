using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private G_Manager manager;
    [SerializeField]
    private NavMeshAgent controller;
    [SerializeField]
    private Animator a_anima;
    [SerializeField]
    private float f_pSpeed = 100.0f, f_aspd = 1.5f;
    [SerializeField]
    private Transform t_bulletOrigin;
    [SerializeField]
    private ParticleSystem p_system;
    public HPBar hpBar;
    private bool b_cooldown = false, b_firing = false;
    public bool b_gotKey = false;
    private Coroutine co_fire;

    // Initialize all variables
    public void StartPlayer()
    {
        b_gotKey = false;
        p_system.Stop();
        try
        {
            manager.uiManager.a_joystick.OnPush -= Shoot;
            manager.uiManager.a_joystick.OnRelease -= Release;
        }
        catch { }
        manager.uiManager.a_joystick.OnPush += Shoot;
        manager.uiManager.a_joystick.OnRelease += Release;
        hpBar.SetHPValue = 1000.0f;
        transform.position = Vector3.zero;
    }

    // Start the shooring function
    private void Shoot()
    {
        p_system.Play();
        b_firing = true;
        co_fire = StartCoroutine(ShootTime());
    }

    // Stops the shooting function
    private void Release()
    {
        p_system.Stop();
        b_firing = false;
        StopCoroutine(co_fire);
    }

    // Determines de shooting ratio
    private IEnumerator ShootTime()
    {
        while (b_firing)
        {
            manager.ShootTo(t_bulletOrigin, transform);
            yield return new WaitForSeconds(1 / f_aspd);
        }
    }

    // Move the players and blend the animation depending on the joystick position
    public void MovePlayer(Vector2 move, Vector2 dir)
    {
        controller.Move(new Vector3(move.x, 0, move.y) * Time.deltaTime * f_pSpeed);
        a_anima.SetFloat("Speed", move.magnitude);
        var rad = dir != Vector2.zero ? Mathf.Atan2(dir.y, dir.x) : Mathf.Atan2(move.y, move.x);
        var deg = 90 - (rad * (180 / Mathf.PI));
        if (deg != 90)
            a_anima.transform.rotation = Quaternion.Euler(new Vector3(0, deg, 0));
        a_anima.SetBool("Shoot", dir.magnitude > 0.0f ? true : false);
    }

    // Determines if the player takes damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
            hpBar.ChangeHPValue = 20.0f;
        if (other.tag == "Key")
        {
            b_gotKey = true;
            manager.GotItem();
            other.gameObject.SetActive(false);
        }
    }
}
