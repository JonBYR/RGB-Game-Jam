using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Room is used to generate terrain for a subsect of each level. In this case this is a 10x8 chunk.
public class Room
{
    public int Id { get; set; } //sets id for tile
    public int Type { get; set; } //states the type of room this will be i.e a corridor, a room to drop down from or a room that you drop in from
    public int X { get; set; }
    public int Y { get; set; }
    public static readonly int RWIDTH = 10;
    public static readonly int RHEIGHT = 8; //constant and will dictate the size of each room
    public struct Tile
    {
        public LevelGen.TileID id;
        public Vector3Int pos; //sets the id which will be the type of tile and the position of the tile
    }
    public Tile[] tiles = new Tile[RWIDTH * RHEIGHT];
    public Room(int id, int x, int y)
    {
        X = x;
        Y = y;
        Id = id;
        Type = 0; //this will be changed later
    }
    public Vector2 centre()
    {
        int offsetX = X * RWIDTH;
        int offsetY = -Y * RHEIGHT; //rememeber that spleunky levels start at the top and progress downwards
        return new Vector2 (offsetX + RWIDTH/2, offsetY + RHEIGHT/2); //centre point for each room is here
    }
    public Vector2 origin()
    {
        int offsetX = X * RWIDTH;
        int offsetY = -Y * RHEIGHT;
        return new Vector2(offsetX, offsetY + RHEIGHT); //point in which room starts
    }
}
