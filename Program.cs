// Decompiled with JetBrains decompiler
// Type: CS_Client_Siemens.Program
// Assembly: CS_Client_Siemens, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 99E93972-B612-44C3-9448-CD860EF726C4
// Assembly location: F:\PLC_PROGETTO\CS_Client_Siemens_esempio_comunicazione\Debug\CS_Client_Siemens.exe

using System;
using System.Windows.Forms;

namespace CS_Client_Siemens
{
  internal static class Program
  {
    //  TODO: Chiedere a Caceffo/Bonato cosa caspita è [STAThread]
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainForm());
    }
  }
}
