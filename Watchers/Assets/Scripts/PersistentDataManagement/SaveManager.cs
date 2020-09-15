using System;
using UnityEngine;
using Assets.Scripts.Attributes;
using Assets.Scripts.BattleSystem;

namespace Assets.Scripts.PersistentDataManagement
{
    public class SaveManager : MonoBehaviour
    {
        public event EventHandler OnGameSaved;

        [SerializeField] private BattleSystemManager _battleSystem;
        [SerializeField] private GameObject _saveablePlayerGameObject;
        private ISaveablePlayer _saveablePlayer;

        private void Awake()
        {
            if (_saveablePlayerGameObject != null)
            {
                _saveablePlayer = _saveablePlayerGameObject.GetComponent<ISaveablePlayer>();
            }

            _battleSystem.OnWaveEnded += BattleSystem_OnWaveEnded;
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("hasSaved"))
            {
                LoadPlayerData();
            }
        }

        public void SavePlayerData()
        {
            if (_saveablePlayer == null)
            {
                return;
            }

            PlayerPrefs.SetInt("hasSaved", 1);

            SaveAttributes();

            int playerSouls = _saveablePlayer.GetSouls();
            PlayerPrefs.SetInt("playerSouls", playerSouls);
            PlayerPrefs.Save();

            OnGameSaved?.Invoke(this, EventArgs.Empty);
        }

        private void LoadPlayerData()
        {
            LoadAttributes();

            int playerSouls = PlayerPrefs.GetInt("playerSouls");
            _saveablePlayer.SetSouls(playerSouls);
        }

        private void LoadAttributes()
        {
            foreach (AttributeType attributeType in Enum.GetValues(typeof(AttributeType)))
            {
                if (!PlayerPrefs.HasKey($"player{attributeType}"))
                    continue;

                int currentAttributeValue = PlayerPrefs.GetInt($"player{attributeType}");
                _saveablePlayer.SetAttribute(attributeType, currentAttributeValue);
            }
        }

        private void SaveAttributes()
        {
            foreach (AttributeType attributeType in Enum.GetValues(typeof(AttributeType)))
            {
                int currentAttributeValue = _saveablePlayer.GetAttribute(attributeType);
                PlayerPrefs.SetInt($"player{attributeType}", currentAttributeValue);
            }
        }

        private void BattleSystem_OnWaveEnded(object sender, EventArgs e)
        {
            SavePlayerData();
        }
    }
}
