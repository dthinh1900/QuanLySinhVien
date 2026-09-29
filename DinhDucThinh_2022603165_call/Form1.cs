using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace DinhDucThinh_2022603165_call
{
    public partial class Form1 : Form
    {

        private List<SinhVien> sinhViens;
        private int iD;


        public Form1()
        {
            InitializeComponent();
        }
        private List<SinhVien> GetSinhViens()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:12345/api/")
            };

            var response = client.GetAsync("SinhVien").Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<SinhVien>>().Result;
            }

            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sinhViens = GetSinhViens();
            dgvdata.DataSource = sinhViens;

        }
        private bool Add(SinhVien sv)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:12345/api/")
            };
            var response = client.PostAsJsonAsync("SinhVien", sv).Result;
            return response.IsSuccessStatusCode;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMasv.Text) ||
                string.IsNullOrWhiteSpace(txtTensv.Text) ||
                string.IsNullOrWhiteSpace(txtLop.Text) ||
                string.IsNullOrWhiteSpace(txtDiemtb.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo");
                return;
            }
            SinhVien sv = new SinhVien()
            {
                MaSV = txtMasv.Text,
                TenSV = txtTensv.Text,
                Lop = txtLop.Text,
                DiemTB = double.Parse(txtDiemtb.Text)
            };
            if (Add(sv))
            {
                MessageBox.Show("Đã thêm sinh viên thành công", "Thông báo");
                sinhViens = GetSinhViens();
                dgvdata.DataSource = sinhViens;
            }
            if (sinhViens.Any(x => x.MaSV == sv.MaSV))
            {
                MessageBox.Show("Mã sinh viên đã tồn tại!", "Thông báo");
                return;
            }
            else
            {
                MessageBox.Show("Không thêm được sinh viên!", "Thông báo");
            }

        }

        private bool Update(SinhVien sv)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:12345/api/")
            };

            var response = client.PutAsJsonAsync("SinhVien", sv).Result;

            return response.IsSuccessStatusCode;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMasv.Text) ||
        string.IsNullOrWhiteSpace(txtTensv.Text) ||
        string.IsNullOrWhiteSpace(txtLop.Text) ||
        string.IsNullOrWhiteSpace(txtDiemtb.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo");
                return;
            }



            SinhVien sv = new SinhVien()
            {
                MaSV = txtMasv.Text,
                TenSV = txtTensv.Text,
                Lop = txtLop.Text,
                DiemTB = double.Parse(txtDiemtb.Text)
            };
            if (!sinhViens.Any(x => x.MaSV == sv.MaSV))
            {
                MessageBox.Show("Mã sinh viên không tồn tại!", "Thông báo");
                return;
            }
            if (Update(sv))
            {
                sinhViens = GetSinhViens();
                dgvdata.DataSource = sinhViens;
                MessageBox.Show("Sửa thành công");
            }
            else
            {
                MessageBox.Show("Không sửa được sinh viên!");
            }

        }

        private bool Delete(string maSV)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:12345/api/")
            };
            var response = client.DeleteAsync($"SinhVien?id={maSV}").Result;
            return response.IsSuccessStatusCode;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSV = txtMasv.Text;
            if (!sinhViens.Any(x => x.MaSV == maSV))
            {
                MessageBox.Show("Mã sinh viên Không tồn tại!", "Thông báo");
                return;
            }
            if (MessageBox.Show(
        "Bạn có chắc chắn muốn xóa sinh viên này?", "Thông báo",
        MessageBoxButtons.OKCancel,
        MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Delete(maSV))
                {
                    sinhViens = GetSinhViens();
                    dgvdata.DataSource = sinhViens;
                    MessageBox.Show("Xóa thành công");
                }
                else
                {
                    MessageBox.Show("Không Xóa được sinh viên!");
                }
            }

        }

        private void CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SinhVien choose_s = (SinhVien)dgvdata.CurrentRow.DataBoundItem;
            txtMasv.Text = choose_s.MaSV;
            txtDiemtb.Text = choose_s.DiemTB.ToString();
            txtLop.Text = choose_s.Lop;
            txtTensv.Text = choose_s.TenSV;

        }
    }
}
