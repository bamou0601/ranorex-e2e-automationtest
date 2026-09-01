/*
 * Created by Ranorex
 * User: mo_ba
 * Date: 2026/09/01
 * Time: 15:24
 * 
 * To change this template use Tools > Options > Coding > Edit standard headers.
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
    /// Description of NavigationHelper.
    /// </summary>
    [TestModule("6B44CC14-812A-46B5-B55F-A837C77F36AB", ModuleType.UserCode, 1)]
    public class NavigationHelper : ITestModule
    {
        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public NavigationHelper()
        {
            // Do not delete - a parameterless constructor is required!
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
        }
    }
}
