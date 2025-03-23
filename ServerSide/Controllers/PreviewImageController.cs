using Domain.DatabaseModels;
using Domain.DataTransfareObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Repositories;
using ServerSide.Repositories.Class;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreviewImageController : ControllerBase
    {
        private readonly PreviewImageRepository _repository;

        public PreviewImageController(PreviewImageRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _repository.GetAsync(id);
            return result.IsSucceeded ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePreviewImageDto model)
        {
            var result = await _repository.InsertAsync(new() { Base64Url =model.Base64Url});
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePreviewImageDto model)
        {
            var result = await _repository.UpdateAsync(new() { Id = model.Id , Base64Url = model.Base64Url});
            return result.IsSucceeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result.IsSucceeded ? Ok(result) : NotFound(result);
        }
    }
}
