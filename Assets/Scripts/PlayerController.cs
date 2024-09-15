using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer; // Zemin layer'�

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private float moveInput;

    private SpriteRenderer spriteRenderer;

    public BoxCollider2D[] whiteWalls;
    public BoxCollider2D[] blueWalls;
    public BoxCollider2D[] redWalls;

    public GameObject whitePlayer;
    public GameObject bluePlayer;
    public GameObject redPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();


    }

    void Update()
    {
        // BoxCollider2D kullanarak zeminle temas kontrol�
        isGrounded = IsGrounded();

        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)  // Zeminle temas varsa veya z�plama hakk� varsa
            {
                Jump();
            }
        }

        // Karakterin y�n�n� belirleme
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Sa�a bak
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Sola bak
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    bool IsGrounded()
    {
        // Zeminle temas kontrol�
        // BoxCollider2D'nin alt k�sm�nda bir raycast �iziyoruz ve zeminle temas edip etmedi�ini kontrol ediyoruz
        float colliderHeight = boxCollider.size.y;
        float colliderWidth = boxCollider.size.x;
        Vector2 colliderCenter = boxCollider.bounds.center;

        // Raycast'lerin a�a��ya do�ru do�ru �al��t���ndan emin olun
        return Physics2D.BoxCast(colliderCenter, new Vector2(colliderWidth, colliderHeight), 0f, Vector2.down, 0.1f, groundLayer);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Herhangi bir nesne ile temas ettiğinde rengi maviye çevir
        if (collision.gameObject.CompareTag("BlueColor")) // Temas edecek nesneye uygun tag ver
        {
            Debug.Log("Renk mavi");
            spriteRenderer.color = Color.blue;
            foreach (BoxCollider2D wall in blueWalls)
            {
                wall.enabled = false;
            }
            foreach (BoxCollider2D wall in redWalls)
            {
                wall.enabled = true;
            }
            foreach (BoxCollider2D wall in whiteWalls)
            {
                wall.enabled = true;
            }
            whitePlayer.SetActive(false);
            redPlayer.SetActive(false);
            bluePlayer.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("RedColor"))
        {
            Debug.Log("Renk kırmızı");
            spriteRenderer.color = Color.red;
            foreach (BoxCollider2D wall in blueWalls)
            {
                wall.enabled = true;
            }
            foreach (BoxCollider2D wall in redWalls)
            {
                wall.enabled = false;
            }
            foreach (BoxCollider2D wall in whiteWalls)
            {
                wall.enabled = true;
            }
            whitePlayer.SetActive(false);
            redPlayer.SetActive(true);
            bluePlayer.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("WhiteColor"))
        {
            Debug.Log("Renk beyaz");
            spriteRenderer.color = Color.white;
            foreach (BoxCollider2D wall in blueWalls)
            {
                wall.enabled = true;
            }
            foreach (BoxCollider2D wall in redWalls)
            {
                wall.enabled = true;
            }
            foreach (BoxCollider2D wall in whiteWalls)
            {
                wall.enabled = false;
            }
            whitePlayer.SetActive(true);
            redPlayer.SetActive(false);
            bluePlayer.SetActive(false);
        }
    }
}
