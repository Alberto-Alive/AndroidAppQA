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
    [Activity(Label = "Dances overview", Theme = "@style/AppTheme")]
    public class CouplesOverviewActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Couple> couples;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CouplesOverview);
            ListView lstVwCouplesOverview = FindViewById<ListView>(Resource.Id.lstVwCouplesOverview);

            couples = uow.Couples.GetAll();

            lstVwCouplesOverview.Adapter = new CouplesOverviewAdapter(this, couples);
            lstVwCouplesOverview.ItemClick += LstVwCouplesOverview_ItemClick;

            Button addANewBtn = FindViewById<Button>(Resource.Id.btnAddNewCouple);
            addANewBtn.Click += AddANewBtn_Click;
        }

        private void AddANewBtn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditCoupleActivity));
            Finish();
            StartActivity(intent);
        }

        private void LstVwCouplesOverview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(EditCoupleActivity));
            intent.PutExtra("CoupleID", couples[e.Position].CoupleID);
            Finish();
            StartActivity(intent);
        }

    }
}