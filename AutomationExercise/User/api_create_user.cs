/*
 * Created by Ranorex
 * 機能: APIによるテストユーザー作成
 * ロジック: 一意のメールアドレスを生成し、
 *           createAccount APIでユーザーを作成する
 * User: mo_ba
 * Date: 2026/08/27
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace AutomationExercise.User
{
    /// <summary>
    /// APIでテストユーザーを作成するモジュール。
    /// </summary>
    [TestModule(
    	"86B70D3C-D1BD-4F6A-9B69-E31636E72A68", 
    	ModuleType.UserCode, 
    	1
    )]
    public class api_create_user : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public api_create_user()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        /// <summary>
        /// ユーザー作成APIを実行する。
        ///
        /// 処理内容:
        /// ・実行ごとに一意のメールアドレスを生成する。
        /// ・createAccount APIへPOSTする。
        /// ・HTTPステータスとAPIレスポンスを検証する。
        /// ・作成したユーザー情報をTestContextへ保存する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        private void CreateUserByApi()
        {
        	// テストユーザー情報を生成
        	string userName = "TestUser";
        	
        	string userEmail = 
        		"test_" +
        		System.DateTime.Now.ToString("yyyyMMddHHmmssfff") +
        		"@example.com";
        	
        	string userPassword = "test1234";
        		
        	// APIリクエストパラメータを作成
            Dictionary<string, string> parameters =
                new Dictionary<string, string>()
                {
                    { "name", userName },
                    { "email", userEmail },
                    { "password", userPassword },
                    { "title", "Mr" },
                    { "birth_date", "1" },
                    { "birth_month", "1" },
                    { "birth_year", "1990" },
                    { "firstname", "Ranorex" },
                    { "lastname", "User" },
                    { "company", "AutomationTest" },
                    { "address1", "Tokyo Test Address 1" },
                    { "address2", "Tokyo Test Address 2" },
                    { "country", "Canada" },
                    { "zipcode", "1000001" },
                    { "state", "Tokyo" },
                    { "city", "Tokyo" },
                    { "mobile_number", "09012345678" }
                };
            
            using (HttpClient client = new HttpClient())
            using (FormUrlEncodedContent content =
                new FormUrlEncodedContent(parameters))
            {
                // ユーザー作成APIを実行
                HttpResponseMessage response =
                    client.PostAsync(
                        "https://automationexercise.com/api/createAccount",
                        content
                    ).Result;
                
                // レスポンス本文を取得
                string responseBody =
                    response.Content.ReadAsStringAsync().Result;

                Report.Info(
                    "HTTP Status: " +
                    (int)response.StatusCode
                );
                
                
                Report.Info(
                    "Create User Response: " +
                    responseBody
                );

                // HTTP通信自体が成功していることを確認
                Validate.AreEqual(
                    (int)response.StatusCode,
                    200
                );

                // JSONレスポンスを解析
                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                ApiResponse result =
                    serializer.Deserialize<ApiResponse>(
                        responseBody
                    );

                // API内部のレスポンスコードを検証
                Validate.AreEqual(
                    result.responseCode,
                    201
                );

                // APIメッセージを検証
                Validate.AreEqual(
                    result.message,
                    "User created!"
                );

                // 作成ユーザー情報を共有
                global::AutomationExercise.TestContext.UserName =
                    userName;

                global::AutomationExercise.TestContext.UserEmail =
                    userEmail;

                global::AutomationExercise.TestContext.UserPassword =
                    userPassword;

                // ログ出力
                Report.Info(
                    "作成ユーザー名: " +
                    global::AutomationExercise.TestContext.UserName
                );

                Report.Info(
                    "作成メールアドレス: " +
                    global::AutomationExercise.TestContext.UserEmail
                );
                
            }
        	
        }
        

        /// <summary>
        /// Performs the playback of actions in this module.
        /// Ranorex Test Suiteから実行される処理。
        /// </summary>
        /// <remarks>You should not call this method directly, instead pass the module
        /// instance to the <see cref="TestModuleRunner.Run(ITestModule)"/> method
        /// that will in turn invoke this method.</remarks>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            // APIでテストユーザーを作成
            CreateUserByApi();
            
        }
    }
    
    /// <summary>
    /// API共通レスポンス。
    /// </summary>
   	public class ApiResponse
   	{
   		public int responseCode { get; set; }
   		
   		public string message { get; set; }
   	}
}
