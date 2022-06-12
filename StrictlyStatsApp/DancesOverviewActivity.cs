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
    public class DancesOverviewActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Dance> dances;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DancesOverview);
            ListView lstVwDancesOverview = FindViewById<ListView>(Resource.Id.lstVwDancesOverview);

            dances = uow.Dances.GetAll();

            lstVwDancesOverview.Adapter = new DancesOverviewAdapter(this, dances);
            lstVwDancesOverview.ItemClick += LstVwDancesOverview_ItemClick;

            Button addANewBtn = FindViewById<Button>(Resource.Id.btnAddNewDance);
            addANewBtn.Click += AddANewBtn_Click;
        }

        private void AddANewBtn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddDanceActivity));
            StartActivity(intent);
        }

        private void LstVwDancesOverview_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Intent intent = new Intent(this, typeof(ViewDanceActivity));
            Intent intent = new Intent(this, typeof(AddDanceActivity));
            intent.PutExtra("DanceID", dances[e.Position].DanceID);
            StartActivity(intent);
        }

    }
}