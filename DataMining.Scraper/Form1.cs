using CsQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMining.Scraper
{
    public partial class Form1 : Form
    {
        const string espnTeamStats = "http://espn.go.com/mens-college-basketball/team/stats/_/id/";
        const string espnTeamHome = "http://espn.go.com/mens-college-basketball/team/_/id/";
        //const string rpiPage = "http://www.cbssports.com/collegebasketball/bracketology/nitty-gritty-report";

        const string recordSelect = "",
            rpiSelect = "",
            ppgSelect = ".total td:nth-child(4)",
            rpgSelect = ".total td:nth-child(5)",
            apgSelect = ".total td:nth-child(6)",
            spgSelect = ".total td:nth-child(7)",
            bpgSelect = ".total td:nth-child(8)",
            tpgSelect = ".total td:nth-child(9)",
            fgpSelect = ".total td:nth-child(10)",
            ftpSelect = ".total td:nth-child(11)",
            tppSelect = ".total td:nth-child(12)";

        private void teamDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTeam.Text = ((ComboBox)sender).Text;
            txtEspnId.Text = ((ComboBox)sender).SelectedValue.ToString();
        }

        string[] PowerConferences = {"ACC",
                                       "Big 12",
                                       "Big Ten",
                                       "Pac-12",
                                       "SEC",
                                       "Big East"
                                    }; // No AAC
        public Form1()
        {
            InitializeComponent();

            var teams = File.ReadAllLines(@"..\..\TeamNames.txt")
                .Select(s => s.Split(','))
                .Select(t => new { Id = t[0], Name = t[1].Trim() })
                .OrderBy(t => t.Name)
                .ToList();
            teamDropdown.DataSource = teams;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;

            HttpClient http = new HttpClient();
            Task<HttpResponseMessage> statsResponse = http.GetAsync(espnTeamStats + txtEspnId.Text);
            Task<HttpResponseMessage> teamsResponse = http.GetAsync(espnTeamHome + txtEspnId.Text);

            string name = txtTeam.Text,
                year = "2016",
                seed = txtSeed.Text,
                finish = "0";

            string teamBody = await (await teamsResponse).Content.ReadAsStringAsync();
            CQ html = new CQ(teamBody);

            // Parse wins and losses to get win pct.
            string recordText = html["li.record"].Text();

            string[] parts = recordText.Substring(0, recordText.Length / 2).Split('-');
            int win  = int.Parse(parts[0]),
                lose = int.Parse(parts[1]);
            string record = Math.Round(win / (float)(win + lose), 4).ToString();

            // Check if team is in major conference
            string subTitle = html["li.ranking"].First().Text();
            string conference = subTitle.Substring(subTitle.IndexOf("in") + 3);
            string isPower = PowerConferences.Contains(conference) ? "1" : "0";

            // Get RPI ranking
            //string rk = html["#teamrankingtable tr.even a"].First().Text();
            //int openPar = rk.IndexOf("(") + 1, closePar = rk.IndexOf(")");
            string rpi = ".000";// double.Parse(rk.Substring(openPar, closePar - openPar)).ToString();

            // Parse remaining per game stats
            string statsBody = await (await statsResponse).Content.ReadAsStringAsync();
            html = new CQ(statsBody);
            CQ body = html["#my-teams-table"];
            string ppg = body[ppgSelect].First().Text(),
                rpg = body[rpgSelect].First().Text(),
                apg = body[apgSelect].First().Text(),
                spg = body[spgSelect].First().Text(),
                bpg = body[bpgSelect].First().Text(),
                tpg = body[tpgSelect].First().Text(),
                fgp = body[fgpSelect].First().Text(),
                ftp = body[ftpSelect].First().Text(),
                tpp = body[tppSelect].First().Text();

            string dm2test_lin = seed + ",\t"
                + record + ",\t"
                + isPower + ",\t"
                + rpi + ",\t"
                + finish + ",\t"
                + ppg + ",\t"
                + rpg + ",\t"
                + apg + ",\t"
                + spg + ",\t"
                + bpg + ",\t"
                + tpg + ",\t"
                + fgp + ",\t"
                + ftp + ",\t"
                + tpp;

            string teams_test = "'" + name + "',\t"
                + year + ",\t"
                + dm2test_lin;

            // Clear the two files above, append line to end of file
            //File.AppendAllText(@"..\..\..\Transform\ARFF Files\dm2test_lin.arff", "\n" + dm2test_lin);
            //File.AppendAllText(@"..\..\..\DataMining.Simulate\teams_test.txt", "\n" + teams_test);
            string.Join(",\r\n",
                "'" + name + "'", year, seed, record, conference, rpi, finish,
                ppg, rpg, apg, spg, bpg, tpg, fgp, ftp, tpp);

            validationTextBox.Text = "'" + name + "',\r\n"
                + year + ",\r\n"
                + seed + ",\r\n"
                + record + ",\r\n"
                + conference + ",\r\n"
                + rpi + ",\r\n"
                + finish + ",\r\n"
                + ppg + ",\r\n"
                + rpg + ",\r\n"
                + apg + ",\r\n"
                + spg + ",\r\n"
                + bpg + ",\r\n"
                + tpg + ",\r\n"
                + fgp + ",\r\n"
                + ftp + ",\r\n"
                + tpp;

            txtTeam.Text = "";
            txtEspnId.Text = "";
            txtSeed.Text = "";
            outputButton.Enabled = true;
        }
    }
}
