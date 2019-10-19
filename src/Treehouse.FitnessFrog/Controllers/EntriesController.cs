using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            //Instantiation of Entry model
            var entry = new Entry()
            {
                Date = DateTime.Today        //Convert.ToDateTime(DateTime.UtcNow.ToString("dd/MM/yyyy").Replace('-','/'))
            };
            SetupActivitiesSelectListItems();       //Populate the activities select list items viewbag property.
            return View(entry);
        }

        
        //[HttpPost]
        /*public ActionResult Add(DateTime? date,int? activityId, double? duration, 
                Entry.IntensityLevel? intensity,bool? exclude, string notes)
        {
            //Getting Inout from the form
            //string date = Request.Form["Date"]; //Or we can simply get into the parameter
            //Parsing Data to its actual type as follow.Like Date can be in DateTime not in string .
            /*DateTime dateValue;
            DateTime.TryParse(date, out dateValue);*/
        //But modelBinder automaticly converts the incoming values to the expected data types. 

        /*ViewBag.Date = date;
        ViewBag.ActivityId = activityId;
        ViewBag.Duration = duration;
        ViewBag.Intensity = intensity;
        ViewBag.Exclude = exclude;
        ViewBag.Notes = notes;*/
        //To handle invalid input we are going to use following code instead of the above code
        /* ViewBag.Date = ModelState["Date"].Value.AttemptedValue;
         ViewBag.ActivityId = ModelState["ActivityId"].Value.AttemptedValue; 
         ViewBag.Duration = ModelState["Duration"].Value.AttemptedValue; 
         ViewBag.Intensity = ModelState["Intensity"].Value.AttemptedValue; 
         ViewBag.Exclude = ModelState["Exclude"].Value.AttemptedValue; 
         ViewBag.Notes = ModelState["Notes"].Value.AttemptedValue; */
        //Since now we are using Html Helper classes so need not to use the above code.
        //THe form automaticly hold the data after the submission.
        //return View();
        //}
        [HttpPost]
        public ActionResult Add(Entry entry)    //Using post method like this we can take the full advantage of our model(Entry).
        {
            //Adding a global validation message
            //ModelState.AddModelError("", "This is global validation message.");

            ValidateEntry(entry);   //Server side validation code.

            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                //Using temp data to display notification
                TempData["Message"] = "Your Entry was successfully added!";

                return RedirectToAction("index");
            }
            SetupActivitiesSelectListItems();
            return View(entry);
        }

        

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get the requested entry from the repository.
            Entry entry = _entriesRepository.GetEntry((int)id);

            //Return a status of "Not Found" if the entry was not found.
            if (entry == null)
                return HttpNotFound();

            SetupActivitiesSelectListItems();

            return View(entry);
        }
        [HttpPost]
        public ActionResult Edit(Entry entry)
        {
            ValidateEntry(entry);
            //If the Entry is valid
            if (ModelState.IsValid)
            {
                //Use the repository to update the entry
                _entriesRepository.UpdateEntry(entry);

                TempData["Message"] = "Your Entry was successfully updated!";

                //Redirect user to the "Entries" List page
                return RedirectToAction("index");
            }
            //Populate the activities select list items viewbag property.
            SetupActivitiesSelectListItems();

            return View(entry);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Retrive entry for the provided id parameter value
            Entry entry = _entriesRepository.GetEntry((int)id);

            //return "Not Found" if an entry was't found
            if (entry == null)
                return HttpNotFound();

            //pass the entry to the view
            return View(entry);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Delete the entry
            _entriesRepository.DeleteEntry(id);

            TempData["Message"] = "Your Entry was successfully deleted!";

            //Redirect user to the entries list page
            return RedirectToAction("Index");           
        }


        //New Methods to DRY(create new method from your set of codes using ctrl+.)
        private void SetupActivitiesSelectListItems()
        {
            //Populate the activities select list items viewbag property.
            ViewBag.ActivitiesSelectListItems = new SelectList(
                Data.Data.Activities, "Id", "Name");
        }

        private void ValidateEntry(Entry entry)
        {
            //If there aren't  any "Duration" field validation errors
            //Then make sure that the duration is greater than "0"
            if (ModelState.IsValidField("Duration") && entry.Duration <= 0)
            {
                ModelState.AddModelError("Duration", "The Duration field value must be greater than '0'. ");
            }
        }

    }
}