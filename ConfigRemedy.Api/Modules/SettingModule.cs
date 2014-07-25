using ConfigRemedy.Api.Annotations;
using ConfigRemedy.Core;
using ConfigRemedy.Core.Modules;
using ConfigRemedy.Domain;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Raven.Client;
using Environment = ConfigRemedy.Domain.Environment;

namespace ConfigRemedy.Api.Modules
{
    [UsedImplicitly]
    public class SettingModule : BaseModule
    {
        private readonly IDocumentSession _session;

        public SettingModule(IDocumentSession session)
        {
            _session = session;

            Get["/applications/{appName}/settings/{settingKey}"] 
                = @params => GetSetting(@params.appName, @params.settingKey);

            Put["/applications/{appName}/settings/{settingKey}"]
                = @params => UpdateSetting(@params.appName, @params.settingKey);

            Delete["/applications/{appName}/settings/{settingKey}"] 
                = @params => DeleteSetting(@params.appName, @params.settingKey);

            Post["/applications/{appName}/settings"] 
                = @params => CreateSetting(@params.appName);


            // TODO: Move to another module

            Post["/applications/{appName}/settings/{envName}/"]
                = @params => CreateSettingOverride(@params.envName, @params.appName);

            Get["/applications/{appName}/settings/{envName}/{settingKey}"]
                = @params => GetSettingOverrideForEnvironment(@params.envName, @params.appName, @params.settingKey);

            Delete["/applications/{appName}/settings/{envName}/{settingKey}"] 
                = @params => DeleteSettingOverride(@params.envName, @params.appName, @params.settingKey);
        }

        private dynamic DeleteSettingOverride(string envName, string appName, string settingKey)
        {
            Guard.NotNullOrEmpty(() => envName, envName);
            Guard.NotNullOrEmpty(() => appName, appName);
            Guard.NotNullOrEmpty(() => settingKey, settingKey);

            var app = GetApplication(_session, appName);
            if (app == null)
                return AppNotFoundResponse();

            var env = GetEnvironment(_session, envName);
            if (env == null)
                return EnvironmentNotFoundResponse();

            var setting = app.GetSetting(settingKey);
            if (setting == null)
                return SettingNotFoundResponse();

            setting.RemoveOverride(envName);

            _session.Store(setting);
            _session.SaveChanges();

            return HttpStatusCode.NoContent;
        }

        private dynamic GetSettingOverrideForEnvironment(string envName, string appName, string settingKey)
        {
            Guard.NotNullOrEmpty(() => envName, envName);
            Guard.NotNullOrEmpty(() => appName, appName);
            Guard.NotNullOrEmpty(() => settingKey, settingKey);

           var app = GetApplication(_session, appName);
            if (app == null)
                return AppNotFoundResponse();

            var env = GetEnvironment(_session, envName);
            if (env == null)
                return EnvironmentNotFoundResponse();

            var setting = app.GetSetting(settingKey);
            if (setting == null)
                return SettingNotFoundResponse();

            return new TextResponse(setting.GetValueForEnvironment(envName));
        }

        private dynamic CreateSettingOverride(string envName, string appName/*, string settingKey*/)
        {
            Guard.NotNullOrEmpty(() => envName, envName);
            Guard.NotNullOrEmpty(() => appName, appName);
            //Guard.NotNullOrEmpty(() => settingKey, settingKey);

            var settingOverride = this.Bind<SettingOverride>();

            var app = GetApplication(_session, appName);
            if (app == null)
                return AppNotFoundResponse();

            var env = GetEnvironment(_session, envName);
            if (env == null)
                return EnvironmentNotFoundResponse();

            var setting = app.GetSetting(settingOverride.Key);
            if (setting == null)
                return SettingNotFoundResponse();

            setting.SetValueInEnvironment(envName, settingOverride);

            _session.Store(app);
            _session.SaveChanges();

            // NOTE: Try to generalize this in time
            var modulePath = ModulePath.Replace("{appName}", appName);

            return Negotiate
                .WithModel(setting) // TODO: Test for object structure
                .WithHeader("Location", string.Format("{0}/settings/{1}/{2}", modulePath, envName, setting.Key))
                .WithStatusCode(HttpStatusCode.Created);
        }


        private dynamic CreateSetting(string appName)
        {
            Guard.NotNullOrEmpty(() => appName, appName);

            var app = GetApplication(_session, appName);
            var setting = this.Bind<Setting>(s => s.Deleted, s => s.Overrides, s => s.History);

            if (app.HasSetting(setting.Key))
                return DuplicatesNotAllowedResponse();

            app.AddSetting(setting);

            _session.Store(app);
            _session.SaveChanges();

            return Negotiate
                .WithModel(setting) // TODO: Test for object structure
                .WithHeader("Location", string.Format("{0}/{1}", Request.Path, setting.Key))
                .WithStatusCode(HttpStatusCode.Created);
        }

        private dynamic DeleteSetting(string appName, string settingKey)
        {
            Guard.NotNullOrEmpty(() => appName, appName);
            Guard.NotNullOrEmpty(() => settingKey, settingKey);

            var app = GetApplication(_session, appName);
            app.DeleteSetting(settingKey);

            _session.Store(app);
            _session.SaveChanges();

            return HttpStatusCode.NoContent;
        }

        private dynamic UpdateSetting(string appName, string settingKey)
        {
            Guard.NotNullOrEmpty(() => appName, appName);
            Guard.NotNullOrEmpty(() => settingKey, settingKey);

            var app = GetApplication(_session, appName);

            if (!app.HasSetting(settingKey))
                return new NotFoundResponse();

            var setting = this.Bind<Setting>(s => s.Deleted, s => s.Overrides, s => s.History);
            if (app.HasSetting(setting.Key))
                return DuplicatesNotAllowedResponse();

            app.UpdateSetting(settingKey, setting);

            _session.Store(app);
            _session.SaveChanges();

            return HttpStatusCode.NoContent;
        }


        private dynamic GetSetting(string appName, string settingKey)
        {
            Guard.NotNullOrEmpty(() => appName, appName);
            Guard.NotNullOrEmpty(() => settingKey, settingKey);

            var app = GetApplication(_session, appName);

            if (!app.HasSetting(settingKey))
                return new NotFoundResponse();

            return app.GetSetting(settingKey);
        }

        private static Response SettingNotFoundResponse()
        {
            return new TextResponse(HttpStatusCode.NotFound, "Setting not found")
            {
                ReasonPhrase = "Setting not found"
            };
        }

        private static Response EnvironmentNotFoundResponse()
        {
            return new TextResponse(HttpStatusCode.NotFound, "Environment not found")
            {
                ReasonPhrase = "Environment not found"
            };
        }

        private static Response AppNotFoundResponse()
        {
            return new TextResponse(HttpStatusCode.NotFound, "Application not found")
            {
                ReasonPhrase = "Application not found"
            };
        }

        private static Response DuplicatesNotAllowedResponse()
        {
            return new TextResponse(HttpStatusCode.Forbidden, "Duplicates are not allowed")
            {
                ReasonPhrase = "Duplicates are not allowed"
            };
        }

        private static Application GetApplication(IDocumentSession session, string appName)
        {
            return session.Load<Application>("applications/" + appName);
        }

        private static Environment GetEnvironment(IDocumentSession session, string envName)
        {
            return session.Load<Environment>("environments/" + envName);
        }
    }
}