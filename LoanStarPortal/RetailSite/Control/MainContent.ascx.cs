using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal.RetailSite.Control
{
    public partial class MainPage : AppControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindPhoto();
        }
        private void BindPhoto()
        {
            string imgFile = CurrentUser.PhotoUploaded;
            if (!String.IsNullOrEmpty(imgFile) && CurrentUser.DisplayPhoto)
            {
                string pathToImage = Constants.USERPHOTOFOLDER + "/" + CurrentUser.PhotoUploaded;
                string fullPath = Server.MapPath(pathToImage);
                FileInfo fi = new FileInfo(fullPath);
                if (fi.Exists)
                {
                    imgPhoto.ImageUrl = pathToImage;
                }
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("RetailPage.aspx?control=input");
        }
    }
}