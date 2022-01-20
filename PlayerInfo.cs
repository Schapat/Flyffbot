using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flyffbot2
{
    class PlayerInfo
    {
        private uint _base;  // offset - 0
        private float x, y, z;      // offset - 164
        private float hyp;          // comes from calc
        private uint lvl;  // offset - 6F0
        private uint hp;   // offset - 710
        private String name;

        private uint hpAdr;

        public PlayerInfo()
        {

        }
        public PlayerInfo(uint _base, float x, float y, float z, uint lvl, uint hp, String name )
        {
            this._base = _base;
            this.x = this._base + x;
            this.y = this._base + y;
            this.z = this._base + z;
            this.lvl = this._base + lvl;
            this.hp = this._base + hp;
            this.name = this._base + name;
        }

        public uint Base { get => _base; set => _base = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }
        public float Hyp { get => hyp; set => hyp = value; }
        public uint Lvl { get => lvl; set => lvl = value; }
        public uint Hp { get => hp; set => hp = value; }
        public string Name { get => name; set => name = value; }
        public uint HpAdr { get => hpAdr; set => hpAdr = value; }
    }
}
