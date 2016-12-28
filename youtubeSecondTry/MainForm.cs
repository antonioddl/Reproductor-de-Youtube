using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeSearch;

namespace youtubeSecondTry
{
    public partial class MainForm : Form
    {
        #region Constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Botones

        private void buscarButton_Click(object sender, EventArgs e)
        {
            try
            {
                VideoSearch items = new VideoSearch();
                //Lista de la clase video
                List<Video> list = new List<Video>();
                // Por cada video que resulte de la busqueda, lo agregamos a la lista videos
                // Para despues pasarlo al datagrid.
                foreach (var item in items.SearchQuery(searchTextBox.Text, 1))
                {
                    Video video = new Video();
                    video.Titulo = item.Title;
                    video.Autor = item.Author;
                    video.Url = item.Url;
                    byte[] ImagenBytes = new WebClient().DownloadData(item.Thumbnail);
                    using (MemoryStream ms = new MemoryStream(ImagenBytes))
                    {
                        video.Thumbnail = Image.FromStream(ms);
                    }
                    list.Add(video);
                }
                videoBindingSource.DataSource = list;
                searchTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            youtubeWebBrowser.Url = new Uri(modificarUrl(resultsDataGridView.SelectedCells[3].Value.ToString()));
            playButton.Visible = false;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Eventos

        private void resultsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            playButton.Visible = true;
            
        }

        #endregion

        #region Métodos

        public string modificarUrl(string url)
        {
            return url.Replace("watch?v=", "v/");
        }

        #endregion

       
    }
}
