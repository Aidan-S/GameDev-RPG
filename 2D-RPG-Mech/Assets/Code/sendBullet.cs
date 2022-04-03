using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendBullet : MonoBehaviour
{
    private Vector3 mouseInput; 
    public float speed;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        mouseInput.z = 0;
        mouseInput = Vector3.Normalize(mouseInput);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(mouseInput.x * Time.deltaTime * speed, mouseInput.y * Time.deltaTime * speed, 0);
        if(System.Math.Abs((player.transform.position - transform.position).magnitude) > 20)
        {
            Destroy (gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            Destroy (gameObject);
        }
    }
}
