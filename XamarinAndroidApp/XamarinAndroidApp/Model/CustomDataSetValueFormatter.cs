using System;
using System.Collections.Generic;
using System.Text;
using UltimateXF.Widget.Charts.Models.Formatters;

namespace XamarinAndroidApp.Model
{
    class CustomDataSetValueFormatter : IDataSetValueFormatter
    {
        public string GetFormattedValue(float value, int dataSetIndex)
        {
            return value.ToString("N2") + "$";
        }
    }
}
