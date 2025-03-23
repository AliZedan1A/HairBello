using System.Text.Json.Serialization;

namespace Domain.DatabaseModels
{
    public class BarberScheduleModel
    {
        public int Id { get; set; }
        public int BarberId { get; set; }
        public string DayName { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        [JsonIgnore]
        public BarberModel Barber { get; set; }

    }
}