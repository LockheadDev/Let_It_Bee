using UnityEngine;

public class CursorBeh : MonoBehaviour
{

    public bool isTouchingBee = false;
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = mousePosition;
    }
     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bee")
        {
            isTouchingBee = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bee")
        {
            isTouchingBee = false;
        }
    }
}
