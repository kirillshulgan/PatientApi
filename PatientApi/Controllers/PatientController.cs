using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;

/// <summary>
/// Контроллер для управления данными пациентов.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly PatientContext _context;

    /// <summary>
    /// Инициализирует новый экземпляр класса с заданным контекстом базы данных.
    /// </summary>
    /// <param name="context">Контекст базы данных для работы с данными пациентов.</param>
    public PatientController(PatientContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить список пациентов.
    /// </summary>
    /// <returns>Список пациентов.</returns>
    /// <param>Без параметров.</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        return await _context.Patients.ToListAsync();
    }

    /// <summary>
    /// Получить пациента по ID.
    /// </summary>
    /// <returns>Пациент.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }

    /// <summary>
    /// Создать нового пациента.
    /// </summary>
    /// <param name="patient">Объект пациента, который нужно создать.</param>
    /// <returns>Создан пациент с кодом состояния 201 (Created).</returns>
    /// <response code="201">Пациент успешно создан.</response>
    /// <response code="400">Переданы неверные данные.</response>
    /// <response code="500">Произошла ошибка на сервере.</response>
    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
    {
        patient.BirthDate = DateOnly.Parse(patient.BirthDate.ToString());
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
    }

    /// <summary>
    /// Обновить существующего пациента.
    /// </summary>
    /// <param name="id">Идентификатор пациента, которого нужно обновить.</param>
    /// <param name="patient">Объект пациента с обновленными данными.</param>
    /// <returns>Статус ответа 204 (No Content) при успешном обновлении.</returns>
    /// <response code="204">Пациент успешно обновлен.</response>
    /// <response code="400">Идентификаторы не совпадают.</response>
    /// <response code="404">Пациент не найден.</response>
    /// <response code="500">Произошла ошибка на сервере.</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(Guid id, Patient patient)
    {
        if (id != patient.Id) return BadRequest();

        var existingPatient = await _context.Patients.Include(p => p.Name).FirstOrDefaultAsync(P => P.Id == id);
        if (existingPatient == null) return NotFound();

        existingPatient.Gender = patient.Gender;
        existingPatient.BirthDate = patient.BirthDate;
        existingPatient.Active = patient.Active;

        existingPatient.Name.Use = patient.Name.Use;
        existingPatient.Name.Family = patient.Name.Family;
        existingPatient.Name.Given = patient.Name.Given;

        _context.Entry(existingPatient.Name).Property(n => n.Use).IsModified = true;
        _context.Entry(existingPatient.Name).Property(n => n.Family).IsModified = true;
        _context.Entry(existingPatient.Name).Property(n => n.Given).IsModified = true;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Удалить пациента по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пациента, которого нужно удалить.</param>
    /// <returns>Статус ответа 204 (No Content) при успешном удалении.</returns>
    /// <response code="204">Пациент успешно удален.</response>
    /// <response code="404">Пациент не найден.</response>
    /// <response code="500">Произошла ошибка на сервере.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Поиск пациентов по дате рождения.
    /// </summary>
    /// <param name="birthDate">Дата рождения для поиска пациентов (в формате yyyy-mm-dd).</param>
    /// <returns>Список пациентов с указанной датой рождения.</returns>
    /// <response code="200">Возвращен список найденных пациентов.</response>
    /// <response code="404">Пациенты не найдены.</response>
    /// <response code="500">Произошла ошибка на сервере.</response>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Patient>>> SearchByBirthDate(DateOnly birthDate)
    {
        var patients = await _context.Patients
            .Where(p => p.BirthDate == birthDate)
            .ToListAsync();

        if (patients == null || !patients.Any())
        {
            return NotFound();
        }

        return Ok(patients);
    }
}