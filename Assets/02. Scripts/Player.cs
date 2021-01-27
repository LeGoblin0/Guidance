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

    

    //private int parabola = 0; // 0일시 지상 모드 충돌시 포물선으로 1일시 넉백 공중모드는 포물선 없애는 변수
    public int Speed;
    private float FastSpeed;
    public float AddSpeed;
    public float DefaultTime;
    private float DashTime;

    float moveX, moveY;

    // Start is called before the first frame update
    void Start()
    {
        obj = GetComponent<GameObject>();
        rig = GetComponent<Rigidbody2D>();
        AirMoveSpeed = Speed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = anImg.GetComponent<Animator>();
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
    bool Desh = false;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))//모드 전환
        {
            if (Mode == 1)
            {
                Mode = 0;
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
            /*    
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

            //부드러운 이동 코드
            moveX = Input.GetAxis("Horizontal") * AirMoveSpeed * Time.deltaTime;
            moveY = Input.GetAxis("Vertical") * AirMoveSpeed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + moveX, transform.position.y + moveY);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                FastSpeed = 1;
            }
            if(DashTime <= 0)
            {
                rig.velocity = new Vector3(PointX, PointY, 0) * AirMoveSpeed;
                AirMoveSpeed = Speed;
                Desh = false;
                rig.sharedMaterial = null;
                if (FastSpeed > 0)
                {
                    Debug.Log(0);
                    AirMoveSpeed = AddSpeed;
                    rig.sharedMaterial = DDD;
                    rig.AddForce(new Vector3(PointX, PointY, 0) * AirMoveSpeed,ForceMode2D.Impulse);
                    //PlayerSens.gameObject.SetActive(false);
                    Desh = true;
                    DashTime = DefaultTime;
                }
            }
            else
            {
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
            if (JumpNow && Input.GetKey(KeyCode.F))//점프
            {
                JumpNow = false;
                rig.velocity = new Vector2(0, JumpPower);
            }
            transform.Translate(new Vector3(PointX, 0, 0) * Time.deltaTime * GroundMoveSpeed);
        }

    }
    bool JumpNow = true;
    public Transform PlayerSens;
    public PhysicsMaterial2D DDD;//데쉬할떄 머테리얼

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") //벽과 충돌 시
        {
            /*
           if (Mode == 0) //공중 모드 일 시 
           {
                parabola = 0;
                WallKnockBack(collision.transform.position); //넉백 함수 호출
                Invoke("KnockBackStop", KnockBackDelay); //KnockBackDelay 만큼 뒤에 넉백 멈춤
                parabola = 1;
           }
           else
           {
                WallKnockBack(collision.transform.position); //넉백 함수 호출
           }
           */
            //if (Mode == 0)
            //{
            //    PointX *= -2;
            //    PointY *= -2;
            //}
        }
        else if (collision.gameObject.tag == "Ground") //땅과 충돌 시
        {
            JumpNow = true;//점프 가능
            //if (Mode == 0)
            //{
            //    PointX *= -1;
            //    PointY *= -1;
            //    rig.velocity = Vector3.zero;
            //}
        }
        else if (collision.gameObject.tag == "Baby") //아이와 충돌시
        {
            if (Mode == 0)
            {
                Mode = 1;
                ChangeMode();
                Baby.gameObject.SetActive(false);
                Baby.tag = "Null";
            }
        }
        else if (collision.gameObject.tag == "Null") //아이와 충돌시
        {
                Baby.tag = "Baby";
        }

    }
    /*
    void WallKnockBack(Vector2 targetPos) //충돌시 반대편으로 넉백
    {
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //현재 왼쪽으로 이동중인지 오른쪽으로 이동중인지 판단후 반대로
        rig.AddForce(new Vector2(dirc, parabola) * KnockBackPower, ForceMode2D.Impulse); //힘을 준다
    }

    void KnockBackStop()//이동 멈춤
    {
        rig.velocity = Vector3.zero;
    }
    */
    
}
