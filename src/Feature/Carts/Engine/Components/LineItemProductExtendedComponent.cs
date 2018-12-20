using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Feature.Carts.Engine
{
    public class LineItemProductExtendedComponent : Component
    {
        public LineItemProductExtendedComponent()
        {
            Brand = string.Empty;
            ParentCategoryList = string.Empty;
        }

        public string Brand { get; set; }

        public string ParentCategoryList { get; set; }
    }
}

