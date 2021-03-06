﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp;
using NWebsec.AspNetCore.Mvc.HttpHeaders.Csp.Internals;

namespace NWebsec.AspNetCore.Mvc.Tests.HttpHeaders.Csp.Internals
{
    [TestFixture]
    public class CspDirectiveAttributeBaseTests
    {

        [Test]
        public void ValidateParams_EnabledAndNoDirectives_ThrowsException()
        {
            var cspSandboxAttributeBaseMock = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            Assert.Throws<ArgumentException>(() => cspSandboxAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_DisabledAndNoDirectives_NoException()
        {
            var cspSandboxAttributeBaseMock = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            cspSandboxAttributeBaseMock.Enabled = false;

            Assert.DoesNotThrow(() => cspSandboxAttributeBaseMock.ValidateParams());
        }

        [Test]
        public void ValidateParams_EnabledAndDirectives_NoException()
        {
            foreach (var cspSandboxAttributeBase in ConfiguredAttributes())
            {
                Assert.DoesNotThrow(() => cspSandboxAttributeBase.ValidateParams());
            }
        }

        [Test]
        public void ValidateParams_EnabledAndMalconfiguredDirectives_NoException()
        {
            foreach (var cspSandboxAttributeBase in MalconfiguredAttributes())
            {
                Assert.Throws<ArgumentException>(() => cspSandboxAttributeBase.ValidateParams());
            }
        }

        private IEnumerable<CspDirectiveAttributeBase> ConfiguredAttributes()
        {
            var attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            attribute.None = true;
            yield return attribute;

            attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            attribute.Self = true;
            yield return attribute;

            var styleattribute = new CspStyleSrcAttribute { UnsafeInline = true };
            yield return styleattribute;

            var scriptAttribute = new CspScriptSrcAttribute { UnsafeEval = true };
            yield return scriptAttribute;
        }

        private IEnumerable<CspDirectiveAttributeBase> MalconfiguredAttributes()
        {
            var attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            attribute.Self = true;
            attribute.None = true;
            yield return attribute;

            attribute = new Mock<CspDirectiveAttributeBase>(MockBehavior.Strict).Object;
            attribute.None = true;
            attribute.CustomSources = "www.nwebsec.com";
            yield return attribute;

            var styleattribute = new CspStyleSrcAttribute { None = true, UnsafeInline = true };
            yield return styleattribute;

            var scriptAttribute = new CspScriptSrcAttribute { None = true, UnsafeEval = true };
            yield return scriptAttribute;
        }
    }
}