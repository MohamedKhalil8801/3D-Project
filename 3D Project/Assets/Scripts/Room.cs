using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    
    public GameObject Door;

    public GameObject Wall_F;
    public GameObject Wall_B;
    public GameObject Wall_L;
    public GameObject Wall_R;

    private List<GameObject> doors = new List<GameObject>();

    public Room PrevRoom;
    public RoomType Entrance;

    private RoomGenerator roomGenerator;
    private RoomType roomType;
    bool canSpawn = true;

    public void SetRoomType(RoomType type)
    {
        roomType = type;
        transform.name = "Room " + roomType;
        ConfigureRoomType(roomType);
    }

    private void Start()
    {
        roomGenerator = transform.parent.GetComponent<RoomGenerator>();
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
                ReplaceWallWithDoor(Door, ref Wall_F);
                break;
            case RoomType.B:
                ReplaceWallWithDoor(Door, ref Wall_B);
                break;
            case RoomType.L:
                ReplaceWallWithDoor(Door, ref Wall_L);
                break;
            case RoomType.R:
                ReplaceWallWithDoor(Door, ref Wall_R);
                break;
            case RoomType.FB:
                ReplaceWallWithDoor(Door, ref Wall_F);
                ReplaceWallWithDoor(Door, ref Wall_B);
                break;
            case RoomType.FL:
                ReplaceWallWithDoor(Door, ref Wall_F);
                ReplaceWallWithDoor(Door, ref Wall_L);
                break;
            case RoomType.FR:
                ReplaceWallWithDoor(Door, ref Wall_F);
                ReplaceWallWithDoor(Door, ref Wall_R);
                break;
            case RoomType.BL:
                ReplaceWallWithDoor(Door, ref Wall_B);
                ReplaceWallWithDoor(Door, ref Wall_L);
                break;
            case RoomType.BR:
                ReplaceWallWithDoor(Door, ref Wall_B);
                ReplaceWallWithDoor(Door, ref Wall_R);
                break;
            case RoomType.LR:
                ReplaceWallWithDoor(Door, ref Wall_L);
                ReplaceWallWithDoor(Door, ref Wall_R);
                break;
            default:
                break;
        }
    }

    private void ReplaceWallWithDoor(GameObject door, ref GameObject wall)
    {
        string name = wall.name;
        Vector3 pos = wall.transform.position;
        Quaternion rot = wall.transform.rotation;
        RoomType type = wall.GetComponent<Door>().Type;
        Destroy(wall);
        wall = Instantiate(door, pos, rot, gameObject.transform);
        wall.name = name;
        wall.GetComponent<Door>().Type = type;
        doors.Add(wall);
    }

    public void SpawnNextRoom()
    {
        int countFalse = 0;
        foreach (GameObject door in doors)
        {
            if (!roomGenerator.SpawnNextRoom(door.GetComponent<Door>().Type, this, transform.position))
            {
                countFalse++;
            }
            else
            {
                Entrance = door.GetComponent<Door>().Type;
            }
        }

        canSpawn = false;

        if(countFalse >= 2 && roomGenerator.CanCreateMoreRooms())
        {
            roomGenerator.ReplaceRoom(this);
        }
    }
}
