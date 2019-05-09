using System;

namespace DSAA.Service.IService
{
    public interface IJudgeAppService
    {

        /// <summary>
        /// 尝试评测机登录
        /// </summary>
        /// <param name="serverID">评测机ID</param>
        /// <param name="secretKey">评测机密钥</param>
        /// <param name="userip">用户IP</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否登录成功</returns>
        Boolean TryJudgeServerLogin(String serverID, String secretKey, out String error);


        /// <summary>
        /// 获取用户登录状态
        /// </summary>
        /// <returns>若用户已登录则返回空，否则返回出错状态</returns>
        String GetJudgeServerLoginStatus();


        /// <summary>
        /// 获取评测列表的Json信息
        /// </summary>
        /// <param name="lanaugeSupport">评测机支持语言</param>
        /// <param name="count">评测机请求个数</param>
        /// <returns>评测列表Json信息</returns>
        Boolean TryGetPendingListJson(String lanaugeSupport, String count, out String result, out String error);


        /// <summary>
        /// 获取题目数据
        /// </summary>
        /// <param name="pid">题目ID</param>
        /// <param name="dataPath">题目数据路径</param>
        /// <param name="error">错误信息</param>
        /// <returns>获取是否成功</returns>
        Boolean TryGetProblemDataPath(String pid, out String dataPath, out String error);

        /// <summary>
        /// 更新评测状态
        /// </summary>
        /// <param name="sid">提交ID</param>
        /// <param name="pid">题目ID</param>
        /// <param name="username">用户名</param>
        /// <param name="result">评测结果</param>
        /// <param name="detail">出错信息</param>
        /// <param name="tcost">花费时间</param>
        /// <param name="mcost">花费内存</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否更新成功</returns>
        Boolean TryUpdateSolutionStatus(String sid, String pid, String username, String result, String detail, String tcost, String mcost, out String error);

    }
}
