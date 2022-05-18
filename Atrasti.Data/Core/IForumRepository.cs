using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface IForumRepository
    {
        Task<IList<string>> GetGenresAsync();

        Task<IDictionary<string, IList<ForumThread>>> GetThreadsByGenre();

        Task CreateThread(ForumThread thread);

        Task CreatePost(ForumPost post);

        Task<IList<ForumThread>> GetThreadsAsync();

        Task<ForumThread> GetThreadAsync(uint id);

        Task<IList<ForumPost>> GetPostsAsync(uint threadId);
    }
}
