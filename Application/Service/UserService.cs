using Application.Interfaces;
using Application.Models.DTO.User;
using Application.Repository;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class UserService : IUserService
    {

        public UserService(IMapper mapper,IUserRepo repo) 
        {
            _mapper = mapper;
            _entityRepo = repo;
        }
             private readonly IMapper _mapper;
             private readonly IUserRepo _entityRepo;

        public async Task<string> AddAsync(CreateReqUserDTO requestDTO)
        {
         
            User entity = _mapper.Map<User>(requestDTO);

            EntityEntry<User> entityEntry = _entityRepo.Add(entity);

            await _entityRepo.SaveChangesAsync();
            int entityId = entityEntry.Entity.Id;
            if (entityId <= 0)
                throw new Exception("NotSaveChange");

            return "ADDED";
        }
        public async Task<List<GetReqUserDTO>> GetAllAsync()
        {
            IQueryable<User> entitiesQuery = _entityRepo.GetAll();

            List<GetReqUserDTO> datadto = _mapper.Map<List<GetReqUserDTO>>(entitiesQuery);

            return datadto;
        }

        public async Task DeleteAsync(int id)
        {
            User? user =await _entityRepo.GetByIdStrictAsync(id);

            if (user == null)
                throw new Exception("entity is not defined");

             _entityRepo.Remove(user);
            bool issuccess = await _entityRepo.SaveChangesAsync();

            if (!issuccess)
                throw new Exception("Entity hasn't been updated.");
        }

        public async Task UpdateAsync(UpdateUserDTO user)
        {
            User mappeduser = _mapper.Map<User>(user);

             _entityRepo.Update(mappeduser);

            bool issuccess = await _entityRepo.SaveChangesAsync();

            if (!issuccess)
                throw new Exception("Entity hasn't been updated.");
        }
    }
}
