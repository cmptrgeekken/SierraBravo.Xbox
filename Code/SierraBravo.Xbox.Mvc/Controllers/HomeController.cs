using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using SierraBravo.Xbox.Mvc.Models;
using SierraBravo.Xbox.Services.Interfaces;

namespace SierraBravo.Xbox.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoGameVotingService _votingService;

        public HomeController()
        {
            _votingService = new WindsorContainer(new XmlInterpreter()).Resolve<IVideoGameVotingService>();
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var allGames = _votingService.GetAllGames().OrderBy(g => g.Title);
            var ownedGames = allGames.Where(g => g.IsOwned);
            var wantedGames = allGames.Where(g => !g.IsOwned);

            return View(new VideoGameViewModel
                            {
                                OwnedGames = ownedGames,
                                WantedGames = wantedGames
                            });
        }

        public ActionResult Add()
        {
            return View(new VideoGameAddModel());
        }

        [HttpPost]
        public ActionResult Add(VideoGameAddModel model)
        {
            ActionResult result = null;
            if (ModelState.IsValid)
            {
                var success = _votingService.AddGame(model.Title.Trim());
                if (success)
                {
                    SetSuccessMessage("Game added successfully.");

                    result = RedirectToAction("Add");
                }else
                {
                    ModelState.AddModelError("Title", "Title already exists.");
                }
            }


            if( result == null )
            {
                result = View(model);
            }

            return result;
        }
        
        public ActionResult Vote(int id)
        {
            ActionResult result;
            var game = _votingService.GetGameById(id);
            if( game == null )
            {
                SetErrorMessage("Game not found.");

                result = RedirectToAction("Index");
            }else
            {
                var model = new VideoGameSubmitModel
                                {
                                    Id = game.Id,
                                    Title = game.Title
                                };

                result = View(model);
            }
            return result;
        }


        [HttpPost]
        public ActionResult Vote(VideoGameSubmitModel model)
        {
            var success = _votingService.VoteForGame(model.Id);
            if( success )
            {
                var game = _votingService.GetGameById(model.Id);
                SetSuccessMessage("Vote successfully cast.");
            }else
            {
                SetErrorMessage("Cannot vote for a game that's already owned.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult MarkOwned(int id)
        {
            ActionResult result;
            var game = _votingService.GetGameById(id);
            if (game == null)
            {
                SetErrorMessage("Game not found.");

                result = RedirectToAction("Index");
            }
            else
            {
                var model = new VideoGameSubmitModel
                                {
                                    Id = game.Id,
                                    Title = game.Title
                                };

                result = View(model);
            }

            return result;
        }

        [HttpPost]
        public ActionResult MarkOwned(VideoGameSubmitModel model)
        {
            var success = _votingService.MarkGameAsOwned(model.Id);
            if( success )
            {
                var game = _votingService.GetGameById(model.Id);
                SetSuccessMessage(String.Format("<b>{0}</b> marked as owned successfully.", game.Title));
            }else
            {
                SetErrorMessage("Cannot mark a game as owned more than once.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Clear(string txtConfirm)
        {
            if (txtConfirm == "confirm")
            {
                var success = _votingService.ClearAllGames();
                if( success )
                {
                    SetSuccessMessage("Games successfully cleared.");
                }else
                {
                    SetErrorMessage("Clearing games failed. Please try again later.");
                }
            }
            return RedirectToAction("Index");
        }

        private void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        private void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }
    }
}
