using Assets.Scripts.Attributes;
using UnityEngine;

public class TestingAttributes : MonoBehaviour
{
    [SerializeField] private CharacterMenu _characterMenu;
    [SerializeField] private DebugMenu _debugMenu;

    void Start()
    {
        Attributes attributes = new Attributes(11, 11, 11, 11, 11, 11, 11);

        _characterMenu.SetAttributes(attributes);
        _debugMenu.SetAttributes(attributes);
    }
}
