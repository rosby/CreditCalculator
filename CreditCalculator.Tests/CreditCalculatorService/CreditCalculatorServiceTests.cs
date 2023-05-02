using System;
using System.Linq;
using CreditCalculator.Controllers.Models;
using CreditCalculator.Domain;
using CreditCalculator.Exceptions;
using DecimalMath;
using Xunit;

namespace CreditCalculator.Tests;

    public class CreditCalculatorServiceTests
    {
        [Fact]
        public void CalculateCredit_ThrowsAppException_WhenIssueDateIsInThePast()
        {
            // Arrange
            var service = new CreditCalculatorService();
            var creditBody = 100_000;
            var issueDate = DateTime.Now.AddDays(-1);
            var closingDate = DateTime.Now.AddMonths(12);
            var interestRate = 10;
            var chartType = СhartType.Annuity;

            // Act and Assert
            Assert.Throws<AppException>(() =>
                service.CalculateCredit(creditBody, issueDate, closingDate, interestRate, chartType));
        }

        [Fact]
        public void CalculateCredit_ThrowsAppException_WhenIssueDateEqualsClosingDate()
        {
            // Arrange
            var service = new CreditCalculatorService();
            var creditBody = 100_000;
            var issueDate = DateTime.Now;
            var closingDate = DateTime.Now;
            var interestRate = 10;
            var chartType = СhartType.Annuity;

            // Act and Assert
            Assert.Throws<AppException>(() =>
                service.CalculateCredit(creditBody, issueDate, closingDate, interestRate, chartType));
        }

        [Fact]
        public void CalculateCredit_ThrowsAppException_WhenInterestRateIsNegativeOrZero()
        {
            // Arrange
            var service = new CreditCalculatorService();
            var creditBody = 100_000;
            var issueDate = DateTime.Now;
            var closingDate = DateTime.Now.AddMonths(12);
            var interestRate = -10;
            var chartType = СhartType.Annuity;

            // Act and Assert
            Assert.Throws<AppException>(() =>
                service.CalculateCredit(creditBody, issueDate, closingDate, interestRate, chartType));
        }

        [Fact]
        public void CalculateCredit_ThrowsAppException_WhenCreditBodyIsNegativeOrZero()
        {
            // Arrange
            var service = new CreditCalculatorService();
            var creditBody = 0;
            var issueDate = DateTime.Now;
            var closingDate = DateTime.Now.AddMonths(12);
            var interestRate = 10;
            var chartType = СhartType.Annuity;

            // Act and Assert
            Assert.Throws<AppException>(() =>
                service.CalculateCredit(creditBody, issueDate, closingDate, interestRate, chartType));
        }
    }
 
