﻿using API.Extensions;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected ActionResult HandelResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value == null) return NotFound(); 
        if (result.IsSuccess && result.Value != null) return Ok(result.Value);
             
        return BadRequest(result.Error);
    } 
    
    protected ActionResult HandelPagedResult<T>(Result<PageList<T>> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value == null) return NotFound();
        if (result.IsSuccess && result.Value != null) 
        {
            Response.AddPaginationHeader(result.Value.CurrentPage, 
                result.Value.PageSize, result.Value.TotalCount, 
                result.Value.TotalPages);
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}
