/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using SierraBravo.Xbox.Mvc.Filters;
using SierraBravo.Xbox.Mvc.Models;
using SierraBravo.Xbox.Services.Interfaces;

namespace SierraBravo.Xbox.Mvc.Controllers
{
    [SessionState(SessionStateBehavior.Required)]
    public class HomeController : BaseController<IVideoGameVotingService>
    {       
        public ActionResult Index()
        {
            var allGames = ControllerService.GetAllGames().OrderByDescending(g => g.NumberOfVotes);
            var ownedGames = allGames.Where(g => g.IsOwned);
            var wantedGames = allGames.Where(g => !g.IsOwned);

            return View(new VideoGameViewModel
                            {
                                OwnedGames = ownedGames,
                                WantedGames = wantedGames
                            });
        }

        [RequiresAction(false)]
        public ActionResult Add()
        {
            return View(new VideoGameAddModel());
        }

        [HttpPost,RequiresAction(true)]
        public ActionResult Add(VideoGameAddModel model)
        {
            ActionResult result = null;
            if (ModelState.IsValid)
            {
                var success = ControllerService.AddGame(model.Title.Trim());
                if (success)
                {
                    SetSuccess("Game added successfully.");

                    result = RedirectToAction("Index");
                }else
                {
                    SetError("Title already exists.");
                }
            }else
            {
                SetError("Title improperly formatted.");
            }

            return result ?? View(model);
        }

        [RequiresAction(false)]
        public ActionResult Vote(int id)
        {
            ActionResult result;
            var game = ControllerService.GetGameById(id);
            if( game == null )
            {
                SetError("Game not found.");

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


        [HttpPost,RequiresAction(true)]
        public ActionResult Vote(VideoGameSubmitModel model)
        {
            var success = ControllerService.VoteForGame(model.Id);
            if( success )
            {
                SetSuccess("Vote successfully cast.");
            }else
            {
                SetError("Cannot vote for a game that's already owned.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult MarkOwned(int id)
        {
            ActionResult result;
            var game = ControllerService.GetGameById(id);
            if (game == null)
            {
                SetError("Game not found.");

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
            var success = ControllerService.MarkGameAsOwned(model.Id);
            if( success )
            {
                var game = ControllerService.GetGameById(model.Id);
                
                SetSuccess(String.Format("<b>{0}</b> marked as owned successfully.", game.Title));
            }else
            {
                SetError("Cannot mark a game as owned more than once.");
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
                var success = ControllerService.ClearAllGames();
                if( success )
                {
                    SetSuccess("Games successfully cleared.");
                }else
                {
                    SetError("Clearing games failed. Please try again later.");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Faq()
        {
            return View();
        }

        private ActionResult ReadOnlyMessage()
        {
            SetError("Site is in read-only mode. Action forbidden.");

            return RedirectToAction("Index");
        }
    }
}
