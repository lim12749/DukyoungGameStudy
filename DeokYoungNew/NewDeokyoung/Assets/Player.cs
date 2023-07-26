using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriternder;
    public Sprite[] sprites;
    private int spriteIndex;
    private Rigidbody2D _rb;
    public AudioSource audioSource;
    public float strength = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        spriternder = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprete), 0.15f, 0.15f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //키보드 마우스 인풋
        {
            _rb.velocity = Vector2.up * strength;
            audioSource.Play();
        }
        /*
        if(Input.touchCount >0) //터치 인풋
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
            }
        } 
        */
        //direction.y += gravity * Time.deltaTime;

    }
    private void AnimateSprete()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriternder.sprite = sprites[spriteIndex];
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Optical")
        {
            FindObjectOfType<GameManager>().GameOverFnc();
            Debug.Log("?");
        }
        else if (other.gameObject.tag == "Score")
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }
}

