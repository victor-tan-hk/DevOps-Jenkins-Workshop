#include "linuxcppapp/calculator.h"

#include <gtest/gtest.h>

#include <stdexcept>

TEST(CalculatorTest, AddsPositiveNumbers) {
    EXPECT_EQ(linuxcppapp::add(2, 3), 5);
}

TEST(CalculatorTest, AddsNegativeNumbers) {
    EXPECT_EQ(linuxcppapp::add(-2, -3), -5);
}

TEST(CalculatorTest, SubtractsNumbers) {
    EXPECT_EQ(linuxcppapp::subtract(10, 4), 6);
}

TEST(CalculatorTest, MultipliesNumbers) {
    EXPECT_EQ(linuxcppapp::multiply(6, 7), 42);
}

TEST(CalculatorTest, CalculatesZeroFactorial) {
    EXPECT_EQ(linuxcppapp::factorial(0), 1);
}

TEST(CalculatorTest, CalculatesPositiveFactorial) {
    EXPECT_EQ(linuxcppapp::factorial(5), 120);
}

TEST(CalculatorTest, RejectsNegativeFactorial) {
    EXPECT_THROW(
        linuxcppapp::factorial(-1),
        std::invalid_argument);
}

TEST(CalculatorTest, IdentifiesEvenNumber) {
    EXPECT_TRUE(linuxcppapp::is_even(12));
}

TEST(CalculatorTest, IdentifiesOddNumber) {
    EXPECT_FALSE(linuxcppapp::is_even(11));
}

TEST(CalculatorTest, HandlesNegativeEvenNumber) {
    EXPECT_TRUE(linuxcppapp::is_even(-4));
}