using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.Entity
{
    public enum ControlType
    {
        None,
        TextBox,
        ComboBox,
        PasswordBox,
        DatePicker,
        TreeSelect,
        MultiComboBox,
        MultiTreeSelect,
        CheckBox,
        ToggleButton,

        IntegerUpDown = 100,
        LongUpDown,
        DoubleUpDown,
        DecimalUpDown,
        DateTimeUpDown,

        Query = 200,
        Submit,
        Add,
        Delete,
    }
}
