using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Spa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atrasti.Spa.Controllers
{
    [Route("chat/[action]")]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Friends([FromBody] RequestFriendsViewModel requestFriendsModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            IList<ChatFriend> friends = await _chatRepository.FetchFriendsAsync(8);

            return Ok(friends);
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] PostChatViewModel postChatViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool chatSuccess = true;

            if (chatSuccess)
            {
                return Ok(new
                {
                    invalid = false
                });
            }
            else
            {
                return Ok(new
                {
                    invalid = true
                });
            }
        }
    }
}