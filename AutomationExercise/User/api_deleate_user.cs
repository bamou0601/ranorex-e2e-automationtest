/*
 * Created by Ranorex
 * User: mo_ba
 * Date: 2026/08/27
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace AutomationExercise.User
{
    /// <summary>
    /// Description of api_deleate_user.
    /// </summary>
    [TestModule(
    	"5CF745F8-F3DD-4B30-870C-81A91CE85841", 
    	ModuleType.UserCode, 
    	1
    )]
    public class api_deleate_user : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public api_deleate_user()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        /// <summary>
        /// ユーザー削除APIを実行する。
        ///
        /// 処理内容:
        /// ・TestContextからユーザー情報を取得する。
        /// ・deleteAccount APIへDELETEリクエストを送信する。
        /// ・HTTPステータスとAPIレスポンスを検証する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        private void DeleteUserByApi()
        {
        	// TestContextからユーザー情報を取得
            string userEmail =
                global::AutomationExercise.TestContext.UserEmail;

            string userPassword =
                global::AutomationExercise.TestContext.UserPassword;

            // 削除対象ユーザーをログ出力
            Report.Info(
                "削除対象メールアドレス: " + userEmail
            );

            // APIリクエストパラメータを作成
            Dictionary<string, string> parameters =
                new Dictionary<string, string>()
                {
                    { "email", userEmail },
                    { "password", userPassword }
                };

            using (HttpClient client = new HttpClient())
            using (FormUrlEncodedContent content =
                new FormUrlEncodedContent(parameters))
            {
                // DELETEリクエストを作成
                HttpRequestMessage request =
                    new HttpRequestMessage(
                        HttpMethod.Delete,
                        "https://automationexercise.com/api/deleteAccount"
                    );

                request.Content = content;

                // ユーザー削除APIを実行
                HttpResponseMessage response =
                    client.SendAsync(request).Result;

                // レスポンス本文を取得
                string responseBody =
                    response.Content.ReadAsStringAsync().Result;

                // HTTPステータスをログ出力
                Report.Info(
                    "HTTP Status: " +
                    (int)response.StatusCode
                );

                Report.Info(
                    "Delete User Response: " +
                    responseBody
                );

                // HTTPステータスを検証
                Validate.AreEqual(
                    (int)response.StatusCode,
                    200
                );

                // JSONレスポンスを解析
                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                DeleteUserResponse result =
                    serializer.Deserialize<DeleteUserResponse>(
                        responseBody
                    );

                // API内部のresponseCodeを検証
                Validate.AreEqual(
                    result.responseCode,
                    200
                );

                // APIメッセージを検証
                Validate.AreEqual(
                    result.message,
                    "Account deleted!"
                );

                Report.Info(
                    "ユーザー削除成功: " + userEmail
                );
            }
        }

        /// <summary>
        /// Performs the playback of actions in this module.
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
             // APIでテストユーザーを削除
             DeleteUserByApi();
        }
    }
    
    public class DeleteUserResponse
    {	
    	public int responseCode { get; set; }
    	
    	public string message { get; set; }
    }
}
