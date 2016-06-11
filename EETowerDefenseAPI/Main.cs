using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;

namespace EETowerDefenseAPI
{
    /// <summary>
    /// Enemies are the attackers. Those are what towers will focus on.
    /// </summary>
    public class Enemy
    {
        public int hp;
        public int blockId;
        public bool TWOxTWO;
        public int blockId1x1;
        public int blockId2x1;
        public int blockId1x2;
        public int blockId2x2;
        /// <summary>
        /// The amount of armour the enemy has until start losing hp
        /// </summary>
        public int armour;
        /// <summary>
        /// Wether the enemy is protected by Metal or not
        /// </summary>
        public bool isMetal;
        /// <summary>
        /// How many blocks the enemy moves every timer tick
        /// </summary>
        public int gridSpeed;

        public Enemy(int hp, int blockId, int gridSpeed, bool isMetal = false, int armour = 0, bool TWOxTWO = false, int blockId1x1 = 1, int blockId2x1 = 1, int blockId1x2 = 1, int blockId2x2 = 1)
        {
            this.hp = hp;
            this.blockId = blockId;
            this.gridSpeed = gridSpeed;
            this.isMetal = isMetal;
            this.armour = armour;
            this.TWOxTWO = TWOxTWO;
            this.blockId1x1 = blockId1x1;
            this.blockId2x1 = blockId2x1;
            this.blockId2x2 = blockId2x2;
            this.blockId1x2 = blockId1x2;
        }

        public void takeDamage(int popPower, int armourPopPowerBonus, bool removeMetal = false)
        {
            if (this.armour > 0)
            {
                if (this.armour - (popPower + armourPopPowerBonus) < 0)
                {
                    this.hp = this.hp - ((popPower  + armourPopPowerBonus) - this.armour);
                    this.armour = 0;
                }
                else
                {
                    this.armour = this.armour - (popPower + armourPopPowerBonus);
                }
            }
            else
            {
                this.hp = this.hp - popPower;
            }
            if (removeMetal == true)
                this.isMetal = false;
        }
    }
    /// <summary>
    /// The player is the one playing.
    /// </summary>
    public class Player
    {
        public int Cash;
        public int Lifes;
        public Loadout towers;

        public Player(int Cash = 100, int Lifes = 1)
        {
            this.Cash = Cash;
            this.Lifes = Lifes;
        }

        public bool Buy(int amount)
        {
            if (amount > this.Cash)
            {
                this.Cash = this.Cash - amount;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RecieveCash(int amount)
        {
            this.Cash = this.Cash + amount;
        }
        public void DefineLoadout(Loadout Loadout)
        {
            this.towers = Loadout;
        }
        public void TakeLifes(int amount)
        {
            this.Lifes = this.Lifes - amount;
        }
        public void AddLifes(int amount)
        {
            this.Lifes = this.Lifes + amount;
        }
    }
    /// <summary>
    /// Modifiers make it easy to make upgrades. Modifiers contain all modifications made to towers while upgraded.
    /// </summary>
    public class Modifiers
    {
        public readonly int rangeIncrement = 0;
        public readonly int popPowerIncrement = 0;
        public readonly int speedIncrement = 0;
        public readonly bool canPopMetal = false;
        public readonly int armourPopPowerBonus = 0;
        
        public Modifiers(int rangeIncrement = 0, int popPowerIncrement = 0, int speedIncrement = 0, bool canPopMetal = false, int armourPopPowerBonus = 0)
        {
            this.rangeIncrement = rangeIncrement;
            this.popPowerIncrement = popPowerIncrement;
            this.speedIncrement = speedIncrement;
            this.canPopMetal = canPopMetal;
            this.armourPopPowerBonus = armourPopPowerBonus;
        }
    }
    /// <summary>
    /// Upgrades are an essencial part of the game. They modify the tower's values.
    /// </summary>
    public class Upgrade
    {
        public readonly string Name;
        public readonly string Description;
        public readonly int rangeIncrement = 0;
        public readonly int popPowerIncrement = 0;
        public readonly int speedIncrement = 0;
        public readonly bool canPopMetal = false;
        public readonly int armourPopPowerBonus = 0;
        public readonly int Cost = 0;

        public Upgrade(string Name, int Cost = 0, string Description = "")
        {
            if (Name == null)
                this.Name = "Null";
            else
                this.Name = Name;

            if (Description == null)
                this.Description = "Null";
            else
                this.Description = Description;

            this.Cost = Cost;
        }
        public Upgrade(string Name, Modifiers modifications, int Cost = 0, string Description = "")
        {
            if (Name == null)
                this.Name = "Null";
            else
                this.Name = Name;

            if (Description == null)
                this.Description = "Null";
            else
                this.Description = Description;

            this.rangeIncrement = modifications.rangeIncrement;
            this.popPowerIncrement = modifications.popPowerIncrement;
            this.speedIncrement = modifications.speedIncrement;
            this.canPopMetal = modifications.canPopMetal;
            this.armourPopPowerBonus = modifications.armourPopPowerBonus;
            this.Cost = Cost;
        }
        public Upgrade(string Name, int Cost = 0, int rangeIncrement = 0, int popPowerIncrement = 0, int speedIncrement = 0, bool canPopMetal = false, int armourPopPowerBonus = 0, string Description = "")
        {
            if (Name == null)
                this.Name = "Null";
            else
                this.Name = Name;

            if (Description == null)
                this.Description = "Null";
            else
                this.Description = Description;

            this.Cost = Cost;

            this.rangeIncrement = rangeIncrement;
            this.popPowerIncrement = popPowerIncrement;
            this.speedIncrement = speedIncrement;
            this.canPopMetal = canPopMetal;
            this.armourPopPowerBonus = armourPopPowerBonus;
        }
    }
    /// <summary>
    /// Towers are the vital key to Tower Defense. Without towers, it would end up as only Defense!
    /// </summary>
    public class Tower
    {
        public readonly string Name;
        public readonly string Description;
        public readonly int Cost;
        public int Range;
        public int PopPower;
        public int Speed;
        public DateTime lastUse;
        public bool CanPopMetal;
        public int ArmourPopPowerBonus;
        public readonly int xStart;
        public readonly int yStart;

        public Tower(string Name, int Range, int PopPower, int Speed, bool CanPopMetal, int ArmourPopPowerBonus, string Description = "", int Cost = 0, int xStart = 0, int yStart = 0)
        {
            if (Name != null)
                this.Name = Name;
            else
                this.Name = "Null";

            if (Description != null)
                this.Description = Description;
            else
                this.Description = "Null";

            this.Range = Range;
            this.PopPower = PopPower;
            this.Speed = Speed;
            this.ArmourPopPowerBonus = ArmourPopPowerBonus;
            this.CanPopMetal = CanPopMetal;

            this.xStart = xStart;
            this.yStart = yStart;

            this.Cost = Cost;
        }
        public Tower(Tower Clone)
        {
            this.Name = Clone.Name;
            this.Description = Clone.Description;
            this.Cost = Clone.Cost;
            this.ArmourPopPowerBonus = Clone.ArmourPopPowerBonus;
            this.CanPopMetal = Clone.CanPopMetal;
            this.Range = Clone.Range;
            this.PopPower = Clone.PopPower;
            this.Speed = Clone.Speed;

            this.xStart = Clone.xStart;
            this.yStart = Clone.yStart;
        }
        public int getLayer(int id)
        {
            if (id >= 500 && id < 1000)
                return 1;
            else
                return 0;
        }
        public void draw(Connection Connection, int b1x1, int b2x1, int b3x1, int b1x2, int b2x2, int b3x2, int b1x3, int b2x3, int b3x3)
        {
            Connection.Send("b", getLayer(b1x1), this.xStart, this.yStart, b1x1);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b2x1), this.xStart + 1, this.yStart, b2x1);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b3x1), this.xStart + 2, this.yStart, b3x1);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b1x2), this.xStart, this.yStart + 1, b1x2);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b2x2), this.xStart + 1, this.yStart + 1, b2x2);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b3x2), this.xStart + 2, this.yStart + 1, b3x2);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b1x3), this.xStart, this.yStart + 2, b1x3);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b2x3), this.xStart + 1, this.yStart + 2, b2x3);
            System.Threading.Thread.Sleep(18);
            Connection.Send("b", getLayer(b3x3), this.xStart + 2, this.yStart + 2, b3x3);
        }
        public bool inRange(int enemyXStart, int enemyYStart)
        {
            if (enemyXStart >= this.xStart-2 || enemyXStart <= this.xStart+2)
            {
                return true;
            }
            else if (enemyYStart >= this.yStart-2 || enemyYStart <= this.yStart+2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void attackEnemy(Enemy enemy)
        {
            if (lastUse==null)
            {
                enemy.takeDamage(this.PopPower, this.ArmourPopPowerBonus, this.CanPopMetal);
                lastUse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            }
            else
            {
                int minute = DateTime.Now.Minute;
                int second = DateTime.Now.Second;
                int ms = DateTime.Now.Millisecond;

                if (minute>lastUse.Minute)
                {
                    int secondBonus = 0;
                    if (second > lastUse.Second)
                        secondBonus = second;
                    else
                        secondBonus = lastUse.Second - second;

                    if (Speed<=(((minute - lastUse.Minute)*60) + secondBonus))
                    {
                        enemy.takeDamage(this.PopPower, this.ArmourPopPowerBonus, this.CanPopMetal);
                        lastUse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                    }
                }
                else if (second > lastUse.Second)
                {
                    if (Speed<=second-lastUse.Second)
                    {
                        enemy.takeDamage(this.PopPower, this.ArmourPopPowerBonus, this.CanPopMetal);
                        lastUse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                    }
                }
                else if (ms > lastUse.Millisecond)
                {
                    if (Speed<=ms*1000)
                    {
                        enemy.takeDamage(this.PopPower, this.ArmourPopPowerBonus, this.CanPopMetal);
                        lastUse = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Loadout permitts you to set, select and modify towers.
    /// </summary>
    public class Loadout
    {
        public readonly List<Tower> Towers;

        public Loadout(List<Tower> Towers)
        {
            this.Towers = Towers;
        }
        public Loadout(Tower[] Towers)
        {
            for (int i = 0; i < Towers.Length; i++)
                this.Towers.Add(Towers[i]);
        }
    }
}
