using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    private Rigidbody2D rb;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 使用方向键来控制玩家2的移动
        movement.x = Input.GetAxisRaw("Horizontal2");
        movement.y = Input.GetAxisRaw("Vertical2");
    }

    void FixedUpdate()
    {
        // 移动玩家2
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.AddForce(movement * moveSpeed);
    }
}
