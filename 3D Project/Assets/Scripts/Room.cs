using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject wall_F;
    [SerializeField]
    private GameObject wall_B;
    [SerializeField]
    private GameObject wall_L;
    [SerializeField]
    private GameObject wall_R;

    [HideInInspector]
    public Room PrevRoom;
    [HideInInspector]
    public RoomType Entrance;

    private RoomGenerator roomGenerator;
    private GameObject door;

    private List<GameObject> doors = new List<GameObject>();

    private RoomType roomType;

    bool canSpawn = true;

    private void Awake()
    {
        roomGenerator = RoomGenerator.Instance;
        door = roomGenerator.Door;
    }

    private void Update()
    {
        if (canSpawn)
        {
            SpawnNextRoom();
        }

    }

    public void InitRoom(RoomType type)
    {
        SetRoomType(type);
        SetRoomName();
        ConfigureRoomType(roomType);
    }

    private void SetRoomName()
    {
        transform.name = "Room " + roomType + " #" + roomGenerator.GetRoomCount();
    }

    private void SetRoomType(RoomType type)
    {
        roomType = type;
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
                ReplaceWallWithDoor(ref wall_F);
                break;
            case RoomType.B:
                ReplaceWallWithDoor(ref wall_B);
                break;
            case RoomType.L:
                ReplaceWallWithDoor(ref wall_L);
                break;
            case RoomType.R:
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.FB:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_B);
                break;
            case RoomType.FL:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_L);
                break;
            case RoomType.FR:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.BL:
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_L);
                break;
            case RoomType.BR:
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.LR:
                ReplaceWallWithDoor(ref wall_L);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.FBL:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_L);
                break;
            case RoomType.FBR:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.FLR:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_L);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.BLR:
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_L);
                ReplaceWallWithDoor(ref wall_R);
                break;
            case RoomType.FBLR:
                ReplaceWallWithDoor(ref wall_F);
                ReplaceWallWithDoor(ref wall_B);
                ReplaceWallWithDoor(ref wall_L);
                ReplaceWallWithDoor(ref wall_R);
                break;
            default:
                break;
        }
    }

    private void ReplaceWallWithDoor(ref GameObject wall)
    {
        string name = wall.name;
        Vector3 pos = wall.transform.position;
        Quaternion rot = wall.transform.rotation;
        RoomType type = wall.GetComponent<Wall>().Type;
        Destroy(wall);
        wall = Instantiate(door, pos, rot, gameObject.transform);
        wall.name = name;
        wall.GetComponent<Wall>().Type = type;
        doors.Add(wall);
    }

    public void SpawnNextRoom()
    {
        int countFalse = 0;
        foreach (GameObject currentDoor in doors)
        {
            int output = roomGenerator.SpawnNextRoom(currentDoor.GetComponent<Wall>().Type, this, transform.position);
            if (output != 0)
            {
                countFalse++;
            }
            else
            {
                Entrance = currentDoor.GetComponent<Wall>().Type;
            }
        }

        canSpawn = false;

        if(countFalse >= doors.Count && roomGenerator.CanCreateMoreRooms())
        {
            roomGenerator.ReplaceRoom(this);
        }
    }
}
