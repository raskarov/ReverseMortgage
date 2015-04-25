using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration
{
    public partial class UploadPhoto : System.Web.UI.Page
    {
        private const string CURRENTUSER = "currentuser";
        private const string CANCELEDIT = "<script type=\"text/javascript\">EndEdit()</script>";

        AppUser user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = Session[CURRENTUSER] as AppUser;
            BindData();
        }
        private void BindData()
        { 
            string photoImage = CheckPhoto();
            if (String.IsNullOrEmpty(photoImage))
            {
                trPhotoImage.Visible = false;
                lblUploadPhoto.Text = "Upload photo:";
            }
            else
            {
                trPhotoImage.Visible = true;
                lblUploadPhoto.Text = "Change photo:";
                imgPhoto.ImageUrl = photoImage;
            }
            cbDisplayPhoto.Checked = user.DisplayPhoto;
        }
        private string CheckPhoto()
        {
            string res = String.Empty;
            if (!String.IsNullOrEmpty(user.PhotoUploaded))
            {
                res = Constants.USERPHOTOFOLDER + "/" + user.PhotoUploaded;
            }
            string fullPath = Server.MapPath(res);
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Exists)
            {
                res = String.Empty;
            }            
            return res;
        }       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveImage();
            CloseWindow();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWindow();
        }
        private void CloseWindow()
        {
            InjectScript.Text = CANCELEDIT;
        }
        private void SaveImage()
        {
            if (UserPhoto.HasFile && UserPhoto.PostedFile.ContentLength > 0)
            {
                string fileName = GetFileName(UserPhoto.FileName);
                string str = Server.MapPath(Constants.USERPHOTOFOLDER) + "\\" + fileName;
                UserPhoto.SaveAs(str);
                user.PhotoUploaded = fileName;
                user.DisplayPhoto = cbDisplayPhoto.Checked;
                Session[CURRENTUSER]=user;
            }
        }
        private string GetFileName(string sourceFileName)
        {
            return (Guid.NewGuid()).ToString() + "."+ sourceFileName.Substring(sourceFileName.LastIndexOf('.') + 1);
        }

    }
}
