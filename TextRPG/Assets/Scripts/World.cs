using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class World : MonoBehaviour
    {
        public Room[,] Dungeon { get; set; }
        public Vector2Int Grid = new Vector2Int(10, 10);

        private void Awake()
        {
            Dungeon = new Room[Grid.x,Grid.y];
            //Debug.Log(Grid);
            StartCoroutine(GenerateFloor());
        }

        public IEnumerator  GenerateFloor()
        {
            Debug.Log("Generating floor!");
            for (int x = 0; x < Grid.x; x++)
            {
                for(int y = 0; y < Grid.y; y++)
                {
                    //Debug.LogFormat("Position x: {0} ; Position y: {1}", x, y);
                    Dungeon[x, y] = new Room
                    {
                        RoomIndex = new Vector2(x, y)
                    };                   
                }
            }

            yield return new WaitForEndOfFrame();

            Debug.Log("Adding exit!");
            Vector2Int exitLocation = new Vector2Int(Random.Range(0, Grid.x), Random.Range(0, Grid.y));
            Debug.LogFormat("Exit is at: {0}", exitLocation);
            Dungeon[exitLocation.x, exitLocation.y].Exit = true;
            Dungeon[exitLocation.x, exitLocation.y].Empty = false;
            //Debug.LogFormat("Exit is at: {0}", exitLocation);
            
        }
    }
}