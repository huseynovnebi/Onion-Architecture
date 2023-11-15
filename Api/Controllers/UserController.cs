using Application.Interfaces;
using Application.Models.DTO.User;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitofwork _unitofwork;
        private readonly IMapper _mapper;

        private readonly IMemoryCache _memorycache;

        public UserController(IUnitofwork unitofwork,IMapper mapper,IMemoryCache memorycache)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
            _memorycache = memorycache;
        }

        [Route("api/[controller]/CreateUser")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReqUserDTO createReq)
        {
          
            User node = _mapper.Map<User>(createReq);

            await _unitofwork.User.Add(node);
            bool issuccess = await _unitofwork.SaveChangesAsync();
            if (!issuccess)
                return StatusCode(500, "Internal Server Error: Entity hasn't been updated.");

            return Created(string.Empty, "Created");
        }

        [Route("api/[controller]/GetAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetReqUserDTO> node;
            if (!_memorycache.TryGetValue("AllUsers", out node))
            {
                List<User> responce = await _unitofwork.User.GetAll();
                node = _mapper.Map<List<GetReqUserDTO>>(responce);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromSeconds(45))
               .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
               .SetPriority(CacheItemPriority.Normal);

                _memorycache.Set("AllUsers", node, cacheEntryOptions);
            }
            Response.ContentType = "application/json";

            return Ok(node);
        }

        [Route("api/[controller]/DeleteUser")]
        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("User with the specified id is not true.");

            User? user = await _unitofwork.User.GetByIdStrictAsync(id);

            if (user == null)
                return NotFound("User with the specified id is not found.");

            _unitofwork.User.Remove(user);
            bool issuccess = await _unitofwork.SaveChangesAsync();

            if (!issuccess)
            {
                return StatusCode(500, "Internal Server Error: Entity hasn't been updated.");
            }

            return Ok("User with the specified id has been deleted.");
        }

        //        Accepted: If the deletion is a long-running operation and is accepted for processing but not completed yet, you can return an Accepted response.This informs the client that the request has been received and is being processed, but the deletion may not be immediate.
        //         return Accepted("Deletion request accepted and in progress.");

        [Route("api/[controller]/UpdateUser")]
        [HttpPut]
        //[HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateUserDTO user) 
        {
            if (id <= 0 || id != user.Id)
            {
                var errorResponse = new
                {
                    Error = "Bad Request",
                    Message = "Id is not valid."
                };

                return BadRequest(errorResponse);
            }
          
            User node = _mapper.Map<User>(user);

              _unitofwork.User.Update(node);

            bool issuccess = await _unitofwork.SaveChangesAsync();
            if (!issuccess)
                return new ObjectResult("Internal Server Error.An unexpected error occurred.")
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            return Ok("User Updated!");
        }
        
    }
}
