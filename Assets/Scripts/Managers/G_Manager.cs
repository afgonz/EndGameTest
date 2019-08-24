using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class G_Manager : MonoBehaviour
{
    public Player player;
    public UI_Manager uiManager;
    public P_Manager poolManager;
    public bool b_gameActive = false;
    [HideInInspector]
    public List<Transform> l_players;
    [SerializeField]
    private float f_EnemiesFOV = 8.0f;
    [SerializeField]
    private GameObject go_menu, go_key, go_Environment;
    [SerializeField]
    private AudioSource a_source;
    [SerializeField]
    private AudioClip[] a_clip;
    private Transform t_NewTarget;
    //[HideInInspector]
    public int score = 0;

    // Initialize variables and objects properties
    public void StartGame()
    {
        score = 0;
        uiManager.SetScore(score);
        go_key.SetActive(true);
        uiManager.go_key.SetActive(false);
        b_gameActive = true;
        player.StartPlayer();
        for(int e = 0; e < l_players.Count; e++)
        {
            if (l_players[e] != player)
                try { l_players[e].GetComponent<Enemy>().StartEnemy(); } catch { }
        }
        foreach (Transform obj in go_Environment.transform)
        {
            if(obj.GetComponent<Sc_TEnDis>() != null)
                obj.GetComponent<Sc_TEnDis>().OnStart();
        }
    }

    // Only Update for the game, checks the game status, important flags and behabiour
    private void Update()
    {
        if (b_gameActive)
        {
            player.MovePlayer(uiManager.m_joystick.v_jsVect, uiManager.a_joystick.v_jsVect);
            CheckTargets();
            if (player.hpBar.SetHPValue <= 0)
            {
                b_gameActive = false;
                go_menu.SetActive(true);
            }
        }
    }

    // Check all players position relative to other players, determine if must attack
    private void CheckTargets()
    {
        for (int p = 0; p < l_players.Count; p++)
        {
            if (l_players[p] != player.transform && l_players[p].gameObject.activeSelf)
            {
                for (int e = 0; e < l_players.Count; e++)
                {
                    if (l_players[p] != l_players[e] && l_players[e].gameObject.activeSelf)
                    {
                        if (Vector3.Distance(l_players[p].position, l_players[e].position) < f_EnemiesFOV && Vector3.Distance(l_players[p].position, l_players[e].position) >= 2)
                        {
                            l_players[p].GetComponent<Enemy>().SetTarget = l_players[e];
                        }
                        else
                        {
                            if (Vector3.Distance(l_players[p].position, l_players[p].GetComponent<Enemy>().SetTarget.position) < 2)
                            {
                                l_players[p].GetComponent<Enemy>().GetNewPos();
                            }
                        }
                    }
                }
                l_players[p].GetComponent<Enemy>().MoveEnemy();
            }
        }
    }

    // Item collection function
    public void GotItem()
    {
        a_source.PlayOneShot(a_clip[0]);
        uiManager.go_key.SetActive(true);
    }

    // Set the player score;
    public void AddScore()
    {
        score += 1;
        uiManager.SetScore(score);
    }

    // Bridge to the Shoot function and Pool Manager
    public void ShootTo(Transform origin, Transform ownwer)
    {
        if(b_gameActive)
            poolManager.Shoot(origin, ownwer);
    }

    // Exit game function
    public void QuitGame()
    {
        Application.Quit();
    }
}
