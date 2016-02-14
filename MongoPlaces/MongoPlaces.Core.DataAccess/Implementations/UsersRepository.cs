using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.DataAccess.Entities;
using MongoPlaces.Core.DataAccess.Interfaces;

namespace MongoPlaces.Core.DataAccess.Implementations
{
    public class UsersRepository : AbstractRepository<User>, IUsersRepository
    {
        protected override string CollectionName => "Users";

        public async Task CreateAsync(User user)
        {
            await Collection.InsertOneAsync(user);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var cursor = await Collection.FindAsync(u => u.Email == email);
            var user = await cursor.FirstOrDefaultAsync();
            return user;
        }

        public async Task AddToFavoritesAsync(string username, string placeId)
        {
            await Collection.UpdateOneAsync(Builders<User>.Filter.Eq(u => u.Email, username), Builders<User>.Update.AddToSet(u => u.FavoritePlaces, new ObjectId(placeId)));
        }

        public async Task<ObjectId[]> GetFavoritePointIds(string email)
        {
            var result =
                await Collection.FindAsync(Builders<User>.Filter.Eq(u => u.Email, email), new FindOptions<User, User>()
                {
                    Projection =
                        Builders<User>.Projection.Include(u => u.FavoritePlaces)
                            .Exclude(u => u.Id)
                            .Exclude(u => u.PasswordHash)
                            .Exclude(u => u.SaltHash)
                            .Exclude(u => u.Email)
                });
            return (await result.ToListAsync()).SelectMany(u => u.FavoritePlaces).ToArray();
        }

        public async Task RemoveFromFavorites(string email, string id)
        {
            await Collection.UpdateOneAsync(Builders<User>.Filter.Eq(u => u.Email, email),
                Builders<User>.Update.Pull(u => u.FavoritePlaces, new ObjectId(id)));
        }
    }
}
