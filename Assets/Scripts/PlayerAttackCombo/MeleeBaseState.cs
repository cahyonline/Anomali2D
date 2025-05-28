using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    // How long this state should be active for before moving on
    public float duration;
    // Cached animator component
    protected Animator animator;
    // bool to check whether or not the next attack in the sequence should be played or not
    protected bool shouldCombo;
    // The attack index in the sequence of attacks
    protected int attackIndex;

    // The cached hit collider component of this attack
    protected Collider2D hitCollider;
    // Cached already struck objects of said attack to avoid overlapping attacks on same target
    private List<Collider2D> collidersDamaged;
    // The Hit Effect to Spawn on the afflicted Enemy
    private GameObject[] HitEffectPrefab;

    // Input buffer Timer
    private float AttackPressedTimer = 0;

    //
    private bool hasMissedAttack = false;


    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
        collidersDamaged = new List<Collider2D>();
        hitCollider = GetComponent<ComboCharacter>().hitbox;
        HitEffectPrefab = GetComponent<ComboCharacter>().Hiteffect;
    }

    public override void OnUpdate()
    {
        
        base.OnUpdate();
        AttackPressedTimer -= Time.deltaTime;

        if (animator.GetFloat("Weapon.Active") > 0f)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AttackPressedTimer = 2;
        }

        if (animator.GetFloat("AttackWindow.Open") > 0f && AttackPressedTimer > 0)
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected void Attack()
    {
        Collider2D[] collidersToDamage = new Collider2D[20];
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int colliderCount = Physics2D.OverlapCollider(hitCollider, filter, collidersToDamage);

        bool hitSomething = false;

        for (int i = 0; i < colliderCount; i++)
        {
            Collider2D collider = collidersToDamage[i];
            Vector2 hitPosition = collider.bounds.center;
            if (collider != null)
            {
                if (collider.CompareTag("Batu") || (collider.CompareTag("Tanah")))
                {
                    //if (hitSomething == false) ;
                }
                else if (collider.CompareTag("Enemy"))
                {
                    // Spawn efek hit dan simpan referensinya
                    GameObject hitEffect = UnityEngine.Object.Instantiate(HitEffectPrefab[attackIndex], hitPosition, Quaternion.identity);

                    // Hancurkan efek setelah 0.5 detik
                    Destroy(hitEffect, 0.5f); // <<=== INI YANG DITAMBAHKAN

                    // Tandai enemy yang sudah kena hit
                    collidersDamaged.Add(collider);
                    hitSomething = true;
                }
            }
        }
    }
}