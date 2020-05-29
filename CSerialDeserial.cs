// Decompiled with JetBrains decompiler
// Type: CS_Client_Siemens.CSerialDeserial
// Assembly: CS_Client_Siemens, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 99E93972-B612-44C3-9448-CD860EF726C4
// Assembly location: F:\PLC_PROGETTO\CS_Client_Siemens_esempio_comunicazione\Debug\CS_Client_Siemens.exe

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CS_Client_Siemens
{
  public static class CSerialDeserial
  {
    public static void WriteFile(CDataBlock db)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (CDataBlock));
      Stream stream = (Stream) new FileStream(db.DBName + ".xml", FileMode.Create, FileAccess.Write, FileShare.Read);
      xmlSerializer.Serialize(stream, (object) db);
      stream.Close();
    }

    public static void ReadFile(ref CDataBlock db)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (CDataBlock));
      try
      {
        Stream stream = (Stream) new FileStream(db.DBName + ".xml", FileMode.Open, FileAccess.Read, FileShare.Write);
        db = (CDataBlock) xmlSerializer.Deserialize(stream);
        stream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }
  }
}
