using System.ComponentModel.DataAnnotations;

namespace WeatherStats.Model
{
    class Student
    {
        public Student()
        {
        }

        public int Student_ID { get; set; }
        public string StudentName { get; set; }
        
        //[Index( "IX_REG", IsClustered=true, IsUnique=true )]
        public int RegistrationNumber { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
