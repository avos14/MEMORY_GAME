using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace first_game
{
    public partial class Form1 : Form
    {
        const int TIME_FINISH_EASY = 120;
        const int TIME_FINISH_MEDIUM = 90;
        const int TIME_FINISH_HARD = 60;
        const int NUM_CARDS = 12;
        int ticks;
        int FirstOpenedCard = -1;
        int MatchingsFound = 0;
        enum GameStatus_t
        {
            Ended,
            Running,
            Paused
        }
        enum Cards_t
        {
            A1_Photo,
            A1_Text,
            A2_Photo,
            A2_Text,
            A3_Photo,
            A3_Text,
            A4_Photo,
            A4_Text,
            A5_Photo,
            A5_Text,
            A6_Photo,
            A6_Text
        }
       
        private Cards_t[] CardLocations = new Cards_t[NUM_CARDS];
        enum CardStatus_t
        {
            Open,
            Close
        }
        private GameStatus_t GameStatus;
        private CardStatus_t[] CardStatus = new CardStatus_t[NUM_CARDS];

        enum CardOpeningStatus_t
        {
            BEFORE_OPENING_FIRST_CARD,
            AFTER_OPENING_FIRST_CARD
        }

        private CardOpeningStatus_t CardOpeningStatus;

        private System.Windows.Forms.PictureBox[] pictureBoxes;
        public Form1()
        {
            InitializeComponent();
            GameStatus = GameStatus_t.Ended;
            pictureBoxes = new System.Windows.Forms.PictureBox[12]{
                pictureBox0,
                pictureBox1,
                pictureBox2,
                pictureBox3,
                pictureBox4,
                pictureBox5,
                pictureBox6,
                pictureBox7,
                pictureBox8,
                pictureBox9,
                pictureBox10,
                pictureBox11
            };
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            labelGamePaused.Hide();
                for (int i = 0; i < 12; i++)
                {
                    int x = 200 + (i % 4) * 135;
                    int y = 12 + (int)(i / 4) * 115;
                    pictureBoxes[i].Location = new System.Drawing.Point(x, y);
                    pictureBoxes[i].MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
                    pictureBoxes[i].MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
                    pictureBoxes[i].BringToFront();
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (GameStatus) {
                case GameStatus_t.Ended:
                    //start game:
                    Shuffle();
                    StartTimer();
                    labelGamePaused.Hide();
                    GameStatus = GameStatus_t.Running;
                    CardOpeningStatus = CardOpeningStatus_t.BEFORE_OPENING_FIRST_CARD;
                    label_score.Text = "0";
                    button1.Text = "Pause";
                    MatchingsFound = 0;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    break;
                case GameStatus_t.Paused:
                    // resume game:
                    labelGamePaused.Hide();
                    GameStatus = GameStatus_t.Running;
                    button1.Text = "Pause";
                    break;
                case GameStatus_t.Running:
                    // change game status to pause:
                    labelGamePaused.Text = "Game Paused";
                    labelGamePaused.Show();
                    GameStatus = GameStatus_t.Paused;
                    button1.Text = "Resume";
                    break;
            }
        }
        private void Shuffle()
        {
            for (int i=0; i< NUM_CARDS; i++)
            {
                CardStatus[i] = CardStatus_t.Close;
            }
            var rand = new Random();
            for (int i=0; i<12; i++)
            {
                Boolean is_ok = false;
                while (is_ok == false)
                {
                    CardLocations[i] = (Cards_t)rand.Next(12);
                    is_ok = true;
                    for (int j = 0; j < i; j++)
                    {
                        if (CardLocations[i] == CardLocations[j])
                        {
                            is_ok = false;
                            break;
                        }
                    }
                }
            }
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

            pictureBox0.Image = Properties.Resources.back;
            pictureBox1.Image = Properties.Resources.back;
            pictureBox2.Image = Properties.Resources.back;
            pictureBox3.Image = Properties.Resources.back;
            pictureBox4.Image = Properties.Resources.back;
            pictureBox5.Image = Properties.Resources.back;
            pictureBox6.Image = Properties.Resources.back;
            pictureBox7.Image = Properties.Resources.back;
            pictureBox8.Image = Properties.Resources.back;
            pictureBox9.Image = Properties.Resources.back;
            pictureBox10.Image = Properties.Resources.back;
            pictureBox11.Image = Properties.Resources.back;
        }

        private System.Drawing.Bitmap[] card_images = new System.Drawing.Bitmap[12] {
            Properties.Resources.dog,
            Properties.Resources.dog_text,
            Properties.Resources.dolphin,
            Properties.Resources.dolphin_text,
            Properties.Resources.fox,
            Properties.Resources.fox_text,
            Properties.Resources.giraffe,
            Properties.Resources.giraffe_text,
            Properties.Resources.lion,
            Properties.Resources.lion_text,
            Properties.Resources.tiger,
            Properties.Resources.tiger_text,
        };
        private int time_finish;

        void StartTimer()
        {
            ticks = 0;
            if (radioButton1.Checked)
                time_finish = TIME_FINISH_EASY;
            else if (radioButton2.Checked)
                time_finish = TIME_FINISH_MEDIUM;
            else
                time_finish = TIME_FINISH_HARD;
            label_time.Text = "" + time_finish;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (GameStatus)
            {
                case GameStatus_t.Ended:
                    break;
                case GameStatus_t.Paused:
                    break;
                case GameStatus_t.Running:
                    ticks++;
                    label_time.Text = "" + (time_finish - ticks);
                    if (ticks == time_finish)
                    {
                        timer1.Stop();
                        MessageBox.Show("your time has ran out");
                        GameStatus = GameStatus_t.Ended;
                        radioButton1.Enabled = true;
                        radioButton2.Enabled = true;
                        radioButton3.Enabled = true;
                        button1.Text = "Start";
                        for (int i=0; i<12; i++)
                        {
                            pictureBoxes[i].Image = card_images[(int)CardLocations[i]];
                        }
                    }
                    break;
            }
        }

        private void handle_card_click(int card_clicked)
        {
            if (GameStatus != GameStatus_t.Running)
            {
                MessageBox.Show("Please start the game");
                return;
            }
            // input param: card_clicked - 0-based card number

            if (CardOpeningStatus == CardOpeningStatus_t.BEFORE_OPENING_FIRST_CARD)
            {
                if (CardStatus[card_clicked] == CardStatus_t.Close)
                {
                    // open the card
                    pictureBoxes[card_clicked].Image = card_images[(int)CardLocations[card_clicked]];
                    CardOpeningStatus = CardOpeningStatus_t.AFTER_OPENING_FIRST_CARD;
                    FirstOpenedCard = card_clicked;
                    CardStatus[card_clicked] = CardStatus_t.Open;
                } else
                {
                    MessageBox.Show("ERROR: please click on a closed card");
                }
            } else
            {
                if (CardStatus[card_clicked] == CardStatus_t.Close)
                {
                    // open the card
                    pictureBoxes[card_clicked].Image = card_images[(int)CardLocations[card_clicked]];
                    Cards_t firstCardType = CardLocations[FirstOpenedCard];
                    Cards_t secondCardType = CardLocations[card_clicked];
                    if (((int)firstCardType) / 2 == ((int)secondCardType) / 2) { // user found a match
                        MessageBox.Show("Very Good, You Found a Match!");
                        CardStatus[card_clicked] = CardStatus_t.Open;
                        int prevScore = Int32.Parse(label_score.Text);
                        label_score.Text = "" + (prevScore + 10);
                        MatchingsFound++;
                        CardOpeningStatus = CardOpeningStatus_t.BEFORE_OPENING_FIRST_CARD;
                        if (MatchingsFound == 6)
                        {
                            timer1.Stop();
                            GameStatus = GameStatus_t.Ended;
                            button1.Text = "Start";
                            MessageBox.Show("Congratulations, You finished the game!");
                            labelGamePaused.Show();
                            labelGamePaused.Text = "Game Finished";
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            radioButton3.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("wrong match");
                        pictureBoxes[card_clicked].Image = Properties.Resources.back;
                        pictureBoxes[FirstOpenedCard].Image = Properties.Resources.back;
                        int prevScore = Int32.Parse(label_score.Text);
                        label_score.Text = "" + (prevScore - 5);
                        CardStatus[FirstOpenedCard] = CardStatus_t.Close;
                        CardOpeningStatus = CardOpeningStatus_t.BEFORE_OPENING_FIRST_CARD;
                    }
                }
                else
                {
                    MessageBox.Show("ERROR: please click on a closed card");
                }
            }
        }

        private void pictureBox0_Click(object sender, EventArgs e)
        {
            handle_card_click(0);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            handle_card_click(1);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            handle_card_click(2);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            handle_card_click(3);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            handle_card_click(4);
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            handle_card_click(9);
        }
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            handle_card_click(11);
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            handle_card_click(6);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            handle_card_click(5);
        }
        private void pictureBox10_Click(object sender, EventArgs e)
        {
            handle_card_click(10);
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            handle_card_click(8);
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            handle_card_click(7);
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (GameStatus == GameStatus_t.Running)
            {
                System.Windows.Forms.PictureBox pic = (System.Windows.Forms.PictureBox)sender;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Cursor = Cursors.Hand;
            }
        }
        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.PictureBox pic = (System.Windows.Forms.PictureBox)sender;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.Cursor = Cursors.Default;
        }
    }
}
