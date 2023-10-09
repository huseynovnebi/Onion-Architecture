using Application.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> AddAsync(CreateReqUserDTO requestDTO);

        Task<List<GetReqUserDTO>> GetAllAsync();

        Task DeleteAsync(int id);

        Task UpdateAsync(UpdateUserDTO user);


    }
}
