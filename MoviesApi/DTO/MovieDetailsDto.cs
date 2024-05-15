using MoviesApi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTO
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        public string StoreLine { get; set; }
        public byte[] Poster { get; set; }

        public byte GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
