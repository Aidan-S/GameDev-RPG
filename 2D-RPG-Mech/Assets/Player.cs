using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private SpriteRenderer spriteRenderer;
    public Sprite sprite_idle;
    public Sprite sprite_right;
    public Sprite sprite_left;
    public Sprite sprite_up;
    public Sprite sprite_down;
    private RaycastHit2D hit;
    private Camera cam;
    public GameObject myPrefab;
    public GameObject swingHitbox;

    public int speed;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void attack()
    {
        Instantiate(myPrefab, this.transform.position, Quaternion.identity);
    }

    private void swing()
    {
        swingHitbox.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Vector3 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseInput.z = 0;
        mouseInput = Vector3.Normalize(mouseInput);
        Vector2 a1 = new Vector2(mouseInput.x, mouseInput.y);
        Vector2 a2 = new Vector2(0, 0);
       
        float angle = (Mathf.Atan2(a1.y, a1.x) * 180) / Mathf.PI;

        swingHitbox.transform.Rotate(0, 0, angle);

        StartCoroutine(inAndOut());    
    }


    IEnumerator inAndOut()
    {
        swingHitbox.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        swingHitbox.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            attack();
        }

        if (Input.GetKeyDown("z"))
        {
            swing();
        }
    }

    private void FixedUpdate()
    {
        

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        //up down if no horizontal movement
        if(x == 0)
        {
            if(y > 0)
            {
                spriteRenderer.sprite = sprite_up;
            }
            else if(y < 0)
            {
                spriteRenderer.sprite = sprite_down;
            }
            else
            {
                spriteRenderer.sprite = sprite_idle;
            }
        }
        else
        //otherwise, we want side to take precident
        {
            if(x > 0)
            {
                spriteRenderer.sprite = sprite_right;
            }
            else
            {
                spriteRenderer.sprite = sprite_left;
            }
        }

        if(x != 0 && y != 0)
        {
            x = x / (float)System.Math.Sqrt(2);
            y = y / (float)System.Math.Sqrt(2);
        }

        moveDelta = new Vector3(x, y, 0);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime * speed, 0);
        }
        
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector3(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * speed, 0, 0);
        }
    }

}
