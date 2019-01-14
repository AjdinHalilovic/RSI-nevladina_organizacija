using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Helpers.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));
            
            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult.ToString().Length == 0)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var dateString = valueProviderResult.FirstValue;
            var currentCultureInfoName = CultureInfo.CurrentCulture.Name;

            var dateFormat = string.Empty;
            foreach (var item in Configuration.SupportedSystemLanguages)
            {
                if (!item.Name.Equals(currentCultureInfoName, StringComparison.OrdinalIgnoreCase))
                    continue;
                var dateLength = valueProviderResult.ToString().Length;
                dateFormat =dateLength < 11 ? item.DateFormat :  item.DateTimeFormat;
                break;
            }

            if (!DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"{bindingContext.ModelName} should be in format '{dateFormat}'");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(date);
            return Task.CompletedTask;
        }
    }
}