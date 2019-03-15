using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Windows.Forms;
using MyQ.CuotaService;

namespace MyQ
{
    public partial class Form1 : Form
    {
        string fichero = "conf.myq";
        public int lastSORT = 1;
        float totalMB = 0, totalMBPos = 0;
        bool USER_INCORRECT_FLAG = false;
        public List<User> USERS = new List<User>();
        public List<string> USERS_AUX = new List<string>();
        public List<string> PASS_AUX = new List<string>();

        public Form1()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(Utils.ValidarCertificado);
            timUpd.Start();
            InicializarData();
        }
        public void AdicionarUsuario(string u, string p)
        {
            var user = ObtenerUsuario(u, p);
            if (!USER_INCORRECT_FLAG)
            {
                var U = new User(u, p);
                var c = (float)user.cuota_usada;
                var x = user.cuota - (float)user.cuota_usada;

                U.RestanteFloat = x;
                U.ConsumidaFloat = c;
                U.TotalFloat = user.cuota;

                string res = x.ToString("F");
                string con = c.ToString("F");
                string tot = user.cuota.ToString();

                U.Restante = res;
                U.Consumida = con;
                U.Total = tot;
                USERS.Add(U);
                InsertarTabla(lastSORT);
                UpdateData();
            }
            tbUser.Text = "";
            tbPass.Text = "";
        }

        public Usuario ObtenerUsuario(string name, string pass)
        {
            try
            {
                InetCuotasWSService serv = new InetCuotasWSService();
                Usuario user = serv.ObtenerCuota(name, pass, "uci.cu");
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show(name + " da palo\n" + ex.Message);
                USER_INCORRECT_FLAG = true;
                return null;
            }
        }

        public void AddUser(Usuario u, int p)
        {
            if (u == null)
            {
                USER_INCORRECT_FLAG = true;
                return;
            }
            var c = (float)u.cuota_usada;
            var x = u.cuota - (float)u.cuota_usada;

            USERS[p].RestanteFloat = x;
            USERS[p].ConsumidaFloat = c;
            USERS[p].TotalFloat = u.cuota;

            string res = x.ToString("F");
            string con = c.ToString("F");
            string tot = u.cuota.ToString();

            USERS[p].Restante = res;
            USERS[p].Consumida = con;
            USERS[p].Total = tot;
        }
        
        public void UpdateLabel()
        {
            totalMB = totalMBPos = 0;
            foreach (var item in USERS)
            {
                float val = item.RestanteFloat;
                if (val > 0)
                    totalMB += val;
                totalMBPos += item.TotalFloat;
            }
            lbCuota.Text = "Cuentas: " + (USERS.Count) + " Total MB: " + totalMB.ToString("F") + " / " + totalMBPos + " MB";
        }

        public void InicializarData()
        {
            if (!File.Exists(fichero))
                return;

            LoadData();
            var listaPalo = new List<string>();
            for (int i = 0; i < USERS.Count; i++)
            {
                USER_INCORRECT_FLAG = false;
                AddUser(ObtenerUsuario(USERS[i].Name, USERS[i].Pass), i);
                if (USER_INCORRECT_FLAG)
                    listaPalo.Add(USERS[i].Name);
            }
            foreach (var name in listaPalo)
            {
                for(int i = 0; i < USERS.Count; i++)
                    if(USERS[i].Name == name)
                        USERS.RemoveAt(i);
            }
            InsertarTabla(1);
            UpdateData();
        }

        public void UpdateData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fichero))
                {
                    foreach (User u in USERS)
                    {
                        string cad = Utils.cifrar(u.Name + "__" + u.Pass);
                        writer.WriteLine(cad);
                    }
                    writer.Close();
                }
                Utils.Datos(fichero);
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadData()
        {
            List<string> lines = new List<string>();
            using (StreamReader lector = new StreamReader(fichero))
            {
                while (lector.Peek() > -1)
                {
                    string linea = lector.ReadLine();
                    if (!String.IsNullOrEmpty(linea))
                    {
                        lines.Add(Utils.descifrar(linea));
                    }
                }
                lector.Close();
            }
            foreach (var cad in lines)
            {
                string u = "", p = "";
                int i = 0; bool flag = false;
                int n = cad.Length;
                var user = new User();
                while (i < n)
                {
                    if (i + 1 < n && cad[i] == '_' && cad[i + 1] == '_')
                    {
                        flag = true;
                        user.Name = u;
                        u = "";
                        i += 2;
                        continue;
                    }
                    else if (!flag)
                        u += cad[i];
                    else if (i == n - 1)
                    {
                        p += cad[i];
                        user.Pass = p;
                        USERS.Add(user);
                        p = "";
                    }
                    else if (flag)
                        p += cad[i];

                    i++;
                }
            }
        }
        public void InsertarTabla(string a, string b, string c, string d)
        {
            var lvi = new ListViewItem(a);
            lvi.SubItems.Add(b);
            lvi.SubItems.Add(c);
            lvi.SubItems.Add(d);
            lvUsers.Items.Add(lvi);
        }

        public bool ExisteUser(string name)
        {
            return USERS.Any(user => name == user.Name);
        }

        public void LimpiarTabla()
        {
            while (lvUsers.Items.Count != 0)
                for (int i = 0; i < lvUsers.Items.Count; i++)
                    lvUsers.Items.RemoveAt(i);
        }

        public void InsertarTabla(int sort)
        {
            LimpiarTabla();

            switch (sort)
            {
                case 0:
                    USERS = USERS.OrderBy(u => u.Name).ToList();
                    break;
                case 1:
                    USERS = USERS.OrderBy(u => u.RestanteFloat).ToList();
                    break;
                case 2:
                    USERS = USERS.OrderBy(u => u.ConsumidaFloat).ToList();
                    break;
                case 3:
                    USERS = USERS.OrderBy(u => u.TotalFloat).ToList();
                    break;
            }

            foreach (var item in USERS)
                InsertarTabla(item.Name, item.Restante, item.Consumida, item.Total);
            UpdateLabel();
        }

        public void EliminarUser(int p)
        {
            if (p != -1)
            {
                USERS.RemoveAt(p);
                InsertarTabla(lastSORT);
                UpdateData();
            }
        }

        public List<string> CargarUsers()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "MYQ files (*.myq)|*.myq";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;
            List<string> lines = new List<string>();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream myStream = null;
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            using (StreamReader lector = new StreamReader(myStream))
                            {

                                while (lector.Peek() > -1)
                                {
                                    string linea = lector.ReadLine();
                                    if (!String.IsNullOrEmpty(linea))
                                    {
                                        lines.Add(Utils.descifrar(linea));
                                    }
                                }
                                lector.Close();
                            }
                        }
                    }
                    return lines;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return null;
                }
            }
            return null;
        }

        public void AdicionarNuevosUsers()
        {
            USERS_AUX.Clear();
            PASS_AUX.Clear();

            var lines = CargarUsers();
            if (lines != null)
            {
                foreach (var cad in lines)
                {
                    string u = "", p = "";
                    int i = 0;
                    bool flag = false, existe = false;
                    int n = cad.Length;
                    var user = new User();
                    while (i < n)
                    {
                        if (i + 1 < n && cad[i] == '_' && cad[i + 1] == '_')
                        {
                            flag = true;
                            existe = ExisteUser(u);
                            if (!existe)
                                USERS_AUX.Add(u);
                            u = "";
                            i += 2;
                            continue;
                        }
                        else if (!flag)
                            u += cad[i];
                        else if (i == n - 1)
                        {
                            p += cad[i];
                            user.Pass = p;
                            if (!existe)
                                PASS_AUX.Add(p);
                            p = "";
                        }
                        else if (flag)
                            p += cad[i];

                        i++;
                    }
                }
                for (int i = 0; i < USERS_AUX.Count; i++)
                    AdicionarUsuario(USERS_AUX[i], PASS_AUX[i]);

                MessageBox.Show(USERS_AUX.Count + " usuarios nuevos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void UpdateUser(int p, bool flag)
        {
            var last = USERS[p].RestanteFloat;
            var name = USERS[p].Name;
            Usuario u = ObtenerUsuario(USERS[p].Name, USERS[p].Pass);
            User U = new User(USERS[p].Name, USERS[p].Pass);
            EliminarUser(p);

            var c = (float) u.cuota_usada;
            var x = u.cuota - (float) u.cuota_usada;

            U.RestanteFloat = x;
            U.ConsumidaFloat = c;
            U.TotalFloat = u.cuota;

            string res = x.ToString("F");
            string con = c.ToString("F");
            string tot = u.cuota.ToString();

            U.Restante = res;
            U.Consumida = con;
            U.Total = tot;

            USERS.Add(U);
            InsertarTabla(lastSORT);
            UpdateData();

            if (flag)
            {
                timShow.Start();
                label3.Visible = true;
                label3.Text = "Usuario " + name + ": " + (last - x).ToString("F") + " MB cons.";
                //label3.Text = "Usuario sdrodriguez: 12,43 MB cons.";
            }
        }

        private void btnUpd_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
                UpdateUser(lvUsers.Items.IndexOf(lvUsers.SelectedItems[0]), true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(tbUser.Text != "" && tbPass.Text != "")
                AdicionarUsuario(tbUser.Text, tbPass.Text);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
                EliminarUser(lvUsers.Items.IndexOf(lvUsers.SelectedItems[0]));
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
                Clipboard.SetText(USERS[lvUsers.Items.IndexOf(lvUsers.SelectedItems[0])].Pass);
        }
        private void lvUsers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lastSORT = e.Column;
            InsertarTabla(lastSORT);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdicionarNuevosUsers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InsertarTabla("sdrodriguezfern", "1000,00", "1024,00", "1000");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUpd.Checked)
            {
                nupTime.Enabled = true;
                timUpd.Interval = 60000 * (int) nupTime.Value;
                timUpd.Start();
            }
            else
            {
                nupTime.Enabled = false;
                timUpd.Stop();
            }
        }

        private void timUpd_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("lalalla");
        }

        private void timShow_Tick(object sender, EventArgs e)
        {
            label3.Visible = false;
            timShow.Stop();
        }

        private void tbPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '\r')
                if (tbUser.Text != "" && tbPass.Text != "")
                    AdicionarUsuario(tbUser.Text, tbPass.Text);
        }

        private void nupTime_ValueChanged(object sender, EventArgs e)
        {
            timUpd.Stop();
            timUpd.Interval = 60000 * (int)nupTime.Value;
            timUpd.Start();
        }
    }
}
