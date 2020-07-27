using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [SerializeField] private string _tutorialMessage = "Sample Text";
    [SerializeField] private float _duration = 5.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // This could and should be placed inside a TutorialManager that preloads all tutorial zones on startup to increase performance.
        Transform parentTransform = GameObject.Find("CharacterMenu").transform;
        TutorialPopup.Create(_tutorialMessage, 5.0f, parentTransform);

        Destroy(gameObject);
    }
}
