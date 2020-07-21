using Assets.Scripts.Attributes;
using System;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private StatManager _statManager;
    [SerializeField] private Transform _healthPool;
    [SerializeField] private Transform _manaPool;
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributeChanged;
        _statManager.OnHealthChanged += StatManager_OnHealthChanged;
        _statManager.OnManaChanged += StatManager_OnManaChanged;
        UpdateStatistics();
    }

    private void Attributes_OnAttributeChanged(object sender, EventArgs e)
    {
        UpdateStatistics();
    }

    private void StatManager_OnHealthChanged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    private void StatManager_OnManaChanged(object sender, EventArgs e)
    {
        UpdateManaBar();
    }

    private void UpdateStatistics()
    {
        _healthPool.localScale = new Vector2(_statManager.GetHealthPercentage(), _healthPool.localScale.y);
        _manaPool.localScale = new Vector2(_statManager.GetManaPercentage(), _manaPool.localScale.y);
    }

    private void UpdateHealthBar()
    {
        _healthPool.localScale = new Vector2(_statManager.GetHealthPercentage(), _healthPool.localScale.y);
    }

    private void UpdateManaBar()
    {
        _manaPool.localScale = new Vector2(_statManager.GetManaPercentage(), _manaPool.localScale.y);
    }
}