using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject Room;

    public int MinRooms = 4, MaxRooms = 10;

    private int selectedNumRooms, currentRoomNum = 0;

    private Room startRoom, endRoom;

    private Vector3[] takenRoomPositions = new Vector3[40];


    void Start()
    {
        FindSelectedNumRooms();
        Debug.Log(selectedNumRooms);
        CreateStartRoom();
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
        Room currentRoom = Instantiate(Room, pos, Quaternion.identity, transform).GetComponent<Room>();
        currentRoom.SetRoomType(type);
        IncreaseRoomCount();
        currentRoom.transform.name = currentRoom.transform.name + " #" + currentRoomNum;
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

    public RoomType NegateRoomType(RoomType type)
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
            RoomType type = NegateRoomType(entrance);
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
        isDeletedRoom = true;
        DecreaseRoomCount();
        SpawnNextRoom(room.PrevRoom.Entrance, room.PrevRoom, room.PrevRoom.transform.position);
        Destroy(room.gameObject);
    }
}
