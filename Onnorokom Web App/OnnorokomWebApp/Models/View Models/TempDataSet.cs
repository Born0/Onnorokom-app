namespace OnnorokomWebApp.Models.View_Models
{
    public static class TempDataSet
    {
        public static User GetTempAdmin()
        {
            return new User { Id = -1, Name = "T Admin", Password = "password", Role = "ADMIN" };
        }
    }
}
