using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using WcGraph.ComponentModel;
using Xunit;

namespace WcGraphTests.ComponentModel.PropertyInfoExtensionsTests
{
    public class GetLabelName_Should
    {
        [Fact]
        public void ReturnPropertyNameInSnakeCaseWhenNoNameSpecified()
        {
            var entity = new EntityWithLabels();
            var property = entity.GetType().GetProperty(nameof(entity.PropertyWithAttributeAndNoNameSpecified));

            var labelName = property.GetLabelName();

            labelName.ShouldBe("property_with_attribute_and_no_name_specified");
        }

        [Fact]
        public void ReturnSpecifiedName()
        {
            var entity = new EntityWithLabels();
            var property = entity.GetType().GetProperty(nameof(entity.PropertyWithAttirbuteAndNameSpecified));

            var labelName = property.GetLabelName();

            labelName.ShouldBe("my_label");
        }

        [Fact]
        public void ReturnPropertyNameInSnakeCaseWhenNoAttribute()
        {
            var entity = new EntityWithLabels();
            var property = entity.GetType().GetProperty(nameof(entity.PropertyWithoutAttribute));

            var labelName = property.GetLabelName();

            labelName.ShouldBe("property_without_attribute");
        }
    }
}
