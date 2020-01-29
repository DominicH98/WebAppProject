Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports MaritimeApplication.MaritimeApplication

Namespace Controllers
    Public Class DataController
        Inherits System.Web.Mvc.Controller

        Private db As New MaritimeDatabase_1Entities1

        ' GET: Data
        Function Index() As ActionResult
            Return View(db.Data.ToList())
        End Function

        ' GET: Data/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim datum As Datum = db.Data.Find(id)
            If IsNothing(datum) Then
                Return HttpNotFound()
            End If
            Return View(datum)
        End Function

        ' GET: Data/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Data/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="Id,NumberData")> ByVal datum As Datum) As ActionResult
            If ModelState.IsValid Then
                db.Data.Add(datum)
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(datum)
        End Function

        ' GET: Data/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim datum As Datum = db.Data.Find(id)
            If IsNothing(datum) Then
                Return HttpNotFound()
            End If
            Return View(datum)
        End Function

        ' POST: Data/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="Id,NumberData")> ByVal datum As Datum) As ActionResult
            If ModelState.IsValid Then
                db.Entry(datum).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(datum)
        End Function

        ' GET: Data/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim datum As Datum = db.Data.Find(id)
            If IsNothing(datum) Then
                Return HttpNotFound()
            End If
            Return View(datum)
        End Function

        ' POST: Data/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim datum As Datum = db.Data.Find(id)
            db.Data.Remove(datum)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
