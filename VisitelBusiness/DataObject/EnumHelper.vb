#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Extended List creation along with triplet value
' Author: Anjan Kumar Paul
' Start Date: 07 March 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                07 March 2015    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection
Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization

Namespace VisitelBusiness.DataObject
    Public Module Extensions

#Region "public properties and methods"

#Region "properties"

#End Region

#Region "methods"


#Region "GetDescription"

        ''' <summary>
        ''' Gets the <see cref="DescriptionAttribute"/> of an <see cref="Enum"/> type value.
        ''' </summary>
        ''' <param name="value">The <see cref="Enum"/> type value.</param>
        ''' <returns>A string containing the text of the <see cref="DescriptionAttribute"/>.</returns>
        ''' <remarks></remarks>
        <System.Runtime.CompilerServices.Extension()> _
        Public Function GetDescription(value As [Enum]) As String
            If value Is Nothing Then
                Throw New ArgumentNullException("value")
            End If

            Dim description As String = value.ToString()
            Dim fieldInfo As FieldInfo = value.[GetType]().GetField(description)
            Dim attributes As EnumDescriptionAttribute() = DirectCast(fieldInfo.GetCustomAttributes(GetType(EnumDescriptionAttribute), False), EnumDescriptionAttribute())

            If attributes IsNot Nothing AndAlso attributes.Length > 0 Then
                description = attributes(0).Description
            End If
            Return description
        End Function
#End Region

#Region "ToExtendedList"


        ''' <summary>
        ''' Converts the <see cref="Enum"/> type to an <see cref="IList"/> compatible object.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="type">The <see cref="Enum"/> type.</param>
        ''' <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        ''' <remarks></remarks>
        <SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification:="This is a more advanced use of the ToList function; providing a type parameter has no semantic meaning for this function and would actually make the calling syntax more complicated.")> _
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ToExtendedList(Of T)(type As Type) As IList
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            If Not type.IsEnum Then
                Throw New ArgumentException("Argument Exception Must Be Enum", "type")
            End If

            Dim list As New ArrayList()
            Dim enumValues As Array = [Enum].GetValues(type)

            Dim triplet As KeyValueTriplet = Nothing

            For Each value As [Enum] In enumValues
                triplet.m_key = value
                triplet.NumericKey = DirectCast(Convert.ChangeType(value, GetType(T), CultureInfo.InvariantCulture), T)
                triplet.Value = GetDescription(value)
                list.Add(triplet)
            Next

            Return list
        End Function
#End Region

#Region "ToList"
        ''' <summary>
        ''' Converts the Enum type to an IList compatible object.
        ''' </summary>
        ''' <param name="type">The Enum type.</param>
        ''' <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        ''' <remarks></remarks>
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ToList(type As Type) As IList
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            If Not type.IsEnum Then
                Throw New ArgumentException("Argument Exception Must Be Enum", "type")
            End If

            Dim list As New ArrayList()
            Dim enumValues As Array = [Enum].GetValues(type)

            For Each value As [Enum] In enumValues
                list.Add(New KeyValuePair(Of [Enum], String)(value, GetDescription(value)))
            Next

            Return list
        End Function

#Region "ToList(this Type type)"

        ''' <summary>
        ''' Converts the <see cref="Enum"/> type to an <see cref="IList"/> compatible object.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="type"></param>
        ''' <returns>An <see cref="IList"/> containing the enumerated type value and description.</returns>
        ''' <remarks></remarks>
        <SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification:="This is a more advanced use of the ToList function; providing a type parameter has no semantic meaning for this function and would actually make the calling syntax more complicated.")> _
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ToList(Of T)(type As Type) As IList
            If type Is Nothing Then
                Throw New ArgumentNullException("type")
            End If

            If Not type.IsEnum Then
                Throw New ArgumentException("Argument Exception Must Be Enum", "type")
            End If

            Dim list As New ArrayList()
            Dim enumValues As Array = [Enum].GetValues(type)

            For Each value As [Enum] In enumValues
                list.Add(New KeyValuePair(Of T, String)(DirectCast(Convert.ChangeType(value, GetType(T), CultureInfo.InvariantCulture), T), GetDescription(value)))
            Next

            Return list
        End Function
#End Region

#End Region
#End Region
#End Region

    End Module

End Namespace