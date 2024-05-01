using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Lucky_Paws
{
    public partial class Form1 : Form
    {

        string connectionString = "Data Source=DESKTOP-H2JR9D8;Initial Catalog=CreateAcc;Integrated Security=True";
        string connectionStringLFS = "Data Source=DESKTOP-H2JR9D8;Initial Catalog=LFPost_DB;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }
        /*Panels:
        Login_Panel.Hide();
        GetStarted_Panel.Hide();
        Home_Panel.Hide();
        CreateAccount_Panel.Hide();
         */
        public void HideAllPanel() // parent panels
        {
            Login_Panel.Hide();
            GetStarted_Panel.Hide();
            Home_Panel.Hide();
            PetFeed_Panel.Hide();
        }
        void Clear()
        {
            Password_TextBox.Text = "Password"; Password_TextBox.FontSize = MetroFramework.MetroTextBoxSize.Tall; Password_TextBox.UseSystemPasswordChar = false; Password_TextBox.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            // CreateAcc  
            CreateAcc_FirstName.Text = "First Name"; CreateAcc_FirstName.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_FirstName.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_LastName.Text = "Last Name"; CreateAcc_LastName.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_LastName.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_MobileNumber.Text = "Mobile Number"; CreateAcc_MobileNumber.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_MobileNumber.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_EmailAddress.Text = "Email Address"; CreateAcc_EmailAddress.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_EmailAddress.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_CompleteAddress.Text = "Complete Address"; CreateAcc_CompleteAddress.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_CompleteAddress.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_Username.Text = "Username"; CreateAcc_Username.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_Username.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_Password.Text = "Password"; CreateAcc_Password.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_Password.UseSystemPasswordChar = false; CreateAcc_Password.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_ConfirmPassword.Text = "Confirm Password"; CreateAcc_ConfirmPassword.FontSize = MetroFramework.MetroTextBoxSize.Tall; CreateAcc_ConfirmPassword.UseSystemPasswordChar = false; CreateAcc_ConfirmPassword.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            CreateAcc_BDay.Text = "Day"; CreateAcc_BDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))); CreateAcc_BDay.ForeColor = System.Drawing.Color.Silver;
            CreateAcc_BYear.Text = "Year"; CreateAcc_BYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))); CreateAcc_BYear.ForeColor = System.Drawing.Color.Silver;
            CreateAcc_PP.Image = null;
            PP_label.Visible = true;
            // Look for shelter
            LFS_PetPhoto.Image = null;
            LFSName.Texts = LFSDescription.Texts = LFSAddress.Texts = LFSAge.Texts = LFSColor.Texts = LFSSex.Texts = LFSAnimalType.Texts = ""; 
        }


        //GetStarted Panel
        private void Form1_Load(object sender, EventArgs e)
        {
            HideAllPanel();
            GetStarted_Panel.Show();
            GetStarted_Panel.BringToFront();
        }
        private void GetStarted_ButtonClick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(500);
                this.Invoke(new Action(() =>
                {
                    GetStarted_Panel.Hide();
                    Login_Panel.Show();
                    Login_Panel.BringToFront();
                }));
            });
        }


        //Login Panel
        private void UsernameTextBox_LogInClick(object sender, EventArgs e)
        {
            this.Username_TextBox.Text = "";
            this.Username_TextBox.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void PasswordTextBox_LogInClick(object sender, EventArgs e)
        {
            this.Password_TextBox.Text = "";
            this.Password_TextBox.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.Password_TextBox.UseSystemPasswordChar = true;
        }
        private void Button_LogIn_Click(object sender, EventArgs e)
        {
            //profile picture load
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = con.CreateCommand();
            con.Open();
            command.Parameters.AddWithValue("@Username", Username_TextBox.Text);
            command.CommandText = "SELECT * FROM UserTable Where Username = @Username";
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                MemoryStream stream = new MemoryStream(reader.GetSqlBytes(1).Buffer);
                Prof_pic1.Image = Image.FromStream(stream);
                Prof_pic2.Image = Image.FromStream(stream);
            }
            con.Close();

            //profile details load and proceed log in
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string query = "Select * from UserTable Where Username = '" + Username_TextBox.Text.Trim()
                           + "' and Password = '" + Password_TextBox.Text.Trim() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, sqlCon);
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            if (dtbl.Rows.Count == 1)
            {
                HideAllPanel();
                Home_Panel.Show();
                Home_Panel.BringToFront();
                sqlCon.Open();
                SqlCommand cmd = new SqlCommand("Select Username, FirstName, LastName, BMonth, BDay, BYear, Gender, CompleteAddress, MobileNumber, EmailAddress from UserTable where Username =@Username", sqlCon);
                cmd.Parameters.AddWithValue("@Username", Username_TextBox.Text);
                SqlDataReader da = cmd.ExecuteReader();
                while (da.Read())
                {
                    PP_Username.Text = da.GetValue(0).ToString();
                    PP_FullName.Text = da.GetValue(1).ToString() + " " + da.GetValue(2).ToString();
                    PP_Birthday.Text = da.GetValue(3).ToString() + " " + da.GetValue(4).ToString() + ", " + da.GetValue(5).ToString();
                    PP_Gender.Text = da.GetValue(6).ToString();
                    PP_CompleteAddress.Text = da.GetValue(7).ToString();
                    PP_ContactNumber.Text = da.GetValue(8).ToString();
                    PP_Email.Text = da.GetValue(9).ToString();
                }
                Clear();
                sqlCon.Close();
            }
            else
            {
                MessageBox.Show("You have entered a wrong username or password.");
            }
        }


            //~Create Account Panel
        private void CreateAcc_Button_Click(object sender, EventArgs e)
        {
            CreateAccount_Panel.Visible = true;
            CreateAccount_Panel.Show();
            CreateAccount_Panel.BringToFront();
        }
        private void CreateAcc_FirstName_Click(object sender, EventArgs e)
        {
            CreateAcc_FirstName.Text = "";
            CreateAcc_FirstName.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_LastName_Click(object sender, EventArgs e)
        {
            CreateAcc_LastName.Text = "";
            CreateAcc_LastName.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_MobileNumber_Click(object sender, EventArgs e)
        {
            CreateAcc_MobileNumber.Text = "";
            CreateAcc_MobileNumber.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_EmailAddress_Click(object sender, EventArgs e)
        {
            CreateAcc_EmailAddress.Text = "";
            CreateAcc_EmailAddress.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_CompleteAddress_Click(object sender, EventArgs e)
        {
            CreateAcc_CompleteAddress.Text = "";
            CreateAcc_CompleteAddress.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_Username_Click(object sender, EventArgs e)
        {
            CreateAcc_Username.Text = "";
            CreateAcc_Username.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
        }
        private void CreateAcc_Password_Click(object sender, EventArgs e)
        {
            CreateAcc_Password.Text = "";
            CreateAcc_Password.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            CreateAcc_Password.UseSystemPasswordChar = true;
        }
        private void CreateAcc_ConfirmPassword_Click(object sender, EventArgs e)
        {
            CreateAcc_ConfirmPassword.Text = "";
            CreateAcc_ConfirmPassword.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            CreateAcc_ConfirmPassword.UseSystemPasswordChar = true;
        }
        private void CreateAcc_BDay_Click(object sender, EventArgs e)
        {
            CreateAcc_BDay.Text = "";
            CreateAcc_BDay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CreateAcc_BDay.ForeColor = System.Drawing.Color.Black;
        }
        private void CreateAcc_BYear_Clicked(object sender, EventArgs e)
        {
            CreateAcc_BYear.Text = "";
            CreateAcc_BYear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CreateAcc_BYear.ForeColor = System.Drawing.Color.Black;
        }
        private void UploadPP_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog OD = new OpenFileDialog();
            OD.FileName = "";
            OD.Filter = "Supported Images|*.jpg;*.jpeg;*.png";
            if (OD.ShowDialog() == DialogResult.OK)
                CreateAcc_PP.Load(OD.FileName);
            PP_label.Visible = false;
        }
        private void UploadValidID_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog OD = new OpenFileDialog();
            OD.Filter = "Supported Images|*.jpg;*.jpeg;*.png";
            if (OD.ShowDialog() == DialogResult.OK)
            ValidID_TextBox.Text = OD.FileName;
        }
        private void SignUp_Button_Click(object sender, EventArgs e)
        {
            if (checkUsername())
            {
                MessageBox.Show("This username is already taken. Choose a unique one.");
            }
            else if (CreateAcc_Password.Text == "" || CreateAcc_Username.Text == "")
            {
                MessageBox.Show("Please complete the required fields.");
            }
            else if (CreateAcc_Password.Text != CreateAcc_ConfirmPassword.Text)
            {
                MessageBox.Show("Password do not match.");
            }
            else
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand command = con.CreateCommand();
                var image = new ImageConverter().ConvertTo(CreateAcc_PP.Image, typeof(Byte[]));
                command.Parameters.AddWithValue("@ProfilePic", image);
                command.Parameters.AddWithValue("@Username", CreateAcc_Username.Text.Trim());
                command.Parameters.AddWithValue("@Password", CreateAcc_Password.Text.Trim());
                command.Parameters.AddWithValue("@FirstName", CreateAcc_FirstName.Text.Trim());
                command.Parameters.AddWithValue("@LastName", CreateAcc_LastName.Text.Trim());
                command.Parameters.AddWithValue("@BMonth", CreateAcc_BMonth.Text.Trim());
                command.Parameters.AddWithValue("@BDay", CreateAcc_BDay.Text.Trim());
                command.Parameters.AddWithValue("@BYear", CreateAcc_BYear.Text.Trim());
                command.Parameters.AddWithValue("@Gender", CreateAcc_Gender.Text.Trim());
                command.Parameters.AddWithValue("@CompleteAddress", CreateAcc_CompleteAddress.Text.Trim());
                command.Parameters.AddWithValue("@MobileNumber", CreateAcc_MobileNumber.Text.Trim());
                command.Parameters.AddWithValue("@EmailAddress", CreateAcc_EmailAddress.Text.Trim());
                command.CommandText = "INSERT INTO UserTable (ProfilePic,Username,Password,FirstName,LastName,BMonth,BDay,BYear,Gender,CompleteAddress,MobileNumber,EmailAddress) VALUES (@ProfilePic,@Username,@Password,@FirstName,@LastName,@BMonth,@BDay,@BYear,@Gender,@CompleteAddress,@MobileNumber,@EmailAddress)";
                if (command.ExecuteNonQuery() > 0)
                    MessageBox.Show("Account created successfully!");
                con.Close();
                Clear();
            }
        }
        bool checkUsername()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            string username = CreateAcc_Username.Text;
            string query = "Select * from UserTable Where Username =@Username";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            DataTable dtbl = new DataTable();
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            adapter.SelectCommand = cmd;
            adapter.Fill(dtbl);
            if (dtbl.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CreateAcc_Close_Click(object sender, EventArgs e)
        {
            CreateAccount_Panel.Visible = false;
            CreateAccount_Panel.Hide();
        }


        
        //Home Panel
        private void PickMe_Button_Click(object sender, EventArgs e)
        {
            HideAllPanel();
            PetFeed_Panel.Show();
            PetFeed_Panel.BringToFront();
        }


        //Petfeed Panel
        private void Home_Click(object sender, EventArgs e)
        {
            HideAllPanel();
            Home_Panel.Show();
            Home_Panel.BringToFront();
        }
            //~Profile Panel
        private void Profile_Button_Click(object sender, EventArgs e)
        {
            Profile_Panel.Visible = true;
            Profile_Panel.Show();
            Profile_Panel.BringToFront();
        }

        private void Profile_Close_Click(object sender, EventArgs e)
        {
            Profile_Panel.Visible = false;
            Profile_Panel.Hide();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Profile_Panel.Visible = false;
            Profile_Panel.Hide();
            HideAllPanel();
            Login_Panel.Show();
            Login_Panel.BringToFront();
        }

        private void Profile_Panel_Paint(object sender, PaintEventArgs e)
        {
            Profile_Panel.Location = new System.Drawing.Point(213, 12);
        }


        //Look for shelter panel
        private void LFShelter_Button_Click(object sender, EventArgs e)
        {
            LookForShelter_Panel.Visible = true;
            Clear();
            LookForShelter_Panel.Show();
            LookForShelter_Panel.BringToFront();
        }

        private void LFShelter_Close_Click(object sender, EventArgs e)
        {
            LookForShelter_Panel.Visible = false;
            LookForShelter_Panel.Hide();
        }

        private void LFS_PetPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog OD = new OpenFileDialog();
            OD.FileName = "";
            OD.Filter = "Supported Images|*.jpg;*.jpeg;*.png";
            if (OD.ShowDialog() == DialogResult.OK)
                LFS_PetPhoto.Load(OD.FileName);
        }

        public static int i = 0;
        private void LFSPost_Button_Click(object sender, EventArgs e)
        {
           
                SqlConnection con = new SqlConnection(connectionStringLFS);
                con.Open();
                SqlCommand command = con.CreateCommand();
                var image = new ImageConverter().ConvertTo(LFS_PetPhoto.Image, typeof(Byte[]));
            var image11 = new ImageConverter().ConvertTo(Prof_pic1.Image, typeof(Byte[]));
            command.Parameters.AddWithValue("@Pet_Photo", image);
                command.Parameters.AddWithValue("@Username", PP_Username.Text);
                command.Parameters.AddWithValue("@Name", LFSName.Texts.Trim());
                command.Parameters.AddWithValue("@Sex", LFSSex.Texts.Trim());
                command.Parameters.AddWithValue("@Age", LFSAge.Texts.Trim());
                command.Parameters.AddWithValue("@Animal_Type", LFSAnimalType.Texts.Trim());
                command.Parameters.AddWithValue("@Color", LFSColor.Texts.Trim());
                command.Parameters.AddWithValue("@Address", LFSAddress.Texts.Trim());
                command.Parameters.AddWithValue("@Description", LFSDescription.Texts.Trim());
                command.Parameters.AddWithValue("@Email", PP_Email.Text);
                command.Parameters.AddWithValue("@Contact_Number", PP_ContactNumber.Text);
            command.Parameters.AddWithValue("@PP", image11);
            command.CommandText = "INSERT INTO LFS_Table (Pet_Photo,Username,Name,Sex,Age,Animal_Type,Color,Address,Description,Email,Contact_Number,PP) VALUES (@Pet_Photo,@Username,@Name,@Sex,@Age,@Animal_Type,@Color,@Address,@Description,@Email,@Contact_Number,@PP)";
                command.ExecuteNonQuery();
                MessageBox.Show("Pet posted successfully!");
                con.Close();
                Clear();
                LookForShelter_Panel.Visible = false;
                LookForShelter_Panel.Hide();
                i++;

                con.Open();
                command.CommandText = "SELECT * FROM LFS_Table Where Name = @Name";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    MemoryStream stream = new MemoryStream(reader.GetSqlBytes(1).Buffer);
                    LFS_PetPhoto.Image = Image.FromStream(stream);
                    PF_Content1.Image = Image.FromStream(stream);
                    PFContent1_PBox.Image = Image.FromStream(stream);
                C1_LFSName.Text = reader.GetValue(0).ToString();
                C1_LFSSex.Text = reader.GetValue(2).ToString();
                C1_LFSAge.Text = reader.GetValue(3).ToString();
                C1_LFSAnimalType.Text = reader.GetValue(4).ToString();
                C1_LFSColor.Text = reader.GetValue(5).ToString();
                C1_LFSAddress.Text = reader.GetValue(6).ToString();
                C1_LFSDescription.Text = reader.GetValue(7).ToString();
                Poster_Username.Text = reader.GetValue(8).ToString();
                Poster_Email.Text = reader.GetValue(9).ToString();
                Poster_Number.Text = reader.GetValue(10).ToString();
                MemoryStream stream11 = new MemoryStream(reader.GetSqlBytes(11).Buffer);
                Prof_pic3.Image = Image.FromStream(stream11);
            }
            con.Close();
        }

        private void PF_Content1_Click(object sender, EventArgs e)
        {
            PF_Content1_Panel.Visible = true;
            PF_Content1_Panel.Show();
            PF_Content1_Panel.BringToFront();

        }

        private void PF_Content1_Panel_Close_Click(object sender, EventArgs e)
        {
            PetPicked_Panel.Visible = PF_Content1_Panel.Visible = false;
            PetPicked_Panel.Hide();
        }

        private void PF_Content1_Panel_Paint(object sender, PaintEventArgs e)
        {
            PF_Content1_Panel.Location = new System.Drawing.Point(172, 9);
            
            if (Poster_Username.Text == PP_Username.Text)
            {
                Adopted_Button.Visible = true;
                Adopt_Button.Visible = false;
            }
            else
            {
                Adopt_Button.Visible = true;
                Adopted_Button.Visible = false;
            }
        }

        private void Adopt_Button_Click(object sender, EventArgs e)
        {
            PetPicked_Panel.Visible = true;
            PetPicked_Panel.Location = new System.Drawing.Point(21, 198);
            PetPicked_Panel.Show();
            PetPicked_Panel.BringToFront();
        }

        private void PetFeedContent_Panel_Paint(object sender, PaintEventArgs e)
        {
            if (i == 1)
            {
                PF_Content1.Visible = true;
            }
            if (i == 2)
            {
                PF_Content2.Visible = true;
            }
            if (i == 3)
            {
                PF_Content3.Visible = true;
            }
            if (i == 4)
            {
                PF_Content4.Visible = true;
            }
            if (i == 5)
            {
                PF_Content5.Visible = true;
            }
            if (i == 6)
            {
                PF_Content6.Visible = true;
            }
            if (i == 7)
            {
                PF_Content7.Visible = true;
            }
            if (i == 8)
            {
                PF_Content8.Visible = true;
            }
        }

        private void PF_Content2_Click(object sender, EventArgs e)
        {
            PF_Content1_Panel.Visible = true;
            PF_Content1_Panel.Show();
            PF_Content1_Panel.BringToFront();

        }

        private void Adopted_Button_Click(object sender, EventArgs e)
        {
            i--;
            PetPicked_Panel.Visible = PF_Content1_Panel.Visible = false;
            PetPicked_Panel.Hide();
            PF_Content1.Visible = false;
            
        }

       
    }
}
