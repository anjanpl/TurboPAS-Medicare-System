#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Getting Augmented Attribute
' Author: Anjan Kumar Paul
' Start Date: 07 March 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                07 March 2015    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Namespace VisitelBusiness.DataObject
#Region "class ArgumentAttribute"
    ''' <summary>
    ''' Provides a description for an enumerated type.
    ''' </summary>
    <AttributeUsage(AttributeTargets.[Enum] Or AttributeTargets.Field, AllowMultiple:=False)> _
    Public NotInheritable Class EnumDescriptionAttribute
        Inherits Attribute
#Region "events and delegates"

#End Region

#Region "class-wide fields"

        Private m_description As String

#End Region

#Region "private and internal properties and methods"

#Region "properties"

#End Region

#Region "methods"

#End Region

#End Region

#Region "public properties and methods"

#Region "properties"

#Region "Description"
        ''' <summary>
        ''' Gets the description stored in this attribute.
        ''' </summary>
        ''' <value>The description stored in the attribute.</value>
        Public ReadOnly Property Description() As String
            Get
                Return Me.m_description
            End Get
        End Property
#End Region

#End Region

#Region "methods"

#Region "constructor"
        ''' <summary>
        ''' Initializes a new instance of the 
        ''' <see cref="EnumDescriptionAttribute"/> class.
        ''' </summary>
        ''' <param name="description">The description to store in this attribute.</param>
        Public Sub New(description As String)
            MyBase.New()
            Me.m_description = description
        End Sub
#End Region

#End Region

#End Region
    End Class
#End Region
End Namespace
