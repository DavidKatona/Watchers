using Assets.Scripts.Attributes;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private StatManager _statManager;
    [SerializeField] private Transform _healthPool;
    [SerializeField] private Transform _manaPool;
    [SerializeField] private Transform _soulsCounter;
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributeChanged;
        _attributes.OnSoulsChanged += Attributes_OnSoulsChanged;
        _statManager.OnHealthChanged += StatManager_OnHealthChanged;
        _statManager.OnManaChanged += StatManager_OnManaChanged;

        // Update all statistics & counters upon initialization.
        UpdateStatistics();
    }

    private void Attributes_OnAttributeChanged(object sender, EventArgs e)
    {
        UpdateStatistics();
    }

    private void Attributes_OnSoulsChanged(object sender, EventArgs e)
    {
        UpdateSouls();
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
        _soulsCounter.GetComponent<Text>().text = _attributes.GetSouls().ToString("#,0");
    }

    private void UpdateHealthBar()
    {
        _healthPool.localScale = new Vector2(_statManager.GetHealthPercentage(), _healthPool.localScale.y);
    }

    private void UpdateManaBar()
    {
        _manaPool.localScale = new Vector2(_statManager.GetManaPercentage(), _manaPool.localScale.y);
    }

    private void UpdateSouls()
    {
        _soulsCounter.GetComponent<Text>().text = _attributes.GetSouls().ToString("#,0");
    }
}