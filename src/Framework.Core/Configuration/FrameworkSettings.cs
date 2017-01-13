namespace Framework.Core.Configuration
{
    internal static class FrameworkSettings
    {
        internal static class Configuration
        {
            internal static Models DataModelConfig = null;
            internal static ViewModels ViewModelConfig = null;
            internal static Services ServiceConfig = null;

            static Configuration()
            {
                DataModelConfig = new Models();
                ViewModelConfig = new ViewModels();
                ServiceConfig = new Services();
            }

            internal sealed class Models
            {
                public string Assembly { get; set; }
                public string NamespaceModels { get; set; }
                public string Suffix { get; set; }
            }

            internal sealed class ViewModels
            {
                public string Assembly { get; set; }
                public string Suffix { get; set; }
            }

            internal sealed class Services
            {
                public string Assembly { get; set; }
            }
        }
    }
}
