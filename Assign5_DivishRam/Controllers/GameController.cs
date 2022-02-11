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
    public class GameController : Controller
    {
        //Easier to create private variables for data controllers
        private GameDataController = gamedatacontroller = new GameDataController();

        //Get game article reviews
        public ActionResult Index()
        {
            return View();
        }

        //Get: /Article/List
        public ActionResult List(string SearchKey = null)
        {
            try
            {
                //attempt to retrieve list of game article reviews
                IEnumerable<Articles> Articles = gamedatacontroller.ListArticles(SearchKey);
                return View(Articles);
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;

            }
        }

        //Get: /Article/Error
        //display any errors
        public ActionResult Error()
        {
            return View();
        }

        //Get: /Article/{id}
        public ActionResult Show(int id)
        {
            try
            {
                Article SelectedArticle = Controllers.FindArticle(id);
                return View(SelectedArticle);
            }
            //Catch and show error
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Get: /Article/DeleteConfirm/{id{}
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                Article NewArticle = Controllers.FindArticle(id);
                return View(NewArticle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Post: /Article/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)

        {
            try
            {
                Controllers.DeleteArticle(id);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Get: /Article/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Article/Create
        [HttpPost]
        public ActionResult Create(string Title, int Rating)
        {
            try
            {
                Article NewArticle = new Article();
                NewArticle.Title = Title;
                NewArticle.Rating = Rating;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        //Updates Article
        //GET: /Article/Update/1
        public ActionResult Update(int id)
        {
            try
            {
                Article SelectedArticle = Controllers.FindArticle(id);
                return View(SelectedArticle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                TempData["ErrorMessage"] = ex.Message;
            }
        }





    }

}