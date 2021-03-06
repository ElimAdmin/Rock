﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents a <see cref="Rock.Model.Campus" /> in Rock where a <see cref="Rock.Model.MarketingCampaign"/> is being promoted at and/or being promoted for.  A <see cref="Rock.Model.MarketingCampaign"/> can be promoted 
    /// at one or more <see cref="Rock.Model.Campus"/>.
    /// </summary>
    [Table( "MarketingCampaignCampus")]
    [DataContract]
    public partial class MarketingCampaignCampus : Model<MarketingCampaignCampus>
    {
        /// <summary>
        /// Gets or sets the MarketingCampaignId of the <see cref="Rock.Model.MarketingCampaign"/> that is being promoted at this <see cref="Rock.Model.Campus"/>.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the MarketingCampaignId of the <see cref="Rock.Model.MarketingCampaign"/> that is being promoted.
        /// </value>
        [DataMember]
        public int MarketingCampaignId { get; set; }

        /// <summary>
        /// Gets or sets the CampusId of the <see cref="Rock.Model.Campus" /> where the <see cref="Rock.Model.MarketingCampaign"/> is being promoted for.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the CampusID of the <see cref="Rock.Model.Campus"/> that the <see cref="Rock.Model.MarketingCampaign"/> is being promoted for/targeted to.
        /// </value>
        [DataMember]
        public int CampusId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.MarketingCampaign"/> that is being promoted.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.MarketingCampaign"/> that is being promoted.
        /// </value>
        [DataMember]
        public virtual MarketingCampaign MarketingCampaign { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Campus"/> where the <see cref="Rock.Model.MarketingCampaign"/> is being promoted at.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Campus"/> where the <see cref="Rock.Model.MarketingCampaign"/> is being promoted at.
        /// </value>
        [DataMember]
        public virtual Campus Campus { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class MarketingCampaignCampusConfiguration : EntityTypeConfiguration<MarketingCampaignCampus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketingCampaignCampusConfiguration" /> class.
        /// </summary>
        public MarketingCampaignCampusConfiguration()
        {
            this.HasRequired( p => p.MarketingCampaign ).WithMany(p => p.MarketingCampaignCampuses ).HasForeignKey( p => p.MarketingCampaignId ).WillCascadeOnDelete( true );
            this.HasRequired( p => p.Campus ).WithMany().HasForeignKey( p => p.CampusId ).WillCascadeOnDelete( true );
        }
    }
}