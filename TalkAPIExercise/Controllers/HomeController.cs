using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        //TODO add a function to add channels - could be integrated in later applications
        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, TalkApiHandler talkApi)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _talkApi = talkApi;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string channelName = "")
        {
            var model = await CreateChatModel(channelName);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ChatModel model)
        {
            if (string.IsNullOrWhiteSpace(model.MessageContent))
            {
                return RedirectToAction("Index" ,new {model.SelectedChannel});
            }
            MessageModel message = new MessageModel
            {
                Author = User.FindFirst(ClaimTypes.Email).Value,
                Channel = model.SelectedChannel,
                Message = model.MessageContent
            };
            var newMessage = await _talkApi.SendMessage(message);
            return RedirectToAction("Index", new {channelName = model.SelectedChannel});
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
            return RedirectToAction("Index", new{channelName});
        }

        private async Task<ChatModel> CreateChatModel(string selectedChannel = "")
        {
            var model = new ChatModel
            {
                Channels = await _talkApi.getChannels(),
                Messages = await _talkApi.GetMessages(),
                SelectedChannel = selectedChannel
            };
            if (!string.IsNullOrWhiteSpace(selectedChannel))
            {
                model = await FilterModelMessages(model);
            }
            return model;
        }

        private async Task<ChatModel> FilterModelMessages(ChatModel model)
        {
            foreach (MessageModel message in model.Messages.ToList())
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