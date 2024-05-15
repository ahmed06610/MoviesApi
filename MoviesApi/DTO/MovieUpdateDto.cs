namespace MoviesApi.DTO
{
    public class MovieUpdateDto:MoviebaseDto
    {
        public IFormFile? Poster { get; set; }

    }
}
