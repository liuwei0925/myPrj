using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Web服务测试.QCHATQ2;


namespace ClassLibrary2
{
    public struct QChat_Query
    {
        public string username;//头参数
        public string password;
        public string deviceID;

        public string mainParam;//查询参数
        public int pageSize;
        public int startPos;
        public string urlIp;
    };
    public struct QChat_Group
    {
        public string username;//头参数
        public string password;
        public string deviceID;
        public string urlIp;

        public string adminAccount;//增加群组参数
        public int type;//增加群组参数
        public string name;//增加、修改群组参数
        public string title;//增加、修改群组参数
        public int memberCount;//增加、修改群组参数
        public string code;//删除、修改群组参数
    };

    public struct QChat_GroupMember
    {
        public string username;//头参数
        public string password;
        public string deviceID;
        public string urlIp;

        public string groupCode;//增加、修改、删除群组成员参数
        public int memberSize;
        public string number_mem;
        public string name_mem;
        public int type_mem;
        public int lebel;
    };
    public struct QChat_User
    {
        public string username;//头参数
        public string password;
        public string deviceID;
        public string urlIp;

        public string callRestriction;//修改用户参数
        public string level;//修改用户参数
        public string number_user;//修改用户参数              //遥毙/复活
        public string password_user;//修改用户参数

        public string status;//遥毙/复活
    };
    public class telecomService
    {
        
        //9.1.1	查询集团
        public string QueryCompany(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询参数
            QueryRequest queryRequest = new QueryRequest();
            queryRequest.mainParam = qchat.mainParam;//"xghcnc933731"; //为管理员名称
            queryRequest.pageSize = qchat.pageSize;//10; //查询条数，默认为10
            queryRequest.startPos = qchat.startPos;//0; //起始位置
            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryRequest;

            QedResponse response = qqs.queryCompany(qedr);
            CompanyResponse companyResponse = response.companyResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.companyInfo != null)
            {
                sb.AppendLine("adminCount:" + companyResponse.companyInfo.adminCount.ToString());
                sb.AppendLine("companyName:" + companyResponse.companyInfo.companyName.ToString());
                sb.AppendLine("groupCount:" + companyResponse.companyInfo.groupCount.ToString());
                sb.AppendLine("userCount:" + companyResponse.companyInfo.userCount.ToString());
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);
            //Console.ReadKey();

            return sb.ToString();
        }
        //9.1.2	查询集团预定义组
        public string QueryCompanyGroup(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询参数
            QueryRequest queryRequest = new QueryRequest();
            queryRequest.mainParam = qchat.mainParam;//"xghcnc933731"; //为管理员名称
            queryRequest.pageSize = qchat.pageSize;//10; //查询条数，默认为10
            queryRequest.startPos = qchat.startPos;//0; //起始位置
            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryRequest;

            QedResponse response = qqs.queryCompanyGroup(qedr);
            CompanyGroupResponse companyResponse = response.companyGroupResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.groupInfo != null)
            {
                sb.AppendLine("count:" + companyResponse.count.ToString());
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode + "\n");

                sb.AppendLine("groupInfo: " + companyResponse.groupInfo.ToString());

                for (int i = 0; i < companyResponse.groupInfo.Length; i++)
                {
                    sb.AppendLine("结果" + i);
                    sb.AppendLine("code:" + companyResponse.groupInfo[i].code);
                    sb.AppendLine("domain:" + companyResponse.groupInfo[i].domain);
                    sb.AppendLine("memberCount:" + companyResponse.groupInfo[i].memberCount.ToString());
                    sb.AppendLine("name:" + companyResponse.groupInfo[i].name);
                    sb.AppendLine("policy:" + companyResponse.groupInfo[i].policy);
                    sb.AppendLine("title: " + companyResponse.groupInfo[i].title);
                    sb.AppendLine("type:" + companyResponse.groupInfo[i].type.ToString());
                }
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.1.3	查询集团用户
        public string QueryCompanyUser(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询参数
            QueryRequest queryRequest = new QueryRequest();
            queryRequest.mainParam = qchat.mainParam;//"xghcnc933731"; //为管理员名称
            queryRequest.pageSize = qchat.pageSize;//10; //查询条数，默认为10
            queryRequest.startPos = qchat.startPos;//0; //起始位置
            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryRequest;

            QedResponse response = qqs.queryCompanyUser(qedr);
            CompanyUserResponse companyResponse = response.companyUserResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.userInfo != null)
            {
                sb.AppendLine("count:" + companyResponse.count.ToString());
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);

                sb.AppendLine("groupInfo:" + companyResponse.userInfo.ToString());
                for (int i = 0; i < companyResponse.userInfo.Length; i++)
                {
                    sb.AppendLine("用户" + i);
                    sb.AppendLine("domain:" + companyResponse.userInfo[i].domain);
                    sb.AppendLine("callRestriction:" + companyResponse.userInfo[i].callRestriction);
                    sb.AppendLine("imsi:" + companyResponse.userInfo[i].imsi);
                    sb.AppendLine("level:" + companyResponse.userInfo[i].level);
                    sb.AppendLine("name:" + companyResponse.userInfo[i].name);
                    sb.AppendLine("number:" + companyResponse.userInfo[i].number);
                    sb.AppendLine("password:" + companyResponse.userInfo[i].password);
                    sb.AppendLine("status:" + companyResponse.userInfo[i].status);

                }
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.1	查询群组
        public string QueryGroup(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询参数
            QueryRequest queryRequest = new QueryRequest();
            queryRequest.mainParam = qchat.mainParam;//"xghcnc933731"; //为管理员名称
            queryRequest.pageSize = qchat.pageSize;//10; //查询条数，默认为10
            queryRequest.startPos = qchat.startPos;// 0; //起始位置
            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryRequest;

            QedResponse response = qqs.queryGroup(qedr);
            GroupResponse companyResponse = response.groupResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.groupInfo != null)
            {
                sb.AppendLine("code:" + companyResponse.groupInfo.code);
                sb.AppendLine("domain:" + companyResponse.groupInfo.domain);
                sb.AppendLine("memberCount:" + companyResponse.groupInfo.memberCount.ToString());
                sb.AppendLine("name:" + companyResponse.groupInfo.name);
                sb.AppendLine("policy:" + companyResponse.groupInfo.policy);
                sb.AppendLine("title:" + companyResponse.groupInfo.title);
                sb.AppendLine("type:" + companyResponse.groupInfo.type.ToString());
                sb.AppendLine("description:" + companyResponse.response.description);
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.2	增加群组
        public string AddGroup(QChat_Group qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //增加参数
            AddGroupRequest queryRequest_add = new AddGroupRequest();
            GroupInfo gi = new GroupInfo();
            gi.name = qchat.name;//"test";
            gi.title = qchat.title;//"title";
            gi.memberCount = qchat.memberCount;//20;
            gi.type = qchat.type;//2;
            gi.policy = "2";//固定
            gi.domain = (gi.type == 2 ? "2" : "3"); ;
            gi.codec = 2;//固定
            gi.code = string.Empty;//固定

            queryRequest_add.adminAccount = qchat.adminAccount;//"admin057100622"; //集团管理员名称
            queryRequest_add.groupInfo = gi;

            QedRequest qedr_add = new QedRequest();
            qedr_add.addGroupRequest = queryRequest_add;

            QedResponse response = qqs.addGroup(qedr_add);
            GroupResponse companyResponse = response.groupResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.response != null)
            {
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.3	删除群组
        public string deleteGroup(QChat_Group qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //6删除群组
            GroupRequest deleteRequest = new GroupRequest();
            GroupInfo gi2 = new GroupInfo();
            gi2.code = qchat.code;//"gmikym193313";
            deleteRequest.groupInfo = gi2;
            QedRequest qedr_del = new QedRequest();
            qedr_del.groupRequest = deleteRequest;
            QedResponse response = qqs.deleteGroup(qedr_del);
            GroupResponse companyResponse = response.groupResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null)//&& companyResponse.response != null)
            {
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.4	修改群组
        public string updateGroup(QChat_Group qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //7修改群组
            GroupRequest updateRequest = new GroupRequest();
            GroupInfo gi3 = new GroupInfo();
            gi3.name = qchat.name;//"123";
            gi3.memberCount = qchat.memberCount;//101;
            gi3.code = qchat.code;// "zppaam678685";
            gi3.title = qchat.title;// "title";
            updateRequest.groupInfo = gi3;
            QedRequest qedr_update = new QedRequest();
            qedr_update.groupRequest = updateRequest;
            QedResponse response = qqs.updateGroup(qedr_update);
            GroupResponse companyResponse = response.groupResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.response != null)
            {
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.5	查询群组成员
        public string queryGroupMember(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询群组成员
            QueryRequest queryMemRequest = new QueryRequest();
            queryMemRequest.mainParam = qchat.mainParam;// "xghcnc933731"; //为管理员名称
            queryMemRequest.pageSize = qchat.pageSize;// 10; //查询条数，默认为10
            queryMemRequest.startPos = qchat.startPos;// 0; //起始位置
            QedRequest qedr_query = new QedRequest();
            qedr_query.queryRequest = queryMemRequest;

            QedResponse response = qqs.queryGroupMember(qedr_query);
            QueryGroupMemberResponse companyResponse = response.QGMemResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.response != null)
            {
                sb.AppendLine("count:" + companyResponse.count);
                sb.AppendLine("Response:" + companyResponse.response.description.ToString());
                sb.AppendLine("reserve:" + companyResponse.response.reserve);
                sb.AppendLine("returnCode:" + companyResponse.response.returnCode);
                //kzs?为什么为空
                if (companyResponse.gMemInfo != null)
                {
                    for (int i = 0; i < companyResponse.gMemInfo.Length; i++)
                    {
                        sb.AppendLine("成员" + (i + 1));
                        sb.AppendLine("number:" + companyResponse.gMemInfo[i].number);
                        sb.AppendLine("name:" + companyResponse.gMemInfo[i].name);
                        sb.AppendLine("level:" + companyResponse.gMemInfo[i].level);
                        sb.AppendLine("type:" + companyResponse.gMemInfo[i].type);

                    }
                }
                
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.6	增加群组成员
        public string AddGroupMember(QChat_GroupMember qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //增加群组成员
            GroupMemberRequest gmr = new GroupMemberRequest();
            gmr.memberSize = qchat.memberSize;// 2;
            gmr.groupCode = qchat.groupCode;// "zppaam678685";
            GroupMemberInfo[] groupMemberCollection = new GroupMemberInfo[1];
            GroupMemberInfo groupmember1 = new GroupMemberInfo();

            groupmember1.number = qchat.number_mem;// "15314604085";
            groupmember1.type = qchat.type_mem;// 1;
            groupmember1.name = qchat.name_mem;// "testname";
            groupmember1.level = qchat.lebel;// 4;

            groupMemberCollection[0] = groupmember1;
            gmr.gMemInfo = groupMemberCollection;
            QedRequest qedr_addmem = new QedRequest();
            qedr_addmem.groupMemRequest = gmr;

            QedResponse response = qqs.addGroupMember(qedr_addmem);
            GroupMemberResponse companyResponse = response.groupMemResponse;
            StringBuilder sb = new StringBuilder();

            if (companyResponse != null && companyResponse.response != null)
            {
                sb.AppendLine("count:" + companyResponse.count);
                for (int i = 0; i < companyResponse.count; i++)
                {
                    sb.AppendLine("description:" + companyResponse.response[i].description);
                    sb.AppendLine("reserve:" + companyResponse.response[i].reserve);
                    sb.AppendLine("returnCode:" + companyResponse.response[i].returnCode);
                }
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.7	修改群组成员
        public string UpdateGroupMember(QChat_GroupMember qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //修改群组成员
            GroupMemberRequest gmr = new GroupMemberRequest();
            gmr.memberSize = qchat.memberSize;// 2;
            gmr.groupCode = qchat.groupCode;// "zppaam678685";
            GroupMemberInfo[] groupMemberCollection = new GroupMemberInfo[1];
            GroupMemberInfo groupmember1 = new GroupMemberInfo();

            groupmember1.number = qchat.number_mem;// "15314604085";
            groupmember1.type = qchat.type_mem;// 1;
            groupmember1.name = qchat.name_mem;// "testname";
            groupmember1.level = qchat.lebel;// 4;

            groupMemberCollection[0] = groupmember1;
            gmr.gMemInfo = groupMemberCollection;
            QedRequest qedr = new QedRequest();
            qedr.groupMemRequest = gmr;

            StringBuilder sb = new StringBuilder();
            QedResponse response = qqs.updateGroupMember(qedr);

            GroupMemberResponse gresponse = response.groupMemResponse;
            if (gresponse != null && gresponse.response != null)
            {
                sb.AppendLine("Count: " + gresponse.count);
                for (int i = 0; i < gresponse.count; i++)
                {
                    sb.AppendLine("description:" + gresponse.response[i].description);
                    sb.AppendLine("reserve:" + gresponse.response[i].reserve);
                    sb.AppendLine("returnCode:" + gresponse.response[i].returnCode);
                }
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.2.8	删除群组成员
        public string DeleteGroupMember(QChat_GroupMember qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //删除群组成员
            GroupMemberRequest gmr = new GroupMemberRequest();
            gmr.memberSize = qchat.memberSize;// 2;
            gmr.groupCode = qchat.groupCode;// "zppaam678685";
            GroupMemberInfo[] groupMemberCollection = new GroupMemberInfo[1];
            GroupMemberInfo groupmember1 = new GroupMemberInfo();

            groupmember1.number = qchat.number_mem;// "15314604085";
            groupmember1.type = qchat.type_mem;// 1;
            groupmember1.name = qchat.name_mem;// "testname";
            groupmember1.level = qchat.lebel;// 4;

            groupMemberCollection[0] = groupmember1;
            gmr.gMemInfo = groupMemberCollection;
            QedRequest qedr = new QedRequest();
            qedr.groupMemRequest = gmr;

            StringBuilder sb = new StringBuilder();
            QedResponse response = qqs.deleteGroupMember(qedr);

            GroupMemberResponse gresponse = response.groupMemResponse;
            if (gresponse != null && gresponse.response != null)
            {
                sb.AppendLine("Count: " + gresponse.count);
                for (int i = 0; i < gresponse.count; i++)
                {
                    sb.AppendLine("description:" + gresponse.response[i].description);
                    sb.AppendLine("reserve:" + gresponse.response[i].reserve);
                    sb.AppendLine("returnCode: " + gresponse.response[i].returnCode);
                }
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.3.1	查询用户
        public string QueryUser(QChat_Query qchat){
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            //查询用户
            QueryRequest queryrequest = new QueryRequest();
            queryrequest.mainParam = qchat.mainParam;//"15314604129";
            queryrequest.startPos = qchat.startPos;//0;
            queryrequest.pageSize = qchat.pageSize;//10;

            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryrequest;

            UserResponse userresponse = qqs.queryUser(qedr).userResponse;

            StringBuilder sb = new StringBuilder();
            if (userresponse != null && userresponse.userInfo != null)
            {
                sb.AppendLine("callRestriction:" + userresponse.userInfo.callRestriction);
                sb.AppendLine("domain:" + userresponse.userInfo.domain);
                sb.AppendLine("imsi:" + userresponse.userInfo.imsi);
                sb.AppendLine("level:" + userresponse.userInfo.level);
                sb.AppendLine("name:" + userresponse.userInfo.name);
                sb.AppendLine("number:" + userresponse.userInfo.number);
                sb.AppendLine("password:" + userresponse.userInfo.password);
                sb.AppendLine("status:" + userresponse.userInfo.status);

                sb.AppendLine("description:" + userresponse.response.description);
                sb.AppendLine("reserve:" + userresponse.response.reserve);
                sb.AppendLine("returnCode:" + userresponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.3.2	查询用户归属群组信息
        public string QueryUserGroup(QChat_Query qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            
            //查询用户归属群组信息
            QueryRequest queryrequest = new QueryRequest();
            queryrequest.mainParam = qchat.mainParam;//"15314604129";
            queryrequest.startPos = qchat.startPos;//0;
            queryrequest.pageSize = qchat.pageSize;//10;

            QedRequest qedr = new QedRequest();
            qedr.queryRequest = queryrequest;

            UserGroupResponse userGroupresponse = qqs.queryUserGroup(qedr).UGroupResponse;
            StringBuilder sb = new StringBuilder();

            if (userGroupresponse != null && userGroupresponse.userGroupInfo != null)
            {
                sb.AppendLine("Count:" + userGroupresponse.count);
                for (int i = 0; i < userGroupresponse.count; i++)
                {
                    sb.AppendLine("companyID:" + userGroupresponse.userGroupInfo[i].companyID);
                    sb.AppendLine("groupCode:" + userGroupresponse.userGroupInfo[i].groupCode);
                    sb.AppendLine("groupName:" + userGroupresponse.userGroupInfo[i].groupName);
                    sb.AppendLine("groupType:" + userGroupresponse.userGroupInfo[i].groupType.ToString());
                    sb.AppendLine("level:" + userGroupresponse.userGroupInfo[i].level.ToString());
                    sb.AppendLine("memberAliasName:" + userGroupresponse.userGroupInfo[i].memberAliasName);
                    sb.AppendLine("memberType:" + userGroupresponse.userGroupInfo[i].memberType.ToString());
                    sb.AppendLine("policy:" + userGroupresponse.userGroupInfo[i].policy.ToString());
                    sb.AppendLine("title:" + userGroupresponse.userGroupInfo[i].title);
                }
                sb.AppendLine("description:" + userGroupresponse.response.description);
                sb.AppendLine("reserve:" + userGroupresponse.response.reserve);
                sb.AppendLine("returnCode:" + userGroupresponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.3.3	修改用户
        public string updateUser(QChat_User qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);

            
            //修改用户
            UserRequest userequest = new UserRequest();
            UserInfo ui = new UserInfo();
            ui.callRestriction = qchat.callRestriction;//"1111111111";
            ui.level = qchat.level;//"3";
            ui.number = qchat.number_user; //"15314604129";
            ui.password = qchat.password_user;//"abcdef1111";
            userequest.userInfo = ui;

            QedRequest qedr = new QedRequest();
            qedr.userRequest = userequest;

            QedResponse response = qqs.updateUser(qedr);
            UserResponse userresponse = response.userResponse;

            StringBuilder sb = new StringBuilder();

            if (userresponse.response != null)
            {
                sb.AppendLine("description" + userresponse.response.description);
                sb.AppendLine("reserve:" + userresponse.response.reserve);
                sb.AppendLine("returnCode:" + userresponse.response.returnCode);
            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);

            //Console.ReadKey();
            return sb.ToString();
        }
            //9.4	遥毙/复活
        public string updateUserStatus(QChat_User qchat)
        {
            QchatQEDService qqs = new QchatQEDService();
            qqs = getQchatQEDService(qchat.username, qchat.password, qchat.deviceID, qchat.urlIp);
            
            //遥毙/复活
            UserInfo ui = new UserInfo();
            ui.number = qchat.number_user;// "15314604129";
            ui.status = qchat.status;// "0";
            UserRequest userequest = new UserRequest();
            userequest.userInfo = ui;

            QedRequest qedr = new QedRequest();
            qedr.userRequest = userequest;

            QedResponse response = qqs.updateUserStatus(qedr);
            UserResponse userresponse = response.userResponse;

            StringBuilder sb = new StringBuilder();
            if (userresponse != null && userresponse.response != null)
            {
                sb.AppendLine("description:" + userresponse.response.description);
                sb.AppendLine("reserve:" + userresponse.response.reserve);
                sb.AppendLine("returnCode:" + userresponse.response.returnCode);

            }
            else
            {
                sb.AppendLine("返回信息为null");
            }
            Console.WriteLine(sb);
            //Console.ReadKey();
            return sb.ToString();
        }
        public QchatQEDService getQchatQEDService(string username,string password,string deviceID, string urlIp) {
            QchatQEDService qqs = new QchatQEDService();

            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate; //验证服务器证书回调自动验证    
            @string strUn = new @string();
            strUn.Text = new string[] { username }; //用户名
            string1 str = new string1();
            string _pword = E_MD5(password); //md5加密
            str.Text = new string[] { _pword };
            qqs.Username = strUn;
            qqs.Password = str;

            string2 s2 = new string2();
            s2.Text = new string[] { deviceID }; //关于 deviceID 如果有问题请咨询电信相关人员
            qqs.DeviceID = s2;

            //qqs.Url = "https://114.81.253.12:8443/CPS_QED/services/QchatQEDService";
            qqs.Url = "https://" + urlIp + ":8443/CPS_QED/services/QchatQEDService";
            string3 s3 = new string3();
            string s = Environment.TickCount.ToString();
            s3.Text = new string[] { DateTime.Now.ToString("yyyyMMddHHmmss") + s.Substring(s.Length - 4, 4) };
            qqs.Sequence = s3;

            return qqs;
        }
        static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            // System.Console.WriteLine("Warning, trust any certificate");
            //为了通过证书验证，总是返回true
            return true;
        }

        static string E_MD5(string input)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] res = Encoding.Default.GetBytes(input);
                byte[] tempData = md5Hasher.ComputeHash(res);
                byte[] data = md5Hasher.ComputeHash(tempData);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    int v = data[i] & 0xFF;
                    sBuilder.Append(v.ToString("x2").ToUpper() + "");
                }
                return sBuilder.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
