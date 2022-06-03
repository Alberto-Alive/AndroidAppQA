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
    //[Activity(Label = "CoupleScoresBreakdownDetailsActivity")]
    [Activity(Label = "Couple scores breakdown details", Theme = "@style/AppTheme")]
    public class CoupleScoresBreakdownDetailsActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoupleScoresBreakdownDetails);
            int coupleID = Intent.GetIntExtra("CoupleID", -1);

            Couple couple = uow.Couples.GetById(coupleID);
            IList<Score> scores = uow.Scores.GetScoresForCoupleWithDance(coupleID);
            TextView txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
            TextView txtScoreAverage = FindViewById<TextView>(Resource.Id.txtScoreAverage);

            ListView lstVwCoupleScores = FindViewById<ListView>(Resource.Id.lstVwCoupleScores);

            txtHeading.Text = couple.ToString();
            txtScoreAverage.Text = ($"Score average: {((Decimal)scores.Sum<Score>(s => s.Grade) / scores.Count):0.00}");

            lstVwCoupleScores.Adapter = new CoupleScoresBreakdownDetailsAdapter(this, scores);
            
        }
    }
}