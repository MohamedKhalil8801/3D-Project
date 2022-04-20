using System.Collections;
using UnityEngine;

public class NextRoomDetector : MonoBehaviour
{
    private RoomGenerator roomGenerator;
    private GameObject wall;
    private GameObject door;

    private void Awake()
    {
        roomGenerator = RoomGenerator.Instance;
        wall = roomGenerator.Wall;
        door = roomGenerator.Door;
    }


    void Start()
    {
        StartCoroutine(Detect());
    }

    IEnumerator Detect()
    {
        yield return new WaitForSeconds(1);
        //Debug.DrawRay(transform.TransformPoint(new Vector3(15, 4, -0.2f)), transform.TransformDirection(Vector3.forward), Color.red, 120);
        //Debug.DrawRay(transform.TransformPoint(new Vector3(15, 2, -0.2f)), transform.TransformDirection(Vector3.forward), Color.red, 120);

        // Delete Door if facing empty space
        if (!Physics.Raycast(transform.TransformPoint(new Vector3(15, 4, -0.2f)), transform.TransformDirection(Vector3.forward), 1))
        {
            Instantiate(wall, transform.position, transform.rotation, transform.parent).name = transform.name;
            Destroy(gameObject);
        }

        // Delete Wall if facing Door
        RaycastHit hit;

        if (Physics.Raycast(transform.TransformPoint(new Vector3(15, 2, -0.2f)), transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            if (hit.collider.GetComponent<Wall>())
            {
                Instantiate(door, hit.collider.transform.position, hit.collider.transform.rotation, hit.collider.transform.parent).name = hit.collider.transform.name;
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
