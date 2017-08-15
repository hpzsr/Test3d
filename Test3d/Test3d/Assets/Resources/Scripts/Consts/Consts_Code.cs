using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Consts_Code
{
    public enum Code
    {
        Code_Ok = 1000,
        Code_ParamError,                // 参数错误
        Code_RealNameFail,              // 实名认证失败
        Code_ErrorNetInterface,         // 错误的网络接口
    };

    public enum GameId
    {
        GameId_njmj = 1,
        GameId_tlj,
        GameId_xlch,
    };
}
