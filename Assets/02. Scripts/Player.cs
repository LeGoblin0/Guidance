using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("캐릭터 이동 속도")]
    public float AirMoveSpeed;
    [Tooltip("캐릭터 지상이동 속도")]
    public float GroundMoveSpeed;
    public float JumpPower = 5;
    [Tooltip("몇 초 동안 넉백 될지")]
    public float KnockBackDelay;
    [Tooltip("넉백 힘")]
    public float KnockBackPower;
    Rigidbody2D rig;
    SpriteRenderer spriteRenderer;
    GameObject obj;
    Animator ani;
    public Transform anImg;

    [Tooltip("0=공중 , 1= 지상")]
    public int Mode = 0;

    bool JumpKey;
    float keyTime;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    //private int parabola = 0; // 0일시 지상 모드 충돌시 포물선으로 1일시 넉백 공중모드는 포물선 없애는 변수
    public int Speed;
    private float FastSpeed;
    public float AddSpeed;
    public float DefaultTime;
    private float DashTime;

    float moveX, moveY;

    float moveInput;

    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<GameObject>();
        rig = GetComponent<Rigidbody2D>();
        AirMoveSpeed = Speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = anImg.GetComponent<Animator>();
    }
    [SerializeField]
    [Header("스테미너")]
    float Stm = 100;
    private void FixedUpdate()
    {
        if (Mode == 1)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rig.velocity = new Vector2(moveInput * Speed, rig.velocity.y);
        }
    }

    // Update is called once per frame
    public Transform Baby;
    void ChangeMode()
    {
        if (Mode == 0)
        {
            rig.gravityScale = 0;
            rig.velocity = Vector2.zero;
        }
        else
        {
            rig.gravityScale = 2;
        }
    }

    int PointX = 0;
    int PointY = 0;
    int LastMoveX = 1;
    int LastMoveY = 0;
    bool Desh = false;
    void SetBaby()
    {
        Baby.tag = "Baby";
    }
    void Update()
    {
        
        
        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.DownArrow) && Stm>0)//모드 전환
        {
            if (Mode == 1)
            {
                Mode = 0;
                Invoke("SetBaby", .1f);
                if (!Baby.gameObject.activeSelf)
                {
                    Baby.tag = "Null";
                    Baby.transform.position = transform.position;
                    Baby.gameObject.SetActive(true);
                    int xx = 0, yy = 0;
                    if (Input.GetKey(KeyCode.UpArrow)) yy = 1;
                    if (Input.GetKey(KeyCode.RightArrow)) xx = 1;
                    if (Input.GetKey(KeyCode.LeftArrow)) xx = -1;
                    Baby.GetComponent<Rigidbody2D>().velocity = new Vector3(-xx, -yy, 0);
                }
                ChangeMode();
                Desh = false;
            }
        }
        if (Mode == 0)//공중모드
        {
            if (Stm <= 0)//스테미너가 0이면
            {
                Mode = 1;
                ChangeMode();//지상모드
            }
            else
            {
                Stm -= Time.deltaTime;
            }
            //애니메니션
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    anImg.localScale = new Vector2(+1, 1);
                    ani.SetInteger("Move", 2);

                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    anImg.localScale = new Vector2(-1, 1);
                    ani.SetInteger("Move", 2);

                }
                else ani.SetInteger("Move", 1); 
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    anImg.localScale = new Vector2(+1, 1);
                    ani.SetInteger("Move", 4);

                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    anImg.localScale = new Vector2(-1, 1);
                    ani.SetInteger("Move", 4);

                }
                else ani.SetInteger("Move", 5);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                ani.SetInteger("Move", 3);
                anImg.localScale = new Vector2(+1, 1);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                ani.SetInteger("Move", 3);
                anImg.localScale = new Vector2(-1, 1);
            }
            else
            {
                ani.SetInteger("Move", 0);
            }//애니메이션 끝

            if (Input.GetKeyDown(KeyCode.Space))//데쉬 입력
            {
                FastSpeed = 1;
            }
            rig.velocity = new Vector3(moveX, moveY, 0);
            //Debug.Log("x : " + (int)moveX+ "  y : " + (int)moveY);
            if (DashTime <= 0)//데쉬중이 아닐때
            {
                //부드러운 이동 코드
                moveX = Input.GetAxis("Horizontal") * AirMoveSpeed;// * Time.deltaTime;
                moveY = Input.GetAxis("Vertical") * AirMoveSpeed;// * Time.deltaTime;

                if (moveX > 0) LastMoveX = 1;
                else if (moveX < 0) LastMoveX = -1;
                else LastMoveX = 0;
                if (moveY > 0) LastMoveY = 1;
                else if (moveY < 0) LastMoveY = -1;
                else  LastMoveY = 0;

                AirMoveSpeed = Speed;
                Desh = false;
                rig.sharedMaterial = null;
                if (FastSpeed > 0) //데쉬중이 아닐때 대쉬 시작할때
                {
                    Stm -= 20;
                    
                    AirMoveSpeed = AddSpeed;
                    Desh = true;
                    DashTime = DefaultTime;
                }
            }
            else //데쉬중일때
            {
                moveX = LastMoveX * AirMoveSpeed;//데쉬는 항상 최고속도
                moveY = LastMoveY * AirMoveSpeed;//데쉬는 항상 최고속도
                DashTime -= Time.deltaTime;

            }
            FastSpeed = 0;
       
        }
        else if (Mode == 1)//지상모드
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                PointX = -1;
            }
            else if ( Input.GetKey(KeyCode.RightArrow)) 
            {
                PointX = 1;
            }
            else
            {
                PointX = 0;
            }

            if (JumpNow && Input.GetKeyDown(KeyCode.F))//점프
            {
                JumpNow = false;
                isJumping = true;
                jumpTimeCounter = jumpTime;
                rig.velocity = new Vector2(0, JumpPower);
                transform.Translate(new Vector3(PointX, 0, 0) * Time.deltaTime * GroundMoveSpeed);
            }
            if (Input.GetKey(KeyCode.F) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    rig.velocity = new Vector2(0, JumpPower);
                    transform.Translate(new Vector3(PointX, 0, 0) * Time.deltaTime * GroundMoveSpeed);
                    jumpTimeCounter -= Time.deltaTime;
                    rig.gravityScale = 4;
                }
                else
                {
                    rig.gravityScale = 2;
                    isJumping = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                isJumping = false;
            }
        }

    }
    bool JumpNow = true;
    public Transform PlayerSens;
    /// <summary>
    /// 충돌 체크
    /// </summary>
    /// <param name="i">1=up 2=down 3=left 4=right</param>
    public void HitGround(int i)
    {
        if (Mode == 1)
        {
            if (i == 2)
            {
                JumpNow = true;//점프 가능
                Stm = 100;
            }
        }
        else if (Mode == 0)
        {
            if (DashTime > 0 && (i == 1 || i == 2))
            {
                LastMoveY = -LastMoveY;
                DashTime = .1f;
                Stm -= 10;
            }

            if (DashTime > 0 && (i == 3 || i == 4))
            {
                LastMoveX = -LastMoveX;
                DashTime = .1f;
                Stm -= 10;
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Baby") //아이와 충돌시
        {
            if (Mode == 0)
            {
                Mode = 1;
                ChangeMode();
                Baby.gameObject.SetActive(false);
                rig.velocity = Vector3.zero; 
                Baby.tag = "Null";
            }
        }
    }
    
}
