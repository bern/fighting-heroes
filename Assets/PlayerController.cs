﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BoxCollider2D attackHitbox;
    public PlayerController enemy;
    public BoxCollider2D enemyHurtbox;
    public GameObject floor;
    public HealthBar healthBar;
    public LayerMask groundLayer;
    private Animator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer renderer;
    public SpriteRenderer head;

    public float characterHp = 100f;
    public bool isPlayer2;
    public float jumpForce = 550f;
    public float walkSpeed = 7f;
    private float distanceToFloor;
    private bool isHitting;
    private float moveInput;

    private float comboTimeRemaining;
    private float COMBO_DELAY_SECONDS = 0.5f;
    private int comboStep = 0;
    private float ATTACK_DELAY_TIMER = 0.5f;
    private float attackDelayRemaining;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        distanceToFloor = GetComponent<Collider2D>().bounds.extents.y;
        if (isPlayer2)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        healthBar.setMaxHp(characterHp);
        healthBar.resetHp();
    }

    private void Update()
    {
        if (IsDead())
        {
            animator.SetBool("Dead", true);
            return;
        }
        if ((isPlayer2 ? Input.GetKeyDown(KeyCode.I) : Input.GetKeyDown(KeyCode.W)) && IsGrounded())
        {
            rb2d.AddForce(Vector3.up * jumpForce);
        }
        moveInput = isPlayer2 ? Input.GetAxis("p2_Horizontal") : Input.GetAxis("Horizontal");
        if (moveInput >= 0.5 || moveInput <= -0.5)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        if(isPlayer2 ? Input.GetKeyDown(KeyCode.P) : Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("attackDelayRemaining: " + attackDelayRemaining);
            if (comboStep == 0 && attackDelayRemaining == 0)
            {
                attackDelayRemaining = ATTACK_DELAY_TIMER;
                comboTimeRemaining = COMBO_DELAY_SECONDS;
                animator.SetTrigger("LightAttack");
                //animator.ResetTrigger("LightAttack2");
                comboStep++;
            } else if (comboStep == 1 && comboTimeRemaining > 0)
            {
                Debug.Log("combo attack2");
                attackDelayRemaining = ATTACK_DELAY_TIMER;
                //animator.ResetTrigger("LightAttack");
                animator.SetTrigger("LightAttack2");
                comboStep = 0;
            }
        }
        if (IsGrounded())
        {
            //Debug.Log("Not Jumping");
            animator.SetBool("Jumping", false);
        } else
        {
            animator.SetBool("Jumping", true);
        }
        if (attackHitbox.IsTouching(enemyHurtbox) && !isHitting)
        {
            Debug.Log(this.name + " scored a hit!");
            enemy.handleDamage(5);
            isHitting = true;
        } else if (!attackHitbox.IsTouching(enemyHurtbox))
        {
            isHitting = false;
        }
        if (attackDelayRemaining > 0)
        {
            attackDelayRemaining -= Time.deltaTime;
            attackDelayRemaining = attackDelayRemaining < 0 ? 0 : attackDelayRemaining;
            if (attackDelayRemaining == 0)
            {
                attackDelayRemaining = 0;
            }
        }
        if (comboTimeRemaining > 0)
        {
            comboTimeRemaining -= Time.deltaTime;
            comboTimeRemaining = comboTimeRemaining < 0 ? 0 : comboTimeRemaining;
            if (comboTimeRemaining == 0)
            {
                comboStep = 0;
                Debug.Log("Out of combo time!");
            }
        }
    }

    public void UpdateSprite(Sprite sprite)
    {
        Debug.Log("here we go!");
        this.head.sprite = sprite;
        Debug.Log("after: " + this.head.sprite.bounds);
        Debug.Log(this.head.sprite.name);
        this.head.transform.localScale = new Vector2(0.05f, 0.05f);
    }

    // Physics should ALWAYS go in FixedUpdate
    void FixedUpdate()
    {
        if (IsDead())
        {
            return;
        }
        moveInput = isPlayer2 ? Input.GetAxis("p2_Horizontal") : Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(moveInput * walkSpeed, rb2d.velocity.y);
        if (rb2d.velocity.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rb2d.velocity.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public bool IsDead()
    {
        return healthBar.getHp() <= 0;
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.9f;

        Debug.DrawRay(position, direction * distance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    public void handleDamage(int dmg)
    {
        Debug.Log(this.name + " took " + dmg + " damage!");
        animator.SetTrigger("Hit");
        healthBar.changeHealth(-1 * dmg);
    }
}
