using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTO;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMapper _mapper;
        private IMovieService _movieService;
        private IGenreService _genreService;

        public MoviesController(IMovieService movieService, IGenreService genreService, IMapper mapper)
        {
            _movieService = movieService;
            _genreService = genreService;
            _mapper = mapper;
        }

        private new List<string> allowedExt = new List<string> { ".jpg", ".png" };
        private long _maxallwedPosterSize = 1048576;
        
        [HttpGet]
        public async Task<IActionResult> GETALL()
        {
            var movies = await _movieService.GetAll();
            //TODO: map Movie to DTO
            var data=_mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return Ok(data);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var m = await _movieService.GetById(id);
            if (m == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(m);
            return Ok(dto);
        }

        [HttpGet("GetByGenreID/{genreid}")]
        public async Task<IActionResult> GetByGenreID(byte genreid)
        {
            var movies = await _movieService.GetAll(genreid);
            //TODO: map Movie to DTO
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return Ok(data);

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MovieDto dto)
        {
            if (!allowedExt.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("only .png and .jpg images are allowed!");

            if (dto.Poster.Length > _maxallwedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB !");

            var isvalidgenre = await _genreService.IsvalidGenre(dto.GenreId);
            if (!isvalidgenre)
                return BadRequest("Invalid Genre ID !");


            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster=datastream.ToArray();

            _movieService.Add(movie);

            return Ok(movie);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] MovieUpdateDto dto)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound($"No Movie was found with ID {id}");
            var isvalidgenre = await _genreService.IsvalidGenre(dto.GenreId);
            if (!isvalidgenre)
                return BadRequest("Invalid Genre ID !");
            if (dto.Poster != null)
            {
                if (!allowedExt.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxallwedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB !");
                using var datastream = new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster = datastream.ToArray();
            }

            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            movie.StoreLine = dto.StoreLine;
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            _movieService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null) return NotFound($"No Movie was found with ID {id}");

            _movieService.Delete(movie);
            return Ok(movie);
        }
    }
}
