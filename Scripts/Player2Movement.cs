using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
    private Rigidbody2D rb;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ʹ�÷�������������2���ƶ�
        movement.x = Input.GetAxisRaw("Horizontal2");
        movement.y = Input.GetAxisRaw("Vertical2");
    }

    void FixedUpdate()
    {
        // �ƶ����2
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.AddForce(movement * moveSpeed);
    }
}
