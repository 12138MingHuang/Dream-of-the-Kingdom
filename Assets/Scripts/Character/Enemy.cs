using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSo;
    public EnemyAction currentAction;
    
    protected Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSo.actions.Count);
        currentAction = actionDataSo.actions[randomIndex];
    }
    
    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {

            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
        }
    }

    protected virtual void Skill()
    {
        currentAction.effect.Execute(this,this);
    }

    protected virtual void Attack()
    {
        currentAction.effect.Execute(this,player);
    }
}