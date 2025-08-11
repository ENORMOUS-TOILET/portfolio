using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f; // �ƶ��ٶ�
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
        // ʹ�� Horizontal1 �� Vertical1 ���������1���ƶ�
        movement.x = Input.GetAxisRaw("Horizontal1");
        movement.y = Input.GetAxisRaw("Vertical1");
    }

    void FixedUpdate()
    {
        // �ƶ����1
        rb.AddForce(movement * moveSpeed);
    }

    //void GetHit(GameObject hitObject)
    //{
    //    if (hitObject.gameObject.CompareTag("Punch2"))
    //    {
    //        player1Character.currentHealth -= 1;
    //        Debug.Log("Player1��������");
    //    }
    //}
}
