using Newtonsoft.Json;

namespace OrdenesDeCompras.Extensions
{
    public static class SessionExtension
    {


        //Guardar datos en la sesión
        //session es para indicar la clase que se va extender
        //key y value se utilizan para guardar los datos
        public static void SetObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }


        //Recuperar datos de la sesión
        //Se indica de que clase se extiende(session) y la clave(key) para recuperar los datos de la sesión
        public static T GetObject<T> (this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
    }
}
