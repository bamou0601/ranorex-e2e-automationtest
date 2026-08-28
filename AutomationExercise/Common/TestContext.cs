/*
 * Created by Ranorex
 * User: mo_ba
 * Date: 2026/08/25
 * Time: 16:51
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
 */
using System;
using System.Collections.Generic;

namespace AutomationExercise
{
    /// <summary>
    /// APIテストとUIテスト間で
    /// 共有する実行データを管理する。
    /// </summary>
	public static class TestContext
	{	
		// API検索キーワード
        public static string SearchProduct { get; set; }
		
        // APIで取得した商品名
        public static string ApiProductName { get; set; }
        
		// APIで取得した商品件数
        public static int ApiProductCount { get; set; }
        
        // APIで取得した商品名一覧
        public static List<string> ApiProductNames { get; set; }
        
        
        // ユーザー関連
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static string UserPassword { get; set; }
        
        // ブランド関連
        public static int ApiBrandCount { get; set; }
        public static List<string> ApiBrandNames { get; set; }
	}
}
