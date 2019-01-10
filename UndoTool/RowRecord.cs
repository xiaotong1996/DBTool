using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB.UndoTool
{
    class RowRecord
    {
        public int RowIndex { set; get; }
        public string Tag { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public string Length { set; get; }
        public bool Index { set; get; }
    }
}
