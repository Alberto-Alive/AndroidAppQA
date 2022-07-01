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
        TextView coupleName;
        TextView avgTextView;
        IList<Score> scores;
        ListView lstVwCoupleScores;
        ListView lstVwCouples;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CouplesScoresBreakdown);

            //Reference variables to relative objects on CouplesScoresBreakdownPage layout.
            coupleName = FindViewById<TextView>(Resource.Id.detailsTitle);
            avgTextView = FindViewById<TextView>(Resource.Id.avgTextView);
            lstVwCoupleScores = FindViewById<ListView>(Resource.Id.lstVwCoupleScores);

            //Reference variable lstVwCouples to the ListView on CouplesScoresBreakdownPage layout and allow one item selection at a time.
            lstVwCouples = FindViewById<ListView>(Resource.Id.lstVwCouples);
            lstVwCouples.ChoiceMode = ChoiceMode.Single;

            //Get all couple objects from the database and populate the ListView reusing the CoupleScoresBreakdownAdapter adapter.
            couples = uow.Couples.GetAll();
            lstVwCouples.Adapter = new CoupleScoresBreakdownAdapter(this, couples);

            //Link click event to LstVwCouples_ItemClick method, set selection on lstVwCouples list to index '0' and send details about selected item 
            lstVwCouples.ItemClick += LstVwCouples_ItemClick;
            lstVwCouples.SetSelection(0);
            LstVwCouples_ItemClick(null, new AdapterView.ItemClickEventArgs(null, null, 0, 0));
        }
        private void LstVwCouples_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Get and display the name of the selected couple
            Couple couple = uow.Couples.GetById(couples[e.Position].CoupleID);
            coupleName.Text = Resources.GetString(Resource.String.couple_details, couple.ToString()) ;

            //Get and display a list of scores for the selected couple
            scores = uow.Scores.GetScoresForCoupleWithDance(couples[e.Position].CoupleID);
            lstVwCoupleScores.Adapter = new CoupleScoresBreakdownDetailsAdapter(this, scores);

            //Calculate and display the average of the scores for the selected couple
            if (scores.Count != 0)
                avgTextView.Text = ($"Score average: {((Decimal)scores.Sum<Score>(s => s.Grade) / scores.Count):0.00}");
            else
                avgTextView.Text = "No scores available for this couple.";

        }

    }
}