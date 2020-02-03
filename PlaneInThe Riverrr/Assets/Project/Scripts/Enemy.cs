using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void KillHandler();
    public event KillHandler OnKill;

    public float speed;
    public float bulletSpeed;
    public float shootingInterval = 6f;
    public Vector2 distanceFromPlane;
    public GameObject bulletPrefab;
    public GameObject firePrefab;

    private float shootingTimer;
    // Start is called before the first frame update
    void OnEnable()
    {
        shootingTimer = Random.Range(0f, shootingInterval);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
    }

    // Update is called once per frame
    void Update()
    {
        shootingTimer -= Time.deltaTime;
        if(shootingTimer <= 0f)
        {
            shootingTimer = shootingInterval;
            GameObject bulletInstance1 = Instantiate(bulletPrefab);
            GameObject fireInstance1 = Instantiate(firePrefab);
            bulletInstance1.transform.SetParent(transform.parent);
            fireInstance1.transform.SetParent(transform.parent);
            bulletInstance1.transform.position = new Vector2(transform.position.x + distanceFromPlane.x, transform.position.y + distanceFromPlane.y);
            fireInstance1.transform.position = new Vector2(transform.position.x + distanceFromPlane.x, transform.position.y + distanceFromPlane.y);
            bulletInstance1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            Destroy(bulletInstance1, 1f);
            Destroy(fireInstance1, 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerBullet")
        {
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
            if (OnKill != null)
            {
                OnKill();
            }
        }
    }
}
