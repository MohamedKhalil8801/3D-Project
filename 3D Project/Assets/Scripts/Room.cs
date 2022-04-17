using UnityEngine;

public class Room : MonoBehaviour
{
    
    public GameObject Door;
    public GameObject Wall_F;
    public GameObject Wall_B;
    public GameObject Wall_L;
    public GameObject Wall_R;

    public Room PrevRoom;
    public RoomType Entrance;

    private RoomType roomType;
    bool canSpawn = true;

    public void SetRoomType(RoomType type)
    {
        roomType = type;
        transform.name = "Room " + roomType;
        ConfigureRoomType(roomType);
    }

    private void Update()
    {
        if (canSpawn)
        {
            SpawnNextRoom();
        }
        
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }

    private void ConfigureRoomType(RoomType type)
    {
        switch (type)
        {
            case RoomType.F:
                ReplaceWallWithDoor(Door, Wall_F);
                break;
            case RoomType.B:
                ReplaceWallWithDoor(Door, Wall_B);
                break;
            case RoomType.L:
                ReplaceWallWithDoor(Door, Wall_L);
                break;
            case RoomType.R:
                ReplaceWallWithDoor(Door, Wall_R);
                break;
            case RoomType.FB:
                ReplaceWallWithDoor(Door, Wall_F, Wall_B);
                break;
            case RoomType.FL:
                ReplaceWallWithDoor(Door, Wall_F, Wall_L);
                break;
            case RoomType.FR:
                ReplaceWallWithDoor(Door, Wall_F, Wall_R);
                break;
            case RoomType.BL:
                ReplaceWallWithDoor(Door, Wall_B, Wall_L);
                break;
            case RoomType.BR:
                ReplaceWallWithDoor(Door, Wall_B, Wall_R);
                break;
            case RoomType.LR:
                ReplaceWallWithDoor(Door, Wall_L, Wall_R);
                break;
            default:
                break;
        }
    }

    private void ReplaceWallWithDoor(GameObject door, params GameObject[] walls)
    {
        foreach (GameObject wall in walls)
        {
            string name = wall.name;
            Vector3 pos = wall.transform.position;
            Quaternion rot = wall.transform.rotation;
            Instantiate(door, pos, rot, gameObject.transform).name = name;
            Destroy(wall);
        }
    }

    private void SpawnNextRoom()
    {
        if (!Wall_F)
        {
            GameObject.FindObjectOfType<RoomGenerator>().SpawnNextRoom(RoomType.F, this, transform.position);
            canSpawn = false;
        }
        if (!Wall_B)
        {
            GameObject.FindObjectOfType<RoomGenerator>().SpawnNextRoom(RoomType.B, this, transform.position);
            canSpawn = false;
        }
        if (!Wall_L)
        {
            GameObject.FindObjectOfType<RoomGenerator>().SpawnNextRoom(RoomType.L, this, transform.position);
            canSpawn = false;
        }
        if (!Wall_R)
        {
            GameObject.FindObjectOfType<RoomGenerator>().SpawnNextRoom(RoomType.R, this, transform.position);
            canSpawn = false;
        }
    }

    public void GetEntrance()
    {
        string prevStr = GameObject.FindObjectOfType<RoomGenerator>().NegateRoomType(PrevRoom.GetRoomType()).ToString();
        string currentStr = GetRoomType().ToString();

        string newStr = currentStr;

        foreach (char item in prevStr)
        {
            newStr = newStr.Replace(item.ToString(), "");
        }

        Debug.Log(transform.name + " " + newStr);
    }
}
