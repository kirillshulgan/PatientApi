namespace PatientApi.Models
{
    /// <summary>
    /// Класс с перечислениями.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Перечисление для указания пола пациента (0 - Male, 1 - Female, 2 - Other, 3 - Unknown).
        /// </summary>
        public enum Gender
        {
            Male,
            Female,
            Other,
            Unknown
        }

        /// <summary>
        /// Перечисление для указания статуса активности (0 - True, 1 - False).
        /// </summary>
        public enum Active
        {
            True,
            False
        }

    }
}
