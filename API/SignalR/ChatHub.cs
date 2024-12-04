using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

[Authorize]
public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendComment(Create.Command command)
    {
        var comment = await _mediator.Send(command);

        await Clients.Group(command.ActivityId.ToString())
            .SendAsync("ReceiveComment", comment.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var activitId = httpContext.Request.Query["activityId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, activitId);

        var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activitId) });
        await Clients.Caller.SendAsync("LoadComments", result.Value);
    }
}
