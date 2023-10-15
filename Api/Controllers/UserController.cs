using Application.Interfaces;
using Application.Models.DTO.User;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitofwork _unitofwork;
        private readonly IMapper _mapper;

        public UserController(IUnitofwork unitofwork,IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReqUserDTO createReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest((400, "modelisnotvalid"));
            }
            User node = _mapper.Map<User>(createReq);

            await _unitofwork.User.Add(node);
            bool issuccess = await _unitofwork.SaveChangesAsync();
            if (!issuccess)
                throw new Exception("Entity hasn't been updated.");
            return Created(string.Empty, "created");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<User> responce = await _unitofwork.User.GetAll();

            List<GetReqUserDTO> node = _mapper.Map<List<GetReqUserDTO>>(responce);

            return Ok(node);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Id is not true");

            User? user = await _unitofwork.User.GetByIdStrictAsync(id);

            if (user == null)
                throw new Exception("entity is not defined");

             _unitofwork.User.Remove(user);
            bool issuccess = await _unitofwork.SaveChangesAsync();
            if (!issuccess)
                throw new Exception("Entity hasn't been updated.");
            return NoContent();
        }

        [HttpPut("{id}")]
        //[HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateUserDTO user) 
        {
            if (id <= 0)
                return BadRequest("Id is not true");
            User node = _mapper.Map<User>(user);

              _unitofwork.User.Update(node);

            bool issuccess = await _unitofwork.SaveChangesAsync();
            if (!issuccess)
                throw new Exception("Entity hasn't been updated.");
            return Ok();
        }
        
    }
}
