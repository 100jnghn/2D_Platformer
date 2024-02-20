using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    public float maxSpeed;
    public float stopSpeed;
    public float jumpPower;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 키 입력에 따른 움직임
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed ) // right max speed
        {
            rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y);
            spriteRenderer.flipX = false; // 방향 전환
        }
        else if (rigid.velocity.x < maxSpeed *-1) // left max speed
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
            spriteRenderer.flipX = true;// 방향 전환
        }

        // 점프 후 착지
        // Ray를 Platform 레이어에만 쏴서 충돌한 물체와의 거리가 0.5보다 작으면 점프 중이 아닌 상태이다.
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector2.down, Color.green);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
            }
        }
        
        
    }

    void Update()
    {
        // stop speed
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * stopSpeed, rigid.velocity.y);
        }

        // 멈췄다면
        if(Mathf.Abs(rigid.velocity.x) <= 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else // 움직이는 중이라면
        {
            anim.SetBool("isWalking", true);
        }

        // 점프
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            onDamaged(collision.transform.position);
        }
    }

    void onDamaged(Vector2 targetPos)
    {
        // 플레이어를 무적 상태로 만들기
        // 레이어를 PlayerDamaged로 변경
        gameObject.layer = 10;

        // 플레이어 투명하게 변경
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 플레이어가 튕겨나감
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dir, 1) * 7, ForceMode2D.Impulse);

        // 애니메이션 변경
        anim.SetTrigger("doDamaged");



        // 무적상태 해제
        Invoke("offDamaged", 2f);
    }

    void offDamaged()
    {
        // 레이어를 다시 Player로 변경
        gameObject.layer = 9;

        // 플레이어 투명 효과 원상 복구
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
