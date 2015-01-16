using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Chi.SocialNetwork
{
    // workaround to fix kendo grid requests to WebApi.
    // looks like Kendo.Mvc.UI.WebApiDataSourceRequestModelBinder v2014.3.1314.545 is no working with WebApi 2 or at least framework 4.5.
    // it's trying to use System.Web.Http, Version=5.0.0.0 instead of System.Web.Http, Version=5.2.2.0
    // then throws a FileLoadException exception on Global.asax.cs Application_Start() GlobalConfiguration.Configure(WebApiConfig.Register).
    // so I replicated it to use System.Web.Http, Version=5.2.2.0
    public class WebApiDataSourceRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            DataSourceRequest request = new DataSourceRequest();

            string sort, group, filter, aggregates;
            int currentPage;
            int pageSize;

            if (TryGetValue(bindingContext, GridUrlParameters.Sort, out sort))
            {
                request.Sorts = GridDescriptorSerializer.Deserialize<SortDescriptor>(sort);
            }

            if (TryGetValue(bindingContext, GridUrlParameters.Page, out currentPage))
            {
                request.Page = currentPage;
            }

            if (TryGetValue(bindingContext, GridUrlParameters.PageSize, out pageSize))
            {
                request.PageSize = pageSize;
            }

            if (TryGetValue(bindingContext, GridUrlParameters.Filter, out filter))
            {
                request.Filters = FilterDescriptorFactory.Create(filter);
            }

            if (TryGetValue(bindingContext, GridUrlParameters.Group, out group))
            {
                request.Groups = GridDescriptorSerializer.Deserialize<GroupDescriptor>(group);
            }

            if (TryGetValue(bindingContext, GridUrlParameters.Aggregates, out aggregates))
            {
                request.Aggregates = GridDescriptorSerializer.Deserialize<AggregateDescriptor>(aggregates);
            }

            bindingContext.Model = request;
            return true;
        }

        private bool TryGetValue<T>(ModelBindingContext bindingContext, string key, out T result)
        {
            var value = bindingContext.ValueProvider.GetValue(key);

            if (value == null)
            {
                result = default(T);

                return false;
            }

            result = (T)value.ConvertTo(typeof(T));

            return true;
        }
    }
}