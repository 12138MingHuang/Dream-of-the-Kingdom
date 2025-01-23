using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
        
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType != EffectTargetType.Self)
        {
            Debug.LogWarning("DrawCardEffect is only valid on Self");
            return;
        }
        
        drawCardEvent?.RaiseEvent(value, this);
    }
}