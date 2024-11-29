using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PhotosController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromForm] Add.Command command)
    {
        return HandelResult(await Mediator.Send(command));
    }

    [HttpPost("{id}/setMain")]
    public async Task<IActionResult> SetMain(string id)
    {
        return HandelResult(await Mediator.Send(new SetMain.Command { PhotoId = id }));
    }

    [HttpDelete("{id}/")]
    public async Task<IActionResult> Delete(string id)
    {
        return HandelResult(await Mediator.Send(new Delete.Command{ PhotoId = id}));
    }
}
