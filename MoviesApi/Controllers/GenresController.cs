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
    public class GenresController : ControllerBase
    {
       private IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GETAll()
        {
            return Ok(await _genreService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreDto dto)
        {

            var genre=new Genre {Name=dto.Name };
           await _genreService.Add(genre);

            return Ok(genre);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre=await _genreService.GetByID(id);
            if (genre is null)
                return NotFound($"NO Genre was found with ID:{id} ");

            genre.Name = dto.Name;

            _genreService.Update(genre);

            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            var genre = await _genreService.GetByID(id);
            if (genre is null)
                return NotFound($"NO Genre was found with ID:{id} ");

           _genreService.Delete(genre);

            return Ok(genre);

        }




    }
}
