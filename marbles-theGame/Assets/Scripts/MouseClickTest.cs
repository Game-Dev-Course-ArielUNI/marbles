using UnityEngine;

public class MouseClickTest : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown fired on " + gameObject.name);
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp fired on " + gameObject.name);
    }
}
