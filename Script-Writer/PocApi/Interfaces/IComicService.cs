using PocApi.Models;

namespace PocApi.Interfaces
{
    public interface IComicService
    {
        public Task<List<Comic>> GetAll();

        public Task<Comic?> GetWithId(string id);

        public Task Create(Comic newComic);

        public Task Update(string id, Comic updatedComic);

        public Task Delete(string id);
    }
}
