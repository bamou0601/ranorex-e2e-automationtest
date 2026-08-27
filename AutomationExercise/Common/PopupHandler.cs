/*
 * Created by Ranorex
 * User: mo_ba
 * Date: 2026/08/27
 * Time: 14:30
 * 
 * 機能: ポップアップ広告の共通処理
 * ロジック: 広告のClose要素を検索し、存在する場合のみ閉じる
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using WinForms = System.Windows.Forms;

using Ranorex;
using Ranorex.Core;
using Ranorex.Core.Testing;

namespace AutomationExercise.Common
{
    /// <summary>
    /// ポップアップ広告を処理する共通モジュール。
    /// </summary>
    [TestModule("C6D29153-CD32-4035-AED8-B405E1526A99", ModuleType.UserCode, 1)]
    public class PopupHandler : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public PopupHandler()
        {
            // Do not delete - a parameterless constructor is required!
        }
        
        /// <summary>
        /// ポップアップ広告が表示された場合のみ閉じる。
        ///
        /// 処理内容:
        /// ・広告iframe内のClose要素を検索する。
        /// ・存在する場合のみクリックする。
        /// ・存在しない場合はそのまま処理を継続する。
        ///
        /// Returns:
        /// なし。
        /// </summary>
        private void ClosePopupAdIfExists()
        {
        	// iframe番号に依存しないRxPath
            string closePath =
                "/dom[@domain='www.automationexercise.com']" +
                "//iframe//div[#'dismiss-button-element']" +
                "/div[@innertext='Close']";

            // Close要素を検索
            IList<Ranorex.Unknown> closeButtons =
                Host.Local.Find<Ranorex.Unknown>(
                    closePath
                );

            // 広告が存在しない場合
            if (closeButtons.Count == 0)
            {
                Report.Info(
                    "ポップアップ広告は表示されていません。"
                );

                return;
            }

            // 広告を閉じる
            closeButtons[0].Click();

            Report.Info(
                "ポップアップ広告を閉じました。"
            );
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
            
            // ポップアップ広告を処理
            ClosePopupAdIfExists();
        }
    }
}
