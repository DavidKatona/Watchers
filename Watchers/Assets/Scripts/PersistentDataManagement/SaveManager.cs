﻿using System;
using UnityEngine;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.PersistentDataManagement
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private GameObject _saveablePlayerGameObject;
        private ISaveablePlayer _saveablePlayer;

        private void Awake()
        {
            _saveablePlayer = _saveablePlayerGameObject.GetComponent<ISaveablePlayer>();
            PlayerPrefs.DeleteAll();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SavePlayerData();
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadPlayerData();
            }
        }

        private void SavePlayerData()
        {
            SaveAttributes();

            float playerPositionX = _saveablePlayer.GetPositionX();
            float playerPositionY = _saveablePlayer.GetPositionY();
            PlayerPrefs.SetFloat("playerPositionX", playerPositionX);
            PlayerPrefs.SetFloat("playerPositionY", playerPositionY);

            int playerSouls = _saveablePlayer.GetSouls();
            PlayerPrefs.SetInt("playerSouls", playerSouls);

            float playerCurrentHealth = _saveablePlayer.GetHealth();
            float playerCurrentMana = _saveablePlayer.GetMana();
            PlayerPrefs.SetFloat("playerCurrentHealth", playerCurrentHealth);
            PlayerPrefs.SetFloat("playerCurrentMana", playerCurrentMana);


            PlayerPrefs.Save();
            Debug.Log("Game saved!");
        }

        private void LoadPlayerData()
        {
            LoadAttributes();

            if (PlayerPrefs.HasKey("playerPositionX") && PlayerPrefs.HasKey("playerPositionY"))
            {
                float playerPositionX = PlayerPrefs.GetFloat("playerPositionX");
                float playerPositionY = PlayerPrefs.GetFloat("playerPositionY");
                _saveablePlayer.SetPosition(new Vector2(playerPositionX, playerPositionY));
            }

            int playerSouls = PlayerPrefs.GetInt("playerSouls");
            _saveablePlayer.SetSouls(playerSouls);

            if (PlayerPrefs.HasKey("playerCurrentHealth") && PlayerPrefs.HasKey("playerCurrentMana"))
            {
                float playerCurrentHealth = PlayerPrefs.GetFloat("playerCurrentHealth");
                float playerCurrentMana = PlayerPrefs.GetFloat("playerCurrentMana");
                _saveablePlayer.SetHealth(playerCurrentHealth);
                _saveablePlayer.SetMana(playerCurrentMana);
            }

            Debug.Log("Game loaded!");
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
    }
}
