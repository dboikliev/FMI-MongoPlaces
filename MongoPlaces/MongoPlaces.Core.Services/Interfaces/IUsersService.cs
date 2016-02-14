using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoPlaces.Core.Contracts.Dto;

namespace MongoPlaces.Core.Services.Interfaces
{
    public interface IUsersService
    {
        Task CreateAsync(UserDto user);
        Task<UserDto> GetAsync(string username);
        Task<bool> CheckCredentials(string username, string password);
    }
}
