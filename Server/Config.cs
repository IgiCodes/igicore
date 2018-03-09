using CitizenFX.Core.Native;

namespace IgiCore.Server
{
    public static class Config
    {
        public static string MySqlConnString => API.GetConvar("mysql_connection", string.Empty);
    }
}
