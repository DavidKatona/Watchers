using Assets.Scripts.BattleSystem;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemHUD : MonoBehaviour
{
    [SerializeField] private BattleSystemManager _battleSystemManager;
    [SerializeField] private GameObject _activeStateElements;
    [SerializeField] private GameObject _idleStateElements;
    [SerializeField] private RectTransform _waveHealthPool;
    private float _healthPoolUnit;

    void Awake()
    {
        _battleSystemManager.OnWaveStarted += BattleSystemManager_OnWaveStarted;
        _battleSystemManager.OnWaveEnded += BattleSystemManager_OnWaveEnded;
        _battleSystemManager.OnEnemyNumbersReduced += BattleSystemManager_OnEnemyNumbersReduced;
    }

    private void BattleSystemManager_OnWaveStarted(object sender, EventArgs e)
    {
        _activeStateElements.SetActive(true);
        _idleStateElements.SetActive(false);

        _healthPoolUnit = _waveHealthPool.localScale.x / _battleSystemManager.NumberOfEnemiesRemaining;
    }

    private void BattleSystemManager_OnWaveEnded(object sender, EventArgs e)
    {
        transform.Find("IdleStateElements/WaveStartText").GetComponent<Text>().text = $"Press \"Interact\" to launch Wave {_battleSystemManager.WaveNumber}.";

        _activeStateElements.SetActive(false);
        _idleStateElements.SetActive(true);

        _waveHealthPool.localScale = new Vector2(1, _waveHealthPool.localScale.y);
    }

    private void BattleSystemManager_OnEnemyNumbersReduced(object sender, EventArgs e)
    {
        var xScale = _waveHealthPool.localScale.x - _healthPoolUnit;
        Debug.Log(xScale);
        _waveHealthPool.localScale = new Vector2(xScale, _waveHealthPool.localScale.y);
    }
}
