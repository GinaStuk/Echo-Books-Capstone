﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using Capstone.Web.Models;
using System.Collections.Generic;

namespace Capstone.Web.Tests.DALTests
{
    [TestClass]
    public class BooksSqlDALTest
    {
        private System.Transactions.TransactionScope tran;
        private string connectionString = ConfigurationManager.ConnectionStrings["EchoBooks"].ConnectionString;
        int bookId = -1;
        public object books { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cm = new SqlCommand("Delete from readingList; Delete from reviews; Delete from books", conn);
                cm.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand("Insert into books values('The Test Title', 'Author, Test', 'Test Character', 'Test Setting', 'Test Genre', '11/30/2016', 'Test Description', 'Test imageLink');Select cast(Scope_Identity() as int)", conn);
                bookId = (int)cmd2.ExecuteScalar();


            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void TestGetBooksMethod_TestByTitle()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            List<BookModel> books = booksSqlDal.GetBooks("Test", "Title" );

            Assert.IsNotNull(books);
            Assert.AreEqual(1, books.Count);
        }
        [TestMethod]
        public void TestGetBooksMethod_TestByAuthor()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            List<BookModel> books = booksSqlDal.GetBooks("Test", "Author");

            Assert.IsNotNull(books);
            Assert.AreEqual(1, books.Count);
        }
        [TestMethod]
        public void TestGetBooksMethod_TestBySetting()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            List<BookModel> books = booksSqlDal.GetBooks("Test", "Setting");

            Assert.IsNotNull(books);
            Assert.AreEqual(1, books.Count);
        }
        [TestMethod]
        public void TestGetBooksMethod_TestByCharacter()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            List<BookModel> books = booksSqlDal.GetBooks("Test", "Character");

            Assert.IsNotNull(books);
            Assert.AreEqual(1, books.Count);
        }

        [TestMethod]
        public void TestGetBooksByIDMethod()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            BookModel bookByID = booksSqlDal.GetBooksById(bookId);

            Assert.IsNotNull(bookByID);
            Assert.AreEqual("The Test Title", bookByID.Title.ToString());
        }

        [TestMethod]
        public void TestAddNewBook()
        {
            BooksSqlDAL dal = new BooksSqlDAL();
            BookModel model = new BookModel();
            model.Author = "Stuk, Gina";
            model.Title = "";
            model.MainCharacter = "Detective Brandon";
            model.Setting = "Cleveland, OH";
            model.Genre = "Mystery";
            model.Description = "Lorem";
            model.ImageLink = "https://images-na.ssl-images";
            

            bool isAdded = dal.AddNewBook(model);

            
            Assert.IsTrue(isAdded);
        }

        [TestMethod]
        public void TestGetNewBookList()
        {
            BooksSqlDAL booksSqlDal = new BooksSqlDAL();
            List<BookModel> booksList = booksSqlDal.GetNewBookList();

            Assert.IsNotNull(booksList);
            Assert.AreEqual(1, booksList.Count);

        }

        [TestMethod]
        public void TestGetNewAuthorList()
        {
            BooksSqlDAL booksSqldal = new BooksSqlDAL();
            List<String> authorList = booksSqldal.GetNewAuthorList();

            Assert.IsNotNull(authorList);
            Assert.AreEqual(1, authorList.Count); 

        }
    }
}