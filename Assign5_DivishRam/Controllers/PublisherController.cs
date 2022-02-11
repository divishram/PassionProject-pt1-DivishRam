using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogProject.Models;
using BlogProject.Models.ViewModels;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class PublisherController : Controller
    {
        //Easier to create private variables for data controllers
        private PublisherDataController = gamedatacontroller = new PublisherDataController();

        //Get publisher info
        public ActionResult Index()
        {
            return View();
        }

        //Get: /Publisher/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //attempt to retrieve list of publisher
                IEnumerable<Publishers> Publishers = publisherdatacontroller.ListPublishers(SearchKey);
                return View(Publishers);
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;

            }
        }

        //Get: /Publisher/Error
        //display any errors
        public ActionResult Error()
        {
            return View();
        }

        //Get: /Publisher/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Publisher SelectedPublisher = Controllers.FindPublisher(id);
                return View(SelectedPublisher);
            }
            //Catch and show error
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Get: /Publisher/DeleteConfirm/{id{}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Publisher NewPublisher = Controllers.FindPublisher(id);
                return View(NewPublisher);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Post: /Publisher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)

        {
            try
            {
                Controllers.DeletePublisher(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Get: /Publisher/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Publisher/Create
        [HttpPost]
        public ActionResult Create(string PublisherName, string Founder, string Country, string City)
        {
            try
            {
                Publisher NewPublisher = new Publisher();
                NewPublisher.PublisherName = PublisherName;
                NewPublisher.Founder = Founder;
                NewPublisher.Country = Country;
                NewPublisher.City = City;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Updates Publisher
        //GET: /Publisher/Update/1
        public ActionResult Update(int id)
        {
            try
            {
                Publisher SelectedPublisher = Controllers.FindPublisher(id);
                return View(SelectedPublisher);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

    }

}