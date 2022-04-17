using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject Room;
    public int MinRooms = 4;
    public int MaxRooms = 10;

    private int numRooms;
    private int currentRoomCount = 0;
    private Room startRoom, endRoom, prevRoom;

    private Vector3[] positions = new Vector3[20];

    private LinkedList<RoomType> prevRoomTypes = new LinkedList<RoomType>();


    void Start()
    {
        //Debug.Log(positions.ToString());
        FindNumRooms();
        Debug.Log(numRooms);

        startRoom = prevRoom = CreateRoom(RandomEnum<RoomType>(), Vector3.zero);
        
        //string currentRoomType = startRoom.GetRoomType().ToString();
        //foreach (char type in currentRoomType)
        //{
        //    RoomType roomType = (RoomType)System.Enum.Parse(typeof(RoomType), type.ToString(), true);
            
        //}
        
    }

    private void FindNumRooms()
    {
        numRooms = Random.Range(MinRooms, MaxRooms);
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

    

    private Room CreateRoom(RoomType type, Vector3 pos)
    {
        //positions[(int)pos.x / 30][(int)pos.z / 30] = 1;
        Room currentRoom = Instantiate(Room, pos, Quaternion.identity, transform).GetComponent<Room>();
        currentRoom.SetRoomType(type);
        currentRoomCount++;
        currentRoom.transform.name = currentRoom.transform.name + " #" + currentRoomCount;
        return currentRoom;
    }

    private bool isPosAvailable(Vector3 pos)
    {
        foreach (Vector3 currentPos in positions)
        {
            if(currentPos == pos)
            {
                return false;
            }
        }
        positions[currentRoomCount] = pos;
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

    bool skipFirstFlag = true;
    public void SpawnNextRoom(RoomType entrance, Room parent, Vector3 prevRoomPos)
    {
        if (currentRoomCount < numRooms)
        {
            Vector3 pos = prevRoomPos + GetNextRoomPosition(entrance);
            RoomType type = NegateRoomType(entrance);
            RoomType newType;

            do
            {
                newType = RandomEnum<RoomType>(type);
            } while (newType == type);
            if (isPosAvailable(pos))
            {
                Room currentRoom = CreateRoom(newType, pos);
                currentRoom.PrevRoom = parent;
                prevRoom = currentRoom;
                //currentRoom.Entrance = entrance;
                currentRoom.GetEntrance();

                if (!skipFirstFlag)
                {
                    prevRoomTypes.AddLast(entrance);
                }
                skipFirstFlag = false;

                
            }

            
            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(prevRoomTypes.Last.Previous.Previous.Value);
        }
    }
}
