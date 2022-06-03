using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    //[Activity(Label = "We Have a Winner!")]
    [Activity(Label = "We have a winner!", Theme = "@style/AppTheme")]
    public class WinnerActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Winner);

            int winningCoupleID = Intent.GetIntExtra("WinningCoupleID", -1);

            Couple couple = uow.Couples.GetById(winningCoupleID);

            TextView winnerTextView = FindViewById<TextView>(Resource.Id.winnerTextView);
            Button okButton = FindViewById<Button>(Resource.Id.okButton);

            winnerTextView.Text = $"The winning couple is {couple}. CONGRATULATIONS!!";

            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}