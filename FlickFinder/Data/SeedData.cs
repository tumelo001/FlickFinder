using FlickFinder.Models;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace FlickFinder.Data
{
    public static class SeedData
    {
        public static async void EnsureDataPopulated(IApplicationBuilder app)
        {

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://api.themoviedb.org/3/genre/movie/list?language=en&api_key=ddaf2dd3a28f3f67bbfd39b53f1c066f");

            GenreJSON _genre = new GenreJSON();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                _genre = JsonConvert.DeserializeObject<GenreJSON>(data);  
            }

            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.Database.GetMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Genres.Any())
            {
                if (_genre.genres != null)
                {
                    foreach (var genre in _genre.genres)
                    {
                        context.Genres.Add(new Genre { GenreName = genre.GenreName });
                    }
                }
            }

            context.SaveChanges();  
        }
    }

    public class GenreJSON 
    {
        public Genre[] genres { get; set; } = new Genre[0];
    }

}
