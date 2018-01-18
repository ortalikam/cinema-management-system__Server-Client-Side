using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class PlayTimeController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/PlayTime/{id}")]
        public List<PlayTime> Get(int id)
        {
            movieDBConnection db = new movieDBConnection();
            return db.PlayTimes.Where(x => x.movie_id == id).ToList();
        }

        // prevent movie time Discrepancy
        [HttpGet]

        static public bool is_Valid_projc_time(DateTime projectionTime, long langthTicks)
        {
            movieDBConnection db = new movieDBConnection();

            List<PlayTime> closestTime = db.PlayTimes.OrderBy(t => t.play).ToList();

            if (closestTime.Count == 0)
                return true;

            PlayTime close = closestTime[0];
            long min = Math.Abs((close.play - projectionTime).Ticks);
            foreach(PlayTime x in closestTime)
            {
                if(Math.Abs((x.play - projectionTime).Ticks) < min) {
                    min = Math.Abs((x.play - projectionTime).Ticks);
                    close = x;
                }
            }

            long duration = (close.play < projectionTime) ? new TimeSpan(0, close.Movie.langth, 0).Ticks : langthTicks;

            if (Math.Abs((close.play - projectionTime).Ticks) < duration)
                return false;
            return true;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}