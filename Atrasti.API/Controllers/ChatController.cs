using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.API.Services.Firebase;
using Atrasti.API.Helpers;
using Atrasti.API.Models.Chat;
using Atrasti.API.Models.Profile;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly IFirebaseService _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public ChatController(IChatRepository chatRepository, IFirebaseService firebaseService, IUserRepository userRepository, UserManager<AtrastiUser> userManager)
        {
            _chatRepository = chatRepository;
            _firebaseService = firebaseService;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFriends()
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);

            if (!user.CompanyInfoSetup && !user.CompanySetup)
            {
                return BadRequest(new InvalidChatModelError(InvalidChatModelError.USER_PROFILE_NOT_SET,
                    "User profile is not set."));
            }

            IList<ChatFriend> friends = await _chatRepository.FetchFriendsAsync(user.Id);

            return Ok(new ChatFriends_Res()
            {
                Friends = friends.MapChatFriends()
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChatMessages([FromBody] ChatMessages_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            ICollection<ChatMessage> messages = await _chatRepository.FetchMessagesAsync(req.ChatId);

            var mappedMessages = messages.MapChatMessages();
            foreach (var chatMessage in mappedMessages)
            {
                if (chatMessage.SenderId == user.Id) chatMessage.FromMe = true;
            }
            
            return Ok(new ChatMessages_Res()
            {
                Messages = mappedMessages
            });
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendChat([FromBody] SendChatMessage_Req req)
        {
            AtrastiUser user = await _userManager.GetUserAsync(User);
            AtrastiUser toUser = await _userRepository.FindSetupUserById(req.FriendId);
            if (toUser == null)
            {
                return BadRequest(new InvalidChatModelError(InvalidChatModelError.USER_NOT_SET, "User doesn't exist."));
            }

            req.Message = req.Message.TrimEnd();
            
            ChatMessage message = new ChatMessage
            {
                SenderId = user.Id,
                ChatId = req.ChatId,
                Message = req.Message,
                Created = DateTime.Now,
                ChatType = ChatType.CHAT
            };

            int chatId = await _chatRepository.WriteMessageAsync(message);
            if (chatId == -1)
            {
                return BadRequest();
            }

            message.Id = chatId;
            var res = message.MapChatMessage();
            res.FromMe = true;
            res.Author = user.Company;

            string data = await _firebaseService.SendMessage(new FirebaseChatMessage(res, toUser.UserData.FcmToken));
            Console.WriteLine(data);
            
            return Ok(res);
        }
    }
}