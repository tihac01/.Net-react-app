using Application.Followers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FollowController : BaseApiController
{    
    [HttpGet("{username}")]
    public async Task<IActionResult> GetFollowings(string username, string predicate)
    {
        return HandelResult(await Mediator.Send(new List.Query { Username = username, Predicate = predicate }));
    }

    [HttpPost("{username}")]
    public async Task<IActionResult> Follow(string username)
    {
        return HandelResult(await Mediator.Send(new FollowToggle.Command{ TargetUsername = username }));
    }
}
