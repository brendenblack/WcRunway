using System;
using System.Collections.Generic;
using System.Text;

namespace WcGraph.ComponentModel
{
    public enum LabelBehaviour
    {
        /// <summary>
        /// Require that labels for node entities be explicitly annotated with <see cref="GraphLabelAttribute"/>
        /// </summary>
        OPT_IN,
        /// <summary>
        /// Assume that all properties of the node entity should be a label, unless annotated with <see cref="NotMappedAttribute"/>
        /// </summary>
        OPT_OUT
    }
}
