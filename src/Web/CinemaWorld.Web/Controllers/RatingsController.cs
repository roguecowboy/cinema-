﻿namespace CinemaWorld.Web.Controllers
{
    using System.Threading.Tasks;

    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.Ratings;
    using CinemaWorld.Models.ViewModels.Ratings;
    using CinemaWorld.Services.Data.Contracts;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService ratingsService;
        private readonly UserManager<CinemaWorldUser> userManager;

        public RatingsController(IRatingsService ratingsService, UserManager<CinemaWorldUser> userManager)
        {
            this.ratingsService = ratingsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<StarRatingResponseModel>> Post(RatingInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.ratingsService.VoteAsync(input.MovieId, userId, input.Rating);
            var starRatingsSum = this.ratingsService.GetStarRatings(input.MovieId);

            return new StarRatingResponseModel
            {
                StarRatingsSum = starRatingsSum,
            };
        }
    }
}
