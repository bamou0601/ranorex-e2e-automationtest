/*
 * 機能: ブランド一覧APIテスト
 * ロジック: brandsList APIを実行し、
 *           ブランド件数とブランド名をTestContextへ保存する
 * User: mo_ba
 * Date: 2026/08/28
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

namespace AutomationExercise.Brand
{
    /// <summary>
    /// Description of api_brand_list.
    /// </summary>
    [TestModule(
    	"0668DCA6-0F77-457F-A083-13C9C3898048", 
    	ModuleType.UserCode, 
    	1
    )]
    public class api_brand_list : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public api_brand_list()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        /// <summary>
        /// ブランド一覧APIを実行する。
        ///
        /// 処理内容:
        /// ・brandsList APIへGETリクエストを送信する。
        /// ・HTTPステータスを検証する。
        /// ・API内部のresponseCodeを検証する。
        /// ・ブランド一覧を取得する。
        /// ・ブランド件数とブランド名をTestContextへ保存する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        private void GetBrandListByApi()
        {
        	// APIクライアントを生成
        	using (HttpClient client = new HttpClient())
        	{
        		// ブランド一覧APIを実行
        		HttpResponseMessage response =
        			client.GetAsync(
        				"https://automationexercise.com/api/brandsList"
        			).Result;
        		
        		// レスポンス本文を取得
        		string responseBody =
        			response.Content
        				.ReadAsStringAsync()
        				.Result;
        		int statusCode = (int)response.StatusCode;
        		
        		// HTTPステータスをログ出力
        		Report.Info(
        			"HTTP Status: " +
        			statusCode
        		);
        		
        		// APIレスポンスをログ出力
        		Report.Info(
        			"Brand List Response: " +
                    responseBody
        		);
        		
        		
        		// HTTP 200を検証
        		Validate.AreEqual(
        			statusCode,
        			200
        		);
        		
        		
        		// JSONをオブジェクトへ変換
        		JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                BrandListResponse result =
                    serializer.Deserialize<BrandListResponse>(
                        responseBody
                    );
                
                // レスポンスが存在することを検証
                Validate.IsTrue(
                	result != null
                );
        		
                
                // API内部のresponseCodeを検証
                Validate.AreEqual(
                	result.responseCode,
                	200
                );
                
                
                // ブランドが1件以上存在することを検証
                Validate.IsTrue(
                	result.brands != null &&
                	result.brands.Count > 0
                );
                
                // ブランド件数を共有
                TestContext.ApiBrandCount =
                	result.brands.Count;
                
                // ブランド名リストを初期化
                TestContext.ApiBrandNames =
                	new List<string>();
                
                // APIブランド名を保存
                foreach (Brand brand in result.brands)
                {
                	TestContext.ApiBrandNames.Add(
                		brand.brand
                	);
                }
                
                // ブランド件数をログ出力
           		Report.Info(
                    "APIブランド総数: " +
                    TestContext.ApiBrandCount
                );
				
                // ブランド名をログ出力
                for (int i = 0;
                     i < TestContext.ApiBrandNames.Count;
                     i++)
                {
                    Report.Info(
                        "APIブランド[" +
                        (i + 1) +
                        "]: " +
                        TestContext.ApiBrandNames[i]
                    );
                }
	
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
            
            // ブランド一覧APIを実行
            GetBrandListByApi();
        }
        
        /// ブランド一覧APIレスポンス。
        public class BrandListResponse
		{
    		public int responseCode { get; set; }

    		public List<Brand> brands { get; set; }
		}
		
         /// ブランド情報。
		public class Brand
		{
    		public int id { get; set; }

    		public string brand { get; set; }
		}
    }
}
