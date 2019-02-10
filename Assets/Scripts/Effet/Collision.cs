using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collision"))
        {
            transform.position = new Vector3((GetComponent<RectTransform>().rect.width +Random.Range(300,500)), Random.Range(130, 890), 0);
        }
    }
    
}
