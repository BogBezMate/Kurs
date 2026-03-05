using Kurs.Data;
using Kurs.Models;

namespace Kurs.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(CarServiceContext db)
        {
            // Если пользователи уже есть — ничего не делаем
            if (db.Users.Any()) return;

            // Создаём сотрудника-администратора
            var admin = new Employee
            {
                Last_Name = "Администратор",
                First_Name = "Главный",
                Position = "Администратор",
                Phone = "",
                Email = ""
            };
            db.Employees.Add(admin);
            db.SaveChanges();

            // Создаём пользователя для входа
            var user = new User
            {
                ID_Employees = admin.ID_Employees,
                Username = "admin",
                Password = "admin"
            };
            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
