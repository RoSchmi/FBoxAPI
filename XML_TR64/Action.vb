﻿Imports System.Xml.Serialization
<DebuggerStepThrough>
<Serializable()>
Public Class Action
    <XmlElement("name")> Public Name As String
    <XmlArray("argumentList")> <XmlArrayItem("argument")> Public Property ArgumentList As List(Of Argument)

    Friend Function GetInputArguments() As Hashtable
        Dim InputHashTable As New Hashtable

        ArgumentList?.FindAll(Function(Argument) Argument.Direction = ArgumentDirection.IN).ForEach(Sub(Argument) InputHashTable.Add(Argument.Name, String.Empty))

        Return InputHashTable
    End Function

End Class



