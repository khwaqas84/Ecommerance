using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Implemantations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db)
        {
        }

        public bool CreateUser(User user, string Role)
        {
            try 
            {
                Role role = _db.Roles.Where(r => r.Name == Role).FirstOrDefault();
                if (role != null)
                {
                    user.Password=BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Roles.Add(role);
                    _db.SaveChanges();
                    return true;
                }

            }
            catch(Exception ex) {
                throw ex;
            }  
            return false;
        }

        public UserModel ValidateUser(string email, string Password)
        {
           User user = _db.Users.Include(u=>u.Roles).Where(u=>u.Email==email).FirstOrDefault();
            if(user != null)
            {
                bool isVerified=BCrypt.Net.BCrypt.Verify(Password, user.Password);
                if (isVerified)
                {
                    UserModel model = new UserModel()
                    {
                        Id=user.Id,
                        Email=user.Email,
                        Name=user.Name,
                        PhoneNumber=user.PhoneNumber,
                        Roles=user.Roles.Select(r => r.Name).ToArray(),
                    };
                    return model;
                }
            }
            return null;
        }
    }
}
