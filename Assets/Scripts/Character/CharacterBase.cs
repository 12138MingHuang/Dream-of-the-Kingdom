using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public int CurrentHP
    {
        get => hp.currentValue;
        set => hp.SetValue(value);
    }
    public int MaxHP
    {
        get => hp.maxValue;
    }
    
    protected Animator animator;
    public bool isDead;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (CurrentHP >= damage)
        {
            CurrentHP -= damage;
        }
        else
        {
            CurrentHP = 0;
            // 人物死亡
            isDead = true;
        }
    }
}