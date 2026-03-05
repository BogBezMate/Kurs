using System.Windows;
using Microsoft.EntityFrameworkCore;
using Kurs.Data;
using Kurs.Models;

namespace Kurs
{
    public partial class App : Application
    {
        // Текущий авторизованный пользователь
        public static User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Автоматически применяем все миграции и создаём БД при первом запуске
            using var db = new CarServiceContext();
            db.Database.Migrate();         // создаёт БД и применяет миграции
            DatabaseSeeder.Seed(db);       // заполняет начальными данными
        }
    }
}
