using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandelResult(await Mediator.Send(new Details.Query { UserName = username }));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetActivitiesForUser(string username, string predicate)
        {
            return HandelResult(await Mediator.Send(new ListActivities.Query { 
                UserName = username, 
                Predicate = predicate 
            }));
        }

        [HttpPut]
        public async Task<IActionResult> EditAppUser(Edit.Command command)
        {
            return HandelResult(await Mediator.Send(command));
        }
    }
}
