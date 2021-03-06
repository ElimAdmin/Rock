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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Rock.Data;
using Rock.Reporting;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// Represents a Report (based off of a <see cref="Rock.Model.DataView"/> in Rock.
    /// </summary>
    [Table( "Report" )]
    [DataContract]
    public partial class Report : Model<Report>, ICategorized
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets a flag indicating if this Report is part of the Rock core system/framework. This property is required.
        /// </summary>
        /// <value>
        ///  A <see cref="System.Boolean"/> that is <c>true</c> if the Report is part of the Rock core system/framework; otherwise <c>false</c>.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Report. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> that represents the Name of the Report.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Report's Description.
        /// </summary>
        /// <value>
        /// A <see cref="System.String"/> representing the Report's Description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the CategoryId of the <see cref="Rock.Model.Category"/> that the Report belongs to.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the CateogryId of the <see cref="Rock.Model.Category"/> that the report belongs to. If the Report does not belong to
        /// a <see cref="Rock.Model.Category"/> this value will be null.
        /// </value>
        [DataMember]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that is being reported on.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the EntityTypeId of the <see cref="Rock.Model.EntityType"/> that is being reported on.
        /// </value>
        [DataMember]
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or the DataViewId of the root <see cref="Rock.Model.DataView"/> that this Report is based on.
        /// </summary>
        /// <value>
        /// A <see cref="System.Int32"/> representing the DataViewId of the root <see cref="Rock.Model.DataView"/> that this Report is based on.
        /// </value>
        [DataMember]
        public int? DataViewId { get; set; }

        /// <summary>
        /// Gets or sets the number of records to fetch in the report.  Null means all records.
        /// </summary>
        /// <value>
        /// The fetch top.
        /// </value>
        [DataMember]
        public int? FetchTop { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.Category"/> that this Report belongs to.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Category"/> that this Report belongs to. If the Report does not belong to a <see cref="Rock.Model.Category"/> this value will be null.
        /// </value>
        [DataMember]
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rock.Model.EntityType"/> that is being reported on. 
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.EntityType"/> that is being reported on.
        /// </value>
        [DataMember]
        public virtual EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the base/root <see cref="Rock.Model.DataView"/> that this Report is based on.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.DataView"/> that this Report is based on.
        /// </value>
        [DataMember]
        public virtual DataView DataView { get; set; }

        /// <summary>
        /// Gets or sets the report fields.
        /// </summary>
        /// <value>
        /// The report fields.
        /// </value>
        [DataMember]
        public virtual ICollection<ReportField> ReportFields
        {
            get
            {
                return _reportFields ?? ( _reportFields = new Collection<ReportField>() );
            }

            set
            {
                _reportFields = value;
            }
        }
        private ICollection<ReportField> _reportFields;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityFields">The entity fields.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="selectComponents">The select components.</param>
        /// <param name="sortProperty">The sort property.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public List<object> GetDataSource( RockContext context, Type entityType, Dictionary<int,EntityField> entityFields, Dictionary<int,AttributeCache> attributes, Dictionary<int,ReportField> selectComponents, Rock.Web.UI.Controls.SortProperty sortProperty, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            if ( entityType != null )
            {
                Type[] modelType = { entityType };
                Type genericServiceType = typeof( Rock.Data.Service<> );
                Type modelServiceType = genericServiceType.MakeGenericType( modelType );

                IService serviceInstance = Activator.CreateInstance( modelServiceType, new object[] { context } ) as IService;

                if ( serviceInstance != null )
                {
                    ParameterExpression paramExpression = serviceInstance.ParameterExpression;
                    MemberExpression idExpression = Expression.Property( paramExpression, "Id" );

                    // Get AttributeValue queryable and parameter
                    var attributeValues = context.Set<AttributeValue>();
                    ParameterExpression attributeValueParameter = Expression.Parameter( typeof( AttributeValue ), "v" );

                    // Create the dynamic type
                    var dynamicFields = new Dictionary<string, Type>();
                    dynamicFields.Add( "Id", typeof( int ) );
                    foreach (var f in entityFields)
                    {
                        dynamicFields.Add( string.Format( "Entity_{0}_{1}", f.Value.Name, f.Key ), f.Value.PropertyType );
                    }

                    foreach (var a in attributes)
                    {
                        dynamicFields.Add( string.Format( "Attribute_{0}_{1}", a.Value.Id, a.Key ), typeof( string ) );
                    }

                    foreach ( var reportField in selectComponents )
                    {
                        DataSelectComponent selectComponent = DataSelectContainer.GetComponent( reportField.Value.DataSelectComponentEntityType.Name );
                        if ( selectComponent != null )
                        {
                            dynamicFields.Add( string.Format( "Data_{0}_{1}", selectComponent.ColumnPropertyName, reportField.Key ), selectComponent.ColumnFieldType );
                        }
                    }
                    
                    if (dynamicFields.Count == 0)
                    {
                        errorMessages.Add( "At least one field must be defined" );
                        return null;
                    }

                    Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType( dynamicFields );
                    ConstructorInfo methodFromHandle = dynamicType.GetConstructor( Type.EmptyTypes );

                    // Bind the dynamic fields to their expressions
                    var bindings = new List<MemberBinding>();
                    bindings.Add( Expression.Bind( dynamicType.GetField( "id" ), idExpression ) );

                    foreach (var f in entityFields)
                    {
                        bindings.Add( Expression.Bind( dynamicType.GetField( string.Format( "entity_{0}_{1}", f.Value.Name, f.Key ) ), Expression.Property( paramExpression, f.Value.Name ) ) );
                    }

                    foreach (var a in attributes)
                    {
                        bindings.Add( Expression.Bind( dynamicType.GetField( string.Format( "attribute_{0}_{1}", a.Value.Id, a.Key ) ), GetAttributeValueExpression( attributeValues, attributeValueParameter, idExpression, a.Value.Id ) ) );
                    }
                    foreach ( var reportField in selectComponents )
                    {
                        DataSelectComponent selectComponent = DataSelectContainer.GetComponent( reportField.Value.DataSelectComponentEntityType.Name );
                        if ( selectComponent != null )
                        {
                            bindings.Add( Expression.Bind( dynamicType.GetField( string.Format( "data_{0}_{1}", selectComponent.ColumnPropertyName, reportField.Key ) ), selectComponent.GetExpression( context, idExpression, reportField.Value.Selection ?? string.Empty ) ) );
                        }
                    }

                    ConstructorInfo constructorInfo = dynamicType.GetConstructor( Type.EmptyTypes );
                    NewExpression newExpression = Expression.New( constructorInfo );
                    MemberInitExpression memberInitExpression = Expression.MemberInit( newExpression, bindings );
                    Expression selector = Expression.Lambda( memberInitExpression, paramExpression );
                    
                    // NOTE: having a NULL Dataview is OK.
                    Expression whereExpression = null;
                    if ( this.DataView != null )
                    {
                        whereExpression = this.DataView.GetExpression( serviceInstance, paramExpression, out errorMessages );
                    }

                    MethodInfo getMethod = serviceInstance.GetType().GetMethod( "Get", new Type[] { typeof( ParameterExpression ), typeof( Expression ), typeof( Rock.Web.UI.Controls.SortProperty ), typeof( int? ) } );
                    if ( getMethod != null )
                    {
                        var getResult = getMethod.Invoke( serviceInstance, new object[] { paramExpression, whereExpression, sortProperty, this.FetchTop } );
                        var qry = getResult as IQueryable<IEntity>;

                        var selectExpression = Expression.Call( typeof( Queryable ), "Select", new Type[] { qry.ElementType, dynamicType }, Expression.Constant( qry ), selector );
                        var query = qry.Provider.CreateQuery( selectExpression ).AsNoTracking();

                        // enumerate thru the query results and put into a list
                        var reportResult = new List<object>();
                        var enumerator = query.GetEnumerator();
                        while ( enumerator.MoveNext() )
                        {
                            reportResult.Add( enumerator.Current );
                        }

                        return reportResult;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the attribute value expression.
        /// </summary>
        /// <param name="attributeValues">The attribute values.</param>
        /// <param name="attributeValueParameter">The attribute value parameter.</param>
        /// <param name="parentIdProperty">The parent identifier property.</param>
        /// <param name="attributeId">The attribute identifier.</param>
        /// <returns></returns>
        private Expression GetAttributeValueExpression( IQueryable<AttributeValue> attributeValues, ParameterExpression attributeValueParameter, Expression parentIdProperty, int attributeId )
        {
            MemberExpression attributeIdProperty = Expression.Property( attributeValueParameter, "AttributeId" );
            MemberExpression entityIdProperty = Expression.Property( attributeValueParameter, "EntityId" );
            Expression attributeIdConstant = Expression.Constant( attributeId );

            Expression attributeIdCompare = Expression.Equal( attributeIdProperty, attributeIdConstant );
            Expression entityIdCompre = Expression.Equal( entityIdProperty, Expression.Convert( parentIdProperty, typeof( int? ) ) );
            Expression andExpression = Expression.And( attributeIdCompare, entityIdCompre );

            var match = new Expression[] {
                Expression.Constant(attributeValues),
                Expression.Lambda<Func<AttributeValue, bool>>( andExpression, new ParameterExpression[] { attributeValueParameter })
            };
            Expression whereExpression = Expression.Call( typeof( Queryable ), "Where", new Type[] { typeof( AttributeValue ) }, match );

            MemberExpression valueProperty = Expression.Property( attributeValueParameter, "Value" );
            Expression valueLambda = Expression.Lambda( valueProperty, new ParameterExpression[] { attributeValueParameter } );

            Expression selectValue = Expression.Call( typeof( Queryable ), "Select", new Type[] { typeof( AttributeValue ), typeof( string ) }, whereExpression, valueLambda );

            Expression firstOrDefault = Expression.Call( typeof( Queryable ), "FirstOrDefault", new Type[] { typeof( string ) }, selectValue );

            return firstOrDefault;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// Campus Configuration class.
    /// </summary>
    public partial class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportConfiguration"/> class.
        /// </summary>
        public ReportConfiguration()
        {
            this.HasOptional( r => r.Category ).WithMany().HasForeignKey( r => r.CategoryId ).WillCascadeOnDelete( false );
            this.HasOptional( r => r.DataView ).WithMany().HasForeignKey( r => r.DataViewId ).WillCascadeOnDelete( false );
        }
    }

    #endregion
}
