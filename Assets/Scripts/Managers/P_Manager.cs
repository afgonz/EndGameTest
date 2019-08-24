using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class P_Manager : MonoBehaviour
    {
        [SerializeField]
        private G_Manager manager;
        [SerializeField]
        private GameObject pf_Bullet, pf_Enemy;
        [SerializeField]
        private Transform t_BulletPool, t_EnemyPool;
        private List<GameObject> l_bullets;
        [SerializeField]
        private AudioSource au_source;
        [SerializeField]
        private AudioClip[] au_clip;

        private void Start()
        {
            l_bullets = new List<GameObject> { };
        }

        public void NewEnemy()
        {
            GameObject newEnemy = null;
            for (int e = 0; e < manager.l_players.Count; e++)
            {
                if (!manager.l_players[e].gameObject.activeSelf)
                {
                    newEnemy = manager.l_players[e].gameObject;                    
                    break;
                }
            }
            if (newEnemy == null)
            {
                newEnemy = Instantiate(pf_Enemy, t_EnemyPool);
                newEnemy.GetComponent<Enemy>().SetManager = manager;
                newEnemy.transform.position = new Vector3(UnityEngine.Random.Range(-11.5f, 11.5f), 0, UnityEngine.Random.Range(-8.0f, 5.0f));
                manager.l_players.Add(newEnemy.transform);
            }
            newEnemy.SetActive(true);
            newEnemy.GetComponent<Enemy>().StartEnemy();
        }

        private GameObject NewBullet()
        {
            GameObject Bullet = null;
            for (int b = 0; b< l_bullets.Count; b++)
            {
                if (!l_bullets[b].activeSelf)
                    Bullet = l_bullets[b];
            }
            if (Bullet == null)
            {
                Bullet = Instantiate(pf_Bullet, t_BulletPool);
                l_bullets.Add(Bullet);
            }
            return Bullet;
        }

        public void Shoot(Transform origin)
        {
            GameObject Bullet = NewBullet();
            au_source.pitch = UnityEngine.Random.Range(1.5f, 2.0f);
            au_source.PlayOneShot(au_clip[0]);
            Bullet.transform.position = origin.position;
            Bullet.transform.rotation = origin.rotation;
            Bullet.SetActive(true);
        }

    }
}