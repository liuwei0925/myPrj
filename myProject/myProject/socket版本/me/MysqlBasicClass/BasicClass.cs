using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

//mysql
using System.Web;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MysqlBasicClass
{
    public class BasicClass
    {
        public BasicClass()
        {
            conn = null;
            cmd = null;
            sdr = null;
            MysqlOperationLog = Application.StartupPath + "\\Mysql_Log.txt";

        }
        ~BasicClass()
        {

        }
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataReader sdr;
        MySqlConnectionStringBuilder connectionStringBuilder;
        string MysqlOperationLog;

        //log记录/
        public void WriteMysqlLog(String ex)
        {
            StreamWriter sw = null;
            DateTime CurrentTime = DateTime.Now;
            string MsgLog;
            if (File.Exists(MysqlOperationLog)==false)
	        {
		        File.CreateText(MysqlOperationLog);
	        }
            try
            {

                sw = File.AppendText(MysqlOperationLog);
                MsgLog = CurrentTime.ToString("yyyy-MM-dd  HH:mm:ss") + "  " + ex + "\r\n";
                sw.WriteLine(MsgLog);
            }
            finally
            {
                if (null != sw)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }


        //
        //数据库基础操作
        //
        public void connectMysql(string server, string user, string passWord, string dataBase)
        {//数据库连接
            //string appPath = Application.StartupPath + "\\IMS_AppConfig.ini";
            string conectString = "server=" + server + ";port=3306" + ";user id=" + user
                + ";password=" + passWord + ";database=" + dataBase + ";Max Pool Size=256;Charset=utf8;Allow User Variables=True";
            try
            {
                connectionStringBuilder = new MySqlConnectionStringBuilder(conectString);
            }
            catch (InvalidOperationException ex)
            {
                connectionStringBuilder = null;
            }
            catch (MySqlException ex)
            {
                connectionStringBuilder = null;
            }
            catch (ArgumentException ex)
            {
                connectionStringBuilder = null;
            }
            string logString;
            if (connectionStringBuilder != null)
            {
                if (string.IsNullOrEmpty(connectionStringBuilder.ToString()))
                {
                    logString = "connectMysql fail,connectionStringBuilder to string is null"
                                + conectString;
                    WriteMysqlLog(logString);
                    return;
                }

                conn = new MySqlConnection(connectionStringBuilder.ToString());
                logString = "connectMysql success: " + conectString;
                WriteMysqlLog(logString);
            }
            else
            {
                logString = "connectMysql fail,connectionStringBuilder is null "
                            + conectString;
                WriteMysqlLog(logString);
            }
        }
        MySqlConnection GetConnOpen()//开启一个连接
        {
            if (conn == null)
                return null;
            if (conn.State == ConnectionState.Closed)
            {
                
                
                    conn.Open();
                
                
            }
            return conn;
        }
        public void GetConnClose()//关闭一个连接
        {
            if (conn == null)
                return;
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        //执行mysql命令，返回受影响的行数。
        public int ExecuteNonQuery(String cmdText, CommandType cmdType)
        {
            int res = 0;
            MySqlConnection cnn = GetConnOpen();
            //判断连接和命令字符串是否为空
            if (cnn == null || string.IsNullOrEmpty(cmdText))
            {
                return res;
            }

            //判断连接状态是否是关闭，如果是关闭状态则返回。
            if (cnn.State == ConnectionState.Closed)
            {
                return res;
            }
            cmd = new MySqlCommand(cmdText, cnn);
            cmd.CommandType = cmdType;
            res = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return res;
        }
        //按照mysql的事务方式执行命令。返回的是受影响的表的行数
        public int MysqlTranscationExecuteNonQuery(String cmdText, CommandType cmdType)
        {
            int res = 0;
            MySqlTransaction trans = null;
            try
            {
                MySqlConnection cnn = GetConnOpen();
                //判断连接和命令串是否为空
                if (cnn == null || string.IsNullOrEmpty(cmdText))
                {
                    return res;
                }
                //判断连接状态如果为关闭，则返回。
                if (cnn.State == ConnectionState.Closed)
                {
                    return res;
                }

                cmd = new MySqlCommand(cmdText, cnn);
                cmd.CommandType = cmdType;
                trans = cnn.BeginTransaction();
                res = cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (SystemException ex)
            {
                trans.Rollback();
            }
            finally
            {
                cmd.Dispose();
            }
            return res;
        }

        //按照命令串和类型执行mysql命令，返回受影响的表的所有表项。
        public DataTable ExecuteQuery(String cmdText, CommandType cmdType)
        {
            DataTable dt = new DataTable();
            MySqlConnection cnn = GetConnOpen();
            //判断连接和命令串是否为空
            if (cnn == null || string.IsNullOrEmpty(cmdText))
            {
                return null;
            }
            //判断连接状态如果为关闭，则返回。
            if (cnn.State == ConnectionState.Closed)
            {
                return null;
            }

            cmd = new MySqlCommand(cmdText, cnn);
            cmd.CommandType = cmdType;
            sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);//执行完之后关闭此次连接
            dt.Load(sdr);
            return dt;
        }
        //建立表项
        public void CreateTable(string tableName, string tableInfo)
        {
            int ret = 0;
            string logString;
            //如果表名称和表信息其中有一项为空，则无法建表，返回。
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(tableInfo))
            {
                return;
            }
            string cmdString = "create table if not exists "
                              + tableName + " (" + tableInfo
                              + ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT "
                              + "CHARACTER SET utf8 COLLATE utf8_general_ci";
            
            ret = ExecuteNonQuery(cmdString, CommandType.Text);
            logString = "Create Table success!cmdString is: " + cmdString;
            WriteMysqlLog(logString);
            WriteMysqlLog("ret is: " + ret.ToString());
           
        }
        /// 依据sTableName删除表
        /// sTableName：表名。
        public void DeleteTable(string sTableName)
        {

            string logString;
            string cmdString;
            //判断各个入参是否为空。
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "DeleteTable() fail. sTableName is null.";
                WriteMysqlLog(logString);
                return;
            }
            //生成删除表的命令串
            cmdString = "drop table if exists " + sTableName;
            logString = "DeleteTable() cmdString is: " + cmdString;
            WriteMysqlLog(logString);
            ExecuteNonQuery(cmdString, CommandType.Text);

            logString = "DeleteTable() success. cmdString is:" + cmdString;

            WriteMysqlLog(logString);
            return;
        }
        /// 增加一行
        /// sTableName:表名称；
        /// sColumnsInfo:增加的列域名称串，每个域之间用","分开
        /// sColumnsValues:增加的列值串，每个值用","分开；顺序和sColumnsInfo的顺序是对应的
        /// 返回成功的行数
        public int AddRecordOperation(string sTableName, string sColumnsInfo, string sColumnsValues)
        {
            int ret = 0;
            string logString;
            string cmdString;
            //判断各个入参是否为空。
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "AddRecordOperation() fail. table name is null.";
                WriteMysqlLog(logString);
                return ret;
            }

            //生成插入一行的命令串
            cmdString = "insert into " + sTableName + " (" + sColumnsInfo
                        + ") " + "values (" + sColumnsValues + ")";
            logString = "AddRecordOperation() cmdString is:" + cmdString;
            WriteMysqlLog(logString);
           // cmdString = "insert into PSX_IMS_USER(name) values(" + "\"" + "123" + "\"" + ")";
            ret = ExecuteNonQuery(cmdString, CommandType.Text);

            if (ret != 0)
            {
                logString = "AddRecordOperation() success. cmdString is:" + cmdString
                            + ". ret is: " + ret.ToString();
            }
            else
            {
                logString = "AddRecordOperation() fail. cmdString is:" + cmdString
                                          + ". ret is: " + ret.ToString();
            }
            WriteMysqlLog(logString);
            return ret;
        }


        /// 依据sWhereInfo信息删除表项
        /// sTableName：表名
        /// sWhereInfo：删除的条件
        /// 返回受影响的行数.大于等于零都是正确执行了命令。
        public int DeleteRecordsOperation(string sTableName, string sWhereInfo)
        {
            int ret = 0;
            string logString;
            string cmdString;
            //判断各个入参是否为空。
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "DeleteRecordsOperation() fail. sTableName is null.";
                WriteMysqlLog(logString);
                return ret;
            }
            if (string.IsNullOrEmpty(sWhereInfo))
            {
                logString = "DeleteRecordsOperation() sWhereInfo is null.";
                WriteMysqlLog(logString);
                //生成删除的命令串
                cmdString = "delete from " + sTableName;
            }
            else
            {
                //生成删除的命令串
                cmdString = "delete from " + sTableName + " where " + sWhereInfo;
            }

            ret = ExecuteNonQuery(cmdString, CommandType.Text);

            if (ret > 0)
            {
                logString = "DeleteRecordsOperation() success. cmdString is:" + cmdString
                             + ". ret is: " + ret.ToString();
            }
            else
            {
                logString = "DeleteRecordsOperation() fail. cmdString is:" + cmdString
                             + ". ret is: " + ret.ToString();
            }
            WriteMysqlLog(logString);
            return ret;
        }
        /// 获取表中记录信息
        /// sTableName:表名
        /// sWhereInfo:where信息；
        /// 返回ArrayList。每个记录格式columnname1:columnvalue1;columnname2:columnvalue2...
        public ArrayList GetRecordsOperation(string sTableName, string sWhereInfo)
        {
            string logString;
            string cmdString;
            ArrayList alRetString = new ArrayList();
            string sTmpString = "";
            DataTable dtRetDataTable;
            //判断表名称是否为空。为空返回
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "GetRecordsOperation() fail, sTableName is null.";
                WriteMysqlLog(logString);
                return alRetString;
            }
            //判断sWhereInfo是否为空。为空返回
            if (string.IsNullOrEmpty(sWhereInfo))
            {
                logString = "GetRecordsOperation() , sWhereInfo is null.";
                WriteMysqlLog(logString);
                cmdString = "select * from " + sTableName;
            }
            else
            {
                cmdString = "select * from " + sTableName + " where " + sWhereInfo;
            }
            
            logString = "GetRecordsOperation() cmdString is: " + cmdString;
            WriteMysqlLog(logString);
            dtRetDataTable = ExecuteQuery(cmdString, CommandType.Text);
            if (dtRetDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtRetDataTable.Rows.Count; i++)
                {
                   
                    for (int j = 0; j < dtRetDataTable.Columns.Count; j++)
                    {
                        
                        if (j == (dtRetDataTable.Columns.Count - 1))
                        {
                            sTmpString += dtRetDataTable.Columns[j].ColumnName.ToString() + ":"
                                         + dtRetDataTable.Rows[i][j].ToString();
                        }
                        else
                        {
                            sTmpString += dtRetDataTable.Columns[j].ColumnName.ToString() + ":"
                                         + dtRetDataTable.Rows[i][j].ToString() + ";";
                        }
                    }
                    logString = i.ToString() + " row is: " + sTmpString;
                    WriteMysqlLog(logString);
                    alRetString.Add(sTmpString);
                    sTmpString = "";
                }
                    
            }

            logString = "GetRecordsOperation() success. sTableName is:" + sTableName
                        + " sWhereInfo is: " + sWhereInfo;

            WriteMysqlLog(logString);
            return alRetString;
        }
        
        /// 获取表中行数
        /// sTableName:表名
        public int GetRecordCount(string sTableName)
        {
            int ret = -1;
            string logString;
            string cmdString;
            DataTable dtRetDataTable;
            //判断表名称是否为空。为空返回
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "GetRecordCount() fail, sTableName is null.";
                WriteMysqlLog(logString);
                return ret;
            }

            cmdString = "select * from " + sTableName;
            logString = "GetRecordCount() cmdString is: " + cmdString;
            WriteMysqlLog(logString);
            dtRetDataTable = ExecuteQuery(cmdString, CommandType.Text);
            ret = dtRetDataTable.Rows.Count;

            logString = "GetRecordCount() success. sTableName is:" + sTableName
                        + ". row count is:" + ret.ToString();
           
            WriteMysqlLog(logString);
            return ret;
        }



        //修改表内容。命令格式：updata tableName set col_name1 = value1,.. where id=id1, limit 1
        /// sTableName:表名称；
        /// sSetString:需要修改的set之后的字符串；
        /// sWhereInfo：需要修改的where信息；
        /// iLimit：受影响的行数限制；

        public int UpdateRecordsOperation(string sTableName, string sSetString, string sWhereInfo, int iLimit)
        {
            int ret = -1;
            string logString;
            string cmdString;

            //判断表名称是否为空。为空返回
            if (string.IsNullOrEmpty(sTableName))
            {
                logString = "UpdateRecordsOperation() fail, sTableName is null.";
                WriteMysqlLog(logString);
                return ret;
            }
            if (string.IsNullOrEmpty(sSetString))
            {
                logString = "UpdateRecordsOperation() fail, sSetString is null.";
                WriteMysqlLog(logString);
                return ret;
            }
            if (iLimit != 0)
            {
                cmdString = "update " + sTableName + " set " + sSetString + " where "
                        + sWhereInfo + " limit " + iLimit.ToString();
            }
            else
            {
                cmdString = "update " + sTableName + " set " + sSetString + " where "
                        + sWhereInfo;
            }
            logString = "UpdateRecordsOperation() cmdString is: " + cmdString;
            WriteMysqlLog(logString);
            ret = ExecuteNonQuery(cmdString, CommandType.Text);

            if (ret != 0)
            {
                logString = "UpdateRecordsOperation() success. cmdString is:" + cmdString
                            + ". ret is: " + ret.ToString();
            }
            else
            {
                logString = "UpdateRecordsOperation() fail. cmdString is:" + cmdString
                            + ". ret is: " + ret.ToString();
            }
            WriteMysqlLog(logString);
            return ret;
        }
        //初始化主键
        public void initTableID(string sTableName)
        {
            string logString;
            string cmdString;
            if (string.IsNullOrEmpty(sTableName))
            {
                return;
            }
            cmdString = "alter table " + sTableName + " AUTO_INCREMENT=1";
            ExecuteNonQuery(cmdString, CommandType.Text);
        }
    };
  
    //定义网关表的结构体
    public struct GATEWAY
    {
        public int id;
        public string name;
        public string sipNum;
        public string pocNum;
        public string groupid;
        public string groupNum;
        public string type;
        public string available;
    }
    //定义用户信息表的结构体
    public struct USER_INFO
    {
        public int id;
        public string phone;
        public string name;
        public string localRoomId;
        public string serverRoomId;
        public string type;

    }
   
    //高级管理类
    public class ManagerTables
    {
        public ManagerTables()
        {
            MysqlBasicMethods = new BasicClass();
            sUserInfoTableName   = "PSX_IMS_USER";           //用户信息表
            sGatewayTableName    = "PSX_IMS_GATEWAY";             //网关表
        }
        ~ManagerTables()
        {
            if (MysqlBasicMethods != null)
            {
                MysqlBasicMethods.GetConnClose();
                MysqlBasicMethods = null;
            }
        }
        //建立各个表
        public int CreateAllTable(string sServerIp, string sMysqlUserName, string sMysqlUserPass, string sMysqlDataBasaName)
        {
           
          
            string sGatewayTableString = "id int(10) primary key not null auto_increment,"
                                         + "name varchar(60),sipNum varchar(20),pocNum varchar(20),"
                                         + "groupid varchar(20), groupNum varchar(60), type varchar(20),"
                                         + "available varchar(20) default 0";
          
            string sUserInfoTableString = "id int(10) primary key not null auto_increment,"
                                          + "phone varchar(20),name varchar(60), localRoomId varchar(20) default 0,"
                                          + "serverRoomId varchar(20) default 0, type  varchar(20) ";
           
            MysqlBasicMethods.connectMysql(sServerIp, sMysqlUserName, sMysqlUserPass, sMysqlDataBasaName);


            try
            {
                //建立网关表
                MysqlBasicMethods.CreateTable(sGatewayTableName, sGatewayTableString);
                //建用户信息表
                MysqlBasicMethods.CreateTable(sUserInfoTableName, sUserInfoTableString);
            }
            catch (MySqlException ex)
            {
                return -1;
            }
            return 0;
        }
       

        public int AddGateway(GATEWAY gateway)
        {
            string sColumnsInfo = "";
            string sColumnsValues = "";
            string sLogString = "AddGatewayTableRecord() ";
            int iRet = 0;
          
            if (!string.IsNullOrEmpty(gateway.name))
            {
                sColumnsInfo += "name";
                sColumnsValues += "\"" + gateway.name + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.sipNum))
            {
                sColumnsInfo += ",sipNum";
                sColumnsValues += "," + "\"" + gateway.sipNum + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.pocNum))
            {
                sColumnsInfo += ",pocNum";
                sColumnsValues += "," + "\"" + gateway.pocNum + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.groupid))
            {
                sColumnsInfo += ",groupid";
                sColumnsValues += "," + "\"" + gateway.groupid + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.groupNum))
            {
                sColumnsInfo += ",groupNum";
                sColumnsValues += "," + "\"" + gateway.groupNum + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.type))
            {
                sColumnsInfo += ",type";
                sColumnsValues += "," + "\"" + gateway.type + "\"";
            }
            if (!string.IsNullOrEmpty(gateway.available))
            {
                sColumnsInfo += ",available";
                sColumnsValues += "," + "\"" + gateway.available + "\"";
            }

            sLogString += "sColumnsInfo is: " + sColumnsInfo + "\n"
                          + "; sColumnsValues is: " + sColumnsValues + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的增加记录函数
            iRet = MysqlBasicMethods.AddRecordOperation(sGatewayTableName, sColumnsInfo, sColumnsValues);
            if (iRet > 0)
            {
                sLogString = "AddGatewayTableRecord() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "AddGatewayTableRecord() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }
        /// <summary>
        /// 删除网关表记录
        /// </summary>
        /// <param name="sIdValue"></param>网关表的id值
        /// <param name="sSipNumberValue"></param>网关对应的sip号码
        /// <param name="sPocNumberValue"></param>网关对应的poc号码
        /// <param name="sSpTypeValue"></param>网关对应的运营商类型
        /// <param name="sBelongGroupidValue"></param>网关对应的groupid值
        /// <returns></returns>
        public int DeleteGateway(GATEWAY gateway)
        {
            string sWhereInfo = "";
            string sLogString = "DeleteGatewayTableRecords() ";
            int iRet = 0;
            int iFlag = 0;
            if (!string.IsNullOrEmpty(gateway.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + gateway.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + gateway.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.sipNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "sipNum=" + "\"" + gateway.sipNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and sipNum=" + "\"" + gateway.sipNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.pocNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "pocNum=" + "\"" + gateway.pocNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and pocNum=" + "\"" + gateway.pocNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupid))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupid=" + "\"" + gateway.groupid + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupid=" + "\"" + gateway.groupid + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupNum=" + "\"" + gateway.groupNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupNum=" + "\"" + gateway.groupNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type=" + "\"" + gateway.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type=" + "\"" + gateway.type + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.available))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "available=" + "\"" + gateway.available + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and available=" + "\"" + gateway.available + "\"";
                }
            }
            sLogString += "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的删除记录函数
            iRet = MysqlBasicMethods.DeleteRecordsOperation(sGatewayTableName, sWhereInfo);
            if (iRet > 0)
            {
                sLogString = "DeleteGatewayTableRecords() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "DeleteGatewayTableRecords() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }
        /// <summary>
        /// 更新网关表记录，可以是一条，也可以是多条记录
        /// </summary>
        /// <param name="sIdValue"></param>要设置的网关新id值，不改变为空
        /// <param name="sSipNumberValue"></param>要设置的网关新sip号码值，不改变为空
        /// <param name="sPocNumberValue"></param>要设置的网关新poc号码值，不改变为空
        /// <param name="sSpTypeValue"></param>要设置的网关新运营商类型值，不改变为空
        /// <param name="sBelongGroupidValue"></param>要设置的网关新所属groupid值，不改变为空
        /// <param name="sWhereIdValue"></param>要更改网关表的条件中id值，与这个值无关为空
        /// <param name="sWhereSipNumberValue"></param>要更改网关表的条件中sip号码值，与这个值无关为空
        /// <param name="sWherePocNumberValue"></param>要更改网关表的条件中poc号码值，与这个值无关为空
        /// <param name="sWhereSpTypeValue"></param>要更改网关表的条件中运营商类型值，与这个值无关为空
        /// <param name="sWhereBelongGroupidValue"></param>要更改网关表的条件中网关所属groupid值，与这个值无关为空
        /// <returns></returns>
        public int UpdateGateway(GATEWAY gateway, GATEWAY whereGateway)
        {
            string sSetString = "";
            string sWhereInfo = "";
            string sLogString = "UpdateGatewayTableRecords() ";
            int iRet = 0;
            int iFlag = 0;
            //生成set串
            if (!string.IsNullOrEmpty(gateway.name))
            {
                if (iFlag == 0)
                {
                    sSetString += "name=" + "\"" + gateway.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",name=" + "\"" + gateway.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.sipNum))
            {
                if (iFlag == 0)
                {
                    sSetString += "sipNum=" + "\"" + gateway.sipNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",sipNum=" + "\"" + gateway.sipNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.pocNum))
            {
                if (iFlag == 0)
                {
                    sSetString += "pocNum=" + "\"" + gateway.pocNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",pocNum=" + "\"" + gateway.pocNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupid))
            {
                if (iFlag == 0)
                {
                    sSetString += "groupid=" + "\"" + gateway.groupid + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",groupid=" + "\"" + gateway.groupid + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupNum))
            {
                if (iFlag == 0)
                {
                    sSetString += "groupNum=" + "\"" + gateway.groupNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",groupNum=" + "\"" + gateway.groupNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.type))
            {
                if (iFlag == 0)
                {
                    sSetString += "type=" + "\"" + gateway.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",type=" + "\"" + gateway.type + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.available))
            {
                if (iFlag == 0)
                {
                    sSetString += "available=" + "\"" + gateway.available + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",available=" + "\"" + gateway.available + "\"";
                }
            }
            //生成where串
            iFlag = 0;
            if (!string.IsNullOrEmpty(whereGateway.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + whereGateway.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + whereGateway.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.sipNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "sipNum=" + "\"" + whereGateway.sipNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and sipNum=" + "\"" + whereGateway.sipNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.pocNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "pocNum=" + "\"" + whereGateway.pocNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and pocNum=" + "\"" + whereGateway.pocNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.groupid))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupid=" + "\"" + whereGateway.groupid + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupid=" + "\"" + whereGateway.groupid + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.groupNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupNum=" + "\"" + whereGateway.groupNum  + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupNum="  + "\"" + whereGateway.groupNum  + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type=" + "\"" + whereGateway.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type=" + "\"" + whereGateway.type + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereGateway.available))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "available=" +  "\"" + whereGateway.available + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and available=" + "\""+ whereGateway.available + "\"";
                }
            }
            sLogString += "sSetString is: " + sSetString + "\n"
                          + "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的更新记录函数
            iRet = MysqlBasicMethods.UpdateRecordsOperation(sGatewayTableName, sSetString, sWhereInfo, 0);
            if (iRet > 0)
            {
                sLogString = "UpdateGatewayTableRecords() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "UpdateGatewayTableRecords() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }
  
        /// <summary>
        /// 得到网关表记录，可以是一条记录，也可以是多条记录
        /// </summary>
        /// <param name="sIdValue"></param>查找条件中的网关记录id值，如果无关为空
        /// <param name="sSipNumberValue"></param>查找条件中的网关记录sip_number值，如果无关为空
        /// <param name="sPocNumberValue"></param>查找条件中的网关记录poc_number值，如果无关为空
        /// <param name="sSpTypeValue"></param>查找条件中的网关记录网关所属运营商值，如果无关为空
        /// <param name="sBelongGroupidValue"></param>查找条件中的网关记录网关所属groupid值，如果无关为空
        /// <returns></returns>返回变长的数据结构数组，每条记录放在一个结构体中
        public GATEWAY[] GetGateway(GATEWAY gateway)
        {
            string sWhereInfo = "";
            string sLogString = "GetGatewayTableRecords_new() ";
            ArrayList alTmpRet = new ArrayList();
            Char[] cSplitColumnStr = { ';' };//分隔每一列的分隔符
            Char[] cSplitFieldStr = { ':' };//分隔每个列名称和列值的分隔符
            string[] sSplitColumninfo;
            string[] sSplitFieldinfo;
            GATEWAY[] stGatewayRecord;
            int iFlag = 0;

            //生成where串
            if (!string.IsNullOrEmpty(gateway.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + gateway.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + gateway.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.sipNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "sipNum=" + "\"" + gateway.sipNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and sipNum=" + "\"" + gateway.sipNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.pocNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "pocNum=" + "\"" + gateway.pocNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and pocNum=" + "\"" + gateway.pocNum + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupid))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupid=" + "\"" + gateway.groupid + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupid=" + "\"" + gateway.groupid + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.groupNum))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "groupNum=" + "\"" + gateway.groupNum + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and groupNum=" + "\""+gateway.groupNum  + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type =" + "\"" + gateway.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type =" + "\"" + gateway.type + "\"";
                }
            }
            if (!string.IsNullOrEmpty(gateway.available))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "available="  + "\"" + gateway.available  + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and available="  + "\"" + gateway.available  + "\"";
                }
            }
            sLogString += "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的GET记录函数
            alTmpRet = MysqlBasicMethods.GetRecordsOperation(sGatewayTableName, sWhereInfo);

            if (alTmpRet.Count > 0)
            {
                stGatewayRecord = new GATEWAY[alTmpRet.Count];
                for (int i = 0; i < alTmpRet.Count; i++)
                {
                    sSplitColumninfo = Convert.ToString(alTmpRet[i]).Split(cSplitColumnStr);
                    for (int j = 0; j < sSplitColumninfo.Length; j++)
                    {
                        sSplitFieldinfo = Convert.ToString(sSplitColumninfo[j]).Split(cSplitFieldStr);
                        switch (sSplitFieldinfo[0])
                        {
                            //case "id":
                            //    {
                            //        if (!string.IsNullOrEmpty(sSplitFieldinfo[1]))
                            //        {
                            //            stGatewayRecord[i].id = Convert.ToInt32(sSplitFieldinfo[1]);
                            //        }
                            //    }
                            //    break;
                            case "name":
                                {
                                    stGatewayRecord[i].name = sSplitFieldinfo[1];
                                }
                                break;
                            case "sipNum":
                                {
                                    stGatewayRecord[i].sipNum = sSplitFieldinfo[1];
                                }
                                break;
                            case "pocNum":
                                {
                                    stGatewayRecord[i].pocNum = sSplitFieldinfo[1];
                                }
                                break;
                            case "groupid":
                                {
                                    stGatewayRecord[i].groupid = sSplitFieldinfo[1];
                                }
                                break;
                            case "groupNum":
                                {
                                    stGatewayRecord[i].groupNum = sSplitFieldinfo[1];
                                }
                                break;
                            case "type":
                                {

                                    stGatewayRecord[i].type = sSplitFieldinfo[1];
                                }
                                break;
                            case "available":
                                {
                                    stGatewayRecord[i].available = sSplitFieldinfo[1];
                                }
                                break;
                            default:
                                break;
                        }
                    }

                }

                sLogString = "GetGatewayTableRecords_new() success.";
            }
            else
            {
                stGatewayRecord = new GATEWAY[0];
                sLogString = "GetGatewayTableRecords_new() fail.";
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);

            return stGatewayRecord;
        }



        public int AddUser(USER_INFO userInfo)
        {
            string sColumnsInfo = "";
            string sColumnsValues = "";
            string sLogString = "AddUserTableRecord() ";
            int iRet = 0;
            if (!string.IsNullOrEmpty(userInfo.phone))
            {
                sColumnsInfo += "phone";
                sColumnsValues += "\"" + userInfo.phone + "\"";
            }
            if (!string.IsNullOrEmpty(userInfo.name))
            {
                sColumnsInfo += ",name";
                sColumnsValues += "," + "\"" + userInfo.name + "\"";
            }
            if (!string.IsNullOrEmpty(userInfo.localRoomId))
            {
                sColumnsInfo += ",localRoomId";
                sColumnsValues += "," + "\"" + userInfo.localRoomId + "\"";
            }
            if (!string.IsNullOrEmpty(userInfo.serverRoomId))
            {
                sColumnsInfo += ",serverRoomId";
                sColumnsValues += "," + "\"" + userInfo.serverRoomId + "\"";
            }
            if (!string.IsNullOrEmpty(userInfo.type))
            {
                sColumnsInfo += ",type";
                sColumnsValues += "," + "\"" +userInfo.type + "\"";
            }
            

            sLogString += "sColumnsInfo is: " + sColumnsInfo + "\n"
                          + "; sColumnsValues is: " + sColumnsValues + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的增加记录函数
            iRet = MysqlBasicMethods.AddRecordOperation(sUserInfoTableName, sColumnsInfo, sColumnsValues);
            if (iRet > 0)
            {
                sLogString = "AddUserTableRecord() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "AddUserTableRecord() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }
        /// <summary>
        /// 删除网关表记录
        /// </summary>
        /// <param name="sIdValue"></param>网关表的id值
        /// <param name="sSipNumberValue"></param>网关对应的sip号码
        /// <param name="sPocNumberValue"></param>网关对应的poc号码
        /// <param name="sSpTypeValue"></param>网关对应的运营商类型
        /// <param name="sBelongGroupidValue"></param>网关对应的groupid值
        /// <returns></returns>
        public int DeleteUser(USER_INFO userInfo)
        {
            string sWhereInfo = "";
            string sLogString = "DeleteUserTableRecords() ";
            int iRet = 0;
            int iFlag = 0;
            if (!string.IsNullOrEmpty(userInfo.phone))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "phone=" + "\"" + userInfo.phone + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and phone=" + "\"" + userInfo.phone + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + userInfo.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + userInfo.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.localRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.serverRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type=" + "\"" + userInfo.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type=" + "\"" + userInfo.type + "\"";
                }
            }
           
            sLogString += "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的删除记录函数
            iRet = MysqlBasicMethods.DeleteRecordsOperation(sUserInfoTableName, sWhereInfo);
            if (iRet > 0)
            {
                sLogString = "DeleteUserTableRecords() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "DeleteUserTableRecords() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }
        /// <summary>
        /// 更新网关表记录，可以是一条，也可以是多条记录
        /// </summary>
        /// <param name="sIdValue"></param>要设置的网关新id值，不改变为空
        /// <param name="sSipNumberValue"></param>要设置的网关新sip号码值，不改变为空
        /// <param name="sPocNumberValue"></param>要设置的网关新poc号码值，不改变为空
        /// <param name="sSpTypeValue"></param>要设置的网关新运营商类型值，不改变为空
        /// <param name="sBelongGroupidValue"></param>要设置的网关新所属groupid值，不改变为空
        /// <param name="sWhereIdValue"></param>要更改网关表的条件中id值，与这个值无关为空
        /// <param name="sWhereSipNumberValue"></param>要更改网关表的条件中sip号码值，与这个值无关为空
        /// <param name="sWherePocNumberValue"></param>要更改网关表的条件中poc号码值，与这个值无关为空
        /// <param name="sWhereSpTypeValue"></param>要更改网关表的条件中运营商类型值，与这个值无关为空
        /// <param name="sWhereBelongGroupidValue"></param>要更改网关表的条件中网关所属groupid值，与这个值无关为空
        /// <returns></returns>
        public int UpdateUser(USER_INFO userInfo, USER_INFO whereUserInfo)
        {
            string sSetString = "";
            string sWhereInfo = "";
            string sLogString = "UpdateUserTableRecords() ";
            int iRet = 0;
            int iFlag = 0;
            //生成set串
            if (!string.IsNullOrEmpty(userInfo.phone))
            {
                if (iFlag == 0)
                {
                    sSetString += "phone=" + "\"" + userInfo.phone + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",phone=" + "\"" + userInfo.phone + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.name))
            {
                if (iFlag == 0)
                {
                    sSetString += "name=" + "\"" + userInfo.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",name=" + "\"" + userInfo.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.localRoomId))
            {
                if (iFlag == 0)
                {
                    sSetString += "localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.serverRoomId))
            {
                if (iFlag == 0)
                {
                    sSetString += "serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.type))
            {
                if (iFlag == 0)
                {
                    sSetString += "type=" +"\"" + userInfo.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sSetString += ",type="  + "\"" +userInfo.type  + "\"";
                }
            }
          
            //生成where串
            iFlag = 0;
            if (!string.IsNullOrEmpty(whereUserInfo.phone))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "phone=" + "\"" + whereUserInfo.phone + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and phone=" + "\"" + whereUserInfo.phone + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereUserInfo.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + whereUserInfo.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + whereUserInfo.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereUserInfo.localRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "localRoomId=" + "\"" + whereUserInfo.localRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and localRoomId=" + "\"" + whereUserInfo.localRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereUserInfo.serverRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "serverRoomId=" + "\"" + whereUserInfo.serverRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and serverRoomId=" + "\"" + whereUserInfo.serverRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(whereUserInfo.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type=" + "\""+whereUserInfo.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type=" + "\""+ whereUserInfo.type + "\"";
                }
            }
           
            sLogString += "sSetString is: " + sSetString + "\n"
                          + "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的更新记录函数
            iRet = MysqlBasicMethods.UpdateRecordsOperation(sUserInfoTableName, sSetString, sWhereInfo, 0);
            if (iRet > 0)
            {
                sLogString = "UpdateUserTableRecords() success. iRet is: " + iRet.ToString();
            }
            else
            {
                sLogString = "UpdateUserTableRecords() fail. iRet is: " + iRet.ToString();
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            return iRet;
        }

        /// <summary>
        /// 得到网关表记录，可以是一条记录，也可以是多条记录
        /// </summary>
        /// <param name="sIdValue"></param>查找条件中的网关记录id值，如果无关为空
        /// <param name="sSipNumberValue"></param>查找条件中的网关记录sip_number值，如果无关为空
        /// <param name="sPocNumberValue"></param>查找条件中的网关记录poc_number值，如果无关为空
        /// <param name="sSpTypeValue"></param>查找条件中的网关记录网关所属运营商值，如果无关为空
        /// <param name="sBelongGroupidValue"></param>查找条件中的网关记录网关所属groupid值，如果无关为空
        /// <returns></returns>返回变长的数据结构数组，每条记录放在一个结构体中
        public USER_INFO[] GetUser(USER_INFO userInfo)
        {
            string sWhereInfo = "";
            string sLogString = "GetUserTableRecords() ";
            ArrayList alTmpRet = new ArrayList();
            Char[] cSplitColumnStr = { ';' };//分隔每一列的分隔符
            Char[] cSplitFieldStr = { ':' };//分隔每个列名称和列值的分隔符
            string[] sSplitColumninfo;
            string[] sSplitFieldinfo;
            USER_INFO[] stGatewayRecord;
            int iFlag = 0;

            //生成where串
            if (!string.IsNullOrEmpty(userInfo.phone))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "phone=" + "\"" + userInfo.phone + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and phone=" + "\"" + userInfo.phone + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.name))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "name=" + "\"" + userInfo.name + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and name=" + "\"" + userInfo.name + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.localRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and localRoomId=" + "\"" + userInfo.localRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.serverRoomId))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and serverRoomId=" + "\"" + userInfo.serverRoomId + "\"";
                }
            }
            if (!string.IsNullOrEmpty(userInfo.type))
            {
                if (iFlag == 0)
                {
                    sWhereInfo += "type=" + "\"" + userInfo.type + "\"";
                    iFlag = 1;
                }
                else
                {
                    sWhereInfo += " and type=" + "\"" + userInfo.type + "\"";
                }
            }
            
            sLogString += "sWhereInfo is: " + sWhereInfo + "\n";
            MysqlBasicMethods.WriteMysqlLog(sLogString);
            //调用基本的GET记录函数
            alTmpRet = MysqlBasicMethods.GetRecordsOperation(sUserInfoTableName, sWhereInfo);

            if (alTmpRet.Count > 0)
            {
                stGatewayRecord = new USER_INFO[alTmpRet.Count];
                for (int i = 0; i < alTmpRet.Count; i++)
                {
                    sSplitColumninfo = Convert.ToString(alTmpRet[i]).Split(cSplitColumnStr);
                    for (int j = 0; j < sSplitColumninfo.Length; j++)
                    {
                        sSplitFieldinfo = sSplitColumninfo[j].Split(cSplitFieldStr);
                        switch (sSplitFieldinfo[0])
                        {

                            case "phone":
                                stGatewayRecord[i].phone = sSplitFieldinfo[1];
                                break;
                            case "name":
                                stGatewayRecord[i].name = sSplitFieldinfo[1];
                                break;
                            case "localRoomId":
                                stGatewayRecord[i].localRoomId = sSplitFieldinfo[1];
                                break;
                            case "serverRoomId":
                                stGatewayRecord[i].serverRoomId = sSplitFieldinfo[1];
                                break;
                            case "type":
                                stGatewayRecord[i].type = sSplitFieldinfo[1];
                                break;
                            default:
                                break;
                        }
                    }

                }

                sLogString = "GetUserTableRecords() success.";
            }
            else
            {
                stGatewayRecord = new USER_INFO[0];
                sLogString = "GetUserTableRecords() fail.";
            }
            MysqlBasicMethods.WriteMysqlLog(sLogString);

            return stGatewayRecord;
        }

        private BasicClass MysqlBasicMethods;
        private string sUserInfoTableName;        //用户信息表
        private string sGatewayTableName;         //网关表

       
    }
}