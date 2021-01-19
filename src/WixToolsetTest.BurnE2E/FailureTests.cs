// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolsetTest.BurnE2E
{
    using System;
    using System.IO;
    using Xunit;
    using Xunit.Abstractions;

    public class FailureTests : BurnE2ETests
    {
        public FailureTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void CanCancelMsiPackageVeryEarly()
        {
            var packageA = this.CreatePackageInstaller("PackageA");
            var packageB = this.CreatePackageInstaller("PackageB");
            var bundleA = this.CreateBundleInstaller("BundleA");
            var testBAController = this.CreateTestBAController();

            var packageASourceCodeInstalled = packageA.GetInstalledFilePath("Package.wxs");
            var packageBSourceCodeInstalled = packageB.GetInstalledFilePath("Package.wxs");

            // Source file should *not* be installed
            Assert.False(File.Exists(packageASourceCodeInstalled), $"Package A payload should not be there on test start: {packageASourceCodeInstalled}");
            Assert.False(File.Exists(packageBSourceCodeInstalled), $"Package B payload should not be there on test start: {packageBSourceCodeInstalled}");

            // Cancel package B right away.
            testBAController.SetPackageCancelExecuteAtProgress("PackageB", 1);

            bundleA.Install((int)MSIExec.MSIExecReturnCode.ERROR_INSTALL_USEREXIT);
            bundleA.VerifyUnregisteredAndRemovedFromPackageCache();

            packageA.VerifyInstalled(false);
            packageB.VerifyInstalled(false);
        }

        [Fact]
        public void CanCancelMsiPackageVeryLate()
        {
            var packageA = this.CreatePackageInstaller("PackageA");
            var packageB = this.CreatePackageInstaller("PackageB");
            var bundleA = this.CreateBundleInstaller("BundleA");
            var testBAController = this.CreateTestBAController();

            var packageASourceCodeInstalled = packageA.GetInstalledFilePath("Package.wxs");
            var packageBSourceCodeInstalled = packageB.GetInstalledFilePath("Package.wxs");

            // Source file should *not* be installed
            Assert.False(File.Exists(packageASourceCodeInstalled), $"Package A payload should not be there on test start: {packageASourceCodeInstalled}");
            Assert.False(File.Exists(packageBSourceCodeInstalled), $"Package B payload should not be there on test start: {packageBSourceCodeInstalled}");

            // Cancel package B at the last moment possible.
            testBAController.SetPackageCancelExecuteAtProgress("PackageB", 100);

            bundleA.Install((int)MSIExec.MSIExecReturnCode.ERROR_INSTALL_USEREXIT);
            bundleA.VerifyUnregisteredAndRemovedFromPackageCache();

            packageA.VerifyInstalled(false);
            packageB.VerifyInstalled(false);
        }

        [Fact]
        public void CanCancelExecuteWhileCaching()
        {
        }

        [Fact]
        public void CanSkipMissingNonVitalPackage()
        {
        }
    }
}
