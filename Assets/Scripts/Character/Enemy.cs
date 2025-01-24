using System;
using System.Collections;
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
    }

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSo.actions.Count);
        currentAction = actionDataSo.actions[randomIndex];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();
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
        StartCoroutine(ProcessDelayAction("skill"));
    }

    protected virtual void Attack()
    {
        StartCoroutine(ProcessDelayAction("attack"));
    }

    private IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f
            && !animator.IsInTransition(0)
            && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        
        if(actionName == "attack")
            currentAction.effect.Execute(this,player);
        else if(actionName == "skill")
            currentAction.effect.Execute(this,this);
    }
}