using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLogin
{
    public partial class MasterProduct : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VHM5NEN\SQLEXPRESS;Initial Catalog=MyPractice;Integrated Security=True;");
        public MasterProduct()
        {
            InitializeComponent();
        }
        DataClasses1DataContext db = new DataClasses1DataContext();
        private void btnSave_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtProductID.Text);
            string item = txtItem.Text;
            string design = txtDesign.Text;
            string color = cbColor.Text;
            DateTime expiredDate = DateTime.Parse(dateExpired.Text);
            var data = new TB_M_PRODUCT
            {
                ID = id,
                itemName = item,
                color = color,
                design = design,
                expiredDate = expiredDate
            };
            db.TB_M_PRODUCTs.InsertOnSubmit(data);
            db.SubmitChanges();
            MessageBox.Show("Save Successfully");
            txtDesign.Clear();
            txtItem.Clear();
            cbColor.Items.Clear();
            LoadData();
        }

        void LoadData()
        {
            var st = from tb in db.TB_M_PRODUCTs select tb;
            dt.DataSource = st;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var st = from s in db.TB_M_PRODUCTs where s.itemName == txtSearchItem.Text || s.design == txtSearchDesign.Text select s;
            dt.DataSource = st;
        }

        private void MasterProduct_Load_1(object sender, EventArgs e)
        {
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select isnull(max (cast (ID as int)),0) +1 from TB_M_PRODUCT", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            txtProductID.Text = dt.Rows[0][0].ToString();
            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var delete = from s in db.TB_M_PRODUCTs where s.itemName == txtSearchItem.Text select s;
            foreach (var t in delete)
            {
                db.TB_M_PRODUCTs.DeleteOnSubmit(t);
            }
            db.SubmitChanges();
            MessageBox.Show("Delete Succesfully");
            txtSearchDesign.Clear();
            txtSearchItem.Clear();
            LoadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string item = txtSearchItem.Text;
            string design = txtSearchDesign.Text;
            string color = cbColor.Text;
            DateTime expiredDate = DateTime.Parse(dateExpired.Text);

            var st = (from s in db.TB_M_PRODUCTs where s.ID == int.Parse(txtProductID.Text) select s).First();

            st.itemName = item;
            st.color = color;
            st.design = design;
            st.expiredDate = expiredDate;
            db.SubmitChanges();

            MessageBox.Show("Update Succesfuly");
            txtDesign.Clear();
            txtItem.Clear();
            cbColor.SelectedIndex = -1;
        }
    }
}
