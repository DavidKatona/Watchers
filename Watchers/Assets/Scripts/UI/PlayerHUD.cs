using Assets.Scripts.Attributes;
using System;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private StatManager _statManager;
    // This class should not communicate directly with the combat controller. Related events should be fired from the StatManager and this class should listen to them.
    [SerializeField] private PlayerCombatController _playerCombatController;
    [SerializeField] private Transform _healthPool;
    [SerializeField] private Transform _manaPool;
    private Attributes _attributes;

    public void SetAttributes(Attributes attributes)
    {
        _attributes = attributes;
        _attributes.OnAttributeChanged += Attributes_OnAttributeChanged;
        _playerCombatController.OnDamaged += Player_OnDamaged;
        UpdateStatistics();
    }

    private void Player_OnDamaged(object sender, EventArgs e)
    {
        UpdateStatistics();
    }

    private void Attributes_OnAttributeChanged(object sender, EventArgs e)
    {
        UpdateStatistics();
    }

    private void UpdateStatistics()
    {
        _healthPool.localScale = new Vector2(_statManager.GetHealthPercentage(), _healthPool.localScale.y);
        _manaPool.localScale = new Vector2(_statManager.GetManaPercentage(), _manaPool.localScale.y);
    }
}
