using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flyffbot2
{
    class TargetInfo
    {
        private uint _base;
        private float x, y, z;
        private uint offsetX, offsetY, offsetZ;
        private float hyp;
        private uint lvl;
        private uint offsetLvl;
        private uint hp;
        private uint offsetHp;
        private String name;
        private uint offsetName;


        private uint hpAdr;

        public TargetInfo()
        {

        }
        public TargetInfo(uint offsetX, uint offsetY, uint offsetZ, uint offsetLvl, uint offsetHp, uint offsetName)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.offsetZ = offsetZ;
            this.offsetLvl = offsetLvl;
            this.offsetHp = offsetHp;
            this.offsetName = offsetName;
        }

        public uint Base { get => _base; set => _base = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }
        public uint OffsetX { get => offsetX; set => offsetX = value; }
        public uint OffsetY { get => offsetY; set => offsetY = value; }
        public uint OffsetZ { get => offsetZ; set => offsetZ = value; }
        public float Hyp { get => hyp; set => hyp = value; }
        public uint Lvl { get => lvl; set => lvl = value; }
        public uint OffsetLvl { get => offsetLvl; set => offsetLvl = value; }
        public uint Hp { get => hp; set => hp = value; }
        public uint OffsetHp { get => offsetHp; set => offsetHp = value; }
        public string Name { get => name; set => name = value; }
        public uint OffsetName { get => offsetName; set => offsetName = value; }
        public uint HpAdr { get => hpAdr; set => hpAdr = value; }
    }
}
