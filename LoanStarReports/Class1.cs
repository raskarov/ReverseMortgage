using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace LoanStarReports
{
    public class Class1
    {
        public Class1() { }
        public DataSet testDS = null;
        public DataTable testTbl = null;

        public int x = 0;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
    }
}
