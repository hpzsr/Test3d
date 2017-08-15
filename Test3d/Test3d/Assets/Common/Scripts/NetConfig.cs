using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetConfig
{
    static string s_ipAddress = "127.0.0.1";
    //static string s_ipAddress = "222.73.65.206";
    //static string s_ipAddress = "syauth.51v.cn";
    //static string s_ipAddress = "10.224.5.110";

    public static int s_ipPort = 10001;
    //public static int s_ipPort = 10100;

    public static string s_qiniuDownloadUrl = "http://oru510uv8.bkt.clouddn.com/";

    public static string getIpAddress()
    {
        //return Dns.GetHostEntry(s_ipAddress).AddressList[0].ToString();

        return s_ipAddress;
    }
}
