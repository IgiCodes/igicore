using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IgiCore.Models;

namespace IgiCore.Server.Extentions
{
    public static class ModelExtentions
    {
        public static IEnumerable<T> NotDeleted<T>(this IEnumerable<T> source) where T : IdentityModel => source.Where(c => c.Deleted == null);

        public static async Task SoftDelete<T>(this T model) where T : IdentityModel
        {
            T dbModel = Server.Db.Set<T>().NotDeleted().FirstOrDefault(r => r.Id == model.Id);
            if (dbModel == null) throw new ArgumentNullException(nameof(model));
            dbModel.Deleted = DateTime.UtcNow;
            await Server.Db.SaveChangesAsync();
        }
    }
}
