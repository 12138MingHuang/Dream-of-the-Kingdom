using System;

public class Player : CharacterBase
{
    public IntVariable playMana;
    public int maxMana;

    public int CurrentMana
    {
        get => playMana.currentValue;
        set => playMana.SetValue(value);
    }

    private void OnEnable()
    {
        playMana.maxValue = maxMana;
        CurrentMana = playMana.maxValue;
    }

    /// <summary>
    /// 监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        
        if(CurrentMana <= 0) CurrentMana = 0;
    }

    public void NewGame()
    {
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;
        NewTurn();
    }
}