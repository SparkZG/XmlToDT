using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using Microsoft.Win32;
using DevExpress.Xpf.Grid;

namespace XmlToDT
{
    public class XmlToDTViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private DataTable detailTable = new DataTable();
        public DataTable DetailTable
        {
            get
            {
                return detailTable;
            }
            set
            {
                detailTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetailTable"));
            }
        }

        private string detailComment = string.Empty;
        public string DetailComment
        {
            get
            {
                return detailComment;
            }
            set
            {
                detailComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetailComment"));
            }
        }

        private int selectIndex = 0;
        public int SelectIndex
        {
            get
            {
                return selectIndex;
            }
            set
            {
                selectIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectIndex"));
            }
        }

        private string header = string.Empty;
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Header"));
            }
        }

        private List<GridColumn> listColumn = new List<GridColumn> { };
        public List<GridColumn> ListColumn
        {
            get
            {
                return listColumn;
            }
            set
            {
                listColumn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListColumn"));
            }
        }

        public void AddRow()
        {
            if (SelectIndex < 0)
            {
                return;
            }
            DataRow dr = DetailTable.NewRow();
            DetailTable.Rows.InsertAt(dr, SelectIndex);
        }

        public void RemoveRow()
        {
            if (SelectIndex < 0)
            {
                return;
            }
            DetailTable.Rows.RemoveAt(SelectIndex);
        }

        public XmlToDTViewModel(string headerNmae)
        {
            Header = DealXML.GetComment(headerNmae);
            DetailComment = DealXML.GetDetailComment(headerNmae);
        }


    }
}
