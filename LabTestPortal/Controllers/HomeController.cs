using LabTestPortal.CustomClass;
using LabTestPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabTestPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        public ActionResult Search()
        {
            Search search = new Search
            {
                Criteria = new Person { person_id = 0 },
                State_List = CommonHelper.StateList,
                Results = DbAccessHelper.PersonSearch(new Person
                                                                {
                                                                    person_id = null,
                                                                    first_name = null,
                                                                    last_name = null,
                                                                    state_id = null,
                                                                    gender = null,
                                                                    dob = null
                                                                })
            };

            return View(search);
        }

        public ActionResult PersonUpSertJson(Person person)
        {
            var response = DbAccessHelper.PersonUpsert(person);
            var partialViewHtmlStr = CommonHelper.RenderPartialViewToString(this, "DisplayTemplates/_TableRow", response.ResponseObject.FirstOrDefault() as Person);

            if (person.person_id == 0)
            {
                string trID = Guid.NewGuid().ToString("N");
                partialViewHtmlStr = $"<tr id={trID}>{partialViewHtmlStr}</tr>";
            }

            return Json(new {
                success = response.IsSuccess,
                partialViewHtml = partialViewHtmlStr
            });
        }

        public ActionResult PersonSearchJson(Person person)
        {
            var response = DbAccessHelper.PersonSearch(person);
            return Json(new {
                success = true,
                partialViewHtml = CommonHelper.RenderPartialViewToString(this, "DisplayTemplates/_Table", response.ToList())
            });
        }

        public ActionResult GetPersonEditJson(Person person)
        {
            if (person.person_id > 0)
            {
                var response = DbAccessHelper.PersonSearch(person);

                if (response.Any())
                {
                    return Json(new
                    {
                        success = true,
                        partialViewHtml = CommonHelper.RenderPartialViewToString(this, "EditorTemplates/Person", response.FirstOrDefault())
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Unable to locate this Person!"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    success = true,
                    partialViewHtml = CommonHelper.RenderPartialViewToString(this, "EditorTemplates/Person", person)
                });
            }
           
        }
    }
}