using CandidateManagement.Application.Employees.Commands;
using CandidateManagement.Application.Employees.Dtos;
using CandidateManagement.Application.Employees.Queries;
using CandidateManagement.Application.Employees.Requests;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CandidateManagement.Api.Controllers;

[Route("[controller]")]
[SwaggerTag("Управление сотрудниками - создание сотрудников из кандидатов")]
public class EmployeeController : CustomControllerBase
{
    private readonly ISender _mediatr;

    public EmployeeController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet("{id}", Name = "GetEmployeeById")]
    [SwaggerOperation(
        Summary = "Получить сотрудника по ID",
        Description = "Возвращает информацию о сотруднике по его идентификатору"
    )]
    [SwaggerResponse(200, "Сотрудник найден", typeof(EmployeeReadDto))]
    [SwaggerResponse(404, "Сотрудник не найден")]
    public async Task<ActionResult<EmployeeReadDto>> GetEmployeeById(Guid id)
    {
        var queyry = new GetEmployeeByIdQuery(id);
        var result = await _mediatr.Send(queyry);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Создать сотрудника из кандидата",
        Description = "Перевод кандидата в сотрудника с созданием соответствующей записи"
    )]
    [SwaggerResponse(201, "Сотрудник успешно создан", typeof(EmployeeReadDto))]
    [SwaggerResponse(400, "Ошибка при создании сотрудника")]
    [SwaggerResponse(404, "Кандидат не найден")]
    [SwaggerResponse(403, "Доступ запрещен")]
    public async Task<ActionResult<EmployeeReadDto>> CreateEmployeeFromCandidate(CreateEmployeeFromCandidateRequest request)
    {
        var command = request.Adapt<CreateEmployeeFromCandidateCommand>() with { CurrentUserId = GetCurrentUserId() };
        var result = await _mediatr.Send(command);

        return CreatedAtRoute(nameof(GetEmployeeById),
            new { id = result.Id }, result);
    }
}