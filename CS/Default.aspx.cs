using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxUploadControl;

public partial class _Default : System.Web.UI.Page {
    public Dictionary<object, byte[]> Files {
        get {
            if (Session["files"] == null)
                Session["files"] = new Dictionary<object, byte[]>();
            return Session["files"] as Dictionary<object, byte[]>;
        }
        set {
            Session["files"] = value;
        }
    }

    protected void Page_Init(object sender, EventArgs e) {
        ASPxGridView1.DataSource = Enumerable.Range(0, 10).Select(i => new {
            ID = i,
            PersonName = "Name " + i,
            FileName = ""
        });
        ASPxGridView1.DataBind();

    }
    protected void ASPxUploadControl1_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
        var name = e.UploadedFile.FileName;
        e.CallbackData = name;

        if (Files.ContainsKey(hiddenField["visibleIndex"]))
            Files[hiddenField["visibleIndex"]] = e.UploadedFile.FileBytes;
        else
            Files.Add(hiddenField["visibleIndex"], e.UploadedFile.FileBytes);
    }
    protected void ASPxGridView1_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e) {
        //update the datasource here using the uploaded files
        //clear the dictionary after
        Files.Clear();
    }
    protected void ASPxGridView1_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e) {
        if (e.Exception.GetType() == typeof(NotSupportedException))
            e.ErrorText = "Online data modification is not supported. Download the example and implement your logic in the BatchUpdate event handler.";
    }
    protected void ASPxGridView1_BeforeGetCallbackResult(object sender, EventArgs e) {
        Files.Clear();
    }
}