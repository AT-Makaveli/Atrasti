using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Repository
{
    internal class ForumRepository : BaseRepository, IForumRepository
    {
        private static IList<string> _genres = new List<string>();
        private static int _genreAccesCount = 0;

        public ForumRepository(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        public async Task<IList<string>> GetGenresAsync()
        {
            if (_genres.Count > 0 && _genreAccesCount != 50) return _genres;

            return await WithConnection(async connection =>
            {
                _genreAccesCount = 0;
                var genres = await connection.QueryAsync<string>("SELECT * FROM forum_genres;");

                _genres = genres.AsList();
                return _genres;
            }, CancellationToken.None);
        }

        public async Task<IDictionary<string, IList<ForumThread>>> GetThreadsByGenre()
        {
            IList<string> genres = await GetGenresAsync();

            return await WithConnection(async connection =>
            {
                IDictionary<string, IList<ForumThread>> threads = new Dictionary<string, IList<ForumThread>>();

                foreach (string genre in genres) {
                    var threadsforGenre = await connection.SelectMultipleAsync<ForumThread>("SELECT * FROM forum_threads WHERE Genre = @0;", genre);
                    if (threadsforGenre.Count > 0)
                    {
                        threads.Add(genre, threadsforGenre);
                    }
                }

                return threads;
            }, CancellationToken.None);
        }

        public Task CreateThread(ForumThread thread)
        {
            return WithConnection(connection =>
            {
                return connection.Insert(
                    "INSERT INTO `forum_threads`(`Author`, `Title`, `Date`, `Views`) VALUES (@0,@1,@2,@3);", thread.AuthorId, thread.Title, thread.PublishDate, thread.Views);
            }, CancellationToken.None);
        }

        public Task CreatePost(ForumPost post)
        {
            return WithConnection(connection =>
            {
                return connection.Insert(
                    "INSERT INTO `forum_posts`(`AuthorId`, `Text`, `Date`, `ThreadId`) VALUES (@0, @1, @2, @3);", post.AuthorId, post.Text, post.Date, post.ThreadId);
            }, CancellationToken.None);
        }

        public Task<IList<ForumThread>> GetThreadsAsync()
        {
            return WithConnection(connection =>
            {
                return connection.SelectMultipleAsync<ForumThread>("SELECT * FROM forum_threads;");
            }, CancellationToken.None);
        }

        public Task<ForumThread> GetThreadAsync(uint id)
        {
            return WithConnection(connection =>
            {
                return connection.SelectSingleAsync<ForumThread>("SELECT * FROM forum_threads WHERE Id = @0 LIMIT 1;", id);
            }, CancellationToken.None);
        }

        public Task<IList<ForumPost>> GetPostsAsync(uint threadId)
        {
            return WithConnection(connection =>
            {
                return connection.SelectMultipleAsync<ForumPost>("SELECT forum_posts.*, users.Company FROM forum_posts JOIN users ON forum_posts.AuthorId = users.Id WHERE ThreadId = @0;", threadId);
            }, CancellationToken.None);
        }
    }
}
