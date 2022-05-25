using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    [Activity(Label = "CoupleScoresBreakdownDetailsActivity")]
    public class CoupleScoresBreakdownDetailsActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.CoupleScoresBreakdownDetails);
            int coupleID = Intent.GetIntExtra("CoupleID", -1);

            Couple couple = uow.Couples.GetById(coupleID);

            TextView txtHeading = FindViewById<TextView>(Resource.Id.txtHeading);
            TextView txtDetail = FindViewById<TextView>(Resource.Id.txtDetail);

            txtHeading.Text = couple.ToString();
            txtDetail.Text = couple.ProfessionalLastName;
        }
    }
}