using System.IO;
using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Api.Infrastructure;
using ConfigRemedy.Api.Infrastructure.Settings;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Raven.Client;

namespace ConfigRemedy.Api
{
    using Nancy;

    [UsedImplicitly]
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            CustomPipelines.Configure(pipelines);
        }
        
        /// <summary>
        /// Belived to behave as "singletons"
        /// </summary>
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register(RavenFactory.Create());
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var documentStore = container.Resolve<IDocumentStore>();
            container.Register(documentStore.OpenSession());
        }

        public static void ConfigureLogging()
        {
            if (LogManager.Configuration != null)
            {
                return;
            }

            var nlogConfig = new LoggingConfiguration();


            var fileTarget = new FileTarget
            {
                FileName = Path.Combine(Settings.LogPath, "${shortdate}.txt"),
                Layout = new SimpleLayout("${longdate}|${threadid}|${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"),
            };

            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = new SimpleLayout("${date} [${level}] ${logger} ${message}${onexception:${newline}${exception:format=tostring}}"),
                UseDefaultRowHighlightingRules = true,
            };

            nlogConfig.LoggingRules.Add(new LoggingRule("Raven.*", LogLevel.Warn, fileTarget));
            nlogConfig.LoggingRules.Add(new LoggingRule("Raven.*", LogLevel.Warn, consoleTarget) { Final = true });

            nlogConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, fileTarget));
            nlogConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            
            nlogConfig.AddTarget("debugger", new AsyncTargetWrapper(fileTarget));
            nlogConfig.AddTarget("console", consoleTarget);
            LogManager.Configuration = nlogConfig;
        }
    }
}