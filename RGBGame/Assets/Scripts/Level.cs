using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public Level(int w, int h)
    {
        width = w;
        height = h;
    }
    private int width;
    private int height; //these relate to the dimensions of the level
    private Room[] rooms; //number of rooms
    private HashSet<Room> path; //this is a set path from enterence to exit
    private Room enterance;
    private Room exit; //there needs to be one room that is an enterance and one that is an exit
    private Vector3Int spawnPos; //this will be where the player spawns
    public Room[] Rooms { get => rooms; }
    public HashSet<Room> Path { get => path; }
    public Room Enterance { get => enterance; }
    public Room Exit { get => exit; }
    public Vector3Int SpawnPos { get => spawnPos; set => spawnPos = value; }
    public void Generate()
    {
        Initialize();
        GeneratePath();
    }
    private void Initialize()
    {
        rooms = new Room[width * height]; //creates the total number of rooms for each chunk of the level
        path = new HashSet<Room>();
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                rooms[GetRoomId(x, y)] = new Room(GetRoomId(x, y), x, y); //id of room relates to position of it in level along with x and y position
            }
        }
    }
    private void GeneratePath()
    {
        int start = Random.Range(0, width); //picks a random room position at the top of the level to create the enterance
        int x = start, prevX = start;
        int y = 0, prevY = 0;
        rooms[GetRoomId(x, y)].Type = 1; //defined as a corridor
        enterance = rooms[GetRoomId(x, y)]; //this will be the enterance room
        while(y < height) //generate a set path from enterance and exit, which will be until the bottom floor is reached
        { //new direction picked each time while loop called
            switch (pickDirection()) //picks a direction to generate the next room from
            {
                case Direction.RIGHT:
                    //right condition checked first
                    if (x < width - 1 && rooms[GetRoomId(x + 1, y)].Type == 0) x++; //checks if the next room along is empty 
                    else if (x > 0 && rooms[GetRoomId(x - 1, y)].Type == 0) x--; //conversely if room on the right is not empty (or border reached), check the left
                    else goto case Direction.DOWN; //if neither room is empty (or you have reached the border of the level), then the path must move down
                    rooms[GetRoomId(x, y)].Type = 1; //set as a corridor room on the horizontal level
                    break;
                case Direction.LEFT:
                    //same operation as above, but checks left first
                    if (x > 0 && rooms[GetRoomId(x - 1, y)].Type == 0) x--;  
                    else if (x < width - 1 && rooms[GetRoomId(x + 1, y)].Type == 0) x++; 
                    else goto case Direction.DOWN;
                    rooms[GetRoomId(x, y)].Type = 1;
                    break;
                case Direction.DOWN:
                    //called when randomly allocated by pickDirection() OR when all horizontal rooms have been filled on prior level
                    y++;
                    //check that y condition is not exceeding final floor
                    if (y < height)
                    {
                        rooms[GetRoomId(prevX, prevY)].Type = 2; //room that player falls from
                        rooms[GetRoomId(x, y)].Type = 3; //room that player falls into
                    }
                    else exit = rooms[GetRoomId(x, y - 1)]; //if final floor is reached then create exit room    
                    break;
            }
            path.Add(rooms[GetRoomId(prevX, prevY)]);
            prevX = x;
            prevY = y;
        }
    }
    enum Direction
    {
        UP = 0,
        LEFT = 1,
        RIGHT = 2,
        DOWN = 3 //Up is never called, however all other enums will relate to direction algorithm travels
    }
    Direction pickDirection()
    {
        int choice = Mathf.FloorToInt(Random.value * 4.99f); //random.value returns a float between 0-1. Highest value will therefore be 4
        switch(choice)
        {
            case 0: case 1: return Direction.LEFT;
            case 2: case 3: return Direction.RIGHT; //according to the creator of this algorithm, there is a 40% for going left, 40% for going right and a 20% of going down
            default: return Direction.DOWN;
        }
    }
    private int GetRoomId(int x, int y)
    {
        return y * width + x; //gets specific room position
    }
}
