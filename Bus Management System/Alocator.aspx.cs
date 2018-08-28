using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bus_Management_System
{
    public partial class Alocator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Call FillGridView Method
                FillGridView();
            }
        }

        /// <summary>
        /// Fill record into GridView
        /// </summary>
        public void FillGridView()
        {
            try
            {
                string cnString = ConfigurationManager.ConnectionStrings["BusData"].ConnectionString;
                SqlConnection con = new SqlConnection(cnString);
                BusinessLayer.adap = new SqlDataAdapter("select Employee_Id, Operation, FlightNb, Pax, RadioGate, RadioNeon, Created from Operations", con);
                SqlCommandBuilder bui = new SqlCommandBuilder(BusinessLayer.adap);
                BusinessLayer.dt = new DataTable(BusinessLayer.GetCurrentOp());
                BusinessLayer.adap.Fill(BusinessLayer.dt);
                GridView1.DataSource = BusinessLayer.dt;
                GridView1.DataBind();
            }
            catch
            {
                Response.Write("<script> alert('Connection String Error...') </script>");
            }
        }
        /// <summary>
        /// Edit record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void editRecord(object sender, GridViewEditEventArgs e)
        {
            // Get the image path for remove old image after update record
            Image imgEditPhoto = GridView1.Rows[e.NewEditIndex].FindControl("imgPhoto") as Image;
            BusinessLayer.imgEditPath = imgEditPhoto.ImageUrl;
            // Get the current row index for edit record
            GridView1.EditIndex = e.NewEditIndex;
            FillGridView();
        }

        /// <summary>
        /// Cancel the operation (e.g. edit)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cancelRecord(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            FillGridView();
        }

        /// <summary>
        /// Add new row into DataTable if no record found in Table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddNewRecord(object sender, EventArgs e)
        {
            try
            {
                if (BusinessLayer.dt.Rows.Count > 0)
                {
                    GridView1.EditIndex = -1;
                    GridView1.ShowFooter = true;
                    FillGridView();
                }
                else
                {
                    GridView1.ShowFooter = true;
                    DataRow dr = BusinessLayer.dt.NewRow();
                    dr["name"] = "0";
                    dr["age"] = 0;

                    dr["salary"] = 0;
                    dr["country"] = "0";
                    dr["city"] = "0";
                    dr["photopath"] = "0";
                    BusinessLayer.dt.Rows.Add(dr);
                    GridView1.DataSource = BusinessLayer.dt;
                    GridView1.DataBind();
                    GridView1.Rows[0].Visible = false;
                }
            }
            catch
            {
                Response.Write("<script> alert('Row not added in DataTable...') </script>");
            }
        }

        /// <summary>
        /// Cancel new added record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddNewCancel(object sender, EventArgs e)
        {
            GridView1.ShowFooter = false;
            FillGridView();
        }

        /// <summary>
        /// Insert New Record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InsertNewRecord(object sender, EventArgs e)
        {
            try
            {
                string strName = BusinessLayer.dt.Rows[0]["name"].ToString();
                if (strName == "0")
                {
                    BusinessLayer.dt.Rows[0].Delete();
                    BusinessLayer.adap.Update(BusinessLayer.dt);
                }
                TextBox txtName = GridView1.FooterRow.FindControl("txtNewName") as TextBox;
                TextBox txtAge = GridView1.FooterRow.FindControl("txtNewAge") as TextBox;
                TextBox txtSalary = GridView1.FooterRow.FindControl("txtNewSalary") as TextBox;
                TextBox txtCountry = GridView1.FooterRow.FindControl("txtNewCountry") as TextBox;
                TextBox txtCity = GridView1.FooterRow.FindControl("txtNewCity") as TextBox;
                FileUpload fuPhoto = GridView1.FooterRow.FindControl("fuNewPhoto") as FileUpload;
                Guid FileName = Guid.NewGuid();
                fuPhoto.SaveAs(Server.MapPath("~/Images/" + FileName + ".png"));
                DataRow dr = BusinessLayer.dt.NewRow();
                dr["name"] = txtName.Text.Trim();
                dr["age"] = txtAge.Text.Trim();
                dr["salary"] = txtSalary.Text.Trim();
                dr["country"] = txtCountry.Text.Trim();
                dr["city"] = txtCity.Text.Trim();
                dr["photopath"] = "~/Images/" + FileName + ".png";
                BusinessLayer.dt.Rows.Add(dr);
                BusinessLayer.adap.Update(BusinessLayer.dt);
                GridView1.ShowFooter = false;
                FillGridView();
            }
            catch
            {
                Response.Write("<script> alert('Record not added...') </script>");
            }

        }
        /// <summary>
        /// Update the record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void updateRecord(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                TextBox txtName = GridView1.Rows[e.RowIndex].FindControl("txtName") as TextBox;
                TextBox txtAge = GridView1.Rows[e.RowIndex].FindControl("txtAge") as TextBox;
                TextBox txtSalary = GridView1.Rows[e.RowIndex].FindControl("txtSalary") as TextBox;
                TextBox txtCountry = GridView1.Rows[e.RowIndex].FindControl("txtCountry") as TextBox;
                TextBox txtCity = GridView1.Rows[e.RowIndex].FindControl("txtCity") as TextBox;
                FileUpload fuPhoto = GridView1.Rows[e.RowIndex].FindControl("fuPhoto") as FileUpload;
                Guid FileName = Guid.NewGuid();
                if (fuPhoto.FileName != "")
                {
                    fuPhoto.SaveAs(Server.MapPath("~/Images/" + FileName + ".png"));
                    BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["photopath"] = "~/Images/" + FileName + ".png";
                    File.Delete(Server.MapPath(BusinessLayer.imgEditPath));
                }
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["name"] = txtName.Text.Trim();
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["age"] = Convert.ToInt32(txtAge.Text.Trim());
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["salary"] = Convert.ToInt32(txtSalary.Text.Trim());
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["country"] = txtCountry.Text.Trim();
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex]["city"] = txtCity.Text.Trim();
                BusinessLayer.adap.Update(BusinessLayer.dt);
                GridView1.EditIndex = -1;
                FillGridView();
            }
            catch
            {
                Response.Write("<script> alert('Record updation fail...') </script>");
            }
        }

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                BusinessLayer.dt.Rows[GridView1.Rows[e.RowIndex].RowIndex].Delete();
                BusinessLayer.adap.Update(BusinessLayer.dt);
                // Get the image path for removing deleted's record image from server folder

                Image imgPhoto = GridView1.Rows[e.RowIndex].FindControl("imgPhoto") as Image;
                File.Delete(Server.MapPath(imgPhoto.ImageUrl));
                FillGridView();
            }
            catch
            {
                Response.Write("<script> alert('Record not deleted...') </script>");
            }
        }
    }
}