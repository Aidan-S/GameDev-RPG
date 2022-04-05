using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    //player animation
    private Animator anim;
    //red fire
    public GameObject redFire;
    private Animator redFireAnim;
    //blue fire
    public GameObject blueFire;
    private Animator blueFireAnim;
    //purple fire
    public GameObject purpleFire;
    private Animator purpleFireAnim;

    //input system
    private PlayerActionControls playerControls;
    private InputAction move;
    private InputAction a_Button;
    private InputAction b_Button;
    private InputAction x_Button;
    private InputAction y_Button;

    //player speed
    public int speed;

    //player combo
    public enum comboStatus { noCombo, hold, combo };

    public float comboTimer;
    public string comboValue;

    public float bStartTime;
    public float xStartTime;
    public float yStartTime;

    public comboStatus mode;

    private void Awake()
    {
        playerControls = new PlayerActionControls();
    }

    private void OnEnable()
    {
        move = playerControls.Basic.Move;
        move.Enable();

        a_Button = playerControls.Basic.aButton;
        a_Button.Enable();
        a_Button.performed += aButton;


        b_Button = playerControls.Basic.bButton;
        b_Button.Enable();
        b_Button.performed += bButtonDown;
        b_Button.canceled += bButtonUp;


        x_Button = playerControls.Basic.xButton;
        x_Button.Enable();
        x_Button.performed += xButtonDown;
        x_Button.canceled += xButtonUp;


        y_Button = playerControls.Basic.yButton;
        y_Button.Enable();
        y_Button.performed += yButtonDown;
        y_Button.canceled += yButtonUp;

    }

    private void OnDisable()
    {
        move.Disable();
        a_Button.Disable();
        b_Button.Disable();
        x_Button.Disable();
        y_Button.Disable();
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        redFireAnim = redFire.GetComponent<Animator>();
        purpleFireAnim = purpleFire.GetComponent<Animator>();
        blueFireAnim = blueFire.GetComponent<Animator>();
        comboTimer = -1;
        comboValue = "";
        mode = comboStatus.noCombo;
    }

    private void aButton(InputAction.CallbackContext context)
    {
        redFireAnim.SetBool("isLit", !redFireAnim.GetBool("isLit"));
        blueFireAnim.SetBool("isLit", !blueFireAnim.GetBool("isLit"));
        purpleFireAnim.SetBool("isLit", !purpleFireAnim.GetBool("isLit"));
    }

    private void bButtonUp(InputAction.CallbackContext context)
    {
        if(mode == comboStatus.hold && Time.time - bStartTime < .4)
        {
            mode = comboStatus.combo;
        }
        else if (mode == comboStatus.hold && Time.time - bStartTime > .4)
        {
            mode = comboStatus.noCombo;
        }

        bStartTime = 0;
    }

    private void xButtonUp(InputAction.CallbackContext context)
    {
        if (mode == comboStatus.hold && Time.time - xStartTime < .4)
        {
            mode = comboStatus.combo;
        }
        else if(mode == comboStatus.hold && Time.time - xStartTime > .4)
        {
            mode = comboStatus.noCombo;
        }

        xStartTime = 0;
    }

    private void yButtonUp(InputAction.CallbackContext context)
    {
        if (mode == comboStatus.hold && Time.time - yStartTime < .4)
        {
            mode = comboStatus.combo;
        }
        else if (mode == comboStatus.hold && Time.time - yStartTime > .4)
        {
            mode = comboStatus.noCombo;
        }

        yStartTime = 0;
    }

    private void bButtonDown(InputAction.CallbackContext context)
    {
        bStartTime = Time.time;
        if (comboValue == "")
        {
            mode = comboStatus.hold;
        }
        combo("b");
    }

    private void xButtonDown(InputAction.CallbackContext context)
    {
        xStartTime = Time.time;
        if (comboValue == "")
        {
            mode = comboStatus.hold;
        }
        combo("x");
    }

    private void yButtonDown(InputAction.CallbackContext context)
    {
        yStartTime = Time.time;
        if (comboValue == "")
        {
            mode = comboStatus.hold;
        }
        combo("y");
    }

    private void combo(string v)
    {
        //new combo
        if (comboTimer == -1)
        {
            comboTimer = Time.time;
            comboValue = v;
        }
        //continue combo
        else
        {
            //good combo
            if (comboValue.IndexOf(v) == -1)
            {
                comboValue += v;
                comboTimer = Time.time;
            }
            //failed combo
            else
            {
                resetCombo();
            }
        }

        if (comboValue.Length == 3)
        {
            Debug.Log("I did this combo: " + comboValue);
            resetCombo();
        }
    }

    private void resetCombo()
    {
        comboTimer = -1;
        comboValue = "";
        mode = comboStatus.noCombo;
    }

    private void Update()
    {
        if (comboTimer != -1 && Time.time - comboTimer > 0.7f && mode != comboStatus.hold)
        {
            resetCombo();
        }

        if (comboValue == "b" && bStartTime != 0 && Time.time - bStartTime >= 2f)
        {
            //strong attack
            Debug.Log("strong b");
            resetCombo();
        }

        if (comboValue == "x" && xStartTime != 0 && Time.time - xStartTime >= 2f)
        {
            //strong attack
            Debug.Log("strong x");
            resetCombo();
        }

        if (comboValue == "y" && yStartTime != 0 && Time.time - yStartTime >= 2f)
        {
            //strong attack
            Debug.Log("strong y");
            resetCombo();
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = move.ReadValue<Vector2>();

        float x = input.x;
        float y = input.y;
        
        if(!(x == 0 && y == 0))
        {
            anim.SetFloat("XInput", x);
            anim.SetFloat("YInput", y);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if(x != 0 && y != 0)
        {
            x = x / (float)System.Math.Sqrt(2);
            y = y / (float)System.Math.Sqrt(2);
        }

        moveDelta = new Vector3(x, y, 0);

        hit = Physics2D.BoxCast(boxCollider.offset, boxCollider.size, 0, new Vector3(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime * speed, 0);
        }
        
        hit = Physics2D.BoxCast(boxCollider.offset, boxCollider.size, 0, new Vector3(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * speed, 0, 0);
        }
    }

}
