using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class LevelGen : MonoBehaviour
{
    [SerializeField] private int lHeight = 4;
    [SerializeField] private int lWidth = 4;
    Level l;
    public Vector3 spawnPos;
    public enum TileID : uint //these can be the types of tiles that appear in each room
    {
        DIRT,
        STONE,
        ENTERANCE,
        EXIT,
        LADDER,
        ITEM,
        RANDOM,
        BACKGROUND,
        EMPTY
    }
    [SerializeField] TileBase[] tiles;
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap doorMap;
    [SerializeField] private Tilemap items;
    [SerializeField] private Tilemap ladders;
    [SerializeField] private Tilemap backgroundMap; //seperate tilemaps used to populate rooms
    public Tilemap GroundMap { get => groundMap; }

    [System.Serializable]
    public struct Templates
    {
        public Texture2D[] images; //these relate to the image templates that can be used for each room type
    }
    [SerializeField] public Templates[] temps = new Templates[4]; //0,1,2,3 for each room type
    public Dictionary<Color32, TileID> byColor; //on each template, each colour on the template represents a specific tile map
    public PlayerController play;
    private void Awake()
    {
        byColor = new Dictionary<Color32, TileID>()
        {
            [Color.black] = TileID.DIRT,
            [Color.blue] = TileID.STONE,
            [Color.red] = TileID.LADDER,
            [Color.green] = TileID.RANDOM,
            [Color.white] = TileID.EMPTY,
            [Color.clear] = TileID.EMPTY
        };
        play = FindObjectOfType<PlayerController>();
        Generation();
    }
    //public Player play;
    public bool setup;
    public void Generation()
    {
        setup = true;
        var watch = System.Diagnostics.Stopwatch.StartNew(); //keeps track of time for level gen
        ClearTiles();
        Border();
        l = new Level(lWidth, lHeight);
        l.Generate();
        RoomBuilder();
        play.transform.position = spawnPos;
        watch.Stop();
        var elpasedMs = watch.ElapsedMilliseconds;
        setup = false;
    }
    private void ClearTiles()
    {
        groundMap.ClearAllTiles();
        doorMap.ClearAllTiles();
        items.ClearAllTiles();
        ladders.ClearAllTiles(); //clears all relevent tilemaps
    }
    private void Border()
    {
        int w = (lWidth * 10);
        int h = (lHeight * 8); //border is equal to width of rooms * width of level * height of level * height of room
        BoundsInt area = new BoundsInt(0, -h + 8, 0, w, h, 1); //creates level area
        TileBase[] tileArray = new TileBase[w * h]; //create new tilemap of set size
        for(int x = -1; x <= w; x++)
        {
            for(int y = -1; y <= h; y++) //encompases 1 beyond/behind level area
            {
                if (x == -1 || y == -1 || x == w || y == h) groundMap.SetTile(new Vector3Int(x, -y + 8 - 1, 0), tiles[(uint)TileID.DIRT]); //creates a dirt tile for the border
                else tileArray[y * w + x] = tiles[(uint)TileID.BACKGROUND]; //generates background tile
            }
        }
        backgroundMap.SetTilesBlock(area, tileArray);
    }
    void RoomBuilder()
    {
        foreach(Room r in l.Rooms)
        {
            int offsetX = r.X * 10;
            int offsetY = -r.Y * 8;
            Color32[] colors = temps[r.Type].images[Random.Range(0, temps[r.Type].images.Length)].GetPixels32(); //gets colours from image
            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 10; x++)
                {
                    Vector3Int p = new Vector3Int(x + offsetX, y + offsetY, 0); //sets tile position
                    if (byColor.TryGetValue(colors[y * 10 + x], out TileID id)) //try get the color value and retrospective tile id
                    {
                        r.tiles[y * 10 + x].pos = p;
                        r.tiles[y * 10 + x].id = id;
                        if (id == TileID.EMPTY) continue; //this is just a background tile so nothing needed
                        switch (id)
                        {
                            case TileID.RANDOM:
                                if (Random.value <= .25f)
                                    groundMap.SetTile(p, tiles[(uint)id]);
                                else if (Random.value <= .25f)
                                    groundMap.SetTile(p, tiles[(uint)TileID.DIRT]);
                                break;
                            case TileID.LADDER:
                                ladders.SetTile(p, tiles[(uint)id]);
                                break;
                            default:
                                groundMap.SetTile(p, tiles[(uint)id]);
                                break;
                        }
                    }
                    else Debug.Log("ERROR with image");
                }
            }
            PlaceItems(r);
            if (r == l.Enterance) spawnPos = groundMap.GetCellCenterWorld(PlaceEntrance(r));
            else if (r == l.Exit) PlaceExit(r);
        }
    }
    public void PlaceItems(Room r)
    {
        foreach(Room.Tile t in r.tiles)
        {
            var p = t.pos;
            if(groundMap.GetTile(p) == null && groundMap.GetTile(p + Vector3Int.down) != null) //check that there is empty space and a platform below for the item
            {
                if (CheckWalls(p, groundMap) > 2 && Random.value < 0.5f) items.SetTile(p, tiles[(uint)TileID.ITEM]);
                else if (Random.value < 0.2f) items.SetTile(p, tiles[(uint)TileID.ITEM]);
            }
        }
    }
    int CheckWalls(Vector3Int p, Tilemap t)
    {
        int walls = 0;
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if ((i != 0 && j != 0) || (i == 0 && j == 0)) continue; //skips centre and corner tiles
                if (groundMap.GetTile(new Vector3Int(p.x + i, p.y + j, 0)) != null) walls++; //checks if a tile exists around current position
            }
        }
        return walls;
    }
    public Vector3Int PlaceEntrance(Room r)
    {
        Debug.Log("Enterence Called");
        Vector3Int p = RandomDoorPos(r);
        items.SetTile(p, tiles[(uint)TileID.ENTERANCE]); //we do not want this to be an interactable door
        return p;
    }
    public void PlaceExit(Room r)
    {
        Debug.Log("Called");
        Vector3Int pos = RandomDoorPos(r);
        doorMap.SetTile(pos, tiles[(uint)TileID.ENTERANCE]);
    }
    public Vector3Int RandomDoorPos(Room r)
    {
        List<Vector3Int> availablePos = new List<Vector3Int>();
        foreach (Room.Tile t in r.tiles)
        {
            var pos = t.pos;
            if (groundMap.GetTile(pos) == null && groundMap.GetTile(pos + Vector3Int.down) != null && groundMap.GetTile(pos + Vector3Int.up) == null) availablePos.Add(pos);
            //checks for all tiles with empty space at a specific position (as well as empty space above and below

        }
        Vector3Int doorP = availablePos[Random.Range(0, availablePos.Count)];
        return doorP;
    }
    void DrawPath()
    {
        Room previous = null;
        foreach (Room i in l.Path)
        {
            if (previous != null)
            {
                Handles.color = Color.white;
                Handles.DrawDottedLine(i.centre(), previous.centre(), 3);
                Handles.color = Color.magenta;
                Quaternion rot = Quaternion.LookRotation(i.centre() - previous.centre()).normalized;
                Handles.ConeHandleCap(0, (i.centre() + previous.centre()) / 2 + (previous.centre() - i.centre()).normalized, rot, 1f, EventType.Repaint);
            }
            previous = i;
        }
    }
}
