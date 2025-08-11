using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f; // 移动速度
    private Rigidbody2D rb;
    private Character player1Character;

    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player1Character = GetComponent<Character>();
    }

    void Update()
    {
        // 使用 Horizontal1 和 Vertical1 来控制玩家1的移动
        movement.x = Input.GetAxisRaw("Horizontal1");
        movement.y = Input.GetAxisRaw("Vertical1");
    }

    void FixedUpdate()
    {
        // 移动玩家1
        rb.AddForce(movement * moveSpeed);
    }

    //void GetHit(GameObject hitObject)
    //{
    //    if (hitObject.gameObject.CompareTag("Punch2"))
    //    {
    //        player1Character.currentHealth -= 1;
    //        Debug.Log("Player1被击中了");
    //    }
    //}
}
