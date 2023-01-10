using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;


namespace PL.Controllers
{
    public class MovieController : Controller
    {
        // GET: MovieController
        public ActionResult Favorito()
        {
            ML.Movie movie = new ML.Movie();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("account/16835969/favorite/movies?api_key=efdb86e6d01f5440a9dd3ba13c9ac0b4&session_id=ce4751d348f3a41676a18e85d62eb0129ff60554&language=es-Mx&sort_by=created_at.asc&page=1");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    foreach (var resultitem in resultJSON.results)
                    {
                        ML.Movie movieItem = new ML.Movie();
                        movieItem.Titulo = resultitem.original_title;
                        movieItem.Descripcion = resultitem.overview;
                        movieItem.Imagen = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2" + resultitem.poster_path;
                        movie.Movies.Add(movieItem);
                    }
                }
            }
            return View(movie);
        }

        public ActionResult Popular()
        {
            ML.Movie movie = new ML.Movie();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("movie/popular?api_key=efdb86e6d01f5440a9dd3ba13c9ac0b4&language=es-Es&page=1");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    foreach (var resultitem in resultJSON.results)
                    {
                        ML.Movie movieItem = new ML.Movie();
                        movieItem.IdMovie = resultitem.id;
                        movieItem.Titulo = resultitem.original_title;
                        movieItem.Descripcion = resultitem.overview;
                        movieItem.Imagen = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2" + resultitem.poster_path;
                        movie.Movies.Add(movieItem);
                    }
                }
            }
            return View(movie);
        }
        [HttpGet]
        public ActionResult Agregar(int IdMovie, bool favorite)
        {
            ML.Favorita movieFavorite = new ML.Favorita();
            movieFavorite.media_type = "movie";
            movieFavorite.media_id = IdMovie;
            movieFavorite.favorite = favorite;
            ML.Result result = new ML.Result();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                    string responseTask = "account/16835969/favorite?api_key=efdb86e6d01f5440a9dd3ba13c9ac0b4&session_id=ce4751d348f3a41676a18e85d62eb0129ff60554";
                    var postTask = client.PostAsJsonAsync<ML.Favorita>(responseTask, movieFavorite);
                    postTask.Wait();

                    var resultServicio = postTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
              
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
            }
            if (result.Correct)
            {
                return Redirect("Movie");
            }
            else
            {
                return Redirect("PopularMovie");
            }
        }
    }
}
