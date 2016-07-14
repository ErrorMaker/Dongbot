using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sulakore.Communication;
using Tangine;
using Sulakore.Modules;
using Tangine.Habbo;
using Sulakore.Habbo;
using Sulakore.Protocol;
using hackmydick.Properties;
using System.Media;

namespace hackmydick
{

    [Module("Dongbot ", "ez games, ez life ")]
    [Author("nomakta", HabboName = "hackmydick", Hotel = HHotel.Nl)] //  hackmydick banned FeelsBad


    public partial class Form1 : ExtensionForm
    {
        #region Vars
        private List<GameInfo> GGames = new List<GameInfo>();
        private GameInfo CurrentGame { get; set; }
        private List<Panel> LeftPanels = new List<Panel>();
        private List<Panel> RightPanels = new List<Panel>();
        private List<Panel> HitPanels = new List<Panel>();
        public static List<Point> Coords = new List<Point>();
        public Custom CForm { get; set; }
        private int LastBlueLeftPanel = 0;
        private int LastBlueRightPanel = 0;

        private int LeftID = 0;
        private int RightID = 0;
        private int LeverID = 0;
        private int XDeviation = 0;
        private int YDeviation = 0;

        private int YCoord = 0;
        private int XCoord = 0;

        public bool CustomHit = false;
        private Panel CustomPanel { get; set; }

        private bool Running = false;
        int accuratePositionX, accuratePositionY;
        #endregion


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public enum KeyModifier
        {
            Control = 2
        }

        public async void Headershit()
        {
            var move = Game.GetMessages("814e9490db3f636bf970d72e072d7ef1")[0];
            var click = Game.GetMessages("1d783bdbfb54f51403c1f40d931d3043")[0];
            var furniload = Game.GetMessages("540de3e1e0baf1632ce3107fc99780f4")[0];
            var alert = Game.GetMessages("823973d6a28dcd0da8954c594b62c54b")[0];
            var rotate = Game.GetMessages("1101e72b4882377d9dc313cfa46d6d3d")[0];
            var rank = Game.GetMessages("6a1000d94433c253892c267672fc82f6")[0];

            Settings.Default.FurniMove = Game.GetMessageHeader(move);
            Settings.Default.UseFurni = Game.GetMessageHeader(click);
            Settings.Default.FurniLoad = Game.GetMessageHeader(furniload);
            Settings.Default.Alert = Game.GetMessageHeader(alert);
            Settings.Default.Rotate = Game.GetMessageHeader(rotate);
            Settings.Default.Rank = Game.GetMessageHeader(rank);


            Settings.Default.Save();
            Settings.Default.Reload();

             Triggers.InAttach(Settings.Default.FurniMove, WiredMoveFurniture);
            
            HMessage popup = new HMessage(Settings.Default.Alert);
            popup.WriteString("dong.totaly_unimportant");
            popup.WriteInteger(3);
            popup.WriteString("image");
            popup.WriteString("");
            popup.WriteString("title");
            popup.WriteString($"D O N G B O T");
            popup.WriteString("message");
            popup.WriteString($"<h1><font color=\"#FF0000\">Welcome to Dongbot!</font></h1><br><br>Based off GBOT. If you would like to read more about the 'start' of wired scripting you can read <b>http://mika.host/grabber.pdf</b> </b><br><br>- <i> hackmydick, Solo <br></br><font color=\"#0D7517\">(Special thanks to Mika, Speaqer & Niewiarowski </font>");
            await Connection.SendToClientAsync(popup.ToBytes());

        }

        
        #region Hotkeys Method
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                int id = m.WParam.ToInt32();
                switch (key)
                {
                    case Keys.D1: StartBot(); break;
                    case Keys.D2: StopBot("Action aborted"); break;
                    case Keys.D3: WinBx.Checked = true; break;
                    case Keys.D4: LoseBx.Checked = true; break;
                    case Keys.D5: CustomHitBx.Checked = true; break;
                }
            }
        }

        #endregion

        public Form1()
        {

            Headershit();
            InitializeComponent();
            LogsRTB.AppendText($"[HEADERS] Move furni: {Settings.Default.FurniMove}, Use Furni: {Settings.Default.UseFurni}, Load furni: {Settings.Default.FurniLoad}, Rotate: {Settings.Default.Rotate}, Alert: {Settings.Default.Alert}, Rank: {Settings.Default.Rank} \n");
            LogsRTB.AppendText("[INFO] Loaded!\n");
            HeaderBox.Enabled = false;

            CForm = new Custom(this);
            useTB.Text = Settings.Default.UseFurni.ToString(); // change tb to header
            MoveTB.Text = Settings.Default.FurniMove.ToString(); // change tb to header

            int id = 0;
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D1.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D2.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D3.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D4.GetHashCode());
            RegisterHotKey(Handle, id, (int)KeyModifier.Control, Keys.D5.GetHashCode());

            #region GameRooms
            GameInfo gg1 = new GameInfo("Sirjonasxx-VII Testgrijper");
            gg1.LeftID = 225318692;
            gg1.RightID = 225318691;
            gg1.LeverID = 152028365;
            gg1.XDeviation = 4;
            gg1.YDeviation = 4;
            GGames.Add(gg1);

            GameInfo gg2 = new GameInfo("Pretpark Left");
            gg2.LeftID = 135507063;
            gg2.RightID = 218489627;
            gg2.LeverID = 131507864;
            gg2.XDeviation = 1;
            gg2.YDeviation = 6;
            GGames.Add(gg2);

            GameInfo gg3 = new GameInfo("-Handelaar grijper");
            gg3.LeftID = 130124617;
            gg3.RightID = 242750615;
            gg3.LeverID = 168500991;
            gg3.XDeviation = 2;
            gg3.YDeviation = 10;
            GGames.Add(gg3);

            
            GameInfo gg4 = new GameInfo("Shar & Lau");
            gg4.LeftID = 173936507;
            gg4.RightID = 242509285;
            gg4.LeverID = 246353519;
            gg4.XDeviation = 3;
            gg4.YDeviation = 20;
            GGames.Add(gg4);

      

            foreach (GameInfo g in GGames)
            {
                RoomsBx.Items.Add(g.ToString());
            }

            #endregion

            #region Panels 2 Lists
            LeftPanels.Add(panel2);
            LeftPanels.Add(panel3);
            LeftPanels.Add(panel6);
            LeftPanels.Add(panel5);
            LeftPanels.Add(panel4);
            LeftPanels.Add(panel12);

            RightPanels.Add(panel7);
            RightPanels.Add(panel9);
            RightPanels.Add(panel8);
            RightPanels.Add(panel11);
            RightPanels.Add(panel10);
            RightPanels.Add(panel13);

            HitPanels.Add(panel14);
            HitPanels.Add(panel17);
            HitPanels.Add(panel15);
            HitPanels.Add(panel24);
            HitPanels.Add(panel18);
            HitPanels.Add(panel20);
            HitPanels.Add(panel25);
            HitPanels.Add(panel16);
            HitPanels.Add(panel19);
            HitPanels.Add(panel21);
            HitPanels.Add(panel22);
            HitPanels.Add(panel26);
            HitPanels.Add(panel23);
            HitPanels.Add(panel27);
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void mobamattie()
        {
            SoundPlayer audio = new SoundPlayer(hackmydick.Properties.Resources.MOB); // here WindowsFormsApplication1 is the namespace and Connect is the audio file name
            audio.Play();
        }

        private bool IsHit(int x, int y)
        {
            if (!CustomHit)
                return ((x == 2 && y == 1) || (x == 1 && y == 2) || (x == 1 && y == 4) || (x == 4 && y == 1) || (x == 2 && y == 5) || (x == 5 && y == 2) || (x == 3 && y == 3) || (x == 4 && y == 3) || (x == 3 && y == 4) || (x == 3 && y == 6) || (x == 6 && y == 3) || (x == 4 && y == 4) || (x == 5 && y == 6) || (x == 6 && y == 5));

            else
            {
                return Coords.Contains(new Point(x, y));
            }
        }

        private void sKoreButton1_Click(object sender, EventArgs e)
        {
            Settings.Default.FurniMove = ushort.Parse(MoveTB.Text);
            Settings.Default.UseFurni = ushort.Parse(useTB.Text);

            Settings.Default.Save();
            Settings.Default.Reload();
        }
        private void UpdateColors(string side, int number)
        {
            if (VisBx.Checked)
            {
                if (side == "left")
                {
                    if ((number - 1) < (LeftPanels.Count))
                    {
                        if (LastBlueLeftPanel != 0)
                        {
                            LeftPanels[(LastBlueLeftPanel - 1)].BackColor = Color.DarkSeaGreen;
                        }

                        LeftPanels[(number - 1)].BackColor = Color.DarkOrange;
                        LeftLine.Location = new Point(91, (55 + ((number - 1) * 36)));
                        LastBlueLeftPanel = number;
                    }
                }

                if (side == "right")
                {
                    if ((number - 1) < (RightPanels.Count))
                    {
                        if (LastBlueRightPanel != 0)
                        {
                            RightPanels[(LastBlueRightPanel - 1)].BackColor = Color.DarkSeaGreen;
                        }

                        RightPanels[(number - 1)].BackColor = Color.DarkOrange;
                        RightLine.Location = new Point((110 + ((number - 1) * 36)), 35);
                        LastBlueRightPanel = number;
                    }
                }
            }
        }

        public int PillowCount = 0; 
        public void FurnitureMoved(DataInterceptedEventArgs e) //In here, we get ids of moving pillows
        {
            int id = e.Packet.ReadInteger();
            int x = e.Packet.ReadInteger();
            int y = e.Packet.ReadInteger();
            e.Packet.ReadInteger();
            if (Custom.Pillows.Contains(id))
            {
                Point newp = new Point(x - XDeviation, y - YDeviation);
                Point oldp = new Point(1, 1);
                CForm.UpdatePositions(id, oldp, newp);
                PillowCount++;
                CForm.UpdatePercentage(PillowCount);
            }
        }



        private async void WiredMoveFurniture(DataInterceptedEventArgs e)
        {
            var args = new WiredMoveFurniture(e.Packet);
            int furniRealPosX = args.xPos2;
            int furniRealPosY = args.yPos2;
            int furniID = args.FurniID;
            int furniDeviatedPosX = 0;
            int furniDeviatedPosY = 0;


            if (Custom.Pillows.Contains(furniID))
            {
                Point newp = new Point(furniRealPosX - XDeviation, furniRealPosY - YDeviation);
                Point oldp = new Point(args.xPos1 - XDeviation, args.yPos1 - YDeviation);
                if (newp.X < 7 && newp.Y < 7 && oldp.X < 7 && oldp.Y < 7)
                {
                    CForm.UpdatePositions(furniID, oldp, newp);
                    return;
                }
            }

            if (Running)
            {

                // Special thanks to NSA(niewiarowski) for this method of bypassing the
                // anti-script method in certain grijpers

                if (args.FurniID == LeftID)
                    if (args.xPos2 != XDeviation)
                        accuratePositionY = (args.yPos2 - YDeviation) - (args.xPos2 > XDeviation ? args.xPos2 - XDeviation : XDeviation - args.xPos2);
                    else
                        accuratePositionY = args.yPos2 - YDeviation;
                else if (args.FurniID == RightID)
                    if (args.yPos2 != YDeviation)
                        accuratePositionX = (args.xPos2 - XDeviation) - (args.yPos2 > YDeviation ? args.yPos2 - YDeviation : YDeviation - args.yPos2);
                    else
                        accuratePositionX = args.xPos2 - XDeviation;

                if (furniID == LeftID)
                {
                    furniDeviatedPosY = furniRealPosY - YDeviation;
                    YCoord = furniDeviatedPosY;
                    MainYLbl.Text = accuratePositionY.ToString();
                    UpdateColors("left", accuratePositionY);
                }

                if (furniID == RightID)
                {
                    furniDeviatedPosX = furniRealPosX - XDeviation;
                    XCoord = furniDeviatedPosX;
                    MainXLbl.Text = accuratePositionX.ToString();
                    UpdateColors("right", accuratePositionX);
                }
                if (WinBx.Checked && IsHit(accuratePositionX, accuratePositionY))
                {
                    await Connection.SendToServerAsync(Settings.Default.UseFurni, LeverID, 0);
                    LogsRTB.AppendText("[INFO] Won game\n");
                    accuratePositionX = 0;
                    accuratePositionY = 0;
                    StopBot("Won game");
                }

                if (LoseBx.Checked & !IsHit(accuratePositionX, accuratePositionY))
                {
                    await Connection.SendToServerAsync(Settings.Default.UseFurni, LeverID, 0);
                    LogsRTB.AppendText("[INFO] Lost game\n");
                    XCoord = 0;
                    YCoord = 0;

                    StopBot("Lost game");
                }
            }

        

            await Task.Delay(300);
            accuratePositionX = 0;
            accuratePositionY = 0;
        }

    
        private void StartBot()
        {
            if (HotkeySoundCB.Checked)
            {
                mobamattie();
            }
            if (StartBtn.Enabled)
            {
                if (WinBx.Checked)
                    StatusLbl.Text = "Running: Trying to win";
                else if (LoseBx.Checked)
                    StatusLbl.Text = "Running: Trying to lose";
                
                else StatusLbl.Text = "Running: Trying to hit custom spot";
                Running = true;
            }
        }

        private void StopBot(string reason)
        {
            StatusLbl.Text = $"Stopped: {reason} \n";
            Running = false;
        }

        private void StatusBx_Enter(object sender, EventArgs e)
        {

        }


        private async void CustomBtn_Click(object sender, EventArgs e)
        {
            CForm.Show();
            await Connection.SendToClientAsync(Settings.Default.Rank, 12, 12, true);
            Triggers.OutAttach(Settings.Default.Rotate, FurnitureMoved); //Should update this
        }

        private void CustomHitBx_CheckedChanged(object sender, EventArgs e)
        {
               CustomHit = CustomHitBx.Checked;
    
        }

        private void WinBx_CheckedChanged(object sender, EventArgs e)
        {
            if (WinBx.Checked)
            {
                LoseBx.Checked = false;
                CustomHitBx.Checked = false;
                if (!Running)
                    StatusLbl.Text = "Idle: Ready to win";
                else StatusLbl.Text = "Running: Trying to win";
            }
        }

        private void LoseBx_CheckedChanged(object sender, EventArgs e)
        {
            if(LoseBx.Checked)
            {
                WinBx.Checked = false;
                CustomHitBx.Checked = false;
                if (!Running)
                    StatusLbl.Text = "Idle: Ready to lose";
                else StatusLbl.Text = "Running: Trying to lose";
            } 
        }

        private void RoomsBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentGame = GGames.Find(GrijperGame => GrijperGame.ToString() == RoomsBx.Text);

            LeftID = CurrentGame.LeftID;
            RightID = CurrentGame.RightID;
            LeverID = CurrentGame.LeverID;
            XDeviation = CurrentGame.XDeviation;
            YDeviation = CurrentGame.YDeviation;
        }


        private void StartBtn_Click(object sender, EventArgs e)
        {
            StartBot();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            StopBot("Action aborted");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = OnTopSetting.Checked;
        }

        private void HotkeySoundCB_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
