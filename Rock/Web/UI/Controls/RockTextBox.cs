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
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// A <see cref="T:System.Web.UI.WebControls.TextBox"/> control with an associated label.
    /// </summary>
    [ToolboxData( "<{0}:RockTextBox runat=server></{0}:RockTextBox>" )]
    public class RockTextBox : TextBox, IRockControl
    {
        #region IRockControl implementation

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }
            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public virtual bool Required
        {
            get { return ViewState["Required"] as bool? ?? false; }
            set { ViewState["Required"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Required indicator when Required=true
        /// </summary>
        /// <value>
        /// <c>true</c> if [display required indicator]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayRequiredIndicator
        {
            get { return ViewState["DisplayRequiredIndicator"] as bool? ?? true; }
            set { ViewState["DisplayRequiredIndicator"] = value; }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }
            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                return !Required || RequiredFieldValidator == null || RequiredFieldValidator.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        /// <summary>
        /// Gets or sets the group of controls for which the <see cref="T:System.Web.UI.WebControls.TextBox" /> control causes validation when it posts back to the server.
        /// </summary>
        /// <returns>The group of controls for which the <see cref="T:System.Web.UI.WebControls.TextBox" /> control causes validation when it posts back to the server. The default value is an empty string ("").</returns>
        public override string ValidationGroup
        {
            get
            {
                return base.ValidationGroup;
            }
            set
            {
                base.ValidationGroup = value;
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ValidationGroup = value;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the prepend text.
        /// </summary>
        /// <value>
        /// The prepend text.
        /// </value>
        [
        Bindable( false ),
        Category( "Appearance" ),
        DefaultValue(""),
        Description("Text that appears prepended to the front of the text box.")
        ]
        public string PrependText
        {
            get { return ViewState["PrependText"] as string ?? string.Empty; }
            set { ViewState["PrependText"] = value; }
        }

        /// <summary>
        /// Gets or sets the append text.
        /// </summary>
        /// <value>
        /// The append text.
        /// </value>
        [
        Bindable( false ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "Text that appears appended to the end of the text box." )
        ]
        public string AppendText
        {
            get { return ViewState["AppendText"] as string ?? string.Empty; }
            set { ViewState["AppendText"] = value; }
        }

        /// <summary>
        /// Gets or sets the placeholder text to display inside textbox when it is empty
        /// </summary>
        /// <value>
        /// The placeholder text
        /// </value>
        public string Placeholder
        {
            get { return ViewState["Placeholder"] as string ?? string.Empty; }
            set { ViewState["Placeholder"] = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RockTextBox" /> class.
        /// </summary>
        public RockTextBox()
            : base()
        {
            RockControlHelper.Init( this );
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();
            RockControlHelper.CreateChildControls(this, Controls);
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            // logic to add input groups for preappend and append labels
            bool renderInputGroup = false;
            string cssClass = this.CssClass;

            if ( !string.IsNullOrWhiteSpace( PrependText ) || !string.IsNullOrWhiteSpace( AppendText ) )
            {
                renderInputGroup = true;
            }

            if ( renderInputGroup )
            {
                writer.AddAttribute( "class", "input-group " + cssClass );
                if (this.Style[HtmlTextWriterStyle.Display] == "none")
                {
                    // render the display:none in the inputgroup div instead of the control itself
                    writer.AddStyleAttribute( HtmlTextWriterStyle.Display, "none" );
                    this.Style[HtmlTextWriterStyle.Display] = string.Empty;
                }
                
                writer.RenderBeginTag( HtmlTextWriterTag.Div );

                this.CssClass = string.Empty;
            }

            if ( !string.IsNullOrWhiteSpace( PrependText ) )
            {
                writer.AddAttribute( "class", "input-group-addon" );
                writer.RenderBeginTag( HtmlTextWriterTag.Span );
                writer.Write( PrependText );
                writer.RenderEndTag();
            }

            ( (WebControl)this ).AddCssClass( "form-control" );
            if (!string.IsNullOrWhiteSpace(Placeholder))
            {
                this.Attributes["placeholder"] = Placeholder;
            }
            
            base.RenderControl( writer );

            if ( !string.IsNullOrWhiteSpace( AppendText ) )
            {
                writer.AddAttribute( "class", "input-group-addon" );
                writer.RenderBeginTag( HtmlTextWriterTag.Span );
                writer.Write( AppendText );
                writer.RenderEndTag();
            }

            if ( renderInputGroup )
            {
                writer.RenderEndTag();  // input-group
                this.CssClass = cssClass;
            }

            RenderDataValidator( writer );

        }

        /// <summary>
        /// Renders any data validator.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected virtual void RenderDataValidator( HtmlTextWriter writer )
        {
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public virtual void ShowErrorMessage( string errorMessage )
        {
            RequiredFieldValidator.ErrorMessage = errorMessage;
            RequiredFieldValidator.IsValid = false;
        }

        /// <summary>
        /// Gets or sets the text content of the <see cref="T:System.Web.UI.WebControls.TextBox" /> control.
        /// </summary>
        /// <returns>The text displayed in the <see cref="T:System.Web.UI.WebControls.TextBox" /> control. The default is an empty string ("").</returns>
        public override string Text
        {
            get
            {
                if ( base.Text == null )
                {
                    return null;
                }
                else
                {
                    return base.Text.Trim();
                }
            }
            set
            {
                base.Text = value;
            }
        }

    }
}