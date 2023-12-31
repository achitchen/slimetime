using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int speed = 5;
    [SerializeField] GameObject spriteObject;
    private Vector2 lookDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    public Direction direction = Direction.Zero;
    public bool isMoving;
    private Animator animator;
    private PlayerDodge playerDodge;

    // Start is called before the first frame update
    void Start()
    {
        lookDir = Vector2.zero;
        moveDir = Vector2.zero;
        direction = Direction.Zero;
        playerDodge = GetComponent<PlayerDodge>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDodge.riposteActivated && !GameObject.Find("Game Manager").GetComponent<GameManager>().isDead)
        {
            GetMoveDir();
            transform.Translate(speed * moveDir.normalized * Time.deltaTime);
            if (playerDodge.dodgeActive == PlayerDodge.Dodge.None)
            {
                if (isMoving)
                {
                    animator.SetTrigger("runTrigger");
                    animator.ResetTrigger("spikeTrigger");
                    animator.ResetTrigger("idleTrigger");
                    animator.ResetTrigger("duckTrigger");
                    animator.ResetTrigger("splitTrigger");
                    animator.ResetTrigger("reflectTrigger");
                }
                else
                {
                    animator.SetTrigger("idleTrigger");
                    animator.ResetTrigger("spikeTrigger");
                    animator.ResetTrigger("runTrigger");
                    animator.ResetTrigger("duckTrigger");
                    animator.ResetTrigger("splitTrigger");
                    animator.ResetTrigger("reflectTrigger");
                }
            }
            if (lookDir == Vector2.left)
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if (lookDir == Vector2.right)
            {
                spriteObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void GetMoveDir()
    {
        if (Input.GetKey(KeyCode.A))
        {
            isMoving = true;
            if (direction == Direction.Zero || direction == Direction.Right)
            {
                direction = Direction.Left;
                lookDir = Vector2.left;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                {
                    direction = Direction.Left;
                    lookDir = Vector2.left;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDir = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector2(-1, -1);
            }
            else
            {
                moveDir = Vector2.left;
            }
        }

        else if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            if (direction == Direction.Zero || direction == Direction.Down)
            {
                direction = Direction.Up;
                lookDir = Vector2.up;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    direction = Direction.Up;
                    lookDir = Vector2.up;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDir = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir = Vector2.one;
            }
            else
            {
                moveDir = Vector2.up;
            }
        }

        else if (Input.GetKey(KeyCode.D))
        {
            isMoving = true;
            if (direction == Direction.Zero || direction == Direction.Left)
            {
                direction = Direction.Right;
                lookDir = Vector2.right;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
                {
                    direction = Direction.Right;
                    lookDir = Vector2.right;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                moveDir = Vector2.one;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDir = new Vector2(1, -1);
            }
            else
            {
                moveDir = Vector2.right;
            }
        }

        else if (Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            if (direction == Direction.Zero || direction == Direction.Up)
            {
                direction = Direction.Down;
                lookDir = Vector2.down;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
                {
                    direction = Direction.Down;
                    lookDir = Vector2.down;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDir = new Vector2(-1, -1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir = new Vector2(1, -1);
            }
            else
            {
                moveDir = Vector2.down;
            }
        }

        else if (direction != Direction.Zero)
        {
            isMoving = false;
            direction = Direction.Zero;
            moveDir = Vector2.zero;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Zero
    }
}
