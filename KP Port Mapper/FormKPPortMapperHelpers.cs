using KP_Port_Mapper.Helpers;
using System.Linq;
using System.Windows.Forms;

internal static class FormKPPortMapperHelpers
{

    private static void ConfigureAndAlignColumns(DataGridView dataGridView, params (string DataPropertyName, string Name, int Width)[] columns)
    {
        dataGridView.Columns.AddRange(columns
            .Select(column => new DataGridViewTextBoxColumn
            {
                DataPropertyName = column.DataPropertyName,
                Name = column.Name,
                Width = column.Width,
                AutoSizeMode = column.Width == 0 ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.NotSet,
                ReadOnly = true
            })
            .ToArray());

        DataGridExtensions.AlignColumns(dataGridView.Columns);
    }
}