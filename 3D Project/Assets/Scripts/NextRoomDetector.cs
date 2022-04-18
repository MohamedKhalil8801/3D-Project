using System.Collections;
using UnityEngine;

public class NextRoomDetector : MonoBehaviour
{
    public GameObject Wall;

    void Start()
    {
        StartCoroutine(Detect());
    }

    IEnumerator Detect()
    {
        yield return new WaitForSeconds(3);
        Debug.DrawRay(transform.TransformPoint(new Vector3(15, 4, -0.2f)), transform.TransformDirection(Vector3.forward), Color.red, 120);
        if (!Physics.Raycast(transform.TransformPoint(new Vector3(15, 4, -0.2f)), transform.TransformDirection(Vector3.forward), 1))
        {
            Debug.Log(transform.parent.name);
            Instantiate(Wall, transform.position, transform.rotation, transform.parent).name = transform.name;
            Destroy(gameObject);
        }
    }
}
