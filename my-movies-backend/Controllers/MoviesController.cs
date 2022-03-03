using Microsoft.AspNetCore.Mvc;
using my_movies_backend.Data;
using my_movies_backend.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace my_movies_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesContext _context;

        public MoviesController(MoviesContext context)
        {
            _context = context;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public string Get()
        {
            var movies = _context.Movies.ToList();

            return System.Text.Json.JsonSerializer.Serialize(movies);
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {
                var movie = _context.Movies.Single(m => m.Id == id);

                return System.Text.Json.JsonSerializer.Serialize(movie);
            }
            catch
            {
                Console.WriteLine("Get request id is incorrect");
                return "{}";
            }
        }

        // POST api/<MoviesController>
        [HttpPost]
        public IActionResult Post([FromBody] JsonElement body)
        {
            //JsonElement titleJsonElement, releaseDateJsonElement;
            //int? releaseDateInt = null;

            //// check if Title is not empty or not white spaces only
            //if (body.TryGetProperty("Title", out titleJsonElement) == false || string.IsNullOrWhiteSpace(titleJsonElement.ToString()))
            //{
            //    return BadRequest(JsonConvert.SerializeObject(new { title = "Title parameter missing", status = 400 }));
            //}

            //// check if provided optional ReleaseDate is in correct format
            //if (body.TryGetProperty("ReleaseDate", out releaseDateJsonElement))
            //{
            //    try
            //    {
            //        releaseDateInt = Convert.ToInt32(releaseDateJsonElement.ToString());
            //    }
            //    catch
            //    {
            //        return BadRequest(JsonConvert.SerializeObject(new { title = "ReleaseDate value is incorrect", status = 400 }));
            //    }
            //}

            string title, releaseDateStr;
            int? releaseDateInt = null;

            // check if Title is not empty or not white spaces only
            if (getValueFromJsonElement(body, "Title", out title) == false)
            {
                return BadRequest(JsonConvert.SerializeObject(new { title = "Title parameter missing", status = 400 }));
            }

            // check if provided optional ReleaseDate is in correct format
            if (getValueFromJsonElement(body, "ReleaseDate", out releaseDateStr))
            {
                try
                {
                    releaseDateInt = Convert.ToInt32(releaseDateStr);
                }
                catch
                {
                    return BadRequest(JsonConvert.SerializeObject(new { title = "ReleaseDate value is incorrect", status = 400 }));
                }
            }

            // add movie to the database
            var movie = new Movie { Title = title, ReleaseDate = releaseDateInt };
            _context.Movies.Add(movie);
            _context.SaveChanges();

            Debug.WriteLine(movie.Id.ToString());
            Debug.WriteLine(title + " " + releaseDateInt.ToString());

            return Ok(System.Text.Json.JsonSerializer.Serialize(movie));
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] JsonElement body)
        {
            string title, releaseDateStr;
            int releaseDateInt;

            try
            {
                var movie = _context.Movies.Single(m => m.Id == id);

                if (getValueFromJsonElement(body, "Title", out title))
                {
                    movie.Title = title;
                }

                if (getValueFromJsonElement(body, "ReleaseDate", out releaseDateStr))
                {
                    try
                    {
                        releaseDateInt = Convert.ToInt32(releaseDateStr);
                        movie.ReleaseDate = releaseDateInt;
                    }
                    catch { }
                }

                _context.SaveChanges();


                return Ok(System.Text.Json.JsonSerializer.Serialize(movie));
            }
            catch
            {
                return BadRequest(JsonConvert.SerializeObject(new { title = "Put request id is incorrect", status = 400 }));
            }
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var movie = _context.Movies.Single(m => m.Id == id);
                _context.Movies.Remove(movie);
                _context.SaveChanges();

                return Ok(System.Text.Json.JsonSerializer.Serialize(movie));
            }
            catch
            {
                return BadRequest(JsonConvert.SerializeObject(new { title = "Delete request id is incorrect", status = 400 }));
            }
        }

        /// <summary>
        /// Helper method to get non-empty value from json.
        /// </summary>
        /// <param name="jsonElement">Json element to get value from</param>
        /// <param name="key">Json property key name</param>
        /// <param name="value">Json property value as string</param>
        /// <returns>True if key exists and is non-empty, false otherwise.</returns>
        private bool getValueFromJsonElement(JsonElement jsonElement, string key, out string? value)
        {
            value = null;
            JsonElement tempJsonElement;

            try
            {
                if (jsonElement.TryGetProperty(key, out tempJsonElement) == false)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            string tempValue = tempJsonElement.ToString();

            if (string.IsNullOrWhiteSpace(tempValue))
            {
                return false;
            }

            value = tempValue;
            return true;
        }
    }
}
