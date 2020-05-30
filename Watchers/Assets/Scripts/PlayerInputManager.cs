using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerBrain _playerBrain;

    public float HorizontalInputModifier { get; private set; }
    public float VerticalInputModifier { get; private set; }
    public bool IsJumpPressed { get; private set; }
    public bool IsJumpHeld { get; private set; }
    public bool IsDashPressed { get; private set; }
    public bool IsAttackHeldDown { get; private set; }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        //Movement Modifiers
        HorizontalInputModifier = Input.GetAxisRaw("Horizontal");
        VerticalInputModifier = Input.GetAxisRaw("Vertical");

        //Jump Pressed
        if (Input.GetButtonDown("Jump"))
        {
            IsJumpPressed = true;
        }
        else
        {
            IsJumpPressed = false;
        }

        //Jump Being Held Down
        if (Input.GetButton("Jump"))
        {
            IsJumpHeld = true;
        }
        else
        {
            IsJumpHeld = false;
        }

        //Dashing
        if (Input.GetButtonDown("Dash"))
        {
            IsDashPressed = true;
        }
        else
        {
            IsDashPressed = false;
        }

        //Attacking
        if (Input.GetButtonDown("Attack"))
        {
            IsAttackHeldDown = true;
        }
        else
        {
            IsAttackHeldDown = false;
        }
    }
}
