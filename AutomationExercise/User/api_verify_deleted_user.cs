/*
 * 機能: 削除済みテストユーザーの存在確認
 * ロジック: TestContextに保存されたユーザー情報を使用して
 *           verifyLogin APIを実行し、ユーザーが存在しないことを検証する
 * User: mo_ba
 * Date: 2026/08/27
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
    /// 削除済みユーザーが存在しないことをAPIで検証するモジュール。
    /// </summary>
    [TestModule(
        "ここはRanorexが生成したGUIDを使用",
        ModuleType.UserCode,
        1
    )]
    public class api_verify_deleted_user : ITestModule
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public api_verify_deleted_user()
        {
            // Do not delete - a parameterless constructor is required!
        }

        /// <summary>
        /// 削除済みユーザーの存在確認APIを実行する。
        ///
        /// 処理内容:
        /// ・TestContextからメールアドレスとパスワードを取得する。
        /// ・verifyLogin APIへPOSTする。
        /// ・削除済みユーザーが存在しないことを検証する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        private void VerifyDeletedUserByApi()
        {
            // TestContextからユーザー情報を取得
            string userEmail =
                global::AutomationExercise.TestContext.UserEmail;

            string userPassword =
                global::AutomationExercise.TestContext.UserPassword;

            Report.Info(
                "削除確認対象メールアドレス: " + userEmail
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
                // ログイン確認APIを実行
                HttpResponseMessage response =
                    client.PostAsync(
                        "https://automationexercise.com/api/verifyLogin",
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
                    "Verify Deleted User Response: " +
                    responseBody
                );

                // HTTP通信を検証
                Validate.AreEqual(
                    (int)response.StatusCode,
                    200
                );

                // JSONレスポンスを解析
                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                VerifyUserResponse result =
                    serializer.Deserialize<VerifyUserResponse>(
                        responseBody
                    );

                // 削除済みユーザーが存在しないことを検証
                Validate.AreEqual(
                    result.responseCode,
                    404
                );

                Validate.AreEqual(
                    result.message,
                    "User not found!"
                );

                Report.Info(
                    "削除済みユーザーが存在しないことを確認しました。"
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
            
            // 削除済みユーザーの存在を確認
            VerifyDeletedUserByApi();
        }
    }
    
    /// <summary>
    /// ユーザー確認APIレスポンス。
    /// </summary>
    public class VerifyUserResponse
    {
        public int responseCode { get; set; }

        public string message { get; set; }
    }
}
