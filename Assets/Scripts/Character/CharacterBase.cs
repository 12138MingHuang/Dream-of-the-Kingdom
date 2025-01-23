using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;
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

    public VFXController vfxController;

    // 强化效果
    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;
    
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;
        ResetDefense();
    }

    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage-defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);
        
        if (CurrentHP >= damage)
        {
            CurrentHP -= damage;
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHP = 0;
            // 人物死亡
            isDead = true;
        }
    }
    
    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }
    
    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        
        vfxController.BuffPlay();
    }
    
    public void SetUpStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = strengthEffect + baseStrength;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            vfxController.BuffPlay();
        }
        else
        {
            vfxController.DeBuffPlay();
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;
        buffRound.SetValue(baseStrength == 1 ? 0 : currentRound);
    }

    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if(buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
}