using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;


namespace WebPlotter
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string query = "select distinct (convert(varchar(10), [Timestamp], 120)) as TemperatureMeasurement from [WeatherMeasurements]";
                DataTable dt = GetData(query);
                dropDownListTimestamp.DataSource = dt;
                dropDownListTimestamp.DataTextField = "TemperatureMeasurement";
                dropDownListTimestamp.DataValueField = "TemperatureMeasurement";
                dropDownListTimestamp.DataBind();
                dropDownListTimestamp.Items.Insert(0, new ListItem("Select", ""));


            }
        }

        protected void dropDownListTimestamp_SelectedIndexChanged(object sender, EventArgs e)
        {

            // TODO add interpolated values as new series of measurements here. Use different color to indicate the measurements are calculated
            Chart1.Visible = dropDownListTimestamp.SelectedValue != "";
            string query = string.Format("SELECT [Timestamp],[TemperatureMeasurement] FROM [WeatherMeasurements] Where Timestamp between '{0}' and DATEADD(DAY, 1, '{0}')", dropDownListTimestamp.SelectedValue);
            DataTable dt = GetData(query);
            string[] x = new string[dt.Rows.Count];
            double[] y = new double[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                x[i] = dt.Rows[i][0].ToString();
                y[i] = Convert.ToDouble(dt.Rows[i][1]);
            }

            Chart1.Series[0].Points.DataBindXY(x, y);
            Chart1.Series[0].ChartType = SeriesChartType.Line;
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            Chart1.Legends[0].Enabled = true;
        }

        private static DataTable GetData(string query)
        {

            //TODO use the code in shared to access the database
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query);
            String constr = ConfigurationManager.ConnectionStrings["WeatherStats"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            return dt;
        }
    }
}