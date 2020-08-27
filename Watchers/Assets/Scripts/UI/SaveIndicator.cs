using Assets.Scripts.PersistentDataManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SaveIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject _saveSystemObject;
        private SaveManager _saveManager;

        private void Awake()
        {
            _saveManager = _saveSystemObject.GetComponent<SaveManager>();
            _saveManager.OnGameSaved += SaveSystem_OnGameSaved;

            gameObject.SetActive(false);
        }

        private void SaveSystem_OnGameSaved(object sender, EventArgs e)
        {
            gameObject.SetActive(true);
            StartCoroutine(DisableAfterDuration(6f));
        }

        private IEnumerator DisableAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}
