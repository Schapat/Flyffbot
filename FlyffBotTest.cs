using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Flyffbot2;
using Magic;
using MetroFramework.Controls;

namespace FlyffBot2
{
    class FlyffBot
    {
        BlackMagic _flyff;
        public TargetInfo tiBot;

        private uint _select_addr;
        private uint _maxInView_addr;
        private uint _targetBase_addr;
        private uint _me_addr;

        public uint Select_addr { get => _select_addr; set => _select_addr = value; }
        public uint MaxInView_addr { get => _maxInView_addr; set => _maxInView_addr = value; }
        public uint TargetBase_addr { get => _targetBase_addr; set => _targetBase_addr = value; }
        public uint Me_addr { get => _me_addr; set => _me_addr = value; }

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
        public void select(uint target)
        {
            uint pointed = 0;
            pointed = _flyff.ReadUInt(_select_addr);
            _flyff.WriteUInt(pointed + 0x20, target);
        }

        public uint getSelect()
        {
            try
            {
                uint pointed = 0;
                pointed = _flyff.ReadUInt(_select_addr);
                pointed = _flyff.ReadUInt(pointed + 0x20);
                return pointed;
            }
            catch
            {
                return 0;
            }
        }

        public uint getMe()
        {
            return _flyff.ReadUInt(_me_addr);
        }

        public float getHyp(TargetInfo ti, TargetInfo offsets)
        {
            float x, z;
            x = _flyff.ReadFloat(getMe() + offsets.OffsetX);
            z = _flyff.ReadFloat(getMe() + offsets.OffsetZ);
            return (float)Math.Sqrt((x - ti.X) * (x - ti.X) + (z - ti.Z) * (z - ti.Z));
        }

        public TargetInfo getClosestTargetInView(String name, TargetInfo offsets)
        {
            uint maxInView = 0;
            TargetInfo closest_ti = new TargetInfo();

            maxInView = _flyff.ReadUInt(_maxInView_addr);

            for (uint i = 0x0; i < maxInView; i++)
            {
                uint target = 0;
                uint type = 0;
                uint lvl = 0;
                uint hp = 0;
                String targetName = "";
                try
                {
                    target = _flyff.ReadUInt(_targetBase_addr + i * 0x4);
                    type = _flyff.ReadUInt(target + 0x4);
                    lvl = _flyff.ReadUInt(target + offsets.OffsetLvl);
                    hp = _flyff.ReadUInt(target + offsets.OffsetHp);
                    targetName = System.Text.Encoding.ASCII.GetString(_flyff.ReadBytes(target + offsets.OffsetName, 255)).ToString();
                }
                catch
                {
                    Thread.Sleep(10);
                }
                if (target != getMe() && type == 18 && lvl > 0 && lvl < 10000 && hp > 1 && /*targetName.Contains(name) && */targetName.StartsWith(name))
                {
                    TargetInfo ti = new TargetInfo();

                    ti.X = (_flyff.ReadFloat(target + offsets.OffsetX));
                    ti.Y = (_flyff.ReadFloat(target + offsets.OffsetY));
                    ti.Z = (_flyff.ReadFloat(target + offsets.OffsetZ));
                    ti.Hyp = (getHyp(ti, offsets));

                    if (ti.Hyp < closest_ti.Hyp || closest_ti.Hyp == 0)
                    {
                        ti.Base = target;
                        ti.Lvl = lvl;
                        ti.Hp = hp;
                        ti.Name = System.Text.Encoding.ASCII.GetString(_flyff.ReadBytes(target + offsets.OffsetName, 255)).ToString().Trim();
                        ti.HpAdr = target + offsets.OffsetHp;
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

        


        public void startBot(String nameWindow, String nameTarget, TargetInfo offsets)
        {
            //select(0);
            select(0);
            IntPtr hwnd = FindWindow(null, nameWindow);

            for(; ;Thread.Sleep(20) )
            {
                    //RTB.Text = "...";
                    if (getSelect() == 0)
                    {
                    tiBot = getClosestTargetInView(nameTarget, offsets);

                        Thread.Sleep(10);
                        PostMessage(hwnd, 0x0100, 0x27, 0); ;
                        Thread.Sleep(15);
                        PostMessage(hwnd, 0x0101, 0x27, 0);
                    

                    if (tiBot.Base != 0)
                        { 
                                 /*   _flyff.WriteFloat(_flyff.ReadUInt(Me_addr) + offsets.OffsetX, tiBot.X);
                                    _flyff.WriteFloat(_flyff.ReadUInt(Me_addr) + offsets.OffsetY, tiBot.Y);
                                    _flyff.WriteFloat(_flyff.ReadUInt(Me_addr) + offsets.OffsetZ, tiBot.Z); **/
                                
                                select(tiBot.Base);
                                PostMessage(hwnd, WM_KEYDOWN, 0x70, 0);
                                Thread.Sleep(20);
                                PostMessage(hwnd, WM_KEYUP, 0x70, 0);
                                Thread.Sleep(1000);
                            
                        }
                    }

               // Thread.Sleep(10);
               // PostMessage(hwnd, 0x0100, 0x27, 0); ;
               // Thread.Sleep(15);
               // PostMessage(hwnd, 0x0101, 0x27, 0);

            }
        }
    }
}

    

