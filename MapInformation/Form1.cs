using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
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
using static GMap.NET.MapProviders.StrucRoads.SnappedPoint;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MapInformation
{
    public partial class Form1 : Form
    {

        GMapOverlay overlay1;
        List<Arac> list;
        SqlConnection connection = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = ProjelerVT; Integrated Security = True");
        public Form1()
        {
            InitializeComponent();
            InitializeMap();
            aracListesiniOlustur();
        }

        private void araclariHaritadaGoster()
        {
            foreach (Arac arac in list)
            {
                GMarkerGoogle markerTmp = new GMarkerGoogle(arac.Konum1, GMarkerGoogleType.blue_dot);
                markerTmp.Tag = arac.Plaka1;
                markerTmp.ToolTipText = arac.ToString();
                overlay1.Markers.Add(markerTmp);
            }
        }

        private void aracListesiniOlustur()
        {
            list = new List<Arac>();
            try
            {
                connection.Open();
                string cmd = "SELECT Plaka, AracTipi, Nereden, Nereye, Enlem, Boylam FROM Cars";

                SqlDataAdapter da = new SqlDataAdapter(cmd, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0 )
                {
                    dataGridView1.DataSource = dt;
                }

                for ( int i = 0; i < dt.Rows.Count; i++ )
                {
                    list.Add(new Arac(dt.Rows[i][0].ToString()
                                     ,dt.Rows[i][1].ToString()
                                     ,dt.Rows[i][2].ToString()
                                     ,dt.Rows[i][3].ToString()
                                     ,new PointLatLng(Convert.ToDouble(dt.Rows[i][4].ToString())
                                     ,Convert.ToDouble(dt.Rows[i][5].ToString()))));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu, Hata Kodu:101\n" + ex.Message);
            }
            finally 
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void InitializeMap()
        {
            map.DragButton = MouseButtons.Left;
            //map.MapProvider = GMapProviders.GoogleMap;
            map.MapProvider = GMapProviders.GoogleHybridMap;
            map.Position = new GMap.NET.PointLatLng(36.0, 42.0);
            map.Zoom = 4;
            map.MinZoom = 3;
            map.MaxZoom = 25;

            overlay1 = new GMapOverlay();


            //Bir overlay (katman) olacak.
            //Harita üzerinde görüntülenecek tüm componentleri bu katmana ekleyecek

            //GMapOverlay overlay1 = new GMapOverlay();

            //İlk olarak yeni oluşan katmanı haritaya ekliyoruz

            map.Overlays.Add(overlay1);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            map.Dispose();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointLatLng location1 = new PointLatLng(Convert.ToDouble(textBoxEnlem.Text), Convert.ToDouble(textBoxBoylam.Text));
            GMarkerGoogle marker1 = new GMarkerGoogle(location1, GMarkerGoogleType.blue_dot);
            marker1.ToolTipText = "Lokasyon";
            marker1.ToolTip.Fill = Brushes.Black;
            marker1.ToolTip.Foreground = Brushes.White;
            marker1.ToolTip.Stroke = Pens.Black;
            marker1.ToolTip.TextPadding = new Size(10, 10);
            marker1.ToolTipMode = MarkerTooltipMode.Always;  // normali OnMouseOver
            marker1.Tag = 101;
            
            //Sonra markerlar eklencek
            overlay1.Markers.Add(marker1);

        }

        private void map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            string secilenAracinPlakasi = (string)item.Tag;
            foreach (Arac arac in list)
            {
                if(secilenAracinPlakasi.Equals(arac.Plaka1))
                {
                    textBoxP.Text = secilenAracinPlakasi;
                    textBoxTip.Text = arac.Tipi1;
                    textBoxF.Text = arac.From1;
                    textBoxTo.Text = arac.To1;
                    break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            araclariHaritadaGoster();
        }
    }
} 