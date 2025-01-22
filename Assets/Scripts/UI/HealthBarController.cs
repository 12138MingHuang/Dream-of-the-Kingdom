using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;
    
    [Header("Elements")]
    public Transform healthBarTrans;
    private UIDocument healthBarDoc;
    private ProgressBar healthBar;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
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
        
        healthBar.highValue = currentCharacter.MaxHP;
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
        }
    }
}