/*
 * Created by Ranorex
 * User: mo_ba
 * 機能: 商品検索API共通処理
 * ロジック: 検索キーワードを受け取り、
 *           商品検索APIを実行して結果を返却する
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

namespace AutomationExercise.Product.Search
{
    /// <summary>
    /// 商品検索APIを実行するテストモジュール。
    /// </summary>
    [TestModule(
    	"B958290D-4F68-4FF9-AFFD-03D37B45DFA3", 
    	ModuleType.UserCode, 
    	1
    )]
    public class api_searching_product : ITestModule
    {	
    	// Data Sourceから受け取る検索キーワード
    	private string searchProduct = "";
    	
    	/// <summary>
        /// 商品検索キーワード。
        /// Test CaseのData Sourceから値を受け取る。
        /// </summary>
        [TestVariable(
        	"5e9b2cb9-e1cb-4d44-b562-8381c8b426f9"
        )]
        public string SearchProduct
        {
        	get { return searchProduct; }
        	
        	set { searchProduct = value; }
        }
    	
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public api_searching_product()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        /// <summary>
        /// 商品検索APIを実行する。
        ///
        /// 処理内容:
        /// ・Data Sourceから検索キーワードを取得する。
        /// ・商品検索APIへPOSTする。
        /// ・HTTPステータスを検証する。
        /// ・API内部のresponseCodeを検証する。
        /// ・商品検索結果を取得する。
        /// ・検索結果をTestContextへ保存する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        public void SearchProductByApi()
        {
            // 検索キーワードをログ出力
            Report.Info(
                "検索キーワード: " +
                searchProduct
            );

			// 検索キーワードが設定されていることを確認
            Validate.IsTrue(
                !string.IsNullOrEmpty(searchProduct)
            );            
        	
			// POSTリクエストパラメータを作成
			Dictionary<string, string> parameters =
                    new Dictionary<string, string>()
                    {
                        { "search_product", searchProduct }
                    };
	
			
        	// APIクライアントを生成
            using (HttpClient client = new HttpClient())
             
            using (FormUrlEncodedContent content =
            	new FormUrlEncodedContent(parameters))
           	{
            	// 商品検索APIを実行
                HttpResponseMessage response =
                    client.PostAsync(
                        "https://automationexercise.com/api/searchProduct",
                        content
                    ).Result;

             	// レスポンス本文を取得
                string responseBody =
                    response.Content
                        .ReadAsStringAsync()
                        .Result;

               	// ステータスコードをレポート出力
                Report.Info(
                    "HTTP Status: " +
                    (int)response.StatusCode
                );
               	
                // APIレスポンスをログ出力
                Report.Info(
                    "API Response: " +
                    responseBody
                );

                // HTTP 200を検証
                Validate.AreEqual(
                    (int)response.StatusCode,
                    200
                );
                    
                // JSONをオブジェクトへ変換
                JavaScriptSerializer serializer =
                    new JavaScriptSerializer();

                    
                ProductSearchResponse result =
                    serializer.Deserialize
                    <ProductSearchResponse>(
                        responseBody
                    );
                
                
               	// API内部のresponseCodeを検証
            	Validate.AreEqual(
                	result.responseCode,
                	200
            	);   

            	// 商品が1件以上存在することを検証
            	Validate.IsTrue(
                	result.products != null &&
                	result.products.Count > 0
            	);

            	// API検索結果をTestContextへ保存
            	// 検索キーワードを共有
            	TestContext.SearchProduct = searchProduct;
            	// 最初の商品名を共有
            	TestContext.ApiProductName = result.products[0].name;
            	// API商品数を共有
            	TestContext.ApiProductCount = result.products.Count;
            		
            	// 全商品名を保存するListを初期化
            	TestContext.ApiProductNames = new List<string>();
            		
            	// APIから取得した全商品名を保存
            	foreach (Product product in result.products)
            	{
            		TestContext.ApiProductNames.Add(
            			product.name
            		);
            	}

            	Report.Info(
                	"API取得商品数: " + TestContext.ApiProductCount
            	);
            		
            	// APIの商品名をすべてログへ出力
            	for (int i = 0;
            		    i < TestContext.ApiProductNames.Count;
            		    i++)
            	{
            		Report.Info(
            			"API商品[" +
            			(i + 1) +
            			"]: " +
            			TestContext.ApiProductNames[i]
            		);
            	}
                    
         	}
        }
            

        /// <summary>
        /// Ranorex Test Suiteから実行される処理。
        ///
        /// 処理内容:
        /// ・現在のIterationに設定された検索キーワードを使用する。
        /// ・商品検索APIを実行する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        void ITestModule.Run()
        {
            Mouse.DefaultMoveTime = 300;
            Keyboard.DefaultKeyPressTime = 100;
            Delay.SpeedFactor = 1.0;
            
            // 商品検索APIを実行
            SearchProductByApi();
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
