using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RoomGenerator : MonoBehaviour
{
    public static RoomGenerator Instance;

    [Header("References")]
    public GameObject Room;
    public GameObject Wall;
    public GameObject Door;

    [Header("Settings")]
    public int MinRooms = 10;
    public int MaxRooms = 20;

    private int selectedNumRooms, currentRoomNum = 0;

    private Room startRoom, endRoom;

    private Vector3[] takenRoomPositions;

    private NavMeshSurface[] surfaces;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitTakenRoomPositionsArray();
        FindSelectedNumRooms();
        Debug.Log("Selected Number of Rooms " + selectedNumRooms);
        CreateStartRoom();
    }

    private void InitTakenRoomPositionsArray()
    {
        takenRoomPositions = new Vector3[MaxRooms];
    }

    private void CreateStartRoom()
    {
        startRoom = CreateRoom(RandomEnum<RoomType>(), Vector3.zero);
    }

    private void FindSelectedNumRooms()
    {
        selectedNumRooms = Random.Range(MinRooms, MaxRooms);
    }

    public T RandomEnum<T>()
    {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        return values[Random.Range(0, values.Length)];
    }

    public T RandomEnum<T>(RoomType hasType)
    {
        T[] values = (T[])System.Enum.GetValues(typeof(T));
        T currentValue;
        do
        {
            currentValue = values[Random.Range(0, values.Length)];
        } while (!currentValue.ToString().Contains(hasType.ToString()));
        return currentValue;
    }

    public bool CanCreateMoreRooms()
    {
        return currentRoomNum < selectedNumRooms;
    }

    public int GetRoomCount()
    {
        return currentRoomNum;
    }

    private void IncreaseRoomCount()
    {
        currentRoomNum++;
    }

    private void DecreaseRoomCount()
    {
        currentRoomNum--;
    }

    private Room CreateRoom(RoomType type, Vector3 pos)
    {
        IncreaseRoomCount();
        Room currentRoom = Instantiate(Room, pos, Quaternion.identity, transform).GetComponent<Room>();
        currentRoom.InitRoom(type);
        return currentRoom;
    }

    private bool isPosAvailable(Vector3 pos)
    {
        foreach (Vector3 currentPos in takenRoomPositions)
        {
            if(currentPos == pos)
            {
                return false;
            }
        }
        takenRoomPositions[currentRoomNum] = pos;
        return true;
    }

    public RoomType NegateEntranceType(RoomType type)
    {
        switch (type)
        {
            case RoomType.F:
                return RoomType.B;
            case RoomType.B:
                return RoomType.F;
            case RoomType.L:
                return RoomType.R;
            case RoomType.R:
                return RoomType.L;
            default:
                return type;
        }
    }

    private Vector3 GetNextRoomPosition(RoomType type)
    {
        switch (type)
        {
            case RoomType.F:
                return new Vector3(0, 0, 30);
            case RoomType.B:
                return new Vector3(0, 0, -30);
            case RoomType.L:
                return new Vector3(-30, 0, 0);
            case RoomType.R:
                return new Vector3(30, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    bool isDeletedRoom = false;
    public int SpawnNextRoom(RoomType entrance, Room parent, Vector3 prevRoomPos)
    {
        if (CanCreateMoreRooms())
        {
            Vector3 pos = prevRoomPos + GetNextRoomPosition(entrance);
            RoomType type = NegateEntranceType(entrance);
            RoomType newType = type;

            if (currentRoomNum < selectedNumRooms - 1)
            {
                do
                {
                    newType = RandomEnum<RoomType>(type);
                } while (newType == type);
            }
            

            if (isPosAvailable(pos) || isDeletedRoom)
            {
                Room currentRoom = CreateRoom(newType, pos);
                currentRoom.PrevRoom = parent;

                if(currentRoomNum == selectedNumRooms)
                {
                    endRoom = currentRoom;
                    //StartCoroutine(UpdateNavMesh());
                }

                isDeletedRoom = false;

                return 0;
            }    
        }
        else
        {
            return 2;
        }

        return 1;
    }

    public void ReplaceRoom(Room room)
    {
        //Debug.Log("Working on " + room.name);
        isDeletedRoom = true;
        DecreaseRoomCount();
        if (room.PrevRoom)
        {
            SpawnNextRoom(room.PrevRoom.Entrance, room.PrevRoom, room.PrevRoom.transform.position);
        }
        Destroy(room.gameObject);
    }

    IEnumerator UpdateNavMesh()
    {
        yield return new WaitForSeconds(5);
        surfaces = gameObject.GetComponentsInChildren<NavMeshSurface>();
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
