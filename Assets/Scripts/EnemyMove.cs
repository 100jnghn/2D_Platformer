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
        // ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // ���� üũ
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector2.down, Color.yellow);
        RaycastHit2D rayHIt = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));
    
        // ���� ����������� ���� ��ȯ
        if(rayHIt.collider == null)
        {
            turn();
        }
    }

    // ��� �Լ�
    void think()
    {
        nextMove = Random.Range(-1, 2);

        // �ִϸ��̼� ����
        anim.SetInteger("walkSpeed", nextMove); 

        // �Ĵٺ��� ���� ��ȯ
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }

        // ����Լ� ȣ���� ���� ���� �������� �ۼ�
        float nextThinkTime = Random.Range(2f, 5f); // �����ϴ� ��Ÿ�� ��������
        Invoke("think", nextThinkTime);
    }

    void turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke(); // 5�� �� think() �ϴ� Invoke ���
        Invoke("think", 5); // �ٽ� Invoke�� think() ����
    }
}
