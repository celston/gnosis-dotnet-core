using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using Gnosis.Entities.Mvc.Models;
using Gnosis.Entities.Examples.Widgets;

namespace Gnosis.Entities.Examples.WebApplication.Areas.Widgets.Models
{
    public class WidgetCreateRequestModel : EntityCreateRequestModel, IWidgetCreateRequest
    {
        public override Guid? Author
        {
            get { return null; }
        }

        [DisplayName("String One")]
        [Required(ErrorMessage =  "String One is required.")]
        public string S1 { get; set; }

        [DisplayName("String Two: List")]
        public IEnumerable<string> S2 { get; set; }

        [DisplayName("String Three")]
        [Required]
        public string S3
        {
            get { throw new NotImplementedException(); }
        }
    }
}