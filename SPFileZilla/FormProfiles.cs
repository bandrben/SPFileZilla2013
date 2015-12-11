using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BandR;
using BandR.CustObjs;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SPFileZilla2013
{
    public partial class FormProfiles : Form
    {

        public Form1 form1;

        internal List<ProfileDetail> lstProfiles;

        /// <summary>
        /// </summary>
        public FormProfiles()
        {
            InitializeComponent();

            this.Closed += new EventHandler(FormProfiles_Closed);

            // retrieve saved profiles from file, put in listbox
            lstProfiles = new List<ProfileDetail>();
            GetRecentSessionInfo();
            LoadProfilesIntoListbox();
        }

        /// <summary>
        /// </summary>
        void FormProfiles_Closed(object sender, EventArgs e)
        {
            // save profiles to file
            SaveCurrentSessionInfo();
        }

        /// <summary>
        /// </summary>
        private void SaveCurrentSessionInfo()
        {
            StreamWriter sw = null;

            try
            {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\" + "profiles.dat";
                sw = new StreamWriter(iniPath, false);
                var xml = XmlSerialization.Serialize(lstProfiles);

                xml = GenUtil.Cypher(xml);

                sw.Write(xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                if (sw != null) sw.Dispose();
            }
        }

        /// <summary>
        /// </summary>
        private void GetRecentSessionInfo()
        {
            StreamReader sr = null;

            try
            {
                string iniPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\" + "profiles.dat";
                var fi = new FileInfo(iniPath);

                if (fi.Exists)
                {
                    sr = new StreamReader(iniPath);
                    var content = sr.ReadToEnd();
                    content = GenUtil.Cypher(content);
                    lstProfiles = XmlSerialization.Deserialize<List<ProfileDetail>>(content);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                if (sr != null) sr.Dispose();
            }
        }

        /// <summary>
        /// </summary>
        private void LoadProfilesIntoListbox()
        {
            lbProfiles.Items.Clear();

            foreach (ProfileDetail profile in lstProfiles.OrderBy(x => x.profileName))
            {
                lbProfiles.Items.Add(profile.profileName);
            }
        }

        /// <summary>
        /// </summary>
        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load selected profile details
            if (lbProfiles.SelectedIndex < 0)
            {
                return;
            }

            var profName = lbProfiles.SelectedItem.ToString();

            if (GenUtil.IsNull(profName))
            {
                return;
            }

            var prof = lstProfiles.FirstOrDefault(x => x.profileName.Trim().ToLower() == profName.Trim().ToLower());

            if (prof == null)
            {
                return;
            }

            tbProfileName.Text = prof.profileName;
            tbSiteUrl.Text = prof.siteUrl;
            tbUsername.Text = prof.username;
            tbPassword.Text = prof.password;
            tbDomain.Text = prof.domain;
            cbIsSharePointOnline.Checked = prof.isSpOnline;

        }

        /// <summary>
        /// </summary>
        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            // save profile details
            var newProfile = new ProfileDetail();

            newProfile.profileName = tbProfileName.Text.Trim();
            newProfile.siteUrl = tbSiteUrl.Text.Trim();
            newProfile.username = tbUsername.Text.Trim();
            newProfile.password = tbPassword.Text.Trim();
            newProfile.domain = tbDomain.Text.Trim();
            newProfile.isSpOnline = cbIsSharePointOnline.Checked;

            if (GenUtil.IsNull(newProfile.profileName))
            {
                MessageBox.Show("Profile name is required.", "Error");
                return;
            }
            if (GenUtil.IsNull(newProfile.siteUrl))
            {
                MessageBox.Show("Site url is required.", "Error");
                return;
            }
            if (GenUtil.IsNull(newProfile.username))
            {
                MessageBox.Show("Username is required.", "Error");
                return;
            }
            if (GenUtil.IsNull(newProfile.password))
            {
                MessageBox.Show("Password is required.", "Error");
                return;
            }
            if (!cbIsSharePointOnline.Checked && GenUtil.IsNull(newProfile.domain))
            {
                MessageBox.Show("Domain is required.", "Error");
                return;
            }

            // get matching profile or create new
            var existingProfile = lstProfiles.FirstOrDefault(x => x.profileName.Trim().ToLower() == newProfile.profileName.Trim().ToLower());

            if (existingProfile == null)
            {
                lstProfiles.Add(newProfile);
                LoadProfilesIntoListbox();
            }
            else
            {
                existingProfile.profileName = newProfile.profileName;
                existingProfile.siteUrl = newProfile.siteUrl;
                existingProfile.username = newProfile.username;
                existingProfile.password = newProfile.password;
                existingProfile.domain = newProfile.domain;
                existingProfile.isSpOnline = newProfile.isSpOnline;
            }

        }

        /// <summary>
        /// </summary>
        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            DialogResult dgResult = MessageBox.Show("Are you sure?",
                "Delete Profile",
                MessageBoxButtons.YesNo);

            if (dgResult != DialogResult.Yes)
            {
                return;
            }

            // delete profile, reload listbox
            var profName = tbProfileName.Text.Trim();

            lstProfiles.RemoveAll(x => x.profileName.Trim().ToLower() == profName.Trim().ToLower());

            LoadProfilesIntoListbox();

            btnAddProfile_Click(null, null);
        }

        /// <summary>
        /// </summary>
        private void btnAddProfile_Click(object sender, EventArgs e)
        {
            // create new profile and load its details
            tbProfileName.Text = "";
            tbSiteUrl.Text = "";
            tbUsername.Text = "";
            tbPassword.Text = "";
            tbDomain.Text = "";
            cbIsSharePointOnline.Checked = false;

            lbProfiles.SelectedIndex = -1;
        }

        /// <summary>
        /// </summary>
        private void btnConnectProfile_Click(object sender, EventArgs e)
        {
            // save profile first
            btnSaveProfile_Click(null, null);

            // get saved profile
            var prof = lstProfiles.FirstOrDefault(x => x.profileName.Trim().ToLower() == tbProfileName.Text.Trim().ToLower());

            if (prof == null)
            {
                return;
            }

            form1.LoadProfile(prof);
            this.Hide();

            form1.Show();
            form1.Focus();

            this.Close();
        }

    }
}
