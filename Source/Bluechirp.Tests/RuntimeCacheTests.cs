﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Model;
using Bluechirp.Library.Services;
using Bluechirp.Tests.Generators;

namespace Bluechirp.Tests
{
    [TestClass]
    public class RuntimeCacheTests
    {
        [TestMethod]
        public void CacheTest()
        {
            const string generatedContent = "hii";
            TimelineCache cacheToStore = TimelineData.GenerateTimelineCache(TimelineType.Home);
            RuntimeCacheService.StoreCache(cacheToStore, cacheToStore.CurrentTimelineSettings.CurrentTimelineType);
            var retreivedTimelineResult = RuntimeCacheService.RetreiveCache(TimelineType.Home);

            Assert.IsTrue(retreivedTimelineResult.isCacheAvailable);
            Assert.IsTrue(retreivedTimelineResult.cache.Toots[0].Content == generatedContent); 
        }

        [TestMethod]
        public void CacheClearTest()
        {
            TimelineCache cacheToStore = TimelineData.GenerateTimelineCache(TimelineType.Home);
            RuntimeCacheService.StoreCache(cacheToStore, cacheToStore.CurrentTimelineSettings.CurrentTimelineType);

            RuntimeCacheService.ClearCache(TimelineType.Home);

            var retreivedTimelineResult = RuntimeCacheService.RetreiveCache(TimelineType.Home);
            Assert.IsFalse(retreivedTimelineResult.isCacheAvailable);
        }
    }
}