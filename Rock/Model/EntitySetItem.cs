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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Entity Set Item POCO Entity.
    /// </summary>
    [Table( "EntitySetItem" )]
    [DataContract]
    public partial class EntitySetItem : Model<EntitySetItem>, IOrdered
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the entity set identifier.
        /// </summary>
        /// <value>
        /// The entity set identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int EntitySetId { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int EntityId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the metric.
        /// </summary>
        /// <value>
        /// The metric.
        /// </value>
        public virtual EntitySet EntitySet { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        public override Security.ISecured ParentAuthority
        {
            get { return this.EntitySet; }
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// EntitySetItem Configuration class.
    /// </summary>
    public partial class EntitySetItemConfiguration : EntityTypeConfiguration<EntitySetItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySetItemConfiguration"/> class.
        /// </summary>
        public EntitySetItemConfiguration()
        {
            this.HasRequired( p => p.EntitySet ).WithMany( p => p.Items ).HasForeignKey( p => p.EntitySetId ).WillCascadeOnDelete( true );
        }
    }

    #endregion
}