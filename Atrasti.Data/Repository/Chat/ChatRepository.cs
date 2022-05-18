using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using Org.BouncyCastle.Crypto.Tls;

namespace Atrasti.Data.Repository.Chat
{
    internal class ChatRepository : BaseRepository, IChatRepository
    {
        public ChatRepository(ConnectionProvider connectionFactory)
            : base(connectionFactory)
        {
        }

        public Task<int> WriteMessageAsync(ChatMessage chatMessage)
        {
            const string query =
                "INSERT INTO `chat_messages`(SenderId, `ChatId`, `ChatMessage`, Created, `ChatType`) VALUES(@0, @1, @2, @3, @4); SELECT LAST_INSERT_ID()";
            return WithConnection(
                async connection =>
                {
                    var value = await connection.InsertScalar(query, chatMessage.SenderId, chatMessage.ChatId, chatMessage.Message, chatMessage.Created, chatMessage.ChatType);
                    if (value == null) return -1;

                    return Convert.ToInt32(value);
                },
                CancellationToken.None);
        }

        public Task<IList<ChatMessage>> FetchMessagesAsync(int chatId)
        {
            const string query = "SELECT chat_messages.*, u.Company as Author FROM chat_messages JOIN Users u ON SenderId = u.Id WHERE ChatId = @0;";

            return WithConnection(
                connection => connection.SelectMultipleAsync<ChatMessage>(query, chatId),
                CancellationToken.None);
        }

        public Task<IList<ChatFriend>> FetchFriendsAsync(int userId)
        {
            const string query = @"
            SELECT a.user_two_id as FriendId, a.Id AS ChatId, b.Company as FriendCompany
            FROM chat_friends as a 
            JOIN Users as b ON a.user_two_id = b.Id
            WHERE a.user_one_id = @0;";

            return WithConnection(
                connection => connection.SelectMultipleAsync<ChatFriend>(query, userId),
                CancellationToken.None);
        }

        public Task<ChatFriend> FetchFriendAsync(int userId, int friendId)
        {
            const string query = @"
            SELECT a.user_two_id as FriendId, a.Id AS ChatId, b.Company as FriendCompany
            FROM chat_friends as a 
            JOIN Users as b ON a.user_two_id = b.Id
            WHERE a.user_one_id = @0;";

            return WithConnection(
                connection => connection.SelectSingleAsync<ChatFriend>(query, userId),
                CancellationToken.None);
        }
    }
}