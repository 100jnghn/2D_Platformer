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
        // Ű �Է¿� ���� ������
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed ) // right max speed
        {
            rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y);
            spriteRenderer.flipX = false; // ���� ��ȯ
        }
        else if (rigid.velocity.x < maxSpeed *-1) // left max speed
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
            spriteRenderer.flipX = true;// ���� ��ȯ
        }

        // ���� �� ����
        // Ray�� Platform ���̾�� ���� �浹�� ��ü���� �Ÿ��� 0.5���� ������ ���� ���� �ƴ� �����̴�.
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

        // ����ٸ�
        if(Mathf.Abs(rigid.velocity.x) <= 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else // �����̴� ���̶��
        {
            anim.SetBool("isWalking", true);
        }

        // ����
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
        // �÷��̾ ���� ���·� �����
        // ���̾ PlayerDamaged�� ����
        gameObject.layer = 10;

        // �÷��̾� �����ϰ� ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // �÷��̾ ƨ�ܳ���
        int dir = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dir, 1) * 7, ForceMode2D.Impulse);

        // �ִϸ��̼� ����
        anim.SetTrigger("doDamaged");



        // �������� ����
        Invoke("offDamaged", 2f);
    }

    void offDamaged()
    {
        // ���̾ �ٽ� Player�� ����
        gameObject.layer = 9;

        // �÷��̾� ���� ȿ�� ���� ����
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
