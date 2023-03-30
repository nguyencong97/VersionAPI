using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.Cors;

namespace VersionAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            #region Config routes
            //config.Routes.MapHttpRoute("DefaultAPIWithID", "api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultAPIWithAction", "api/{controller}/{action}");

            //config.Routes.MapHttpRoute("DefaultAPIGet", "api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            //config.Routes.MapHttpRoute("DefaultAPIPost", "api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
            //config.Routes.MapHttpRoute("DefaultAPIPut", "api/{controller}", new { action = "Put" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) });
            //config.Routes.MapHttpRoute("DefaultAPIDelete", "api/{controller}", new { action = "Delete" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });
            #endregion
            //config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            //DateTime for params
            config.BindParameter(typeof(DateTime), new DateTimeModelBinder());
            config.BindParameter(typeof(DateTime?), new DateTimeModelBinder());
            //DateTime for JSON format
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new MyDateFormatConverter());

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);


            config.MessageHandlers.Add(new TokenValidationHandler());

        }

        private static CultureInfo culture = new CultureInfo("de-CH");

        #region datetime json format
        public class MyDateFormatConverter : DateTimeConverterBase
        {
            public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
            {
                try
                {
                    return DateTime.Parse(reader.Value.ToString(), culture);
                }
                catch { return null; }
            }

            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
            {
                writer.WriteValue(((DateTime)value).ToString(culture));
            }
        }
        #endregion

        #region datetime params format
        public class DateTimeModelBinder : IModelBinder
        {
            public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
            {
                ValidateBindingContext(bindingContext);
                if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName) ||
                    !CanBindType(bindingContext.ModelType))
                {
                    return false;
                }
                var modelName = bindingContext.ModelName;
                var attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;
                try
                {
                    bindingContext.Model = DateTime.Parse(attemptedValue, culture);
                }
                catch (FormatException e)
                {
                    bindingContext.ModelState.AddModelError(modelName, e);
                }
                return true;
            }

            private static void ValidateBindingContext(ModelBindingContext bindingContext)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException("bindingContext");
                }

                if (bindingContext.ModelMetadata == null)
                {
                    throw new ArgumentException("ModelMetadata cannot be null", "bindingContext");
                }
            }

            public static bool CanBindType(Type modelType)
            {
                return modelType == typeof(DateTime) || modelType == typeof(DateTime?);
            }
        }
        public class DateTimeModelBinderProvider : ModelBinderProvider
        {
            readonly DateTimeModelBinder binder = new DateTimeModelBinder();
            public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
            {
                if (DateTimeModelBinder.CanBindType(modelType))
                {
                    return binder;
                }
                return null;
            }
        }
        #endregion
    }
}
