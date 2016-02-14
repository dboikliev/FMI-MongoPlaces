using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoPlaces.Core.DataAccess.Entities;

namespace MongoPlaces.Core.DataAccess.Interfaces
{
    public interface IUsersRepository
    {
        Task CreateAsync(User user);
        Task<User> FindByEmailAsync(string email);
        Task AddToFavoritesAsync(string username, string placeId);
        Task<ObjectId[]> GetFavoritePointIds(string email);
        Task RemoveFromFavorites(string email, string id);
    }
}
