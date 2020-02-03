using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void FuelHandler();
    public event FuelHandler OnFuel;

    public float horizontalSpeed = 1.5f;
    public float verticalSpeed = 1f;
    public float horizontalLimit = 2.8f;
    public float bulletSpeed = 2f;
    public Vector2 distanceFromPlane;
    public GameObject bulletPrefab;
    public GameObject firePrefab;

    private bool fired = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal")* horizontalSpeed,verticalSpeed );

        if(transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        }
        else if(transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        if (Input.GetAxis("Fire1") == 1f)
        {
            if (!fired)
            {
                fired = true;
                GameObject bulletInstance1 = Instantiate(bulletPrefab);
                GameObject bulletInstance2 = Instantiate(bulletPrefab);
                GameObject fireInstance1 = Instantiate(firePrefab);
                GameObject fireInstance2 = Instantiate(firePrefab);
                bulletInstance1.transform.SetParent(transform.parent);
                bulletInstance2.transform.SetParent(transform.parent);
                fireInstance1.transform.SetParent(transform.parent);
                fireInstance2.transform.SetParent(transform.parent);
                bulletInstance1.transform.position =  new Vector2(transform.position.x + distanceFromPlane.x,transform.position.y + distanceFromPlane.y);
                bulletInstance2.transform.position = new Vector2(transform.position.x - distanceFromPlane.x, transform.position.y + distanceFromPlane.y);
                fireInstance1.transform.position = new Vector2(transform.position.x + distanceFromPlane.x, transform.position.y + distanceFromPlane.y);
                fireInstance2.transform.position = new Vector2(transform.position.x - distanceFromPlane.x, transform.position.y + distanceFromPlane.y);
                bulletInstance1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                bulletInstance2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
                Destroy(bulletInstance1, 1f);
                Destroy(bulletInstance2, 1f);
                Destroy(fireInstance1, 0.03f);
                Destroy(fireInstance2, 0.03f);

            }
            /*else
            {                        
                Debug.Log("Przeładowanie!");
                fired = false;
            }*/
        }
        if(Input.GetAxis("Fire1")== 0f && fired)
        {
            fired = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        } else if(collision.tag == "Fuel")
        {
            Destroy(collision.gameObject);
            if (OnFuel != null)
            {
                OnFuel();
            }
        }
    }
}
