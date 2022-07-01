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
        ListView titlesList;
        int _select;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InstructionsWithFragments);
            
            titlesList = FindViewById<ListView>(Resource.Id.titlesList);
            titlesList.ChoiceMode = ChoiceMode.Single;

            //Get all instructions from the database and populate the view using an adapter.
            instructions = uow.Instructions.GetAll();
            titlesList.Adapter = new InstructionsAdapter(this, instructions);

            titlesList.ItemClick += titlesList_ItemClick;

            //Check if savedInstanceState was loaded before. 
            if (savedInstanceState != null)
            {
                //Get the saved selected_instruction_id and pass its value to _select.
                _select = savedInstanceState.GetInt("selected_instruction_id", 0);
                titlesList.SetSelection(_select);
            }
            else
            {
                //Set selection to index 0.
                _select = 0;
                titlesList.SetSelection(_select);
            }
            titlesList_ItemClick(null, new AdapterView.ItemClickEventArgs(null, null, _select, 0));
        }

        private void titlesList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {   
            //Update _select value with the new position when the user selects an item.
            _select = e.Position;

            //Update the view contents with the selected instruction title and description.
            TextView detailsTitle = FindViewById<TextView>(Resource.Id.detailsMainTitle);
            TextView detailsText = FindViewById<TextView>(Resource.Id.detailsText);
            instructionItem = uow.Instructions.GetById(instructions[e.Position].InstructionID);
            detailsText.Text = instructionItem.InstructionDetail;
            detailsTitle.Text = Resources.GetString(Resource.String.instruction_detail, instructionItem.InstructionHeading);
        }

        //Save the value of _select between the activity states.
        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("selected_instruction_id", _select);
            base.OnSaveInstanceState(outState);
        }
    }
}