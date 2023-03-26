using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProfesionalesMVC.Controllers {
    public static class Extension {

        public static async Task<List<T>> SearchBy<T> (this DbContext cnt, string field, string search, DbSet<T> list) where T : class {
            var asynList = await list.ToListAsync();

            return asynList.Where(x => cnt.ContainsValueProperty<T>(x, field, search)).ToList();
        }
        public static bool ContainsValueProperty<T> (this DbContext cnt, T obj, string propertyName, string shouldValue) {

            PropertyInfo? prop = typeof(T).GetProperty(propertyName);

            string? value = prop?.GetValue(obj, null)?.ToString();

            return (value ?? "").Contains(shouldValue);

        }


    }
}
