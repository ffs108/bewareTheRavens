// ISTA 425 / INFO 525 Algorithms for Games
//
// Sample code file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float defaultPosY = -3.92f;

    [Tooltip("Frame-rate independent movement")]
    public float MoveRate = 5.0f;

    [Tooltip("Player-relative ortho camera")]
    public GameObject playerCamera;

    [Tooltip("Time to fade sounds end of action, in seconds")]
    public float fadeTime = 0.1f;

    public float JumpForce = 3.0f;
    public float Gravity = -0.1f;
    public float curAcceleration;

    // local reference to GameController EventSystem
    GameObject GC;
    GameController eventSystem;

    Animator wizardAnim;
    SpriteRenderer wizardSprite;

    enum ActionType
    {
        Cast,
        Hack,
        Jump,
        Die
    }

    // Reality check: Is this character alive?
    bool dead = false;

    // Is the character on the ground? If not, are they in mid-air falling?
    bool onGround = true;

    // animation state machine metadata
    int runTrigger = Animator.StringToHash("isRunning");
    int idleTrigger = Animator.StringToHash("isIdling");
    int jumpTrigger = Animator.StringToHash("isJumping");
    int fallTrigger = Animator.StringToHash("isFalling");
    int deathTrigger = Animator.StringToHash("isDying");

    int attackTrigger = Animator.StringToHash("isAttacking");
    int attackType = Animator.StringToHash("attackType");

    private float distancedMoved = 0;

    public void SetAnim(float x, float y)
    {
        SetAnimState(x, y);
    }

    // Private method sends messages to the animation state machine based
    // on the current user input for running, attacking, etc.
    private void SetAnimState(float x, float y)
    {
        bool cast = false;
        bool hack = false;
        bool jump = false;
        bool die = false;

        // get new inputs only if wizard is not dead
        if (!dead)
        {
            cast = eventSystem.getInput(GameController.ControlType.Cast);
            hack = eventSystem.getInput(GameController.ControlType.Hack);
            jump = eventSystem.getInput(GameController.ControlType.Jump);
            die = eventSystem.getInput(GameController.ControlType.Die);
        }

        // set the state of the controller
        AnimatorStateInfo state = wizardAnim.GetCurrentAnimatorStateInfo(0);

        if (die)
        {
            wizardAnim.SetTrigger(deathTrigger);
            dead = true;
        }

        if (jump)
        {
            Debug.Log("jump trigger");
            wizardAnim.SetTrigger(jumpTrigger);
        }

        // set movement state if wizard is moving left or right
        if (x != 0.0f)
        {
            // face the direction of move
            if (x > 0.0f)
                wizardSprite.flipX = false;
            else if (x < 0.0f)
                wizardSprite.flipX = true;
            if (onGround)
            {
                wizardAnim.SetTrigger(runTrigger);
            }

        }
        // the wizard is standing still, idling
        else if (x == 0.0f && y == 0.0f)
        {
            wizardAnim.SetTrigger(idleTrigger);

            // spell casting takes precendence over attacking
            if (cast)
            {
                wizardAnim.SetInteger(attackType, (int)ActionType.Cast);
                wizardAnim.SetTrigger(attackTrigger);
            }
            else if (hack)
            {
                wizardAnim.SetInteger(attackType, (int)ActionType.Hack);
                wizardAnim.SetTrigger(attackTrigger);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController");
        eventSystem = GC.GetComponent<GameController>();

        curAcceleration = 0.0f;

        wizardAnim = GetComponent<Animator>();
        wizardSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0.0f;
        float y = 0.0f;

        // get new inputs only if wizard is not dead
        if (!dead)
        {
            // input from up/down, left/right keys
            x = eventSystem.getAxis(GameController.AxisType.X);
            y = eventSystem.getAxis(GameController.AxisType.Y);
            if (eventSystem.getInput(GameController.ControlType.Jump) && onGround)
            {
                onGround = false;
                curAcceleration = JumpForce;
            }
        }

        // setup the wizard's current animation state.
        SetAnimState(x, y);

        Vector3 move = new Vector3(x, 0.0f, 0.0f) * MoveRate * Time.deltaTime;
        // if wizard is moving
        if (move != Vector3.zero)
        {
            // increment the scroller position for the background sprites
            float totalMove = eventSystem.playerMove.x + move.x;
            distancedMoved += Mathf.Abs(move.x); // gets the absolute distance traveled to use in the procedual background  
            float clampMove = eventSystem.clamp(totalMove);

            eventSystem.scrollerMove.x = totalMove;
            eventSystem.playerMove.x = totalMove;

        }
    }

    void FixedUpdate()
    {
        if (!onGround)
        {
            if ((transform.position + transform.up * curAcceleration * Time.fixedDeltaTime).y <= defaultPosY)
            {
                transform.position = new Vector3(transform.position.x, defaultPosY, transform.position.z);
                curAcceleration = 0.0f;
                onGround = true;
            }
            else
            {
                curAcceleration += Gravity;
                transform.position += transform.up * curAcceleration * Time.fixedDeltaTime;
            }

        }

    }

    /*
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("adsf");
        if (collider.gameObject.tag == "Hazard" || collider.gameObject.tag == "Enemy")
        {
            //float x = eventSystem.getAxis(GameController.AxisType.X);
            //float y = eventSystem.getAxis(GameController.AxisType.Y);

            //eventSystem.playerDeath();
            //SetAnimState(0.0f, 0.0f);
        }
    }*/

    // get the distanced moved
    public float GetDistancedMoved()
    {
        return distancedMoved;
    }

    // set a new distanced moved
    public void SetDistanceMoved(int newMove)
    {
        distancedMoved = newMove;
    }
}
