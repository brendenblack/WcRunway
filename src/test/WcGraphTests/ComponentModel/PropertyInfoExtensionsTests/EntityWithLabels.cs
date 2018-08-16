using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.ComponentModel.DataAnnotations.Schema;

namespace WcGraphTests.ComponentModel.PropertyInfoExtensionsTests
{
    public class EntityWithLabels
    {
        [GraphLabel]
        public string PropertyWithAttributeAndNoNameSpecified { get; set; }

        [GraphLabel("my_label")]
        public string PropertyWithAttirbuteAndNameSpecified { get; set; }

        public string PropertyWithoutAttribute { get; set; }
    }
}
