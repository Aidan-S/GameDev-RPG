using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private SpriteRenderer spriteRenderer;
    private RaycastHit2D hit;
    private Camera cam;
    public GameObject myPrefab;
    public GameObject swingHitbox;
    private Animator anim;
    //red fire
    public GameObject redFire;
    private Animator redFireAnim;
    //blue fire
    public GameObject blueFire;
    private Animator blueFireAnim;
    //purple fire
    public GameObject purpleFire;
    private Animator purpleFireAnim;

    public int speed;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        anim = GetComponent<Animator>();
        redFireAnim = redFire.GetComponent<Animator>();
        purpleFireAnim = purpleFire.GetComponent<Animator>();
        blueFireAnim = blueFire.GetComponent<Animator>();
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
            redFireAnim.SetBool("isLit", !redFireAnim.GetBool("isLit"));
            blueFireAnim.SetBool("isLit", !blueFireAnim.GetBool("isLit"));
            purpleFireAnim.SetBool("isLit", !purpleFireAnim.GetBool("isLit"));
        }
    }

    private void FixedUpdate()
    {

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        if(!(x == 0 && y == 0))
        {
            anim.SetFloat("XInput", x);
            anim.SetFloat("YInput", y);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if(x != 0 && y != 0)
        {
            x = x / (float)System.Math.Sqrt(2);
            y = y / (float)System.Math.Sqrt(2);
        }

        moveDelta = new Vector3(x, y, 0);

        hit = Physics2D.BoxCast(boxCollider.offset, boxCollider.size, 0, new Vector3(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime * speed, 0);
        }
        
        hit = Physics2D.BoxCast(boxCollider.offset, boxCollider.size, 0, new Vector3(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime * speed), LayerMask.GetMask("Actor", "Blocking"));
        if(hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime * speed, 0, 0);
        }
    }

}
