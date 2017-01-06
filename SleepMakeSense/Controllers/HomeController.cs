﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SleepMakeSense.Models;
using Microsoft.AspNet.Identity;

namespace SleepMakeSense.Controllers
{
    public class HomeController : Controller
    {
        private SleepBetaDataContext Db = new SleepBetaDataContext();

        public ActionResult Index()
        {
            HomePageViewModel viewModel = new HomePageViewModel();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DateTime endStop = DateTime.UtcNow.Date.AddDays(-2);
                viewModel.DiarySetup = false;
                viewModel.TodayDiaryEntry = false;
                string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

                //Getting Table Data
                
                IEnumerable<UserQuestion> userQuestions = from table in Db.UserQuestions
                                    where table.AspNetUserId.Equals(userId)
                                    select table;
                                    
                IEnumerable<DiaryData> lastSynced = from table in Db.DiaryDatas
                                 where table.AspNetUserId.Equals(userId) && table.DateStamp >= endStop
                                 orderby table.DateStamp
                                 select table;
                
                foreach (UserQuestion userQuestion in userQuestions)
                {
                    if (userQuestion.AspNetUserId == userId )
                    {
                        viewModel.DiarySetup = true;
                    }
                }
                
                if (lastSynced.Count() != 0)
                {
                    foreach (DiaryData diaryData in lastSynced)
                    {
                        if (diaryData.AspNetUserId == userId && diaryData.DateStamp == DateTime.UtcNow.Date)
                        {
                            viewModel.TodayDiaryEntry = true;
                        }
                    }
                }
                return View(viewModel);

            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}