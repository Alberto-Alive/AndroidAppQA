using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrictlyStats
{
    [Activity(Label = "View Dance", Theme = "@style/AppTheme")]
    public class ViewDanceActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        int danceID;
        Dance dance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ViewDance);

            danceID = Intent.GetIntExtra("DanceID", -1);
            dance = uow.Dances.GetById(danceID);
           
            TextView txtDanceViewName = FindViewById<TextView>(Resource.Id.txtDanceViewName);
            txtDanceViewName.Text = dance.DanceName;
            TextView txtDanceViewDifficulty = FindViewById<TextView>(Resource.Id.txtDanceViewDifficulty);
            txtDanceViewDifficulty.Text = dance.DegreeOfDifficulty.ToString();
            TextView txtDanceViewDescription = FindViewById<TextView>(Resource.Id.txtDanceViewDescription);
            txtDanceViewDescription.Text = dance.Description;
            TextView btnDeleteDance = FindViewById<Button>(Resource.Id.btnDeleteDance);
            btnDeleteDance.Click += BtnDeleteDance_Click;
            
           
        }

        private void BtnDeleteDance_Click(object sender, System.EventArgs e)
        {
            uow.Dances.Delete(dance);
            Intent dancesOverviewIntent = new Intent(this, typeof(DancesOverviewActivity));
            Finish();
            StartActivity(dancesOverviewIntent);
        }

        private void BtnEditDance_Click(object sender, System.EventArgs e)
        {
            /*Intent editDanceIntent = new Intent(this, typeof(EditDanceActivity));
            editDanceIntent.PutExtra("DanceID", danceID);
            StartActivity(editDanceIntent);*/
        }
    }
}