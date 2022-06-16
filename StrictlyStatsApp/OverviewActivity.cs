using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using Android.Content;
using StrictlyStatsDataLayer;

namespace StrictlyStats
{
    [Activity(Label = "Overview", Theme = "@style/AppTheme")]
    public class OverviewActivity : AppCompatActivity
    {
        Button couplesButton;
        Button dancesButton;
        Button scoresButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Overview);

            couplesButton = FindViewById<Button>(Resource.Id.couplesButton);
            couplesButton.Click += CouplesButton_Click;

            dancesButton = FindViewById<Button>(Resource.Id.dancesButton);
            dancesButton.Click += DancesButton_Click;

            scoresButton = FindViewById<Button>(Resource.Id.scoresButton);
            scoresButton.Click += ScoresButton_Click;
        }

       
        private void CouplesButton_Click(object sender, System.EventArgs e)
        {
            Intent couplesOverviewIntent = new Intent(this, typeof(CouplesOverviewActivity));

            StartActivity(couplesOverviewIntent);
        }

        private void DancesButton_Click(object sender, System.EventArgs e)
        {
            Intent dancesOverviewIntent = new Intent(this, typeof(DancesOverviewActivity));

            StartActivity(dancesOverviewIntent);
        }

        private void ScoresButton_Click(object sender, System.EventArgs e)
        {
            /*Intent selectCoupleIntent = new Intent(this, typeof(SelectCoupleActivity));
            selectCoupleIntent.PutExtra("ActivityType", (int)ActivityType.VoteOff);
            StartActivity(selectCoupleIntent);*/
        }

    }
}