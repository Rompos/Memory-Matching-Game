using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matching_Game
{
    public partial class MatchingGame_Form : Form
    {

        bool allowClick = false;
        PictureBox firstGuess;
        Random rnd = new Random();
        Timer clickTimer = new Timer();
        int time = 120;
        Timer timer = new Timer { Interval = 1000 };




        public MatchingGame_Form()
        {
            InitializeComponent();
        }

        private PictureBox[] pictureBoxes
        {
            get {return  Controls.OfType<PictureBox>().ToArray(); }
        }

        private static IEnumerable<Image> images
        {
            get {
                return new Image[]
                {
                Properties.Resources.emoticon1,
                Properties.Resources.emoticon2,
                Properties.Resources.emoticon3,
                Properties.Resources.emoticon4,
                Properties.Resources.emoticon5,
                Properties.Resources.emoticon6,
                Properties.Resources.emoticon7,
                Properties.Resources.emoticon8,
                Properties.Resources.emoticon9,
                Properties.Resources.emoticon10,
                Properties.Resources.emoticon11,
                Properties.Resources.emoticon12,
                Properties.Resources.emoticon13,
                Properties.Resources.emoticon14,
                Properties.Resources.emoticon15,
                Properties.Resources.emoticon16,

                };
            }
        }


        private void startGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time <= 0)
                {
                    timer.Stop();//if you ran out of time stop timer and show the message below
                    DialogResult dialogResult = MessageBox.Show("Out of Time!!\nPlease try again!","Game Over ",MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ResetImages();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        Application.Exit();
                    }
                    //ResetImages();
                }

                var ssTime = TimeSpan.FromSeconds(time);
                lblTime.Text =  ssTime.ToString();
                //lblTime.Text = "00:" + time.ToString();
            };
        }

        

        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }

            HideImages();
            setRandomImages();
            
            time = 60;
            timer.Start();
        }

        private void HideImages()//hides all the pictures with a question mark image or my photo
        {
            foreach(var pic in pictureBoxes)
            {
                //pic.Image = Properties.Resources.question;
                pic.Image = Properties.Resources.pixel_art;
            }
        }

        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }

        private void setRandomImages()
        {
            foreach(var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        private void CLICKTIMER_TICK(object sender, EventArgs e)//gives a time lag between each selection of pairs
        {
            HideImages();

            allowClick = true;
            clickTimer.Stop();
        }

        private void clickImage( object sender , EventArgs e)
        {
            if (!allowClick)
                return;

            var pic = (PictureBox)sender;//in this line we creating a local variable ,which will only be used inside this function called pic
            //this variable will identify which picture box was clicked or where this event came from

            if(firstGuess == null)//if the first guess is null or empty then wea are going to allow the pic variable to become our first guess,since the pic variable ia a type of picture box .....
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }/*.... it has the same properties as one , therefor we can use the pic.Image property to set any image to it.
            In the later line we are setting an image to the pic variable using (Imagepic.Tag value.Fianlly , we are returning to the program.*/

            pic.Image = (Image)pic.Tag;//when the images are found they will set the appropriate tag to the picture box. 

            if(pic.Image == firstGuess.Image && pic != firstGuess)//checks to see if we found the same pair
            {
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                HideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();//allows the selection of each pair
            }

            firstGuess = null;
            if (pictureBoxes.Any(pictureboxes => pictureboxes.Visible))//this line is checkingto see if there is any visible picture boxes left on the screen.If there is we continue to play the game if not then we run the Message line below it
                return;
            timer.Stop();//if you win stop the timer and show the message below
            DialogResult dialogResult = MessageBox.Show("You win !!!\nDo you want to Try Again!","Victory",MessageBoxButtons.YesNo);
            
            if (dialogResult == DialogResult.Yes)
            {
                ResetImages();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
            //ResetImages();
            
        }

        private void startGame(object sender, EventArgs e)
        {
            allowClick = true;
            setRandomImages();
            HideImages();
            startGameTimer();
            clickTimer.Interval = 1500;
            clickTimer.Tick += CLICKTIMER_TICK;
            button1.Enabled = false;//after the first start disable the start button
        }

        private void Exit_Btn(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
