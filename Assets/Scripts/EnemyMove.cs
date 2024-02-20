using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        think();
    }

    void FixedUpdate()
    {
        // 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 지형 체크
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, Color.yellow);
        RaycastHit2D rayHIt = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));
    
        // 앞이 낭떠러지라면 방향 전환
        if(rayHIt.collider == null)
        {
            turn();
        }
    }

    // 재귀 함수
    void think()
    {
        nextMove = Random.Range(-1, 2);

        // 애니메이션 변경
        anim.SetInteger("walkSpeed", nextMove); 

        // 쳐다보는 방향 전환
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        // 재귀함수 호출은 보통 제일 마지막에 작성
        float nextThinkTime = Random.Range(2f, 5f); // 생각하는 쿨타임 랜덤으로
        Invoke("think", nextThinkTime);
    }

    void turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // 5초 후 think() 하는 Invoke 취소
        Invoke("think", 5); // 다시 Invoke로 think() 예약
    }
}
