using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTO
{
    public class MovieDto:MoviebaseDto
    {
       
        public IFormFile Poster { get; set; }

    }
}
