using System.Text.Json;

namespace MiTienda.Utilities
{
    public static class SessionExtensions
    {
    //Toma cualquier objeto de C# (T value) y lo convierte a una cadena de texto
    // Set para guardar y Get para devolver
        public static void Set<T>(this ISession session, string key, T Value)
        {
            session.SetString(key, JsonSerializer.Serialize(Value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
