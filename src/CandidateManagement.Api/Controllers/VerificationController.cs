using CandidateManagement.Application.Verifications.Commands;
using CandidateManagement.Application.Verifications.Dtos;
using CandidateManagement.Application.Verifications.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Route("[controller]")]
[SwaggerTag("Проверка кандидатов - поиск дубликатов по ФИО в кандидатах и сотрудниках")]
public class VerificationController : CustomControllerBase
{
    private readonly ISender _mediatr;

    public VerificationController(ISender mediatr)
    {
        _mediatr = mediatr;
    }
    [HttpPost("start")]
    [SwaggerOperation(
        Summary = "Запустить проверку кандидата по ФИО",
        Description = "Запускает асинхронную проверку на наличие совпадений по ФИО в базе кандидатов и сотрудников. Возвращает ID проверки без ожидания завершения."
    )]
    [SwaggerResponse(202, "Проверка запущена, возвращен ID проверки", typeof(Guid))]
    [SwaggerResponse(400, "Некорректные данные для проверки")]
    public async Task<ActionResult<Guid>> StartVerification([FromBody] StartVerificationRequest request)
    {
        var command = request.Adapt<StartVerificationCommand>() with { CurrentUserId = GetCurrentUserId() };
        var verificationId = await _mediatr.Send(command);
        return Accepted(verificationId);
    }

    [HttpGet("{id:guid}/results")]
    [SwaggerOperation(
        Summary = "Получить результаты проверки",
        Description = "Возвращает результаты проверки по ID, включая найденные ID кандидатов и сотрудников с совпадающим ФИО"
    )]
    [SwaggerResponse(200, "Результаты проверки получены", typeof(VerificationResultDto))]
    [SwaggerResponse(404, "Проверка с указанным ID не найдена")]
    public async Task<ActionResult<VerificationResultDto>> GetVerificationResults(Guid id)
    {
        var query = new GetVerificationResultQuery(id);
        var result = await _mediatr.Send(query);
        return Ok(result);
    }
}