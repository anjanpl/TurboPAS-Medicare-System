#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: String Key, Numeric Key and Value Triplet Manage 
' Author: Anjan Kumar Paul
' Start Date: 07 March 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                07 March 2015    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Diagnostics.CodeAnalysis

Namespace VisitelBusiness.DataObject
#Region "struct KeyValueTriplet"

    ''' <summary>
    ''' Defines a key/numeric key/value triplet that can be set or retrieved.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable(), StructLayout(LayoutKind.Sequential)> _
    <SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")> _
    <SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")> _
    Public Structure KeyValueTriplet
#Region "class-wide fields"
        Public m_key As Object
        Public m_value As String
        Public m_numericKey As Integer
#End Region

#Region "public properties and methods"

#Region "properties"

#Region "Key"
        ''' <summary>
        ''' Gets and Sets the key in the key/numeric key/value triplet.
        ''' </summary>
        ''' <value>A <typeparamref name="TKey"/> that is the key of the <see cref="KeyValueTriplet{TKey, TNumericKey, TValue}"/>.</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key() As Object
            Get
                Return Me.m_key
            End Get
            Set(value As Object)
                Me.m_key = value
            End Set
        End Property
#End Region

#Region "NumericKey"
        ''' <summary>
        ''' Gets and Sets the numeric representation of the <see cref="Key"/> in the key/numeric key/value triplet.
        ''' </summary>
        ''' <value>A <typeparamref name="TNumericKey"/> that is the numeric key of the <see cref="KeyValueTriplet{TKey, TNumericKey, TValue}"/>.</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumericKey() As Object
            Get
                Return Me.m_numericKey
            End Get
            Set(value As Object)
                Me.m_numericKey = value
            End Set
        End Property
#End Region

#Region "Value"
        ''' <summary>
        ''' Gets and Sets the value in the key/numeric key/value triplet.
        ''' </summary>
        ''' <value>A <typeparamref name="TValue"/> that is the value of the <see cref="KeyValueTriplet{TKey, TNumericKey, TValue}"/>.</value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Value() As String
            Get
                Return Me.m_value
            End Get
            Set(value As String)
                Me.m_value = value
            End Set
        End Property
#End Region

#End Region

#Region "methods"

#Region "ToString"
        ''' <summary>
        ''' Returns a string representation of the <see cref="KeyValueTriplet{TKey, TNumericKey, TValue}"/>,
        ''' using the string representations of the key, numeric key, and value.
        ''' </summary>
        ''' <returns>A string representation of the <see cref="KeyValueTriplet{TKey, TNumericKey, TValue}"/>,
        ''' which includes the string representations of the key, numeric key, and value.</returns>
        Public Overrides Function ToString() As String
            Dim builder As New StringBuilder()
            builder.Append("["c)
            If Me.Key IsNot Nothing Then
                builder.Append(Me.Key.ToString())
            End If
            builder.Append(", ")
            If Me.NumericKey IsNot Nothing Then
                builder.Append(Me.NumericKey.ToString())
            End If
            builder.Append(", ")
            If Me.Value IsNot Nothing Then
                builder.Append(Me.Value.ToString())
            End If
            builder.Append("]"c)
            Return builder.ToString()
        End Function
#End Region

#End Region

#End Region
    End Structure

#End Region
End Namespace


