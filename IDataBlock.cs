// Decompiled with JetBrains decompiler
// Type: CS_Client_Siemens.IDataBlock
// Assembly: CS_Client_Siemens, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 99E93972-B612-44C3-9448-CD860EF726C4
// Assembly location: F:\PLC_PROGETTO\CS_Client_Siemens_esempio_comunicazione\Debug\CS_Client_Siemens.exe

namespace CS_Client_Siemens
{
  public interface IDataBlock
  {
    string DBName { get; set; }

    short Word01 { get; set; }

    short Word02 { get; set; }

    short Word03 { get; set; }

    short Word04 { get; set; }

    short Word05 { get; set; }
  }
}
