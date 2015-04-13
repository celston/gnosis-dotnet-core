using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Gnosis.Mvc
{
    public static class Extensions
    {
        public static MvcHtmlString ControlLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder tb = new TagBuilder("label");
            tb.Attributes.Add("for", StringUtility.LowercaseFirst(html.NameFor(expression).ToString()));
            tb.AddCssClass("control-label");

            tb.InnerHtml = html.DisplayNameFor(expression).ToString();
            
            return new MvcHtmlString(tb.ToString());
        }

        public static MvcHtmlString PrimaryModelName<TModel>(this HtmlHelper<TModel> html)
        {
            object o = html.ViewData.Model;
            ValidationUtility.AssertNotNull(o, new ArgumentNullException("ViewData.Model"));
            
            return new MvcHtmlString(StringUtility.LowercaseFirst(o.GetType().Name));
        }

        public static MvcHtmlString StandardizedNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return new MvcHtmlString(StringUtility.LowercaseFirst(html.NameFor(expression).ToString()));
        }

        public static MvcHtmlString NgTextInput<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string placeholder = "", string ng_model = null, string ng_required = null, bool ng_trim = true, bool inRepeat = false)
        {
            string name = StringUtility.LowercaseFirst(html.NameFor(expression).ToString());
            if (string.IsNullOrWhiteSpace(ng_model))
            {
                ng_model = string.Format("data.{0}", name);
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            TagBuilder tb = new TagBuilder("input");

            if (inRepeat)
            {
                tb.Attributes.Add("ng-blur", string.Format("blurred.{0}[$index] = true", name));
                ng_model += "[$index]";
                name += "_{{$index}}";
            }
            else
            {
                tb.Attributes.Add("name", name); 
                tb.Attributes.Add("ng-blur", string.Format("blurred.{0} = true", name));
            }
            
            tb.AddCssClass("form-control");
            tb.Attributes.Add("type", "text");
            tb.Attributes.Add("placeholder", placeholder);
            tb.Attributes.Add("ng-model", ng_model);
            
            if (ng_required != null)
            {
                tb.Attributes.Add("ng-required", ng_required);
            }
            else if (metadata.IsRequired)
            {
                tb.Attributes.Add("required", "required");
            }

            if (!ng_trim)
            {
                tb.Attributes.Add("ng-trim", "false");
            }
            
            return new MvcHtmlString(tb.ToString());
        }

        public static MvcHtmlString NgNumberInput<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, decimal min = 0.01m, string placeholder = "")
        {
            object o = html.ViewData.Model;
            ValidationUtility.AssertNotNull(o, new ArgumentNullException("ViewData.Model"));
            
            string formName = StringUtility.LowercaseFirst(o.GetType().Name);
            string name = StringUtility.LowercaseFirst(html.NameFor(expression).ToString());
            string model = string.Format("{0}.{1}", formName, name);

            TagBuilder tb = new TagBuilder("input");

            tb.AddCssClass("form-control");
            tb.Attributes.Add("type", "number");
            tb.Attributes.Add("name", name);
            tb.Attributes.Add("ng-model", model);
            tb.Attributes.Add("ng-blur", string.Format("blurred.{0} = true", name));
            tb.Attributes.Add("min", min.ToString());
            tb.Attributes.Add("placeholder", placeholder);

            return new MvcHtmlString(tb.ToString());
        }
    }
}
