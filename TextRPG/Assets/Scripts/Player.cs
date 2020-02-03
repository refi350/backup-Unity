using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Player : Character
    {
        public int Floor { get; set; }
        public Room Room { get; set; }

        [SerializeField]
        Encounter encounter;
        public World world;

        // Start is called before the first frame update
        void Start()
        {
            Floor = 0;
            Energy = 30;
            Attack = 10;
            Defence = 3;
            Inventory = new List<string>();
            RoomIndex = new Vector2(2, 2);
            this.Room = world.Dungeon[(int)RoomIndex.x, (int)RoomIndex.y];
            this.Room.Empty = true;
            this.Room.Enemy = null;
            UIController.OnPlayerStatChange();
            UIController.OnPlayerInventoryChange();
        }

        public void Move(int direction)
        {
            Debug.Log("Start Moving!");
            Debug.Log(this.Room.Enemy);
            if (this.Room.Enemy)
            {
                return;
            }

            Debug.Log("Moving!");
            if (direction == 0 && RoomIndex.y > 0)
            {
                Journal.Instance.Log("You move north.");
                RoomIndex -= Vector2.up;
            }
            if (direction == 1 && RoomIndex.x < world.Dungeon.GetLength(0) - 1)
            {
                Journal.Instance.Log("You move east.");
                RoomIndex += Vector2.right;
            }
            if (direction == 2 && RoomIndex.y < world.Dungeon.GetLength(1) - 1)
            {
                Journal.Instance.Log("You move south.");
                RoomIndex -= Vector2.down;
            }
            if (direction == 3 && RoomIndex.x > 0)
            {
                Journal.Instance.Log("You move north.");
                RoomIndex += Vector2.left;
            }
            if(this.Room.RoomIndex != RoomIndex)
                Investigate();
        }
        
        public void Investigate()
        {
            this.Room = world.Dungeon[(int)RoomIndex.x, (int)RoomIndex.y];
            encounter.ResetDynamicControls();
            if (this.Room.Empty)
            {
                Journal.Instance.Log("You find yourself in an empty room.");
            }
            else if(this.Room.Chest != null)
            {
                encounter.StartChest();
                Journal.Instance.Log("You've found a chest! What would you like to do?");
            }
            else if(this.Room.Enemy != null)
            {
                Journal.Instance.Log("You are attacked by " + Room.Enemy.Description + "! What you would like to do?");
                encounter.StartCombat();
            }
            else if (this.Room.Exit)
            {
                encounter.StartExit();
                Journal.Instance.Log("You found the exit! Would you like to move to next floor?");
            }
        }

        public void AddItem(string item)
        {
            Journal.Instance.Log("You were given item: " + item);
            Inventory.Add(item);
            UIController.OnPlayerInventoryChange();
            UIController.OnPlayerStatChange();

        }

        public override void TakeDamage(int amount)
        {           
            Debug.Log("Player TakeDamage.");
            base.TakeDamage(amount);
            UIController.OnPlayerStatChange();
        }

        public override void Die()
        {
            Debug.Log("Player died. Game over.");
            base.Die();
        }
    }
}
