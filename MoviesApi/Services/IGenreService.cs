using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetByID(Byte id);
        Task<bool> IsvalidGenre(Byte id);
        Task<Genre> Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);

    }
}
