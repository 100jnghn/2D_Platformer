using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;

    public float maxSpeed;
    public float stopSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 키 입력에 따른 움직임
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed ) // right max speed
        {
            rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed *-1) // left max speed
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        }
    }

    void Update()
    {
        // stop speed
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * stopSpeed, rigid.velocity.y);
        }
    }
}
