using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Atrasti.Models.Forum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Controllers
{
    [NonController]
    public class ForumController : Controller
    {
        private readonly IForumRepository _forumRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly UserManager<AtrastiUser> _userManager;

        public ForumController(IForumRepository forumRepository, ISubscriptionRepository subscriptionRepository, UserManager<AtrastiUser> userManager)
        {
            _forumRepository = forumRepository;
            _subscriptionRepository = subscriptionRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IDictionary<string, Subscription> subscriptions = await _subscriptionRepository.FetchSubscriptions();
            if (subscriptions.TryGetValue("search_hero", out Subscription subscription))
                ViewData["hero"] = subscription;

            IDictionary<string, IList<ForumThread>> threadsByGenre = await _forumRepository.GetThreadsByGenre();
            ViewData["user"] = await _userManager.GetUserAsync(User);
            ViewData["forumThreads"] = threadsByGenre;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Thread(uint id)
        {
            ForumThread forumThread = await _forumRepository.GetThreadAsync(id);
            if (forumThread != null)
            {
                forumThread.Author = await _userManager.FindByIdAsync(forumThread.AuthorId.ToString());
                ViewData["forumPosts"] = await _forumRepository.GetPostsAsync(id);
            }

            ViewData["forumThread"] = forumThread;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Thread(CreatePostModel createPostModel)
        {
            if (createPostModel.PostContent == null || !User.Identity.IsAuthenticated)
            {
                return await Thread(createPostModel.ThreadId);
            }

            AtrastiUser user = await _userManager.GetUserAsync(User);

            await _forumRepository.CreatePost(new ForumPost()
            {
                AuthorId = user.Id,
                Date = DateTime.Now,
                Text = createPostModel.PostContent,
                ThreadId = createPostModel.ThreadId
            });

            return Redirect(Url.Action("Thread", "Forum", new { id = createPostModel.ThreadId }));
        }
    }
}