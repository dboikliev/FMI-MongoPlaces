using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.DataAccess.Entities;
using MongoPlaces.Core.DataAccess.Implementations;
using MongoPlaces.Core.DataAccess.Interfaces;
using MongoPlaces.Core.Services.Helpers;
using MongoPlaces.Core.Services.Interfaces;

namespace MongoPlaces.Core.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService()
        {
            _usersRepository = new UsersRepository();
        }

        public async Task CreateAsync(UserDto user)
        {
            var hash = new PasswordHash(user.Password);
            await _usersRepository.CreateAsync(new User()
            {
                Email = user.Email,
                PasswordHash = Convert.ToBase64String(hash.PasswordBytes),
                SaltHash = Convert.ToBase64String(hash.SaltBytes)
            });
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await _usersRepository.FindByEmailAsync(username);
            return new UserDto
            {
                Email = user.Email
            };
        }

        public async Task<bool> CheckCredentials(string username, string password)
        {
            var user = await _usersRepository.FindByEmailAsync(username);
            if (user == null)
            {
                return false;
            }
            var hash = new PasswordHash(password, Convert.FromBase64String(user.SaltHash));
            var isPasswordValid = Convert.FromBase64String(user.PasswordHash).SequenceEqual(hash.PasswordBytes);
            return isPasswordValid;
        } 
    }
}
