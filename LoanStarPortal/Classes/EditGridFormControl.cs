using System;
using System.Collections;

namespace LoanStar.Common
{
    public class EditGridFormControl : AppControl
    {
        public Hashtable fields = new Hashtable();

        #region delegates
        public event CancelHandler OnCancel;
        public delegate void CancelHandler();
        public event SaveHandler OnSave;
        public delegate void SaveHandler(Object o,ArrayList logs);
        #endregion

        public virtual string ObjectName
        {
            get { return ""; }
            set { ;}
        }
        public virtual object EditObject
        {
            set { }
        }
        public virtual bool EnableValidation
        {
            get { return false; }
            set { ;}
        }
        public virtual void BindData()
        {
        }
        protected virtual void Save(Object o, ArrayList logs)
        {
            if(OnSave != null)
            {
                OnSave(o,logs);
            }
        }
        protected virtual void Cancel()
        {
            if (OnCancel != null)
            {
                OnCancel();
            }
        }
        protected bool IsFieldEditable(string fieldName)
        {
            bool res = true;
            if (fields.ContainsKey((fieldName)))
                res = !(bool)fields[fieldName];
            return res;
        }
    }

}
