﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using SQADemicApp;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace SQADemicAppTest
{
    /// <summary>
    /// Summary description for UITest
    /// </summary>
    [TestClass]
    public class UITest
    {
        
        public UITest()
        {
        }

        private GameBoardModels _models;
        readonly string[] _players = { "Dispatcher", "Scientist", "Medic", "Operations Expert" };

        [TestInitialize]
        public void SetUp()
        {
            _models = new GameBoardModels(_players);
        }

        //[TestMethod]
        //public void CodedUITestMethod1()
        //{
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        #region CharacterPane

        public void AssertHidePlayerButtonIsCorrect(int playerCount)
        {
            var pane = new CharacterPane();
            pane.HidePlayersByCount(playerCount);
            for (var i = playerCount; i < 4; i++)
            {
                Assert.AreEqual(false, pane.GetPlayerBtns()[i].Visible);
            }
        }

        [TestMethod]
        public void TestOnePlayer()
        {
            AssertHidePlayerButtonIsCorrect(1);
        }

        [TestMethod]
        public void TestTwoPlayers()
        {
            AssertHidePlayerButtonIsCorrect(2);
        }

        [TestMethod]
        public void TestThreePlayers()
        {
            AssertHidePlayerButtonIsCorrect(3);
        }
        #endregion

        #region GameBoard

        public void AssertUpdatePlayerButtonByTextIsCorrect(int index)
        {
            var pane = new CharacterPane();
            var cityName = "Chicago";
            GameBoard.UpdatePlayerButtonTextAccordingToIndex(pane, index, cityName);
            Assert.AreEqual("Player " + (index + 1) + "\n" + GameBoardModels.GetPlayerByIndex(index) + "\n" + cityName,
               pane.GetPlayerBtns()[index].Text);
        }

        [TestMethod]
        public void TestUpdatePlayerButtonTextOfIndexZero()
        {
            AssertUpdatePlayerButtonByTextIsCorrect(0);
        }

        [TestMethod]
        public void TestUpdatePlayerButtonTextOfIndexOne()
        {
            AssertUpdatePlayerButtonByTextIsCorrect(1);
        }

        [TestMethod]
        public void TestUpdatePlayerButtonTextOfIndexTwo()
        {
            AssertUpdatePlayerButtonByTextIsCorrect(2);
        }

        [TestMethod]
        public void TestUpdatePlayerButtonTextOfIndexThree()
        {
            AssertUpdatePlayerButtonByTextIsCorrect(3);
        }
        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
    }
}
