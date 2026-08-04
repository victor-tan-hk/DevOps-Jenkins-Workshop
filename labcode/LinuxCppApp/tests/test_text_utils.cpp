#include "linuxcppapp/text_utils.h"

#include <gtest/gtest.h>

TEST(TextUtilsTest, ConvertsTextToUppercase) {
    EXPECT_EQ(
        linuxcppapp::to_upper("Hello Jenkins"),
        "HELLO JENKINS");
}

TEST(TextUtilsTest, HandlesEmptyTextWhenConvertingToUppercase) {
    EXPECT_EQ(linuxcppapp::to_upper(""), "");
}

TEST(TextUtilsTest, ReversesText) {
    EXPECT_EQ(
        linuxcppapp::reverse_text("Jenkins"),
        "snikneJ");
}

TEST(TextUtilsTest, CountsWords) {
    EXPECT_EQ(
        linuxcppapp::count_words("Jenkins builds C++ applications"),
        4U);
}

TEST(TextUtilsTest, IgnoresRepeatedSpacesWhenCountingWords) {
    EXPECT_EQ(
        linuxcppapp::count_words("one    two     three"),
        3U);
}

TEST(TextUtilsTest, ReturnsZeroForEmptyWordInput) {
    EXPECT_EQ(linuxcppapp::count_words(""), 0U);
}

TEST(TextUtilsTest, IdentifiesSimplePalindrome) {
    EXPECT_TRUE(linuxcppapp::is_palindrome("level"));
}

TEST(TextUtilsTest, IgnoresCaseAndSpacesInPalindrome) {
    EXPECT_TRUE(
        linuxcppapp::is_palindrome("Never odd or even"));
}

TEST(TextUtilsTest, RejectsNonPalindrome) {
    EXPECT_FALSE(
        linuxcppapp::is_palindrome("Continuous integration"));
}