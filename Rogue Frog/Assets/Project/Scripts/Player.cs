using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpDistance = 0.32f;

    public delegate void PlayerHandler();
    public event PlayerHandler OnPlayerMoved;
    public event PlayerHandler OnPlayerEscaped;

    private bool jumped;
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        jumped = false;
        //Debug.Log(Screen.width);
        //Debug.Log(Screen.height);
        Debug.Log((Screen.width / 100f) / 2f);
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement Logic
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        //Debug.Log(horizontalMovement + " " + verticalMovement);
        //Debug.Log((Screen.height / 100f) / 2f);
        
        if (!jumped)
        {
            Vector2 targetPosition = Vector2.zero;
            //Vector2 fixedPosition = Vector2.zero;
            bool tryingToMove = false;

            if (horizontalMovement!= 0)
            {
                targetPosition = new Vector2(
                    transform.position.x + (horizontalMovement > 0 ? jumpDistance : -jumpDistance),
                    transform.position.y);
                tryingToMove = true;
            }
            else if(verticalMovement != 0)
            {
                targetPosition = new Vector2(
                    transform.position.x ,
                    transform.position.y + (verticalMovement > 0 ? jumpDistance : -jumpDistance));
                tryingToMove = true;
            }
            //if(targetPosition!= Vector2.zero)
            //{
            //     fixedPosition = new Vector2(transform.position.x, transform.position.y + fixMove);
            // }
            Collider2D hitCollider = Physics2D.OverlapCircle(targetPosition, 0.105f);
            if(tryingToMove == true && (hitCollider == null || hitCollider.GetComponent<Enemy>() != null))
            {
                transform.position = targetPosition;
                jumped = true;
                GetComponent<AudioSource>().Play();
                OnPlayerMoved?.Invoke();
            }
        }
        else
        {
            if(horizontalMovement == 0 && verticalMovement == 0)
            {
                jumped = false;
            }
        }
        //-(Screen.height / 100f) / 2f
        // lewy -2.88
        //prawy 3.2
        // góra 3.2
        //Keep the frog inside bound.
        if (transform.position.y < -(Screen.height / 100f)/2f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + jumpDistance);
        }
        if (transform.position.x <= -(Screen.width / 100f)/2f)
        {
            transform.position = new Vector2(transform.position.x + jumpDistance, transform.position.y);
        }
        if (transform.position.x > (Screen.width / 100f)/2f)
        {
            transform.position = new Vector3(transform.position.x - jumpDistance, transform.position.y);
        }
        if (transform.position.y > (Screen.height / 100f)/2f - 0.1f)
        {
            transform.position = startingPosition;
            OnPlayerEscaped?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Enemy> () != null)
        {
            Destroy(gameObject);
        }
    }
}
