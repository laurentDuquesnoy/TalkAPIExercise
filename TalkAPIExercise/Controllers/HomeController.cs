using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TalkAPIExercise.Model;
using TalkAPIExercise.Models;
using TalkApiExercise.SDK;

namespace TalkAPIExercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TalkApiHandler _talkApi;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, TalkApiHandler talkApi)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _talkApi = talkApi;
        }

        public async Task<IActionResult> Index(ChatModel model)
        {
            if (model.Channels.Count == 0)
            {
                model = await CreateChatModel();
            }
            return View(model);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public async Task<IActionResult> Channel_click(string channelName)
        {
            var model = CreateChatModel(channelName);
            return RedirectToAction("Index", model);
        }

        private async Task<ChatModel> CreateChatModel(string selectedChannel = "")
        {
            var model = new ChatModel
            {
                Channels = await _talkApi.getChannels(),
                Messages = await _talkApi.GetMessages(),
                SelectedChannel = selectedChannel
            };
            return model;
        }

        private ChatModel FilterModelMessages(ChatModel model)
        {
            foreach (MessageModel message in model.Messages)
            {
                if (message.Channel != model.SelectedChannel)
                {
                    model.Messages.Remove(message);
                }
            }
            return model;
        }
    }
}