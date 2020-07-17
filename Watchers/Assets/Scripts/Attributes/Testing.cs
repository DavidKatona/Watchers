using Assets.Scripts.Attributes;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private CharacterMenu _characterMenu;

    void Start()
    {
        Attributes attributes = new Attributes(99, 12, 31, 41, 22, 58, 69);

        _characterMenu.SetAttributes(attributes);
    }
}
