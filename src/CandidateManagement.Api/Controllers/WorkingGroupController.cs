using CandidateManagement.Application.WorkingGroups.Commands;
using CandidateManagement.Application.WorkingGroups.Dtos;
using CandidateManagement.Application.WorkingGroups.Queries;
using CandidateManagement.Application.WorkingGroups.Requests;
using CandidateManagement.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Authorize(Policy = "AdminOnly")]
[Route("[controller]")]
[SwaggerTag("Управление рабочими группами - создание групп, управление участниками")]
public class WorkingGroupController : CustomControllerBase
{
    private readonly ISender _mediatr;

    public WorkingGroupController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{id}", Name = "GetById")]
    [SwaggerOperation(
        Summary = "Получить рабочую группу по ID",
        Description = "Возвращает информацию о рабочей группе и ее участниках"
    )]
    [SwaggerResponse(200, "Рабочая группа найдена", typeof(WorkingGroupReadDto))]
    [SwaggerResponse(404, "Рабочая группа не найдена")]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    public async Task<ActionResult<WorkingGroupReadDto>> GetById(Guid id)
    {
        var query = new GetWorkingGroupByIdQuery(id);
        var result = await _mediatr.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Создать рабочую группу",
        Description = "Создание новой рабочей группы в системе"
    )]
    [SwaggerResponse(201, "Рабочая группа успешно создана", typeof(WorkingGroup))]
    [SwaggerResponse(400, "Ошибка валидации данных")]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    public async Task<ActionResult<WorkingGroup>> Create(CreateWorkingGroupRequest request)
    {
        var command = request.Adapt<CreateWorkingGroupCommand>() with { CurrentUserId = GetCurrentUserId() };

        var result = await _mediatr.Send(command);
        return CreatedAtRoute(nameof(GetById),
            new { id = result.Id }, result);
    }

    [HttpPut("{id}/participants")]
    [SwaggerOperation(
        Summary = "Обновить состав рабочей группы",
        Description = "Обновление списка участников рабочей группы с автоматической миграцией кандидатов при перемещении HR"
    )]
    [SwaggerResponse(204, "Состав группы успешно обновлен")]
    [SwaggerResponse(400, "Некорректные данные для обновления")]
    [SwaggerResponse(404, "Рабочая группа не найдена")]
    [SwaggerResponse(403, "Доступ запрещен - требуется роль администратора")]
    public async Task<IActionResult> UpdateWorkingGroupMembers(Guid id, [FromBody] UpdateWorkingGroupParticipantsRequest request)
    {
        var command = request.Adapt<UpdateWorkingGroupMembersCommand>() with
        {
            WorkingGroupId = id,
            CurrentUserId = GetCurrentUserId()
        };

        await _mediatr.Send(command);
        return NoContent();
    }
}