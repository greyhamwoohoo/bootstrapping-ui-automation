﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TheInternet.SystemTests.Raw.Tests
{
    [TestClass]
    public abstract class TheInternetTestBase : SeleniumTestBase
    {
        protected override string BaseUrl => DriverSession.EnvironmentSettings.BaseUrl;

        [TestInitialize]
        public void SetupTheInternet()
        {
        }

        [TestCleanup]
        public void TeardownTheInternet()
        {
        }
    }
}
