#include "linuxcppapp/text_utils.h"

#include <algorithm>
#include <cctype>
#include <sstream>
#include <string>

namespace linuxcppapp {

std::string to_upper(const std::string& text) {
    std::string result = text;

    std::transform(
        result.begin(),
        result.end(),
        result.begin(),
        [](const unsigned char character) {
            return static_cast<char>(std::toupper(character));
        });

    return result;
}

std::string reverse_text(const std::string& text) {
    return std::string(text.rbegin(), text.rend());
}

std::size_t count_words(const std::string& text) {
    std::istringstream input(text);
    std::string word;
    std::size_t count = 0;

    while (input >> word) {
        ++count;
    }

    return count;
}

bool is_palindrome(const std::string& text) {
    std::string normalized;

    for (const unsigned char character : text) {
        if (std::isalnum(character)) {
            normalized.push_back(
                static_cast<char>(std::tolower(character)));
        }
    }

    return std::equal(
        normalized.begin(),
        normalized.begin() +
            static_cast<std::string::difference_type>(normalized.size() / 2),
        normalized.rbegin());
}

}  // namespace linuxcppapp