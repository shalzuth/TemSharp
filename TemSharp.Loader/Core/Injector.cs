using System;

namespace TemSharp.Loader.Core
{
    public static class Injector
    {
        public static void Inject(String GameName, Byte[] Assembly, String Namespace, String Class, String Method)
        {
            var injector = new SharpMonoInjector.Injector(GameName);
            injector.Inject(Assembly, Namespace, Class, Method);
        }
    }
}
