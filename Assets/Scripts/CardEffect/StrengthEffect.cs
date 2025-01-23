using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : Effect
{

    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetUpStrength(value, true);
                break;
            case EffectTargetType.Target:
                target.SetUpStrength(value, false);
                break;
            case EffectTargetType.All:
                break;
        }
    }
}