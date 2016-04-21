using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SQADemicAppTest
{
    [TestClass]
    public class SetupGameFormTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            SQADemicApp.SetupGameForm form = new SQADemicApp.SetupGameForm();

            List<String> rolesList = new List<string>() { "Medic", "Medic", "Operations Expert" };

            Assert.IsTrue(form.CheckForDuplicateRoles(rolesList));
        }
    }
}
