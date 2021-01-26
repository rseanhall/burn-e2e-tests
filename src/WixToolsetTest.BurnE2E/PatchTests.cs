// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

namespace WixToolsetTest.BurnE2E
{
    using Xunit;
    using Xunit.Abstractions;

    public class PatchTests : BurnE2ETests
    {
        public PatchTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void CanInstallBundleWithSlipstreamThenRemoveIt()
        {
            var originalVersion = "1.0.0.0";
            var patchedVersion = "1.0.1.0";
            var testRegistryValue = "PackageA";

            var packageAv1 = this.CreatePackageInstaller("PackageAv1");
            var bundleA = this.CreateBundleInstaller("BundleA");
            var bundlePatchA = this.CreateBundleInstaller("BundlePatchA");

            bundleA.Install();
            bundleA.VerifyRegisteredAndInPackageCache();

            packageAv1.VerifyInstalled(true);
            packageAv1.VerifyTestRegistryValue(testRegistryValue, originalVersion);

            bundlePatchA.Install();
            bundlePatchA.VerifyRegisteredAndInPackageCache();

            packageAv1.VerifyTestRegistryValue(testRegistryValue, patchedVersion);

            bundlePatchA.Uninstall();
            bundlePatchA.VerifyUnregisteredAndRemovedFromPackageCache();

            packageAv1.VerifyTestRegistryValue(testRegistryValue, originalVersion);

            bundleA.Uninstall();
            bundleA.VerifyUnregisteredAndRemovedFromPackageCache();

            packageAv1.VerifyInstalled(false);
            packageAv1.VerifyTestRegistryRootDeleted();
        }
    }
}
