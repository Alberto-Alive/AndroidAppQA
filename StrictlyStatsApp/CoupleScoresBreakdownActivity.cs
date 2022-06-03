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

            SetContentView(Resource.Layout.CoupleScoresBreakdown);
            ListView lstVwCouples = FindViewById<ListView>(Resource.Id.lstVwCouples);

            couples = uow.Couples.GetAll();

            lstVwCouples.Adapter = new CoupleScoresBreakdownAdapter(this, couples);
            lstVwCouples.ItemClick += LstVwCouples_ItemClick;
        }
        private void LstVwCouples_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(CoupleScoresBreakdownDetailsActivity));
            intent.PutExtra("CoupleID", couples[e.Position].CoupleID);
            StartActivity(intent);
        }

    }
}