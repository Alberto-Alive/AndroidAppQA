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
    //[Activity(Label = "CoupleScoresBreakdown")]
    [Activity(Label = "Couple scores breakdown", Theme = "@style/AppTheme")]
    public class CoupleScoresBreakdownActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Couple> couples;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.test1);

            ListView lstVwCouples = FindViewById<ListView>(Resource.Id.titlesListView);
            lstVwCouples.ChoiceMode = ChoiceMode.Single;

            couples = uow.Couples.GetAll();

            lstVwCouples.Adapter = new CoupleScoresBreakdownAdapter(this, couples);
            lstVwCouples.ItemClick += LstVwCouples_ItemClick;
            lstVwCouples.SetSelection(0);
            LstVwCouples_ItemClick(null, new AdapterView.ItemClickEventArgs(null, null, 0, 0));
        }
        private void LstVwCouples_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView quoteTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            TextView avgTextView = FindViewById<TextView>(Resource.Id.avgTextView);
            IList<Score> scores = uow.Scores.GetScoresForCoupleWithDance(couples[e.Position].CoupleID);
            ListView lstVwCoupleScores = FindViewById<ListView>(Resource.Id.lstVwCoupleScores);

            lstVwCoupleScores.Adapter = new CoupleScoresBreakdownDetailsAdapter(this, scores);

            Couple couple = uow.Couples.GetById(couples[e.Position].CoupleID);
            quoteTextView.Text = couple.ToString();
            avgTextView.Text = ($"Score average: {((Decimal)scores.Sum<Score>(s => s.Grade) / scores.Count):0.00}");

        }

    }
}