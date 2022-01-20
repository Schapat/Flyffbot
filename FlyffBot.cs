using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Magic;
using Flyffbot2;
using MetroFramework.Controls;
using System.Windows.Forms;

namespace FlyffBot2
{
    class FlyffBot
    {
        BlackMagic _flyff;
        private uint _select_addr;
        private uint _maxInView_addr;
        private uint _targetBase_addr;
        private uint _me_addr;

        public FlyffBot()
        {

        }
        public FlyffBot(BlackMagic flyffObject, uint _select_addr, uint _maxInView_addr, uint _targetBase_addr, uint _me_addr)
        {
            _flyff = flyffObject;
            this._select_addr = _select_addr;
            this._maxInView_addr = _maxInView_addr;
            this._targetBase_addr = _targetBase_addr;
            this._me_addr = _me_addr;
        }
        public struct targetInfo
        {
            public uint _base;  // offset - 0
            public float x, y, z;      // offset - 164
            public float hyp;          // comes from calc
            public uint lvl;  // offset - 6F0
            public uint hp;   // offset - 710
            public char name;
        };

        public void select(uint target)
        {
            uint pointed = 0;
            pointed = _flyff.ReadUInt(_select_addr);
            _flyff.WriteUInt(pointed + 0x20, target);
        }

        public uint getSelect()
        {
            uint pointed = 0;
            pointed = _flyff.ReadUInt(_select_addr);
            pointed = _flyff.ReadUInt(pointed + 0x20);
            return pointed;
        }

        public uint getMe()
        {
            return _flyff.ReadUInt(_me_addr);
        }

        public float getHyp(targetInfo ti)
        {
            float x, z;
            x = _flyff.ReadFloat(getMe() + 0x160);
            z = _flyff.ReadFloat(getMe() + 0x168);
            return (float)Math.Sqrt((x - ti.x) * (x - ti.x) + (z - ti.z) * (z - ti.z));
        }

        public targetInfo getClosestTargetInView()
        {
            uint maxInView = 0;
            targetInfo closest_ti = new targetInfo();

            maxInView = _flyff.ReadUInt(_maxInView_addr);

            for (uint i = 0x0; i < maxInView; i++)
            {
                uint target = 0;
                uint type = 0;
                uint lvl = 0;
                uint hp = 0;
                try
                {
                    target = _flyff.ReadUInt(_targetBase_addr + i * 0x4);
                    type = _flyff.ReadUInt(target + 0x4);
                    lvl = _flyff.ReadUInt(target + 0x69C);
                    hp = _flyff.ReadUInt(target + 0x6C0);
                }
                catch
                {
                    Thread.Sleep(10);
                }
                if (target != getMe() && type == 18 && lvl > 0 && lvl < 10000 && hp > 1)
                {
                    targetInfo ti = new targetInfo();

                    ti.x = _flyff.ReadFloat(target + 0x160);
                    ti.y = _flyff.ReadFloat(target + 0x164);
                    ti.z = _flyff.ReadFloat(target + 0x168);

                    ti.hyp = getHyp(ti);

                    if (ti.hyp < closest_ti.hyp || closest_ti.hyp == 0)
                    {
                        ti._base = target;
                        ti.lvl = lvl;
                        ti.hp = hp;

                        closest_ti = ti;
                    }
                }
            }
            return closest_ti;
        }

        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        uint WM_KEYDOWN = 0x0100;
        uint WM_KEYUP = 0x0101;

        public void startBot(String name)
        {
            select(0);
            IntPtr hwnd = FindWindow(null, name);
            for (; ; Thread.Sleep(20))
            {
                //RTB.Text = "...";
                if (getSelect() == 0)
                {
                    targetInfo ti = getClosestTargetInView();

                    if (ti._base != 0)
                    {
                        select(ti._base);
                        Thread.Sleep(20);
                        PostMessage(hwnd, WM_KEYDOWN, 0x70, 0);
                        Thread.Sleep(20);
                        PostMessage(hwnd, WM_KEYUP, 0x70, 0);
                    }
                }
                Thread.Sleep(10);
                PostMessage(hwnd, 0x0100, 0x27, 0); ;
                Thread.Sleep(10);
                PostMessage(hwnd, 0x0101, 0x27, 0);

            }
        }
    }
}

    

