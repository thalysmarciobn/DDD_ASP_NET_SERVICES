using Common.CQRS;
using Email.Application.Commands;
using Email.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Email.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IDispatcher dispatcher, ILogger<EmailController> logger)
    {
        _dispatcher = dispatcher;
        _logger = logger;
    }

    [HttpPost("send-verification")]
    public async Task<IActionResult> SendVerificationEmail([FromBody] SendVerificationEmailCommand command)
    {
        var result = await _dispatcher.SendAsync<SendVerificationEmailCommand, Result<SendVerificationEmailData>>(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            });
        }

        return Ok(new
        {
            SuccessCode = result.SuccessCode,
            Data = result.Data
        });
    }

    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailCommand command)
    {
        var result = await _dispatcher.SendAsync<ResendVerificationEmailCommand, Result<ResendVerificationEmailData>>(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage
            });
        }

        return Ok(new
        {
            SuccessCode = result.SuccessCode,
            Data = result.Data
        });
    }
}
