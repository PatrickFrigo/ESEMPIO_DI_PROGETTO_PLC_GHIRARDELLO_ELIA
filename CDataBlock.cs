// Decompiled with JetBrains decompiler
// Type: CS_Client_Siemens.CDataBlock
// Assembly: CS_Client_Siemens, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 99E93972-B612-44C3-9448-CD860EF726C4
// Assembly location: F:\PLC_PROGETTO\CS_Client_Siemens_esempio_comunicazione\Debug\CS_Client_Siemens.exe

using System;

namespace CS_Client_Siemens
{
  [Serializable]
  public class CDataBlock : IDataBlock
  {
    private string dbName;
    private short word01;
    private short word02;
    private short word03;
    private short word04;
    private short word05;

    public string DBName
    {
      get
      {
        return this.dbName;
      }
      set
      {
        this.dbName = value;
      }
    }

    public short Word01
    {
      get
      {
        return this.word01;
      }
      set
      {
        this.word01 = value;
      }
    }

    public short Word02
    {
      get
      {
        return this.word02;
      }
      set
      {
        this.word02 = value;
      }
    }

    public short Word03
    {
      get
      {
        return this.word03;
      }
      set
      {
        this.word03 = value;
      }
    }

    public short Word04
    {
      get
      {
        return this.word04;
      }
      set
      {
        this.word04 = value;
      }
    }

    public short Word05
    {
      get
      {
        return this.word05;
      }
      set
      {
        this.word05 = value;
      }
    }
  }
}
