using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;
    
    [Header("Elements")]
    public Transform healthBarTrans;
    private UIDocument healthBarDoc;
    private ProgressBar healthBar;
    private VisualElement defenseElement;
    private Label defenseAmountLable;
    private VisualElement buffElement;
    private Label buffRound;

    public Sprite buffSprite;
    public Sprite deBuffSprite;

    private Enemy enemy;
    private VisualElement intentElement;
    private Label intentAmount;
    
    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPos, Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPos, size, Camera.main);
        element.transform.position = rect.position;
    }
    
    [ContextMenu("InitHealthBar")]
    public void InitHealthBar()
    {
        healthBarDoc = GetComponent<UIDocument>();
        healthBar = healthBarDoc.rootVisualElement.Q<ProgressBar>();
        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmountLable = healthBar.Q<Label>("DefenseAmount");
        buffElement = healthBar.Q<VisualElement>("Buff");
        buffRound = healthBar.Q<Label>("BuffRound");
        intentElement = healthBar.Q<VisualElement>("Intent");
        intentAmount = intentElement.Q<Label>("IntentAmount");
        
        healthBar.highValue = currentCharacter.MaxHP;
        defenseElement.style.display = DisplayStyle.None;
        buffElement.style.display = DisplayStyle.None;
        MoveToWorldPosition(healthBar, healthBarTrans.position, Vector2.zero);
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
            return;
        }

        if (healthBar != null)
        {
            healthBar.style.display = DisplayStyle.Flex;
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";

            healthBar.value = currentCharacter.CurrentHP;
            
            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");
            
            var percent = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;
            
            if(percent < 0.3f)
            {
                
                healthBar.AddToClassList("lowHealth");
            }
            else if (percent < 0.6f)
            {
                healthBar.AddToClassList("mediumHealth");
            }
            else
            {
                healthBar.AddToClassList("highHealth");
            }
            
            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmountLable.text = currentCharacter.defense.currentValue.ToString();
            
            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            buffRound.text = currentCharacter.buffRound.currentValue.ToString();
            buffElement.style.backgroundImage = new StyleBackground(currentCharacter.baseStrength > 1 ? buffSprite : deBuffSprite);
        }
    }

    public void SetIntentElement()
    {
        intentElement.style.display = DisplayStyle.Flex;
        intentElement.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);

        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(DamageEffect))
        {
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }
        
        intentAmount.text = value.ToString();
    }
    
    public void HideIntentElement()
    {
        intentElement.style.display = DisplayStyle.None;
    }
}