using UnityEngine;

public class CharacterMenu : MonoBehaviour
{
    private bool _isOpened;
    public GameObject CharacterMenuUI;
    public AudioSource audioSource;
    public AudioClip menuOpenClip;
    public AudioClip menuCloseClip;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("OpenCharacterMenu"))
        {
            _isOpened = !_isOpened;

            CharacterMenuUI.SetActive(_isOpened);

            if (_isOpened)
            {
                audioSource.clip = menuOpenClip;
            }
            else
            {
                audioSource.clip = menuCloseClip;
            }

            audioSource.Play();
        }

        //if (_isOpened)
        //{
        //    CharacterMenuUI.SetActive(true);
        //}
        //else
        //{
        //    CharacterMenuUI.SetActive(false);
        //}
    }
}
