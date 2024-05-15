using Microsoft.EntityFrameworkCore;
using MoviesApi.DTO;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MovieService : IMovieService
    {
        private AppDbContext _Context;

        public MovieService(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _Context.Movies.AddAsync(movie);

            _Context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _Context.Remove(movie);
            _Context.SaveChanges();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreid = 0)
        {
           return await _Context.Movies
                .Where(m=>m.GenreId == genreid||genreid==0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _Context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id); ;
        }

        public Movie Update(Movie movie)
        {
            _Context.Update(movie);
            _Context.SaveChanges ();

            return movie;
        }
    }
}
