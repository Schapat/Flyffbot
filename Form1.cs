using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Magic;
using FlyffBot2;
using System.Threading;


namespace Flyffbot2
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        BlackMagic _flyff = new BlackMagic();

        Hack colHack;
        Hack ranHack;
        Hack speHack;
        Hack speHackBypass;
        Hack speHackBypass1;
        Hack camHack;

        FlyffBot bot;
        PlayerInfo playerInfo;
        TargetInfo offsets;
        Thread thread;

        uint _base;

        //boolean bestimmt on bot getartet oder terminiert wird
        public bool botOn = false;


        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {  
            LoadProcesses();
        }

        private void MetroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
            metroLabel3.Text = metroTrackBar1.Value.ToString();
            if(speHack.OffsetSpe != 0)
            { 
            speHack.SpeedChange(uint.Parse(metroLabel3.Text),speHack.OffsetSpe);
            }
            else speHack.SpeedChange(uint.Parse(metroLabel3.Text));
        }
        private void MetroTrackBar2_Scroll(object sender, ScrollEventArgs e)
        {
            metroLabel7.Text = metroTrackBar2.Value.ToString();
            camHack.CameraViewChange(float.Parse(metroLabel7.Text));
        }

        private void LoadProcesses()
        {
            metroComboBox1.Items.Clear();
            Process[] MyProcess = Process.GetProcessesByName("Neuz");
            for (int i = 0; i < MyProcess.Length; i++)
            metroComboBox1.Items.Add(string.Format("{0}-{1}-{2}", MyProcess[i].ProcessName, MyProcess[i].Id, MyProcess[i].MainWindowTitle));
        }

        private void MetroButton1_Click(object sender, EventArgs e)
        {
       
            metroButton1.Text = "Stop Bot";
            if (!botOn)
            {
                thread = new Thread(delegate () 
                {  
                    bot.startBot(metroLabel2.Text, metroTextBoxSelectTar.Text, offsets);
                });
                thread.Start();
                botOn = true;
            }
            else
            {
                metroButton1.Text = "Start Bot";
                thread.Suspend();
                botOn = false;
            }

        }

        private void MetroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] _processName = metroComboBox1.Text.Split(new char[] { '-' }, 3, StringSplitOptions.RemoveEmptyEntries);
            metroLabel2.Text = _processName[2];
            // _flyff.OpenProcessAndThread(SProcess.GetProcessFromWindowTitle(metroLabel2.Text));
            _flyff.OpenProcessAndThread(int.Parse(_processName[1]));

            if (metroLabel2.Text.Contains("Dreams"))
            {
                _base = (uint)_flyff.MainModule.BaseAddress;

                colHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x5AB9D4);
                ranHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x26ADEA);
                speHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x5C67B8, 0xFDC);
                camHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x5C7A88);

                bot = new FlyffBot(_flyff, _base + 0x5DA438, _base + 0x792F58
                , _base + 0x78E138, _base + 0x5D9800);

                offsets = new TargetInfo(0x160, 0x164, 0x168, 0x69C, 0x6C0, 0x1324);
               

            }
            if (metroLabel2.Text.Contains("Empress"))
            {
                _base = (uint)_flyff.MainModule.BaseAddress;

                colHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x9CE554);
                ranHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x495F97);
                camHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x9F8828);
                //bypassing the checkMax value of speed & change it to 10000;
                speHackBypass = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x49c42B);
                speHackBypass.bypassMaxSpeed();
                speHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x49C430);


                bot = new FlyffBot(_flyff, _base + 0x9FBE5C, _base + 0x1F15768
                , _base + 0x1F10178, _base + 0x9F7D44);

                offsets = new TargetInfo(0x164, 0x168, 0x16C, 0x734, 0x758,0x159C);
            }
            if (metroLabel2.Text.Contains("Clock"))
            {

                _base = (uint)_flyff.MainModule.BaseAddress;

                colHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x9CE554);
                ranHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x495F97);
                camHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x9F8828);
                //bypassing the checkMax value of speed & change it to 10000;
                speHackBypass = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x49c42B);
                speHackBypass.bypassMaxSpeed();
                speHack = new Hack(_flyff, _flyff.MainModule.BaseAddress + 0x49C430);


                bot = new FlyffBot(_flyff, _base + 0xD52CD4, _base + 0xD38c54
                , _base + 0xD38C58, _base + 0xD2B960);
                offsets = new TargetInfo(0x160, 0x164, 0x168, 0x9F4, 0xA68, 0x19D0);
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                    //Player GUI info
                    metroLabelLevel.Text = (_flyff.ReadUInt(_flyff.ReadUInt(bot.Me_addr) + offsets.OffsetLvl)).ToString();
                    metroLabelName.Text = System.Text.Encoding.ASCII.GetString(_flyff.ReadBytes(_flyff.ReadUInt(bot.Me_addr) + offsets.OffsetName, 255)).ToString();
                    metroLabelHP.Text = (_flyff.ReadUInt(_flyff.ReadUInt(bot.Me_addr) + offsets.OffsetHp)).ToString();

                    if (bot.tiBot.Base != 0)
                    {
                        //Target GUI info
                        metroLabel9.Text = bot.tiBot.Lvl.ToString();
                        metroLabel8.Text = bot.tiBot.Name.ToString();
                        metroLabel11.Text = _flyff.ReadInt(bot.tiBot.HpAdr).ToString();
                        metroProgressBar1.Value = SchadenInProzent((int)bot.tiBot.Hp, _flyff.ReadInt(bot.tiBot.HpAdr));
                    }
                    else
                    {
                        metroLabel9.Text = "0";
                        metroLabel8.Text = "no Target";
                        metroLabel11.Text = "0";
                    }
                
            }
            catch { }
            
        }

        public int SchadenInProzent(int buffer, int healthAdr)
        {
            try { 
                float zahl = (100.0f / (float)buffer);
                float rechnung = zahl * healthAdr;
                    return (int)rechnung;
            }
            catch { return 0; }
        }
        public int SchadenInProzentPlayer(int buffer, int healthAdr)
        {
            try
            {
                float zahl = (100.0f / (float)buffer);
                float rechnung = zahl * healthAdr;
                return (int)rechnung;
            }
            catch { return 0; }
        }

        private void MetroProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void MetroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                metroTextBoxSelectTar.Text = System.Text.Encoding.ASCII.GetString(_flyff.ReadBytes(bot.getSelect() + offsets.OffsetName, 255)).ToString();
            }
            catch
            {
                metroTextBoxSelectTar.Text = "choose Target ingame";
            }
        }

        private void MetroLabel8_Click(object sender, EventArgs e)
        {

        }

        private void MetroProgressBar1_Click_1(object sender, EventArgs e)
        {

        }

        private void MetroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle1.Checked)
            {
                ranHack.RangeOn();
            }
            else
            {
                ranHack.RangeOff();
            }
        }

        private void MetroToggle2_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle2.Checked)
            {
                // colHack.CollisionOff();
                colHack.NoCollisionPattern();
            }
            else
            {
                colHack.CollisionOn();
            }
        }

        private void MetroToggle3_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle2.Checked)
            {

            }
            else
            {

            }
        }
    }  
}
