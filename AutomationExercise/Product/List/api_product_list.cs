/*
 * Created by Ranorex
 * User: mo_ba
 * Date: 2026/08/28
 * Time: 10:59
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

namespace AutomationExercise.Product.List
{
    /// <summary>
    /// Description of apiProductlist.
    /// </summary>
    [TestModule(
    	"41C273FF-F9AC-4E16-B3A5-6B10A0CFC6C0", 
    	ModuleType.UserCode, 
    	1
    )]
    public class api_product_list : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public api_product_list()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
              
        public void GetProductListByApi()
        {
        	// APIクライアントを生成
        	using (HttpClient client = new HttpClient())
        	{
        		// 商品一覧APIを実行
        		HttpResponseMessage response =
            		client.GetAsync(
                		"https://automationexercise.com/api/productsList"
            		).Result;

        		// レスポンス本文を取得
        		string responseBody =
            		response.Content.ReadAsStringAsync().Result;
        		
        		//HTTPステータスを取得
        		int statusCode = 
        			(int)response.StatusCode;
        		
        		Report.Info(
        			"HTTP Status: " + statusCode
        		);
        		
        		// HTTP 200を検証
        		Validate.AreEqual(
        			statusCode,
        			200
        		);
        		
        		// JSONをオブジェクトへ変換
        		JavaScriptSerializer serializer = 
        			new JavaScriptSerializer();
        		
        		ProductSearchResponse result =
        			serializer.Deserialize<ProductSearchResponse>(
        				responseBody
        			);
        		
        		// API内部のresponseCodeを検証
        		Validate.AreEqual(
        			result.responseCode,
        			200
        		);
        		
        		// 商品一覧が存在することを検証
        		Validate.IsTrue(
        			result.products != null &&
        			result.products.Count > 0
        		);
        		
        		
        		// 商品総数を共有データへ保存
        		TestContext.ApiProductCount = 
        			result.products.Count;
        		
        		// API商品総数をログ出力
        		Report.Info(
            		"API商品総数: " +
            		TestContext.ApiProductCount
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
            
            GetProductListByApi();
        }
        
        /// <summary>
		/// 商品検索APIレスポンス。
		/// </summary>
		public class ProductSearchResponse
		{
    		public int responseCode { get; set; }

    		public List<Product> products { get; set; }
		}

		/// <summary>
		/// 商品情報。
		/// </summary>
		public class Product
		{
    		public int id { get; set; }

    		public string name { get; set; }

    		public string price { get; set; }

   			public string brand { get; set; }
		}
    }
}
