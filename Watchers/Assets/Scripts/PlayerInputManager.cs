using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    public float HorizontalInputModifier { get; private set; }
    public float VerticalInputModifier { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsDashPressed { get; private set; }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        //Movement on the X axis
        HorizontalInputModifier = Input.GetAxisRaw("Horizontal");
        VerticalInputModifier = Input.GetAxisRaw("Vertical");

        //Jumping
        if (Input.GetButtonDown("Jump"))
        {
            IsJumpPressed = true;
        }
        else if (!Input.GetButtonDown("Jump"))
        {
            IsJumpPressed = false;
        }

        //Dashing
        if (Input.GetButtonDown("Dash"))
        {
            IsDashPressed = true;
        }
        else if (!Input.GetButtonDown("Dash"))
        {
            IsDashPressed = false;
        }
    }
}
