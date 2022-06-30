using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;
using System;
using System.Windows;

namespace StrictlyStats
{
    [Activity(Label = "Edit Couple", Theme = "@style/AppTheme")]
    public class EditCoupleActivity : AppCompatActivity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        Button btnSaveCouple, btnDeleteCouple, btnCancelSaveCouple;
        Spinner weekNumberSpinner;
        TextView coupleTitle;
        EditText editCFName, editCLName, editPFName, editPLName, editVotedOffWeekNumber;
        CheckBox votedOff_check;
        Couple couple = new Couple();
        Boolean areAllFieldsValid = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditCouple);

            btnDeleteCouple = FindViewById<Button>(Resource.Id.btnDeleteCouple);
            coupleTitle = FindViewById<TextView>(Resource.Id.coupleTitle);
        
        //Get CoupleID of the couple selected from the list, otherwise set CoupleID = 0.
        //Set the page title to the name of the selected couple and display the delete button.
        int coupleID = Intent.GetIntExtra("CoupleID", 0);
            if (coupleID > 0)
            {
                couple = uow.Couples.GetById(coupleID);
                var title = couple.ToString();
                coupleTitle.Text = title.Truncate(40);
                btnDeleteCouple.Visibility = ViewStates.Visible;
            }
            else
            {
                btnDeleteCouple.Visibility = ViewStates.Gone;
            }

            //Reference variables to relative objects on EditCouple layout.
            editCFName = FindViewById<EditText>(Resource.Id.editCFName);
            editCLName = FindViewById<EditText>(Resource.Id.editCLName);
            editPFName = FindViewById<EditText>(Resource.Id.editPFName);
            editPLName = FindViewById<EditText>(Resource.Id.editPLName);
            weekNumberSpinner = FindViewById<Spinner>(Resource.Id.weekNumberSpinner);
            votedOff_check = FindViewById<CheckBox>(Resource.Id.votedOff_check);
            editVotedOffWeekNumber = FindViewById<EditText>(Resource.Id.editVotedOffWeekNumber);

            //Set the text value of objects in EditCouple layout to the values of the couple object retrieved from the database.
            editCFName.Text = couple.CelebrityFirstName;
            editCLName.Text = couple.CelebrityLastName;
            editPFName.Text = couple.ProfessionalFirstName;
            editPLName.Text = couple.ProfessionalLastName;
            
            //Set and populate the spinner for the celebrity star rating.
            int[] items = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, items);
            weekNumberSpinner.Adapter = adapter;
            weekNumberSpinner.SetSelection(Convert.ToInt32(couple.CelebrityStarRating) -1);

            //Set checkbox initial status.
            //UPDATE action (check for null or number => if null checkbox is checked and vote off week number field is disabled);
            //INSERT action (initial state assumed checked = couple to be added is still in competition)
            if (couple.VotedOffWeekNumber != null)
            {
                editVotedOffWeekNumber.Text = couple.VotedOffWeekNumber.ToString();
            }
            else
            {
                votedOff_check.Checked = true;
                editVotedOffWeekNumber.Enabled = false;
            }

            //Handle checkbox actions
            votedOff_check.Click += (o, e) => {
                if (votedOff_check.Checked)
                {
                    Toast.MakeText(this, "This couple is still competing!", ToastLength.Short).Show();
                    editVotedOffWeekNumber.Enabled = false;
                }
                else
                {
                    Toast.MakeText(this, "Vote couple off the competition!", ToastLength.Short).Show();
                    editVotedOffWeekNumber.Enabled = true;
                    editVotedOffWeekNumber.RequestFocus();
                    editVotedOffWeekNumber.Text = couple.VotedOffWeekNumber.ToString();
                }
            };

            //Trigger actions for the Save, Delete and Cancel buttons
            btnSaveCouple = FindViewById<Button>(Resource.Id.btnSaveCouple);
            btnSaveCouple.Click += (sender, e) => { BtnSaveCouple_Click(); };

            btnDeleteCouple.Click += (sender, e) => { btnDeleteCouple_Click(); };
            
            btnCancelSaveCouple = FindViewById<Button>(Resource.Id.btnCancelSaveCouple);
            btnCancelSaveCouple.Click += (sender, e) => { btnCancelSaveCouple_Click(); };
        }

        private void BtnSaveCouple_Click()
        {
            //Validate fields before atempting database operations
            areAllFieldsValid = ValidateFields();
            if (areAllFieldsValid)
            {
            //Create couple object using user inputed values
            couple.CelebrityFirstName = editCFName.Text;
            couple.CelebrityLastName = editCLName.Text;
            couple.ProfessionalFirstName = editPFName.Text;
            couple.ProfessionalLastName = editPLName.Text;

            int position = weekNumberSpinner.SelectedItemPosition;
            couple.CelebrityStarRating = Convert.ToInt32(weekNumberSpinner.GetItemAtPosition(position));

            if (votedOff_check.Checked)
                couple.VotedOffWeekNumber = null;
            else
                couple.VotedOffWeekNumber = Convert.ToInt32(editVotedOffWeekNumber.Text);

            var dlgAlert = (new Android.App.AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage("Please confirm saving the following couplee to database: " + couple.ToString());
            dlgAlert.SetTitle("Save Couple?");
            dlgAlert.SetButton("OK", (c, ev) =>
            {
                //Perform update or insert operation by checking if we receive a couple.CoupleID.
                if (couple.CoupleID > 0)
                {
                    uow.Couples.Update(couple);
                } else
                {
                    uow.Couples.Insert(couple);
                }
                Intent intent = new Intent(this, typeof(CouplesOverviewActivity));
                Finish();
                StartActivity(intent);
            });
            dlgAlert.SetButton2("CANCEL", (c, ev) => {
            });
            dlgAlert.Show();
            }
        }
        
        //Delete couple action
        private void btnDeleteCouple_Click()
        {
            var dlgAlert = (new Android.App.AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage("This couple will be delete permanently: " + couple.ToString());
            dlgAlert.SetTitle("Are you sure you want to delete this record?");
            dlgAlert.SetButton("OK", (c, ev) =>
            {
                uow.Couples.Delete(couple);
                Intent intent = new Intent(this, typeof(CouplesOverviewActivity));
                Finish();
                StartActivity(intent);
            });
            dlgAlert.SetButton2("CANCEL", (c, ev) => {
            });
            dlgAlert.Show();
        }

        //Cancel action
        private void btnCancelSaveCouple_Click()
        {
            if(couple.CoupleID > 0)
            {
                var dlgAlert = (new Android.App.AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage("Any changes made will not be applied to this record: " + couple.ToString());
                dlgAlert.SetTitle("Are you sure you want to cancel?");
                dlgAlert.SetButton("OK", (c, ev) =>
                {
                    Intent intent = new Intent(this, typeof(CouplesOverviewActivity));
                    Finish();
                    StartActivity(intent);
                });
                dlgAlert.SetButton2("CANCEL", (c, ev) => {
                });
                dlgAlert.Show();
            }
                
            else
            {
                var dlgAlert = (new Android.App.AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage("Any changes will be lost!");
                dlgAlert.SetTitle("Are you sure you want to cancel?");
                dlgAlert.SetButton("OK", (c, ev) =>
                {
                    Intent intent = new Intent(this, typeof(CouplesOverviewActivity));
                    Finish();
                    StartActivity(intent);
                });
                dlgAlert.SetButton2("CANCEL", (c, ev) => {
                });
                dlgAlert.Show();
            }
             
        }

        //Override back button behaviour to send back on the CouplesOverviewActivity.
        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(CouplesOverviewActivity));
            Finish();
            StartActivity(intent);
        }

        //Validator for all fields.
        private bool ValidateFields()
        {
            if (editCFName.Length() == 0)
            {
                editCFName.RequestFocus();
                editCFName.SetError("This field is required", null);
                return false;
            }
            else if(editCFName.Text.Contains(" "))
            {
                editCFName.RequestFocus();
                editCFName.SetError("This field should contain one word only", null);
                return false;
            }

            if (editCLName.Length() == 0)
            {
                editCLName.RequestFocus();
                editCLName.SetError("This field is required", null);
                return false;
            }
            else if (editCLName.Text.Contains(" "))
            {
                editCLName.RequestFocus();
                editCLName.SetError("This field should contain one word only", null);
                return false;
            }

            if (editPFName.Length() == 0)
            {
                editPFName.RequestFocus();
                editPFName.SetError("This field is required", null);
                return false;
            }
            else if (editPFName.Text.Contains(" "))
            {
                editPFName.RequestFocus();
                editPFName.SetError("This field should contain one word only", null);
                return false;
            }

            if (editPLName.Length() == 0)
            {
                editPLName.RequestFocus();
                editPLName.SetError("This field is required", null);
                return false;
            }
            else if (editPLName.Text.Contains(" "))
            {
                editPLName.RequestFocus();
                editPLName.SetError("This field should contain one word only", null);
                return false;
            }
            if (weekNumberSpinner == null)
            {
                weekNumberSpinner.RequestFocus();
                ((TextView)weekNumberSpinner.GetChildAt(0)).SetError("This field is required", null);
                return false;
            }
            else if (!votedOff_check.Checked && editVotedOffWeekNumber.Length() == 0)
            {
                editVotedOffWeekNumber.RequestFocus();
                editVotedOffWeekNumber.SetError("This field is required", null);
                return false;
            }

            //After all fields are validated return true.
            return true;
        }
    }

}