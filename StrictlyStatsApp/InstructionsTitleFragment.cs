using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using StrictlyStats;

namespace StrictlyStatsApp
{
    public class InstructionsTitleFragment : Fragment
    {

        public static InstructionsTitleFragment NewInstance()
        {
            var bundle = new Bundle();
            return new InstructionsTitleFragment();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.InstructionsTitleFragment, null);

            //return base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }
    }
}