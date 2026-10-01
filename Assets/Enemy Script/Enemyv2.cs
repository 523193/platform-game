using UnityEngine;

public class Enemyv2 : MonoBehaviour
{
 
    public Transform player;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public LayerMask groundLayerMask;

    float speed = 2f;

    bool isGrounded;
    bool groundAhead;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        groundLayerMask = LayerMask.GetMask("Ground");
    }


    void Update()
    {
  
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
