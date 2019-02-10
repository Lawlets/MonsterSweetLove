using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {

    // Use this for initialization


   
    public void OnScalePlus ()
    {
        RectTransform rectangle = (RectTransform)gameObject.transform;
        rectangle.transform.localScale += new Vector3(0.5f,0.5f, 0);
        print("ji");
    }

    public void OnscaleNormal()
    {
        print("jo");
        RectTransform rectangle = (RectTransform)gameObject.transform;
        rectangle.transform.localScale = new Vector3(1, 1, 0);
    }

}
