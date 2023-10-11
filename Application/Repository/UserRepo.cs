using Application.Interfaces;
using Domain;
using Infastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class UserRepo : GenericRepo<User>,IUserRepo
    {
        public UserRepo(UsersDbContext dbcontext):base(dbcontext)
        {
        }
    }
}
