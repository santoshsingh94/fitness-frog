﻿using System;
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
            return View();
        }

        [HttpPost]
        public ActionResult Add(DateTime? date,int? activityId, double? duration, 
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
            


            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}