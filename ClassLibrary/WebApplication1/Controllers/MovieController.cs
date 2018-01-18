using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class MovieController : ApiController
    {
        // GET api/<controller>
        public List<Movie> Get()
        {
            movieDBConnection db = new movieDBConnection();
            DateTime now = DateTime.Now;
            return db.Movies.Where(a => a.PlayTimes.Any(b => b.play >= now) == true).ToList();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put()
        {
            movieDBConnection db = new movieDBConnection();
            // init Movie
            string st = HttpContext.Current.Request.Params["movie"];
            Movie mv;
            try
            {
                mv = JsonConvert.DeserializeObject<Movie>(st);
                mv.img = "first_img.JPG";
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "חסרים נתונים בסרט");
            }

            // init playTimes -
            List<PlayTime> playTList = new List<PlayTime>();
            int numOfProj = Convert.ToInt32(HttpContext.Current.Request.Params["numOfProj"]);
            for (int i = 0; i< numOfProj; i++)
            {
                PlayTime pt = new PlayTime();
                pt.movie_id = mv.id;
                DateTime temp;
                //check valid date string
                if (!DateTime.TryParse(HttpContext.Current.Request.Params["projection" + i],out temp))
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "זמן הקרנה לא חוקי");
                pt.play = temp;
                pt.total_sits = Convert.ToInt32(HttpContext.Current.Request.Params["sits"]);
                pt.availble_sits = pt.total_sits;

                // check if projection time is valid
                if (!PlayTimeController.is_Valid_projc_time(pt.play, new TimeSpan(0, mv.langth,0).Ticks))
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "זמן ההקרנה המבוקש תפוס");

                playTList.Add(pt);
            }
            mv.PlayTimes = playTList;


            // save on dataBase
            try
            {            
                db.Movies.Add(mv);
                db.SaveChanges();
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "בעיה בגישה למס''ד נתונים \n פעולה לא התבצעה");
            }

            // handle Image
            HttpPostedFile img = HttpContext.Current.Request.Files["img"];
            if (img != null)
            {
                try
                {
                    mv.img = mv.id + Path.GetExtension(img.FileName);
                    db.SaveChanges();

                    // save Image on server
                    img.SaveAs(HttpContext.Current.Server.MapPath("~/images/") + mv.img);
                }
                catch
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "בעיה בשמירת תמונה \n לא נשמרה תמונה");
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, "הסרט נוסף בהצלחה");
        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}