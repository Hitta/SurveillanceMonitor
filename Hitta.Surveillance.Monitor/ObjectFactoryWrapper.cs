using System;
using System.Windows.Forms;
using Spring.Context;
using Spring.Context.Support;

namespace Hitta.Surveillance.Monitor
{
    public class ObjectFactoryWrapper
    {
        readonly IApplicationContext context;

        public ObjectFactoryWrapper()
        {
            context = ContextRegistry.GetContext();
        }

        public T GetObject<T>(string name)
        {
            try
            {
                return (T)context.GetObject(name, typeof(T));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Failed to load object with name: '{0}' the error was: {1}", name, ex.Message));

                throw;
            }
            
        }

        public T GetObject<T>(string name, object[] args)
        {
            try
            {
                return (T)context.GetObject(name, typeof(T), args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Failed to load object with name: '{0}' the error was: {1}", name, ex.Message));
                throw;
            }
        }

        public ObjectFactoryWrapper RegisterSingleton(string name, object newObject)
        {
            var configurableApplicationContext = (IConfigurableApplicationContext)context;

            configurableApplicationContext.ObjectFactory.RegisterSingleton(name, newObject);

            return this;
        }
    }
}
