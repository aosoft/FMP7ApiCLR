using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FMP.FMP7;

namespace FMP7ApiSample
{
	public partial class Form1 : Form
	{
		private const int MaxChannel = 64;	// 7.10a では 64ch までなので

		private FMPWork m_work = null;
		private FMPInfo m_info = null;
		private Label[] m_labelChannelNums;
		private Label[] m_labelSoundTypes;
		private Label[] m_labelNotes;
		private Label[] m_labelTones;
		private Label[] m_labelVolume;
		private Label[] m_labelFreq;
		private Label[] m_labelLfoH;
		private Label[] m_labelLfoL;

		private FMPPartWork[] m_partworks;
		private MusicFileExtInfo m_extinfo;

		private Label CreateLabel(int x, int y)
		{
			var ret = new Label();

			ret.Location = new Point(4 + x * 64, 4 + y * 32);
			ret.Parent = panelParameterView;
			ret.AutoSize = true;

			return ret;
		}

		public Form1()
		{
			InitializeComponent();

			panelParameterView.Size = new Size(4 + 8 * 64, 4 + 32 * Form1.MaxChannel);

			m_labelChannelNums = new Label[Form1.MaxChannel];
			m_labelSoundTypes = new Label[Form1.MaxChannel];
			m_labelNotes = new Label[Form1.MaxChannel];
			m_labelTones = new Label[Form1.MaxChannel];
			m_labelVolume = new Label[Form1.MaxChannel];
			m_labelFreq = new Label[Form1.MaxChannel];
			m_labelLfoH = new Label[Form1.MaxChannel];
			m_labelLfoL = new Label[Form1.MaxChannel];

			for (int i = 0; i < Form1.MaxChannel; i++)
			{
				m_labelChannelNums[i] = CreateLabel(0, i);
				m_labelSoundTypes[i] = CreateLabel(1, i);
				m_labelNotes[i] = CreateLabel(2, i);
				m_labelTones[i] = CreateLabel(3, i);
				m_labelVolume[i] = CreateLabel(4, i);
				m_labelFreq[i] = CreateLabel(5, i);
				m_labelLfoH[i] = CreateLabel(6, i);
				m_labelLfoL[i] = CreateLabel(7, i);

				m_labelChannelNums[i].Text = (i + 1).ToString();
			}

			m_work = new FMPWork();

			m_partworks = new FMPPartWork[Form1.MaxChannel];

			try
			{
				m_info = FMPControl.GetFMPInfo();
				UpdateText();
				m_extinfo = FMPControl.GetMusicFileExtInfo();
			}
			catch
			{
				labelMusicTitle.Text = "";
				labelMusicCreator.Text = "";
			}

			
			var listener = new FMPMessageListener();
			
			HandleCreated += (s, e) =>
				{
					listener.AssignHandle(Handle);
				};

			HandleDestroyed += (s, e) =>
				{
					listener.ReleaseHandle();
				};

			listener.FMPMessageEvent += (s, e) =>
				{
					switch (e.Message)
					{
						case FMPMessage.StartFMP:
							{
								try
								{
									m_info = FMPControl.GetFMPInfo();
									UpdateText();
									m_extinfo = FMPControl.GetMusicFileExtInfo();
								}
								catch
								{
									m_info = null;
									labelMusicTitle.Text = "";
									labelMusicCreator.Text = "";
								}
							}
							break;
						case FMPMessage.EndFMP:
							{
								m_info = null;
							}
							break;
					}

				};

			timer1.Start();
		}

		private void ShowErrorDialog(string msg)
		{
			MessageBox.Show(
				msg,
				this.Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}

		private void UpdateText()
		{
			labelMusicTitle.Text = FMPControl.GetTextData(FMPText.Title);
			labelMusicCreator.Text = FMPControl.GetTextData(FMPText.Creator);
		}

		private void MusicPlay()
		{
			try
			{
				if (listboxMusicList.SelectedIndex >= 0)
				{
					FMPControl.MusicLoad(
						listboxMusicList.SelectedItem.ToString(),
						FMPMusicLoadAction.LoadAndPlay);
				}
			}
			catch (Exception ex)
			{
				ShowErrorDialog(ex.Message);
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			timer1.Stop();
			if (m_work != null)
			{
				m_work.Dispose();
				m_work = null;
			}
		}

		private void listboxMusicList_DragDrop(object sender, DragEventArgs e)
		{
			if (m_extinfo != null)
			{
				try
				{
					if (e.Data.GetDataPresent(DataFormats.FileDrop))
					{
						foreach (string fileName in
							(string[])e.Data.GetData(DataFormats.FileDrop))
						{
							if (m_extinfo.IsSupportFile(fileName))
							{
								listboxMusicList.Items.Add(fileName);
							}
						}
					}
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex.Message);
				}
			}
		}

		private void listboxMusicList_DragEnter(object sender, DragEventArgs e)
		{
			if (m_extinfo != null)
			{
				try
				{
					if (e.Data.GetDataPresent(DataFormats.FileDrop))
					{
						foreach (string fileName in
							(string[])e.Data.GetData(DataFormats.FileDrop))
						{
							if (m_extinfo.IsSupportFile(fileName))
							{
								e.Effect = DragDropEffects.All;
								return;
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private void listboxMusicList_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			MusicPlay();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				if (m_work == null ||
					FMPControl.CheckAvailableFMP() == false)
				{
					return;
				}

				UpdateText();

				FMPGlobalWork gwork;
				object exwork;
	
				m_work.Open(
					m_info,
					FMP.FMP7.AddOn.DriverType.FMP4 |
					FMP.FMP7.AddOn.DriverType.PMD |
					FMP.FMP7.AddOn.DriverType.MXDRV);
				try
				{
					gwork = m_work.GetGlobalWork();
					m_work.CopyPartWork(0, m_partworks, 0, Form1.MaxChannel);
					exwork = m_work.GetExWork();
				}
				finally
				{
					m_work.Close();
				}


				if (exwork is FMP.FMP7.AddOn.FMP4Work)
				{
					var fmp4work = (FMP.FMP7.AddOn.FMP4Work)exwork;
					for (int i = 0; i < FMP.FMP7.AddOn.FMP4Work.PartCountAll; i++)
					{
						if (gwork.Device[i] == FMPSoundDeviceUnit.Rhythm &&
							fmp4work.Mode[i] != FMP.FMP7.AddOn.FMP4PartMode.PDZF)
						{
							m_labelSoundTypes[i].Text = fmp4work.Mode[i].ToString();
						}
						else
						{
							m_labelSoundTypes[i].Text =
								string.Format("{0}{1}", fmp4work.Mode[i], fmp4work.ModeIndex[i]);
						}
						m_labelNotes[i].Text = m_partworks[i].Note.ToString();
						m_labelTones[i].Text = string.Format("@{0}", m_partworks[i].Tone.ToString());

						int vol; 
						if (gwork.Mode[i] == FMPSoundUnit.FM)
						{
							vol = 127 - fmp4work.Volume[i];
						}
						else
						{
							vol = fmp4work.Volume[i];
						}
						if (fmp4work.Mode[i] == FMP.FMP7.AddOn.FMP4PartMode.PDZF)
						{
							vol &= 15;
						}
						m_labelVolume[i].Text = vol.ToString();

						if (m_partworks[i].Note.IsRest)
						{
							m_labelFreq[i].Text = "";
						}
						else
						{
							m_labelFreq[i].Text =
								(m_partworks[i].Freq - m_partworks[i].Note.Freq).ToString();
						}
						m_labelLfoH[i].Text = fmp4work.StatusLFO[i].ToString();
						m_labelLfoL[i].Text = "";
					}
				}
				else if (exwork is FMP.FMP7.AddOn.PMDWork)
				{
					var pmdwork = (FMP.FMP7.AddOn.PMDWork)exwork;

					for (int i = 0; i < FMP.FMP7.AddOn.PMDWork.PartCountAll; i++)
					{
						m_labelSoundTypes[i].Text = pmdwork.Mode[i].ToString();
						m_labelNotes[i].Text = m_partworks[i].Note.ToString();
						m_labelTones[i].Text = string.Format("@{0}", m_partworks[i].Tone.ToString());

						int vol;
						vol = pmdwork.Volume[i];
						m_labelVolume[i].Text = vol.ToString();

						if (m_partworks[i].Note.IsRest)
						{
							m_labelFreq[i].Text = "";
						}
						else
						{
							m_labelFreq[i].Text =
								(m_partworks[i].Freq - m_partworks[i].Note.Freq).ToString();
						}
						m_labelLfoH[i].Text = "";
						m_labelLfoL[i].Text = "";
					}
				}
				else if (exwork is FMP.FMP7.AddOn.MXDRVWork)
				{
					var mxdrvwork = (FMP.FMP7.AddOn.MXDRVWork)exwork;

					for (int i = 0; i < FMP.FMP7.AddOn.MXDRVWork.PartCountAll; i++)
					{
						m_labelSoundTypes[i].Text = mxdrvwork.Mode[i].ToString();
						m_labelNotes[i].Text = m_partworks[i].Note.ToString();
						m_labelTones[i].Text = string.Format("@{0}", m_partworks[i].Tone.ToString());

						int vol;
						vol = mxdrvwork.Volume[i];
						m_labelVolume[i].Text = vol.ToString();

						if (m_partworks[i].Note.IsRest)
						{
							m_labelFreq[i].Text = "";
						}
						else
						{
							m_labelFreq[i].Text =
								(m_partworks[i].Freq - m_partworks[i].Note.Freq).ToString();
						}
						m_labelLfoH[i].Text = "";
						m_labelLfoL[i].Text = "";
					}
				}
				else
				{
					for (int i = 0; i < Form1.MaxChannel; i++)
					{
						m_labelSoundTypes[i].Text = gwork.Device[i].ToString();
						m_labelNotes[i].Text = m_partworks[i].Note.ToString();
						m_labelTones[i].Text = string.Format("@{0}", m_partworks[i].Tone.ToString());
						m_labelVolume[i].Text = m_partworks[i].Volume.ToString();
						if (m_partworks[i].Note.IsRest)
						{
							m_labelFreq[i].Text = "";
						}
						else
						{
							m_labelFreq[i].Text =
								(m_partworks[i].Freq - m_partworks[i].Note.Freq).ToString();
						}
						m_labelLfoH[i].Text = m_partworks[i].LFO0Waveform.ToString();
						m_labelLfoL[i].Text = m_partworks[i].LFO0Type.ToString();
					}
				}

				if (checkAutoPlay.Checked &&
					listboxMusicList.Items.Count > 0)
				{

					if ((gwork.Status & (FMPStat.Play | FMPStat.Loop)) ==
						(FMPStat.Play | FMPStat.Loop))
					{
						FMPControl.MusicFadeOut(16);
					}
					if (gwork.Status == FMPStat.None)
					{
						if (listboxMusicList.SelectedIndex < 0 ||
							listboxMusicList.SelectedIndex >=
								(listboxMusicList.Items.Count - 1))
						{
							listboxMusicList.SelectedIndex = 0;
						}
						else
						{
							listboxMusicList.SelectedIndex++;
						}
						MusicPlay();
					}
				}
			}
			catch
			{
			}
		}

		private void btnAllClearList_Click(object sender, EventArgs e)
		{
			listboxMusicList.Items.Clear();
		}
	}
}
