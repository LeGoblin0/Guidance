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


    private void FixedUpdate()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rig.velocity = new Vector2(moveInput * Speed, rig.velocity.y);
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
            rig.gravityScale = 1;
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
        
        
        if (Input.GetKeyDown(KeyCode.Space))//모드 전환
        {
            if (Mode == 1)
            {
                Mode = 0;
                Baby.tag = "Null";
                Invoke("SetBaby", .1f);
                Baby.transform.position = transform.position;
                Baby.gameObject.SetActive(true);
                ChangeMode();
                Desh = false;
            }
        }
        if (Mode == 0)//공중모드
        {

            //마우스 포인터 따라다니면서 부드러운 이동(기획서 상 레퍼런스 영상의 움직임은 이런 느낌)
            // Vector3 Target = Camera.main.ScreenToWorldPoint(Input.mousePosition); //마우스 포인터 좌표

            //  Target.z = transform.position.z;
            //  transform.position = Vector3.MoveTowards(transform.position, Target, AirMoveSpeed * Time.deltaTime); 
            //   ㄴ마우스 포인터 좌표로 MoveTowards를 사용해서 따라옴

            //if (Input.GetButtonDown("Horizontal"))
            //{
            //    spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            //}
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
            }
            /*    
            if (Desh)
            {

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
               PointX = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {

                PointX = 1;
            }
            else
            {
                PointX = 0;
            }

            if (Desh)
            {

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                PointY = -1;
            }
        
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                PointY = 1;
            }
           
            else
            {
                PointY = 0;
            }
            */
            //if (PlayerSens.gameObject.activeSelf)
            //{
            //    PlayerSens.position = new Vector3(PointX, PointY, 0) + transform.position;
            //}

            //if (!PlayerSens.GetComponent<PlayerSenser>().Stop)
            //{
            //    transform.Translate(new Vector3(PointX, PointY, 0) * AirMoveSpeed * Time.deltaTime);
            //}

            //transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);
            //rig.velocity = Vector2.one;

            if (Input.GetKeyDown(KeyCode.LeftShift))//데쉬 입력
            {
                FastSpeed = 1;
            }
            rig.velocity = new Vector3(moveX, moveY, 0);
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
                    //moveY = LastMoveY;

                    AirMoveSpeed = AddSpeed;
                    //rig.sharedMaterial = DDD;
                    //rig.AddForce(new Vector3(PointX, PointY, 0) * AirMoveSpeed,ForceMode2D.Impulse);
                    //PlayerSens.gameObject.SetActive(false);
                    Desh = true;
                    DashTime = DefaultTime;
                }
            }
            else //데쉬중일때
            {
                moveX = LastMoveX * AirMoveSpeed;//데쉬는 항상 최고속도
                moveY = LastMoveY * AirMoveSpeed;//데쉬는 항상 최고속도
                //PlayerSens.gameObject.SetActive(true);
                DashTime -= Time.deltaTime;
                //AirMoveSpeed = AddSpeed;

            }
            //rig.velocity = Vector3.zero;
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
            /*
            if (Input.GetKeyDown(KeyCode.F))
            {
                JumpKey = true;
                keyTime = 0;
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                JumpKey = false;
                if (keyTime > 0.3f)
                    JumpPower = 8;
                else
                    JumpPower = 5;
            }

            if (true == JumpKey){
                keyTime += Time.deltaTime;}
            */
            /*
            if (JumpNow && Input.GetKeyDown(KeyCode.F))//기존 점프
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                JumpNow = false;
                rig.velocity = new Vector2(0, JumpPower);
            }
            transform.Translate(new Vector3(PointX, 0, 0) * Time.deltaTime * GroundMoveSpeed);
            */
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
                }
                else
                {
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
    //public PhysicsMaterial2D DDD;//데쉬할떄 머테리얼
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
            }
        }
        else if (Mode == 0)
        {
            if (DashTime > 0 && (i == 1 || i == 2))
            {
                //moveX = 0;
                //moveY = -moveY;
                LastMoveY = -LastMoveY;
                DashTime = .1f;
            }

            if (DashTime > 0 && (i == 3 || i == 4))
            {
                //moveY = 0;
                LastMoveX = -LastMoveX;
                DashTime = .1f;
                //moveX = -moveX;
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
