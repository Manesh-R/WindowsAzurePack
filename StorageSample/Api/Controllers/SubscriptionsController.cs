﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

using Terawe.WindowsAzurePack.StarterKit.StorageSample.ApiClient.DataContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace Terawe.WindowsAzurePack.StarterKit.StorageSample.Api.Controllers
{
    /// <summary>
    /// Subscriptions Controller class
    /// </summary>
    public class SubscriptionsController : ApiController
    {
        public static List<Subscription> subscriptions = new List<Subscription>();

        /// <summary>
        /// Gets a subscription collection.
        /// </summary>
        [HttpGet]
        public List<Subscription> GetSubscriptionList()
        {
            return subscriptions;
        }

        /// <summary>
        /// Updates the subscription.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        [HttpPut]
        public void UpdateSubscription(Subscription subscription)
        {
            this.ValidateSubscriptionId(subscription);

            var sub = (from s in subscriptions where s.SubscriptionId == subscription.SubscriptionId select s).FirstOrDefault();

            if (sub != null)
            {
                sub.AdminId = subscription.AdminId;
                sub.SubscriptionName = subscription.SubscriptionName;
                sub.CoAdminIds = subscription.CoAdminIds;
            }
        }

        /// <summary>
        /// Creates the subscription.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        [HttpPost]
        public void AddSubscription(Subscription subscription)
        {
            this.ValidateSubscriptionId(subscription);

            // Add subscription to in memory collection of subscriptions
            // Actual resource provider can save this in their backend store
            subscriptions.Add(new Subscription
            {
                SubscriptionId = subscription.SubscriptionId,
                SubscriptionName = subscription.SubscriptionName,
                AdminId = subscription.AdminId,
                CoAdminIds = subscription.CoAdminIds
            });
        }

        /// <summary>
        /// Deletes the subscription.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        [HttpDelete]
        public void DeleteSubscription(Subscription subscription)
        {
            this.ValidateSubscriptionId(subscription);

            var sub = subscriptions.FirstOrDefault(x => x.SubscriptionId == subscription.SubscriptionId);

            if (sub != null)
            {
                subscriptions.Remove(sub);
            }
        }

        /// <summary>
        /// Validates the subscription id.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns>Subscription Guid</returns>
        private void ValidateSubscriptionId(Subscription subscription)
        {
            if (subscription == null || string.IsNullOrWhiteSpace(subscription.SubscriptionId))
            {
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, ErrorMessages.EmptySubscription);
            }

            Guid id;
            bool parseGuid = Guid.TryParse(subscription.SubscriptionId, out id);

            if (!parseGuid)
            {
                string message = string.Format(CultureInfo.CurrentCulture, ErrorMessages.InvalidSubscriptionFormat, subscription.SubscriptionId);
                throw Utility.ThrowResponseException(this.Request, System.Net.HttpStatusCode.BadRequest, message);
            }
        }
    }
}
