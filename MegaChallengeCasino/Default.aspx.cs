using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MegaChallengeCasino
{
    public partial class Default : System.Web.UI.Page
    {
        Random random = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string[] reels = new string[] { "Seven", "Seven", "Seven" };
                displayReel(reels);
                ViewState.Add("PlayersMoney", 100);
                displayCurrentMoney();
            }
        }

        private void displayCurrentMoney()
        {

            moneyLabel.Text = String.Format("Player's Money: {0:C}", ViewState["PlayersMoney"]);
        }

        protected void pullButton_Click(object sender, EventArgs e)
        {
            int bet = 0;
            if (!int.TryParse(betTextBox.Text, out bet)) return;
            pullLever(bet);

            int winnings = pullLever(bet);
            displayResult(bet, winnings);
            adjustPlayerMoney(bet, winnings);
            displayCurrentMoney();
        }

        private void adjustPlayerMoney(int bet, int winnings)
        {
            int playersMoney = int.Parse(ViewState["PlayersMoney"].ToString());
            playersMoney -= bet;
            playersMoney += winnings;
            ViewState["PlayersMoney"] = playersMoney;
        }

        private void displayResult(int bet, int winnings)
        {
            if (winnings > 0)
            {
                resultLabel.Text = String.Format("You bet {0:C} and won {1:C}!", bet, winnings);
            }
            else
            {
                resultLabel.Text = String.Format("Sorry, you lost {0:C}. Better luck next time.", bet);
            }
        }

        private int pullLever(int bet)
        {
            /*
            string image1 = spinReel();
            string image2 = spinReel();
            string image3 = spinReel();
            
            firstReelImage.ImageUrl = "images/" + image1 + ".png";
            secondReelImage.ImageUrl = "images/" + image2 + ".png";
            thirdReelImage.ImageUrl = "images/" + image3 + ".png";
            */
            string[] reels = new string[] { spinReel(), spinReel(), spinReel() };
            displayReel(reels);

            int multiplier = determineMultiplier(reels);
            return bet * multiplier;
        }

        private int determineMultiplier(string [] reels)
        {
            if (isBar(reels)) return 0;
            if (isJackPot(reels)) return 100;
            int multiplier = 0;
            if (isWinner(reels, out multiplier)) return multiplier;
            return 0;
        }

        private bool isWinner(string[] reels, out int multiplier)
        {
            multiplier = determineCherryMultiplier(reels);
            if (multiplier > 0) return true;
            else return false;

        }

        private int determineCherryMultiplier(string[] reels)
        {
            int cherryCount = determineCherryCount(reels);
            if (cherryCount == 1) return 2;
            if (cherryCount == 2) return 3;
            if (cherryCount == 3) return 4;
            return 0;
        }

        private int determineCherryCount(string[] reels)
        {
            int cherryCount = 0;
            if (reels[0] == "Cherry") cherryCount++;
            if (reels[1] == "Cherry") cherryCount++;
            if (reels[2] == "Cherry") cherryCount++;
            return cherryCount;
        }

        private bool isJackPot(string[] reels)
        {
            if (reels[0] == "Seven" && reels[1] == "Seven" && reels[2] == "Seven") return true;
            else return false;
        }

        private bool isBar(string[] reels)
        {
            if (reels[0] == "Bar" || reels[1] == "Bar" || reels[2] == "Bar") return true;
            else return false;
        }

        private void displayReel(string[] reels)
        {
            firstReelImage.ImageUrl = "images/" + reels[0] + ".png";
            secondReelImage.ImageUrl = "images/" + reels[1] + ".png";
            thirdReelImage.ImageUrl = "images/" + reels[2] + ".png";
        }

        private string spinReel()
        {
            string[] reelImage = new string[] { "Bar", "Bell", "Cherry", "Clover", "Diamond", "HorseShoe", "Lemon", "Orange", "Plum", "Seven", "Strawberry", "Watermelon" };
            return reelImage[random.Next(11)];
        }
    }
}