using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtPencarianBarang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (string.IsNullOrEmpty(txtPencarianBarang.Text))
                    dataGridView1.DataSource = tokobarangBindingSource;
                else
                {
                    var query = from o in this.appData.toko_barang
                                where o.Nama_Barang.Contains(txtPencarianBarang.Text)
                                    || o.Tipe_Barang == txtPencarianBarang.Text
                                    || o.Stok_Barang == txtPencarianBarang.Text
                                    || o.Harga_Barang == txtPencarianBarang.Text
                                select o;
                    dataGridView1.DataSource = query.ToList();
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Apakah kamu yakin hapus data ini ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    tokobarangBindingSource.RemoveCurrent();
            }
        }

        private void btnPilihGambar_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                        pictureBox.Image = Image.FromFile(ofd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
            {
                panel.Enabled = true;
                txtNamaBarang.Focus();
                this.appData.toko_barang.Addtoko_barangRow(this.appData.toko_barang.Newtoko_barangRow());
                tokobarangBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tokobarangBindingSource.ResetBindings(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            panel.Enabled = true;
            txtNamaBarang.Focus();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            panel.Enabled = false;
            tokobarangBindingSource.ResetBindings(false);
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                tokobarangBindingSource.EndEdit();
                toko_barangTableAdapter.Update(this.appData.toko_barang);
                panel.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tokobarangBindingSource.ResetBindings(false);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'appData.toko_barang' table. You can move, or remove it, as needed.
            this.toko_barangTableAdapter.Fill(this.appData.toko_barang);
            tokobarangBindingSource.DataSource = this.appData.toko_barang;
        }
    }
}
