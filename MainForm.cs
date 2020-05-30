// Decompiled with JetBrains decompiler
// Type: CS_Client_Siemens.MainForm
// Assembly: CS_Client_Siemens, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 99E93972-B612-44C3-9448-CD860EF726C4
// Assembly location: F:\PLC_PROGETTO\CS_Client_Siemens_esempio_comunicazione\Debug\CS_Client_Siemens.exe

using Sharp7;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CS_Client_Siemens
{
  public class MainForm : Form
  {
    private byte[] db1Buffer = new byte[10];
    private byte[] db3Buffer = new byte[10];
    private CDataBlock db10 = new CDataBlock(); //  Database 10 del PLC
    private CDataBlock db30 = new CDataBlock(); //  Database 30 del PLC
    private IContainer components = (IContainer) null;
    private S7Client Client;
    private int result;
    private Label lblStatus;
    private Panel panel1;
    private Label lblNrSlot;
    private Label lblNrRack;
    private Label lblIndirizzoIP;
    private TextBox txtIndirizzoIP;
    private TextBox txtNrSlot;
    private TextBox txtNrRack;
    private Button btnSconnettere;
    private Button btnConnettere;
    private TableLayoutPanel tlp_DB10;
    private Label lblValore01;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label lblValore02;
    private Label lblValore03;
    private Label lblValore04;
    private Label lblValore05;
    private TableLayoutPanel tlp_DB30;
    private Label lblValore31;
    private Label label13;
    private Label label14;
    private Label label15;
    private Label label16;
    private Label label17;
    private Label label18;
    private Label label19;
    private Label lblValore32;
    private Label lblValore33;
    private Label lblValore34;
    private Label lblValore35;
    private Timer timer1;
    private Timer timer2;

    private void ShowResult(int Result)
    {
      this.lblStatus.Text = this.Client.ErrorText(Result);
      if (Result != 0) //  Se non ci sono errori allora mostra l'execution time
        return;
      this.lblStatus.Text = this.lblStatus.Text + " (" + this.Client.ExecutionTime.ToString() + " ms)";
      // Pinga il PLC e ritorna il tempo di risposta con .ExecutionTime
    }

    public MainForm()
    {
      this.InitializeComponent();
      this.Client = new S7Client(); //  Crea l'oggetto S7Client (PLC)
      //  Setta i nomi per i database
      this.db10.DBName = "DB10";
      this.db30.DBName = "DB30";
    }

    private void btnConnettere_Click(object sender, EventArgs e)
    {
      int Result = this.Client.ConnectTo(this.txtIndirizzoIP.Text, Convert.ToInt32(this.txtNrRack.Text), Convert.ToInt32(this.txtNrSlot.Text));
      //  ConnectTo(IP del PLC, Rack del PLC, Slot del PLC)
      this.ShowResult(Result);
      if (Result != 0)
        return;
      //  Disabilita i vari campi della WFA
      this.lblStatus.Text = this.lblStatus.Text + " PDU Negotiated : " + this.Client.PduSizeNegotiated.ToString();
      this.txtIndirizzoIP.Enabled = false;
      this.txtNrRack.Enabled = false;
      this.txtNrSlot.Enabled = false;
      this.btnConnettere.Enabled = false;
      this.btnSconnettere.Enabled = true;
      this.tlp_DB10.Enabled = true;
      this.tlp_DB30.Enabled = true;
      this.timer1.Enabled = true;
      this.timer2.Enabled = true;
    }

    private void btnSconnettere_Click(object sender, EventArgs e)
    {
      this.Client.Disconnect();
      this.lblStatus.Text = "Disconnected";
      //  Riabilita i vari campi del WFA
      this.txtIndirizzoIP.Enabled = true;
      this.txtNrRack.Enabled = true;
      this.txtNrSlot.Enabled = true;
      this.btnConnettere.Enabled = true;
      this.btnSconnettere.Enabled = false;
      this.tlp_DB10.Enabled = false;
      this.tlp_DB30.Enabled = false;
      this.timer1.Enabled = false;
      this.timer2.Enabled = false;
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      try
      {
        this.result = this.Client.DBRead(10, 38, 10, this.db1Buffer);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        this.result = -1;
      }
      if ((uint) this.result > 0U)
        this.ShowResult(this.result);
      try
      {
        //  Setta la word del DB10 all'int che ha il PLC(?)
        //  dbBuffer = Array di byte
        this.db10.Word01 = S7.GetIntAt(this.db1Buffer, 0);
        this.db10.Word02 = S7.GetIntAt(this.db1Buffer, 2);
        this.db10.Word03 = S7.GetIntAt(this.db1Buffer, 4);
        this.db10.Word04 = S7.GetIntAt(this.db1Buffer, 6);
        this.db10.Word05 = S7.GetIntAt(this.db1Buffer, 8);
        //  Setta i vari label/etc... dell'UI
        Label lblValore01 = this.lblValore01;
        short num = this.db10.Word01;
        string str1 = num.ToString();
        lblValore01.Text = str1;
        Label lblValore02 = this.lblValore02;
        num = this.db10.Word02;
        string str2 = num.ToString();
        lblValore02.Text = str2;
        Label lblValore03 = this.lblValore03;
        num = this.db10.Word03;
        string str3 = num.ToString();
        lblValore03.Text = str3;
        Label lblValore04 = this.lblValore04;
        num = this.db10.Word04;
        string str4 = num.ToString();
        lblValore04.Text = str4;
        Label lblValore05 = this.lblValore05;
        num = this.db10.Word05;
        string str5 = num.ToString();
        lblValore05.Text = str5;
        //  Scrive il db10 nel file XML, noi prob. dovremmo usare il DBSQL
        CSerialDeserial.WriteFile(this.db10);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        this.result = -1;
      }
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      //  Legge il file XML, noi prob. dovremmo leggere dal DBSQL
      CSerialDeserial.ReadFile(ref this.db30);
      //  Penso che questo db3 sia di appoggio
      //  Non ho capito perchè ha fatto un nuovo buffer
      //  TODO: Capire perchè
      this.db3Buffer = new byte[10];
      S7.SetIntAt(this.db3Buffer, 0, this.db30.Word01);
      S7.SetIntAt(this.db3Buffer, 2, this.db30.Word02);
      S7.SetIntAt(this.db3Buffer, 4, this.db30.Word03);
      S7.SetIntAt(this.db3Buffer, 6, this.db30.Word04);
      S7.SetIntAt(this.db3Buffer, 8, this.db30.Word05);
      //  Scrive nel DB del PLC
      this.result = this.Client.DBWrite(30, 18, this.db3Buffer.Length, this.db3Buffer);
      //  Visualizza i risultati nell'UI
      if ((uint) this.result > 0U)
        this.ShowResult(this.result);
      this.lblValore31.Text = this.db30.Word01.ToString();
      Label lblValore32 = this.lblValore32;
      short num = this.db30.Word02;
      string str1 = num.ToString();
      lblValore32.Text = str1;
      Label lblValore33 = this.lblValore33;
      num = this.db30.Word03;
      string str2 = num.ToString();
      lblValore33.Text = str2;
      Label lblValore34 = this.lblValore34;
      num = this.db30.Word04;
      string str3 = num.ToString();
      lblValore34.Text = str3;
      Label lblValore35 = this.lblValore35;
      num = this.db30.Word05;
      string str4 = num.ToString();
      lblValore35.Text = str4;
    }
    
    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }
    
    //  Ignorate InitializeComponent() semplicemente crea l'UI della WFA
    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.lblStatus = new Label();
      this.panel1 = new Panel();
      this.btnSconnettere = new Button();
      this.btnConnettere = new Button();
      this.txtNrSlot = new TextBox();
      this.txtNrRack = new TextBox();
      this.txtIndirizzoIP = new TextBox();
      this.lblNrSlot = new Label();
      this.lblNrRack = new Label();
      this.lblIndirizzoIP = new Label();
      this.tlp_DB10 = new TableLayoutPanel();
      this.lblValore01 = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.lblValore02 = new Label();
      this.lblValore03 = new Label();
      this.lblValore04 = new Label();
      this.lblValore05 = new Label();
      this.tlp_DB30 = new TableLayoutPanel();
      this.lblValore31 = new Label();
      this.label13 = new Label();
      this.label14 = new Label();
      this.label15 = new Label();
      this.label16 = new Label();
      this.label17 = new Label();
      this.label18 = new Label();
      this.label19 = new Label();
      this.lblValore32 = new Label();
      this.lblValore33 = new Label();
      this.lblValore34 = new Label();
      this.lblValore35 = new Label();
      this.timer1 = new Timer(this.components);
      this.timer2 = new Timer(this.components);
      this.panel1.SuspendLayout();
      this.tlp_DB10.SuspendLayout();
      this.tlp_DB30.SuspendLayout();
      this.SuspendLayout();
      this.lblStatus.BackColor = Color.White;
      this.lblStatus.BorderStyle = BorderStyle.FixedSingle;
      this.lblStatus.Dock = DockStyle.Bottom;
      this.lblStatus.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStatus.Location = new Point(0, 391);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(882, 23);
      this.lblStatus.TabIndex = 0;
      this.panel1.BackColor = SystemColors.Control;
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.btnSconnettere);
      this.panel1.Controls.Add((Control) this.btnConnettere);
      this.panel1.Controls.Add((Control) this.txtNrSlot);
      this.panel1.Controls.Add((Control) this.txtNrRack);
      this.panel1.Controls.Add((Control) this.txtIndirizzoIP);
      this.panel1.Controls.Add((Control) this.lblNrSlot);
      this.panel1.Controls.Add((Control) this.lblNrRack);
      this.panel1.Controls.Add((Control) this.lblIndirizzoIP);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(882, 55);
      this.panel1.TabIndex = 1;
      this.btnSconnettere.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSconnettere.Location = new Point(711, 6);
      this.btnSconnettere.Name = "btnSconnettere";
      this.btnSconnettere.Size = new Size(158, 37);
      this.btnSconnettere.TabIndex = 2;
      this.btnSconnettere.Text = "SCONNETTERE";
      this.btnSconnettere.UseMnemonic = false;
      this.btnSconnettere.UseVisualStyleBackColor = true;
      this.btnSconnettere.Click += new EventHandler(this.btnSconnettere_Click);
      this.btnConnettere.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnConnettere.Location = new Point(549, 6);
      this.btnConnettere.Name = "btnConnettere";
      this.btnConnettere.Size = new Size(136, 37);
      this.btnConnettere.TabIndex = 2;
      this.btnConnettere.Text = "CONNETTERE";
      this.btnConnettere.UseMnemonic = false;
      this.btnConnettere.UseVisualStyleBackColor = true;
      this.btnConnettere.Click += new EventHandler(this.btnConnettere_Click);
      this.txtNrSlot.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtNrSlot.Location = new Point(438, 11);
      this.txtNrSlot.Name = "txtNrSlot";
      this.txtNrSlot.Size = new Size(48, 26);
      this.txtNrSlot.TabIndex = 1;
      this.txtNrSlot.Text = "1";
      this.txtNrSlot.TextAlign = HorizontalAlignment.Center;
      this.txtNrRack.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtNrRack.Location = new Point(306, 11);
      this.txtNrRack.Name = "txtNrRack";
      this.txtNrRack.Size = new Size(48, 26);
      this.txtNrRack.TabIndex = 1;
      this.txtNrRack.Text = "0";
      this.txtNrRack.TextAlign = HorizontalAlignment.Center;
      this.txtIndirizzoIP.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.txtIndirizzoIP.Location = new Point(101, 11);
      this.txtIndirizzoIP.Name = "txtIndirizzoIP";
      this.txtIndirizzoIP.Size = new Size(115, 26);
      this.txtIndirizzoIP.TabIndex = 1;
      this.txtIndirizzoIP.Text = "192.168.0.1";
      this.txtIndirizzoIP.TextAlign = HorizontalAlignment.Center;
      this.lblNrSlot.AutoSize = true;
      this.lblNrSlot.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNrSlot.Location = new Point(369, 17);
      this.lblNrSlot.Name = "lblNrSlot";
      this.lblNrSlot.Size = new Size(71, 16);
      this.lblNrSlot.TabIndex = 0;
      this.lblNrSlot.Text = "Nr. Slot : ";
      this.lblNrRack.AutoSize = true;
      this.lblNrRack.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNrRack.Location = new Point(231, 17);
      this.lblNrRack.Name = "lblNrRack";
      this.lblNrRack.Size = new Size(80, 16);
      this.lblNrRack.TabIndex = 0;
      this.lblNrRack.Text = "Nr. Rack : ";
      this.lblIndirizzoIP.AutoSize = true;
      this.lblIndirizzoIP.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblIndirizzoIP.Location = new Point(5, 17);
      this.lblIndirizzoIP.Name = "lblIndirizzoIP";
      this.lblIndirizzoIP.Size = new Size(99, 16);
      this.lblIndirizzoIP.TabIndex = 0;
      this.lblIndirizzoIP.Text = "Indirizzo IP  : ";
      this.tlp_DB10.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble;
      this.tlp_DB10.ColumnCount = 2;
      this.tlp_DB10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tlp_DB10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tlp_DB10.Controls.Add((Control) this.lblValore01, 1, 1);
      this.tlp_DB10.Controls.Add((Control) this.label1, 0, 0);
      this.tlp_DB10.Controls.Add((Control) this.label2, 1, 0);
      this.tlp_DB10.Controls.Add((Control) this.label3, 0, 1);
      this.tlp_DB10.Controls.Add((Control) this.label4, 0, 2);
      this.tlp_DB10.Controls.Add((Control) this.label5, 0, 3);
      this.tlp_DB10.Controls.Add((Control) this.label6, 0, 4);
      this.tlp_DB10.Controls.Add((Control) this.label7, 0, 5);
      this.tlp_DB10.Controls.Add((Control) this.lblValore02, 1, 2);
      this.tlp_DB10.Controls.Add((Control) this.lblValore03, 1, 3);
      this.tlp_DB10.Controls.Add((Control) this.lblValore04, 1, 4);
      this.tlp_DB10.Controls.Add((Control) this.lblValore05, 1, 5);
      this.tlp_DB10.Location = new Point(53, 92);
      this.tlp_DB10.Name = "tlp_DB10";
      this.tlp_DB10.RowCount = 6;
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB10.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB10.Size = new Size(355, 249);
      this.tlp_DB10.TabIndex = 2;
      this.lblValore01.BackColor = Color.White;
      this.lblValore01.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore01.Dock = DockStyle.Fill;
      this.lblValore01.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore01.Location = new Point(182, 36);
      this.lblValore01.Name = "lblValore01";
      this.lblValore01.Size = new Size(167, 39);
      this.lblValore01.TabIndex = 2;
      this.lblValore01.Text = "1";
      this.lblValore01.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.Dock = DockStyle.Fill;
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(6, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(167, 30);
      this.label1.TabIndex = 0;
      this.label1.Text = "DB10 (FROM PLC)";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.label2.Dock = DockStyle.Fill;
      this.label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(182, 3);
      this.label2.Name = "label2";
      this.label2.Size = new Size(167, 30);
      this.label2.TabIndex = 0;
      this.label2.Text = "VALORE WORD";
      this.label2.TextAlign = ContentAlignment.MiddleCenter;
      this.label3.Dock = DockStyle.Fill;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(6, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(167, 39);
      this.label3.TabIndex = 1;
      this.label3.Text = "1";
      this.label3.TextAlign = ContentAlignment.MiddleCenter;
      this.label4.Dock = DockStyle.Fill;
      this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(6, 78);
      this.label4.Name = "label4";
      this.label4.Size = new Size(167, 39);
      this.label4.TabIndex = 1;
      this.label4.Text = "2";
      this.label4.TextAlign = ContentAlignment.MiddleCenter;
      this.label5.Dock = DockStyle.Fill;
      this.label5.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(6, 120);
      this.label5.Name = "label5";
      this.label5.Size = new Size(167, 39);
      this.label5.TabIndex = 1;
      this.label5.Text = "3";
      this.label5.TextAlign = ContentAlignment.MiddleCenter;
      this.label6.Dock = DockStyle.Fill;
      this.label6.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(6, 162);
      this.label6.Name = "label6";
      this.label6.Size = new Size(167, 39);
      this.label6.TabIndex = 1;
      this.label6.Text = "4";
      this.label6.TextAlign = ContentAlignment.MiddleCenter;
      this.label7.Dock = DockStyle.Fill;
      this.label7.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(6, 204);
      this.label7.Name = "label7";
      this.label7.Size = new Size(167, 42);
      this.label7.TabIndex = 1;
      this.label7.Text = "5";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore02.BackColor = Color.White;
      this.lblValore02.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore02.Dock = DockStyle.Fill;
      this.lblValore02.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore02.Location = new Point(182, 78);
      this.lblValore02.Name = "lblValore02";
      this.lblValore02.Size = new Size(167, 39);
      this.lblValore02.TabIndex = 2;
      this.lblValore02.Text = "1";
      this.lblValore02.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore03.BackColor = Color.White;
      this.lblValore03.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore03.Dock = DockStyle.Fill;
      this.lblValore03.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore03.Location = new Point(182, 120);
      this.lblValore03.Name = "lblValore03";
      this.lblValore03.Size = new Size(167, 39);
      this.lblValore03.TabIndex = 2;
      this.lblValore03.Text = "1";
      this.lblValore03.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore04.BackColor = Color.White;
      this.lblValore04.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore04.Dock = DockStyle.Fill;
      this.lblValore04.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore04.Location = new Point(182, 162);
      this.lblValore04.Name = "lblValore04";
      this.lblValore04.Size = new Size(167, 39);
      this.lblValore04.TabIndex = 2;
      this.lblValore04.Text = "1";
      this.lblValore04.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore05.BackColor = Color.White;
      this.lblValore05.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore05.Dock = DockStyle.Fill;
      this.lblValore05.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore05.Location = new Point(182, 204);
      this.lblValore05.Name = "lblValore05";
      this.lblValore05.Size = new Size(167, 42);
      this.lblValore05.TabIndex = 2;
      this.lblValore05.Text = "1";
      this.lblValore05.TextAlign = ContentAlignment.MiddleCenter;
      this.tlp_DB30.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble;
      this.tlp_DB30.ColumnCount = 2;
      this.tlp_DB30.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tlp_DB30.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tlp_DB30.Controls.Add((Control) this.lblValore31, 1, 1);
      this.tlp_DB30.Controls.Add((Control) this.label13, 0, 0);
      this.tlp_DB30.Controls.Add((Control) this.label14, 1, 0);
      this.tlp_DB30.Controls.Add((Control) this.label15, 0, 1);
      this.tlp_DB30.Controls.Add((Control) this.label16, 0, 2);
      this.tlp_DB30.Controls.Add((Control) this.label17, 0, 3);
      this.tlp_DB30.Controls.Add((Control) this.label18, 0, 4);
      this.tlp_DB30.Controls.Add((Control) this.label19, 0, 5);
      this.tlp_DB30.Controls.Add((Control) this.lblValore32, 1, 2);
      this.tlp_DB30.Controls.Add((Control) this.lblValore33, 1, 3);
      this.tlp_DB30.Controls.Add((Control) this.lblValore34, 1, 4);
      this.tlp_DB30.Controls.Add((Control) this.lblValore35, 1, 5);
      this.tlp_DB30.Location = new Point(491, 92);
      this.tlp_DB30.Name = "tlp_DB30";
      this.tlp_DB30.RowCount = 6;
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Absolute, 30f));
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB30.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
      this.tlp_DB30.Size = new Size(355, 249);
      this.tlp_DB30.TabIndex = 2;
      this.lblValore31.BackColor = Color.White;
      this.lblValore31.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore31.Dock = DockStyle.Fill;
      this.lblValore31.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore31.Location = new Point(182, 36);
      this.lblValore31.Name = "lblValore31";
      this.lblValore31.Size = new Size(167, 39);
      this.lblValore31.TabIndex = 2;
      this.lblValore31.Text = "1";
      this.lblValore31.TextAlign = ContentAlignment.MiddleCenter;
      this.label13.Dock = DockStyle.Fill;
      this.label13.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label13.Location = new Point(6, 3);
      this.label13.Name = "label13";
      this.label13.Size = new Size(167, 30);
      this.label13.TabIndex = 0;
      this.label13.Text = "DB30 (TO PLC)";
      this.label13.TextAlign = ContentAlignment.MiddleCenter;
      this.label14.Dock = DockStyle.Fill;
      this.label14.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(182, 3);
      this.label14.Name = "label14";
      this.label14.Size = new Size(167, 30);
      this.label14.TabIndex = 0;
      this.label14.Text = "VALORE WORD";
      this.label14.TextAlign = ContentAlignment.MiddleCenter;
      this.label15.Dock = DockStyle.Fill;
      this.label15.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(6, 36);
      this.label15.Name = "label15";
      this.label15.Size = new Size(167, 39);
      this.label15.TabIndex = 1;
      this.label15.Text = "1";
      this.label15.TextAlign = ContentAlignment.MiddleCenter;
      this.label16.Dock = DockStyle.Fill;
      this.label16.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(6, 78);
      this.label16.Name = "label16";
      this.label16.Size = new Size(167, 39);
      this.label16.TabIndex = 1;
      this.label16.Text = "2";
      this.label16.TextAlign = ContentAlignment.MiddleCenter;
      this.label17.Dock = DockStyle.Fill;
      this.label17.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(6, 120);
      this.label17.Name = "label17";
      this.label17.Size = new Size(167, 39);
      this.label17.TabIndex = 1;
      this.label17.Text = "3";
      this.label17.TextAlign = ContentAlignment.MiddleCenter;
      this.label18.Dock = DockStyle.Fill;
      this.label18.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(6, 162);
      this.label18.Name = "label18";
      this.label18.Size = new Size(167, 39);
      this.label18.TabIndex = 1;
      this.label18.Text = "4";
      this.label18.TextAlign = ContentAlignment.MiddleCenter;
      this.label19.Dock = DockStyle.Fill;
      this.label19.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(6, 204);
      this.label19.Name = "label19";
      this.label19.Size = new Size(167, 42);
      this.label19.TabIndex = 1;
      this.label19.Text = "5";
      this.label19.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore32.BackColor = Color.White;
      this.lblValore32.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore32.Dock = DockStyle.Fill;
      this.lblValore32.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore32.Location = new Point(182, 78);
      this.lblValore32.Name = "lblValore32";
      this.lblValore32.Size = new Size(167, 39);
      this.lblValore32.TabIndex = 2;
      this.lblValore32.Text = "1";
      this.lblValore32.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore33.BackColor = Color.White;
      this.lblValore33.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore33.Dock = DockStyle.Fill;
      this.lblValore33.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore33.Location = new Point(182, 120);
      this.lblValore33.Name = "lblValore33";
      this.lblValore33.Size = new Size(167, 39);
      this.lblValore33.TabIndex = 2;
      this.lblValore33.Text = "1";
      this.lblValore33.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore34.BackColor = Color.White;
      this.lblValore34.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore34.Dock = DockStyle.Fill;
      this.lblValore34.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore34.Location = new Point(182, 162);
      this.lblValore34.Name = "lblValore34";
      this.lblValore34.Size = new Size(167, 39);
      this.lblValore34.TabIndex = 2;
      this.lblValore34.Text = "1";
      this.lblValore34.TextAlign = ContentAlignment.MiddleCenter;
      this.lblValore35.BackColor = Color.White;
      this.lblValore35.BorderStyle = BorderStyle.Fixed3D;
      this.lblValore35.Dock = DockStyle.Fill;
      this.lblValore35.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValore35.Location = new Point(182, 204);
      this.lblValore35.Name = "lblValore35";
      this.lblValore35.Size = new Size(167, 42);
      this.lblValore35.TabIndex = 2;
      this.lblValore35.Text = "1";
      this.lblValore35.TextAlign = ContentAlignment.MiddleCenter;
      this.timer1.Interval = 1000;
      this.timer1.Tick += new EventHandler(this.timer1_Tick);
      this.timer2.Interval = 1000;
      this.timer2.Tick += new EventHandler(this.timer2_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Silver;
      this.ClientSize = new Size(882, 414);
      this.Controls.Add((Control) this.tlp_DB30);
      this.Controls.Add((Control) this.tlp_DB10);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblStatus);
      this.Name = nameof (MainForm);
      this.Text = "Client SIEMENS";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.tlp_DB10.ResumeLayout(false);
      this.tlp_DB30.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
