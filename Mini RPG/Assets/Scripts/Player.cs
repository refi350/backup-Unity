using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public int curHP;
    public int maxHP;  
    public int damage;
    public float interactRange;
    public List<string> inventory = new List<string>();

    private Vector2 facingDirection;

    [Header("Combat")]
    public KeyCode attackKey;
    public float attackRange;
    public float attackRate;
    public float knockback;
    private float lastAttackTime;

    [Header("Experience")]
    public int curLevel;
    public int curXP;
    public int xpToNextLevel;
    public float levelXPModifier;

    [Header("Sprites")]
    public SpriteRenderer spriteSource;
    [Space]
    public Sprite downSprite;
    public Sprite upSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    //components
    private Rigidbody2D rig;
    private ParticleSystem hitEffect;
    private UIController ui;

    private void Awake()
    {
        //get the components
        rig = GetComponent<Rigidbody2D>();
        hitEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        ui = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        ui.UpdateHealthBar();
        ui.UpdateLevelText();
        ui.UpdateXPBar();
    }

    private void Update()
    {
        Move();

        if (Input.GetKeyDown(attackKey))
        {
            if (Time.time - lastAttackTime >= attackRate)
                Attack();
        }

        CheckInteract();
    }

    private void Move()
    {
        //get the horizontal and vertical keyboard inputs
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 vel = new Vector2(x, y);

        if(vel.magnitude != 0)
        {
            facingDirection = vel;
        }

        UpdateSpriteDirection();

        rig.velocity = vel * moveSpeed;
    }

    void UpdateSpriteDirection()
    {
        if(facingDirection == Vector2.up)
        {
            spriteSource.sprite = upSprite;
        }
        else if(facingDirection == Vector2.down)
        {
            spriteSource.sprite = downSprite;
        }
        else if(facingDirection == Vector2.left)
        {
            spriteSource.sprite = leftSprite;
        }
        else if(facingDirection == Vector2.right)
        {
            spriteSource.sprite = rightSprite;
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, attackRange, 1 << 8);

        if (hit.collider != null)
        {
            hit.collider.GetComponent<Enemy>().TakeDamage(damage);

            //hit.collider.GetComponent<Rigidbody2D>().AddForce(facingDirection * knockback,ForceMode2D.Impulse);
            //Debug.Log(hit.collider.GetComponent<Rigidbody2D>());

            hitEffect.transform.position = hit.collider.transform.position;
            hitEffect.Play();
        }
    }

    public void TakeDamage(int damageTaken)
    {
        curHP -= damageTaken;

        ui.UpdateHealthBar();

        if (curHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void AddXP(int xp)
    {
        curXP += xp;

        ui.UpdateXPBar();

        if(curXP >= xpToNextLevel)
        {
            LevelUP();
        }
    }

    void LevelUP()
    {
        curXP -= xpToNextLevel;
        curLevel++;

        xpToNextLevel = Mathf.RoundToInt((float)xpToNextLevel * levelXPModifier);

        ui.UpdateLevelText();
        ui.UpdateXPBar();

        if (curXP >= xpToNextLevel)
        {
            LevelUP();
        }
    }

    void CheckInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, interactRange, 1 << 9);
        

        if(hit.collider != null)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            ui.SetInteractText(hit.collider.transform.position, interactable.interactDescription);

            if (Input.GetKeyDown(attackKey))
            {
                interactable.Interact();
            }
        }
        else
        {
            ui.DisableInteractText();
        }
    }

    public void AddItemToInventory(string item)
    {
        inventory.Add(item);
        ui.UpdateInventoryText();
    }
}
