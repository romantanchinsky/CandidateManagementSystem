using CandidateManagement.Application.Candidates.Commands;
using CandidateManagement.Application.Candidates.Dtos;
using CandidateManagement.Application.Candidates.Queries;
using CandidateManagement.Application.Candidates.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Authorize]
[Route("[controller]")]
[SwaggerTag("Управление кандидатами - создание, редактирование, поиск кандидатов")]
public class CandidateController : CustomControllerBase
{
    private readonly ISender _mediatr;

    public CandidateController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{id}", Name = "GetCandidateById")]
    [SwaggerOperation(
        Summary = "Получить кандидата по ID",
        Description = "Возвращает информацию о кандидате по его идентификатору"
    )]
    [SwaggerResponse(200, "Кандидат найден", typeof(CandidateReadDto))]
    [SwaggerResponse(404, "Кандидат не найден")]
    [SwaggerResponse(403, "Доступ запрещен")]
    public async Task<ActionResult<CandidateReadDto>> GetCandidateById(Guid id)
    {
        var query = new GetCandidateByIdQuery(id);
        var result = await _mediatr.Send(query);
        return Ok(result);
    }

    [HttpPost]
    
    [SwaggerOperation(
        Summary = "Создать нового кандидата",
        Description = "Создание кандидата в системе в рабочей группе текущего пользователя"
    )]
    [SwaggerResponse(201, "Кандидат успешно создан", typeof(CandidateReadDto))]
    [SwaggerResponse(400, "Ошибка валидации данных")]
    [SwaggerResponse(403, "Пользователь не имеет прав на создание кандидата")]
    public async Task<ActionResult<CandidateReadDto>> CreateCandidate(CreateCandidateRequest createCandidateRequest)
    {
        var command = createCandidateRequest.Adapt<CreateCandidateCommand>() with { CreatedById = GetCurrentUserId() };
        var result = await _mediatr.Send(command);
        return CreatedAtRoute(nameof(GetCandidateById),
            new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Обновить данные кандидата",
        Description = "Обновление всех полей кандидата, кроме даты последнего обновления"
    )]
    [SwaggerResponse(204, "Кандидат успешно обновлен")]
    [SwaggerResponse(400, "Некорректные данные для обновления")]
    [SwaggerResponse(404, "Кандидат не найден")]
    [SwaggerResponse(403, "Доступ запрещен")]
    public async Task<IActionResult> UpdateCandidate(Guid id, [FromBody] UpdateCandidateRequest updateCandidateRequest)
    {
        var command = updateCandidateRequest.Adapt<UpdateCandidateCommand>() with { CandidateId = id, CurrentUserId = GetCurrentUserId() };
        await _mediatr.Send(command);
        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Получить список кандидатов с фильтрацией",
        Description = "Возвращает paginated список кандидатов с поиском по ФИО/email, фильтрацией по рабочему графику и флагом 'Только мои кандидаты'"
    )]
    [SwaggerResponse(200, "Список кандидатов получен", typeof(PaginatedList<CandidateReadDto>))]
    [SwaggerResponse(400, "Некорректные параметры запроса")]
    [SwaggerResponse(403, "Доступ запрещен")]
    public async Task<ActionResult<PaginatedList<CandidateReadDto>>> GetCandidates(
        [FromQuery] GetCandidatesWithPaginationQuery query)
    {
        var command = query with { CurrentUserId = GetCurrentUserId() };

        var result = await _mediatr.Send(command);
        return Ok(result);
    }
}