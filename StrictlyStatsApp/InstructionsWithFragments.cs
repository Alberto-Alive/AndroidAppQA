using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Content;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;
using Android.Support.V7.App;

namespace StrictlyStats
{
    [Activity(Label = "Instructions With Fragments", Theme = "@style/AppTheme")]
    public class InstructionsWithFragments : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        List<Instruction> instructions;
        Instruction instructionItem;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InstructionsWithFragments);

            ListView titlesList = FindViewById<ListView>(Resource.Id.titlesList);
            titlesList.ChoiceMode = ChoiceMode.Single;

            instructions = uow.Instructions.GetAll();
            titlesList.Adapter = new InstructionsAdapter(this, instructions);

            titlesList.ItemClick += titlesList_ItemClick;
            titlesList.SetSelection(0);
            titlesList_ItemClick(null, new AdapterView.ItemClickEventArgs(null, null, 0, 0));

        }

        private void titlesList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            TextView detailsTitle = FindViewById<TextView>(Resource.Id.detailsTitle);
            TextView detailsText = FindViewById<TextView>(Resource.Id.detailsText);
            instructionItem = uow.Instructions.GetById(instructions[e.Position].InstructionID);
            detailsText.Text = instructionItem.InstructionDetail;
            detailsTitle.Text = Resources.GetString(Resource.String.couple_details, instructionItem.InstructionHeading);
        }

    }
}