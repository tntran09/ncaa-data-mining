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

        readonly string[] PowerConferences = {"ACC",
                                       "Big 12",
                                       "Big Ten",
                                       "Pac-12",
                                       "SEC",
                                       "Big East"
                                    }; // No American

        string TeamStatLine { get; set; }
        string FilteredStatLine { get; set; }

        private void teamDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            idTextBox.Text = ((ComboBox)sender).SelectedValue.ToString();
        }

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
            HttpClient http = new HttpClient();
            Task<HttpResponseMessage> statsResponse = http.GetAsync(espnTeamStats + idTextBox.Text);
            Task<HttpResponseMessage> teamsResponse = http.GetAsync(espnTeamHome + idTextBox.Text);

            FilteredStatLine = "";
            TeamStatLine = "";
            btnSubmit.Enabled = false;
            outputButton.Enabled = false;
            validationTextBox.Text = "";

            string teamBody = await (await teamsResponse).Content.ReadAsStringAsync();
            CQ html = new CQ(teamBody);

            string fullTeamName = html[".team-name .link-text"].First().Text();

            // Parse wins and losses to get win pct.
            string recordText = html["li.record"].Text();

            string[] parts = recordText.Substring(0, recordText.Length / 2).Split('-');
            float win  = int.Parse(parts[0]),
                  lose = int.Parse(parts[1]);
            string record = Math.Round(win / (win + lose), 4).ToString();

            // Check if team is in major conference
            string subTitle = html["li.ranking"].First().Text();
            string conference = subTitle.Substring(subTitle.IndexOf("in") + 3);
            string isPower = PowerConferences.Contains(conference) ? "1" : "0";

            // Get RPI ranking
            //string rk = html["#teamrankingtable tr.even a"].First().Text();
            //int openPar = rk.IndexOf("(") + 1, closePar = rk.IndexOf(")");
            //string rpi = double.Parse(rk.Substring(openPar, closePar - openPar)).ToString();
            string rpi = ".000";

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
            
            string name = teamDropdown.Text,
                   year = "2018",
                   seed = seedTextBox.Text,
                 finish = "0";

            FilteredStatLine = string.Join(",\t",
                seed, record, isPower, rpi, finish,
                ppg, rpg, apg, spg, bpg, tpg, fgp, ftp, tpp);

            TeamStatLine = string.Join(",\t",
                "'" + name + "'", year,
                FilteredStatLine);

            validationTextBox.Text = string.Join(",\r\n",
                "'" + fullTeamName + "'", year, seed,
                win + "-" + lose + " " + record, conference, rpi, finish,
                ppg, rpg, apg, spg, bpg, tpg, fgp, ftp, tpp);

            seedTextBox.Text = "";
            btnSubmit.Enabled = true;
            outputButton.Enabled = true;
        }
        
        private void outputButton_Click(object sender, EventArgs e)
        {
            // Clear the two files above, append line to end of file
            File.AppendAllText(@"..\..\..\DataMining.Transform\ARFF Files\dm2test_lin.arff", "\r\n" + FilteredStatLine);
            File.AppendAllText(@"..\..\..\DataMining.Simulate\teams_test.txt", "\r\n" + TeamStatLine);

            FilteredStatLine = "";
            TeamStatLine = "";
            btnSubmit.Enabled = true;
            validationTextBox.Text = "";
            outputButton.Enabled = false;
        }
    }
}
