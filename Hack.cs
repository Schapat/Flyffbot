using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyffBot2;
using Magic;

namespace Flyffbot2
{
    class Hack
    {
        //Construktor
        BlackMagic _flyff;
        private IntPtr _base;
        private uint _offsetSpe;

        private byte[] _buffer;

        public IntPtr Base { get => _base; set => _base = value; }
        public uint OffsetSpe { get => _offsetSpe; set => _offsetSpe = value; }

        public Hack()
        {

        }
        public Hack(BlackMagic _flyff, IntPtr _base)
        {
            this._flyff = _flyff;
            this._base = _base;
        }
        public Hack(BlackMagic _flyff, IntPtr _base, uint _offsetSpe)
        {
            this._flyff = _flyff;
            this._base = _base;
            this._offsetSpe = _offsetSpe;
        }

        public void CollisionOff()
        {
            _flyff.WriteByte((uint)_base,  0x00 );
        }

        public void CollisionOn()
        {
            _flyff.WriteBytes((uint)_base, new byte[] { 0x01 });
        }

        public void RangeOff()
        {
            _flyff.WriteBytes((uint)_base, _buffer); ;
        }

        public void RangeOn()
        {
            _buffer = _flyff.ReadBytes((uint)_base, 6);
            _flyff.WriteBytes((uint)_base, new byte[] { 0x90, 0x90 });
        }

        public void SpeedChange(uint value, uint offset)
        {
            _flyff.WriteUInt(_flyff.ReadUInt((uint)_base) + offset, value);
        }
        public void SpeedChange(uint value)
        {
            _flyff.WriteUInt((uint)_base, value);
        }

        public void CameraViewChange(float value)
        {
            _flyff.WriteFloat((uint)_base, value);
        }

        public void bypassMaxSpeed()
        {
            _flyff.WriteBytes((uint)_base, new byte[] { 0x90, 0x90 });
        }
        public void NoCollisionPattern()
        {
            //patterns für Empress Flyff und FlyForDreams
            //noCollision
            string pattern = "FA 5A 93 76 67 96 64 55 D5 82 64 64 64 64 64 64 64 64";
            string pattern1 = "xxxxxxxxxxxxxxxxxx";

            uint patter = _flyff.FindPattern(pattern, pattern1);

            _flyff.WriteByte(patter + 0x2c, 0x00);
        }

        public void SpeedPattern()
        {
            //patterns für FlyForDreams
            //Range hack
            string pattern = "74 10 57 8B CE E8 4c f5 FF FF 5F 5E 83 c4 34";
            string pattern1 = "x?xxxxxxxxxxxxx";

            uint patter = _flyff.FindPattern(pattern, pattern1);

            _flyff.WriteBytes(patter, new byte[] { 0x90, 0x90 });
        }
    }
}
