using Application.Interfaces;
using Application.Models.DTO.User;
using Application.Service;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _entityService;

        public UserController(IUserService entityService)
        {
            _entityService = entityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReqUserDTO createReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest((400, "modelisnotvalid"));
            }

            await _entityService.AddAsync(createReq);

            return Created(string.Empty, "created");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetReqUserDTO> responce = await _entityService.GetAllAsync();

            return Ok(responce);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Id is not true");

            await _entityService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateUserDTO user) 
        {
            await _entityService.UpdateAsync(user);
            return Ok();
        }
        
    }
}
