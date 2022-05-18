using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Atrasti.WebSocket.Hubs;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Models.Chat;
using Atrasti.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Atrasti.Controllers
{
    [NonController]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<AtrastiUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatRepository chatRepository, UserManager<AtrastiUser> userManager,
            IHubContext<ChatHub> hubContext, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
            _hubContext = hubContext;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<JsonResult> UploadImage([FromBody] ImageData imageData)
        {
            if (!User.Identity.IsAuthenticated) return Json(new {errorCode = -1});
            if (!ModelState.IsValid) return Json(new {errorCode = -1});

            if (!int.TryParse(imageData.to, out int toId)) return Json(new {errorCode = -1});

            AtrastiUser fromUser = await _userManager.GetUserAsync(User);
            AtrastiUser toUser = await _userRepository.FindUserById(toId);

            if (fromUser == null || toUser == null) return Json(new {errorCode = -1});

            ChatFriend friend = await _chatRepository.FetchFriendAsync(fromUser.Id, toId);

            if (friend == null) return Json(new {errorCode = -1});

            var pathToFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/chat/", friend.ChatId.ToString());
            if (!Directory.Exists(pathToFolder)) Directory.CreateDirectory(pathToFolder);

            var base64Data = Regex.Match(imageData.image, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"]
                .Value;
            byte[] b64Data = Convert.FromBase64String(base64Data);
            if (ProfileController.CheckIfImageFile(b64Data))
            {
                var path = Path.Combine(pathToFolder, imageData.fileName);
                await System.IO.File.WriteAllBytesAsync(path, b64Data);
            }

            int timeStamp = NumberUtils.UnixTimeStamp();
            ulong chatId = await _chatRepository.WriteMessageAsync(fromUser.Id, friend.ChatId, imageData.fileName,
                timeStamp,
                (int) ChatType.FILE);

            foreach (var connectionId in ChatHub._connections.GetConnections(toUser.Email))
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveMessage", chatId, fromUser.Id, fromUser.Company, imageData.fileName, timeStamp);
            }

            return new JsonResult(new {errorCode = 1});
        }

        public class ImageData
        {
            public string to { get; set; }

            public string image { get; set; }

            public string fileName { get; set; }
        }
    }
}