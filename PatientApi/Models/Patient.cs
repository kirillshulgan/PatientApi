using System.ComponentModel.DataAnnotations;
using static PatientApi.Models.Enums;

namespace PatientApi.Models
{
    /// <summary>
    /// Представляет имя пациента.
    /// </summary>
    public class Name
    {
        /// <summary>
        /// Указывает, как используется фамилия (например "Официально" или "Неофициально").
        /// </summary>
        public string Use { get; set; } = "official";

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Required]
        public string Family { get; set; } = string.Empty;

        /// <summary>
        /// Массив из имени и отчества.
        /// </summary>
        [MinLength(2, ErrorMessage = "Поле 'Given' должно содержать минимум 2 элемента.")]
        public string[] Given { get; set; } = new string[2] { string.Empty, string.Empty};
    }

    /// <summary>
    /// Представляет пациента.
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Уникальный идентификатор.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Имя пациента.
        /// </summary>
        public Name Name { get; set; } = new Name();

        /// <summary>
        /// Пол.
        /// </summary>
        public Gender Gender { get; set; } = Gender.Unknown;

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [Required]
        public DateOnly BirthDate { get; set; }

        /// <summary>
        /// Статус активности.
        /// </summary>
        public Active Active { get; set; } = Active.True;
    }
}
